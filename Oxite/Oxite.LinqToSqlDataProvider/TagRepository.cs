//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class TagRepository : ITagRepository
    {
        private readonly OxiteLinqToSqlDataContext context;

        public TagRepository(OxiteLinqToSqlDataContext context)
        {
            this.context = context;
        }

        #region ITagRepository Members

        public IQueryable<Tag> GetTags()
        {
            return from t in context.oxite_Tags
                   join pt in context.oxite_Tags on t.ParentTagID equals pt.TagID
                   select new Tag
                   {
                       Created = t.CreatedDate,
                       ID = t.TagID,
                       Name = t.TagName,
                       DisplayName = pt.TagName
                   };
        }

        public IQueryable<KeyValuePair<Tag, int>> GetTagsWithPostCount()
        {
            return from tt in
                       (from t in context.oxite_Tags
                        join ptr in context.oxite_PostTagRelationships on t.TagID equals ptr.TagID
                        join p in context.oxite_Posts on ptr.PostID equals p.PostID
                        where p.State == (byte)EntityState.Normal && p.PublishedDate <= DateTime.Now.ToUniversalTime()
                        select new { Tag = t.oxite_Tag1, Post = p })
                   group tt by tt.Tag into results
                   where results.Key.TagID == results.Key.ParentTagID
                   orderby results.Key.TagName
                   select new KeyValuePair<Tag, int>(
                       new Tag
                       {
                           Created = results.Key.CreatedDate,
                           ID = results.Key.TagID,
                           Name = results.Key.TagName,
                           DisplayName = results.Key.TagName
                       },
                       results.Count()
                       );
        }

        public Tag GetTag(string urlName)
        {
            return (from t in context.oxite_Tags
                    where t.TagName == urlName
                    select new Tag
                    {
                        ID = t.TagID,
                        Name = t.TagName,
                        Created = t.CreatedDate,
                        DisplayName = t.TagName
                    }).FirstOrDefault();
        }

        #endregion
    }
}
