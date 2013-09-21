//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Model
{
    public class Comment : EntityBase
    {
        public UserBase Creator { get; set; }
        public long CreatorIP { get; set; }
        public string CreatorUserAgent { get; set; }
        public Language Language { get; set; }
        public string Body { get; set; }
        public EntityState State { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
