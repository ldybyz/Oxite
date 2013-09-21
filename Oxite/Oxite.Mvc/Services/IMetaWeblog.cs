//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Oxite.Mvc.Services
{
    [ServiceContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public interface IMetaWeblog
    {
        [OperationContract(Action="metaWeblog.newPost")]
        string NewPost(string blogId, string username, string password, Post post, bool publish);

        [OperationContract(Action = "metaWeblog.editPost")]
        bool EditPost(string postId, string username, string password, Post post, bool publish);

        [OperationContract(Action = "metaWeblog.getPost")]
        Post GetPost(string postId, string username, string password);

        [OperationContract(Action = "metaWeblog.newMediaObject")]
        UrlData NewMediaObject(string blogId, string username, string password, FileData file);

        [OperationContract(Action = "metaWeblog.getCategories")]
        CategoryInfo[] GetCategories(string blogId, string username, string password);

        [OperationContract(Action = "metaWeblog.getRecentPosts")]
        Post[] GetRecentPosts(string blogId, string username, string password, int numberOfPosts);

        [OperationContract(Action = "blogger.getUsersBlogs")]
        BlogInfo[] GetUsersBlogs(string apikey, string username, string password);

        [OperationContract(Action = "blogger.deletePost")]
        bool DeletePost(string appkey, string postid, string username, string password, bool publish);
    }

    #region Data Contracts
    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public class Post
    {
        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string link { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string[] categories { get; set; }

        [DataMember]
        public DateTime dateCreated { get; set; }

        [DataMember]
        public string permalink { get; set; }

        [DataMember]
        public string postid { get; set; }

        [DataMember]
        public string userid { get; set; }

        [DataMember]
        public string mt_excerpt { get; set; }

        [DataMember]
        public string mt_basename { get; set; }

        [DataMember]
        public string wp_author { get; set; }

        [DataMember]
        public string wp_author_id { get; set; }

        [DataMember]
        public string wp_author_display_name { get; set; }
    }

    [DataContract]
    public struct UrlData
    {
        [DataMember]
        public string url { get; set; }
    }

    [DataContract]
    public struct FileData
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public byte[] bits { get; set; }
    }

    [DataContract]
    public struct CategoryInfo
    {
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string htmlUrl { get; set; }
        [DataMember]
        public string rssUrl { get; set; }
    }

    [DataContract]
    public struct BlogInfo
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string blogid { get; set; }
        [DataMember]
        public string blogName { get; set; }
    }

    [DataContract]
    public struct NewCategoryInfo
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string slug { get; set; }

        [DataMember]
        public int parent_id { get; set; }

        [DataMember]
        public string description { get; set; }
    }

    [DataContract]
    public struct AuthorInfo
    {
        [DataMember]
        public string user_id { get; set; }
        [DataMember]
        public string user_login { get; set; }
        [DataMember]
        public string display_name { get; set; }
        [DataMember]
        public string user_email { get; set; }
        [DataMember]
        public string meta_value { get; set; }
    }
    #endregion
}
