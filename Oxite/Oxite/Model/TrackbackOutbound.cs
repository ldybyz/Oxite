//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Model
{
    public class TrackbackOutbound
    {
        public Guid ID { get; set; }
        public string TargetUrl { get; set; }
        public Guid PostID { get; set; }
        public string PostTitle { get; set; }
        public string PostAreaTitle { get; set; }
        public string PostBody { get; set; }
        public string PostUrl { get; set; }

        public DateTime? Sent { get; set; }
        public int RemainingRetryCount { get; set; }

        public void MarkAsCompleted()
        {
            Sent = DateTime.Now.ToUniversalTime();
            RemainingRetryCount = 0;
        }

        public void MarkAsFailed()
        {
            Sent = null;
            RemainingRetryCount--;
        }
    }
}
