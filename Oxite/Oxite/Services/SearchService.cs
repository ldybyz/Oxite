//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.Services
{
    public class SearchService : ISearchService
    {
        private readonly IPostRepository repository;

        public SearchService(IPostRepository repository)
        {
            this.repository = repository;
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, SearchCriteria criteria)
        {
            return repository.GetPosts(criteria).GetPage(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, SearchCriteria criteria, DateTime sinceDate)
        {
            return repository.GetPosts(criteria, sinceDate).GetPage(pageIndex, pageSize);
        }
    }
}
