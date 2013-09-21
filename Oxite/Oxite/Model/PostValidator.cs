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
    public class PostValidator : ValidatorBase<Post>
    {
        public PostValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions) { }

        #region IValidator Members

        public override ValidationState Validate(Post post)
        {
            if (post == null) throw new ArgumentNullException("post");

            ValidationState validationState = new ValidationState();

            if (post.Creator == null)
            {
                validationState.Errors.Add(CreateValidationError(post.Creator, "Creator.RequiredError", "Creator is not set."));
            }

            if (post.Area == null)
            {
                validationState.Errors.Add(CreateValidationError(post.Area, "Area.RequiredError", "Area is not set."));
            }

            post.Title = post.Title.Trim();
            if (string.IsNullOrEmpty(post.Title))
            {
                validationState.Errors.Add(CreateValidationError(post.Title, "Title.RequiredError", "Title is not set."));
            }
            else
            {
                if (post.Title.Length > 250)
                {
                    validationState.Errors.Add(CreateValidationError(post.Title, "Title.MaxLengthExceededError", "Title must be {0} characters or less.", 250));
                }
            }

            post.Body = post.Body.Trim();
            if (string.IsNullOrEmpty(post.Body))
            {
                validationState.Errors.Add(CreateValidationError(post.Body, "Body.RequiredError", "Body is not set."));
            }

            post.Slug = post.Slug.Trim();
            if (string.IsNullOrEmpty(post.Slug))
            {
                validationState.Errors.Add(CreateValidationError(post.Slug, "Slug.RequiredError", "Slug is not set."));
            }
            else
            {
                if (!Expressions.IsMatch("IsSlug", post.Slug))
                {
                    validationState.Errors.Add(CreateValidationError(post.Slug, "Slug.NotValidError", "Slug is not valid."));
                }
            }

            return validationState;
        }

        #endregion
    }
}
