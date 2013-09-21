
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    internal class XmlRpcProtocol
    {
        internal const string MethodCall = "methodCall";
        internal const string MethodResponse = "methodResponse";
        internal const string MethodName = "methodName";
        internal const string Int32 = "i4";
        internal const string Integer = "int";
        internal const string DateTime = "dateTime.iso8601";
        internal const string String = "string";
        internal const string ByteArray = "base64";
        internal const string Bool = "boolean";
        internal const string Struct = "struct";
        internal const string Member = "member";
        internal const string Value = "value";
        internal const string Name = "name";
        internal const string Params = "params";
        internal const string Param = "param";
        internal const string Array = "array";
        internal const string Double = "double";
        internal const string Data = "data";
        internal const string Nil = "nil";
    }
}
