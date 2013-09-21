//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Runtime.Serialization;

namespace Oxite.Model
{
    [DataContract]
    public class UserBase : NamedEntity
    {
        [DataMember]
        public string Email { get; set; }
        public string HashedEmail { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}
