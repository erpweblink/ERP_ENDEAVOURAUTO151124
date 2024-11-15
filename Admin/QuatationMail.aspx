<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuatationMail.aspx.cs" Inherits="Admin_QuatationMail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:GridView ID="grd" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="Product Description">
                                    <ItemTemplate>
                                     <asp:Label ID="lbldescription" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HSN / SAC">
                                    <ItemTemplate>
                                          <asp:Label ID="lblhsn" runat="server" Text='<%# Eval("HSN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax">
                                    <ItemTemplate>
                                         <asp:Label ID="lbltax" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                          <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                          <asp:Label ID="lblUnits" runat="server" Text='<%# Eval("Units") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                         <asp:Label ID="lblrate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                          <asp:Label ID="lbldiscount" runat="server" Text='<%# Eval("total") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                         <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("total") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
        </div>
    </form>
</body>
</html>
