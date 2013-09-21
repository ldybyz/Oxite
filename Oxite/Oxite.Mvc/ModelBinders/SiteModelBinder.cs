//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Model;
using Oxite.Mvc.Extensions;

namespace Oxite.Mvc.ModelBinders
{
    public class SiteModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection form = controllerContext.HttpContext.Request.Form;

            Site site = new Site
            {
                Name = form["siteName"],
                Host = new Uri(form["siteHost"]),
                DisplayName = form["siteDisplayName"],
                Description = form["siteDescription"],
                FavIconUrl = form["favIconUrl"],
                SkinDefault = form["skinDefault"],
                LanguageDefault = form["languageDefault"],
                PageTitleSeparator = form["pageTitleSeparator"],
                GravatarDefault = form["gravatarDefault"],
                ScriptsPath = form["scriptsPath"],
                CssPath = form["cssPath"],
                RouteUrlPrefix = form["routeUrlPrefix"]
            };

            if (form["siteID"] != null)
            {
                Guid siteID;
                if (form["siteID"].GuidTryParse(out siteID))
                {
                    site.ID = siteID;
                }
            }

            bool hasMultipleAreas;
            if (bool.TryParse(form["hasMultipleAreas"], out hasMultipleAreas))
            {
                site.HasMultipleAreas = hasMultipleAreas;
            }

            string hostRedirects = form["siteHostRedirects"];
            if (!string.IsNullOrEmpty(hostRedirects))
            {
                string[] hostRedirectList = hostRedirects.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (hostRedirectList.Length > 0)
                {
                    site.HostRedirects = new Uri[hostRedirectList.Length];

                    for (int i = 0; i < hostRedirectList.Length; i++)
                    {
                        site.HostRedirects[i] = new Uri(hostRedirectList[i]);
                    }
                }
            }

            double timezoneOffset;
            if (double.TryParse(form["timezoneOffset"], out timezoneOffset))
            {
                site.TimeZoneOffset = timezoneOffset;
            }

            site.IncludeOpenSearch = form.IsTrue("includeOpenSearch");

            short postEditTimeout;
            if (short.TryParse(form["postEditTimeout"], out postEditTimeout))
            {
                site.PostEditTimeout = postEditTimeout;
            }

            site.CommentStateDefault = form.IsTrue("commentStatePendingByDefault")
                ? EntityState.PendingApproval.ToString()
                : EntityState.Normal.ToString();

            site.AuthorAutoSubscribe = form.IsTrue("authorAutoSubscribe");
            site.CommentingDisabled = form.IsTrue("commentingDisabled");

            return site;
        }
    }
}
