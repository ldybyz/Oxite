//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Xunit;
using Oxite.Mvc.ActionFilters;
using Oxite.Mvc.Tests.Fakes;

namespace Oxite.Mvc.Tests.Filters
{
    public class ActionFilterRegistryTests
    {
        private ActionFilterRegistryContext GetFakeContext()
        {
            return new ActionFilterRegistryContext(new ControllerContext(), new FakeActionDescriptor());
        }

        [Fact]
        public void AddThenGetMatchesOneFilterOfOneType()
        {
            FakeUnityContainer container = new FakeUnityContainer();

            container.Add(new FakeActionFilter());

            ActionFilterRegistry registry = new ActionFilterRegistry(container);

            registry.Add(new[] { new FakeActionFilterCriteria() { IsMatch = true }}, typeof(FakeActionFilter));

            FilterInfo filters = registry.GetFilters(this.GetFakeContext());

            Assert.Equal(1, filters.ActionFilters.Count);
            Assert.IsType<FakeActionFilter>(filters.ActionFilters[0]);
            Assert.Equal(0, filters.AuthorizationFilters.Count);
            Assert.Equal(0, filters.ExceptionFilters.Count);
            Assert.Equal(0, filters.ResultFilters.Count);
        }

        [Fact]
        public void AddThenGetMatchesOneFilterOfMultipleTypes()
        {
            FakeUnityContainer container = new FakeUnityContainer();

            container.Add(new FakeMultiFilter());

            ActionFilterRegistry registry = new ActionFilterRegistry(container);

            registry.Add(new[] { new FakeActionFilterCriteria() { IsMatch = true } }, typeof(FakeMultiFilter));

            FilterInfo filters = registry.GetFilters(this.GetFakeContext());

            Assert.Equal(1, filters.ActionFilters.Count);
            Assert.IsType<FakeMultiFilter>(filters.ActionFilters[0]);
            Assert.Equal(1, filters.AuthorizationFilters.Count);
            Assert.IsType<FakeMultiFilter>(filters.AuthorizationFilters[0]);
            Assert.Equal(1, filters.ExceptionFilters.Count);
            Assert.IsType<FakeMultiFilter>(filters.ExceptionFilters[0]);
            Assert.Equal(1, filters.ResultFilters.Count);
            Assert.IsType<FakeMultiFilter>(filters.ResultFilters[0]);
        }
    }
}
