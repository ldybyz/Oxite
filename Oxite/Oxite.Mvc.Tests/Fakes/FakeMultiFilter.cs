//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeMultiFilter : IActionFilter, IResultFilter, IExceptionFilter, IAuthorizationFilter
    {
        #region IResultFilter Members

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
