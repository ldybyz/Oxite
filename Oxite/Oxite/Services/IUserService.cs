//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Model;
using Oxite.Validation;

namespace Oxite.Services
{
    public interface IUserService
    {
        User GetUser(string name);
        User GetUser(string name, string password);
        void AddUser(User user, out ValidationStateDictionary validationState, out User newUser);
        void EditUser(User user, out ValidationStateDictionary validationState);
        void EnsureAnonymousUser(Language languageDefault);
        bool VerifyAccess(string name, Area area);
        bool VerifyAccess(string name, Page page);
    }
}
