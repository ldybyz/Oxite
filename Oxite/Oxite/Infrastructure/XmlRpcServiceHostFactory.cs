
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcServiceHostFactory : ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new XmlRpcServiceHost(serviceType, null, baseAddresses);
        }
    }
}
