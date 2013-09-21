//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class LocalizationRepository : ILocalizationRepository
    {
        private OxiteLinqToSqlDataContext context;

        public LocalizationRepository(OxiteLinqToSqlDataContext context)
        {
            this.context = context;
        }

        #region ILocalizationRepository Members

        public IQueryable<Phrase> GetPhrases()
        {
            return from r in context.oxite_StringResources
                   select new Phrase()
                   {
                       Key = r.StringResourceKey,
                       Value = r.StringResourceValue,
                       Language = r.Language
                   };
        }

        #endregion
    }
}
