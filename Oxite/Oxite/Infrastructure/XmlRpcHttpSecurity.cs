
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public sealed class XmlRpcHttpSecurity
    {
        // Fields
        internal const XmlRpcHttpSecurityMode DefaultMode = XmlRpcHttpSecurityMode.None;
        private XmlRpcHttpSecurityMode mode;
        private HttpClientCredentialType clientCredentialType;
        private HttpProxyCredentialType proxyCredentialType;
        private string realm;

        // Methods
        internal XmlRpcHttpSecurity()
            : this(XmlRpcHttpSecurityMode.None,HttpClientCredentialType.None,HttpProxyCredentialType.None,null)
        {
            
        }

        private XmlRpcHttpSecurity(XmlRpcHttpSecurityMode mode, HttpClientCredentialType clientCredentialType, HttpProxyCredentialType proxyCredentialType, string realm)
        {
            this.Mode = mode;
            this.ClientCredentialType = clientCredentialType;
            this.ProxyCredentialType = proxyCredentialType;
            this.Realm = realm;

        }

        public XmlRpcHttpSecurityMode Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }

        public HttpClientCredentialType ClientCredentialType
        {
            get { return clientCredentialType; }
            set { clientCredentialType = value; }
        }
        public HttpProxyCredentialType ProxyCredentialType
        {
            get { return proxyCredentialType; }
            set { proxyCredentialType = value; }
        }
        public string Realm
        {
            get { return realm; }
            set { realm = value; }
        }

        
    }
}

