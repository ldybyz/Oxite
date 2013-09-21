//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Oxite.Extensions;
using Oxite.Model;
using Oxite.Model.Extensions;
using Oxite.Repositories;
using Oxite.Routing;
using Oxite.Validation;

namespace Oxite.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;
        private readonly ILocalizationRepository localizationRepository;
        private readonly Site site;
        private readonly IValidationService validator;
        private readonly AbsolutePathHelper absolutePathHelper;

        public PostService(IPostRepository repository, ILocalizationRepository localizationRepository, Site site, IValidationService validator, AbsolutePathHelper absolutePathHelper)
        {
            this.repository = repository;
            this.localizationRepository = localizationRepository;
            this.site = site;
            this.validator = validator;
            this.absolutePathHelper = absolutePathHelper;
        }

        #region IPostService Members

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, DateTime? sinceDate)
        {
            return sinceDate.HasValue ? repository.GetPosts(sinceDate.Value.ToUniversalTime()).GetPage(pageIndex, pageSize) : repository.GetPosts(false).GetPage(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Tag tag, DateTime? sinceDate)
        {
            return sinceDate.HasValue ? repository.GetPosts(tag, sinceDate.Value.ToUniversalTime()).GetPage(pageIndex, pageSize) : repository.GetPosts(tag).GetPage(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, Area area, DateTime? sinceDate)
        {
            return sinceDate.HasValue ? repository.GetPosts(area, sinceDate.Value.ToUniversalTime()).GetPage(pageIndex, pageSize) :  repository.GetPosts(area).GetPage(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, ArchiveData archive)
        {
            return repository.GetPosts(archive).GetPage(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPosts(int pageIndex, int pageSize, SearchCriteria criteria, DateTime? sinceDate)
        {
            return sinceDate.HasValue ? repository.GetPosts(criteria, sinceDate.Value.ToUniversalTime()).GetPage(pageIndex, pageSize) :  repository.GetPosts(criteria).GetPage(pageIndex, pageSize);
        }

        public IPageOfList<Post> GetPostsWithDrafts(int pageIndex, int pageSize)
        {
            return repository.GetPosts(true).GetPage(pageIndex, pageSize);
        }

        public IList<Post> GetPosts(DateTime startDate, DateTime endDate)
        {
            return repository.GetPosts(startDate, endDate);
        }

        public IList<DateTime> GetPostDateGroups()
        {
            return repository.GetPostDateGroups();
        }

        public Post GetPost(Area area, string slug)
        {
            return repository.GetPost(area, slug);
        }

        public Post GetPost(Guid id)
        {
            return repository.GetPost(id);
        }

        public void ValidatePost(Post post, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(Post), validator.Validate(post));
        }

        public void AddPost(Area area, Post post, User creator, out ValidationStateDictionary validationState, out Post newPost)
        {
            validationState = new ValidationStateDictionary();

            post.Area = area;
            post.Creator = creator;

            validationState.Add(typeof(Post), validator.Validate(post));

            if (!validationState.IsValid)
            {
                newPost = null;

                return;
            }

            repository.Save(post);

            if (post.Published.HasValue)
            {
                string postUrl = absolutePathHelper.GetAbsolutePath(post);
                IEnumerable<TrackbackOutbound> trackbacks = extractTrackbacks(post, postUrl, area.DisplayName);

                repository.SaveTrackbacks(trackbacks);
            }

            if (site.AuthorAutoSubscribe && !repository.GetSubscriptionExists(post, creator))
            {
                repository.AddSubscription(post, creator);
            }

            newPost = repository.GetPost(post.ID);
        }

        //TODO: (erikpo) This is lame and should be removed once we have "events" this can be avoided
        public void AddPostWithoutTrackbacks(Area area, Post post, User creator, out ValidationStateDictionary validationState, out Post newPost)
        {
            validationState = new ValidationStateDictionary();

            post.Area = area;
            post.Creator = creator;

            validationState.Add(typeof(Post), validator.Validate(post));

            if (!validationState.IsValid)
            {
                newPost = null;

                return;
            }

            repository.Save(post);

            if (site.AuthorAutoSubscribe && !repository.GetSubscriptionExists(post, creator))
            {
                repository.AddSubscription(post, creator);
            }

            newPost = repository.GetPost(post.ID);
        }

        //todo: (nheskew) need to consolidate
        public ValidationStateDictionary AddPost(Area area, Post post)
        {
            post.Area = area;

            repository.Save(post);

            if (post.Published.HasValue)
            {
                string postUrl = absolutePathHelper.GetAbsolutePath(post);
                IEnumerable<TrackbackOutbound> trackbacks = extractTrackbacks(post, postUrl, area.DisplayName);

                repository.SaveTrackbacks(trackbacks);
            }

            return null;
        }

        public void EditPost(Area area, Post post, Post postEdits, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            postEdits.ID = post.ID;
            postEdits.Creator = post.Creator;
            postEdits.Created = post.Created;
            postEdits.State = post.State;

            postEdits.Area = area;

            validationState.Add(typeof(Post), validator.Validate(postEdits));

            if (!validationState.IsValid)
            {
                return;
            }

            repository.Save(postEdits);

            if (postEdits.Published.HasValue)
            {
                string postUrl = absolutePathHelper.GetAbsolutePath(postEdits);
                IEnumerable<TrackbackOutbound> trackbacksToAdd = extractTrackbacks(postEdits, postUrl, area.DisplayName);
                IEnumerable<TrackbackOutbound> unsentTrackbacks = repository.GetUnsentTrackbacks(postEdits);
                IEnumerable<TrackbackOutbound> trackbacksToRemove = trackbacksToAdd.Where(tb => !unsentTrackbacks.Contains(tb) && !tb.Sent.HasValue);

                repository.RemoveTrackbacks(trackbacksToRemove);
                repository.SaveTrackbacks(trackbacksToAdd);
            }
        }

        //todo: (nheskew) need to consolidate
        public ValidationStateDictionary EditPost(Area area, Post post)
        {
            post.Area = area;

            repository.Save(post);

            if (post.Published.HasValue)
            {
                string postUrl = absolutePathHelper.GetAbsolutePath(post);
                IEnumerable<TrackbackOutbound> trackbacksToAdd = extractTrackbacks(post, postUrl, area.DisplayName);
                IEnumerable<TrackbackOutbound> unsentTrackbacks = repository.GetUnsentTrackbacks(post);
                IEnumerable<TrackbackOutbound> trackbacksToRemove = trackbacksToAdd.Where(tb => !unsentTrackbacks.Contains(tb) && !tb.Sent.HasValue);

                repository.RemoveTrackbacks(trackbacksToRemove);
                repository.SaveTrackbacks(trackbacksToAdd);
            }

            return null;
        }

        public void RemovePost(Post post)
        {
            if (post != null)
                repository.Remove(post);
        }

        public void RemoveAll(Area area)
        {
            repository.RemoveAll(area);
        }

        public IList<KeyValuePair<ArchiveData, int>> GetArchives()
        {
            return repository.GetArchives().ToList();
        }

        public IList<KeyValuePair<ArchiveData, int>> GetArchives(Area area)
        {
            return repository.GetArchives(area).ToList();
        }

        public Comment GetComment(Guid commentID)
        {
            return repository.GetComment(commentID);
        }

        public IList<ParentAndChild<PostBase, Comment>> GetComments()
        {
            return repository.GetComments(false, false).ToList();
        }

        public IList<ParentAndChild<PostBase, Comment>> GetComments(Area area)
        {
            return repository.GetComments(area).ToList();
        }

        public IList<Comment> GetComments(Post post)
        {
            return repository.GetComments(post).ToList();
        }

        public IList<ParentAndChild<PostBase, Comment>> GetComments(Tag tag)
        {
            return repository.GetComments(tag).ToList();
        }

        public IPageOfList<ParentAndChild<PostBase, Comment>> GetComments(int pageIndex, int pageSize)
        {
            return GetComments(pageIndex, pageSize, false, false);
        }

        public IPageOfList<ParentAndChild<PostBase, Comment>> GetComments(int pageIndex, int pageSize, bool includePending, bool sortDescending)
        {
            return repository.GetComments(includePending, sortDescending).GetPage(pageIndex, pageSize);
        }

        public void ValidateComment(Comment comment, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(Comment), validator.Validate(comment));

            if (!(comment.Creator is User))
            {
                validationState.Add(typeof(UserBase), validator.Validate(comment.Creator));

                //some rules change for an anonymous user
                if (!string.IsNullOrEmpty(comment.Creator.HashedEmail)
                    && validationState[typeof(UserBase)] != null
                    && validationState[typeof(UserBase)].Errors.Where(v => v.Name != "Email").FirstOrDefault() == null)
                {
                    validationState.Remove(typeof(UserBase));
                }
            }
        }

        public void AddComment(Area area, Post post, Comment comment, UserBase creator, bool subscribe, out ValidationStateDictionary validationState, out Comment newComment)
        {
            validationState = new ValidationStateDictionary();

            comment.Creator = creator;

            if (comment.State == EntityState.NotSet)
            {
                try
                {
                    comment.State = creator is User
                        ? EntityState.Normal
                        : (EntityState)Enum.Parse(typeof(EntityState), site.CommentStateDefault);
                }
                catch
                {
                    comment.State = EntityState.PendingApproval;
                }
            }

            if (comment.Language == null)
            {
                comment.Language = post.Creator.LanguageDefault;
            }

            validationState.Add(typeof(Comment), validator.Validate(comment));

            //validate anonymous users
            if (!(comment.Creator is User))
            {
                validationState.Add(typeof(UserBase), validator.Validate(comment.Creator));

                comment.Creator.HashedEmail = !string.IsNullOrEmpty(comment.Creator.Email) ? comment.Creator.Email.ComputeHash() : "";
            }

            //validation for subscription
            //todo: (nheskew) moooove and unhack a bit - _feels_ wrong here
            if (subscribe)
            {
                validationState.Add(typeof (PostSubscription), validator.Validate(new PostSubscription {Post = post, User = comment.Creator}));
            }

            if (!validationState.IsValid)
            {
                newComment = null;

                return;
            }

            repository.SaveComment(post, comment);

            if (subscribe && !repository.GetSubscriptionExists(post, creator))
            {
                repository.AddSubscription(post, creator);
            }

            if (comment.State == EntityState.Normal)
            {
                repository.SaveMessages(generateMessages(post, comment));
            }

            newComment = repository.GetComment(comment.ID);
        }

        //TODO: (erikpo) This is lame and should be removed once we have "events" this can be avoided
        public void AddCommentWithoutMessages(Area area, Post post, Comment comment, UserBase creator, bool subscribe, out ValidationStateDictionary validationState, out Comment newComment)
        {
            validationState = new ValidationStateDictionary();

            comment.Creator = creator;

            if (comment.State == EntityState.NotSet)
            {
                try
                {
                    comment.State = creator is User
                        ? EntityState.Normal
                        : (EntityState)Enum.Parse(typeof(EntityState), site.CommentStateDefault);
                }
                catch
                {
                    comment.State = EntityState.PendingApproval;
                }
            }

            if (comment.Language == null)
            {
                comment.Language = post.Creator.LanguageDefault;
            }

            validationState.Add(typeof(Comment), validator.Validate(comment));

            //validate anonymous users
            if (!(comment.Creator is User))
            {
                validationState.Add(typeof(UserBase), validator.Validate(comment.Creator));

                comment.Creator.HashedEmail = !string.IsNullOrEmpty(comment.Creator.Email) ? comment.Creator.Email.ComputeHash() : "";
            }

            //validation for subscription
            //todo: (nheskew) moooove and unhack a bit - _feels_ wrong here
            if (subscribe)
            {
                validationState.Add(typeof(PostSubscription), validator.Validate(new PostSubscription { Post = post, User = comment.Creator }));
            }

            if (!validationState.IsValid)
            {
                newComment = null;

                return;
            }

            repository.SaveComment(post, comment);

            if (subscribe && !repository.GetSubscriptionExists(post, creator))
            {
                repository.AddSubscription(post, creator);
            }

            newComment = repository.GetComment(comment.ID);
        }

        public void EditComment(Area area, Post post, Comment comment, out ValidationStateDictionary validationState)
        {
            throw new NotImplementedException();
        }

        public void RemoveComment(Post post, Guid commentID)
        {
            Comment comment = GetComment(commentID);

            if (comment != null)
            {
                comment.State = EntityState.Removed;

                repository.SaveComment(post, comment);
            }
        }

        public void ApproveComment(Post post, Guid commentID)
        {
            Comment comment = GetComment(commentID);

            if (comment != null)
            {
                comment.State = EntityState.Normal;

                repository.SaveComment(post, comment);

                repository.SaveMessages(generateMessages(post, comment));
            }
        }

        public ValidationStateDictionary AddTrackback(Post post, Trackback trackback)
        {
            repository.SaveTrackback(post, trackback);

            return null;
        }

        public ValidationStateDictionary EditTrackback(Trackback trackback)
        {
            //post.Trackbacks.Add(trackback);

            //repository.Save(post);

            return null;
        }

        #endregion

        #region Private Methods

        private static IEnumerable<TrackbackOutbound> extractTrackbacks(Post post, string postUrl, string postAreaTitle)
        {
            //INFO: (erikpo) Trackback spec: http://www.sixapart.com/pronet/docs/trackback_spec
            Regex r =
                new Regex(
                    @"(?<HTML><a[^>]*href\s*=\s*[\""\']?(?<HRef>[^""'>\s]*)[\""\']?[^>]*>(?<Title>[^<]+|.*?)?</a>)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled
                    );
            MatchCollection m = r.Matches(post.Body);
            List<TrackbackOutbound> trackbacks = Enumerable.Empty<TrackbackOutbound>().ToList();

            if (m.Count > 0)
            {
                trackbacks = new List<TrackbackOutbound>(m.Count);

                foreach (Match match in m)
                {
                    trackbacks.Add(
                        new TrackbackOutbound
                        {
                            TargetUrl = match.Groups["HRef"].Value,
                            PostID = post.ID,
                            PostTitle = post.Title,
                            PostBody = post.GetBodyShort(),
                            PostAreaTitle = postAreaTitle,
                            PostUrl = postUrl,
                            //TODO: (erikpo) Need to figure out the appropriate place to put this value so it doesn't need to be hardcoded
                            RemainingRetryCount = 20
                        }
                        );
                }
            }

            return trackbacks;
        }

        private IEnumerable<MessageOutbound> generateMessages(Post post, Comment comment)
        {
            IEnumerable<PostSubscription> subscriptions = repository.GetSubscriptions(post);
            List<MessageOutbound> messages = new List<MessageOutbound>();

            foreach (PostSubscription subscription in subscriptions)
            {
                MessageOutbound message = new MessageOutbound
                {
                    ID = Guid.NewGuid(),
                    To = string.Format("{0} <{1}>", subscription.User.DisplayName, subscription.User.Email),
                    Subject = string.Format(getPhrase("ReplySubjectFormat", site.LanguageDefault, "RE: {0}"), post.Title),
                    Body = generateMessageBody(post, comment),
                    //TODO: (erikpo) Need to figure out the appropriate place to put this value so it doesn't need to be hardcoded
                    RemainingRetryCount = 1
                };

                messages.Add(message);
            }

            return messages;
        }

        private string generateMessageBody(Post post, Comment comment)
        {
            string body = getPhrase("Messages.NewComment", site.LanguageDefault, getDefaultBody());
            //TODO: (erikpo) Change this to come from the user this message is going to if applicable
            double timeZoneOffset = site.TimeZoneOffset;

            body = body.Replace("{Site.Name}", site.DisplayName);
            body = body.Replace("{User.Name}", comment.Creator.DisplayName);
            body = body.Replace("{Post.Title}", post.Title);
            //TODO: (erikpo) Change the published date to be relative (e.g. 5 minutes ago)
            body = body.Replace("{Comment.Created}", comment.Created.Value.AddHours(timeZoneOffset).ToLongTimeString());
            body = body.Replace("{Comment.Body}", comment.Body);
            body = body.Replace("{Comment.Permalink}", absolutePathHelper.GetAbsolutePath(post, comment).Replace("%23", "#"));

            return body;
        }

        private static string getDefaultBody()
        {
            return
                "<h1>New Comment on {Site.Name}</h1>" +
                "<h2>{User.Name} commented on '{Post.Title}' at {Comment.Created}</h2>" +
                "<p>{Comment.Body}</p>" +
                "<a href=\"{Comment.Permalink}\">{Comment.Permalink}</a>";
        }

        private string getPhrase(string key, string language)
        {
            return getPhrase(key, language, null);
        }

        private string getPhrase(string key, string language, string defaultValue)
        {
            Phrase phrase = localizationRepository.GetPhrases().Where(p => p.Key == key && p.Language == language).FirstOrDefault();

            if (phrase != null)
                return phrase.Value;

            if (defaultValue == null)
                return key;

            return defaultValue;
        }

        #endregion
    }
}
