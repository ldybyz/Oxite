//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using Oxite.Model;

namespace Oxite.Mvc.ViewModels
{
    public class ArchiveViewModel
    {
        public ArchiveViewModel(IList<KeyValuePair<ArchiveData, int>> archives, INamedEntity container)
        {
            Archives = archives;
            Container = container;
        }

        public IList<KeyValuePair<ArchiveData,int>> Archives { get; private set; }
        public INamedEntity Container { get; private set; }
    }
}
