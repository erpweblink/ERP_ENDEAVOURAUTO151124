<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoicepdf.aspx.cs" Inherits="Invoicepdf" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- CSS only -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous">
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="updatepnl" runat="server">
            <ContentTemplate>
                <div>
                    <div class="container-fluid">
                        <div class="row align-items-center">
                            <div class="col-md-2" style="margin-top: 15px;"></div>
                            <div class="col-md-10" style="margin-top: 10px;">
                                <asp:CheckBox ID="Chk_ORIGINAL" runat="server" Text="&nbsp; ORIGINAL FOR BUYER" />
                                <asp:CheckBox ID="Chk_DUPLICATE" runat="server" Text="&nbsp; DUPLICATE FOR TRANSPORTER" />
                                <asp:CheckBox ID="Chk_TRIPLICATE" runat="server" Text="&nbsp; TRIPLICATE FOR SUPPLIER" />
                                <asp:CheckBox ID="Chk_EXTRA" runat="server" Text="&nbsp; EXTRA COPY" />

                                <asp:Button runat="server" ID="btnprint" Text="View" CssClass="btn btn-primary" OnClick="btnprint_Click" />
                            </div>
                            <div class="col-md-2" style="margin-top: 10px;"></div>
                        </div>
                    </div>

                    <iframe id="ifrRight6" runat="server" enableviewstate="false" scrolling="auto" style="width: 100%; -ms-zoom: 0.75; height: 685px;"></iframe>
                </div>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btnOriginal" EventName="Click" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
