<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Oxite.Model.Page>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%
    PageListViewModel pageListViewModel = Model.GetModelItem<PageListViewModel>(); %>
       <div class="admin metadata">
            <ul>
                <li class="input page">
                    <fieldset>
                        <legend><%=Model.Localize("PageRelationship", "Page Relationship") %></legend>
                        <label for="post_parent"><%=Model.Localize("PageParentLabel", "A page under") %></label>
                        <%
                            Oxite.Model.Page parent = Model.Container as Oxite.Model.Page != null ? (Oxite.Model.Page)Model.Container : Model.Item.Parent;
                            SelectList allPages = new SelectList(
                                pageListViewModel.Pages.OrderBy(p => p.Path).Where(p => !(p.Path == Model.Item.Path || p.Path.StartsWith(Model.Item.Path + "/"))).ToList(),
                                "ID",
                                "Path",
                                !string.IsNullOrEmpty(Request.Form.Get("parentID"))
                                    ? Request.Form.Get("parentID")
                                    :  parent != null ? parent.ID.ToString() : ""
                                );

                            ((IList) allPages.Items).Insert(0, new Oxite.Model.Page { Slug = "", ID = Guid.Empty });
                             %>
                        <%= Html.DropDownList("parentID",  allPages,  new { id = "post_parent" }) %>
                    </fieldset>
                </li>
            </ul>
            <% Html.RenderPartial("ItemEditButtons"); %>
        </div>
