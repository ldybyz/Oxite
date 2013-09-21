//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model.Extensions
{
    public static class INamedEntityExtensions
    {
        public static string GetDisplayName(this INamedEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.DisplayName))
                return entity.DisplayName;

            return entity.Name;
        }
    }
}
