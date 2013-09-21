//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using System.Web.Routing;
using Oxite.Model;
using Oxite.Model.Extensions;

namespace Oxite.Routing
{
    public class AbsolutePathHelper
    {
        private readonly Site site;
        private readonly RouteCollection routes;

        public AbsolutePathHelper(Site site, RouteCollection routes)
        {
            this.site = site;
            this.routes = routes;
        }

        public string GetAbsolutePath(Post post)
        {
            if (post == null) throw new ArgumentNullException("post");
            if (string.IsNullOrEmpty(post.Slug) || post.Area == null || string.IsNullOrEmpty(post.Area.Name)) throw new ArgumentException();

            UriBuilder builder = new UriBuilder(site.Host.Scheme, site.Host.Host, site.Host.Port, site.Host.AbsolutePath);

            builder.Path = post.GetUrl(new RequestContext(new DummyHttpContext(null, site.Host), new RouteData()), routes);

            return builder.Uri.ToString();
        }

        public string GetAbsolutePath(Post post, Comment comment)
        {
            if (post == null) throw new ArgumentNullException("post");
            if (comment == null) throw new ArgumentNullException("comment");
            if (string.IsNullOrEmpty(post.Slug) || post.Area == null || string.IsNullOrEmpty(post.Area.Name)) throw new ArgumentException();

            UriBuilder builder = new UriBuilder(site.Host);

            builder.Path = comment.GetUrl(post, new RequestContext(new DummyHttpContext(null, site.Host), new RouteData()), routes);

            return builder.Uri.ToString();
        }

        public Post GetPostFromUri(Uri permalink)
        {
            if (permalink == null)
                throw new ArgumentNullException();

            if (!permalink.ToString().StartsWith(site.Host.ToString(), StringComparison.OrdinalIgnoreCase))
                return null;

            RouteData data = routes["Post"].GetRouteData(new DummyHttpContext(permalink, site.Host));

            if (data != null)
                return new Post { Slug = data.GetRequiredString("slug"), Area = new Area { Name = data.GetRequiredString("areaName") } };

            return null;
        }

        private class DummyHttpContext : HttpContextBase 
        {
            private readonly Uri requestUrl;
            private readonly Uri hostUrl;
            public DummyHttpContext(Uri requestUrl, Uri hostUrl)
            {
                this.requestUrl = requestUrl;
                this.hostUrl = hostUrl;
            }

            public override HttpRequestBase Request
            {
                get
                {
                    return new DummyHttpRequest(requestUrl, hostUrl);
                }
            }
        }

        private class DummyHttpRequest : HttpRequestBase
        {
            private readonly Uri requestUrl;
            private readonly Uri hostUrl;
            public DummyHttpRequest(Uri requestUrl, Uri hostUrl)
            {
                this.requestUrl = requestUrl;
                this.hostUrl = hostUrl;
            }

            public override Uri Url
            {
                get
                {
                    return requestUrl;
                }
            }

            public override string ApplicationPath
            {
                get
                {
                    return hostUrl.AbsolutePath;
                }
            }

            public override string AppRelativeCurrentExecutionFilePath
            {
                get
                {
                    if (hostUrl.AbsolutePath.Length > 1)
                        return "~" + requestUrl.AbsolutePath.Remove(0, hostUrl.AbsolutePath.Length);

                    return "~" + requestUrl.AbsolutePath;
                }
            }

            public override string PathInfo
            {
                get
                {
                    return "";
                }
            }
        }
    }
}
