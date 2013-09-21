//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using Oxite.Model;

namespace Oxite.Mvc.ViewModels
{
    public class AreaListViewModel
    {
        public AreaListViewModel(IList<Area> areas)
        {
            Areas = areas;
        }

        public IList<Area> Areas { get; private set; }
    }
}
