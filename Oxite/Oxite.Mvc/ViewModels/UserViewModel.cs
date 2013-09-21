//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Model;

namespace Oxite.Mvc.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(UserBase user)
        {
            Name = user.Name;
            DisplayName = user.DisplayName;
            Email = user.Email;
            HashedEmail = user.HashedEmail;
            Url = user.Url;
            CanAccessAdmin = user is User; //TODO: (erikpo) Fill in this property with the real value
            IsAuthenticated = user is User;
        }

        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Email { get; set; }
        public string HashedEmail { get; set; }
        public string Url { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool CanAccessAdmin { get; set; }
    }
}
