<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<methodResponse>
    <params>
        <param>
            <value>
                <string>Pingback from <%=ViewData["SourceUrl"] %> to <%=ViewData["TargetUrl"] %> registered. Keep the web talking! :-)</string>
            </value>
        </param>
    </params>
</methodResponse>