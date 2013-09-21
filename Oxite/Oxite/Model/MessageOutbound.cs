//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Model
{
    public class MessageOutbound
    {
        public Guid ID { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public DateTime? Sent { get; set; }
        public int RemainingRetryCount { get; set; }

        public void MarkAsCompleted()
        {
            Sent = DateTime.UtcNow;
            RemainingRetryCount = 0;
        }

        public void MarkAsFailed()
        {
            Sent = null;
            RemainingRetryCount--;
        }
    }
}
