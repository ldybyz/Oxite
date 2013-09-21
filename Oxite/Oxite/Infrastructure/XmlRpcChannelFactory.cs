
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcChannelFactory<T> : ChannelFactory<T>
    {
        public XmlRpcChannelFactory():base(){}
        public XmlRpcChannelFactory(Binding binding) : base(binding) { }
        public XmlRpcChannelFactory(ServiceEndpoint endpoint) : base(endpoint) { }
        public XmlRpcChannelFactory(string endpointConfigurationName) : base(endpointConfigurationName) { }
        protected XmlRpcChannelFactory(Type channelType) : base(channelType) { }
        public XmlRpcChannelFactory(Binding binding, EndpointAddress remoteAddress) : base(binding,remoteAddress) { }
        public XmlRpcChannelFactory(Binding binding, string remoteAddress) : base(binding,remoteAddress) { }
        public XmlRpcChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress) { }

        protected override void ApplyConfiguration(string configurationName)
        {
            base.ApplyConfiguration(configurationName);
            if (Endpoint.Binding is XmlRpcHttpBinding)
            {
                if (Endpoint.Behaviors.Find<XmlRpcEndpointBehavior>() == null)
                {
                    Endpoint.Behaviors.Add(new XmlRpcEndpointBehavior());
                }
            }
        }
    }
}
