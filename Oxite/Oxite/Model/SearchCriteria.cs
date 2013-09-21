//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model
{
    public class SearchCriteria
    {
        public string Term { get;  set; }

        public bool HasCriteria()
        {
            return !string.IsNullOrEmpty(Term);
        }
    }
}
