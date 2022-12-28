<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportsViewer.aspx.cs" Inherits="SchoolManagement.Reports.ReportsViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" style="width:42%"></rsweb:ReportViewer>
            
        </div>
    </form>
</body>
</html>
