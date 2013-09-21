//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Model;
using Oxite.Validation;

namespace Oxite.Services
{
    public interface IPageService
    {
        Page GetPage(string name);
        Page GetPage(string slug, Guid parentID);
        //string GetPagePath(Page page);
        IList<Page> GetPages();
        IList<Page> GetChildren(Page parent);

        void AddPage(Page page, User creator, out ValidationStateDictionary validationState, out Page newPage);
        void AddPage(Page page, Page parent, User creator, out ValidationStateDictionary validationState, out Page newPage);
        void EditPage(Page page, Page pageEdits, out ValidationStateDictionary validationState);
        void EditPage(Page page, Page parent, Page pageEdits, out ValidationStateDictionary validationState);
        //todo: (nheskew) need to consolidate
        ValidationStateDictionary AddPage(Page page);
        //todo: (nheskew) need to consolidate
        ValidationStateDictionary AddPage(Page page, Page parent);
        //todo: (nheskew) need to consolidate
        ValidationStateDictionary EditPage(Page page);
        //todo: (nheskew) need to consolidate
        ValidationStateDictionary EditPage(Page page, Page parent);
        void RemovePage(Page page);
    }
}
