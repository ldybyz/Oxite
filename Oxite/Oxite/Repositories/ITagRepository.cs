//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;

namespace Oxite.Repositories
{
    public interface ITagRepository
    {
        IQueryable<Tag> GetTags();
        IQueryable<KeyValuePair<Tag, int>> GetTagsWithPostCount();
        Tag GetTag(string urlName);
    }
}
