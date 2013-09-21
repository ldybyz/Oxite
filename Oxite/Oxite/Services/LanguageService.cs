//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository repository;

        public LanguageService(ILanguageRepository repository)
        {
            this.repository = repository;
        }

        public Language GetLanguage(string name)
        {
            return repository.GetLanguage(name);
        }

        public void AddLanguage(Language language)
        {
            repository.Save(language);
        }
    }
}
