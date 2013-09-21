
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Configuration;
using System.Xml;
using System.ServiceModel.Configuration;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcHttpBinding : Binding
    {
        // Fields
        private HttpsTransportBindingElement httpsTransport;
        private HttpTransportBindingElement httpTransport;
        private XmlRpcHttpSecurity security;
        private XmlRpcMessageEncodingBindingElement xmlRpcTextEncoding;

        // Methods
        public XmlRpcHttpBinding()
            : this(XmlRpcHttpSecurityMode.None)
        {
        }

        private XmlRpcHttpBinding(XmlRpcHttpSecurity security)
        {
            this.security = new XmlRpcHttpSecurity();
            this.Initialize();
            this.security = security;
        }

        public XmlRpcHttpBinding(XmlRpcHttpSecurityMode securityMode)
        {
            this.security = new XmlRpcHttpSecurity();
            this.Initialize();
            this.security.Mode = securityMode;
        }

        public XmlRpcHttpBinding(string configurationName)
            : this()
        {
            this.ApplyConfiguration(configurationName);
        }

        private void Initialize()
        {
            this.httpTransport = new HttpTransportBindingElement();
            this.httpsTransport = new HttpsTransportBindingElement();
            this.xmlRpcTextEncoding = new XmlRpcMessageEncodingBindingElement();
        }


        private void ApplyConfiguration(string configurationName)
        {
            XmlRpcHttpBindingElement xmlRpcBindingElement = XmlRpcHttpBindingCollectionElement.GetBindingCollectionElement().Bindings[configurationName];
            if (xmlRpcBindingElement == null)
            {
                throw new ConfigurationErrorsException(String.Format("Invalid binding configuration name '{0}' in XmlRpcHttpBinding", configurationName));
            }
            xmlRpcBindingElement.ApplyConfiguration(this);
        }

        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection bindingElements = new BindingElementCollection();
            bindingElements.Add(this.xmlRpcTextEncoding);
            bindingElements.Add(this.GetTransport());            
            return bindingElements.Clone();
        }

        private TransportBindingElement GetTransport()
        {
            if (this.security.Mode == XmlRpcHttpSecurityMode.Transport)
            {
                //CFV:this.security.EnableTransportSecurity(this.httpsTransport);
                return this.httpsTransport;
            }
            if (this.security.Mode == XmlRpcHttpSecurityMode.TransportCredentialOnly)
            {
                //CFV:this.security.EnableTransportAuthentication(this.httpTransport);
                return this.httpTransport;
            }
            //CFV:this.security.DisableTransportAuthentication(this.httpTransport);
            this.httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
            return this.httpTransport;
        }

        
        // Properties
        public bool AllowCookies
        {
            get
            {
                return this.httpTransport.AllowCookies;
            }
            set
            {
                this.httpTransport.AllowCookies = value;
                this.httpsTransport.AllowCookies = value;
            }
        }

        public bool BypassProxyOnLocal
        {
            get
            {
                return this.httpTransport.BypassProxyOnLocal;
            }
            set
            {
                this.httpTransport.BypassProxyOnLocal = value;
                this.httpsTransport.BypassProxyOnLocal = value;
            }
        }

        public EnvelopeVersion EnvelopeVersion
        {
            get
            {
                return EnvelopeVersion.None;
            }
        }

        public HostNameComparisonMode HostNameComparisonMode
        {
            get
            {
                return this.httpTransport.HostNameComparisonMode;
            }
            set
            {
                this.httpTransport.HostNameComparisonMode = value;
                this.httpsTransport.HostNameComparisonMode = value;
            }
        }

        public long MaxBufferPoolSize
        {
            get
            {
                return this.httpTransport.MaxBufferPoolSize;
            }
            set
            {
                this.httpTransport.MaxBufferPoolSize = value;
                this.httpsTransport.MaxBufferPoolSize = value;
            }
        }

        public int MaxBufferSize
        {
            get
            {
                return this.httpTransport.MaxBufferSize;
            }
            set
            {
                this.httpTransport.MaxBufferSize = value;
                this.httpsTransport.MaxBufferSize = value;
                //this.xmlRpcTextEncoding.MaxBufferSize = value;
            }
        }

        public long MaxReceivedMessageSize
        {
            get
            {
                return this.httpTransport.MaxReceivedMessageSize;
            }
            set
            {
                this.httpTransport.MaxReceivedMessageSize = value;
                this.httpsTransport.MaxReceivedMessageSize = value;
            }
        }

        public Uri ProxyAddress
        {
            get
            {
                return this.httpTransport.ProxyAddress;
            }
            set
            {
                this.httpTransport.ProxyAddress = value;
                this.httpsTransport.ProxyAddress = value;
            }
        }

        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {
                return this.xmlRpcTextEncoding.ReaderQuotas;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                value.CopyTo(this.xmlRpcTextEncoding.ReaderQuotas);
            }
        }

        public override string Scheme
        {
            get
            {
                return this.GetTransport().Scheme;
            }
        }

        public XmlRpcHttpSecurity Security
        {
            get
            {
                return this.security;
            }
        }

        
        public TransferMode TransferMode
        {
            get
            {
                return this.httpTransport.TransferMode;
            }
            set
            {
                this.httpTransport.TransferMode = value;
                this.httpsTransport.TransferMode = value;
            }
        }

        public bool UseDefaultWebProxy
        {
            get
            {
                return this.httpTransport.UseDefaultWebProxy;
            }
            set
            {
                this.httpTransport.UseDefaultWebProxy = value;
                this.httpsTransport.UseDefaultWebProxy = value;
            }
        }
    }



}
