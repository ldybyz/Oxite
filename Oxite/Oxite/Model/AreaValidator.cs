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
    public class AreaValidator : ValidatorBase<Area>
    {
        public AreaValidator(ILocalizationService localizationService, Site site, IRegularExpressions expressions)
            : base(localizationService, site, expressions)
        {
        }

        #region IValidator Members

        public override ValidationState Validate(Area area)
        {
            if (area == null) throw new ArgumentNullException("area");

            ValidationState validationState = new ValidationState();

            if (string.IsNullOrEmpty(area.Name))
            {
                validationState.Errors.Add(CreateValidationError(area.Name, "Name.RequiredError", "Name is not set."));
            }
            else
            {
                if (!Expressions.IsMatch("AreaName", area.Name))
                {
                    validationState.Errors.Add(CreateValidationError(area.Name, "Name.InvalidError", "Name is invalid and must be alphanumeric."));
                }
                else if (area.Name.Length > 256)
                {
                    validationState.Errors.Add(CreateValidationError(area.Name, "Name.MaxLengthExceededError", "Name must be less than or equal to {0} characters", 256));
                }
            }

            if (string.IsNullOrEmpty(area.DisplayName))
            {
                validationState.Errors.Add(CreateValidationError(area.DisplayName, "DisplayName.RequiredError", "DisplayName is not set."));
            }
            else
            {
                if (area.DisplayName.Length > 256)
                {
                    validationState.Errors.Add(CreateValidationError(area.DisplayName, "DisplayName.MaxLengthExceededError", "DisplayName must be less than or equal to {0} characters", 256));
                }
            }

            if (!string.IsNullOrEmpty(area.Description) && area.Description.Length > 256)
            {
                validationState.Errors.Add(CreateValidationError(area.Description, "Description.MaxLengthExceededError", "Description must be less than or equal to {0} characters", 256));
            }

            return validationState;
        }

        #endregion
    }
}
