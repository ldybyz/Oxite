//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Text.RegularExpressions;
using Oxite.Infrastructure;

namespace Oxite.Extensions
{
    public static class IRegularExpressionsExtensions
    {
        public static string Slugify(this IRegularExpressions expressions, string value)
        {
            string slug = "";

            if (!string.IsNullOrEmpty(value))
            {
                Regex regex = expressions.GetExpression("SlugReplace");

                slug = value.Trim();
                slug = slug.Replace(' ', '-');
                slug = slug.Replace("---", "-");
                slug = slug.Replace("--", "-");
                if (regex != null)
                {
                    slug = regex.Replace(slug, "");
                }

                if (slug.Length * 2 < value.Length)
                {
                    return "";
                }

                if (slug.Length > 100)
                {
                    slug = slug.Substring(0, 100);
                }
            }

            return slug;
        }

        public static bool IsUrl(this IRegularExpressions expressions, string value)
        {
            if (!(value.StartsWith("http://") || value.StartsWith("https://")))
            {
                value = string.Format("http://{0}", value);
            }

            return expressions.IsMatch("IsUrl", value);
        }
    }
}
