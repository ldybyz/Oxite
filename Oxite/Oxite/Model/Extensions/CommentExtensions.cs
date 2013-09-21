//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Model.Extensions
{
    public static class CommentExtensions
    {
        public static string GetUrl(this Comment comment, Post post, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(
                context,
                "PostCommentPermalink",
                new { areaName = post.Area.Name, slug = post.Slug, comment = comment.GetSlug() }
                );
        }

        public static string GetPendingUrl(this Comment comment, Post post, RequestContext context, RouteCollection routes)
        {
            //todo: (nheskew) really want PostCommentForm w/ a query string inserted but that's not going to happen in the near term so hacking together the URL
            return string.Format("{0}#comment",
                routes.GetUrl(
                    context,
                    "Post",
                    new { areaName = post.Area.Name, slug = post.Slug, pending = bool.TrueString }
                    )
                );
        }

        //todo: (nheskew) need something a little more meaningful than just a timestamp. "comment-200901261012536" would be better but should 
        // be localized (text and format) - something that cannot be currently done w/out controller context :-/
        public static string GetSlug(this Comment comment)
        {
            return comment.Created != null 
                ? string.Format("c-{0}", ((DateTime)comment.Created).ToString("yyyyMMddhhmmssf"))
                : string.Empty;
        }
    }
}
