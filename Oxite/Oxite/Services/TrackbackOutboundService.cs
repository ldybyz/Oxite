//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.Services
{
    public class TrackbackOutboundService : ITrackbackOutboundService
    {
        private readonly ITrackbackOutboundRepository repository;

        public TrackbackOutboundService(ITrackbackOutboundRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<TrackbackOutbound> GetNextOutbound(bool executeOnAll, TimeSpan interval)
        {
            return repository.GetNextOutbound(executeOnAll, interval);
        }

        public void Save(TrackbackOutbound trackback)
        {
            repository.Save(trackback);
        }
    }
}
