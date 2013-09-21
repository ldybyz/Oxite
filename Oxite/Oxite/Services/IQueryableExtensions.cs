//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;
using Oxite.Model;

namespace Oxite.Services
{
    public static class IQueryableExtensions
    {
        public static PageOfList<T> GetPage<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return new PageOfList<T>(query.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, query.Count());
        }
    }
}
