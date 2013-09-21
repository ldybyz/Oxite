//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeController : Controller
    {
        public object ActionWithParameters(string param)
        {
            throw new NotImplementedException();
        }

        public string Action()
        {
            throw new NotImplementedException();
        }

        [ActionName("AliasedAction")]
        public string RealNameIsAliasedAction()
        {
            throw new NotImplementedException();
        }
    }
}
