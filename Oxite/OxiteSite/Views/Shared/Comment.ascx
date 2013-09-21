<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelPartial<ParentAndChild<Post, Comment>>>" %>
<%@import Namespace="Oxite.Extensions" %>
<%@import Namespace="Oxite.Model.Extensions" %> 
<%@import Namespace="Oxite.Mvc.Extensions" %>
<%@import Namespace="Oxite.Mvc.ViewModels.Extensions" %>
<%  
if (Model.PartialModel != null)
{
    Html.RenderPartial("ManageComment"); 
%><div class="contents"><%
            if (Model.RootModel.User.GetCanAccessAdmin() && Model.PartialModel.Child.State == EntityState.PendingApproval)
            {
            %><span class="state" title="<%=Model.RootModel.Localize("PendingApproval", "Pending Approval") %>"><%=Model.RootModel.Localize("PendingApproval", "Pending Approval") %></span><%
            } %>
            <div class="name" id="<%=Model.PartialModel.Child.GetSlug() %>">
                <div><%=Html.LinkOrDefault(Html.Gravatar(Model.RootModel, Model.PartialModel.Child.Creator, "48"), Model.PartialModel.Child.Creator.Url.CleanHref(), new { @class = "avatar" })%></div>
                <p class="comment">
                    <strong><%=Html.LinkOrDefault(Model.PartialModel.Child.Creator.Name.CleanText(), Model.PartialModel.Child.Creator.Url.CleanHref())%></strong>
                    <% Html.RenderPartial("CommentCreatorInfo"); %>
                </p>
            </div>
            <div class="text">
                <p><%=Model.PartialModel.Child.Body.CleanCommentBody() %></p>
            </div>
        </div><%
} %>