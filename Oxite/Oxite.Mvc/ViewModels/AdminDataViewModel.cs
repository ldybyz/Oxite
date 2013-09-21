//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using Oxite.Model;

namespace Oxite.Mvc.ViewModels
{
    public class AdminDataViewModel
    {
        public AdminDataViewModel(IList<Post> posts, IList<ParentAndChild<PostBase, Comment>> comments, IList<Area> areas)
        {
            Posts = posts;
            Comments = comments;
            Areas = areas;
        }

        public IList<Post> Posts { get; private set; }
        public IList<ParentAndChild<PostBase, Comment>> Comments { get; private set; }
        public IList<Area> Areas { get; private set; }
    }
}
