//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Xunit;
using Oxite.Mvc.ViewModels;

namespace Oxite.Mvc.Tests
{
    public class OxiteModelTests
    {
        [Fact]
        public void AddModelItemThenGetModelItemReturnsItem()
        {
            OxiteModel model = new OxiteModel();

            OxiteModelTests modelItem = new OxiteModelTests();

            model.AddModelItem(modelItem);

            var actualItem = model.GetModelItem<OxiteModelTests>();

            Assert.Same(modelItem, actualItem);
        }

        [Fact]
        public void GetModelReturnsNullIfNotFound()
        {
            OxiteModel model = new OxiteModel();

            Assert.Null(model.GetModelItem<OxiteModelTests>());
        }
    }
}
