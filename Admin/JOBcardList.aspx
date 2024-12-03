<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="JOBcardList.aspx.cs" Inherits="Admin_JOBcardList" %>

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

        .btnlnkgrid {
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
                window.location.href = "../Admin/JOBcardList.aspx";
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
            var gvRows = document.querySelectorAll("#<%= gv_JOBCARD.ClientID %> tr:not(.paging)");

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
                    <h5 class="m-0 font-weight-bold text-primary">Job Card List</h5>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <%--   <asp:Label ID="lbljobNo" runat="server"  Text="Job No."></asp:Label>--%>
                        <asp:TextBox runat="server" class="form-control txtsear jobcard" ID="txtjob" name="Search" placeholder="Job No." Width="150px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetjobList" TargetControlID="txtjob" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <%--  <asp:Label ID="lblrepetedno" runat="server"  Text="Repeated No."></asp:Label>--%>
                        <asp:TextBox runat="server" class="form-control txtsear jobcard" ID="txtrepetedno" name="Search" placeholder="Repeated No." Width="150px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetrepeteList" TargetControlID="txtrepetedno" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <%--<asp:Label ID="lblengineername" runat="server"  Text="Engineer Name."></asp:Label>--%>
                        <asp:TextBox runat="server" class="form-control txtsear jobcard" ID="txtengineername" name="Search" placeholder="Engineer Name" Width="150px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetEnginameList" TargetControlID="txtengineername" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <%--<asp:Label ID="lblproductdesc" runat="server"  Text="Product Description"></asp:Label>--%>
                        <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtitemdesc" name="Search" placeholder="Product Description" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="Getitemdesc" TargetControlID="txtitemdesc" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-4 col-xs-7 col-7">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch jobcard mt-top" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary mt-top" OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary mt-top" Text="Create" OnClick="btncreate_Click"></asp:Button>
                        <asp:Button ID="btnexport" runat="server" class="btn btn-primary mt-top" Text="Export" OnClick="btnexport_Click"></asp:Button>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblstatus" runat="server" Text="Status"></asp:Label>
                        <asp:DropDownList runat="server" class="form-control" ID="txtstatus">
                            <asp:ListItem Value="Repaired" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="Repaired" Text="Repaired"></asp:ListItem>
                            <asp:ListItem Value="Not Repaired" Text="Not Repaired"></asp:ListItem>
                            <asp:ListItem Value="Tested" Text="Tested"></asp:ListItem>
                        </asp:DropDownList>
                        <%-- <asp:TextBox runat="server" class="form-control txtsear jobcard" ID="txtstatus" name="Search" placeholder="Status." Width="150px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetstatusList" TargetControlID="txtstatus" runat="server">
                        </asp:AutoCompleteExtender>--%>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lbl_formdate" runat="server" Text="Form Date" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_form_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lbl_todate" runat="server" Text="To Date" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_to_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <!-- Show Entries Dropdown -->
                        <asp:Label ID="lbl_show" runat="server" Text="Show Entries" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                        <asp:DropDownList ID="ddlShowEntries" runat="server" CssClass="form-control" onchange="updateRecords()">
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <br />

                <div style="width: 100%;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_JOBCARD" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_JOBCARD_RowCommand" OnRowDataBound="gv_JOBCARD_RowDataBound">
                            <%--PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_JOBCARD_PageIndexChanging"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">

                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">

                                    <ItemTemplate>
                                        <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobCardNo") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Repeated No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("RepeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblitemdesc" runat="server" Text='<%# Eval("ItemDesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engineer Name ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Engineer Name 2">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName2") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Engineer Name 3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName3") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Engineer Name 4">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName4") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Inward Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("InwardDate")).ToString("dd/MM/yyyy") %>
                                        <%-- <asp:Label ID="lblInward" runat="server" Text='<%# Eval("InwardDate","{0:d}") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Repairing  Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("Reparingdate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Repairing Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Eval("Reparingdate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("Reparingdate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreated" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="70px" HeaderStyle-CssClass="btnlnkgrid">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobCardNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>


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
                    </div>
                </div>


                <div style="width: 100%;">
                    <div class="table-responsive">
                        <asp:GridView ID="sortedgv_JOBCARD" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_JOBCARD_RowCommand" OnRowDataBound="gv_JOBCARD_RowDataBound">
                             <%--PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnPageIndexChanging="sortedgv_JOBCARD_PageIndexChanging"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">

                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">

                                    <ItemTemplate>
                                        <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobCardNo") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Repeated No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("RepeatedNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblitemdesc" runat="server" Text='<%# Eval("ItemDesc") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engineer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- <asp:TemplateField HeaderText="Engineer Name 2">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName2") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Engineer Name 3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName3") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Engineer Name 4">
                                    <ItemTemplate>
                                        <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName4") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Inward Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("InwardDate")).ToString("dd/MM/yyyy") %>
                                        <%-- <asp:Label ID="lblInward" runat="server" Text='<%# Eval("InwardDate","{0:d}") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--                                 <asp:TemplateField HeaderText="Reparing Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("Reparingdate")).ToString("dd/MM/yyyy") %>
                                        <asp:Label ID="lblOutward" runat="server" Text='<%# Eval("outwardDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>


                                <asp:TemplateField HeaderText="Repairing Date" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <%# Eval("Reparingdate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("Reparingdate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By" ItemStyle-Width="98px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcreated" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="70px" HeaderStyle-CssClass="btnlnkgrid">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobCardNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>


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
                    </div>
                </div>


                <%--Export grid start--%>

                <div class="table-responsive">
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
                                    <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobCardNo") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Repeated No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustname" runat="server" Text='<%# Eval("RepeatedNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdesc" runat="server" Text='<%# Eval("ItemDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Engineer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                           <%-- <asp:TemplateField HeaderText="Engineer Name 2">
                                <ItemTemplate>
                                    <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Engineer Name 3">
                                <ItemTemplate>
                                    <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName3") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Engineer Name 4">
                                <ItemTemplate>
                                    <asp:Label ID="lblenginerrname" runat="server" Text='<%# Eval("EngineerName4") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Inward Date" ItemStyle-Width="98px">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("InwardDate")).ToString("dd/MM/yyyy") %>
                                    <%-- <asp:Label ID="lblInward" runat="server" Text='<%# Eval("InwardDate","{0:d}") %>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                           <%-- <asp:TemplateField HeaderText="Reparing Date" ItemStyle-Width="98px">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("Reparingdate")).ToString("dd/MM/yyyy") %>
                                    <asp:Label ID="lblOutward" runat="server" Text='<%# Eval("outwardDate","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                              <asp:TemplateField HeaderText="Repairing Date" ItemStyle-Width="98px">
                                  <ItemTemplate>
                                      <%# Eval("Reparingdate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("Reparingdate")).ToString("dd/MM/yyyy") %>
                                  </ItemTemplate>
                              </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="98px">
                                <ItemTemplate>
                                    <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By" ItemStyle-Width="98px">
                                <ItemTemplate>
                                    <asp:Label ID="lblcreated" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="70px" HeaderStyle-CssClass="btnlnkgrid">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("JobCardNo") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>


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
                </div>

                <%--Export Grid End--%>
            </div>
        </div>
    </form>
</asp:Content>

