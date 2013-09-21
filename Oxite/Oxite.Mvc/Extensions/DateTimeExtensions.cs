//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Mvc.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToStringForEdit(this DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToShortTimeString());
        }

        public static string ToStringForFeed(this DateTime dateTime)
        {
            return dateTime.ToString("R");
        }
    }
}