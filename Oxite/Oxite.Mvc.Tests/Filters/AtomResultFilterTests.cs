using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Oxite.Mvc.ActionFilters;
using System.Web.Mvc;
using Oxite.Mvc.Results;

namespace Oxite.Mvc.Tests.Filters
{
    public class AtomResultFilterTests
    {
        [Fact]
        public void OnActionExecutedReplacesWithAtomResult()
        {
            AtomResultActionFilter filter = new AtomResultActionFilter();

            ActionExecutedContext context = new ActionExecutedContext()
            {
                Result = new ViewResult()
            };

            filter.OnActionExecuted(context);

            FeedResult result = context.Result as FeedResult;

            Assert.NotNull(result);
            Assert.Equal("Atom", result.ViewName);
            Assert.False(result.IsClientCached);
        }
    }
}
