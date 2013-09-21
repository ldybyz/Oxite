//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model
{
    public class ParentAndChild<TParent, TChild>
    {
        public TParent Parent { get; set; }
        public TChild Child { get; set; }
    }
}
