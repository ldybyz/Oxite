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
    public class UserValidator : ValidatorBase<User>
    {
        public UserValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions) { }

        #region IValidator Members

        public override ValidationState Validate(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            ValidationState validationState = new ValidationState();

            if (string.IsNullOrEmpty(user.Name))
            {
                validationState.Errors.Add(CreateValidationError(user.Name, "Name.RequiredError", "Name is not set"));
            }
            else
            {
                if (user.Name.Length > 256)
                {
                    validationState.Errors.Add(CreateValidationError(user.Name, "Name.MaxLengthExceededError", "Username must be less than or equal to {0} characters long.", 256));
                }
            }

            if (string.IsNullOrEmpty(user.DisplayName))
            {
                validationState.Errors.Add(CreateValidationError(user.DisplayName, "DisplayName.RequiredError", "DisplayName is not set"));
            }
            else
            {
                if (user.DisplayName.Length > 256)
                {
                    validationState.Errors.Add(CreateValidationError(user.DisplayName, "DisplayName.MaxLengthExceededError", "DisplayName must be less than or equal to {0} characters long.", 256));
                }
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                if (user.Email.Length > 256)
                {
                    validationState.Errors.Add(CreateValidationError(user.Email, "Email.MaxLengthExceededError", "Email must be less than or equal to {0} characters long.", 256));
                }
                else if (!Expressions.IsMatch("IsEmail", user.Email))
                {
                    validationState.Errors.Add(CreateValidationError(user.Email, "Email.InvalidError", "Email is invalid."));
                }
            }

            return validationState;
        }

        #endregion
    }
}
