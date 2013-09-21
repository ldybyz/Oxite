
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    class XmlRpcEndpointBehaviorSection : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(XmlRpcEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new XmlRpcEndpointBehavior();
        }
    }
}
