//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Specialized;
using System.Text;

namespace Oxite.Mvc.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static bool IsTrue(this NameValueCollection collection, string key)
        {
            bool isTrue;
            return collection != null
                && collection.GetValues(key) != null 
                && bool.TryParse(collection.GetValues(key)[0], out isTrue)
                && isTrue;
        }

        public static string ToQueryString(this NameValueCollection queryString)
        {
            if (queryString.Count > 0)
            {
                StringBuilder qs = new StringBuilder();

                qs.Append("?");
                for (int i = 0; i < queryString.Count; i++)
                {
                    if (i > 0)
                    {
                        qs.Append("&");
                    }
                    qs.AppendFormat("{0}={1}", queryString.Keys[i], queryString[i]);
                }

                return qs.ToString();
            }

            return string.Empty;
        }
    }
}