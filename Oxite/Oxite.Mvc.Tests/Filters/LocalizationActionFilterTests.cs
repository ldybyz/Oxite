using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Oxite.Mvc.Tests.Fakes;
using Oxite.Mvc.ActionFilters;
using System.Web.Mvc;
using Oxite.Mvc.ViewModels;
using Oxite.Model;

namespace Oxite.Mvc.Tests.Filters
{
    public class LocalizationActionFilterTests
    {
        [Fact]
        public void OnActionExecutedAddsAllPhrasesToModel()
        {
            FakeLocalizationService locService = new FakeLocalizationService();

            OxiteModel model = new OxiteModel();

            ActionExecutedContext context = new ActionExecutedContext()
            {
                Result = new ViewResult() { ViewData = new ViewDataDictionary(model) }
            };

            LocalizationActionFilter filter = new LocalizationActionFilter(locService);

            filter.OnActionExecuted(context);

            Assert.NotNull(model.GetModelItem<ICollection<Phrase>>());
            Assert.Same(locService.Phrases, model.GetModelItem<ICollection<Phrase>>());
        }
    }
}
