//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Infrastructure;
using Oxite.Services;
using Oxite.Validation;

namespace Oxite.Model
{
    public abstract class ValidatorBase<T> : IValidator<T>
    {
        private readonly ILocalizationService localizationService;
        private readonly Site site;
        private ICollection<Phrase> phrases;

        protected ValidatorBase(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
        {
            this.localizationService = localizationService;
            this.site = site;
            Expressions = expressions;
        }

        protected IRegularExpressions Expressions { get; private set; }

        #region IValidator<T> Members

        public abstract ValidationState Validate(T entity);

        #endregion

        protected ValidationError CreateValidationError(object value, string validationKey, string validationMessage, params object[] validationMessageParameters)
        {
            if (validationMessageParameters != null && validationMessageParameters.Length > 0)
            {
                validationMessage = string.Format(validationMessage, validationMessageParameters);
            }

            return new ValidationError(
                validationKey,
                value,
                new InvalidOperationException(localize(validationKey, validationMessage))
                );
        }

        private string localize(string key, string defaultValue)
        {
            if (phrases == null)
                phrases = localizationService.GetTranslations();

            Phrase foundPhrase = phrases.Where(p => p.Key == key && p.Language == site.LanguageDefault).FirstOrDefault();

            if (foundPhrase != null)
                return foundPhrase.Value;

            return defaultValue;
        }
    }
}
