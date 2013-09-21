//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.Infrastructure;

namespace Oxite.Mvc.ActionFilters
{
    public class AntiForgeryAuthorizationFilter : IAuthorizationFilter
    {
        private readonly Site site;

        public AntiForgeryAuthorizationFilter(Site site)
        {
            this.site = site;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!(filterContext.RouteData.Values["validateAntiForgeryToken"] is bool
                && (bool)filterContext.RouteData.Values["validateAntiForgeryToken"]
                && filterContext.HttpContext.Request.HttpMethod == "POST"
                && filterContext.RequestContext.HttpContext.Request.IsAuthenticated))
            {
                return;
            }

            string salt = site.ID.ToString();

            AntiForgeryToken antiForgeryToken = new AntiForgeryToken(salt);

            long ticks;

            string formTicks = filterContext.HttpContext.Request.Cookies.Get(AntiForgeryToken.TicksName) != null
                ? filterContext.HttpContext.Request.Cookies.Get(AntiForgeryToken.TicksName).Value
                : null;

            string formHash = filterContext.HttpContext.Request.Form[AntiForgeryToken.TokenName];

            if (string.IsNullOrEmpty(formTicks) || !long.TryParse(formTicks, out ticks) || string.IsNullOrEmpty(formHash))
            {
                throw new HttpAntiForgeryException("Bad Anti-Forgery Token");
            }

            TimeSpan timeOffset = new TimeSpan(antiForgeryToken.Ticks - ticks);

            //todo: (nheskew)drop the time span into some configurable property
            // and handle the "exception" better than just throwing one. ideally we should give the form back, populated with the same data, with a 
            // message saying something like "fall asleep at your desk? writing a novel? try that submission again!"
            if (!(AntiForgeryToken.GetHash(salt, ticks.ToString()) == formHash && timeOffset.TotalMinutes < 60))
            {
                throw new HttpAntiForgeryException("Bad Anti-Forgery Token");
            }
        }
    }
}
