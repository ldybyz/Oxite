<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
                    <div class="sub search">
                        <form id="search" method="get" action="<%=Url.Search() %>">
                            <fieldset>
                                <label for="search_term"><%=Model.Localize("SearchInputTitle", "Search this site...")%></label>
                                <%= Html.TextBox("term", "", new { id = "search_term", @class = "text", title = Model.Localize("SearchInputTitle", "Search this site...") })%>
                                <input type="submit" value="<%=Model.Localize("Search") %>" class="button" />
                            </fieldset>
                        </form>
                    </div>