//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Infrastructure
{
    public interface IFormsAuthentication
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);
        void SignOut();
    }
}
