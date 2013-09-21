//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Hosting;
using Oxite.Model;

namespace Oxite.Mvc.Skinning
{
    public class VirtualPathProviderSkinEngine : ISkinEngine
    {
        protected static readonly string[] masterLocationFormats = new[]
        {
            "~/Skins/{2}/Views/{1}/{0}.master",
            "~/Skins/{2}/Views/Shared/{0}.master",
            "~/Views/{1}/{0}.master",
            "~/Views/Shared/{0}.master"
        };

        protected static readonly string[] viewLocationFormats = new[]
        {
            "~/Skins/{2}/Views/{1}/{0}.aspx",
            "~/Skins/{2}/Views/{1}/{0}.ascx",
            "~/Skins/{2}/Views/Shared/{0}.aspx",
            "~/Skins/{2}/Views/Shared/{0}.ascx",
            "~/Views/{1}/{0}.aspx",
            "~/Views/{1}/{0}.ascx",
            "~/Views/Shared/{0}.aspx",
            "~/Views/Shared/{0}.ascx"
        };

        protected static readonly string[] partialViewLocationFormats = new[]
        {
            "~/Skins/{2}/Views/{1}/{0}.aspx",
            "~/Skins/{2}/Views/{1}/{0}.ascx",
            "~/Skins/{2}/Views/Shared/{0}.aspx",
            "~/Skins/{2}/Views/Shared/{0}.ascx",
            "~/Views/{1}/{0}.aspx",
            "~/Views/{1}/{0}.ascx",
            "~/Views/Shared/{0}.aspx",
            "~/Views/Shared/{0}.ascx"
        };

        private readonly VirtualPathProvider virtualPathProvider;
        private readonly Site site;

        public VirtualPathProviderSkinEngine(VirtualPathProvider virtualPathProvider, Site site)
        {
            this.virtualPathProvider = virtualPathProvider;
            this.site = site;
        }

        public virtual string FindMasterPath(string name, string folderName, out string[] locationsSearched)
        {
            return FindPath(masterLocationFormats, name, folderName, out locationsSearched);
        }

        public virtual string FindViewPath(string name, string folderName, out string[] locationsSearched)
        {
            return FindPath(viewLocationFormats, name, folderName, out locationsSearched);
        }

        public virtual string FindPartialViewPath(string name, string folderName, out string[] locationsSearched)
        {
            return FindPath(partialViewLocationFormats, name, folderName, out locationsSearched);
        }

        protected string FindPath(string[] locationsToSearch, string name, string folderName, out string[] locationsSearched)
        {
            int locationCount = 0;
            locationsSearched = new string[locationsToSearch.Length];

            foreach (string locationFormat in locationsToSearch)
            {
                string path = string.Format(locationFormat, name, folderName, site.SkinDefault);
                locationsSearched[locationCount++] = path;
                if (virtualPathProvider.FileExists(path))
                {
                    return path;
                }
            }

            return null;
        }
    }
}
