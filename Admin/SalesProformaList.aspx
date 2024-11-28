<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SalesProformaList.aspx.cs" Inherits="Admin_SalesProformaList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        .btncreate {
            float: right;
            margin-right: 18px;
        }

        .txtsear {
            margin-left: 22px;
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
        .active1{
            float:right;
            margin-right:80px
        }
        .lblvendorpo{
            margin-left:15px;
        }
    </style>
    <style type="text/css">
   .paging
        {
        }
         
        .paging a
        {
            background-color: #0755A1;
            padding: 1px 7px;
            text-decoration: none;
            border: 1px solid #0755A1;
        }
         
        .paging a:hover
        {
            background-color: #E1FFEF;
            color: white;
            border: 1px solid #47417c;
        }
         
        .paging span
        {
            background-color: #0755A1;
            padding: 1px 7px;
            color: white;
            border: 1px solid #0755A1;
        }
         
        tr.paging
        {
            background: none !important;
        }
         
        tr.paging tr
        {
            background: none !important;
        }
        tr.paging td
        {
            border: none;
        }
        .lnksearch
        {
          
        }
        .btnlnkgrid{
            width:150px;
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
                window.location.href = "../Admin/SalesProformaList.aspx";
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Sales Proforma List</h5>
                </div>
                <div class="row">
                <div class="col-md-2">
                        <asp:Label ID="lbl_invoiceno" runat="server" Text="Invoice No. :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_Invoice_search" CssClass="form-control" placeholder="Invoice No" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetinvoicenoList" TargetControlID="txt_Invoice_search" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                <div class="col-md-2">
                        <asp:Label ID="lbljobno" runat="server" Text="Job No. :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txtJobno" CssClass="form-control" placeholder="Job No" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJobNOList" TargetControlID="txtJobno" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                <div class="col-md-2">
                        <asp:Label ID="lblcustmername" runat="server" Text="Company Name :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txtcompany" CssClass="form-control" placeholder="Company Name" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetcomapnyList" TargetControlID="txtcompany" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                <div class="col-md-2">
                        <asp:Label ID="lbl_todate" runat="server" Text="Proforma Date :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_to_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                

                      <div class="col-md-2">
                        <br />
                        <asp:LinkButton ID="btn_search"  CssClass="btn btn-primary "  OnClick="btn_search_Click" runat="server"><i class="fa fa-search" aria-hidden="true" style="font-size:24px"></i></asp:LinkButton>
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        <asp:LinkButton ID="btn_refresh" OnClick="btn_refresh_Click"  CssClass="btn btn-primary" runat="server"><i class="fa fa-refresh " style="font-size:24px"></i></asp:LinkButton>
                        <br />
                    </div>
                     <div class="col-md-2">
                <br />
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreate" Text="Create"  OnClick="btncreate_Click"></asp:Button>
                    </div>
                    </div>
                 <div class="table-responsive text-center">
                        <asp:GridView ID="GvPurchaseOrderList" runat="server" AutoGenerateColumns="false" CssClass="grid"  Width="100%"
                            OnRowCommand="GvPurchaseOrderList_RowCommand">
                            <%--AllowPaging="true"  PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging"--%> 
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                              <%--  <asp:TemplateField HeaderText="Invoice No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                              <%--   <asp:TemplateField HeaderText="Job No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_company" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Proforma Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("InvoiceDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_createdby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_createddate" runat="server" Text='<%# Eval("Createddate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="LnkEdit1" runat="server" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;
                                      <%--  <a href="<%#"TaxInvoice.aspx?Id="+Eval("Id") %>"><i class='far fa-edit' style='font-size: 26px'></i></a>--%>

                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" ToolTip="Delete" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
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
                        <asp:HiddenField ID="hdId" runat="server"></asp:HiddenField>
                    </div>

                </div>
            </div>
       </form>

</asp:Content>

