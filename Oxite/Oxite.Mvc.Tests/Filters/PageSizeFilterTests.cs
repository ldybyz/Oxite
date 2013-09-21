//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;
using Oxite.Mvc.ActionFilters;
using Oxite.Mvc.Tests.Fakes;

namespace Oxite.Mvc.Tests.Filters
{
    public class PageSizeFilterTests
    {
        [Fact]
        public void OnActionExecutingSetsPageSizeTo10ForNoDataFormat()
        {
            PageSizeActionFilter filter = new PageSizeActionFilter();

            Dictionary<string, object> parameters = new Dictionary<string,object>();

            parameters.Add("pageSize", null);

            ActionExecutingContext context = new ActionExecutingContext(new ControllerContext(), new FakeActionDescriptor(), parameters);

            filter.OnActionExecuting(context);

            Assert.Equal(10, parameters["pageSize"]);
        }

        [Fact]
        public void OnActionExecutingDoesNotSetSizeWhenNotInDictionary()
        {
            PageSizeActionFilter filter = new PageSizeActionFilter();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            ActionExecutingContext context = new ActionExecutingContext(new ControllerContext(), new FakeActionDescriptor(), parameters);

            filter.OnActionExecuting(context);

            Assert.False(parameters.ContainsKey("pageSize"));
        }

        [Fact]
        public void OnActionExecutingSetsPageSizeTo50ForRSS()
        {
            PageSizeActionFilter filter = new PageSizeActionFilter();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("pageSize", null);

            ActionExecutingContext context = new ActionExecutingContext(new ControllerContext(), new FakeActionDescriptor(), parameters);
            context.RouteData.Values.Add("dataFormat", "RSS");

            filter.OnActionExecuting(context);

            Assert.Equal(50, parameters["pageSize"]);
        }

        [Fact]
        public void OnActionExecutingSetsPageSizeTo50ForAtom()
        {
            PageSizeActionFilter filter = new PageSizeActionFilter();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("pageSize", null);

            ActionExecutingContext context = new ActionExecutingContext(new ControllerContext(), new FakeActionDescriptor(), parameters);
            context.RouteData.Values.Add("dataFormat", "Atom");

            filter.OnActionExecuting(context);

            Assert.Equal(50, parameters["pageSize"]);
        }
    }
}
