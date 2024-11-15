<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DeliveryChallanReport.aspx.cs" Inherits="Admin_DeliveryChallanReport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        .btncreate {
            float: right;
            margin-right: 41px;
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
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "/Admin/DeliveryChallanList.aspx";
            })
        };
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
                <h5 class="m-0 font-weight-bold text-primary">Delivery Challan Report</h5>
            </div>
            <div class="row" style="margin-left: 27px;">
                <div class="col-md-2">
                    <asp:Label ID="lblchallanNo" runat="server" Text="Challan No. :" CssClass="control-label col-sm-6 "></asp:Label>
                    <asp:TextBox ID="txtchallanNo" CssClass="form-control txtcust" placeholder="Challan No." runat="server"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetchallanList" TargetControlID="txtchallanNo" runat="server">
                    </asp:AutoCompleteExtender>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lbl_Customername" runat="server" Text="Customer Name :" CssClass="control-label col-sm-6 lbljob"></asp:Label>
                    <asp:TextBox ID="txt_Customername_search" CssClass="form-control " placeholder=" Customer Name" runat="server" onkeypress="return character(event)"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txt_Customername_search" runat="server">
                    </asp:AutoCompleteExtender>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lbl_formdate" runat="server" Text="Form Date :" CssClass="control-label col-sm-6"></asp:Label>
                    <asp:TextBox ID="txt_formsearch" TextMode="Date" CssClass="form-control" runat="server" Width="174px"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lbl_todate" runat="server" Text="TO Date :" CssClass="control-label col-sm-6"></asp:Label>
                    <asp:TextBox ID="txt_Tosearch" TextMode="Date" CssClass="form-control" runat="server" Width="174px"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <br />
                    <asp:LinkButton ID="btn_search" runat="server" CssClass="btn btn-primary" OnClick="btn_search_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                    <asp:LinkButton ID="lnkBtn_rfresh" runat="server" CssClass="btn btn-primary" OnClick="lnkBtn_rfresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                     <asp:Button ID="btnExportExcel" OnClick="btnExportExcel_Click"  CssClass="btn btn-primary" runat="server" Text="Export Excel" />
                </div>
            </div>
            <div class="card-body">
                <div style="width: 100%; padding: 20px;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_Deliverychallan" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Deliverychallan_RowCommand" PagerStyle-CssClass="paging" OnPageIndexChanging="gv_Deliverychallan_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbljobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Challan No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblchaalanNo" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblorderNo" runat="server" Text='<%# Eval("OrderNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile No. ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblkindAtt" runat="server" Text='<%# Eval("KindAtt") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Challan Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToDateTime( Eval("ChallanDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>

                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("ChallanDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreated" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkshow" ToolTip="Edit Quotation" Visible="false" CommandName="RowShow" CommandArgument='<%# Eval("Id") %>'><i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView" CommandArgument='<%# Eval("ChallanNo") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>
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

                        <%--Export grid strat--%> 

                          <asp:GridView ID="sortedgv" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Deliverychallan_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbljobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Challan No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblchaalanNo" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblorderNo" runat="server" Text='<%# Eval("OrderNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile No. ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblkindAtt" runat="server" Text='<%# Eval("KindAtt") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Challan Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToDateTime( Eval("ChallanDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>

                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("ChallanDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreated" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkshow" ToolTip="Edit Quotation" Visible="false" CommandName="RowShow" CommandArgument='<%# Eval("Id") %>'><i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView" CommandArgument='<%# Eval("ChallanNo") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>
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
            </div>
        </div>
    </form>
</asp:Content>

