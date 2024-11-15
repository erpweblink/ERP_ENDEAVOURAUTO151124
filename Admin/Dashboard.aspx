<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Admin_Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .card-bg {
            background-color: #0755a1;
        }

        .lblcount {
            color: white !important;
        }

        .mar {
            margin-left: 10px;
        }
    </style>
    <style type="text/css">
        .paging {
        }

            .paging a {
                background-color: #0755A1;
                padding: 1px 7px;
                text-decoration: none;
                border: 1px solid #0755A1;
            }

                .paging a:hover {
                    background-color: #E1FFEF;
                    color: white;
                    border: 1px solid #47417c;
                }

            .paging span {
                background-color: #0755A1;
                padding: 1px 7px;
                color: white;
                border: 1px solid #0755A1;
            }

        tr.paging {
            background: none !important;
        }

            tr.paging tr {
                background: none !important;
            }

            tr.paging td {
                border: none;
            }

        .card1 {
            margin-top: 78px;
        }

        .listdash {
            width: 50%;
            padding: 33px;
        }

        @media only screen and (max-width: 767px) {
            .card1 {
                margin-top: 0px;
            }

            .card_boxss {
                margin-top: 20%;
            }

            .bg-navbar {
                background-color: #0755a1;
                width: 90% !important;
                position: fixed;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="update">
            <ContentTemplate>

                <%--Admin Dashboard Start--%>
                <div id="DivAdminDashboard" runat="server" visible="False">
                    <div class="row card_boxss" style="padding: 20px;">

                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Divcountblock" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">

                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Customers
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="lblcustomercount" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fa fa-users fa-2x text-info" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Divcountblock1" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Vendors
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="lblvendorcount" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fa fa-industry fa-2x text-warning" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Divcountblock2" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Products
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="lblproductcount" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fab fa-fw fa-product-hunt fa-2x text-success"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Divcountblock3" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Components
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="lblcompcount" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fa fa-wrench fa-2x text-danger" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--  TEST--%>
                        <%-- <div class="col-xl-3 col-md-6 mb-4">
                        <div id="Div1" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                            <div class="card-body">
                                <div class="row no-gutters align-items-center">
                                    <div class="col mr-2">
                                        <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                            Testing Product Count
                                           
                                        </div>
                                        <div class="row no-gutters align-items-center">
                                            <div class="col-auto">
                                                <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                    <asp:Label ID="lbltodaytestingcount" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-auto">
                                        <i class="fa fa-wrench fa-2x text-danger" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                        <%--  TEST--%>
                    </div>
                    <div class="row">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Testing List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_todaytestingprod" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_todaytestingprod_PageIndexChanging" OnRowDataBound="gv_todaytestingprod_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryDate" runat="server" Text='<%# Convert.ToDateTime( Eval("EntryDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Pending Testing List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_pendingtesting" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="5" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_pendingtesting_PageIndexChanging" OnRowDataBound="gv_pendingtesting_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryDate" runat="server" Text='<%# Convert.ToDateTime( Eval("EntryDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inward Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("inwardEntrystatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Created Quotation List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_Quot_List" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_Quot_List_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--  <asp:TemplateField HeaderText="Qua.No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quatation_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>

                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amt.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quo. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("Quotation_Date","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Pending Quotation List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="Gv_totalQuatationList" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="Gv_totalQuatationList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quo.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amt.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quo. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuDate" runat="server" Width="100px" Text='<%# Eval("Quotation_Date","{0:d}") %>'></asp:Label>
                                                <%--  date converyt problem--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Customer P.O. List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GvCustomerpoListtoday" runat="server" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="GvCustomerpoListtoday_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O No." ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>' Width="100px"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No." ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>

                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Total Customer P.O. List(Top 20)</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_totalCustomerPOList" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" Width="100%" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="gv_totalCustomerPOList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No." ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>' Width="100px"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ref No." ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Vendor P.O. List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GvPurchaseOrderList" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Vendorname" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy").TrimEnd("0:0".ToCharArray()) %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Total Vendor P.O. List(Top 20)</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_totalvendorlist" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="gv_totalvendorlist_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Vendorname" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%#Convert.ToDateTime (Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy").TrimEnd("0:0".ToCharArray()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Invoice List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_todayInvoiceList" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_todayInvoiceList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Challan No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Term">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Total Invoice List(Top 20)</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_totalinvoicelist" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_totalinvoicelist_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Challan No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay. Terms">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Admin Dashboard End--%>

                <%--Account Dashboard Start--%>
                <div id="DivAccountDashboard" runat="server" visible="False">
                    <div class="row card_boxss" style="padding: 20px;">

                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Div1" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">

                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Customers
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="Label1" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fa fa-users fa-2x text-info" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Div2" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Vendors
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="Label2" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fa fa-industry fa-2x text-warning" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Div3" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Products
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="Label3" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fab fa-fw fa-product-hunt fa-2x text-success"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div id="Div4" runat="server" class="card1 border-left-info shadow h-90 py-2 card-bg">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1 lblcount">
                                                Components
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h4 mb-0 mr-3 font-weight-bolder text-white">
                                                        <asp:Label ID="Label4" runat="server" Text="Label" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fa fa-wrench fa-2x text-danger" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Testing List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_todaytestingprod_PageIndexChanging" OnRowDataBound="gv_todaytestingprod_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryDate" runat="server" Text='<%# Convert.ToDateTime( Eval("EntryDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Pending Testing List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_pendingtesting_PageIndexChanging" OnRowDataBound="gv_pendingtesting_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryDate" runat="server" Text='<%# Convert.ToDateTime( Eval("EntryDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inward Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("inwardEntrystatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Created Quotation List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_Quot_List_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--  <asp:TemplateField HeaderText="Qua.No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quatation_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>

                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amt.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quo. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("Quotation_Date","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Pending Quotation List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="Gv_totalQuatationList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quo.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amt.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quo. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuDate" runat="server" Width="100px" Text='<%# Eval("Quotation_Date","{0:d}") %>'></asp:Label>
                                                <%--  date converyt problem--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Customer P.O. List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView5" runat="server" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="GvCustomerpoListtoday_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O No." ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>' Width="100px"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No." ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>

                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Total Customer P.O. List(Top 20)</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" Width="100%" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="gv_totalCustomerPOList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No." ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>' Width="100px"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ref No." ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Vendor P.O. List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Vendorname" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy").TrimEnd("0:0".ToCharArray()) %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Total Vendor P.O. List(Top 20)</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="gv_totalvendorlist_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Vendorname" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoDate" runat="server" Text='<%#Convert.ToDateTime (Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy").TrimEnd("0:0".ToCharArray()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding: 20px;">
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Today Invoice List</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_todayInvoiceList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Challan No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Term">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="listdash">
                            <h5 style="font-weight: bolder">Total Invoice List(Top 20)</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_totalinvoicelist_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.O. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Challan No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay. Terms">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Account Dashboard End--%>

                <%--Reception Dashboard Start--%>
                <div id="DivReceptionDashboard" runat="server" visible="False">
                    <div class="card shadow-sm mb-4">

                        <div class="row">
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Today Testing List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridReception" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridReception_PageIndexChanging" OnRowDataBound="GridReception_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entry Date">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("EntryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Model No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="listdash">
                                <h5 style="font-weight: bolder">Pending Testing List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridReceptionpendingestinglist" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridReceptionpendingestinglist_PageIndexChanging" OnRowDataBound="GridReceptionpendingestinglist_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entry Date">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("EntryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Model No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inward Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("inwardEntrystatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Reception Dashboard End--%>

                <%--Technical Dashboard Start--%>
                <div id="DivTechnicalDashboard" runat="server" visible="False">
                    <div class="card shadow-sm mb-4">

                        <div class="row">
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Today Testing List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridTechnicaltodaytestinglist" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridTechnicaltodaytestinglist_PageIndexChanging" OnRowDataBound="GridTechnicaltodaytestinglist_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entry Date">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("EntryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                          <%--  <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Model No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="listdash">
                                <h5 style="font-weight: bolder">Pending Testing List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="Gridtechnicalpendingtestinglist" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="Gridtechnicalpendingtestinglist_PageIndexChanging" OnRowDataBound="Gridtechnicalpendingtestinglist_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entry Date">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("EntryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                         <%--   <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Model No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inward Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("inwardEntrystatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Technical Dashboard End--%>

                <%--Technical Dashboard Start--%>
                <div id="DivCustomerSupportDashboard" runat="server" visible="false">
                    <div class="card shadow-sm mb-4">

                        <div class="row">
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Today Testing List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridCustomerSupportTodayTesting" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridCustomerSupportTodayTesting_PageIndexChanging" OnRowDataBound="GridCustomerSupportTodayTesting_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entry Date">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("EntryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Model No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="listdash">
                                <h5 style="font-weight: bolder">Pending Testing List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridCustomerSupportPendingTestingList" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridCustomerSupportPendingTestingList_PageIndexChanging" OnRowDataBound="GridCustomerSupportPendingTestingList_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entry Date">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("EntryDate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Model No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inward Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("inwardEntrystatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Technical Dashboard End--%>

                <%--Schneider Dashboard Start--%>
                <div id="DivSchneiderDashboard" runat="server" visible="false">

                    <div class="card shadow-sm mb-4">
                        <div class="row" style="padding: 20px;">
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Today Created Quotation List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridSubCustomerTodayCreatedQuotation" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridSubCustomerTodayCreatedQuotation_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Company Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amt.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Quo. Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("Quotation_Date","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Pending QuotationList</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridSubcustomerpendingquotationlist" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="GridSubcustomerpendingquotationlist_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quo.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amt.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Quo. Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("Quotation_Date","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding: 20px;">
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Today Customer P.O. List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridSubcustomertodaycustomerpolist" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" PagerStyle-CssClass="paging" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" PageSize="10" OnPageIndexChanging="GridSubcustomertodaycustomerpolist_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P.O No." ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P.O. Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyy") %>' Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ref No." ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>

                            </div>
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Total Customer P.O. List(Top 20)</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridSubcustomertotalcustomer" runat="server" AutoGenerateColumns="false" AllowPaging="true" Width="100%" PagerStyle-CssClass="paging" PageSize="10" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="GridSubcustomertotalcustomer_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P.O. No." ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="P.O. Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>' Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Ref No." ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Mobile No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding: 20px;">
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Today Invoice List</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="Gridsubcustomertodayinvoicelist" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" AllowPaging="true" PageSize="10" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" OnPageIndexChanging="Gridsubcustomertodayinvoicelist_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P.O. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Term">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="listdash">
                                <h5 style="font-weight: bolder">Total Invoice List(Top 20)</h5>
                                <div class="table-responsive">
                                    <asp:GridView ID="Gridsubcustomertotalinvoicelist" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" AllowPaging="true" PageSize="10" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" OnPageIndexChanging="Gridsubcustomertotalinvoicelist_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P.O. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Terms">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Schneider Dashboard End--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>

