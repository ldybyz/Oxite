//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.Extensions;

namespace Oxite.Mvc.Infrastructure
{
    public class PostVisitor : Visitor
    {
        private readonly UrlHelper urlHelper;

        public PostVisitor(UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public string Visit(HomePageContainer container)
        {
            return urlHelper.Posts();
        }

        public string Visit(SearchPageContainer container)
        {
            return "Search";
        }

        public string Visit(Post post)
        {
            return urlHelper.Post(post);
        }

        public string Visit(Page page)
        {
            throw new System.NotImplementedException();
        }

        public string Visit(Area area)
        {
            return urlHelper.Posts(area);
        }

        public string Visit(Area area, string dataFormat)
        {
            return urlHelper.Posts(area, dataFormat);
        }

        public string Visit(Tag tag)
        {
            return urlHelper.Posts(tag);
        }

        public string Visit(Tag tag, string dataFormat)
        {
            return urlHelper.Posts(tag, dataFormat);
        }
    }
}
