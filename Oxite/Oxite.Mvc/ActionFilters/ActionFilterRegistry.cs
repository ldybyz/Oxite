//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Oxite.Mvc.ActionFilters
{
    public class ActionFilterRegistry : IActionFilterRegistry
    {
        private readonly IUnityContainer container;
        private readonly List<ActionFilterRecord> registry;

        public ActionFilterRegistry(IUnityContainer container)
        {
            this.container = container;
            registry = new List<ActionFilterRecord>();
        }

        public void Clear()
        {
            registry.Clear();
        }

        public void Add(IEnumerable<IActionFilterCriteria> criteria, Type filterType)
        {
            registry.Add(new ActionFilterRecord(criteria, filterType));
        }

        public FilterInfo GetFilters(ActionFilterRegistryContext context)
        {
            FilterInfo filters = new FilterInfo();

            foreach (ActionFilterRecord record in registry)
            {
                if (record.Match(context))
                {
                    object filterObject = container.Resolve(record.FilterType);

                    if (filterObject is IActionFilter)
                        filters.ActionFilters.Add((IActionFilter)filterObject);

                    if (filterObject is IAuthorizationFilter)
                        filters.AuthorizationFilters.Add((IAuthorizationFilter)filterObject);

                    if (filterObject is IExceptionFilter)
                        filters.ExceptionFilters.Add((IExceptionFilter)filterObject);

                    if (filterObject is IResultFilter)
                        filters.ResultFilters.Add((IResultFilter)filterObject);
                }
            }

            return filters;
        }
    }
}
