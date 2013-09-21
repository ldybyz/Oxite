//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using Oxite.Infrastructure;
using Oxite.Services;
using Oxite.Validation;

namespace Oxite.Model
{
    public class PageValidator : ValidatorBase<Page>
    {
        public PageValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions) { }

        #region IValidator Members

        public override ValidationState Validate(Page page)
        {
            if (page == null) throw new ArgumentNullException("page");

            ValidationState validationState = new ValidationState();

            if (page.Creator == null)
            {
                validationState.Errors.Add(CreateValidationError(page.Creator, "Creator.RequiredError", "Creator is not set."));
            }

            page.Title = page.Title.Trim();
            if (string.IsNullOrEmpty(page.Title))
            {
                validationState.Errors.Add(CreateValidationError(page.Title, "Title.RequiredError", "Title is not set."));
            }
            else
            {
                if (page.Title.Length > 250)
                {
                    validationState.Errors.Add(CreateValidationError(page.Title, "Title.MaxLengthExceededError", "Title must be {0} characters or less.", 250));
                }
            }

            page.Body = page.Body.Trim();
            if (string.IsNullOrEmpty(page.Body))
            {
                validationState.Errors.Add(CreateValidationError(page.Body, "Body.RequiredError", "Body is not set."));
            }

            page.Slug = page.Slug.Trim();
            if (string.IsNullOrEmpty(page.Slug))
            {
                validationState.Errors.Add(CreateValidationError(page.Slug, "Slug.RequiredError", "Slug is not set."));
            }
            else
            {
                if (!Expressions.IsMatch("IsSlug", page.Slug))
                {
                    validationState.Errors.Add(CreateValidationError(page.Slug, "Slug.NotValidError", "Slug is not valid."));
                }
            }

            return validationState;
        }

        #endregion
    }
}
