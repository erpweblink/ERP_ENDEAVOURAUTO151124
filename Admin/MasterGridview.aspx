<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterGridview.aspx.cs" Inherits="Admin_MasterGridview" MasterPageFile="~/Admin/AdminMaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        .btncreate {
            float: right;
            margin-right: 18px;
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

        .active1 {
            float: right;
            margin-right: 80px
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

        .lnksearch {
        }

        .btnlnkgrid {
            width: 150px;
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
                window.location.href = "../Admin/CustomerList.aspx";
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
        <div class="col-md-12">

            <div class="card">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h3 class="m-0 font-weight-bold text-primary">Master List </h3>
                </div>

                <div class="row" style="margin-left: 10px;">
                    <div class="col-md-3">
                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Customer Name:" CssClass="lblcustomer"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear" ID="txtSearch" name="Search" placeholder="Customer Name" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Evolution Eng. Name:" CssClass="lblcustomer"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear" ID="txtEvolutionEnginName" name="Search" placeholder="Evolution Eng. Name" AutoPostBack="true" OnTextChanged="txtEvolutionEnginName_TextChanged" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetEvolutionEnginNameList" TargetControlID="txtEvolutionEnginName" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type :<span class="spncls"></span></asp:Label>
                        <asp:DropDownList runat="server" class="form-control" OnTextChanged="ddlservicetype_TextChanged" AutoPostBack="true" ID="ddlservicetype">
                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                            <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                            <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Repairing Eng. Name:" CssClass="lblcustomer"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear" ID="txtRepairingENGName" name="Search" placeholder="Repairing Eng. Name" AutoPostBack="true" OnTextChanged="txtRepairingENGName_TextChanged" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetRepairingEnginNameList" TargetControlID="txtRepairingENGName" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lbljobno" runat="server" Font-Bold="true" Text="Job No.:" CssClass="lblcustomer"></asp:Label>
                        <asp:TextBox ID="txtJobno" CssClass="form-control jobtxtcustomer" AutoPostBack="true" OnTextChanged="txtJobno_TextChanged" placeholder="Job No." runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJobNoList" TargetControlID="txtJobno" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="Label6" runat="server" Font-Bold="true" Text="Repearing Status.:" CssClass="lblcustomer"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text=" From Date :"></asp:Label>
                        <div>
                            <asp:TextBox ID="txtfromdate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Text=" To Date :"></asp:Label>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-1">

                        <asp:LinkButton ID="btnSearch" OnClick="btnSearch_Click" runat="server" Style="margin-top: 24px" Width="100%" CssClass="btn btn-info"><i style="color:white" class="fa">&#xf002;</i> &nbsp;</asp:LinkButton>

                    </div>

                    <div class="col-md-1">

                        <asp:LinkButton ID="btnrefresh" runat="server" Style="margin-top: 24px" OnClick="btnrefresh_Click" Width="100%" CssClass="btn btn-warning"><i style="color:white" class="fa">&#xf021;</i> &nbsp;</asp:LinkButton>

                    </div>
                    <%--      <div class="col-md-3">
                            <asp:Label ID="lblcustomername" runat="server" Text="Customer Name" CssClass="control-label col-sm-6 lblcust lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txtcustomername" CssClass="form-control txtcust" placeholder="Customer" runat="server"></asp:TextBox>                     
                        </div>--%>
                </div>
            </div>

            <div class="card-body">
                <div style="width: 100%; padding: 0px;">
                    <div class="table-responsive">
                        <asp:GridView ID="Gvmasterlist" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                           
                            PagerStyle-CssClass="paging" OnPageIndexChanging="Gvmasterlist_PageIndexChanging" OnRowDataBound="Gvmasterlist_RowDataBound">
                            <Columns>
                                <%--<asp:BoundField DataField="Status" HeaderText="Current stage" />--%>
                                <asp:TemplateField HeaderText="Sr.No." HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobNo" HeaderText="Job No." />
                                <asp:BoundField DataField="CustName" HeaderText="Customer Name" />
                                <asp:BoundField DataField="Subcustomer" HeaderText="Sub Customer" />
                                <asp:BoundField DataField="InwardDate" HeaderText="Inward Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                <asp:BoundField DataField="ModelNo" HeaderText="Model No." />
                                <asp:BoundField DataField="JobAssignDate" HeaderText="Job Assign Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                                <asp:BoundField DataField="EvaluatonDate" HeaderText="Evalution Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="EngineerName" HeaderText="Evalution Eng.Name" />
                                <asp:BoundField DataField="Quotation_Date" HeaderText="Quotation Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="QuotationBaseAmount" HeaderText="Quotation Base Amoun" />
                                <asp:BoundField DataField="PODATE" HeaderText="P.O. Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="POBaseammount" HeaderText="P.O. Base Amount" />
                                <asp:BoundField DataField="CompRecDate" HeaderText="Component Receive Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="RepearingStatus" HeaderText="Repairing Status" />
                                <asp:BoundField DataField="Reparingdate" HeaderText="Repairing  Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="RepearingEngineerName" HeaderText="Repair Engineer Name" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="InvoiceBaseamount" HeaderText="Invoice Base amount" />
                                <asp:BoundField DataField="InwardDate" HeaderText="Inward Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="OutwardDate" HeaderText="Outward Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="Days" HeaderText="Days count" />
                                <asp:TemplateField HeaderText="Repeated Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRepeatedDate" runat="server" Text='<%# Bind("repeatedDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReturnDate" runat="server" Text='<%# Bind("ReturnDate") %>' />
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

            <%--new grid start--%>

               <div class="card-body">
                <div style="width: 100%; padding: 0px;">
                    <div class="table-responsive">
                        <asp:GridView ID="GridTechnical" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                           
                            PagerStyle-CssClass="paging" OnPageIndexChanging="Gvmasterlist_PageIndexChanging" OnRowDataBound="Gvmasterlist_RowDataBound">
                            <Columns>
                                <%--<asp:BoundField DataField="Status" HeaderText="Current stage" />--%>
                                <asp:TemplateField HeaderText="Sr.No." HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobNo" HeaderText="Job No." />
                                <%--<asp:BoundField DataField="CustName" HeaderText="Customer Name" />--%>
                                <%--<asp:BoundField DataField="Subcustomer" HeaderText="Sub Customer" />--%>
                                <asp:BoundField DataField="InwardDate" HeaderText="Inward Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                <asp:BoundField DataField="ModelNo" HeaderText="Model No." />
                                <asp:BoundField DataField="JobAssignDate" HeaderText="Job Assign Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                                <asp:BoundField DataField="EvaluatonDate" HeaderText="Evalution Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="EngineerName" HeaderText="Evalution Eng.Name" />
                               <%-- <asp:BoundField DataField="Quotation_Date" HeaderText="Quotation Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="QuotationBaseAmount" HeaderText="Quotation Base Amoun" />--%>
                                <asp:BoundField DataField="PODATE" HeaderText="P.O. Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <%--<asp:BoundField DataField="POBaseammount" HeaderText="P.O. Base Amount" />--%>
                                <asp:BoundField DataField="CompRecDate" HeaderText="Component Receive Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="RepearingStatus" HeaderText="Repairing Status" />
                                <asp:BoundField DataField="Reparingdate" HeaderText="Repairing  Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="RepearingEngineerName" HeaderText="Repair Engineer Name" />
                                <%-- <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:MM/dd/yyyy}" />-%>
                                <%--<asp:BoundField DataField="InvoiceBaseamount" HeaderText="Invoice Base amount" />--%>
                                <asp:BoundField DataField="InwardDate" HeaderText="Inward Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="OutwardDate" HeaderText="Outward Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="Days" HeaderText="Days count" />
                                <asp:TemplateField HeaderText="Repeated Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRepeatedDate" runat="server" Text='<%# Bind("repeatedDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReturnDate" runat="server" Text='<%# Bind("ReturnDate") %>' />
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


            <%--new grid End--%>


        </div>
        </div>

    </form>
</asp:Content>

