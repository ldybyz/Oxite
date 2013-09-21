
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.ServiceModel;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcHttpBindingElement : StandardBindingElement
    {
        // Fields
        private ConfigurationPropertyCollection properties;

        // Methods
        public XmlRpcHttpBindingElement()
            : this(null)
        {
        }

        public XmlRpcHttpBindingElement(string name)
            : base(name)
        {
        }

        protected override void InitializeFrom(Binding binding)
        {
            base.InitializeFrom(binding);
            XmlRpcHttpBinding xmlRpcHttpBinding = (XmlRpcHttpBinding)binding;
            this.BypassProxyOnLocal = xmlRpcHttpBinding.BypassProxyOnLocal;
            this.HostNameComparisonMode = xmlRpcHttpBinding.HostNameComparisonMode;
            this.MaxBufferSize = xmlRpcHttpBinding.MaxBufferSize;
            this.MaxBufferPoolSize = xmlRpcHttpBinding.MaxBufferPoolSize;
            this.MaxReceivedMessageSize = xmlRpcHttpBinding.MaxReceivedMessageSize;
            this.ProxyAddress = xmlRpcHttpBinding.ProxyAddress;
            this.TransferMode = xmlRpcHttpBinding.TransferMode;
            this.UseDefaultWebProxy = xmlRpcHttpBinding.UseDefaultWebProxy;
            this.AllowCookies = xmlRpcHttpBinding.AllowCookies;
            this.Security.InitializeFrom(xmlRpcHttpBinding.Security);
    
            this.ReaderQuotas.MaxDepth = xmlRpcHttpBinding.ReaderQuotas.MaxDepth;
            this.ReaderQuotas.MaxStringContentLength = xmlRpcHttpBinding.ReaderQuotas.MaxStringContentLength;
            this.ReaderQuotas.MaxArrayLength = xmlRpcHttpBinding.ReaderQuotas.MaxArrayLength;
            this.ReaderQuotas.MaxBytesPerRead = xmlRpcHttpBinding.ReaderQuotas.MaxBytesPerRead;
            this.ReaderQuotas.MaxNameTableCharCount = xmlRpcHttpBinding.ReaderQuotas.MaxNameTableCharCount;
        }

        protected override void OnApplyConfiguration(Binding binding)
        {
            XmlRpcHttpBinding xmlRpcHttpBinding = (XmlRpcHttpBinding)binding;
            xmlRpcHttpBinding.BypassProxyOnLocal = this.BypassProxyOnLocal;
            xmlRpcHttpBinding.HostNameComparisonMode = this.HostNameComparisonMode;
            xmlRpcHttpBinding.MaxBufferPoolSize = this.MaxBufferPoolSize;
            xmlRpcHttpBinding.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            xmlRpcHttpBinding.TransferMode = this.TransferMode;
            xmlRpcHttpBinding.UseDefaultWebProxy = this.UseDefaultWebProxy;
            xmlRpcHttpBinding.AllowCookies = this.AllowCookies;
            xmlRpcHttpBinding.ProxyAddress = this.ProxyAddress;
            if (base.ElementInformation.Properties["maxBufferSize"].ValueOrigin != PropertyValueOrigin.Default)
            {
                xmlRpcHttpBinding.MaxBufferSize = this.MaxBufferSize;
            }
            this.Security.ApplyConfiguration(xmlRpcHttpBinding.Security);
            if (this.ReaderQuotas.ElementInformation.IsPresent)
            {
                xmlRpcHttpBinding.ReaderQuotas.MaxDepth = this.ReaderQuotas.MaxDepth;
                xmlRpcHttpBinding.ReaderQuotas.MaxStringContentLength = this.ReaderQuotas.MaxStringContentLength;
                xmlRpcHttpBinding.ReaderQuotas.MaxArrayLength = this.ReaderQuotas.MaxArrayLength;
                xmlRpcHttpBinding.ReaderQuotas.MaxBytesPerRead = this.ReaderQuotas.MaxBytesPerRead =
                xmlRpcHttpBinding.ReaderQuotas.MaxNameTableCharCount = this.ReaderQuotas.MaxNameTableCharCount;
            }
        }

        // Properties
        [ConfigurationProperty("allowCookies", DefaultValue = false)]
        public bool AllowCookies
        {
            get
            {
                return (bool)base["allowCookies"];
            }
            set
            {
                base["allowCookies"] = value;
            }
        }

        protected override Type BindingElementType
        {
            get
            {
                return typeof(XmlRpcHttpBinding);
            }
        }

        [ConfigurationProperty("bypassProxyOnLocal", DefaultValue = false)]
        public bool BypassProxyOnLocal
        {
            get
            {
                return (bool)base["bypassProxyOnLocal"];
            }
            set
            {
                base["bypassProxyOnLocal"] = value;
            }
        }

        [ConfigurationProperty("hostNameComparisonMode", DefaultValue = 0)]
        public HostNameComparisonMode HostNameComparisonMode
        {
            get
            {
                return (HostNameComparisonMode)base["hostNameComparisonMode"];
            }
            set
            {
                base["hostNameComparisonMode"] = value;
            }
        }

        [LongValidator(MinValue = 0), ConfigurationProperty("maxBufferPoolSize", DefaultValue = 0x80000)]
        public long MaxBufferPoolSize
        {
            get
            {
                return (long)base["maxBufferPoolSize"];
            }
            set
            {
                base["maxBufferPoolSize"] = value;
            }
        }

        [ConfigurationProperty("maxBufferSize", DefaultValue = 0x10000), IntegerValidator(MinValue = 1)]
        public int MaxBufferSize
        {
            get
            {
                return (int)base["maxBufferSize"];
            }
            set
            {
                base["maxBufferSize"] = value;
            }
        }

        [LongValidator(MinValue = 1), ConfigurationProperty("maxReceivedMessageSize", DefaultValue = 0x10000)]
        public long MaxReceivedMessageSize
        {
            get
            {
                return (long)base["maxReceivedMessageSize"];
            }
            set
            {
                base["maxReceivedMessageSize"] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (this.properties == null)
                {
                    ConfigurationPropertyCollection properties = base.Properties;
                    properties.Add(new ConfigurationProperty("allowCookies", typeof(bool), false, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("bypassProxyOnLocal", typeof(bool), false, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("hostNameComparisonMode", typeof(HostNameComparisonMode), HostNameComparisonMode.StrongWildcard, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("maxBufferSize", typeof(int), 0x10000, null, new IntegerValidator(1, 0x7fffffff, false), ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("maxBufferPoolSize", typeof(long), (long)0x80000, null, new LongValidator((long)0, 0x7fffffffffffffff, false), ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("maxReceivedMessageSize", typeof(long), (long)0x10000, null, new LongValidator((long)1, 0x7fffffffffffffff, false), ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("proxyAddress", typeof(Uri), null, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("readerQuotas", typeof(XmlDictionaryReaderQuotasElement), new XmlDictionaryReaderQuotasElement(), null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("security", typeof(XmlRpcHttpSecurityElement), null, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("transferMode", typeof(TransferMode), TransferMode.Buffered, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("useDefaultWebProxy", typeof(bool), true, null, null, ConfigurationPropertyOptions.None));
                    this.properties = properties;
                }
                return this.properties;
            }
        }

        [ConfigurationProperty("proxyAddress", DefaultValue = null)]
        public Uri ProxyAddress
        {
            get
            {
                return (Uri)base["proxyAddress"];
            }
            set
            {
                base["proxyAddress"] = value;
            }
        }

        [ConfigurationProperty("readerQuotas")]
        public XmlDictionaryReaderQuotasElement ReaderQuotas
        {
            get
            {
                return (XmlDictionaryReaderQuotasElement)base["readerQuotas"];
            }
        }

        [ConfigurationProperty("security")]
        public XmlRpcHttpSecurityElement Security
        {
            get
            {
                return (XmlRpcHttpSecurityElement)base["security"];
            }
        }

        [ConfigurationProperty("transferMode", DefaultValue = 0)]
        public TransferMode TransferMode
        {
            get
            {
                return (TransferMode)base["transferMode"];
            }
            set
            {
                base["transferMode"] = value;
            }
        }

        [ConfigurationProperty("useDefaultWebProxy", DefaultValue = true)]
        public bool UseDefaultWebProxy
        {
            get
            {
                return (bool)base["useDefaultWebProxy"];
            }
            set
            {
                base["useDefaultWebProxy"] = value;
            }
        }
    }
}
