//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Reflection;
using System.Web.Mvc;
using Xunit;
using Oxite.Mvc.ActionFilters;
using Oxite.Mvc.Tests.Fakes;

namespace Oxite.Mvc.Tests.Filters
{
    public class ControllerActionCriteriaTests
    {
        private ActionFilterRegistryContext GetContext(MethodInfo actionMethod)
        {
            return new ActionFilterRegistryContext(new ControllerContext(), new ReflectedActionDescriptor(actionMethod, actionMethod.Name, new ReflectedControllerDescriptor(actionMethod.DeclaringType)));
        }

        [Fact]
        public void MatchMatchesMethod()
        {
            ControllerActionCriteria criteria = new ControllerActionCriteria();

            criteria.AddMethod<FakeController>(c => c.Action());

            MethodInfo actionMethod = typeof(FakeController).GetMethod("Action");

            Assert.True(criteria.Match(this.GetContext(actionMethod)));
        }

        [Fact]
        public void AddThrowsInvalidOperationExceptionOnNonMethodCall()
        {
            ControllerActionCriteria criteria = new ControllerActionCriteria();

            Assert.Throws(typeof(InvalidOperationException), () => criteria.AddMethod<DateTime>(d => d.Year));
        }

        [Fact]
        public void MatchMatchesMultipleMethods()
        {
            ControllerActionCriteria criteria = new ControllerActionCriteria();

            criteria.AddMethod<FakeController>(c => c.Action());
            criteria.AddMethod<FakeController>(c => c.ActionWithParameters(null));

            MethodInfo actionMethod = typeof(FakeController).GetMethod("Action");
            MethodInfo actionMethodWithParameters = typeof(FakeController).GetMethod("ActionWithParameters");

            Assert.True(criteria.Match(this.GetContext(actionMethod)));
            Assert.True(criteria.Match(this.GetContext(actionMethodWithParameters)));

        }

        [Fact]
        public void MatchWorksOnNonReflectedActionDescriptor()
        {
            ControllerActionCriteria criteria = new ControllerActionCriteria();

            criteria.AddMethod<FakeController>(c => c.RealNameIsAliasedAction());

            MethodInfo actionMethod = typeof(FakeController).GetMethod("RealNameIsAliasedAction");

            ActionFilterRegistryContext context = new ActionFilterRegistryContext(new ControllerContext(), new FakeActionDescriptor() { Name = "AliasedAction" });

            Assert.True(criteria.Match(context));
        }
    }
}
