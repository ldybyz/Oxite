using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Oxite.Mvc.ActionFilters;
using System.Web.Mvc;
using Oxite.Mvc.Tests.Fakes;
using System.Web.Routing;
using Oxite.Mvc.Results;
using Oxite.Mvc.ViewModels;
using Oxite.Model;

namespace Oxite.Mvc.Tests.Filters
{
    public class RssResultFilterTests
    {
        [Fact]
        public void OnExecutedReplacesResultWithRss()
        {
            RssResultActionFilter filter = new RssResultActionFilter();

            ActionExecutedContext context = new ActionExecutedContext()
                {
                    Result = new ViewResult(),
                    Controller = new FakeController() { ViewData = new ViewDataDictionary(new OxiteModelList<Post>() { List = new List<Post>(new[] { new Post() }) }) }
                };

            filter.OnActionExecuted(context);

            FeedResult result = context.Result as FeedResult;

            Assert.NotNull(result);
            Assert.True(string.Equals("Rss", result.ViewName, StringComparison.OrdinalIgnoreCase));
            Assert.False(result.IsClientCached);
        }

        [Fact]
        public void OnExecutedSetsClientCachedToTrueForEmptyList()
        {
            RssResultActionFilter filter = new RssResultActionFilter();

            ActionExecutedContext context = new ActionExecutedContext()
            {
                Result = new ViewResult(),
                Controller = new FakeController() { ViewData = new ViewDataDictionary(new OxiteModelList<Post>() { List = Enumerable.Empty<Post>().ToList() }) }
            };

            filter.OnActionExecuted(context);

            FeedResult result = context.Result as FeedResult;

            Assert.NotNull(result);
            Assert.True(result.IsClientCached);
        }
    }
}
