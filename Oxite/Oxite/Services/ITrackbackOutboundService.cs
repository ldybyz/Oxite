//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Model;

namespace Oxite.Services
{
    public interface ITrackbackOutboundService
    {
        IEnumerable<TrackbackOutbound> GetNextOutbound(bool executeOnAll, TimeSpan interval);
        void Save(TrackbackOutbound trackback);
    }
}
