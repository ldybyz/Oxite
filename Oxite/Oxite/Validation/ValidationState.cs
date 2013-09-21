//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Validation
{
    public class ValidationState
    {
        private readonly ValidationErrorCollection errors;

        public ValidationState()
        {
            errors = new ValidationErrorCollection();
        }

        public ValidationErrorCollection Errors
        {
            get { return errors; }
        }

        public bool IsValid
        {
            get
            {
                return Errors.Count == 0;
            }
        }
    }
}
