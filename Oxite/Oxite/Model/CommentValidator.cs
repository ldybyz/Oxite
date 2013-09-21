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
    public class CommentValidator : ValidatorBase<Comment>
    {
        public CommentValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions)
        {
        }

        #region IValidator Members

        public override ValidationState Validate(Comment comment)
        {
            if (comment == null) throw new ArgumentNullException("comment");

            ValidationState validationState = new ValidationState();

            if (string.IsNullOrEmpty(comment.Body))
            {
                validationState.Errors.Add(CreateValidationError(comment.Body, "Body.RequiredError", "Comment (body) is not set."));
            }

            if (comment.Creator == null)
            {
                validationState.Errors.Add(CreateValidationError(comment.Creator, "Creator.RequiredError", "Comment creator is not set."));
            }

            if (comment.CreatorIP < 0)
            {
                validationState.Errors.Add(CreateValidationError(comment.CreatorIP, "CreatorIP.InvalidError", "CreatorIP value is invalid."));
            }

            if (comment.CreatorUserAgent == null)
            {
                validationState.Errors.Add(CreateValidationError(comment.CreatorUserAgent, "CreatorUserAgent.RequiredError", "CreatorUserAgent string is not set."));
            }

            if (comment.Language == null)
            {
                validationState.Errors.Add(CreateValidationError(comment.Language, "Language.RequiredError", "Comment language is not set"));
            }

            if (comment.State == EntityState.NotSet)
            {
                validationState.Errors.Add(CreateValidationError(comment.State, "State.RequiredError", "Comment state is not set."));
            }

            return validationState;
        }

        #endregion
    }
}
