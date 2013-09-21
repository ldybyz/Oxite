//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using Oxite.Mvc.Controllers;
using Oxite.Mvc.Tests.Fakes;
using Xunit;
using Oxite.Model;

namespace Oxite.Mvc.Tests.Controllers
{
    public class AreaControllerTests
    {
        [Fact]
        public void RsdReturnsValidRsdDoc()
        {
            FakeAreaService areaService = new FakeAreaService();

            areaService.StoredAreas.Add("test", new Oxite.Model.Area() { ID = Guid.NewGuid(), Name = "test" });

            RouteCollection routes = new RouteCollection();

            routes.Add("Posts", new Route("", new MvcRouteHandler()));

            UrlHelper helper = new UrlHelper(new RequestContext(new FakeHttpContext(new Uri("http://oxite.net/"),"~/"), new RouteData()), routes);

            Site site = new Site() { Host = new Uri("http://oxite.net") };

            AreaController controller = new AreaController(site, areaService, null, null, null, null) { Url = helper };

            ContentResult result = controller.Rsd("test");

            Assert.NotNull(result);

            XDocument rsdDoc = XDocument.Parse(result.Content);
            XNamespace rsdNamespace = "http://archipelago.phrasewise.com/rsd";
            XElement rootElement = rsdDoc.Element(rsdNamespace + "rsd");

            Assert.NotNull(rootElement);
            Assert.NotNull(rootElement.Attribute("version"));
            Assert.Equal("1.0", rootElement.Attribute("version").Value);
            Assert.Equal("Oxite", rootElement.Descendants(rsdNamespace + "engineName").SingleOrDefault().Value);
            Assert.Equal("http://oxite.net", rootElement.Descendants(rsdNamespace + "engineLink").SingleOrDefault().Value);
            Assert.Equal("http://oxite.net/", rootElement.Descendants(rsdNamespace + "homePageLink").SingleOrDefault().Value);

            XElement apisElement = rootElement.Descendants(rsdNamespace + "apis").SingleOrDefault();
            Assert.NotNull(apisElement);
            Assert.Equal(1, apisElement.Elements().Count());

            XElement apiElement = apisElement.Elements().SingleOrDefault();
            Assert.NotNull(apiElement);
            Assert.Equal(rsdNamespace + "api", apiElement.Name);
            Assert.Equal("MetaWeblog", apiElement.Attribute("name").Value);
            Assert.Equal(areaService.StoredAreas["test"].ID.ToString("N"), apiElement.Attribute("blogID").Value);
            Assert.Equal("true", apiElement.Attribute("preferred").Value);
            Assert.Equal("http://oxite.net/MetaWeblog.svc", apiElement.Attribute("apiLink").Value);
        }

        [Fact]
        public void RsdReturnsXmlContentType()
        {
            FakeAreaService areaService = new FakeAreaService();

            areaService.StoredAreas.Add("test", new Oxite.Model.Area() { ID = Guid.NewGuid(), Name = "test" });

            RouteCollection routes = new RouteCollection();

            routes.Add("Posts", new Route("", new MvcRouteHandler()));

            UrlHelper helper = new UrlHelper(new RequestContext(new FakeHttpContext(new Uri("http://oxite.net/"), "~/"), new RouteData()), routes);

            Site site = new Site() { Host = new Uri("http://oxite.net") };

            AreaController controller = new AreaController(site, areaService, null, null, null, null) { Url = helper };

            ContentResult result = controller.Rsd("test");

            Assert.Equal("text/xml", result.ContentType);
        }

        [Fact]
        public void RsdHasSiteAbsoluteUrlWhenPathInSiteHost()
        {
            FakeAreaService areaService = new FakeAreaService();

            areaService.StoredAreas.Add("test", new Oxite.Model.Area() { ID = Guid.NewGuid(), Name = "test" });

            RouteCollection routes = new RouteCollection();

            routes.Add("Posts", new Route("", new MvcRouteHandler()));

            UrlHelper helper = new UrlHelper(new RequestContext(new FakeHttpContext(new Uri("http://oxite.net/blog"), "~/"), new RouteData()), routes);

            Site site = new Site() { Host = new Uri("http://oxite.net/blog") };

            AreaController controller = new AreaController(site, areaService, null, null, null, null) { Url = helper };

            ContentResult result = controller.Rsd("test");

            XDocument rsdDoc = XDocument.Parse(result.Content);
            XNamespace rsdNamespace = "http://archipelago.phrasewise.com/rsd";
            XElement rootElement = rsdDoc.Element(rsdNamespace + "rsd");
            Assert.Equal("http://oxite.net/blog/MetaWeblog.svc", rootElement.Descendants(rsdNamespace + "api").First().Attribute("apiLink").Value);
        }
    }
}
