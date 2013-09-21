//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Model
{
    public class ResourceString
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Language Language { get; set; }
        public short Version { get; set; }
        public User Creator { get; set; }
        public DateTime? Created { get; set; }
        public EntityState State { get; set; }
    }
}
