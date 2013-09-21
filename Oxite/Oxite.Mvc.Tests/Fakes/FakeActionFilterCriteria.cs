//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Mvc.ActionFilters;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeActionFilterCriteria : IActionFilterCriteria
    {
        public bool IsMatch { get; set; }

        #region IActionFilterCriteria Members

        public bool Match(ActionFilterRegistryContext context)
        {
            return IsMatch;
        }

        #endregion
    }
}
