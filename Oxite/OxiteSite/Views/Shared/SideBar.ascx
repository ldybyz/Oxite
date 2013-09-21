<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModel>" %>
<% 
    Html.RenderPartial("Search");
    Html.RenderPartial("Archives", new OxiteModelPartial<ArchiveViewModel>(Model, Model.GetModelItem<ArchiveViewModel>())); %>