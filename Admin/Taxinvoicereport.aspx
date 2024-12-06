<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Taxinvoicereport.aspx.cs" Inherits="Admin_Taxinvoicereport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/TaxInvoice.aspx";
            })
        };
    </script>
    <style>
        .btncreate {
            float: right;
            margin-left: 27px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">

            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Invoice Report</h5>
            </div>
            <hr />

            <div class="card-body">
                <div class="row text-center">
                    <div class="col-md-2">
                        <asp:Label ID="lbl_invoiceno" runat="server" Text="Invoice No. :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_Invoice_search" CssClass="form-control" placeholder="Search Invoice No..." runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetinvoicenoList" TargetControlID="txt_Invoice_search" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbl_pono" runat="server" Text="P.O. No. :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_pono_search" CssClass="form-control" placeholder="PO No." runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetponoList" TargetControlID="txt_pono_search" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblcustomername" runat="server" Text="Customer Name :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txtcustomer" CssClass="form-control" runat="server" placeholder="Customer Name"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtcustomer" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbl_formdate" runat="server" Text="Form Date :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_form_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbl_todate" runat="server" Text="To Date :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_to_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="" CssClass="lblvendorpo"></asp:Label><br />
                        <asp:LinkButton ID="btn_search" CssClass="btn btn-primary lnksearchvendorpo" runat="server" OnClick="btn_search_Click"><i class="fa fa-search" aria-hidden="true" style="font-size:24px"></i></asp:LinkButton>
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        <asp:LinkButton ID="btn_refresh" CssClass="btn btn-primary" runat="server" OnClick="btn_refresh_Click"><i class="fa fa-refresh " style="font-size:24px"></i></asp:LinkButton>
                        <br />
                    </div>
               
                        <div class="col-10">
                        </div>
                        <div class="col">
                            <asp:Button ID="btnExportExcel" OnClick="btnExportExcel_Click" style="margin-top: -28px"  CssClass="btn btn-primary" runat="server" Text="Export Excel" />
                        </div>
                   
                    <div class="table-responsive text-center">
                        <asp:GridView ID="GvPurchaseOrderList" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" OnRowCommand="GvPurchaseOrderList_RowCommand" ShowFooter="True" OnRowDataBound="gv_TaxInvoice_List_RowDataBound">
                            <%--AllowPaging="true" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging" PageSize="10" PagerStyle-CssClass="paging"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
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
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Compname" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
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
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_grandtotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("InvoiceDate").ToString().TrimEnd("0:0".ToCharArray()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_createdby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Type">
                                     <ItemTemplate>
                                         <asp:Label ID="lbl_type" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandName="ShowReport" CommandArgument='<%# Eval("InvoiceNo") %>' Visible="false"><i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
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

                        <%--Exoprt to excel grid start--%>

                         <asp:GridView ID="sortedgv" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" OnRowCommand="GvPurchaseOrderList_RowCommand" ShowFooter="True" OnRowDataBound="gv_TaxInvoice_List_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
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
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Compname" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
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
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_grandtotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbl_Date" runat="server" Text='<%# Convert.ToDateTime( Eval("InvoiceDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("InvoiceDate").ToString().TrimEnd("0:0".ToCharArray()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_createdby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandName="ShowReport" CommandArgument='<%# Eval("InvoiceNo") %>' Visible="false"><i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
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

                        <%--Export to excel grid end--%>


                        <asp:HiddenField ID="hdId" runat="server"></asp:HiddenField>
                    </div>
                </div>
            </div>

        </div>
    </form>
</asp:Content>

