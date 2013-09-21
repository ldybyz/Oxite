<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Oxite.Model.Page>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"%>
            <div class="admin buttons">
                <input type="submit" value="<%=Model.Localize("Save") %>" class="button submit" tabindex="" />
                <%=Html.Button(
                    "cancel",
                    Model.Localize("Cancel"),
                    new { @class = "cancel", tabindex = "", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Model.Localize("really?"), Model.Item != null ? Url.Page(Model.Item.ID != Guid.Empty ? Model.Item : Model.Container as Oxite.Model.Page) : Url.Home()) }
                    )%>
                <%=Html.Link(
                    Model.Localize("Cancel"),
                    Model.Item != null ? Url.Page(Model.Item.ID != Guid.Empty ? Model.Item : Model.Container as Oxite.Model.Page) : Url.Home(),
                    new { @class = "cancel", tabindex = "" })%>
            </div>