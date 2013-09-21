//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Model
{
    public class User : UserBase
    {
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public byte Status { get; set; }
        public Language LanguageDefault { get; set; }
        public IEnumerable<Language> Languages
        {
            get { throw new NotImplementedException(); }
        }
    }
}
