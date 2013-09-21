//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Validation
{
    public class ValidationStateDictionary : Dictionary<Type, ValidationState>
    {
        public ValidationStateDictionary() { }

        public ValidationStateDictionary(Type type, ValidationState validationState)
        {
            Add(type, validationState);
        }

        public bool IsValid
        {
            get
            {
                return  this.All(validationState => validationState.Value.IsValid);
            }
        }
    }
}
