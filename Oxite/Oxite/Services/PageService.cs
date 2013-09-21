//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;
using Oxite.Validation;

namespace Oxite.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository repository;
        private readonly IValidationService validator;

        public PageService(IPageRepository repository, IValidationService validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        #region IPageService Members

        public Page GetPage(string name)
        {
            Page page = null;
            Guid parentPostID = Guid.Empty;
            string[] pagePathParts = name.Split('/');

            for (int i = 0; i < pagePathParts.Length; i++)
            {
                page = repository.GetPage(pagePathParts[i], parentPostID);

                if (page != null)
                {
                    parentPostID = page.ID;
                }
                else
                {
                    break;
                }
            }

            return page;
        }

        public Page GetPage(string slug, Guid parentID)
        {
            return repository.GetPage(slug, parentID);
        }

        public IList<Page> GetPages()
        {
            return repository.GetPages().ToList();
        }

        public IList<Page> GetChildren(Page parent)
        {
            return repository.GetChildren(parent).ToList();
        }

        public void AddPage(Page page, User creator, out ValidationStateDictionary validationState, out Page newPage)
        {
            AddPage(page, null, creator, out validationState, out newPage);
        }

        public void AddPage(Page page, Page parent, User creator, out ValidationStateDictionary validationState, out Page newPage)
        {
            validationState = new ValidationStateDictionary();

            page.Parent = parent ?? new Page();
            page.Creator = creator;

            validationState.Add(typeof(Page), validator.Validate(page));

            if (!validationState.IsValid)
            {
                newPage = null;

                return;
            }

            repository.Save(page);

            newPage = repository.GetPage(page.Slug, page.Parent.ID);
        }

        public void EditPage(Page page, Page pageEdits, out ValidationStateDictionary validationState)
        {
            EditPage(page, null, pageEdits, out validationState);
        }

        public void EditPage(Page page, Page parent, Page pageEdits, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            pageEdits.ID = page.ID;
            pageEdits.Creator = page.Creator;
            pageEdits.Created = page.Created;
            pageEdits.State = page.State;

            pageEdits.Parent = parent;

            validationState.Add(typeof(Page), validator.Validate(page));

            repository.Save(pageEdits);
        }

        //todo: (nheskew) need to consolidate
        public ValidationStateDictionary AddPage(Page page)
        {
            return AddPage(page, null);
        }

        //todo: (nheskew) need to consolidate
        public ValidationStateDictionary AddPage(Page page, Page parent)
        {
            page.Parent = parent;

            repository.Save(page);

            return null;
        }

        //todo: (nheskew) need to consolidate
        public ValidationStateDictionary EditPage(Page page)
        {
            return EditPage(page, null);
        }

        //todo: (nheskew) need to consolidate
        public ValidationStateDictionary EditPage(Page page, Page parent)
        {
            page.Parent = parent;

            repository.Save(page);

            return null;
        }

        public void RemovePage(Page page)
        {
            repository.Remove(page);
        }

        #endregion
    }
}
