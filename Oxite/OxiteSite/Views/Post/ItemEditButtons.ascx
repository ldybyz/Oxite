<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"%>
            <div class="admin buttons">
                <input type="submit" value="<%=Model.Localize("Save") %>" class="button submit" tabindex="" />
                <%=Html.Button(
                    "cancel",
                    Model.Localize("Cancel"),
                    new { @class = "cancel", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Model.Localize("really?"), Model.Item != null ? Url.Post(Model.Item) : Url.Home()) }
                    )%>
                <%=Html.Link(
                    Model.Localize("Cancel"),
                    Model.Item != null ? Url.Post(Model.Item) : Url.Home(),
                    new { @class = "cancel" })%>
            </div>