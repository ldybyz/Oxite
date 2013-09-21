//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model
{
    public class ArchiveContainer : NamedEntity
    {
        public ArchiveData ArchiveData { get; set; }

        public ArchiveContainer(ArchiveData archiveData)
        {
            ArchiveData = archiveData;
        }
    }
}
