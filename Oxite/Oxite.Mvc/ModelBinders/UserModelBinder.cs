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

namespace Oxite.Mvc.ModelBinders
{
    public class UserModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection form = controllerContext.HttpContext.Request.Form;
            User user = null;

            Guid siteID = Guid.Empty;
            if (!string.IsNullOrEmpty(form["siteID"]))
            {
                form["siteID"].GuidTryParse(out siteID);
            }

            if (siteID == Guid.Empty)
            {
                user = new User
                {
                    Name = form["userName"],
                    Email = form["userEmail"],
                    DisplayName = form["userDisplayName"],
                    Password = form["userPassword"]
                };

                Guid userID;
                if (!string.IsNullOrEmpty(form["userID"]) && form["userID"].GuidTryParse(out userID))
                {
                    user.ID = userID;
                }
            }

            return user;
        }
    }
}
