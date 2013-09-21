//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Model;

namespace Oxite.Mvc.ModelBinders
{
    public class AreaModelBinder : IModelBinder
    {
        //TODO: (erikpo) This should be split out into two different model binders
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            Area area = new Area
            {
                Name = controllerContext.RouteData.Values["areaName"] as string
            };

            string areaIDValue = controllerContext.RouteData.Values["areaID"] as string;
            if (!string.IsNullOrEmpty(controllerContext.HttpContext.Request.Form["areaID"]))
                areaIDValue = controllerContext.HttpContext.Request.Form["areaID"];
            if (!string.IsNullOrEmpty(areaIDValue))
            {
                Guid areaID;
                if (areaIDValue.GuidTryParse(out areaID))
                    area.ID = areaID;
            }

            if (!string.IsNullOrEmpty(request.Form.Get("areaName")))
                area.Name = request.Form.Get("areaName");

            if (!string.IsNullOrEmpty(request.Form.Get("areaDisplayName")))
                area.DisplayName = request.Form.Get("areaDisplayName");

            if (!string.IsNullOrEmpty(request.Form.Get("areaDescription")))
                area.Description = request.Form.Get("areaDescription");

            if (!string.IsNullOrEmpty(request.Form["areaCommentingDisabled"]))
            {
                bool areaCommentingDisabled;
                if (bool.TryParse(request.Form.GetValues("areaCommentingDisabled")[0], out areaCommentingDisabled))
                    area.CommentingDisabled = areaCommentingDisabled;
            }
            
            return area;
        }
    }
}
