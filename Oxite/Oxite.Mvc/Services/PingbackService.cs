//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using Oxite.Routing;
using Oxite.Services;

namespace Oxite.Mvc.Services
{
    public class PingbackService : IPingbackServer
    {
        private readonly IPostService postService;
        private readonly AbsolutePathHelper pathHelper;

        public PingbackService(IPostService postService, AbsolutePathHelper pathHelper)
        {
            this.postService = postService;
            this.pathHelper = pathHelper;
        }

        #region IPingbackServer Members

        public string Ping(string sourceUri, string targetUri)
        {
            if (sourceUri == null || targetUri == null)
                throw new ArgumentNullException();

            Oxite.Model.Post postToQuery = pathHelper.GetPostFromUri(new Uri(targetUri));

            Oxite.Model.Post post = postService.GetPost(postToQuery.Area, postToQuery.Slug);

            if (post == null)
                throw new ArgumentException();

            Oxite.Model.Trackback trackback = post.Trackbacks.Where(tb => string.Equals(tb.Url, sourceUri, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (trackback == null)
            {
                trackback = new Oxite.Model.Trackback()
                {
                    Url = sourceUri,
                    Created = DateTime.Now.ToUniversalTime(),
                    Title = string.Empty,
                    BlogName = string.Empty,
                    Body = string.Empty,
                    Source = string.Empty
                };
                postService.AddTrackback(post, trackback);
            }

            return "Success";
        }

        #endregion
    }
}
