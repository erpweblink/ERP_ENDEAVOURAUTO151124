<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="PurchaseOrder.aspx.cs" Inherits="Admin_PurchaseOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


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

    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/PurchaseOrderList.aspx";
            })
        };
    </script>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("divdtls").offsetTop;
            window.scrollTo(0, target);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>


        <div class="col-lg-12 card">
            <asp:HiddenField ID="hdntotal" runat="server" />
            <div class="card-header  py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary" id="headerreport" runat="server">Vendor P.O</h5>

            </div>
            <hr />
            <div class="card-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <h5 class="m-0 font-weight-bold">Details</h5>
                        <br />
                        <asp:HiddenField ID="hdnID" runat="server" />
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lbl_vendor_name" runat="server" class="control-label col-sm-6" for="cust">Vendor Name :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox ID="txt_vendor_name" AutoPostBack="true" OnTextChanged="txt_vendor_name_TextChanged" onkeypress="return character(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Enter Vendor Name" ControlToValidate="txt_vendor_name" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetvendorList" TargetControlID="txt_vendor_name" runat="server">
                                </asp:AutoCompleteExtender>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lbl_po_no" runat="server" class="control-label col-sm-6" for="cust">P.O. No. :<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_po_no" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3 mt-top">
                                <asp:Label ID="lbl_po_date" runat="server" Text="" Class="control-label col-sm-6" for="cust">P.O. Date :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox ID="txt_po_date" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter P.O. Date" ControlToValidate="txt_po_date" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>

                            </div>
                            <div class="col-md-6 mt-top">
                                <asp:Label ID="lbl_kind_att" runat="server" Text="" Class="control-label col-sm-6" for="cust">Kind Att<span class="spncls"></span></asp:Label>
                                <asp:DropDownList ID="txt_kind_att" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Kind Att" ControlToValidate="txt_kind_att" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>--%>

                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_ref_no" runat="server" class="control-label col-sm-6" for="cust">Ref No. :<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_ref_no" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mt-top">
                                <asp:Label ID="lbl_delivery_address" runat="server" Text="" Class="control-label col-sm-6" for="cust">Delivery Address :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox ID="txt_delivery_address" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Delivery Address" ControlToValidate="txt_delivery_address" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_mobile_no" runat="server" class="control-label col-sm-6" for="cust">Mobile No. :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox ID="txt_mobile_no" onkeypress="return isNumberKey(event)" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter Mobile No" ControlToValidate="txt_mobile_no" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_gst_no" runat="server" class="control-label col-sm-6" for="cust">GST No. :<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_gst_no" MaxLength="15" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mt-top">
                                <asp:Label ID="lbl_pay_tern" runat="server" class="control-label col-sm-6" for="cust">Pay Terms :<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_pay_term" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_email_id" runat="server" class="control-label col-sm-6">Email ID :<span class="spncls"></span></asp:Label>
                                <div class="row" id="maildiv" runat="server">
                                    <table class=" col-md-12">
                                        <tr>
                                            <td class="col-11">
                                                <asp:TextBox ID="txt_email_id" runat="server" TextMode="Email" Visible="false" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td class="col-1">
                                                <asp:LinkButton runat="server" ID="Lnkbtn_addmail" CausesValidation="false" Visible="false" OnClick="Lnkbtn_addmail_Click"><i class="far fa-plus-square" aria-hidden="true" style="font-size: 24px"></i></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <div class="table-responsive">
                                        <asp:GridView ID="Grd_MAIL" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                            AllowPaging="true" PagerStyle-CssClass="paging" OnRowDataBound="Grd_MAIL_RowDataBound" DataKeyNames="Id">
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
                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesignation" Text='<%# Eval("designation") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmail" runat="server" />
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
                                    <br />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Label2" runat="server" class="control-label col-sm-6" for="cust">State Code<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txtstatecode" runat="server" CssClass="form-control"></asp:TextBox>
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
                                        <td style="width: 10px;">HSN/SAC</td>
                                        <td style="width: 10px;">Rate</td>
                                        <td style="width: 10px;">Unit</td>
                                        <td style="width: 10px;">Quantity</td>
                                        <td style="width: 10px;">Tax(%)</td>
                                        <td style="width: 10px;">Discount(%)</td>
                                        <td style="width: 10px;">Total Amount</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_discription" runat="server" Width="230px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetdescriptionList" TargetControlID="txt_discription" runat="server">
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
                                            <asp:TextBox ID="txt_tax" AutoPostBack="true" OnTextChanged="txt_tax_TextChanged" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_discount" AutoPostBack="true" OnTextChanged="txt_discount_TextChanged" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_total_amount" ReadOnly="true" runat="server" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_add_more" CausesValidation="false" runat="server" Text="Add More" CssClass="btn btn-facebook" OnClick="btn_add_more_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <hr />
                        <div class="row" id="divdtls">
                            <div class="table-responsive text-center">
                                <asp:UpdatePanel ID="updatepurchase" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:GridView ID="gvPurchaseRecord" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="false" Width="100%" OnRowEditing="gvPurchaseRecord_RowEditing" OnRowDataBound="gvPurchaseRecord_RowDataBound" OnRowUpdating="gvPurchaseRecord_RowUpdating">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr. No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txt_sr_no" ReadOnly="true" runat="server" Text='<%# Container.DataItemIndex+1%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txt_discription_grd" ReadOnly="true" runat="server" Text='<%# Eval("Discription") %>'></asp:Label>
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
                                                            <asp:TextBox ID="txt_rate_grd" onkeypress="return isNumberKey(event)" OnTextChanged="txt_rate_grd_TextChanged" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txt_unit_grd" ReadOnly="true" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quntity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_quntity_grd" ReadOnly="true" runat="server" Text='<%# Eval("Quntity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_quntity_grd" onkeypress="return isNumberKey(event)" OnTextChanged="txt_quntity_grd_TextChanged" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Quntity") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_tax_grd" ReadOnly="true" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_tax_grd" onkeypress="return isNumberKey(event)" OnTextChanged="txt_tax_grd_TextChanged" runat="server" Width="120" AutoPostBack="true" Text='<%# Eval("Tax") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Discount(%)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_discount_grd" ReadOnly="true" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_discount_grd" onkeypress="return isNumberKey(event)" OnTextChanged="txt_discount_grd_TextChanged" runat="server" Width="100" AutoPostBack="true" Text='<%# Eval("Discount") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Total Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_total_amount_grd" ReadOnly="true" runat="server" Text='<%# Eval("Total Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txt_total_amount_grd" ReadOnly="true" runat="server" Width="160" AutoPostBack="true" Text='<%# Eval("Total Amount") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="gv_update" Text="Update" CausesValidation="false" OnClick="gv_update_Click" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton><br />
                                                            <br />
                                                            <asp:LinkButton ID="gv_cancel" CausesValidation="false" OnClick="gv_cancel_Click" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <br />
                        <div class=" form-group row">
                            <div class="col-md-6">
                            </div>
                            <div class="col-md-4" style="text-align: end">
                                <asp:Label ID="lblsubtotal" runat="server" CssClass="lbl" Text="Subtotal :"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_total" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>
                            <div class="col-md-6"></div>
                            <div class="col-md-4" style="text-align: end">
                                <asp:Label ID="lbl_cgst_amt" runat="server" CssClass="lbl" Text="CGST Amount :"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_cgst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-2  ">
                            </div>
                            <div class="col-md-4 text-center m-0">
                                <asp:Label ID="Label1" runat="server" Text="Amount In Word"></asp:Label>
                            </div>
                            <div class="col-md-4" style="text-align: end">
                                <asp:Label ID="lbl_sgst_amt" runat="server" Text="SGST Amount :"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_sgst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-2 ">
                            </div>
                            <div class="col-md-4 text-center m-0 font-weight-bold">
                                <asp:Label ID="lbl_Amount_In_Word" runat="server" CssClass="font-weight:bold" Text="" onclientpopulated="ConvertNumberToWords(this.value)"><span></span></asp:Label>
                                <asp:HiddenField ID="hfTotal" runat="server" />
                            </div>
                            <div class="col-md-4" style="text-align: end">
                                <asp:Label ID="lbl_igst" runat="server" Text="IGST Amount :"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_igst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-6"></div>
                            <div class="col-md-4" style="text-align: end">
                                <asp:Label ID="lbl_round_off" runat="server" Text="Round Off :"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_round_off" OnTextChanged="txt_round_off_TextChanged" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-6"></div>
                            <div class="col-md-4" style="text-align: end">
                                <asp:Label ID="lbl_grand_total" runat="server" Text="Grand Total :"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txt_grand_total" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                        </div>
                        <hr />
                        <h5 class="m-0 font-weight-bold">Terms & Condition</h5>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lbl_term_1" runat="server" class="control-label col-sm-6" for="cust">Term 1<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_term_1" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-6">
                                <asp:Label ID="lbl_condition_1" runat="server" class="control-label col-sm-6" for="cust">Condition 1<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_condition_1" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-6">
                                <asp:Label ID="lbl_term_2" runat="server" class="control-label col-sm-6" for="cust">Term 2<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_term_2" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>

                            <div class="col-md-6">
                                <asp:Label ID="lbl_condition_2" runat="server" class="control-label col-sm-6" for="cust">Condition 2<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_condition_2" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_term_3" runat="server" class="control-label col-sm-6" for="cust">Term 3 <span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_term_3" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_condition_3" runat="server" class="control-label col-sm-6" for="cust">Condition 3 <span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_condition_3" runat="server" CssClass="form-control"></asp:TextBox><br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_term_4" runat="server" class="control-label col-sm-6" for="cust">Term 4<span class="spncls"></span></asp:Label>
                                <asp:TextBox ID="txt_term_4" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbl_condition_4" runat="server" class="control-label col-sm-6" for="cust">Condition 4 <span class="spncls"></span></asp:Label>
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
                                    <asp:Label runat="server" Text="Send Vendor P.O. on Mail ID"></asp:Label>
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
                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btn_save_Click" />
                            <asp:Button ID="btn_Cancel" OnClick="btn_Cancel_Click" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </form>

</asp:Content>

