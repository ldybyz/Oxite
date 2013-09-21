
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcHttpSecurityElement : ConfigurationElement
    {
        // Fields
        private ConfigurationPropertyCollection properties;

        // Methods
        internal void ApplyConfiguration(XmlRpcHttpSecurity security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }
            security.Mode = this.Mode;
            security.ClientCredentialType = this.Transport.ClientCredentialType;
            security.ProxyCredentialType = this.Transport.ProxyCredentialType;
            security.Realm = this.Transport.Realm;
        }

        internal void InitializeFrom(XmlRpcHttpSecurity security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }
            this.Mode = security.Mode;
            this.Transport.ClientCredentialType = security.ClientCredentialType;
            this.Transport.ProxyCredentialType = security.ProxyCredentialType;
            this.Transport.Realm = security.Realm;
        }

        [ConfigurationProperty("mode", DefaultValue = 0)]
        public XmlRpcHttpSecurityMode Mode
        {
            get
            {
                return (XmlRpcHttpSecurityMode)base["mode"];
            }
            set
            {
                base["mode"] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (this.properties == null)
                {
                    ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();
                    properties.Add(new ConfigurationProperty("mode", typeof(XmlRpcHttpSecurityMode), XmlRpcHttpSecurityMode.None, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("transport", typeof(HttpTransportSecurityElement), null, null, null, ConfigurationPropertyOptions.None));
                    this.properties = properties;
                }
                return this.properties;
            }
        }

        [ConfigurationProperty("transport")]
        public HttpTransportSecurityElement Transport
        {
            get
            {
                return (HttpTransportSecurityElement)base["transport"];
            }
        }
    }
}
