//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.LinqToSqlDataProvider;

namespace Oxite.Mvc.Tests.Repository
{
    public class LinqToSqlFixture
    {
        private string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Oxite.Database;Integrated Security=true";

        public OxiteLinqToSqlDataContext Context 
        { 
            get
            {
                return new OxiteLinqToSqlDataContext(ConnectionString);
            } 
        }
    }
}
