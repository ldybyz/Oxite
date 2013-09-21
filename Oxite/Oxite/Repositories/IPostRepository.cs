//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;

namespace Oxite.Repositories
{
    public interface IPostRepository
    {
        IQueryable<Post> GetPosts(bool includeDrafts);
        IQueryable<Post> GetPosts(DateTime sinceDate);
        IQueryable<Post> GetPosts(Tag tag);
        IQueryable<Post> GetPosts(Tag tag, DateTime sinceDate);
        IQueryable<Post> GetPosts(Area area);
        IQueryable<Post> GetPosts(Area area, DateTime sinceDate);
        IQueryable<Post> GetPosts(ArchiveData archive);
        IQueryable<Post> GetPosts(SearchCriteria criteria);
        IQueryable<Post> GetPosts(SearchCriteria criteria, DateTime sinceDate);
        IList<Post> GetPosts(DateTime startDate, DateTime endDate);
        IList<DateTime> GetPostDateGroups();
        Post GetPost(Area area, string slug);
        Post GetPost(Guid id);
        void Save(Post post);
        void Remove(Post post);
        void RemoveAll(Area area);

        IQueryable<KeyValuePair<ArchiveData, int>> GetArchives();
        IQueryable<KeyValuePair<ArchiveData, int>> GetArchives(Area area);

        Comment GetComment(Guid commentID);
        IQueryable<ParentAndChild<PostBase, Comment>> GetComments(bool includePending, bool sortDescending);
        IQueryable<ParentAndChild<PostBase, Comment>> GetComments(Area area);
        IQueryable<Comment> GetComments(Post post);
        IQueryable<ParentAndChild<PostBase, Comment>> GetComments(Tag tag);
        void SaveComment(Post post, Comment comment);

        IEnumerable<TrackbackOutbound> GetUnsentTrackbacks(Post post);
        void SaveTrackbacks(IEnumerable<TrackbackOutbound> trackbacks);
        void RemoveTrackbacks(IEnumerable<TrackbackOutbound> trackbacks);

        void SaveMessages(IEnumerable<MessageOutbound> messages);

        IEnumerable<PostSubscription> GetSubscriptions(Post post);
        bool GetSubscriptionExists(Post post, UserBase user);
        void AddSubscription(Post post, UserBase user);

        void SaveTrackback(Post post, Trackback trackback);
    }
}
