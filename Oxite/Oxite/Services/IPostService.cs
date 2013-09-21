//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Model;
using Oxite.Validation;

namespace Oxite.Services
{
    public interface IPostService
    {
        IPageOfList<Post> GetPosts(int pageIndex, int pageSize, DateTime? sinceDate);
        IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Tag tag, DateTime? sinceDate);
        IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Area area, DateTime? sinceDate);
        IPageOfList<Post> GetPosts(int pageIndex, int pageSize, ArchiveData archive);
        IPageOfList<Post> GetPosts(int pageIndex, int pageSize, SearchCriteria criteria, DateTime? sinceDate);
        IPageOfList<Post> GetPostsWithDrafts(int pageIndex, int pageSize);
        IList<Post> GetPosts(DateTime startDate, DateTime endDate);
        IList<DateTime> GetPostDateGroups();
        Post GetPost(Area area, string slug);
        Post GetPost(Guid id);
        void ValidatePost(Post post, out ValidationStateDictionary validationState);
        void AddPost(Area area, Post post, User creator, out ValidationStateDictionary validationState, out Post newPost);
        void AddPostWithoutTrackbacks(Area area, Post post, User creator, out ValidationStateDictionary validationState, out Post newPost);
        //todo: (nheskew) need to consolidate
        ValidationStateDictionary AddPost(Area area, Post post);
        void EditPost(Area area, Post post, Post postEdits, out ValidationStateDictionary validationState);
        //todo: (nheskew) need to consolidate
        ValidationStateDictionary EditPost(Area area, Post post);
        void RemovePost(Post post);
        void RemoveAll(Area area);

        IList<KeyValuePair<ArchiveData, int>> GetArchives();
        IList<KeyValuePair<ArchiveData, int>> GetArchives(Area area);

        Comment GetComment(Guid commentID);
        IList<ParentAndChild<PostBase,Comment>> GetComments();
        IList<ParentAndChild<PostBase, Comment>> GetComments(Area area);
        IList<Comment> GetComments(Post post);
        IList<ParentAndChild<PostBase, Comment>> GetComments(Tag tag);
        IPageOfList<ParentAndChild<PostBase, Comment>> GetComments(int pageIndex, int pageSize);
        IPageOfList<ParentAndChild<PostBase, Comment>> GetComments(int pageIndex, int pageSize, bool includePending, bool sortDescending);
        void ValidateComment(Comment comment, out ValidationStateDictionary validationState);
        void AddComment(Area area, Post post, Comment comment, UserBase creator, bool subscribe, out ValidationStateDictionary validationState, out Comment newComment);
        void AddCommentWithoutMessages(Area area, Post post, Comment comment, UserBase creator, bool subscribe, out ValidationStateDictionary validationState, out Comment newComment);
        void EditComment(Area area, Post post, Comment comment, out ValidationStateDictionary validationState);
        void RemoveComment(Post post, Guid commentID);
        void ApproveComment(Post post, Guid commentID);

        ValidationStateDictionary AddTrackback(Post post, Trackback trackback);
        ValidationStateDictionary EditTrackback(Trackback trackback);
    }
}
