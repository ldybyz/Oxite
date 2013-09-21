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
    public class UserBaseValidator : ValidatorBase<UserBase>
    {
        public UserBaseValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions) { }

        #region IValidator Members

        public override ValidationState Validate(UserBase userBase)
        {
            if (userBase == null) throw new ArgumentNullException("userBase");

            ValidationState validationState = new ValidationState();

            if (string.IsNullOrEmpty(userBase.Name))
            {
                validationState.Errors.Add(CreateValidationError(userBase.Name, "Name.RequiredError", "Name is required."));
            }

            if (!string.IsNullOrEmpty(userBase.Email))
            {
                if (!Expressions.IsMatch("IsEmail", userBase.Email))
                {
                    validationState.Errors.Add(CreateValidationError(userBase.Email, "Email.InvalidError", "Email is invalid."));
                }
            }

            if (!string.IsNullOrEmpty(userBase.Url))
            {
                if (!Expressions.IsMatch("IsUrl", userBase.Url))
                {
                    validationState.Errors.Add(CreateValidationError(userBase.Url, "Url.InvalidError", "Url is invalid."));
                }
            }

            return validationState;
        }

        #endregion
    }
}
