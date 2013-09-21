//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository repository;

        public TagService(ITagRepository repository)
        {
            this.repository = repository;
        }

        public IList<Tag> GetTags()
        {
            return repository.GetTags().ToList();
        }

        public IList<KeyValuePair<Tag, int>> GetTagsWithPostCount()
        {
            return repository.GetTagsWithPostCount().ToList();
        }

        public Tag GetTag(string name)
        {
            return repository.GetTag(name);
        }
    }
}
