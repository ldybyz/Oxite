<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelPartial<ParentAndChild<Post, Comment>>>" 
%><%@import Namespace="Oxite.Model.Extensions" %> 
<%@import Namespace="Oxite.Mvc.Extensions" %><%
if (Model.PartialModel.Child.Created.HasValue)
{ 
    %> <span><%=Model.RootModel.Localize("said") %><br /><%= 
        Html.Link(
            Html.ConvertToLocalTime(Model.PartialModel.Child.Created.Value, Model.RootModel).ToString("MMMM dd, yyyy"), //todo: (nheskew) localize date format
            Url.Comment(Model.PartialModel.Parent, Model.PartialModel.Child)
            )%></span><%
} %>