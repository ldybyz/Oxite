//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Model;

namespace Oxite.Mvc.ModelBinders
{
    public class SearchCriteriaModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            SearchCriteria criteria = new SearchCriteria
            {
                Term = controllerContext.HttpContext.Request.QueryString["term"]
            };

            return criteria;
        }
    }
}
