//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.Controllers
{
    public class TrackbackController : Controller
    {
        private readonly IPostService postService;

        public TrackbackController(IPostService postService)
        {
            this.postService = postService;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual TrackbackViewModel Add(Guid postID, FormCollection form)
        {
            Post post = postService.GetPost(postID);

            if (post == null)
            {
                return new TrackbackViewModel { ErrorCode = 0, ErrorMessage = "ID is invalid or missing" };
            }

            string incomingUrl = getParameter(form, "url");
            string incomingTitle = getParameter(form, "title");
            string incomingBlogName = getParameter(form, "blog_name");
            string incomingExcerpt = getParameter(form, "excerpt");

            if (string.IsNullOrEmpty(incomingUrl))
                return new TrackbackViewModel { ErrorCode = 1, ErrorMessage = "no url parameter found, please try harder!" };

            Trackback trackback = post.Trackbacks.Where(tb => string.Equals(tb.Url, incomingUrl, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            try
            {
                if (trackback == null)
                {
                    trackback = new Trackback
                    {
                        Title = incomingTitle,
                        Body = incomingExcerpt,
                        Url = incomingUrl,
                        BlogName = incomingBlogName,
                        Source = "",
                        Created = DateTime.Now.ToUniversalTime()
                    };

                    postService.AddTrackback(post, trackback);
                }
                else
                {
                    trackback.Title = incomingTitle;
                    trackback.Body = incomingExcerpt;
                    trackback.BlogName = incomingBlogName;
                    trackback.IsTargetInSource = null;

                    postService.EditTrackback(trackback);
                }

                return new TrackbackViewModel();
            }
            catch
            {
                return new TrackbackViewModel { ErrorCode = 2, ErrorMessage = "Failed to save Trackback." };
            }
        }

        private static string getParameter(NameValueCollection values, string parameterName)
        {
            if (values[parameterName] != null)
            {
                return HttpUtility.HtmlEncode(values[parameterName]);
            }

            return "";
        }
    }
}