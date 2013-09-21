<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%
    AreaListViewModel areaListViewModel = Model.GetModelItem<AreaListViewModel>();

    bool urlIsLocked = Model.Item.State == EntityState.Normal
        && Model.Item.Published.HasValue
        && Model.Item.Published.Value.AddHours(Model.Site.PostEditTimeout) < DateTime.Now.ToUniversalTime();
 %>       <div class="admin metadata">
            <ul class="<%=Model.Site.HasMultipleAreas ? "multi" : "single" %>"><%
                if (Model.Site.HasMultipleAreas) { %>
                <li class="input area">
                    <fieldset>
                        <legend><%=Model.Localize("Area Relationship") %></legend>
                        <label for="post_area"><%=Model.Localize("In") %></label>
                        <%=Html.DropDownList(
                                "areaID",          
                                new SelectList(areaListViewModel.Areas, "ID", "Name", Model.Item != null && Model.Item.Area != null ? Model.Item.Area.ID.ToString() : null),
                                new { id = "post_area" },
                                !urlIsLocked
                                ) %>
                    </fieldset>
                </li><%
                } %>
                <li class="input tags">
                    <label for="post_tags"><%=Model.Localize("Tags") %></label> <%=Html.ValidationMessage("Post.Tags")%>
                    <%= Html.TextBox(
                            "tags", 
                            Model.Item != null ? string.Join(", ", Model.Item.Tags.Select((tag, name) => tag.Name).ToArray()) : string.Empty, 
                            new { id = "post_tags", @class="text", size = "60", title = Model.Localize("post_tags", "Enter comma-delimited list of tags...") }
                            ) %>
                </li>
            </ul>
            <% Html.RenderPartial("ItemEditButtons"); %>
        </div>
