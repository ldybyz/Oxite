//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Model;

namespace Oxite.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string name);
        void Save(User user);
    }
}
