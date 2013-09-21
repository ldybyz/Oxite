//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model
{
    public enum EntityState : byte
    {
        NotSet = 0,
        Normal = 1,
        PendingApproval = 2,
        Removed = 3
    }
}