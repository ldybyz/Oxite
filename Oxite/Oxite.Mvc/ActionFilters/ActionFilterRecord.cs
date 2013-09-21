//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Mvc.ActionFilters
{
    public class ActionFilterRecord
    {
        private readonly List<IActionFilterCriteria> criteria;
        public Type FilterType { get; private set; }
        
        public ActionFilterRecord(IEnumerable<IActionFilterCriteria> criteria, Type filterType)
        {
            this.criteria = new List<IActionFilterCriteria>(criteria);
            FilterType = filterType;
        }

        public bool Match(ActionFilterRegistryContext context)
        {
            return criteria.Aggregate(true, (prev, f) => prev ? f.Match(context) : prev);
        }
    }
}
