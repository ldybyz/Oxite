//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Mvc.ViewModels
{
    public class TrackbackViewModel
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public TrackbackViewModel()
        {
            ErrorCode = 0;
        }
    }
}
