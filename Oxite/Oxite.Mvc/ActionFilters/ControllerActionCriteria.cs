//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Oxite.Mvc.ActionFilters
{
    public class ControllerActionCriteria : IActionFilterCriteria
    {
        private readonly List<ReflectedActionDescriptor> methods = new List<ReflectedActionDescriptor>();

        public void AddMethod<T>(Expression<Func<T, object>> method)
        {
            MethodCallExpression methodCall = method.Body as MethodCallExpression;

            if (methodCall == null)
                throw new InvalidOperationException();

            methods.Add(new ReflectedActionDescriptor(methodCall.Method, methodCall.Method.Name, new ReflectedControllerDescriptor(methodCall.Object.Type)));
        }

        public bool Match(ActionFilterRegistryContext context)
        {
            ReflectedActionDescriptor reflectedDescriptor = context.ActionDescriptor as ReflectedActionDescriptor;
            if (reflectedDescriptor != null)
                return methods.Any(a => a.MethodInfo == reflectedDescriptor.MethodInfo);

            return methods.Any(a => a.ControllerDescriptor.FindAction(context.ControllerContext, context.ActionDescriptor.ActionName) != null);
        }
    }
}
