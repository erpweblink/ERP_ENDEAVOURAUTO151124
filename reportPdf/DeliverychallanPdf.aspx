<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeliverychallanPdf.aspx.cs" Inherits="DeliverychallanPdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div class="row">
                <br />

                <div class="col-3">
   
                    <asp:Button ID="btn_pdf" runat="server" OnClick="btn_pdf_Click" Text="Challan Without Total" CssClass="btn btn-primary" />
                    &nbsp
                      <asp:Button ID="btn_withtotal_pdf" OnClick="btn_withtotal_pdf_Click" runat="server" Text="Challan With Total" CssClass="btn btn-primary" />
                </div>
                <br />
            </div>

            <iframe id="ifrRight6" runat="server" enableviewstate="false" scrolling="auto" style="width: 100%; -ms-zoom: 0.75; height: 685px;"></iframe>
        </div>
    </form>
</body>
</html>
