//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Services;
using Oxite.Validation;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakePostService : IPostService
    {
        public List<Post> AddedPosts { get; set; }
        public List<Post> RemovedPosts { get; set; }
        public Trackback AddedTrackback { get; set; }

        public Dictionary<int, List<Post>> PostPages { get; set; }
        public List<ParentAndChild<PostBase, Comment>> AllComments { get; set; }

        public int LastRequestedPageSize { get; set; }
        public DateTime LastRequestedSinceDate { get; set; }

        public FakePostService()
        {
            AddedPosts = new List<Post>();
            RemovedPosts = new List<Post>();
            AllComments = new List<ParentAndChild<PostBase, Comment>>();
            PostPages = new Dictionary<int, List<Post>>();
        }

        public Post GetPost(Area area, string slug)
        {
            return AddedPosts.Where(p => p.Slug == slug).FirstOrDefault();
        }

        public Post GetPost(Guid id)
        {
            return AddedPosts.Where(p => p.ID == id).FirstOrDefault();
        }

        public void ValidatePost(Post post, out ValidationStateDictionary validationState)
        {
            throw new NotImplementedException();
        }

        public void AddPost(Area area, Post post, User currentUser, out ValidationStateDictionary validationState, out Post newPost)
        {
            validationState = new ValidationStateDictionary(typeof(Post), new ValidationState());

            post.Area = area;
            post.Creator = currentUser;
            post.ID = Guid.NewGuid();
            AddedPosts.Add(post);

            newPost = post;
        }

        public void AddPostWithoutTrackbacks(Area area, Post post, User currentUser, out ValidationStateDictionary validationState, out Post newPost)
        {
            throw new NotImplementedException();
        }

        public ValidationStateDictionary AddPost(Area area, Post post)
        {
            post.Area = area;
            post.ID = Guid.NewGuid();
            AddedPosts.Add(post);

            return new ValidationStateDictionary(typeof(Post), new ValidationState());
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize)
        {
            LastRequestedPageSize = pageSize;
            if (PostPages.ContainsKey(pageIndex))
                return new PageOfList<Post>(PostPages[pageIndex], 0, PostPages.Count, PostPages.Count);

            return null;
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, DateTime? sinceDate)
        {
            LastRequestedSinceDate = sinceDate ?? default(DateTime);
            return GetPosts(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Area area, DateTime? sinceDate)
        {
            return new PageOfList<Post>(AddedPosts.Where(p => p.Area.ID == area.ID), pageIndex, pageSize, AddedPosts.Count);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Tag tag)
        {
            return null;
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Tag tag, DateTime? sinceDate)
        {
            return null;
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, SearchCriteria criteria)
        {
            return null;
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, SearchCriteria criteria, DateTime? sinceDate)
        {
            return null;
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, ArchiveData archive)
        {
            return null;
        }

        public IList<Post> GetPosts(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IList<DateTime> GetPostDateGroups()
        {
            throw new NotImplementedException();
        }

        public void EditPost(Area area, Post post, Post postEdits, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary(typeof(Post), new ValidationState());
        }

        public ValidationStateDictionary EditPost(Area area, Post post)
        {
            return new ValidationStateDictionary();
        }

        public void RemovePost(Post post)
        {
            RemovedPosts.Add(post);
        }

        public void RemoveAll(Area area)
        {
            throw new NotImplementedException();
        }

        public Comment GetComment(Guid commentID)
        {
            throw new NotImplementedException();
        }

        public IList<ParentAndChild<PostBase, Comment>> GetComments()
        {
            return AllComments;
        }

        public IList<ParentAndChild<PostBase, Comment>> GetComments(Area area)
        {
            return AllComments;
        }

        public IList<Comment> GetComments(Post post)
        {
            return AllComments.Select(s => s.Child).ToList();
        }

        public IList<ParentAndChild<PostBase, Comment>> GetComments(Tag tag)
        {
            return AllComments;
        }

        public ValidationStateDictionary AddTrackback(Post post, Trackback trackback)
        {
            AddedTrackback = trackback;
            return null;
        }

        public ValidationStateDictionary EditTrackback(Trackback trackback)
        {
            AddedTrackback = trackback;
            return null;
        }

        #region Unimplemented IPostService Members

        public IPageOfList<Post> GetPostsWithDrafts(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IList<KeyValuePair<ArchiveData, int>> GetArchives()
        {
            throw new NotImplementedException();
        }

        public IList<KeyValuePair<ArchiveData, int>> GetArchives(Area area)
        {
            throw new NotImplementedException();
        }

        public void ValidateComment(Comment comment, out ValidationStateDictionary validationState)
        {
            throw new NotImplementedException();
        }

        public void AddComment(Area area, Post post, Comment comment, UserBase creator, bool subscribe, out ValidationStateDictionary validationState, out Comment newComment)
        {
            throw new NotImplementedException();
        }

        public void AddCommentWithoutMessages(Area area, Post post, Comment comment, UserBase creator, bool subscribe, out ValidationStateDictionary validationState, out Comment newComment)
        {
            throw new NotImplementedException();
        }

        public void EditComment(Area area, Post post, Comment comment, out ValidationStateDictionary validationState)
        {
            throw new NotImplementedException();
        }

        public void RemoveComment(Post postInput, Guid commentID)
        {
            throw new NotImplementedException();
        }

        public void ApproveComment(Post postInput, Guid commentID)
        {
            throw new NotImplementedException();
        }

        public IPageOfList<ParentAndChild<PostBase, Comment>> GetComments(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfList<ParentAndChild<PostBase, Comment>> GetComments(int pageIndex, int pageSize, bool includePending, bool sortDescending)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
