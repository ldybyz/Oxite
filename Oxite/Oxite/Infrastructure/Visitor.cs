//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;

namespace Oxite.Infrastructure
{
    public abstract class Visitor
    {
        public T Visit<T>(object arg, params object[] extraArgs)
        {
            var method = from m in GetType().GetMethods()
                         where m.Name == "Visit" && m.GetParameters().Length == 1 + extraArgs.Length
                         && arg.GetType().IsAssignableFrom(m.GetParameters()[0].ParameterType)
                         && m.ReturnType == typeof(T)
                         orderby m.GetParameters()[0].ParameterType.Name == arg.GetType().Name descending, m.GetParameters()[0].ParameterType.Name ascending
                         select m;

            return (T)method.First().Invoke(this, new[] { arg }.Concat(extraArgs).ToArray());
        }
    }
}
