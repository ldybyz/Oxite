//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;
using Oxite.Extensions;

namespace Oxite.Model.Extensions
{
    public static class PostBaseExtensions
    {
        public static string GetBodyShort(this PostBase postBase)
        {
            return !string.IsNullOrEmpty(postBase.BodyShort)
                       ? postBase.BodyShort
                       : postBase.GetBodyShort(100);
        }

        public static string GetBodyShort(this PostBase postBase, int wordCount)
        {
            string previewText = !string.IsNullOrEmpty(postBase.Body)
                                     ? postBase.Body.CleanHtmlTags().CleanWhitespace()
                                     : string.Empty;

            if (!string.IsNullOrEmpty(previewText))
            {
                previewText = string.Join(" ", previewText.Split(' ').Take(wordCount).ToArray());
            }

            return previewText;
        }

    }
}
