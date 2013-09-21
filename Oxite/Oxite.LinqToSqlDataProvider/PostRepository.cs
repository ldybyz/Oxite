//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Transactions;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class PostRepository : IPostRepository
    {
        private readonly OxiteLinqToSqlDataContext context;
        private readonly Guid siteID;
        private readonly IRegularExpressions expressions;

        public PostRepository(OxiteLinqToSqlDataContext context, Site site, IRegularExpressions expressions)
        {
            this.context = context;
            siteID = site.ID;
            this.expressions = expressions;
        }

        #region IPostRepository Members

        public IQueryable<Post> GetPosts(bool includeDrafts)
        {
            if(includeDrafts)
                return projectPosts(getPostsQuery(siteID));

            return projectPosts(excludeNotYetPublished(getPostsQuery(siteID)));
        }

        public IQueryable<Post> GetPosts(DateTime sinceDate)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(siteID).Where(p => p.PublishedDate > sinceDate)));
        }

        public IQueryable<Post> GetPosts(Tag tag)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(tag)));
        }

        public IQueryable<Post> GetPosts(Tag tag, DateTime sinceDate)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(tag).Where(p => p.PublishedDate > sinceDate)));
        }

        private IQueryable<oxite_Post> getPostsQuery(Tag tag)
        {
            return
                from p in context.oxite_Posts
                join ptr in context.oxite_PostTagRelationships on p.PostID equals ptr.PostID
                where ptr.TagID == tag.ID
                select p;
        }

        public IQueryable<Post> GetPosts(Area area)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(area)));
        }

        public IQueryable<Post> GetPosts(Area area, DateTime sinceDate)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(area).Where(p => p.PublishedDate > sinceDate)));
        }

        private IQueryable<oxite_Post> getPostsQuery(Area area)
        {
            return
                from p in context.oxite_Posts
                join par in context.oxite_PostAreaRelationships on p.PostID equals par.PostID
                where par.AreaID == area.ID
                select p;
        }

        public IQueryable<Post> GetPosts(SearchCriteria criteria)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(criteria)));
        }

        public IQueryable<Post> GetPosts(SearchCriteria criteria, DateTime sinceDate)
        {
            return projectPosts(excludeNotYetPublished(getPostsQuery(criteria).Where(p => p.PublishedDate > sinceDate)));
        }

        public IList<Post> GetPosts(DateTime startDate, DateTime endDate)
        {
            return projectPosts(
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                where a.SiteID == siteID && p.PublishedDate >= startDate && p.PublishedDate < endDate && p.State == (byte)EntityState.Normal
                orderby p.PublishedDate
                select p
                ).ToList();
        }

        public IList<DateTime> GetPostDateGroups()
        {
            return (
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                where a.SiteID == siteID && p.PublishedDate <= DateTime.UtcNow && p.State == (byte)EntityState.Normal
                orderby p.PublishedDate
                group p by new DateTime(p.PublishedDate.Year, p.PublishedDate.Month, 1)
                    into results
                    select results.Key
                ).ToList();
        }

        public IQueryable<KeyValuePair<ArchiveData, int>> GetArchives()
        {
            return from p in context.oxite_Posts
                   join ap in context.oxite_PostAreaRelationships on p.PostID equals ap.PostID
                   join a in context.oxite_Areas on ap.AreaID equals a.AreaID
                   where a.SiteID == siteID && p.PublishedDate <= DateTime.UtcNow && p.State == (byte)EntityState.Normal
                   let month = new DateTime(p.PublishedDate.Year, p.PublishedDate.Month, 1)
                   group p by month into months
                   orderby months.Key descending
                   select new KeyValuePair<ArchiveData, int>(new ArchiveData(months.Key.Year + "/" + months.Key.Month), months.Count());
        }

        public IQueryable<KeyValuePair<ArchiveData, int>> GetArchives(Area area)
        {
            return from p in context.oxite_Posts
                   join ap in context.oxite_PostAreaRelationships on p.PostID equals ap.PostID
                   join a in context.oxite_Areas on ap.AreaID equals a.AreaID
                   where a.SiteID == siteID && a.AreaName.ToLower() == area.Name.ToLower() && p.PublishedDate <= DateTime.UtcNow && p.State == (byte)EntityState.Normal
                   let month = new DateTime(p.PublishedDate.Year, p.PublishedDate.Month, 1)
                   group p by month into months
                   orderby months.Key descending
                   select new KeyValuePair<ArchiveData, int>(new ArchiveData(months.Key.Year + "/" + months.Key.Month), months.Count());
        }

        //TODO: (erikpo) Should call a stored procedure instead for full text searches (maybe a plugin?)
        private IQueryable<oxite_Post> getPostsQuery(SearchCriteria criteria)
        {
            return
                from p in context.oxite_Posts
                join par in context.oxite_PostAreaRelationships on p.PostID equals par.PostID
                join a in context.oxite_Areas on par.AreaID equals a.AreaID
                where a.SiteID == siteID && (p.Title.Contains(criteria.Term) || p.Body.Contains(criteria.Term))
                select p;
        }

        public Comment GetComment(Guid commentID)
        {
            return projectComments(context.oxite_Comments.Where(c => c.CommentID == commentID)).Select(pac => pac.Child).FirstOrDefault();
        }

        public IQueryable<ParentAndChild<PostBase, Comment>> GetComments(bool includePending, bool sortDescending)
        {
            var query =
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                join c in context.oxite_Comments on p.PostID equals c.PostID
                where a.SiteID == siteID
                select c;

            query = includePending
                ? query.Where(c => c.State == (byte)EntityState.Normal || c.State == (byte)EntityState.PendingApproval)
                : query.Where(c => c.State == (byte)EntityState.Normal);

            query = sortDescending
                ? query.OrderByDescending(c => c.CreatedDate)
                : query.OrderBy(c => c.CreatedDate);

            return projectComments(query);
        }

        public IQueryable<ParentAndChild<PostBase, Comment>> GetComments(Area area)
        {
            IQueryable<oxite_Comment> commentsByArea =
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                join c in context.oxite_Comments on p.PostID equals c.PostID
                where a.SiteID == siteID && (a.AreaID == area.ID || a.AreaName == area.Name) && c.State == (byte)EntityState.Normal
                orderby c.CreatedDate descending
                select c;

            return projectComments(commentsByArea);
        }

        public IQueryable<Comment> GetComments(Post post)
        {
            IQueryable<oxite_Comment> commentsByPost =
                from c in context.oxite_Comments
                where c.PostID == post.ID && c.State == (byte)EntityState.Normal
                orderby c.CreatedDate ascending
                select c;

            return projectComments(commentsByPost).Select(pc => pc.Child);
        }

        public IQueryable<ParentAndChild<PostBase, Comment>> GetComments(Tag tag)
        {
            IQueryable<oxite_Comment> commentsByTag =
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                join c in context.oxite_Comments on p.PostID equals c.PostID
                join ptr in context.oxite_PostTagRelationships on p.PostID equals ptr.PostID
                where a.SiteID == siteID && ptr.TagID == tag.ID && c.State == (byte)EntityState.Normal
                orderby c.CreatedDate descending
                select c;

            return projectComments(commentsByTag);
        }

        public void Save(Post post)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                oxite_Post postToSave = null;
                bool postIsNew = false;
                Guid postID = post.ID;

                if (post.ID != Guid.Empty)
                {
                    postToSave = context.oxite_Posts.Where(p => p.PostID == post.ID).FirstOrDefault();
                }

                if (postToSave == null)
                {
                    if (post.ID == Guid.Empty)
                    {
                        postID = Guid.NewGuid();
                    }

                    postToSave = new oxite_Post { PostID = postID };

                    context.oxite_Posts.InsertOnSubmit(postToSave);

                    postIsNew = true;
                }

                postToSave.Body = post.Body;
                postToSave.BodyShort = post.BodyShort;
                postToSave.CreatedDate = post.Created ?? DateTime.UtcNow;
                postToSave.ModifiedDate = DateTime.UtcNow;
                postToSave.PublishedDate = post.Published ?? SqlDateTime.MaxValue.Value;
                postToSave.Slug = post.Slug;
                postToSave.State = (byte)post.State;
                postToSave.Title = post.Title;
                postToSave.CommentingDisabled = post.CommentingDisabled;

                // Tags: Use existing, create new ones if needed. Don't edit old tags
                foreach (Tag tag in post.Tags)
                {
                    string normalizedName = normalizeTagName(tag.Name);

                    oxite_Tag persistenceTag = context.oxite_Tags.Where(t => t.TagName.ToLower() == normalizedName.ToLower()).FirstOrDefault();
                    if (persistenceTag == null)
                    {
                        Guid newTagID = Guid.NewGuid();
                        persistenceTag = new oxite_Tag { TagName = normalizedName, CreatedDate = tag.Created.HasValue ? tag.Created.Value : DateTime.UtcNow, TagID = newTagID, ParentTagID = newTagID };
                        context.oxite_Tags.InsertOnSubmit(persistenceTag);
                    }

                    if (!context.oxite_PostTagRelationships.Where(pt => pt.PostID == postToSave.PostID && pt.TagID == persistenceTag.TagID).Any())
                        context.oxite_PostTagRelationships.InsertOnSubmit(new oxite_PostTagRelationship { PostID = postToSave.PostID, TagID = persistenceTag.TagID, TagDisplayName = tag.DisplayName ?? tag.Name });
                }

                var updatedTagNames = post.Tags.Select(t => normalizeTagName(t.Name).ToLower());

                var tagsRemoved = from t in context.oxite_Tags
                                  join pt in context.oxite_PostTagRelationships on t.TagID equals pt.TagID
                                  where pt.PostID == postToSave.PostID && !updatedTagNames.Contains(t.TagName.ToLower())
                                  select pt;

                context.oxite_PostTagRelationships.DeleteAllOnSubmit(tagsRemoved);

                // The area associated with the post but not changes to the area itself
                oxite_Area area = post.Area.ID == Guid.Empty
                    ? context.oxite_Areas.Where(a => a.AreaName.ToLower() == post.Area.Name.ToLower()).FirstOrDefault()
                    : context.oxite_Areas.Where(a => a.AreaID == post.Area.ID).FirstOrDefault();

                if (area == null)
                    throw new InvalidOperationException(string.Format("Area {0} could not be found.", post.Area.Name ?? post.Area.ID.ToString()));

                if (postIsNew && 
                    (from p in context.oxite_Posts
                     join ap in context.oxite_PostAreaRelationships on p.PostID equals ap.PostID
                     where ap.AreaID == area.AreaID && p.Slug == postToSave.Slug
                     select p).Any())
                    throw new InvalidOperationException(string.Format("There is already a post with slug {0} in area {1}.", post.Slug, area.AreaName));

                if (postToSave.oxite_PostAreaRelationships.Count == 0)
                {
                    context.oxite_PostAreaRelationships.InsertOnSubmit(new oxite_PostAreaRelationship { AreaID = area.AreaID, PostID = postToSave.PostID });
                }
                else
                {
                    oxite_PostAreaRelationship areaMapping = context.oxite_PostAreaRelationships.Where(pa => pa.PostID == postToSave.PostID).FirstOrDefault();

                    areaMapping.AreaID = area.AreaID;
                }

                // The associated user but not changes to the user itself
                oxite_User user = context.oxite_Users.Where(u => u.Username.ToLower() == post.Creator.Name.ToLower()).FirstOrDefault();
                if (user == null)
                    throw new InvalidOperationException(string.Format("User {0} could not be found", post.Creator.Name));

                if (postToSave.CreatorUserID != user.UserID)
                    postToSave.oxite_User = user;

                postToSave.SearchBody = postToSave.Title + postToSave.Body + postToSave.oxite_User.Username + postToSave.oxite_User.DisplayName + string.Join("", post.Tags.Select(t => t.Name + t.DisplayName).ToArray());

                context.SubmitChanges();

                scope.Complete();

                post.ID = postID;
            }
        }

        public IQueryable<Post> GetPosts(ArchiveData archive)
        {
            var query =
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                where a.SiteID == siteID && p.PublishedDate.Year == archive.Year
                select p;

            if (archive.Month > 0)
                query = query.Where(p => p.PublishedDate.Month == archive.Month);

            if (archive.Day > 0)
                query = query.Where(p => p.PublishedDate.Day == archive.Day);

            return projectPosts(query);
        }

        public Post GetPost(Area area, string slug)
        {
            IQueryable<oxite_Post> post =
                from p in context.oxite_Posts
                join pa in context.oxite_PostAreaRelationships on p.PostID equals pa.PostID
                join a in context.oxite_Areas on pa.AreaID equals a.AreaID
                where a.SiteID == siteID && (a.AreaID == area.ID || a.AreaName == area.Name) && string.Compare(p.Slug, slug, true) == 0
                select p;

            return projectPosts(post).FirstOrDefault();
        }

        public Post GetPost(Guid id)
        {
            IQueryable<oxite_Post> post =
                from p in context.oxite_Posts
                where p.PostID == id
                select p;

            return projectPosts(post).FirstOrDefault();
        }

        public void Remove(Post post)
        {
            oxite_Post persistencePost = context.oxite_Posts.Where(p => p.PostID == post.ID).FirstOrDefault();

            if (persistencePost != null)
            {
                persistencePost.State = (byte)EntityState.Removed;
                context.SubmitChanges();
            }
        }

        //INFO: (erikpo) Not sure if this logic should exist here or in the database (as cascade delete on the relationships)
        public void RemoveAll(Area area)
        {
            var postAreaRelationships =
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                where a.SiteID == siteID && par.AreaID == area.ID
                select par;

            var posts =
                from par in postAreaRelationships
                join p in context.oxite_Posts on par.PostID equals p.PostID
                select p;

            var postTagRelationships =
                from p in posts
                join ptr in context.oxite_PostTagRelationships on p.PostID equals ptr.PostID
                select ptr;

            var postComments =
                from p in posts
                join c in context.oxite_Comments on p.PostID equals c.PostID
                select c;

            var postCommentAnonymouses =
                from c in postComments
                join ca in context.oxite_CommentAnonymous on c.CommentID equals ca.CommentID
                select ca;

            var postSubscriptions =
                from p in posts
                join s in context.oxite_Subscriptions on p.PostID equals s.PostID
                select s;

            var postSubscriptionAnonymouses =
                from s in postSubscriptions
                join sa in context.oxite_SubscriptionAnonymous on s.SubscriptionID equals sa.SubscriptionID
                select sa;

            var postTrackbacks =
                from p in posts
                join t in context.oxite_Trackbacks on p.PostID equals t.PostID
                select t;

            context.oxite_Trackbacks.DeleteAllOnSubmit(postTrackbacks);
            context.oxite_SubscriptionAnonymous.DeleteAllOnSubmit(postSubscriptionAnonymouses);
            context.oxite_Subscriptions.DeleteAllOnSubmit(postSubscriptions);
            context.oxite_CommentAnonymous.DeleteAllOnSubmit(postCommentAnonymouses);
            context.oxite_Comments.DeleteAllOnSubmit(postComments);
            context.oxite_PostTagRelationships.DeleteAllOnSubmit(postTagRelationships);
            context.oxite_Posts.DeleteAllOnSubmit(posts);
            context.oxite_PostAreaRelationships.DeleteAllOnSubmit(postAreaRelationships);

            context.SubmitChanges();
        }

        public void SaveComment(Post post, Comment comment)
        {
            oxite_Comment persistenceComment = null;
            Guid commentID = comment.ID;

            if (comment.ID != Guid.Empty)
            {
                persistenceComment = context.oxite_Comments.Where(c => c.CommentID == comment.ID).FirstOrDefault();
            }

            if (persistenceComment == null)
            {
                if (commentID == Guid.Empty)
                {
                    commentID = Guid.NewGuid();
                }

                persistenceComment = new oxite_Comment { CommentID = commentID };

                context.oxite_Comments.InsertOnSubmit(persistenceComment);
            }

            persistenceComment.PostID = post.ID;
            persistenceComment.Body = comment.Body;
            persistenceComment.CreatedDate = comment.Created ?? DateTime.UtcNow;
            persistenceComment.CreatorIP = comment.CreatorIP;
            persistenceComment.ModifiedDate = DateTime.UtcNow;
            persistenceComment.State = (byte)comment.State;
            persistenceComment.UserAgent = comment.CreatorUserAgent;
            persistenceComment.oxite_Language = context.oxite_Languages.Where(l => l.LanguageName == comment.Language.Name).FirstOrDefault();

            if (comment.Creator is User)
            {
                oxite_User persistenceUser = context.oxite_Users.Where(u => u.Username.ToLower() == comment.Creator.Name.ToLower()).FirstOrDefault();

                if (persistenceUser == null)
                    throw new InvalidOperationException(string.Format("User {0} could not be found", comment.Creator.Name));

                persistenceComment.oxite_User = persistenceUser;
            }
            else
            {
                oxite_User anonymousUser = context.oxite_Users.Where(u => u.Username == "Anonymous").FirstOrDefault();

                if (anonymousUser == null)
                    throw new InvalidOperationException("Could not find anonymous user");

                if (persistenceComment.oxite_CommentAnonymous == null)
                    persistenceComment.oxite_CommentAnonymous = new oxite_CommentAnonymous();

                persistenceComment.oxite_CommentAnonymous.Email = comment.Creator.Email;
                persistenceComment.oxite_CommentAnonymous.HashedEmail = comment.Creator.HashedEmail;
                persistenceComment.oxite_CommentAnonymous.Name = comment.Creator.Name;
                persistenceComment.oxite_CommentAnonymous.Url = comment.Creator.Url;
                persistenceComment.oxite_User = anonymousUser;
            }

            context.SubmitChanges();

            comment.ID = commentID;
            comment.Created = persistenceComment.CreatedDate;
        }

        public void SaveTrackbacks(IEnumerable<TrackbackOutbound> trackbacks)
        {
            if (trackbacks != null && trackbacks.Count() > 0)
            {
                context.oxite_TrackbackOutbounds.InsertAllOnSubmit(
                    trackbacks.Select(
                        tb =>
                            new oxite_TrackbackOutbound
                            {
                                TrackbackOutboundID = Guid.NewGuid(),
                                TargetUrl = tb.TargetUrl,
                                PostID = tb.PostID,
                                PostUrl = tb.PostUrl,
                                PostAreaTitle = tb.PostAreaTitle,
                                PostTitle = tb.PostTitle,
                                PostBody = tb.PostBody,
                                RemainingRetryCount = (byte)tb.RemainingRetryCount,
                                LastAttemptDate = null,
                                SentDate = null,
                                IsSending = false
                            }
                        )
                    );

                context.SubmitChanges();
            }
        }

        public void RemoveTrackbacks(IEnumerable<TrackbackOutbound> trackbacks)
        {
            if (trackbacks != null && trackbacks.Count() > 0)
            {
                context.oxite_TrackbackOutbounds.DeleteAllOnSubmit(
                    context.oxite_TrackbackOutbounds.Where(
                        tb => trackbacks.Select(tbInner => tbInner.ID).Contains(tb.TrackbackOutboundID)
                        )
                    );

                context.SubmitChanges();
            }
        }

        public IEnumerable<TrackbackOutbound> GetUnsentTrackbacks(Post post)
        {
            return
                from tb in context.oxite_TrackbackOutbounds
                where tb.PostID == post.ID && !tb.SentDate.HasValue
                select new TrackbackOutbound
                {
                    ID = tb.TrackbackOutboundID,
                    TargetUrl = tb.TargetUrl,
                    PostID = tb.PostID,
                    PostUrl = tb.PostUrl,
                    PostAreaTitle = tb.PostAreaTitle,
                    PostTitle = tb.PostTitle,
                    PostBody = tb.PostBody,
                    RemainingRetryCount = tb.RemainingRetryCount,
                    Sent = tb.SentDate
                };
        }

        public void SaveMessages(IEnumerable<MessageOutbound> messages)
        {
            if (messages != null && messages.Count() > 0)
            {
                context.oxite_MessageOutbounds.InsertAllOnSubmit(
                    messages.Select(
                        m =>
                            new oxite_MessageOutbound
                            {
                                MessageOutboundID = Guid.NewGuid(),
                                MessageTo = m.To,
                                MessageSubject = m.Subject,
                                MessageBody = m.Body,
                                RemainingRetryCount = (byte)m.RemainingRetryCount,
                                LastAttemptDate = null,
                                SentDate = null,
                                IsSending = false
                            }
                        )
                    );

                context.SubmitChanges();
            }
        }

        public IEnumerable<PostSubscription> GetSubscriptions(Post post)
        {
            return
                from s in context.oxite_Subscriptions
                join sa in context.oxite_SubscriptionAnonymous on s.SubscriptionID equals sa.SubscriptionID into outer
                from o in outer.DefaultIfEmpty()
                join u in context.oxite_Users on s.UserID equals u.UserID
                let user = getUser(o, u)
                where s.PostID == post.ID
                select new PostSubscription
                {
                    ID = s.SubscriptionID,
                    Post = new Post
                    {
                        ID = s.oxite_Post.PostID
                    },
                    User = user
                };
        }

        public bool GetSubscriptionExists(Post post, UserBase user)
        {
            if (user is User)
            {
                return (
                    from s in context.oxite_Subscriptions
                    where s.PostID == post.ID && s.UserID == user.ID
                    select s
                    ).Any();
            }

            Guid userID = context.oxite_Users.Where(u => u.Username == "Anonymous").FirstOrDefault().UserID;

            return (
                from s in context.oxite_Subscriptions
                join sa in context.oxite_SubscriptionAnonymous on s.SubscriptionID equals sa.SubscriptionID
                where s.PostID == post.ID && s.UserID == userID && sa.Email == user.Email
                select s
                ).Any();
        }

        public void AddSubscription(Post post, UserBase user)
        {
            Guid subscriptionID = Guid.NewGuid();
            Guid userID;

            if (user is User)
            {
                userID = user.ID;
            }
            else
            {
                userID = context.oxite_Users.Where(u => u.Username == "Anonymous").FirstOrDefault().UserID;
            }

            context.oxite_Subscriptions.InsertOnSubmit(
                new oxite_Subscription
                {
                    PostID = post.ID,
                    SubscriptionID = subscriptionID,
                    UserID = userID
                }
                );

            if (!(user is User))
            {
                context.oxite_SubscriptionAnonymous.InsertOnSubmit(
                    new oxite_SubscriptionAnonymous
                    {
                        SubscriptionID = subscriptionID,
                        Name = user.Name,
                        Email = user.Email
                    }
                    );
            }

            context.SubmitChanges();
        }

        public void SaveTrackback(Post post, Trackback trackback)
        {
            oxite_Trackback persistenceTrackback = new oxite_Trackback
            {
                BlogName = trackback.BlogName,
                Body = trackback.Body,
                CreatedDate = trackback.Created ?? DateTime.UtcNow,
                IsTargetInSource = trackback.IsTargetInSource,
                ModifiedDate = DateTime.UtcNow,
                PostID = post.ID,
                Source = trackback.Source,
                Title = trackback.Title,
                TrackbackID = Guid.NewGuid(),
                Url = trackback.Url
            };

            context.oxite_Trackbacks.InsertOnSubmit(persistenceTrackback);

            context.SubmitChanges();
        }

        #endregion

        #region Private Methods

        private IQueryable<Post> projectPosts(IQueryable<oxite_Post> posts)
        {
            return from p in posts
                   join u in context.oxite_Users on p.CreatorUserID equals u.UserID
                   join pa in context.oxite_PostAreaRelationships on p.PostID equals pa.PostID
                   join a in context.oxite_Areas on pa.AreaID equals a.AreaID
                   let c = getCommentsQuery(p.PostID)
                   let t = getTagsQuery(p.PostID)
                   let tb = getTrackbacksQuery(p.PostID)
                   where p.State != (byte)EntityState.Removed
                   orderby p.PublishedDate descending
                   select new Post
                   {
                       ID = p.PostID,
                       Creator = new User
                       {
                           DisplayName = u.DisplayName,
                           Email = u.Email,
                           HashedEmail = u.HashedEmail,
                           ID = u.UserID,
                           LanguageDefault = new Language
                           {
                               ID = u.oxite_Language.LanguageID,
                               DisplayName = u.oxite_Language.LanguageDisplayName,
                               Name = u.oxite_Language.LanguageName
                           },
                           Name = u.Username,
                           Status = u.Status,
                       },
                       Area = new Area
                       {
                           ID = a.AreaID,
                           Name = a.AreaName,
                           DisplayName = a.DisplayName,
                           Description = a.Description,
                           CommentingDisabled = a.CommentingDisabled,
                           Created = a.CreatedDate,
                           Modified = a.ModifiedDate
                       },
                       Body = p.Body,
                       BodyShort = p.BodyShort,
                       Comments = new LazyList<Comment>(c),
                       Created = p.CreatedDate,
                       Modified = p.ModifiedDate,
                       Published = p.PublishedDate != SqlDateTime.MaxValue.Value ? p.PublishedDate : (DateTime?)null,
                       Slug = p.Slug,
                       State = (EntityState)p.State,
                       Tags = t.ToList(),
                       Title = p.Title,
                       Trackbacks = tb.ToList(),
                       CommentingDisabled = p.CommentingDisabled
                   };
        }

        private IQueryable<Trackback> getTrackbacksQuery(Guid postID)
        {
            return from tb in context.oxite_Trackbacks
                   where tb.PostID == postID
                   select new Trackback
                   {
                       BlogName = tb.BlogName,
                       Body = tb.Body,
                       Created = tb.CreatedDate,
                       ID = tb.TrackbackID,
                       IsTargetInSource = tb.IsTargetInSource,
                       Modified = tb.ModifiedDate,
                       Source = tb.Source,
                       Title = tb.Title,
                       Url = tb.Url
                   };
        }

        private IQueryable<Tag> getTagsQuery(Guid guid)
        {
            return from t in context.oxite_Tags
                   join pt in context.oxite_PostTagRelationships on t.TagID equals pt.TagID
                   where pt.PostID == guid
                   select new Tag
                   {
                       Created = t.CreatedDate,
                       ID = t.TagID,
                       Name = t.TagName,
                       DisplayName = pt.TagDisplayName
                   };
        }

        private IQueryable<Comment> getCommentsQuery(Guid postID)
        {
            IQueryable<oxite_Comment> commentsByPostID =
                from c in context.oxite_Comments
                where c.PostID == postID && c.State == (byte)EntityState.Normal
                orderby c.CreatedDate ascending
                select c;

            return projectComments(commentsByPostID).Select(pc => pc.Child);
        }

        private IQueryable<ParentAndChild<PostBase,Comment>> projectComments(IQueryable<oxite_Comment> comments)
        {
            return from c in comments
                   join au in context.oxite_CommentAnonymous on c.CommentID equals au.CommentID into outer
                   from o in outer.DefaultIfEmpty()
                   join u in context.oxite_Users on c.CreatorUserID equals u.UserID
                   join p in context.oxite_Posts on c.PostID equals p.PostID
                   join pa in context.oxite_PostAreaRelationships on p.PostID equals pa.PostID
                   join a in context.oxite_Areas on pa.AreaID equals a.AreaID
                   let modelUser = getUser(o,u)
                   where p.State != (byte)EntityState.Removed
                   select new ParentAndChild<PostBase, Comment>
                   {
                       Child = getComment(c, modelUser),
                       Parent = GetPost(new Area { ID = a.AreaID }, p.Slug)
                   };
        }

        private static Comment getComment(oxite_Comment comment, UserBase user)
        {
            return new Comment
            {
                Body = comment.Body,
                Created = comment.CreatedDate,
                Creator = user,
                CreatorIP = comment.CreatorIP,
                CreatorUserAgent = comment.UserAgent,
                ID = comment.CommentID,
                Language = new Language
                {
                    DisplayName = comment.oxite_Language.LanguageDisplayName,
                    ID = comment.oxite_Language.LanguageID,
                    Name = comment.oxite_Language.LanguageName
                },
                Modified = comment.ModifiedDate,
                State = (EntityState)comment.State
            };
        }

        private static UserBase getUser(oxite_CommentAnonymous commentAnonymous, oxite_User user)
        {
            if (commentAnonymous == null)
                return getUser(user);

            return new UserBase
            {
                Name = commentAnonymous.Name,
                Email = commentAnonymous.Email,
                HashedEmail = commentAnonymous.HashedEmail,
                Url = commentAnonymous.Url
            };
        }

        private static UserBase getUser(oxite_SubscriptionAnonymous subscriptionAnonymous, oxite_User user)
        {
            if (subscriptionAnonymous == null)
                return getUser(user);

            return new UserBase
            {
                Name = subscriptionAnonymous.Name,
                Email = subscriptionAnonymous.Email
            };
        }

        private static UserBase getUser(oxite_User user)
        {
            return new User
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                HashedEmail = user.HashedEmail,
                ID = user.UserID,
                Name = user.Username,
                Password = user.Password,
                PasswordSalt = user.PasswordSalt,
                Status = user.Status,
            };
        }

        private string normalizeTagName(string tagName)
        {
            if (expressions != null)
                return expressions.Clean("TagReplace", tagName);

            return tagName;
        }

        private IQueryable<oxite_Post> getPostsQuery(Guid siteID)
        {
            return
                from a in context.oxite_Areas
                join par in context.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                join p in context.oxite_Posts on par.PostID equals p.PostID
                where a.SiteID == siteID
                select p;
        }

        private static IQueryable<oxite_Post> excludeNotYetPublished(IQueryable<oxite_Post> query)
        {
            return query.Where(p => p.PublishedDate < DateTime.UtcNow);
        }

        #endregion
    }
}
