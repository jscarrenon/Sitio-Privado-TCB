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

namespace Sitio_Privado.CartolaResumida {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="tann_cartola_resumidaSoap", Namespace="http://servicios.tanner.cl/cartolaresumida")]
    public partial class tann_cartola_resumida : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private Authentication authenticationValueField;
        
        private System.Threading.SendOrPostCallback cns_titulos_cartolaOperationCompleted;
        
        private System.Threading.SendOrPostCallback _cart_selectorOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public tann_cartola_resumida() {
            this.Url = global::Sitio_Privado.Properties.Settings.Default.Sitio_Privado_CartolaResumida_tann_cartola_resumida;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public Authentication AuthenticationValue {
            get {
                return this.authenticationValueField;
            }
            set {
                this.authenticationValueField = value;
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
        public event cns_titulos_cartolaCompletedEventHandler cns_titulos_cartolaCompleted;
        
        /// <remarks/>
        public event _cart_selectorCompletedEventHandler _cart_selectorCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://servicios.tanner.cl/cartolaresumida/cns_titulos_cartola", RequestNamespace="http://servicios.tanner.cl/cartolaresumida", ResponseNamespace="http://servicios.tanner.cl/cartolaresumida", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public _titulos cns_titulos_cartola() {
            object[] results = this.Invoke("cns_titulos_cartola", new object[0]);
            return ((_titulos)(results[0]));
        }
        
        /// <remarks/>
        public void cns_titulos_cartolaAsync() {
            this.cns_titulos_cartolaAsync(null);
        }
        
        /// <remarks/>
        public void cns_titulos_cartolaAsync(object userState) {
            if ((this.cns_titulos_cartolaOperationCompleted == null)) {
                this.cns_titulos_cartolaOperationCompleted = new System.Threading.SendOrPostCallback(this.Oncns_titulos_cartolaOperationCompleted);
            }
            this.InvokeAsync("cns_titulos_cartola", new object[0], this.cns_titulos_cartolaOperationCompleted, userState);
        }
        
        private void Oncns_titulos_cartolaOperationCompleted(object arg) {
            if ((this.cns_titulos_cartolaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.cns_titulos_cartolaCompleted(this, new cns_titulos_cartolaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://servicios.tanner.cl/cartolaresumida/_cart_selector", RequestNamespace="http://servicios.tanner.cl/cartolaresumida", ResponseNamespace="http://servicios.tanner.cl/cartolaresumida", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public _cartola_alt _cart_selector(string _rut, int _selector) {
            object[] results = this.Invoke("_cart_selector", new object[] {
                        _rut,
                        _selector});
            return ((_cartola_alt)(results[0]));
        }
        
        /// <remarks/>
        public void _cart_selectorAsync(string _rut, int _selector) {
            this._cart_selectorAsync(_rut, _selector, null);
        }
        
        /// <remarks/>
        public void _cart_selectorAsync(string _rut, int _selector, object userState) {
            if ((this._cart_selectorOperationCompleted == null)) {
                this._cart_selectorOperationCompleted = new System.Threading.SendOrPostCallback(this.On_cart_selectorOperationCompleted);
            }
            this.InvokeAsync("_cart_selector", new object[] {
                        _rut,
                        _selector}, this._cart_selectorOperationCompleted, userState);
        }
        
        private void On_cart_selectorOperationCompleted(object arg) {
            if ((this._cart_selectorCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this._cart_selectorCompleted(this, new _cart_selectorCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tanner.cl/cartolaresumida")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://servicios.tanner.cl/cartolaresumida", IsNullable=false)]
    public partial class Authentication : System.Web.Services.Protocols.SoapHeader {
        
        private string passwordField;
        
        private string userNameField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tanner.cl/cartolaresumida")]
    public partial class _itemcartola {
        
        private string conceptoField;
        
        private double _valorField;
        
        /// <remarks/>
        public string concepto {
            get {
                return this.conceptoField;
            }
            set {
                this.conceptoField = value;
            }
        }
        
        /// <remarks/>
        public double _valor {
            get {
                return this._valorField;
            }
            set {
                this._valorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tanner.cl/cartolaresumida")]
    public partial class _cartola_alt {
        
        private _itemcartola[] conceptosField;
        
        private string _rutcliField;
        
        private string _periodoField;
        
        /// <remarks/>
        public _itemcartola[] conceptos {
            get {
                return this.conceptosField;
            }
            set {
                this.conceptosField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string _rutcli {
            get {
                return this._rutcliField;
            }
            set {
                this._rutcliField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string _periodo {
            get {
                return this._periodoField;
            }
            set {
                this._periodoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tanner.cl/cartolaresumida")]
    public partial class _titulo {
        
        private int _codeField;
        
        private string _descriptorField;
        
        /// <remarks/>
        public int _code {
            get {
                return this._codeField;
            }
            set {
                this._codeField = value;
            }
        }
        
        /// <remarks/>
        public string _descriptor {
            get {
                return this._descriptorField;
            }
            set {
                this._descriptorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tanner.cl/cartolaresumida")]
    public partial class _titulos {
        
        private _titulo[] _listitulosField;
        
        /// <remarks/>
        public _titulo[] _listitulos {
            get {
                return this._listitulosField;
            }
            set {
                this._listitulosField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void cns_titulos_cartolaCompletedEventHandler(object sender, cns_titulos_cartolaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cns_titulos_cartolaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal cns_titulos_cartolaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public _titulos Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((_titulos)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void _cart_selectorCompletedEventHandler(object sender, _cart_selectorCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class _cart_selectorCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal _cart_selectorCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public _cartola_alt Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((_cartola_alt)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591