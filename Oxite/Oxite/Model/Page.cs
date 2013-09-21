//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model
{
    public class Page : PostBase
    {
        public Page Parent { get; set; }
        public bool HasChildren { get; set; }

        private string path;
        public string Path
        {
            get
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = string.Format(
                        "{0}/{1}", 
                        Parent != null && !string.IsNullOrEmpty(Parent.Path) && Parent.Path != "/"
                            ? Parent.Path 
                            : "",
                        Slug
                        );
                }

                return path;
            }
        }
    }
}
