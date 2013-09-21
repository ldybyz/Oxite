//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Model
{
    public class Trackback : EntityBase
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string BlogName { get; set; }
        public string Source { get; set; }
        public bool? IsTargetInSource { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
