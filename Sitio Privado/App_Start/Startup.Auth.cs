﻿using Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Sitio_Privado.Policies;
using System.Threading;
using IdentityServer3.AccessTokenValidation;
using System.Web.Http;
using Sitio_Privado.Infraestructure.ExceptionHandling;
using NLog;

namespace Sitio_Privado
{
    public partial class Startup
    {
        
        // The ACR claim is used to indicate which policy was executed
        public const string AcrClaimType = "http://schemas.microsoft.com/claims/authnclassreference";
        public const string PolicyKey = "b2cpolicy";
        public const string OIDCMetadataSuffix = "/.well-known/openid-configuration";

        // App config settings
        public static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        public static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        public static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        // B2C policy identifiers
        public static string SignUpPolicyId = ConfigurationManager.AppSettings["ida:SignUpPolicyId"];
        public static string SignInPolicyId = ConfigurationManager.AppSettings["ida:SignInPolicyId"];
        public static string ProfilePolicyId = ConfigurationManager.AppSettings["ida:UserProfilePolicyId"];

        // Custom login process parameters
        public const string objectIdClaimKey = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public static string temporalPasswordTimeout = ConfigurationManager.AppSettings["tempPass:Timeout"];
        public const string isTemporalPasswordClaimKey = "isTemporalPassword";
        public const string temporalPasswordTimestampClaimKey = "temporalPasswordTimestamp";

        public void ConfigureAuth(IAppBuilder app)
        {
            Logger logger = LogManager.GetLogger("SessionLog");

            logger.Info("Starting server...");
            logger.Info("Authority url is: " + ConfigurationManager.AppSettings["AuthorityUrl"]);
            logger.Info("Required group is: " + ConfigurationManager.AppSettings["RequiredGroup"]);

            app.Use<OwinExceptionHandler>(app);
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                ValidationMode = ValidationMode.ValidationEndpoint,
                Authority = ConfigurationManager.AppSettings["AuthorityUrl"]
            });

            logger.Info("Server started.");
        }

        // This notification can be used to manipulate the OIDC request before it is sent.  Here we use it to send the correct policy.
        private async Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            PolicyConfigurationManager mgr = notification.Options.ConfigurationManager as PolicyConfigurationManager;
            if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
            {
                OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, notification.OwinContext.Authentication.AuthenticationResponseRevoke.Properties.Dictionary[Startup.PolicyKey]);
                notification.ProtocolMessage.IssuerAddress = config.EndSessionEndpoint;
            }
            else
            {
                OpenIdConnectConfiguration config = await mgr.GetConfigurationByPolicyAsync(CancellationToken.None, notification.OwinContext.Authentication.AuthenticationResponseChallenge.Properties.Dictionary[Startup.PolicyKey]);
                notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
            }
        }

        // Used for avoiding yellow-screen-of-death
        private Task AuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            notification.Response.Redirect("/Home/Error?message=" + notification.Exception.Message);
            return Task.FromResult(0);
        }
    }
}