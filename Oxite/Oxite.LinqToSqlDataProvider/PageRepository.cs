//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;
using System.Data.SqlTypes;

namespace Oxite.LinqToSqlDataProvider
{
    public class PageRepository : IPageRepository
    {
        private readonly OxiteLinqToSqlDataContext context;
        private readonly Guid siteID;

        public PageRepository(OxiteLinqToSqlDataContext context, Site site)
        {
            this.context = context;
            siteID = site.ID;
        }

        #region IPageRepository Members

        public Page GetPage(string name, Guid parentID)
        {
            IQueryable<oxite_Post> query =
                from p in context.oxite_Posts
                join pr in context.oxite_PostRelationships on p.PostID equals pr.PostID
                where pr.SiteID == siteID && string.Compare(p.Slug, name, true) == 0 && (parentID == Guid.Empty || pr.ParentPostID == parentID)
                select p;

            return projectPages(query).FirstOrDefault();
        }

        public IQueryable<Page> GetPages()
        {
            IQueryable<oxite_Post> query =
                from p in context.oxite_Posts
                join pr in context.oxite_PostRelationships on p.PostID equals pr.PostID
                where pr.SiteID == siteID
                select p;

            return projectPages(query);
        }

        public IQueryable<Page> GetChildren(Page parent)
        {
            IQueryable<oxite_Post> query =
                from p in context.oxite_Posts
                join pr in context.oxite_PostRelationships on p.PostID equals pr.PostID
                where pr.SiteID == siteID && (pr.ParentPostID == parent.ID)
                select p;

            return projectPages(query);
        }

        public void Save(Page page)
        {
            oxite_Post postToSave = null;
            oxite_PostRelationship parentPostRelationship = null;

            if (page.ID != Guid.Empty)
            {
                postToSave = context.oxite_Posts.Where(p => p.PostID == page.ID).FirstOrDefault();

                parentPostRelationship = context.oxite_PostRelationships.Where(pr => pr.SiteID == siteID && pr.PostID == page.ID).FirstOrDefault();
            }

            if (postToSave == null)
            {
                postToSave = new oxite_Post { PostID = Guid.NewGuid() };

                context.oxite_Posts.InsertOnSubmit(postToSave);
            }

            postToSave.Body = page.Body;
            postToSave.BodyShort = page.BodyShort ?? "";
            postToSave.CreatedDate = page.Created ?? DateTime.UtcNow;
            postToSave.ModifiedDate = DateTime.UtcNow;
            postToSave.PublishedDate = page.Published ?? SqlDateTime.MaxValue.Value;
            postToSave.Slug = page.Slug;
            postToSave.State = (byte)page.State;
            postToSave.Title = page.Title;
            postToSave.SearchBody = page.Body;

            // set the parent page
            if (page.Parent == null || page.Parent.ID == Guid.Empty)
            {
                if (parentPostRelationship != null)
                {
                    if (parentPostRelationship.ParentPostID != page.ID)
                    {
                        context.oxite_PostRelationships.DeleteOnSubmit(parentPostRelationship);

                        parentPostRelationship = new oxite_PostRelationship
                        {
                            SiteID = siteID,
                            PostID = postToSave.PostID,
                            ParentPostID = page.ID
                        };

                        context.oxite_PostRelationships.InsertOnSubmit(parentPostRelationship);
                    }
                }
                else
                {
                    parentPostRelationship = new oxite_PostRelationship
                    {
                        SiteID = siteID,
                        PostID = postToSave.PostID,
                        ParentPostID = postToSave.PostID
                    };

                    context.oxite_PostRelationships.InsertOnSubmit(parentPostRelationship);
                }
            }
            else
            {
                if (parentPostRelationship != null)
                {
                    if (parentPostRelationship.ParentPostID != page.Parent.ID)
                    {
                        context.oxite_PostRelationships.DeleteOnSubmit(parentPostRelationship);

                        parentPostRelationship = new oxite_PostRelationship
                        {
                            SiteID = siteID,
                            PostID = postToSave.PostID,
                            ParentPostID = page.Parent.ID
                        };

                        context.oxite_PostRelationships.InsertOnSubmit(parentPostRelationship);
                    }
                }
                else
                {
                    parentPostRelationship = new oxite_PostRelationship
                    {
                        SiteID = siteID,
                        PostID = postToSave.PostID,
                        ParentPostID = page.Parent.ID
                    };

                    context.oxite_PostRelationships.InsertOnSubmit(parentPostRelationship);
                }
            }

            // The associated user but not changes to the user itself
            oxite_User user = context.oxite_Users.Where(u => u.Username.ToLower() == page.Creator.Name.ToLower()).FirstOrDefault();

            if (user == null)
                throw new InvalidOperationException(string.Format("User {0} could not be found", page.Creator.Name));

            if (postToSave.CreatorUserID != user.UserID)
                postToSave.oxite_User = user;

            context.SubmitChanges();
        }

        public void Remove(Page page)
        {
            if (page.ID == Guid.Empty) throw new ArgumentException("ID is required", "page");

            oxite_Post persistencePost = context.oxite_Posts.Where(p => p.PostID == page.ID).FirstOrDefault();

            if (persistencePost != null)
            {
                persistencePost.State = (byte)EntityState.Removed;

                context.SubmitChanges();
            }
        }

        #endregion

        private IQueryable<Page> projectPages(IQueryable<oxite_Post> posts)
        {
            return from p in posts
                   join u in context.oxite_Users on p.CreatorUserID equals u.UserID
                   let c = getCommentsQuery(p.PostID)
                   where p.State != (byte)EntityState.Removed
                   orderby p.PublishedDate descending
                   select new Page
                   {
                       ID = p.PostID,
                       Parent = getParentPage(p),
                       HasChildren = getHasChildren(p),
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
                       Body = p.Body,
                       BodyShort = p.BodyShort,
                       Comments = new LazyList<Comment>(c),
                       Created = p.CreatedDate,
                       Modified = p.ModifiedDate,
                       Published = p.PublishedDate,
                       Slug = p.Slug,
                       State = (EntityState)p.State,
                       Title = p.Title,
                       CommentingDisabled = p.CommentingDisabled
                   };
        }

        private Page getParentPage(oxite_Post post)
        {
            return (
                from pr in context.oxite_PostRelationships
                join p in context.oxite_Posts on pr.ParentPostID equals p.PostID
                where pr.PostID == post.PostID && pr.ParentPostID != post.PostID
                select new Page
                {
                    ID = p.PostID,
                    Slug = p.Slug,
                    Parent = getParentPage(p)
                }
                ).FirstOrDefault();
        }

        private bool getHasChildren(oxite_Post post)
        {
            return (from pr in context.oxite_PostRelationships where pr.ParentPostID == post.PostID select pr).Any();
        }

        private IQueryable<Comment> getCommentsQuery(Guid postID)
        {
            IQueryable<oxite_Comment> commentsByPostID =
                from c in context.oxite_Comments
                where c.PostID == postID
                select c;

            return projectComments(commentsByPostID).Select(pc => pc.Child);
        }

        private IQueryable<ParentAndChild<Page, Comment>> projectComments(IQueryable<oxite_Comment> comments)
        {
            return from c in comments
                   join p in context.oxite_Posts on c.PostID equals p.PostID
                   join pr in context.oxite_PostRelationships on p.PostID equals pr.PostID
                   orderby c.CreatedDate ascending
                   select new ParentAndChild<Page, Comment>
                   {
                       Child = new Comment
                       {
                           Body = c.Body,
                           Created = c.CreatedDate,
                           Creator = new UserBase
                           {
                               //TODO: (erikpo) Need to get these from either oxite_User or oxite_CommentAnonymous
                               /*Email = c.CreatorEmail,
                               HashedEmail = c.CreatorHashedEmail,*/
                               ID = c.CreatorUserID/*,
                               Name = c.CreatorName,
                               Url = c.CreatorUrl*/
                           },
                           CreatorIP = c.CreatorIP,
                           CreatorUserAgent = c.UserAgent,
                           ID = c.CommentID,
                           Language = new Language
                           {
                               DisplayName = c.oxite_Language.LanguageDisplayName,
                               ID = c.oxite_Language.LanguageID,
                               Name = c.oxite_Language.LanguageName
                           },
                           Modified = c.ModifiedDate,
                           State = (EntityState)c.State
                       },
                       Parent = GetPage(p.Slug, pr.ParentPostID)
                   };
        }
    }
}
