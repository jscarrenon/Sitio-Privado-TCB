﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Sitio_Privado.InformacionClienteAgente {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="tann_info_clienteSoap", Namespace="http://servicios.tanner/infocliente/")]
    public partial class tann_info_cliente : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback cli_info_agenteOperationCompleted;
        
        private System.Threading.SendOrPostCallback cli_itokenOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public tann_info_cliente() {
            this.Url = global::Sitio_Privado.Properties.Settings.Default.Sitio_Privado_InformacionClienteAgente_tann_info_cliente;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event cli_info_agenteCompletedEventHandler cli_info_agenteCompleted;
        
        /// <remarks/>
        public event cli_itokenCompletedEventHandler cli_itokenCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://servicios.tanner/infocliente/cli_info_agente", RequestNamespace="http://servicios.tanner/infocliente/", ResponseNamespace="http://servicios.tanner/infocliente/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public _agente cli_info_agente(string _rut, short _sec) {
            object[] results = this.Invoke("cli_info_agente", new object[] {
                        _rut,
                        _sec});
            return ((_agente)(results[0]));
        }
        
        /// <remarks/>
        public void cli_info_agenteAsync(string _rut, short _sec) {
            this.cli_info_agenteAsync(_rut, _sec, null);
        }
        
        /// <remarks/>
        public void cli_info_agenteAsync(string _rut, short _sec, object userState) {
            if ((this.cli_info_agenteOperationCompleted == null)) {
                this.cli_info_agenteOperationCompleted = new System.Threading.SendOrPostCallback(this.Oncli_info_agenteOperationCompleted);
            }
            this.InvokeAsync("cli_info_agente", new object[] {
                        _rut,
                        _sec}, this.cli_info_agenteOperationCompleted, userState);
        }
        
        private void Oncli_info_agenteOperationCompleted(object arg) {
            if ((this.cli_info_agenteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.cli_info_agenteCompleted(this, new cli_info_agenteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://servicios.tanner/infocliente/cli_itoken", RequestNamespace="http://servicios.tanner/infocliente/", ResponseNamespace="http://servicios.tanner/infocliente/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string cli_itoken(string rut) {
            object[] results = this.Invoke("cli_itoken", new object[] {
                        rut});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void cli_itokenAsync(string rut) {
            this.cli_itokenAsync(rut, null);
        }
        
        /// <remarks/>
        public void cli_itokenAsync(string rut, object userState) {
            if ((this.cli_itokenOperationCompleted == null)) {
                this.cli_itokenOperationCompleted = new System.Threading.SendOrPostCallback(this.Oncli_itokenOperationCompleted);
            }
            this.InvokeAsync("cli_itoken", new object[] {
                        rut}, this.cli_itokenOperationCompleted, userState);
        }
        
        private void Oncli_itokenOperationCompleted(object arg) {
            if ((this.cli_itokenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.cli_itokenCompleted(this, new cli_itokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tanner/infocliente/")]
    public partial class _agente {
        
        private int _codigoField;
        
        private string _nombreField;
        
        private string _sucursalField;
        
        private string _emailField;
        
        private string _fonoField;
        
        private string _pathimgField;
        
        private string _fechacertField;
        
        private string _fechavctoField;
        
        private string _glosacertField;
        
        /// <remarks/>
        public int _codigo {
            get {
                return this._codigoField;
            }
            set {
                this._codigoField = value;
            }
        }
        
        /// <remarks/>
        public string _nombre {
            get {
                return this._nombreField;
            }
            set {
                this._nombreField = value;
            }
        }
        
        /// <remarks/>
        public string _sucursal {
            get {
                return this._sucursalField;
            }
            set {
                this._sucursalField = value;
            }
        }
        
        /// <remarks/>
        public string _email {
            get {
                return this._emailField;
            }
            set {
                this._emailField = value;
            }
        }
        
        /// <remarks/>
        public string _fono {
            get {
                return this._fonoField;
            }
            set {
                this._fonoField = value;
            }
        }
        
        /// <remarks/>
        public string _pathimg {
            get {
                return this._pathimgField;
            }
            set {
                this._pathimgField = value;
            }
        }
        
        /// <remarks/>
        public string _fechacert {
            get {
                return this._fechacertField;
            }
            set {
                this._fechacertField = value;
            }
        }
        
        /// <remarks/>
        public string _fechavcto {
            get {
                return this._fechavctoField;
            }
            set {
                this._fechavctoField = value;
            }
        }
        
        /// <remarks/>
        public string _glosacert {
            get {
                return this._glosacertField;
            }
            set {
                this._glosacertField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void cli_info_agenteCompletedEventHandler(object sender, cli_info_agenteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cli_info_agenteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal cli_info_agenteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public _agente Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((_agente)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void cli_itokenCompletedEventHandler(object sender, cli_itokenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cli_itokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal cli_itokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591