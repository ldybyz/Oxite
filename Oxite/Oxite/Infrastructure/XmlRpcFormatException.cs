
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    class XmlRpcFormatException : Exception
    {
        public XmlRpcFormatException():base()
        {
        }

        public XmlRpcFormatException(string message)
            : base(message)
        {
        }
    }
}
