<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SalesProforma.aspx.cs" Inherits="Admin_SalesProforma" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',                                                                                                                                                                                                        
                text: msg,
                timer: 5000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/SalesProformaList.aspx";
            })
        };
    </script>
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
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

        .txtsear {
            margin-left: 23px;
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
            font-size: initial !important;
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


    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("invoice").offsetTop;
            window.scrollTo(0, target);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-header  py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary" id="headerreport" runat="server">Sales Proforma</h5>

                </div>
                <hr />
                <div class="card-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnID" runat="server" />
                            <asp:HiddenField ID="hdnPoProductTot" runat="server" />
                            <asp:HiddenField ID="taxhidden" runat="server" />
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lbl_Invoice_no" runat="server" class="control-label " for="cust">Invoice No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_InvoiceNo" AutoPostBack="true" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Enter InvoiceNo" ControlToValidate="txt_InvoiceNo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lbljob" runat="server" class="control-label col-sm-6" for="cust">Job No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtJobNo" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtJobNo_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJobNOList" TargetControlID="txtJobNo" runat="server">
                                    </asp:AutoCompleteExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Job No." ControlToValidate="txtJobNo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_InvoiceDate" runat="server" Text="" Class="control-label col-sm-6" for="cust">Invoice Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_InvoiceDate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Invoice Date" ControlToValidate="txt_InvoiceDate" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblorderno" runat="server" class="control-label col-sm-6" for="cust">Order No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtorderno" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter InvoiceNo" ControlToValidate="txt_InvoiceNo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblorderdate" runat="server" Text="" Class="control-label col-sm-6" for="cust">Order Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtorederdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Invoice Date" ControlToValidate="txt_InvoiceDate" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblcompname" runat="server" class="control-label col-sm-6" for="cust">Company Name :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtCompName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Enter Company Name" ControlToValidate="txtCompName" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyPanNo" runat="server" class="control-label col-sm-6" for="cust"> Company Pan No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_CompanyPanNo" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyAddress" runat="server" class="control-label col-sm-6" for="cust">Company Address :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_CompanyAddress" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Company Address" ControlToValidate="txt_CompanyAddress" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyRegType" runat="server" class="control-label col-sm-6" for="cust"> Company Register Type :<span class="spncls"></span></asp:Label>
                                    <asp:DropDownList ID="drop_CompanyRegisterType" CssClass="form-control" runat="server">
                                        <asp:ListItem Text="Registered"></asp:ListItem>
                                        <asp:ListItem Text="Non-Registered"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyGstNo" runat="server" class="control-label col-sm-6" for="cust"> Company GST No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_CompanyGSTno" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyStateCode" runat="server" class="control-label col-sm-6" for="cust"> Company State Code :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_CompanyStateCode" runat="server" CssClass="form-control">
                                       
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_KindAtt" runat="server" class="control-label col-sm-6" for="cust">Kind Att. :<span class="spncls"></span></asp:Label>
                                    <asp:DropDownList ID="txt_KindAtt" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="" Text="Select Kind Att."></asp:ListItem>

                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblcrdays" runat="server" class="control-label col-sm-6" for="cust"> Cr. Days :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtCrdays" runat="server" CssClass="form-control">
                                       
                                    </asp:TextBox>
                                </div>

                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="Lbl_mail" runat="server" class="control-label col-sm-6">Mail ID :<span class="spncls"></span></asp:Label>
                                    <div class="table-responsive">
                                        <asp:GridView ID="Grd_MAIL" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                            AllowPaging="true" PagerStyle-CssClass="paging" DataKeyNames="Id" OnRowDataBound="Grd_MAIL_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMAILSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Mail ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmultMail" Text='<%# Eval("Email") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmail" runat="server" />
                                                        <%-- <asp:LinkButton runat="server" ID="lnkMAILDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" OnClick="lnkMAILDelete_Click" CausesValidation="false"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                                            <PagerSettings PageButtonCount="4" LastPageText="Last" />
                                            <PagerStyle BackColor="White" ForeColor="#6777EF" HorizontalAlign="Left" BorderColor="#CCCCCC" />
                                            <RowStyle ForeColor="#000066" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#00547E" />

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <h5 class="m-0 font-weight-bold">Componant Details</h5>
                            <br />
                            <div class="row" id="tbl" runat="server">
                                <div class="col-md-12">
                                    <table class="table-responsive">
                                        <tr>

                                            <td style="width: 10px;">Description</td>
                                            <td style="width: 10px;">HSN / SAC</td>

                                            <td style="width: 10px;">Rate</td>
                                            <td style="width: 10px;">Unit</td>
                                            <td style="width: 10px;">Quantity</td>
                                            <td style="width: 10px;">Tax(%)</td>
                                            <td style="width: 10px;">Discount(%)</td>
                                            <td style="width: 10px;">Total Amount</td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:TextBox ID="txt_discription" AutoPostBack="true" runat="server" Width="230px" OnTextChanged="txt_discription_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetDescriptionList" TargetControlID="txt_discription" runat="server">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_hsn" runat="server" Width="100px"></asp:TextBox>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txt_rate" OnTextChanged="txt_rate_TextChanged" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>

                                            </td>

                                            <td>
                                                <asp:TextBox ID="txt_unit" onkeypress="return character(event)" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_quntity" OnTextChanged="txt_quntity_TextChanged" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_tax" OnTextChanged="txt_tax_TextChanged" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_discount" OnTextChanged="txt_discount_TextChanged" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_total_amount" ReadOnly="true" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_add_more" OnClick="btn_add_more_Click" CausesValidation="false" runat="server" Text="Add More" CssClass="btn btn-facebook" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <hr />
                            <div class="row" id="invoice">
                                <div class="table-responsive text-center">
                                    <asp:GridView ID="gvPurchaseRecord" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="false" Width="100%" OnRowDataBound="gvPurchaseRecord_RowDataBound" OnRowEditing="gvPurchaseRecord_RowEditing">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_sr_no" ReadOnly="true" runat="server" Text='<%# Container.DataItemIndex+1%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_discription_grd" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="HSN / SAC">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_hsn_grd" ReadOnly="true" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_rate_grd" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_rate_grd" onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_unit_grd" ReadOnly="true" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_quntity_grd" ReadOnly="true" runat="server" Text='<%# Eval("Quntity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_quntity_grd" OnTextChanged="txt_quntity_grd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Quntity") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tax">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_tax_grd" ReadOnly="true" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_tax_grd" OnTextChanged="txt_tax_grd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="120" AutoPostBack="true" Text='<%# Eval("Tax") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discount %">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_discount_grd" ReadOnly="true" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_discount_grd" OnTextChanged="txt_discount_grd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="100" AutoPostBack="true" Text='<%# Eval("Discount") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_total_amount_grd" ReadOnly="true" runat="server" Text='<%# Eval("TotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_total_amount_grd" ReadOnly="true" runat="server" Width="160" AutoPostBack="true" Text='<%# Eval("TotalAmount") %>'></asp:TextBox>
                                                    <asp:HiddenField ID="hdn_GrdTTL" runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="gv_update" Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " runat="server" OnClick="gv_update_Click"></asp:LinkButton>&nbsp;&nbsp;
                                                <asp:LinkButton ID="gv_cancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server" OnClick="gv_cancel_Click"></asp:LinkButton>
                                                </EditItemTemplate>
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
                            <br />

                            <div class="row">
                                <div class="table-responsive text-center">
                                    <asp:GridView ID="grd_getDTLS" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="false" Width="100%" OnRowDataBound="grd_getDTLS_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_sr_no_GET" ReadOnly="true" runat="server" Text='<%# Container.DataItemIndex+1%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_discription_GET" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="HSN / SAC">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_hsn_GET" ReadOnly="true" runat="server" Text='<%# Eval("Hsn_Sac") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_rate_GET" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%-- <EditItemTemplate>
                                                    <asp:TextBox ID="txt_rate_GET"  onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                </EditItemTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_unit_GET" ReadOnly="true" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_quntity_GET" ReadOnly="true" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%-- <EditItemTemplate>
                                                    <asp:TextBox ID="txt_quntity_GET"  onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                                </EditItemTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tax">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_tax_GET" ReadOnly="true" runat="server" Text='<%# Eval("TaxPercenteage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%-- <EditItemTemplate>
                                                    <asp:TextBox ID="txt_tax_GET"  onkeypress="return isNumberKey(event)" runat="server" Width="120" AutoPostBack="true" Text='<%# Eval("TaxPercenteage") %>'></asp:TextBox>
                                                </EditItemTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Discount %">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_discount_GET" ReadOnly="true" runat="server" Text='<%# Eval("DiscountPercentage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%-- <EditItemTemplate>
                                                    <asp:TextBox ID="txt_discount_GET"  onkeypress="return isNumberKey(event)" runat="server" Width="100" AutoPostBack="true" Text='<%# Eval("DiscountPercentage") %>'></asp:TextBox>
                                                </EditItemTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_total_amount_GET" ReadOnly="true" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdn_GETTTL" runat="server" />
                                                </ItemTemplate>
                                                <%--  <EditItemTemplate>
                                                    <asp:TextBox ID="txt_total_amount_GET" ReadOnly="true" runat="server" Width="160" AutoPostBack="true" Text='<%# Eval("Total") %>'></asp:TextBox>
                                                </EditItemTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" OnClick="lnkbtnDelete_Click" CausesValidation="false"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--  <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="gv_edit_GET" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="gv_update_GET"  Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>&nbsp
                                                <asp:LinkButton ID="gv_cancel_GET" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>
                                                </EditItemTemplate>
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

                                </div>
                            </div>
                            <br />
                            <div class=" form-group row">
                                <div class="col-6">
                                </div>

                                <div class="col-4" style="text-align: end">
                                    <%--<asp:Label ID="lbl_total" runat="server" Text="Total"></asp:Label>--%>
                                </div>
                                <div class="col-2">
                                    <asp:TextBox ID="txt_subtotal" ReadOnly="true" runat="server" CssClass="form-control" Visible="false"></asp:TextBox><br />
                                </div>

                                <div class="col-6"></div>
                                <div class="col-4" style="text-align: end">
                                    <asp:Label ID="lbl_cgst_amt" runat="server" CssClass="lbl" Text="CGST % On Total Amt."></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:TextBox ID="txt_cgst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>

                                <div class="col-2  ">
                                </div>
                                <div class="col-4 text-center m-0">
                                    <asp:Label ID="Label1" runat="server" Text="Amount In Word."></asp:Label>
                                </div>

                                <div class="col-4" style="text-align: end">
                                    <asp:Label ID="lbl_sgst_amt" runat="server" Text="SGST % On Total Amt."></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:TextBox ID="txt_sgst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>
                                <div class="col-2 ">
                                </div>
                                <div class="col-4 text-center m-0 font-weight-bold">
                                    <asp:Label ID="lbl_Amount_In_Word" runat="server" CssClass="font-weight:bold" Text="" onclientpopulated="ConvertNumberToWords(this.value)"><span></span></asp:Label>
                                    <asp:HiddenField ID="hfTotal" runat="server" />
                                </div>

                                <div class="col-4" style="text-align: end">
                                    <asp:Label ID="lbligst" runat="server" Text="IGST % On Total Amt."></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:TextBox ID="txt_igst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>
                                <div class="col-6"></div>


                                <div class="col-4" style="text-align: end">
                                    <asp:Label ID="lbl_grand_total" runat="server" Text="Grand Total."></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:TextBox ID="txt_grand_total" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>

                            <hr />
                            <br />
                            <h5 class="m-0 font-weight-bold">Terms & Condition</h5>
                            <br />
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_term_1" runat="server" class="control-label col-sm-6" for="cust">Term 1 :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_term_1" runat="server" CssClass="form-control"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please Enter Term" ControlToValidate="txt_term_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_condition_1" runat="server" class="control-label col-sm-6" for="cust">Condition 1<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_condition_1" runat="server" CssClass="form-control"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_term_2" runat="server" class="control-label col-sm-6" for="cust">Term 2 :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_term_2" runat="server" CssClass="form-control"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Please Enter Term" ForeColor="Red" ControlToValidate="txt_term_2" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_condition_2" runat="server" class="control-label col-sm-6" for="cust">Condition 2 :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_condition_2" runat="server" CssClass="form-control"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_2" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_term_3" runat="server" class="control-label col-sm-6" for="cust">Term 3 <span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_term_3" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_condition_3" runat="server" class="control-label col-sm-6" for="cust">Condition 3 :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_condition_3" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_term_4" runat="server" class="control-label col-sm-6" for="cust">Term 4 :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_term_4" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_condition_4" runat="server" class="control-label col-sm-6" for="cust">Condition 4 :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_condition_4" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <div class="col-md-12">
                        <div class="row">

                            <div class="col-md-4">
                            </div>
                            <div class="col-md-4" id="mailcheck" runat="server">
                                <center>
                                <asp:CheckBox ID="ChkSendQuotation" runat="server"></asp:CheckBox>
                                &nbsp;
                                    <asp:Label runat="server" Text="Send Proforma Invoice on Mail ID"></asp:Label>
                                <br />
                                <br />
                                     </center>
                            </div>
                            <div class="col-md-1">
                            </div>

                            <div class="col-md-4">
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-5"></div>
                            <div class="col-sm-7">
                                <asp:Button ID="btn_save" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btn_save_Click" />
                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btn_Cancel_Click" CausesValidation="false" />
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</asp:Content>

