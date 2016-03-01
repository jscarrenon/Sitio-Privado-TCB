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
        
        private System.Threading.SendOrPostCallback cns_total_generalOperationCompleted;
        
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
        public event cns_total_generalCompletedEventHandler cns_total_generalCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://servicios.tanner.cl/cartolaresumida/cns_total_general", RequestNamespace="http://servicios.tanner.cl/cartolaresumida", ResponseNamespace="http://servicios.tanner.cl/cartolaresumida", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public _cartola_alt cns_total_general(string _rut) {
            object[] results = this.Invoke("cns_total_general", new object[] {
                        _rut});
            return ((_cartola_alt)(results[0]));
        }
        
        /// <remarks/>
        public void cns_total_generalAsync(string _rut) {
            this.cns_total_generalAsync(_rut, null);
        }
        
        /// <remarks/>
        public void cns_total_generalAsync(string _rut, object userState) {
            if ((this.cns_total_generalOperationCompleted == null)) {
                this.cns_total_generalOperationCompleted = new System.Threading.SendOrPostCallback(this.Oncns_total_generalOperationCompleted);
            }
            this.InvokeAsync("cns_total_general", new object[] {
                        _rut}, this.cns_total_generalOperationCompleted, userState);
        }
        
        private void Oncns_total_generalOperationCompleted(object arg) {
            if ((this.cns_total_generalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.cns_total_generalCompleted(this, new cns_total_generalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public partial class _itemcartola {
        
        private string conceptoField;
        
        private double _valorField;
        
        private double _porcentajeField;
        
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
        
        /// <remarks/>
        public double _porcentaje {
            get {
                return this._porcentajeField;
            }
            set {
                this._porcentajeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void cns_total_generalCompletedEventHandler(object sender, cns_total_generalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cns_total_generalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal cns_total_generalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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