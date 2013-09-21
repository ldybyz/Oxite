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
    public class PostSubscriptionValidator : ValidatorBase<PostSubscription>
    {
        public PostSubscriptionValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions) { }

        #region IValidator Members

        public override ValidationState Validate(PostSubscription postSubscription)
        {
            if (postSubscription == null) throw new ArgumentNullException("postSubscription");

            ValidationState validationState = new ValidationState();

            if (string.IsNullOrEmpty(postSubscription.User.Email))
            {
                validationState.Errors.Add(CreateValidationError(postSubscription.User.Email, "Email.RequiredError", "Email is required."));
            }
            else if (!Expressions.IsMatch("IsEmail", postSubscription.User.Email))
            {
                validationState.Errors.Add(CreateValidationError(postSubscription.User.Email, "Email.InvalidError", "Email is invalid."));
            }

            return validationState;
        }

        #endregion
    }
}
