//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BlogML.Xml;
using Oxite.Extensions;
using Oxite.Model;

namespace Oxite.Mvc.Extensions
{
    public static class BlogMLExtensions
    {
        public static Post ToPost(this BlogMLPost blogMLPost, BlogMLBlog blog, User user, string slugPattern)
        {
            Post post = new Post();

            post.Body = blogMLPost.Content.Text;
            post.Creator = user;
            post.Created = blogMLPost.DateCreated;
            post.Modified = blogMLPost.DateModified;
            post.Published = blogMLPost.DateCreated;
            if (!string.IsNullOrEmpty(slugPattern))
            {
                Regex regex = new Regex(slugPattern);
                Match match = regex.Match(blogMLPost.PostUrl);

                if (match != null && match.Groups != null && match.Groups.Count >= 2)
                {
                    post.Slug = match.Groups[1].Value;
                }
                else
                {
                    post.Slug = blogMLPost.ID;
                }
            }
            else
            {
                post.Slug = !string.IsNullOrEmpty(blogMLPost.PostUrl)
                    ? blogMLPost.PostUrl
                    : blogMLPost.ID;
            }
            post.Title = blogMLPost.Title;

            post.BodyShort = blogMLPost.HasExcerpt
                ? blogMLPost.Excerpt.Text
                : "";

            post.State = blogMLPost.Approved
                ? EntityState.Normal
                : EntityState.PendingApproval;

            post.CommentingDisabled = false;

            post.Tags = new List<Tag>();

            if (blogMLPost.Categories != null && blogMLPost.Categories.Count > 0)
            {
                foreach (BlogMLCategoryReference bcr in blogMLPost.Categories)
                {
                    foreach (BlogMLCategory tag in blog.Categories)
                    {
                        if (tag.ID == bcr.Ref)
                        {
                            post.Tags.Add(new Tag { ID = Guid.NewGuid(), Name = tag.Title, Created = tag.DateCreated });

                            break;
                        }
                    }
                }
            }

            post.Trackbacks = new List<Trackback>();

            if (blogMLPost.Trackbacks != null && blogMLPost.Trackbacks.Count > 0)
            {
                foreach (BlogMLTrackback tb in blogMLPost.Trackbacks)
                {
                    Trackback trackback = new Trackback
                    {
                        Created = tb.DateCreated,
                        Modified = tb.DateModified,
                        Title = tb.Title,
                        Url = tb.Url,
                        IsTargetInSource = tb.Approved,
                        BlogName = "",
                        Body = "",
                        Source = ""
                    };

                    post.Trackbacks.Add(trackback);
                }
            }

            return post;
        }

        public static Comment ToComment(this BlogMLComment blogMLComment, BlogMLBlog blog, User user, Language language)
        {
            Comment comment = new Comment();

            comment.Body = blogMLComment.Content.Text;
            comment.Created = blogMLComment.DateCreated;
            comment.Modified = blogMLComment.DateModified;
            comment.Language = language;
            comment.CreatorIP = 0;
            comment.CreatorUserAgent = "";

            comment.State = blogMLComment.Approved
                ? EntityState.Normal
                : EntityState.PendingApproval;

            if (blogMLComment.UserEMail == user.Email || blogMLComment.UserEMail == blog.Authors[0].Email)
            {
                comment.Creator = user;
            }
            else
            {
                comment.Creator = new UserBase();

                if (!string.IsNullOrEmpty(blogMLComment.UserEMail))
                {
                    comment.Creator.Email = blogMLComment.UserEMail.Length > 100
                        ? blogMLComment.UserEMail.Substring(0, 100)
                        : blogMLComment.UserEMail;

                    comment.Creator.HashedEmail = comment.Creator.Email.ComputeHash();
                }
                else
                {
                    comment.Creator.Email = comment.Creator.HashedEmail = "";
                }

                if (!string.IsNullOrEmpty(blogMLComment.UserName))
                {
                    comment.Creator.Name = blogMLComment.UserName.Length > 50
                        ? blogMLComment.UserName.Substring(0, 50)
                        : blogMLComment.UserName;
                }
                else
                {
                    comment.Creator.Name = "";
                }

                if (!string.IsNullOrEmpty(blogMLComment.UserUrl))
                {
                    comment.Creator.Url = blogMLComment.UserUrl.Length > 300
                        ? blogMLComment.UserUrl.Substring(0, 300)
                        : blogMLComment.UserUrl;
                }
                else
                {
                    comment.Creator.Url = "";
                }
            }

            return comment;
        }
    }
}
