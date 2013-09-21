//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Mvc.ViewModels
{
    public class OxiteModelPartial<T>
    {
        public OxiteModelPartial(OxiteModel rootModel, T partialModel)
        {
            RootModel = rootModel;
            PartialModel = partialModel;
        }

        public OxiteModel RootModel { get; set; }
        public T PartialModel { get; set; }
    }
}
