//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Runtime.Serialization;

namespace Oxite.Model
{
    [DataContract]
    public class EntityBase
    {
        public Guid ID { get; set; }
    }
}
