﻿using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sitio_Privado.Policies
{
    // This class is a temporary workaround for AAD B2C,
    // while our current libraries are unable to support B2C
    // out of the box.  For the original source code (with comments)
    // visit https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/master/src/Microsoft.IdentityModel.Protocol.Extensions/Configuration/ConfigurationManager.cs
    class PolicyConfigurationManager : IConfigurationManager<OpenIdConnectConfiguration>
    {
        public static readonly TimeSpan DefaultAutomaticRefreshInterval = new TimeSpan(5, 0, 0, 0);

        public static readonly TimeSpan DefaultRefreshInterval = new TimeSpan(0, 0, 0, 30);

        public static readonly TimeSpan MinimumAutomaticRefreshInterval = new TimeSpan(0, 0, 5, 0);

        public static readonly TimeSpan MinimumRefreshInterval = new TimeSpan(0, 0, 0, 1);

        private const string policyParameter = "p";

        private TimeSpan _automaticRefreshInterval = DefaultAutomaticRefreshInterval;
        private TimeSpan _refreshInterval = DefaultRefreshInterval;
        private Dictionary<string, DateTimeOffset> _syncAfter;
        private Dictionary<string, DateTimeOffset> _lastRefresh;

        private readonly SemaphoreSlim _refreshLock;
        private readonly string _metadataAddress;
        private readonly IDocumentRetriever _docRetriever;
        private readonly OpenIdConnectConfigurationRetriever _configRetriever;
        private Dictionary<string, OpenIdConnectConfiguration> _currentConfiguration;

        public PolicyConfigurationManager(string metadataAddress, string[] policies)
            : this(metadataAddress, policies, new HttpDocumentRetriever())
        {
        }

        public PolicyConfigurationManager(string metadataAddress, string[] policies, IDocumentRetriever docRetriever)
        {
            if (string.IsNullOrWhiteSpace(metadataAddress))
            {
                throw new ArgumentNullException("metadataAddress");
            }

            if (docRetriever == null)
            {
                throw new ArgumentNullException("retriever");
            }

            _metadataAddress = metadataAddress;
            _docRetriever = docRetriever;
            _configRetriever = new OpenIdConnectConfigurationRetriever();
            _refreshLock = new SemaphoreSlim(1);
            _syncAfter = new Dictionary<string, DateTimeOffset>();
            _lastRefresh = new Dictionary<string, DateTimeOffset>();
            _currentConfiguration = new Dictionary<string, OpenIdConnectConfiguration>();

            foreach (string policy in policies)
            {
                _currentConfiguration.Add(policy, null);
            }
        }

        public TimeSpan AutomaticRefreshInterval
        {
            get { return _automaticRefreshInterval; }
            set
            {
                if (value < MinimumAutomaticRefreshInterval)
                {
                    throw new ArgumentOutOfRangeException("value", value, string.Format(CultureInfo.InvariantCulture, ErrorMessages.IDX10107, MinimumAutomaticRefreshInterval, value));
                }
                _automaticRefreshInterval = value;
            }
        }

        public TimeSpan RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                if (value < MinimumRefreshInterval)
                {
                    throw new ArgumentOutOfRangeException("value", value, string.Format(CultureInfo.InvariantCulture, ErrorMessages.IDX10106, MinimumRefreshInterval, value));
                }
                _refreshInterval = value;
            }
        }

        // Takes the ohter and copies it to source, preserving the source's multi-valued attributes as a running sum.
        private OpenIdConnectConfiguration MergeConfig(OpenIdConnectConfiguration source, OpenIdConnectConfiguration other)
        {
            ICollection<SecurityToken> existingSigningTokens = source.SigningTokens;
            ICollection<string> existingAlgs = source.IdTokenSigningAlgValuesSupported;
            ICollection<SecurityKey> existingSigningKeys = source.SigningKeys;

            foreach (SecurityToken token in existingSigningTokens)
            {
                other.SigningTokens.Add(token);
            }

            foreach (string alg in existingAlgs)
            {
                other.IdTokenSigningAlgValuesSupported.Add(alg);
            }

            foreach (SecurityKey key in existingSigningKeys)
            {
                other.SigningKeys.Add(key);
            }

            return other;
        }

        // This non-policy specific method effectively gets the metadata for all policies specified in the constructor,
        // and merges their signing key metadata.  It selects the other metadata from one of the policies at random.
        // This is done so that the middleware can take an incoming id_token and validate it against all signing keys
        // for the app, selecting the appropriate signing key based on the key identifiers.
        public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(CancellationToken cancel)
        {
            OpenIdConnectConfiguration configUnion = new OpenIdConnectConfiguration();
            Dictionary<string, OpenIdConnectConfiguration> clone = new Dictionary<string, OpenIdConnectConfiguration>(_currentConfiguration);
            foreach (KeyValuePair<string, OpenIdConnectConfiguration> entry in clone)
            {
                OpenIdConnectConfiguration config = await GetConfigurationByPolicyAsync(cancel, entry.Key);
                configUnion = MergeConfig(configUnion, config);
            }

            return configUnion;
        }

        public async Task<OpenIdConnectConfiguration> GetConfigurationByPolicyAsync(CancellationToken cancel, string policyId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            DateTimeOffset sync;
            if (!_syncAfter.TryGetValue(policyId, out sync))
            {
                sync = DateTimeOffset.MinValue;
            }

            OpenIdConnectConfiguration config;
            if (!_currentConfiguration.TryGetValue(policyId, out config))
            {
                config = null;
            }

            if (config != null && sync > now)
            {
                return config;
            }

            await _refreshLock.WaitAsync(cancel);
            try
            {
                Exception retrieveEx = null;
                if (sync <= now)
                {
                    try
                    {
                        // We're assuming the metadata address provided in the constructor does not contain qp's
                        config = await OpenIdConnectConfigurationRetriever.GetAsync(String.Format(_metadataAddress + "?{0}={1}", policyParameter, policyId), _docRetriever, cancel);
                        _currentConfiguration[policyId] = config;
                        Contract.Assert(_currentConfiguration[policyId] != null);
                        _lastRefresh[policyId] = now;
                        _syncAfter[policyId] = now.UtcDateTime.Add(_automaticRefreshInterval);
                    }
                    catch (Exception ex)
                    {
                        retrieveEx = ex;
                        _syncAfter[policyId] = now.UtcDateTime.Add(_automaticRefreshInterval < _refreshInterval ? _automaticRefreshInterval : _refreshInterval);
                    }
                }

                if (config == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ErrorMessages.IDX10803, _metadataAddress ?? "null"), retrieveEx);
                }

                return config;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        public void RequestRefresh(string policyId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset refresh;
            if (!_lastRefresh.TryGetValue(policyId, out refresh) || now >= _lastRefresh[policyId].UtcDateTime.Add(RefreshInterval))
            {
                _syncAfter[policyId] = now;
            }
        }

        public void RequestRefresh()
        {
            foreach (KeyValuePair<string, OpenIdConnectConfiguration> entry in _currentConfiguration)
            {
                RequestRefresh(entry.Key);
            }
        }
    }
}