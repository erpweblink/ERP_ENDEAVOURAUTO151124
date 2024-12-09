<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SiteVisitList.aspx.cs" Inherits="Admin_SiteVisitList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function HideLabel(msg, flg) {

            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                debugger;
                if (flg == '1') {
                    window.location.href = "../Admin/SiteVisitList.aspx";
                }
                else {
                    window.location.href = "../Admin/SiteVisitList.aspx";
                }
            })
        };
    </script>
    <!---char--->
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

        // Function to change the number of records shown based on dropdown selection
        function updateRecords() {
            var selectedValue = document.getElementById('<%= ddlShowEntries.ClientID %>').value;
            var gvRows = document.querySelectorAll("#<%= gv_Prod.ClientID %> tr:not(.paging)");

            for (var i = 1; i < gvRows.length; i++) {
                if (selectedValue === "All" || i <= parseInt(selectedValue)) {
                    gvRows[i].style.display = "";
                } else {
                    gvRows[i].style.display = "none";
                }
            }
        }

        // Initial setup to show 25 records on page load
        window.onload = function () {
            document.getElementById('<%= ddlShowEntries.ClientID %>').value = "25";
            updateRecords();
        };
    </script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <style>
        .spncls {
            color: red;
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

        .header {
            background-color: #0755a1;
            color: white;
            font-weight: bolder;
        }

        .active1 {
            float: right;
            margin-right: 41px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Site Visit List</h5>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblsearch" runat="server" class="control-label col-sm-6">Customer Name </asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true" ID="txtSearch" name="Search" placeholder="Search Site" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="Getsite" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblengineername" runat="server" class="control-label col-sm-6">Engineer Name </asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear" AutoPostBack="true" ID="txtengineername" name="Search" placeholder="Engineer Name" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="Getengineer" TargetControlID="txtengineername" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-4 col-xs-7 col-7">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch " CausesValidation="False" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary lnksearch" OnClick="lnkrefresh_Click" CausesValidation="false"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                        <asp:Button ID="btnexportexcel" runat="server" class="btn btn-primary btncreate btncreatee" OnClick="btnexportexcel_Click" Text="Export-Excel"></asp:Button>
                        <asp:Button ID="btnceate" OnClick="btnceate_Click" runat="server" class="btn btn-primary btncreate" Text="Create"></asp:Button>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <!-- Show Entries Dropdown -->
                        <asp:Label ID="Label3" runat="server" class="control-label col-sm-6">Show Entries </asp:Label>
                        <asp:DropDownList ID="ddlShowEntries" runat="server" CssClass="form-control" onchange="updateRecords()">
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div style="width: 100%; padding: 20px; overflow: scroll;">
                    <asp:GridView ID="gv_Prod" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                        DataKeyNames="ID" OnRowCommand="gv_Prod_RowCommand">
                        <%--PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="gv_Prod_PageIndexChanging" AllowPaging="true"--%>
                        <Columns>
                            <asp:TemplateField HeaderText="Sr.No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("Custname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Site Location">
                                <ItemTemplate>
                                    <asp:Label ID="lblModel" runat="server" Text='<%# Eval("location") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Engneer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEngineername" runat="server" Text='<%# Eval("Engineername") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("Product") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Visit Date">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Date", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblQuDate" />
                                    <%--<asp:Label ID="lblProduct" runat="server" Text='<%# Eval("Date") %>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblserialNo" runat="server" Text='<%# Eval("Servicetype") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                    &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("ID") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

            <%-- <asp:HiddenField runat="server" ID="hidden" />--%>
        </div>
    </form>
</asp:Content>

