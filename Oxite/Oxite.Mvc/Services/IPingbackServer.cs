//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.ServiceModel;

namespace Oxite.Mvc.Services
{
    [ServiceContract(Namespace = "http://www.xmlrpc.com/pingback")]
    public interface IPingbackServer
    {
        [OperationContract(Action="pingback.ping")]
        string Ping(string sourceUri, string targetUri);
    }
}
