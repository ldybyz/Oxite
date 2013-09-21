//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Model
{
    public class Site
    {
        public IList<Uri> HostRedirects { get; set; }
        public Uri Host { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string LanguageDefault { get; set; }
        public double TimeZoneOffset { get; set; }
        public string PageTitleSeparator { get; set; }
        public string FavIconUrl { get; set; }
        public string ScriptsPath { get; set; }
        public string CssPath { get; set; }
        public string CommentStateDefault { get; set; }
        public bool IncludeOpenSearch { get; set; }
        public bool AuthorAutoSubscribe { get; set; }
        public short PostEditTimeout { get; set; }
        public string GravatarDefault { get; set; }
        public string SkinDefault { get; set; }
        public bool HasMultipleAreas { get; set; }
        public string RouteUrlPrefix { get; set; }
        public bool CommentingDisabled { get; set; }
    }
}
