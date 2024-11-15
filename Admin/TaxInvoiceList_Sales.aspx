<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="TaxInvoiceList_Sales.aspx.cs" Inherits="Admin_TaxInvoiceList" %>

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

    <style type="text/css">
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
    <style>
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
                <h5 class="m-0 font-weight-bold text-primary">Invoice List (Sales)</h5>
            </div>

            <div class="card-body">
                <div class="row text-center">
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
                        <asp:Label ID="lbljobno" runat="server" Text="Customer Name :" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txtJobno" CssClass="form-control" placeholder="Customer Name" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtJobno" runat="server">
                        </asp:AutoCompleteExtender>
                        <br />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbl_formdate" runat="server" Text="Form Date" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_form_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbl_todate" runat="server" Text="To Date" CssClass="lblvendorpo"></asp:Label>
                        <asp:TextBox ID="txt_to_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-xs-7 col-7 ">
                        <br />
                        <asp:LinkButton ID="btn_search" OnClick="btn_search_Click" CssClass="btn btn-primary lnksearchvendorpo" runat="server"><i class="fa fa-search" aria-hidden="true" style="font-size:24px"></i></asp:LinkButton>
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        <asp:LinkButton ID="btn_refresh" OnClick="btn_refresh_Click" CssClass="btn btn-primary" runat="server"><i class="fa fa-refresh " style="font-size:24px"></i></asp:LinkButton>
                        <br />
                    </div>

                    <div class="col-md-2 ">

                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary" Text="Create" OnClick="btncreate_Click" Style="margin-left: -66px"></asp:Button>
                        <asp:Button ID="btnexporttoexcel" runat="server" class="btn btn-primary " Text="Export" OnClick="btnexporttoexcel_Click"></asp:Button>

                    </div>

                    <div class="col-md-3">
                        <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type <span class="spncls"></span></asp:Label>
                        <asp:DropDownList runat="server" class="form-control" OnTextChanged="ddlservicetype_TextChanged" AutoPostBack="true" ID="ddlservicetype">
                            <asp:ListItem Value="Service" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                            <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                            <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                        </asp:DropDownList>
                    </div>


                </div>
                </br>
                <div class="table-responsive text-center">
                    <asp:GridView ID="GvPurchaseOrderList" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" OnRowCommand="GvPurchaseOrderList_RowCommand" AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CompName" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P.O. No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Invoice No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("InvoiceDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblcreteddate" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Invoice Amount">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("GrandTotal") %>' runat="server" ID="lblGrandTotal" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pay Term">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--  <asp:TemplateField HeaderText="Job No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>



                            <asp:TemplateField HeaderText="Challan No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_createdby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>

                                    <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                    &nbsp;
                                        <asp:LinkButton ID="LnkEdit1" runat="server" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                    &nbsp;                                                                                                       
                                      
                                    <asp:LinkButton ID="btn_delete" runat="server" Text="" ToolTip="Delete" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
                                    &nbsp;
                                    
                                <%--<asp:LinkButton ID="btnsendchallan" runat="server" CommandName="SendChallan" CommandArgument='<%# Eval("Id") %>' ToolTip="Send to challan"><i class="fa fa-paper-plane"  style="font-size:26px;color:black"></i></asp:LinkButton>--%>
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
            <%--    sorted Grid start--%>
            <div class="table-responsive text-center">
                <asp:GridView ID="Gvsorted" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" OnRowCommand="GvPurchaseOrderList_RowCommand" AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="Gvsorted_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CompName" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Job No.">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("JobNo") %>' runat="server" ID="lblJobNo" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P.O. No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("InvoiceDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblcreteddate" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Amount">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("GrandTotal") %>' runat="server" ID="lblGrandTotal" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--  <asp:TemplateField HeaderText="Job No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>


                        <asp:TemplateField HeaderText="Pay Term">
                            <ItemTemplate>
                                <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Challan No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>





                        <%--    <asp:TemplateField HeaderText="CGST">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("CGST") %>' runat="server" ID="lblCGST" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SGST">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("SGST") %>' runat="server" ID="lblSGST" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IGST">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("IGST") %>' runat="server" ID="lblIGST" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>



                        <asp:TemplateField HeaderText="Created By">
                            <ItemTemplate>
                                <asp:Label ID="lbl_createdby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>

                                <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                &nbsp;&nbsp;
                                        <asp:LinkButton ID="LnkEdit1" runat="server" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                &nbsp;                                                                                                       
                                      
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
                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
            </div>
            <%--    sorted Grid End--%>


            <%--    Export Grid start--%>
            <div class="table-responsive text-center">
                <asp:GridView ID="GridExportExcel" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" PagerStyle-CssClass="paging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CompName" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job No.">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("JobNo") %>' runat="server" ID="lblJobNo" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P.O. No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Invoice No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Invoiceno" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("InvoiceDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblcreteddate" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Invoice Amount">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("GrandTotal") %>' runat="server" ID="lblGrandTotal" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Pay Term">
                            <ItemTemplate>
                                <asp:Label ID="lbl_PayTerm" runat="server" Text='<%# Eval("PayTerm") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--  <asp:TemplateField HeaderText="Job No.">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>



                        <asp:TemplateField HeaderText="Challan No.">
                            <ItemTemplate>
                                <asp:Label ID="lbl_challanno" runat="server" Text='<%# Eval("ChallanNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created By">
                            <ItemTemplate>
                                <asp:Label ID="lbl_createdby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>

                                <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                &nbsp;&nbsp;
                                        <asp:LinkButton ID="LnkEdit1" runat="server" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                &nbsp;                                                                                                       
                                      
                                    <asp:LinkButton ID="btn_delete" runat="server" Text="" ToolTip="Delete" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>

                                &nbsp;  
                                <asp:LinkButton ID="btnsendchallan" runat="server" CommandName="SendChallan" CommandArgument='<%# Eval("Id") %>' ToolTip="Send to challan"><i class="fa fa-paper-plane"  style="font-size:26px;color:black"></i></asp:LinkButton>

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
                <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
            </div>
            <%--    Export Grid End--%>
        </div>

    </form>

</asp:Content>

