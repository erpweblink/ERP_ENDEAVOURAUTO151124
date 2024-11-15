<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="InwardReport.aspx.cs" Inherits="Admin_InwardReport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style>
        .txtsear {
            margin-left: 22px;
        }

        .txtcust {
            margin-left: 10px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 900;
        }

        .auto-style1 {
            margin-left: 11;
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

        .lbljob {
            margin-left: 20px;
        }

        .lblcust {
            margin-left: 10px;
        }

        .panelinward {
            border: 1px solid darkgray;
            width: 70%;
            padding: 20px !important;
        }

        .floa {
            float: right;
        }

        .lbl {
            font-weight: bold;
        }

        .btnclose {
            margin-top: -2%;
        }
    </style>

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
    </script>
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

    <script type="text/javascript">

        function openModal() {
            $('#exampleModalCenter').modal('show');
        }

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
    </script>
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Inward Entry Report</h5>
            </div>
            <div class="row">

                <div class="col-md-2">
                    <asp:Label ID="lbljobno" runat="server" class="control-label col-sm-6 lbljob">Job No. </asp:Label>
                    <asp:TextBox runat="server" class="form-control txtsear " ID="txtjob" name="Search" placeholder="Job No." />
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJobList" TargetControlID="txtjob" runat="server">
                    </asp:AutoCompleteExtender>

                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblcustomer" runat="server" class="control-label col-sm-6 lblcust">Customer Name </asp:Label>

                    <asp:TextBox runat="server" class="form-control txtcust" ID="txtSearchCust" name="Search" placeholder="Customer Name" onkeypress="return character(event)" />
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearchCust" runat="server">
                    </asp:AutoCompleteExtender>
                </div>
                <%--  <div class="col-md-2">
                <asp:Label ID="lbltestedby" runat="server" class="control-label col-sm-6">Tested By </asp:Label>

                <asp:TextBox runat="server" class="form-control " ID="txtsearchtestedby" name="Search" placeholder="Tested By" onkeypress="return character(event)" />
                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetEngineerList" TargetControlID="txtsearchtestedby" runat="server">
                </asp:AutoCompleteExtender>
            </div>--%>
                <div class="col-md-2">
                    <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date</asp:Label>
                    <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                    <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <br />
                    <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary " OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>

                    <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary " OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>

                    <asp:Button ID="btnExportExcel" OnClick="btnExportExcel_Click" CssClass="btn btn-primary" runat="server" Text="Export Excel" />

                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lbltested" runat="server" class="control-label col-sm-6 lblcust">Testing Product </asp:Label>

                    <asp:TextBox runat="server" class="form-control txtcust" ID="txttested" name="Search" placeholder="Search Product" onkeypress="return character(event)" />
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetProductList" TargetControlID="txttested" runat="server">
                    </asp:AutoCompleteExtender>
                </div>
            </div>

            <div style="width: 100%; padding: 20px;">
                <div class="table-responsive">
                    <asp:GridView ID="gv_Inward" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                        OnPageIndexChanging="gv_Inward_PageIndexChanging" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
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
                            <asp:TemplateField HeaderText="Date In">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblDateIn" runat="server" Text='<%# Convert.ToDateTime( Eval("DateIn","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>
                                    <asp:Label ID="lblDateIn" runat="server" Text='<%# Eval("DateIn","{0:d}") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Serial No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Material Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblMateStatus" runat="server" Text='<%# Eval("MateStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Tested By">
                        <ItemTemplate>
                            <asp:Label ID="lblTestBy" runat="server" Text='<%# Eval("TestBy") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Model No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkshow" runat="server" class="btn btn-primary" CommandArgument='<%#Eval("JobNo")%>' OnClick="lnkshow_Click">Show</asp:LinkButton>
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

                    <%--Export grid start--%>

                    <asp:GridView ID="sortedgv" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">

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
                            <asp:TemplateField HeaderText="Date In">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblDateIn" runat="server" Text='<%# Convert.ToDateTime( Eval("DateIn","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>
                                    <asp:Label ID="lblDateIn" runat="server" Text='<%# Eval("DateIn","{0:d}") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Serial No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Material Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblMateStatus" runat="server" Text='<%# Eval("MateStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Tested By">
                        <ItemTemplate>
                            <asp:Label ID="lblTestBy" runat="server" Text='<%# Eval("TestBy") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Model No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkshow" runat="server" class="btn btn-primary" CommandArgument='<%#Eval("JobNo")%>' OnClick="lnkshow_Click">Show</asp:LinkButton>
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

                    <%--Export grid end--%>
                </div>
            </div>

            <asp:Button ID="btnprof" runat="server" Style="display: none" />
            <asp:ModalPopupExtender ID="modelprofile" runat="server" TargetControlID="btnprof"
                PopupControlID="PopupAddDetail" OkControlID="Closepopdetail" />
            <asp:Panel ID="PopupAddDetail" runat="server" class="w3-panel w3-white panelinward" GroupingText="Inward Details" Direction="LeftToRight" Wrap="true">
                <div class="row btnclose">
                    <div class="col-md-11">
                    </div>
                    <div class="col-md-1">
                        <asp:LinkButton ID="Closepopdetail" runat="server"><i class="fa fa-close" style="font-size:24px;color:red;"></i></asp:LinkButton>
                    </div>
                </div>

                <div class="row">
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <asp:Label ID="lblpjobNo" runat="server" class="control-label lbl">Job No.</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lbljobshow" runat="server" class="control-label "></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblCustName" runat="server" class="control-label lbl">Customer Name</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lblcustomershow" runat="server" class="control-label "></asp:Label>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3 ">
                        <asp:Label ID="lblmodelNo" runat="server" class="control-label  lbl ">Model No.</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lblmodelNoshow" runat="server" class="control-label "></asp:Label>
                    </div>

                    <div class="col-md-3">
                        <asp:Label ID="lblSerialNo" runat="server" class="control-label lbl">Serial No.</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lblSerialNoshow" runat="server" class="control-label "></asp:Label>

                    </div>
                </div>
                <div class="row">
                    <%--  <div class="col-md-3">
                    <asp:Label ID="lbltestby" runat="server" class="control-label lbl">Test By</asp:Label>
                </div>
                <div class="col-md-3">
                    :<asp:Label ID="lbltestbyshow" runat="server" class="control-label "></asp:Label>
                </div>--%>
                    <div class="col-md-3">
                        <asp:Label ID="lblDateIn" runat="server" class="control-label lbl">Entry Date</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lblDateInhow" runat="server" class="control-label "></asp:Label>
                    </div>

                </div>
                <div class="row">

                    <div class="col-md-3">
                        <asp:Label ID="lblmatename" runat="server" class="control-label lbl">Material Name</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lblmatenameshow" runat="server" class="control-label "></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblmaterialstatus" runat="server" class="control-label lbl">Material Status</asp:Label>
                    </div>
                    <div class="col-md-3">
                        :<asp:Label ID="lblmaterialstatusshow" runat="server" class="control-label "></asp:Label>

                    </div>

                </div>
                <%-- <div class="row">
                <div class="col-md-3">
                    <asp:Label ID="lblstatus" runat="server" class="control-label lbl">Status</asp:Label>
                </div>
                <div class="col-md-3">
                    :<asp:Label ID="lblstatusshow" runat="server" class="control-label "></asp:Label>
                </div>
            </div>--%>
            </asp:Panel>
        </div>
    </form>
</asp:Content>

