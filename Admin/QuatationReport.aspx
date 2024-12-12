<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="QuatationReport.aspx.cs" EnableEventValidation="false" Inherits="Admin_QuatationReport" %>

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

        .txtsear {
            margin-left: -48px;
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
            font-size: 12px;
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

        .txtjob {
            margin-left: 22px;
        }

        .txtcust {
            margin-left: 10px;
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
                window.location.href = "";
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Quotation Report</h5>
            </div>
            <hr />
            <div class="card-body">

                <div class="row">

                    <div class="col-md-2">
                        <asp:Label ID="lblQuNo" runat="server" class="control-label col-sm-6 lbljob lblvendorpo"> Quotation No.</asp:Label>
                        <asp:TextBox runat="server" class="form-control txtjob " ID="txtQuNo" name="Search" placeholder="Quotation No." />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetQuatationList" TargetControlID="txtQuNo" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblcustomer" runat="server" class="control-label col-sm-6 lblcust lblvendorpo">Customer Name </asp:Label>
                        <asp:TextBox runat="server" class="form-control txtcust" ID="txtSearchCust" name="Search" placeholder="Customer Name" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearchCust" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6 lblvendorpo">From Date</asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6 lblvendorpo">To Date</asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <br />
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearchvendorpo" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary " OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>

                        <%--<asp:LinkButton ID="btnExort" runat="server" CssClass="btn btn-primary"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>--%>
                    </div>
                </div>

                <div class="row">

                    <div class="col-10">
                    </div>

                    <div class="col" style="margin-top:-42px; margin-left:20px;">
                        <asp:Button ID="btnexporttoexcel" OnClick="btnexporttoexcel_Click" CssClass="btn btn-primary" runat="server" Text="Export Excel" />
                    </div>
                </div>

                <div style="width: 100%; padding: 20px;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_Quot_List" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Quot_List_RowCommand" ShowFooter="True" OnRowDataBound="gv_QuotRe_List_RowDataBound">
                            <%--OnPageIndexChanging="gv_Quot_List_PageIndexChanging" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="JOB No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qua.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblkindAtt" runat="server" Text='<%# Eval("kind_Att") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mobile No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmobNo" runat="server" Text='<%# Eval("Mobile_No") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quotation Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblQuDate" runat="server" Text='<%# Convert.ToDateTime( Eval("Quotation_Date","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>

                                        <asp:Label ID="lblQuDate" runat="server" Text='<%# Eval("Quotation_Date","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreateuser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agains By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblagainsby" runat="server" Text='<%# Eval("Againstby") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandName="ShowReport" CommandArgument='<%# Eval("ID") %>' Visible="false"><i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView"
                                            CommandArgument='<%# Eval("Quotation_no") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblFooterTotalAmt" runat="server" Text="Total Amt:" Font-Bold="True"></asp:Label>
                                    </FooterTemplate>
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

                        <%--Filter grid start--%> 

                          <asp:GridView ID="sortedgv" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Quot_List_RowCommand" ShowFooter="True" OnRowDataBound="gv_QuotRe_List_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="JOB No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qua.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblkindAtt" runat="server" Text='<%# Eval("kind_Att") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mobile No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmobNo" runat="server" Text='<%# Eval("Mobile_No") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quotation Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblQuDate" runat="server" Text='<%# Convert.ToDateTime( Eval("Quotation_Date","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>

                                        <asp:Label ID="lblQuDate" runat="server" Text='<%# Eval("Quotation_Date","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreateuser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Against By">
                                     <ItemTemplate>
                                         <asp:Label ID="lblagainstby" runat="server" Text='<%# Eval("Againstby") %>'></asp:Label>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandName="ShowReport" CommandArgument='<%# Eval("ID") %>' Visible="false"><i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView"
                                            CommandArgument='<%# Eval("Quotation_no") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblFooterTotalAmt" runat="server" Text="Total Amt:" Font-Bold="True"></asp:Label>
                                    </FooterTemplate>
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

                        <%--Filter grid end--%> 


                    </div>
                </div>

            </div>
        </div>
    </form>
</asp:Content>

