<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DebitNote.aspx.cs" Inherits="Admin_DebitNote" %>

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
                window.location.href = "../Admin/DebitNoteList.aspx";
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
                    <h5 class="m-0 font-weight-bold text-primary" id="headerreport" runat="server">Debit Note(Purchase Return)</h5>

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
                                    <asp:Label ID="lbljob" runat="server" class="control-label col-sm-6" for="cust">Job No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtJobNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJobNOList" TargetControlID="txtJobNo" runat="server">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblvocNo" runat="server" class="control-label col-sm-6">Voc. No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtVocNo" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Enter Voc No" ControlToValidate="txtVocNo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblvocDate" runat="server" Text="" Class="control-label col-sm-6">Voc. Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtvochardate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter  Date" ControlToValidate="txtvochardate" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblrefno" runat="server" class="control-label col-sm-6">Ref. No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtrefNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetRefNoList" TargetControlID="txtrefNo" runat="server">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblorderno" runat="server" class="control-label col-sm-6" for="cust">Order No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtorderno" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblorederdate" runat="server" Text="" Class="control-label col-sm-6" for="cust">Order Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtorderdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter  Date" ControlToValidate="txtorderdate" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblnarration" runat="server" class="control-label col-sm-6" for="cust">Narration :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtnarration" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbltype" runat="server" class="control-label col-sm-6">Type :<span class="spncls"></span></asp:Label>
                                    <asp:DropDownList ID="ddldropdownlist" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="select Type"></asp:ListItem>
                                        <asp:ListItem Text="Purchase Return"></asp:ListItem>
                                        <asp:ListItem Text="Debit Note(Rate Difference)"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <br />
                            <div class="row">

                                <div class="col-md-6">
                                    <asp:Label ID="lblcompname" runat="server" class="control-label col-sm-6" for="cust">Customer Name :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtCompName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCompName_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtCompName" runat="server">
                                    </asp:AutoCompleteExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Enter Customer Name" ControlToValidate="txtCompName" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyGstNo" runat="server" class="control-label col-sm-6" for="cust">GST No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_CompanyGSTno" runat="server" CssClass="form-control"></asp:TextBox><br />

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_CompanyAddress" runat="server" class="control-label col-sm-6" for="cust"> Address :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_CompanyAddress" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Company Address" ControlToValidate="txt_CompanyAddress" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblmobno" runat="server" class="control-label col-sm-6" for="cust">Mobile No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtmobileno" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="11"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter Mobile No" ControlToValidate="txtmobileno" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblStateCode" runat="server" class="control-label col-sm-6" for="cust">State Code :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtStateCode" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter state Code" ControlToValidate="txtStateCode" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Lbl_mail" runat="server" class="control-label col-sm-6">Mail ID :<span class="spncls"></span></asp:Label>
                                    <div class="table-responsive">
                                        <asp:GridView ID="Grd_MAIL" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                            AllowPaging="true" PagerStyle-CssClass="paging" DataKeyNames="id">
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

                                                <asp:TemplateField HeaderText="Action" Visible="false">
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

                            <hr />
                            <h5 class="m-0 font-weight-bold">Product Details</h5>
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
                                                <asp:TextBox ID="txt_discription" AutoPostBack="true" runat="server" Width="230px"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" CompletionListCssClass="completionList"
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
                            <br />
                            <div class="row" id="invoice">
                                <div class="table-responsive text-center">
                                    <asp:GridView ID="gvDebitnote" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="false" Width="100%"
                                        OnRowEditing="gvDebitnote_RowEditing" OnRowDataBound="gvDebitnote_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
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
                                                    <asp:TextBox ID="txt_rate_grd" OnTextChanged="txt_rate_grd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Rate") %>'></asp:TextBox>
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

                                            <asp:TemplateField HeaderText="Tax(%)">
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
                                                    <asp:LinkButton ID="gv_update" OnClick="gv_update_Click" Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>&nbsp
                                                <asp:LinkButton ID="gv_cancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>
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


                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="row">
                        <div class="col-sm-5"></div>
                        <div class="col-sm-7">
                            <asp:Button ID="btn_save" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btn_save_Click" />
                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" OnClick="btn_Cancel_Click" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

