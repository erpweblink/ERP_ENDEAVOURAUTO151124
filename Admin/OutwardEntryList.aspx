<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="OutwardEntryList.aspx.cs" Inherits="Reception_OutwardEntryList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        .btncreateout {
            float: right;
            margin-right: -10px;
        }

        .completionList {
            border: solid 1px Gray;
            border-radius: 5px;
            margin: 0px;
            padding: 3px;
            height: 200px;
            overflow: auto;
            width: 500px;
            background-color: #FFFFFF;
            font-size: 16px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 900;
        }
        /*.active{
            float:right;
            margin-right:80px
        }*/
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
    </style>
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/OutwardEntryList.aspx";
            })
        };
    </script>
    <!--char-->
    <script>
        function character(e) {
            isIE = document.all ? 1 : 0
            keyEntry = !isIE ? e.which : event.keyCode;
            if (((keyEntry >= '65') && (keyEntry <= '90')) || ((keyEntry >= '97') && (keyEntry <= '122')) || (keyEntry == '46') || (keyEntry == '32') || keyEntry == '45')

                return true;

            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Outward Entry List</h5>
                </div>
                <div class="row">

                    <div class="col-md-2">
                        <asp:Label ID="lblcustomer" runat="server" class="control-label col-sm-6">Customer Name</asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtSearch" name="Search" placeholder="Customer Name" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbldate" runat="server" class="control-label col-sm-6">Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtDateSearch" name="Search" placeholder=" Enter Date" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblreptedno" runat="server" class="control-label col-sm-6">Repeated No. </asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtrepeatedno" name="Search" placeholder="Repeated No." />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetrepeatednoList" TargetControlID="txtrepeatedno" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label col-sm-6">Status </asp:Label>
                        <asp:DropDownList ID="ddlsearch" runat="server"  AppendDataBoundItems="true"  class="form-control uotwardactive " Width="150px">
                            <asp:ListItem Value="" Text="Select Status"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <%-- -----------------date filter------------------%>

                    <div class="col-md-2">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" placeholder="From Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" placeholder="To Date" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <diV class="col-7">

                    </diV>
                    <div class="col-md-2 col-xs-7 col-7">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary txtsear mt-top" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary mt-top" OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>

                    <div class="col-md-3 col-xs-5 col-5">
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary  mt-top" Text="Create" OnClick="btncreate_Click"></asp:Button>
                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary  mt-top" Text="Export" OnClick="btnExport_Click"></asp:Button>
                    </div>
                </div>

                <%-----------------------------%>

                <%--   <div class="col-md-2 col-xs-7 col-7">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary txtsear mt-top" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary mt-top" OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>--%>

                <%-- <div class="col-md-2 col-xs-5 col-5">
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreateout mt-top" Text="Create" OnClick="btncreate_Click"></asp:Button>
                    </div>--%>

                <div style="width: 100%; padding: 20px;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_Outward" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Outward_RowCommand" OnPageIndexChanging="gv_Outward_PageIndexChanging" OnRowDataBound="gv_Outward_RowDataBound" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repeated No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrepeatedno" runat="server" Text='<%# Eval("repeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date Out" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateOut")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Material Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMatename" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
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
                             <%--   <asp:TemplateField HeaderText="Job Work By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbljobworkby" runat="server" Text='<%# Eval("JobWorkby") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--    <asp:TemplateField HeaderText="Dispatch Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustStatus" runat="server" Text='<%# Eval("Dispatchdate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Dispatch Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("Dispatchdate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateReturn")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repairing Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrepairstatus" runat="server" Text='<%# Eval("ReturnRepair") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Courier">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCourier" runat="server" Text='<%# Eval("Courier") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%# Eval("CreateBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Imagepath") %>' Width="100px" Height="100px" Style="border: 1px solid #acacac;" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

                        <%--         sorted Grid start--%>
                        <asp:GridView ID="Gvsorted" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Outward_RowCommand" OnPageIndexChanging="Gvsorted_PageIndexChanging" OnRowDataBound="gv_Outward_RowDataBound" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repeated No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrepeatedno" runat="server" Text='<%# Eval("repeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date Out" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateOut")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Material Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMatename" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
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
                                <asp:TemplateField HeaderText="Job Work By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbljobworkby" runat="server" Text='<%# Eval("JobWorkby") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--    <asp:TemplateField HeaderText="Dispatch Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustStatus" runat="server" Text='<%# Eval("Dispatchdate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Dispatch Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("Dispatchdate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateReturn")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repairing Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrepairstatus" runat="server" Text='<%# Eval("ReturnRepair") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Courier">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCourier" runat="server" Text='<%# Eval("Courier") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%# Eval("CreateBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Imagepath") %>' Width="100px" Height="100px" Style="border: 1px solid #acacac;" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

                        <%--  sorted Grid End--%>


                        <%--Export To Excel Grid Start--%>
                        <asp:GridView ID="GridExportExcel" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repeated No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrepeatedno" runat="server" Text='<%# Eval("repeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date Out" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateOut")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Material Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMatename" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
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
                                <asp:TemplateField HeaderText="Job Work By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbljobworkby" runat="server" Text='<%# Eval("JobWorkby") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--    <asp:TemplateField HeaderText="Dispatch Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustStatus" runat="server" Text='<%# Eval("Dispatchdate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Dispatch Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("Dispatchdate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateReturn")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repairing Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrepairstatus" runat="server" Text='<%# Eval("ReturnRepair") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Courier">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCourier" runat="server" Text='<%# Eval("Courier") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%# Eval("CreateBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--      <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Imagepath") %>' Width="100px" Height="100px" Style="border: 1px solid #acacac;" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--  <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
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
                        <%--Export To Excel Grid End--%>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

