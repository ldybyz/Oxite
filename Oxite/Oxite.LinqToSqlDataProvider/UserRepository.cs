//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class UserRepository : IUserRepository
    {
        private OxiteLinqToSqlDataContext context;
        private Site site;

        public UserRepository(OxiteLinqToSqlDataContext context, Site site)
        {
            this.context = context;
            this.site = site;
        }

        #region IUserRepository Members

        public User GetUser(string name)
        {
            return (from u in context.oxite_Users
                    where string.Compare(u.Username, name, true) == 0
                    select new User()
                    {
                        ID = u.UserID,
                        Name = u.Username,
                        DisplayName = u.DisplayName,
                        Email = u.Email,
                        HashedEmail = u.HashedEmail,
                        Password = u.Password,
                        PasswordSalt = u.PasswordSalt,
                        Status = u.Status
                    }).FirstOrDefault();
        }

        public void Save(User user)
        {
            oxite_User dbUser = null;
            Guid userID = user.ID;

            if (userID != Guid.Empty)
            {
                dbUser = (from u in context.oxite_Users where u.UserID == userID select u).FirstOrDefault();
            }

            if (dbUser == null)
            {
                if (userID == Guid.Empty)
                {
                    userID = Guid.NewGuid();
                }

                dbUser = new oxite_User();

                dbUser.UserID = userID;

                context.oxite_Users.InsertOnSubmit(dbUser);
            }

            dbUser.DefaultLanguageID = (from l in context.oxite_Languages where l.LanguageName == (site.LanguageDefault ?? "en") select l).First().LanguageID;
            dbUser.Username = user.Name;
            dbUser.DisplayName = user.DisplayName;
            dbUser.Email = user.Email;
            dbUser.HashedEmail = user.HashedEmail;
            dbUser.Password = user.Password;
            dbUser.PasswordSalt = user.PasswordSalt;
            dbUser.Status = user.Status;

            context.SubmitChanges();

            user.ID = userID;
        }

        #endregion
    }
}
