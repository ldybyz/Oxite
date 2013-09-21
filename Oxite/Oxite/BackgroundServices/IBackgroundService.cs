﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using Oxite.Model;

namespace Oxite.BackgroundServices
{
    public interface IBackgroundService : IPlugin
    {
        bool ExecuteOnAll { get; set; }
        TimeSpan Interval { get; set; }
        void Run();
    }
}
