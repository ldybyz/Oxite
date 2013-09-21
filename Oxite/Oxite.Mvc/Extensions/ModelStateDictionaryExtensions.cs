//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Oxite.Validation;

namespace Oxite.Mvc.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelStateDictionary, ValidationStateDictionary validationStateDictionary)
        {
            foreach (KeyValuePair<Type, ValidationState> validationState in validationStateDictionary)
            {
                foreach (ValidationError validationError in validationState.Value.Errors)
                {
                    string modelProperty = validationError.Name.Split('.')[0];
                    string key = string.Format("{0}.{1}", validationState.Key.Name, modelProperty);

                    modelStateDictionary.AddModelError(key, validationError.Message);

                    modelStateDictionary.SetModelValue(
                        key, 
                        new ValueProviderResult(
                            validationError.AttemptedValue, 
                            validationError.AttemptedValue as string,
                            CultureInfo.CurrentCulture
                            )
                        );
                }
            }
        }
    }
}
