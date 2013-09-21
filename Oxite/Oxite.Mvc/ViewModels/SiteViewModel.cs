//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using Oxite.Model;

namespace Oxite.Mvc.ViewModels
{
    public class SiteViewModel
    {
        public SiteViewModel(Site currentSite, string configSiteName)
        {
            ID = currentSite.ID;
            DisplayName = currentSite.DisplayName ?? "My Oxite Site";
            Description = currentSite.Description;
            PageTitleSeparator = currentSite.PageTitleSeparator ?? " - ";
            TimeZoneOffset = currentSite.TimeZoneOffset;
            ScriptsPath = currentSite.ScriptsPath ?? "~/Skins/{0}/Scripts";
            CssPath = currentSite.CssPath ?? "~/Skins/{0}/Styles";
            IncludeOpenSearch = currentSite.IncludeOpenSearch;
            LanguageDefault = currentSite.LanguageDefault;
            GravatarDefault = currentSite.GravatarDefault;
            PostEditTimeout = currentSite.PostEditTimeout;
            SkinDefault = currentSite.SkinDefault ?? "Default";
            FavIconUrl = currentSite.FavIconUrl ?? "~/Content/icons/flame.ico";
            HasMultipleAreas = currentSite.HasMultipleAreas;
            CommentingDisabled = currentSite.CommentingDisabled;
        }

        internal Guid ID { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public string PageTitleSeparator { get; private set; }
        public double TimeZoneOffset { get; private set; }
        public string ScriptsPath { get; private set; }
        public string CssPath { get; private set; }
        public bool IncludeOpenSearch { get; set; }
        public string LanguageDefault { get; set; }
        public string GravatarDefault { get; set; }
        public short PostEditTimeout { get; set; }
        public string SkinDefault { get; set; }
        public string FavIconUrl { get; set; }
        public bool HasMultipleAreas { get; set; }
        public bool CommentingDisabled { get; set; }
    }
}
