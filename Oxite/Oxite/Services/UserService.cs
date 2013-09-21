//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Security.Cryptography;
using System.Text;
using Oxite.Extensions;
using Oxite.Model;
using Oxite.Repositories;
using Oxite.Validation;

namespace Oxite.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IValidationService validator;

        public UserService(IUserRepository repository, IValidationService validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        #region IUserService Members

        public User GetUser(string name)
        {
            return repository.GetUser(name);
        }

        public User GetUser(string name, string password)
        {
            User user = string.Compare(name, "Anonymous", true) != 0 ? repository.GetUser(name) : null;

            if (user != null && user.Password == saltAndHash(password, user.PasswordSalt))
                return user;

            return null;
        }

        public void AddUser(User user, out ValidationStateDictionary validationState, out User newUser)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(User), validator.Validate(user));

            if (!validationState.IsValid)
            {
                newUser = null;

                return;
            }

            user.PasswordSalt = Guid.NewGuid().ToString("N");
            user.Password = saltAndHash(user.Password, user.PasswordSalt);
            user.HashedEmail = user.Email.ComputeHash();

            repository.Save(user);

            newUser = repository.GetUser(user.Name);
        }

        public void EditUser(User user, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(User), validator.Validate(user));

            if (!validationState.IsValid)
            {
                return;
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                user.PasswordSalt = Guid.NewGuid().ToString("N");
                user.Password = saltAndHash(user.Password, user.PasswordSalt);
            }
            user.HashedEmail = user.Email.ComputeHash();

            repository.Save(user);
        }

        public void EnsureAnonymousUser(Language languageDefault)
        {
            User user = GetUser("Anonymous");

            if (user == null)
            {
                user = new User
                {
                    Name = "Anonymous",
                    DisplayName = "",
                    Email = "",
                    HashedEmail = "",
                    Password = "",
                    PasswordSalt = "",
                    Status = 1,
                    LanguageDefault = languageDefault
                };

                repository.Save(user);
            }
        }

        public bool VerifyAccess(string name, Area area)
        {
            throw new NotImplementedException();
        }

        public bool VerifyAccess(string name, Page page)
        {
            throw new NotImplementedException();
        }

        #endregion

        private static string saltAndHash(string rawString, string salt)
        {
            byte[] salted = Encoding.UTF8.GetBytes(string.Concat(rawString, salt));

            SHA256 hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(salted);

            return Convert.ToBase64String(hashed);
        }
    }
}
