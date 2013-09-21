using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Oxite.Mvc.ActionFilters;
using Oxite.Mvc.Tests.Fakes;
using System.Web.Routing;

namespace Oxite.Mvc.Tests.Filters
{
    public class DataFormatCriteriaTests
    {
        [Fact]
        public void MatchMatchesPassedDataFormat()
        {
            DataFormatCriteria criteria = new DataFormatCriteria("RSS");

            RouteData routeData = new RouteData();

            routeData.Values.Add("dataFormat", "RSS");

            ActionFilterRegistryContext context = new ActionFilterRegistryContext(new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), routeData, new FakeController()), new FakeActionDescriptor());

            Assert.True(criteria.Match(context));
        }

        [Fact]
        public void MatchDoesNotMatchWithNoDataFormat()
        {
            DataFormatCriteria criteria = new DataFormatCriteria("RSS");

            RouteData routeData = new RouteData();

            ActionFilterRegistryContext context = new ActionFilterRegistryContext(new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), routeData, new FakeController()), new FakeActionDescriptor());

            Assert.False(criteria.Match(context));
        }
    }
}
