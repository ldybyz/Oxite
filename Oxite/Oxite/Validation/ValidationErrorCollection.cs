//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.ObjectModel;

namespace Oxite.Validation
{
    public class ValidationErrorCollection : Collection<ValidationError>
    {
        public void Add(string name, object attemptedValue, string message)
        {
            Add(new ValidationError(name, attemptedValue, message));
        }

        public void Add(string name, object attemptedValue, Exception exception)
        {
            Add(new ValidationError(name, attemptedValue, exception));
        }
    }
}
