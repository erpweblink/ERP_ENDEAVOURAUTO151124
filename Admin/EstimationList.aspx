<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="EstimationList.aspx.cs" Inherits="Admin_EstimationList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        .btncreate {
            float: right;
            margin-right: 27px;
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
                window.location.href = "../Admin/EstimationList.aspx";
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

        // Function to change the number of records shown based on dropdown selection
        function updateRecords() {
            var selectedValue = document.getElementById('<%= ddlShowEntries.ClientID %>').value;
            var gvRows = document.querySelectorAll("#<%= gv_EstimationList.ClientID %> tr:not(.paging)");

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Estimation List</h5>
                </div>

                <div class="card-body">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:TextBox runat="server" class="form-control txtsear" ID="txtjob" name="Search" placeholder="Job No." />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetjobList" TargetControlID="txtjob" runat="server">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-2">

                            <asp:TextBox runat="server" class="form-control mt-top" ID="txtcustSearch" name="Search" placeholder="Customer Name" onkeypress="return character(event)" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtcustSearch" runat="server">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-2 col-xs-7 col-7">
                            <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary txtsear mt-top" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary mt-top" OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                        </div>
                        <div class="col-md-4 col-xs-5 col-5">
                            <asp:Button ID="btnExportExcel" runat="server" class="btn btn-primary btncreate" Text="Export to Excel" OnClick="btnExportExcel_Click"></asp:Button>
                            <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreate" Text="Create" OnClick="btncreate_Click"></asp:Button>
                        </div>
                         <div class="col-md-2">
                             <!-- Show Entries Dropdown -->
                             <asp:DropDownList ID="ddlShowEntries" runat="server" CssClass="form-control" onchange="updateRecords()">
                                 <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                 <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                 <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                 <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                             </asp:DropDownList>
                         </div>
                    </div>

                    </br>

                    <div style="width: 100%;">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_EstimationList" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                OnRowCommand="gv_EstimationList_RowCommand">
                                <%--PageSize="5" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_EstimationList_PageIndexChanging"--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJObno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Final status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblfinalstatus" runat="server" Text='<%# Eval("FinalStatus") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Final Cost">

                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalcost" runat="server" Text='<%# Eval("FinalCost") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Site Visit Charges">

                                        <ItemTemplate>
                                            <asp:Label ID="lblsitevisitcharges" runat="server" Text='<%# Eval("SiteVisitCharges") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Other Charges">

                                        <ItemTemplate>
                                            <asp:Label ID="lblothercharges" runat="server" Text='<%# Eval("OtherCharges") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Estimated Quotation">

                                        <ItemTemplate>
                                            <asp:Label ID="lblestimatedquo" runat="server" Text='<%# Eval("EstimatedQuotation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Componet Status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompstatus" runat="server" Text='<%# Eval("Componetstatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Created By">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcreateduser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Component Recived Date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcomprecdate" runat="server" Text='<%# Eval("CompRecDate","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created Date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcreatedate" runat="server" Text='<%# Eval("CreatedDate","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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


                    <%--   SortedGrid start--%>

                    <div style="width: 100%;">
                        <div class="table-responsive">
                            <asp:GridView ID="sortedgvEstimations" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                PageSize="5" AllowPaging="true" PagerStyle-CssClass="paging" OnRowCommand="gv_EstimationList_RowCommand" OnPageIndexChanging="sortedgvEstimations_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJObno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Final status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblfinalstatus" runat="server" Text='<%# Eval("FinalStatus") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Final Cost">

                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalcost" runat="server" Text='<%# Eval("FinalCost") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Site Visit Charges">

                                        <ItemTemplate>
                                            <asp:Label ID="lblsitevisitcharges" runat="server" Text='<%# Eval("SiteVisitCharges") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Other Charges">

                                        <ItemTemplate>
                                            <asp:Label ID="lblothercharges" runat="server" Text='<%# Eval("OtherCharges") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Estimated Quotation">

                                        <ItemTemplate>
                                            <asp:Label ID="lblestimatedquo" runat="server" Text='<%# Eval("EstimatedQuotation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Componet Status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompstatus" runat="server" Text='<%# Eval("Componetstatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Created By">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcreateduser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Component Recived Date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcomprecdate" runat="server" Text='<%# Eval("CompRecDate","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created Date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcreatedate" runat="server" Text='<%# Eval("CreatedDate","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                    <%--       Sorted Grid End--%>


                    <%--Export To Excel Grid Strat--%>

                    <div style="width: 100%;">
                        <div class="table-responsive">
                            <asp:GridView ID="Exporttoexcelgrid" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                PagerStyle-CssClass="paging" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJObno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Final status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblfinalstatus" runat="server" Text='<%# Eval("FinalStatus") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Final Cost">

                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalcost" runat="server" Text='<%# Eval("FinalCost") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Site Visit Charges">

                                        <ItemTemplate>
                                            <asp:Label ID="lblsitevisitcharges" runat="server" Text='<%# Eval("SiteVisitCharges") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Other Charges">

                                        <ItemTemplate>
                                            <asp:Label ID="lblothercharges" runat="server" Text='<%# Eval("OtherCharges") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Estimated Quotation">

                                        <ItemTemplate>
                                            <asp:Label ID="lblestimatedquo" runat="server" Text='<%# Eval("EstimatedQuotation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Componet Status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompstatus" runat="server" Text='<%# Eval("Componetstatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Created By">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcreateduser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Component Recived Date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcomprecdate" runat="server" Text='<%# Eval("CompRecDate","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created Date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcreatedate" runat="server" Text='<%# Eval("CreatedDate","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

                    <%--Export To Excel Grid End--%>
                </div>
            </div>
    </form>
</asp:Content>

