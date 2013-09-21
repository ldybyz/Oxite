
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security;
using Microsoft.Practices.Unity;
using Oxite.Infrastructure;

[assembly:AllowPartiallyTrustedCallers()]

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcServiceHost : ServiceHost
    {
        private IUnityContainer container;

        public XmlRpcServiceHost(Type serviceType, IUnityContainer container, params Uri[] baseAddresses):base(serviceType,baseAddresses)
        {
            this.container = container;

            ServiceMetadataBehavior metadata = new ServiceMetadataBehavior();
            metadata.HttpGetEnabled = true;

            this.Description.Behaviors.Add(metadata);

            foreach (ContractDescription contract in this.ImplementedContracts.Values)
            {
                contract.Behaviors.Add(new UnityInstancingBehavior(container, serviceType));
            }
        }

        protected override void InitializeRuntime()
        {
            if (base.Description.Endpoints.Count == 0)
            {
                Type implementedContract = null;
                foreach (ContractDescription contractDescription in base.ImplementedContracts.Values)
                {
                    implementedContract = contractDescription.ContractType;
                    break;
                }
                foreach (Uri uri in base.BaseAddresses)
                {
                    if (uri.Scheme == Uri.UriSchemeHttps)
                    {
                        base.AddServiceEndpoint(implementedContract, new XmlRpcHttpBinding(XmlRpcHttpSecurityMode.Transport), uri);
                    }
                    else if (uri.Scheme == Uri.UriSchemeHttp)
                    {
                        base.AddServiceEndpoint(implementedContract, new XmlRpcHttpBinding(XmlRpcHttpSecurityMode.None), uri);
                    }
                }
                this.AddServiceEndpoint("IMetadataExchange", MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            }
            foreach (ServiceEndpoint endpoint in this.Description.Endpoints)
            {
                if (endpoint.Binding is XmlRpcHttpBinding)
                {
                    if (endpoint.Behaviors.Find<XmlRpcEndpointBehavior>() == null)
                    {
                        endpoint.Behaviors.Add( new XmlRpcEndpointBehavior() );
                    }
                }
            }
            base.InitializeRuntime();
        }
    }
}
