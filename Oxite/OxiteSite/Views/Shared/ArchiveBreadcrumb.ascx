<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelList<Post>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<% 
    ArchiveData archiveData = ((ArchiveContainer)Model.Container).ArchiveData;
    
%><%=Model.Localize("Archives") 
%> / <%=Html.Link(archiveData.Year.ToString(), Url.Posts(archiveData.Year))
%><%=archiveData.Month > 0 ? string.Format(" / {0}", Html.Link(archiveData.ToDateTime().ToString("MMMM"), Url.Posts(archiveData.Year, archiveData.Month))) : ""
%><%=archiveData.Day > 0 ? string.Format(" / {0}", Html.Link(archiveData.Day.ToString(), Url.Posts(archiveData.Year, archiveData.Month, archiveData.Day))) : ""
%>