//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Services;
using Oxite.Model;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeTrackbackOutboundService : ITrackbackOutboundService
    {
        #region ITrackbackOutboundService Members

        public IEnumerable<TrackbackOutbound> GetNextOutbound(bool executeOnAll, TimeSpan interval)
        {
            throw new NotImplementedException();
        }

        public void Save(TrackbackOutbound trackback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
