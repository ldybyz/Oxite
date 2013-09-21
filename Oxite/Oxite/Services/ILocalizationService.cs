//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using Oxite.Model;

namespace Oxite.Services
{
    public interface ILocalizationService
    {
        ICollection<Phrase> GetTranslations();
        ICollection<Phrase> GetTranslations(string languageCode);
    }
}
