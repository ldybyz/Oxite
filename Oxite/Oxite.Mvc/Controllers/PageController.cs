//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.Extensions;
using Oxite.Mvc.ViewModels;
using Oxite.Routing;
using Oxite.Services;
using Oxite.Validation;

namespace Oxite.Mvc.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService pageService;

        public PageController(IPageService pageService)
        {
            this.pageService = pageService;
            ValidateRequest = false;
        }

        public virtual OxiteModelItem<Page> Item(string pagePath)
        {
            Page page = pageService.GetPage(pagePath);

            if (page == null) return null;

            return new OxiteModelItem<Page>
            {
                Item = page
            };
        }

        //todo: (nheskew) need a model binder or filter to clean up pagePath instead of using substring in all the actions
        [ActionName("ItemAdd"), AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModelItem<Page> Add(string pagePath)
        {
            Page page = !string.IsNullOrEmpty(pagePath)
                ? pageService.GetPage(pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Add).Length))
                : new Page();

            if (page == null) return null;

            return new OxiteModelItem<Page>
            {
                Container = page,
                Item = new Page()
            };
        }

        [ActionName("ItemAdd"), AcceptVerbs(HttpVerbs.Post)]
        public virtual object SaveAdd(string pagePath, Page pageInput, User currentUser)
        {
            string parentPagePath = !string.IsNullOrEmpty(pagePath)
                ? pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Add).Length)
                : "";

            Page parentPage = pageService.GetPage(parentPagePath);

            ValidationStateDictionary validationState;
            Page newPage;

            pageService.AddPage(pageInput, parentPage, currentUser, out validationState, out newPage);

            if (!validationState.IsValid)
            {
                ModelState.AddModelErrors(validationState);

                return Add(pagePath);
            }

            return Redirect(Url.Page(newPage));
        }

        [ActionName("ItemEdit"), AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModelItem<Page> Edit(string pagePath)
        {
            Page page = pageService.GetPage(pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Edit).Length));

            if (page == null) return null;

            return new OxiteModelItem<Page>
            {
                Container = page.Parent,
                Item = page
            };
        }

        [ActionName("ItemEdit"), AcceptVerbs(HttpVerbs.Post)]
        public virtual object SaveEdit(string pagePath, Page pageInput, User currentUser)
        {
            string pageToEditPath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Edit).Length);

            if (pageToEditPath.Length == pagePath.Length) return null;

            Page page = pageService.GetPage(pageToEditPath);

            if (page == null) return null;

            ValidationStateDictionary validationState;

            pageService.EditPage(page, pageInput.Parent, pageInput, out validationState);

            if (!validationState.IsValid)
            {
                ModelState.AddModelErrors(validationState);

                return Edit(pageToEditPath);
            }

            page = pageService.GetPage(pageInput.Slug, pageInput.Parent.ID);

            return Redirect(Url.AppPath(page.Path));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Remove(string pagePath, string returnUri)
        {
            string pageRemovePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Remove).Length);

            if (pageRemovePath.Length == pagePath.Length) return null;

            Page page = pageService.GetPage(pageRemovePath);

            pageService.RemovePage(page);

            return Redirect(returnUri);
        }
    }
}