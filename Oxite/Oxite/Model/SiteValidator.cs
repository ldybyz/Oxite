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
    public class SiteValidator : ValidatorBase<Site>
    {
        public SiteValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions) { }

        #region IValidator Members

        public override ValidationState Validate(Site site)
        {
            if (site == null) throw new ArgumentNullException("site");

            ValidationState validationState = new ValidationState();

            if (string.IsNullOrEmpty(site.CommentStateDefault))
            {
                validationState.Errors.Add(CreateValidationError(site.CommentStateDefault, "CommentStateDefault.RequiredError", "CommentStateDefault is not set."));
            }
            else
            {
                if (!(site.CommentStateDefault == EntityState.Normal.ToString() || site.CommentStateDefault == EntityState.PendingApproval.ToString()))
                {
                    validationState.Errors.Add(CreateValidationError(site.CommentStateDefault, "CommentStateDefault.InvalidError", "Invalid value specified for CommentStateDefault."));
                }
            }

            if (string.IsNullOrEmpty(site.CssPath))
            {
                validationState.Errors.Add(CreateValidationError(site.CssPath, "CssPath.RequiredError", "CssPath is not set."));
            }
            else
            {
                if (!site.CssPath.Contains("{0}"))
                {
                    validationState.Errors.Add(CreateValidationError(site.CssPath, "CssPath.FormatError", "CssPath must contain '{0}'."));
                }
            }

            if (string.IsNullOrEmpty(site.DisplayName))
            {
                validationState.Errors.Add(CreateValidationError(site.DisplayName, "DisplayName.RequiredError", "DisplayName is not set."));
            }

            if (site.Host == null)
            {
                validationState.Errors.Add(CreateValidationError(site.Host, "Host.RequiredError", "Host is not set."));
            }

            if (string.IsNullOrEmpty(site.LanguageDefault))
            {
                validationState.Errors.Add(CreateValidationError(site.LanguageDefault, "LanguageDefault.RequiredError", "LanguageDefault is not set."));
            }

            if (string.IsNullOrEmpty(site.Name))
            {
                validationState.Errors.Add(CreateValidationError(site.Name, "Name.RequiredError", "Name is not set."));
            }

            if (string.IsNullOrEmpty(site.PageTitleSeparator))
            {
                validationState.Errors.Add(CreateValidationError(site.PageTitleSeparator, "PageTitleSeparator.RequiredError", "PageTitleSeparator is not set."));
            }

            if (string.IsNullOrEmpty(site.ScriptsPath))
            {
                validationState.Errors.Add(CreateValidationError(site.ScriptsPath, "ScriptsPath.RequiredError", "ScriptsPath is not set."));
            }
            else
            {
                if (!site.ScriptsPath.Contains("{0}"))
                {
                    validationState.Errors.Add(CreateValidationError(site.ScriptsPath, "ScriptsPath.FormatError", "ScriptsPath must contain '{0}'."));
                }
            }

            if (string.IsNullOrEmpty(site.SkinDefault))
            {
                validationState.Errors.Add(CreateValidationError(site.SkinDefault, "SkinDefault.RequiredError", "SkinDefault is not set."));
            }

            return validationState;
        }

        #endregion
    }
}
