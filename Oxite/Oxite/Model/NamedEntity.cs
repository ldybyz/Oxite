//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Runtime.Serialization;

namespace Oxite.Model
{
    [DataContract]
    public class NamedEntity : EntityBase, INamedEntity
    {
        [DataMember]
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
