<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="inwardEntryList.aspx.cs" Inherits="Reception_inwardEntryList" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        element.style {
            margin-top: 23px;
        }

        /*.btncreate {
            float: right;
            margin-right: 27px;
        }*/

        .btncreate {
            float: right;
            margin-top: -66px;
            margin-right: -641px;
        }

        .btncreatee {
            margin-right: -591px;
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
        .cal_Theme1 .ajax__calendar_container {
            background-color: #DEF1F4;
            border: solid 1px #77D5F7;
        }

        .adv {
            margin-top: 9px;
        }

        .cal_Theme1 .ajax__calendar_header {
            background-color: #ffffff;
            margin-bottom: 4px;
        }

        .cal_Theme1 .ajax__calendar_title,
        .cal_Theme1 .ajax__calendar_next,
        .cal_Theme1 .ajax__calendar_prev {
            color: #004080;
            padding-top: 3px;
        }

        .cal_Theme1 .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px #77D5F7;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            color: #004080;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            text-align: center;
        }

        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_day,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_month,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_year,
        .cal_Theme1 .ajax__calendar_active {
            color: #004080;
            font-weight: bold;
            background-color: #DEF1F4;
        }

        .cal_Theme1 .ajax__calendar_today {
            font-weight: bold;
            font-size: 10px;
        }

        .cal_Theme1 .ajax__calendar_other,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_today,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_title {
            color: #bbbbbb;
        }

        .ajax__calendar_body {
            height: 158px !important;
            width: 220px !important;
            position: relative;
            overflow: hidden;
            margin: 0 0 0 -5px !important;
        }

        .ajax__calendar_container {
            padding: 4px;
            cursor: default;
            width: 220px !important;
            font-size: 11px;
            text-align: center;
            font-family: tahoma,verdana,helvetica;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            font-size: 14px;
            text-align: center;
        }

        .ajax__calendar_day {
            height: 22px !important;
            width: 27px !important;
            text-align: right;
            padding: 0 14px !important;
            cursor: pointer;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            margin-left: 12px !important;
            color: #004080;
        }

        .ajax__calendar_year {
            height: 50px !important;
            width: 51px !important;
            font-weight: bold;
            text-align: center;
            cursor: pointer;
            overflow: hidden;
            color: #004080;
        }

        .ajax__calendar_month {
            height: 50px !important;
            width: 51px !important;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
            overflow: hidden;
            color: #004080;
        }

        .grid tr:hover {
            background-color: #d4f0fa;
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
        }

        .listItem {
            color: #191919;
        }

        .uppercase {
            text-transform: uppercase;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 600;
        }

        .pcoded[theme-layout="vertical"][vertical-nav-type="expanded"] .pcoded-header .pcoded-left-header, .pcoded[theme-layout="vertical"][vertical-nav-type="expanded"] .pcoded-navbar {
            width: 210px;
        }
    </style>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">

    <script src="../Scripts/jquery-ui-1.11.4.js"></script>

    <script type="text/javascript">
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: "dd-mm-yy",
                changeMonth: true,
                changeYear: true
            });
        });

        // Function to change the number of records shown based on dropdown selection
        function updateRecords() {
            var selectedValue = document.getElementById('<%= ddlShowEntries.ClientID %>').value;
            var gvRows = document.querySelectorAll("#<%= gv_Inward.ClientID %> tr:not(.paging)");

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
    <style type="text/css">
        .img1 {
            vertical-align: unset !important;
            border-style: none;
        }

            .img1:hover {
                opacity: 0.75;
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
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/InwardEntryList.aspx";
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

        (function () {
            $(document).ready(function () {
                $("#<%= txtSearchProduct.ClientID %>").on("focus", function () {
                    var customerName = $("#<%= txtSearch.ClientID %>").val();
                    var extender = $find("<%= AutoCompleteExtender3.ClientID %>");
                    if (extender) {
                        extender.set_contextKey(customerName);
                    }
                });

                $("#<%= txtSearchStatus.ClientID %>").on("focus", function () {
                    var customerName = $("#<%= txtSearch.ClientID %>").val();
                     var extender = $find("<%= AutoCompleteExtender4.ClientID %>");
                     if (extender) {
                         extender.set_contextKey(customerName);
                     }
                });

                $("#<%= txtreatedNo.ClientID %>").on("focus", function () {
                    var customerName = $("#<%= txtSearch.ClientID %>").val();
                    var extender = $find("<%= AutoCompleteExtender2.ClientID %>");
                    if (extender) {
                        extender.set_contextKey(customerName);
                    }
                });

                $("#<%= txtJobNo.ClientID %>").on("focus", function () {
                    var customerName = $("#<%= txtSearch.ClientID %>").val();
                    var productName = $("#<%= txtSearchProduct.ClientID %>").val();
                                   var extender = $find("<%= AutoCompleteExtender5.ClientID %>");
                                   if (extender) {
                                       // Serialize parameters as a JSON string
                                       extender.set_contextKey(JSON.stringify({ customerName: customerName, productName: productName }));
                                   }
                               });


            });
        })();

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Inward Entry List</h5>
                </div>

                <div class="row">

                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Customer Name :"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtSearch" name="Search" placeholder="Search Customer" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Product :"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtSearchProduct" name="Search" placeholder="Search Product" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetProductList" TargetControlID="txtSearchProduct" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                       <asp:Label ID="lbljobno" runat="server" Text="Job No. :"></asp:Label>
                        <asp:TextBox ID="txtJobNo" class="form-control txtjob " placeholder="Job No." runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJOBNOList" TargetControlID="txtJobNo" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Text="Date :"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtDateSearch" name="Search" placeholder=" Enter Date" TextMode="Date" />
                    </div>

                    <div class="col-md-1 col-xs-3 col-3">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary txtsear" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-1 col-xs-3 col-3" style="margin-left: -31px;">
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary txtsear mt-top" OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>

                </div>
                <div class="row" style="margin-left: 121px;">
                    <div class="col-md-2">
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreate" Text="Create" OnClick="btncreate_Click"></asp:Button>
                    </div>

                    <div class="col-md-2">
                        <button id="btnexportexcel" runat="server" class="btn btn-primary btncreate btncreatee" onserverclick="btnexportexcel_Click">
                            Export <i class="fa fa-file-excel"></i>
                        </button>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblrepetedno" runat="server" Text="Repeated No. :"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtreatedNo" name="Search" placeholder="Search Repeated No.." />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetreapeatedList" TargetControlID="txtreatedNo" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-2">
                        <asp:Label ID="Label3" runat="server" Text="Status :"></asp:Label>
                         <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtSearchStatus" name="Search" placeholder="Search Status" onkeypress="return character(event)" />
                         <asp:AutoCompleteExtender ID="AutoCompleteExtender4" CompletionListCssClass="completionList"
                             CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                             CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetStatusList" TargetControlID="txtSearchStatus" runat="server">
                         </asp:AutoCompleteExtender>
                    </div>

                    <%-- -----------------date filter------------------%>

                    <div class="col-md-3">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                    </div>

                    <%-----------------------------%>

                    <div class="col-md-2">
                        <!-- Show Entries Dropdown -->
                        <asp:Label ID="lbl_show" runat="server" Text="Show Entries" CssClass="control-label col-sm-6"></asp:Label>
                        <asp:DropDownList ID="ddlShowEntries" runat="server" CssClass="form-control" onchange="updateRecords()">
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="width: 100%; padding: 20px;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_Inward" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Inward_RowCommand" OnRowDataBound="gv_Inward_RowDataBound1">
                            <%--OnPageIndexChanging="gv_Inward_PageIndexChanging" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging"--%>
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
                                        <asp:Label ID="lblrepeatedno" runat="server" Text='<%# Eval("RepeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date In" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateIn")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCustname" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbranch" runat="server" Text='<%# Eval("Branch") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Product">
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
                                        <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Fault">
                                    <ItemTemplate>
                                        <asp:Label ID="lblproductfault" runat="server" Text='<%# Eval("ProductFault") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMateStatus" runat="server" Text='<%# Eval("MateStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Final Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfinalStatus" runat="server" Text='<%# Eval("FinalStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Day Count">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldayscount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Information">
                                    <ItemTemplate>
                                        <asp:Label ID="lblotherinfo" runat="server" Text='<%# Eval("otherinfo") %>'></asp:Label>
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
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

                        <%-- Sorted Grid started--%>
                        <asp:GridView ID="sortedgv" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Inward_RowCommand" OnRowDataBound="gv_Inward_RowDataBound1">
                            <%--OnPageIndexChanging="sortedgv_PageIndexChanging" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging"--%>
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
                                        <asp:Label ID="lblrepeatedno" runat="server" Text='<%# Eval("RepeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date In" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateIn")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCustname" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbranch" runat="server" Text='<%# Eval("Branch") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Product">
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
                                        <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Fault">
                                    <ItemTemplate>
                                        <asp:Label ID="lblproductfault" runat="server" Text='<%# Eval("ProductFault") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMateStatus" runat="server" Text='<%# Eval("MateStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Final Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfinalStatus" runat="server" Text='<%# Eval("MateStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Day Count">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldayscount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Information">
                                    <ItemTemplate>
                                        <asp:Label ID="lblotherinfo" runat="server" Text='<%# Eval("otherinfo") %>'></asp:Label>
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
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                        <%--Sorted Grid End--%>

                        <%--Export To Excel Grid Start--%>
                        <asp:GridView ID="GridEporttoexcel" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
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
                                        <asp:Label ID="lblrepeatedno" runat="server" Text='<%# Eval("RepeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date In" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("DateIn")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCustname" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbranch" runat="server" Text='<%# Eval("Branch") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Product">
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
                                        <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Fault">
                                    <ItemTemplate>
                                        <asp:Label ID="lblproductfault" runat="server" Text='<%# Eval("ProductFault") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMateStatus" runat="server" Text='<%# Eval("MateStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Final Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfinalStatus" runat="server" Text='<%# Eval("FinalStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Day Count">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldayscount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Information">
                                    <ItemTemplate>
                                        <asp:Label ID="lblotherinfo" runat="server" Text='<%# Eval("otherinfo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--  <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="gvhead">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Imagepath") %>' Width="100px" Height="100px" Style="border: 1px solid #acacac;" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--       <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                        <%--Export to Excel Grid End--%>
                    </div>
                </div>

            </div>
        </div>
    </form>
</asp:Content>

