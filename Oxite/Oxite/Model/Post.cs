//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;

namespace Oxite.Model
{
    public class Post : PostBase
    {
        public Area Area { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Trackback> Trackbacks { get; set; }
    }
}
