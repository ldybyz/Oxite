
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Oxite.Infrastructure;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            if (endpoint.Binding is XmlRpcHttpBinding)
            {
                foreach (OperationDescription opDesc in endpoint.Contract.Operations)
                {
                    ReplaceFormatterBehavior(opDesc, endpoint);
                }
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            if (endpoint.Binding is XmlRpcHttpBinding)
            {
                endpointDispatcher.ContractFilter = new MatchAllMessageFilter();
                endpointDispatcher.DispatchRuntime.OperationSelector = new XmlRpcOperationSelector(endpoint.Contract);

                foreach (OperationDescription opDesc in endpoint.Contract.Operations)
                {
                    ReplaceFormatterBehavior(opDesc, endpoint);
                }

                endpointDispatcher.DispatchRuntime.InstanceContextInitializers.Add(new InstanceContextDictionary());
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        private void ReplaceFormatterBehavior(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            if (operationDescription.Behaviors.Find<XmlRpcOperationFormatterBehavior>() == null)
            {
                XmlRpcOperationFormatterBehavior formatterBehavior = 
                    new XmlRpcOperationFormatterBehavior(
                      operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>(),
                      operationDescription.Behaviors.Find<XmlSerializerOperationBehavior>());
                operationDescription.Behaviors.Remove<DataContractSerializerOperationBehavior>();
                operationDescription.Behaviors.Remove<XmlSerializerOperationBehavior>();
                operationDescription.Behaviors.Add(formatterBehavior);
            }
        }
    }
}
