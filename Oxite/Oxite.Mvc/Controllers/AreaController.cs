//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using BlogML.Xml;
using Oxite.Model;
using Oxite.Mvc.Extensions;
using Oxite.Mvc.ViewModels;
using Oxite.Services;
using Oxite.Validation;
using Oxite.Routing;

namespace Oxite.Mvc.Controllers
{
    public class AreaController : Controller
    {
        private readonly Site site;
        private readonly IAreaService areaService;
        private readonly IPostService postService;
        private readonly ILanguageService languageService;
        private readonly ISiteService siteService;
        private readonly AbsolutePathHelper absolutePathHelper;

        public AreaController(Site site, IAreaService areaService, IPostService postService, ILanguageService languageService, ISiteService siteService, AbsolutePathHelper absolutePathHelper)
        {
            this.site = site;
            this.areaService = areaService;
            this.postService = postService;
            this.languageService = languageService;
            this.siteService = siteService;
            this.absolutePathHelper = absolutePathHelper;
        }

        public virtual ContentResult Rsd(string areaName)
        {
            return Content(GenerateRsd(areaName).ToString(), "text/xml");
        }

        protected virtual XDocument GenerateRsd(string areaName)
        {
            XNamespace rsdNamespace = "http://archipelago.phrasewise.com/rsd";
            XDocument rsd = new XDocument(
                new XElement(rsdNamespace + "rsd", new XAttribute("version", "1.0"),
                             new XElement(rsdNamespace + "service",
                                          new XElement(rsdNamespace + "engineName", "Oxite"),
                                          new XElement(rsdNamespace + "engineLink", Url.Oxite()),
                                          new XElement(rsdNamespace + "homePageLink", Url.AbsolutePath(Url.Home())),
                                          new XElement(rsdNamespace + "apis",
                                                       GenerateRsdApiList(areaName, rsdNamespace)
                                 )
                    )
                ));

            return rsd;
        }

        protected virtual XElement[] GenerateRsdApiList(string areaName, XNamespace rsdNamespace)
        {
            IList<Area> areas = areaService.GetAreas();
            List<XElement> elements = new List<XElement>(areas.Count);

            string apiLink = new UriBuilder(site.Host) { Path = Url.MetaWeblogApi() }.Uri.ToString();

            foreach (Area area in areas)
            {
                elements.Add(
                    new XElement(
                        rsdNamespace + "api",
                        new XAttribute("name", "MetaWeblog"),
                        new XAttribute("blogID", area.ID.ToString("N")),
                        new XAttribute(
                            "preferred",
                            (areas.Count == 1 || string.Compare(area.Name, areaName, true) == 0).ToString().ToLower()
                            ),
                        new XAttribute("apiLink", apiLink)
                        )
                    );
            }

            return elements.ToArray();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModelList<Area> Find()
        {
            return new OxiteModelList<Area>();
        }

        [ActionName("Find"), AcceptVerbs(HttpVerbs.Post)]
        public virtual OxiteModelList<Area> FindQuery(AreaSearchCriteria searchCriteria)
        {
            IList<Area> foundAreas = areaService.FindAreas(searchCriteria);
            OxiteModelList<Area> model = new OxiteModelList<Area> { List = foundAreas };

            model.AddModelItem(searchCriteria);

            return model;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModelItem<Area> ItemEdit(Area areaInput)
        {
            Area area = areaInput.ID != Guid.Empty ? areaService.GetArea(areaInput.ID) : null;

            return new OxiteModelItem<Area> { Item = area };
        }

        [ActionName("ItemEdit"), AcceptVerbs(HttpVerbs.Post)]
        public virtual object ItemSave(Area areaInput)
        {
            ValidationStateDictionary validationState;

            if (areaInput.ID == Guid.Empty)
            {
                Area area;
                areaService.AddArea(areaInput, out validationState, out area);

                //TODO: (erikpo) Get rid of HasMultipleAreas and make it a calculated field so the following isn't necessary
                Site siteToEdit = siteService.GetSite(site.Name);

                siteToEdit.HasMultipleAreas = true;

                siteService.EditSite(siteToEdit, out validationState);

                if (validationState.IsValid)
                    ((OxiteApplication)HttpContext.ApplicationInstance).ReloadSite();
            }
            else
                areaService.EditArea(areaInput, out validationState);

            if (!validationState.IsValid)
            {
                ModelState.AddModelErrors(validationState);

                return ItemEdit(areaInput);
            }

            return Redirect(Url.Admin());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModel BlogML(Area areaInput)
        {
            //TODO: (erikpo) Change this to be user selectable in the multiple blogs case if the user is a site owner
            Area area = areaService.GetArea(areaInput.Name);

            if (area == null) return null;

            return new OxiteModel { Container = area };
        }

        [ActionName("BlogML"), AcceptVerbs(HttpVerbs.Post)]
        public virtual OxiteModel BlogMLSave(Area areaInput, string slugPattern, User currentUser)
        {
            //TODO: (erikpo) Change this to be user selectable in the multiple blogs case if the user is a site owner
            Area area = areaService.GetArea("Blog");

            if (area == null) return null;

            ValidationStateDictionary validationState = new ValidationStateDictionary();
            XmlTextReader reader = null;
            bool modifiedSite = false;

            try
            {
                reader = new XmlTextReader(Request.Files[0].InputStream);

                BlogMLBlog blog = BlogMLSerializer.Deserialize(reader);
                Language language = languageService.GetLanguage(site.LanguageDefault);

                area.Description = blog.SubTitle;
                area.DisplayName = blog.Title;
                area.Created = blog.DateCreated;
                areaService.EditArea(area, out validationState);

                if (!site.HasMultipleAreas)
                {
                    site.DisplayName = area.DisplayName;
                    site.Description = area.Description;

                    siteService.EditSite(site, out validationState);

                    if (!validationState.IsValid)
                    {
                        throw new Exception();
                    }

                    modifiedSite = true;
                }

                postService.RemoveAll(area);

                foreach (BlogMLPost blogMLPost in blog.Posts)
                {
                    if (string.IsNullOrEmpty(blogMLPost.Title) || string.IsNullOrEmpty(blogMLPost.Content.Text))
                        continue;

                    Post post = blogMLPost.ToPost(blog, currentUser, slugPattern);

                    postService.AddPostWithoutTrackbacks(area, post, currentUser, out validationState, out post);

                    if (!validationState.IsValid)
                    {
                        throw new Exception();
                    }

                    foreach (BlogMLComment blogMLComment in blogMLPost.Comments)
                    {
                        Comment comment = blogMLComment.ToComment(blog, currentUser, language);

                        postService.ValidateComment(comment, out validationState);

                        if (validationState.IsValid)
                        {
                            postService.AddCommentWithoutMessages(area, post, comment, comment.Creator, false, out validationState, out comment);

                            if (!validationState.IsValid)
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelErrors(validationState);

                if (!string.IsNullOrEmpty(ex.Message))
                {
                    ModelState.AddModelError("ModelName", ex);
                }

                return BlogML(areaInput);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            if (modifiedSite)
            {
                ((OxiteApplication)HttpContext.ApplicationInstance).ReloadSite();
            }

            return new OxiteModel { Container = area };
        }
    }
}