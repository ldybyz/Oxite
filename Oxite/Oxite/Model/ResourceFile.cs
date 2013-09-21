//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Model
{
    public class ResourceFile : NamedEntity
    {
        public User Creator { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public EntityState State { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
