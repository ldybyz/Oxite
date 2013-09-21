
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    class XmlRpcOperationSelector : IDispatchOperationSelector
    {
        ContractDescription _contract;
        public XmlRpcOperationSelector(ContractDescription contract)
        {
            _contract = contract;
        }

        public string SelectOperation(ref System.ServiceModel.Channels.Message message)
        {
            if ( message.Properties.ContainsKey("XmlRpcMethodName") )
            {
                foreach (OperationDescription op in _contract.Operations)
                {
                    if ( op.Messages[0].Action.EndsWith((string)message.Properties["XmlRpcMethodName"]))
                    {
                        return op.Name;
                    }
                }
            }
            return null;
        }
    }
}
