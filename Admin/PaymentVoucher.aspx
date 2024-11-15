<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="PaymentVoucher.aspx.cs" Inherits="Admin_PaymentVoucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/PaymentVoucherDetails.aspx";
            })
        };
    </script>
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

        .active1 {
            float: right;
            margin-right: 80px
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
    <!---Number--->
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <style>
        .spncls {
            color: red;
        }

        .txtcustomer {
        }
    </style>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("divdtls").offsetTop;
            window.scrollTo(0, target);
        }
    </script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
                <div class="col-lg-12">
                    <div class="card shadow-sm mb-4">
                        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                            <h5 class="m-0 font-weight-bold text-primary">Payment Voucher</h5>
                        </div>
                        <div class="card-body">
                            <div class="row m-2">
                                <div class="col-6">
                                    <asp:Label ID="lblsuppliername" runat="server" Text="Supplier Name :"></asp:Label>
                                    <asp:TextBox ID="txtsuppliername" AutoPostBack="true" OnTextChanged="txtsuppliername_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtsuppliername" runat="server">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-6">
                                    <asp:Label ID="lblDate" runat="server" Text="Date :"></asp:Label>
                                    <asp:TextBox ID="txtDate" CssClass="form-control" TextMode="Date" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row m-2">
                                <div class="col-6">
                                    <asp:Label ID="lblbankname" runat="server" Text="Bank Name :"></asp:Label>
                                    <asp:TextBox ID="txtbankname" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <asp:Label ID="lblvoucherno" runat="server" Text="Payment Voucher No. :"></asp:Label>
                                    <asp:TextBox ID="txtpaymentvoucherno" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row m-2">
                                <div class="col-6">
                                    <asp:Label ID="lblbankcash" runat="server" Text="Bank Cash(Dr) :"></asp:Label>
                                    <asp:DropDownList ID="ddlbankcashdr" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="1" Text="---Select---"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="CASH"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="BANK OF BARODA-CC"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="BANK OF BARODA-CA"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <asp:Label ID="lblpaymentmode" runat="server" Text="Payment Mode :"></asp:Label>
                                    <asp:DropDownList ID="ddlpaymentmode" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="1" Text="---Select---"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="CASH"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="CHEQUE"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="ONLINE"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="NEFT"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="RTGS"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row m-2">
                                <div class="col-6">
                                    <asp:Label ID="lblchequetransforedno" runat="server" Text="Cheque Transfored No. :"></asp:Label>
                                    <asp:TextBox ID="txtchequeTransfporedno" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <asp:Label ID="lbldownbankcash" runat="server" Text="Down Bank Cash :"></asp:Label>
                                    <asp:TextBox ID="txtdownbankoncash" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row m-2">
                                <div class="col-6">
                                    <asp:Label ID="lbladdress" runat="server" Text="Address :"></asp:Label>
                                    <asp:TextBox ID="txtaddress" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row m-2">
                            <div style="width: 100%; padding: 20px;">
                                <asp:GridView ID="GvPaymentVoucher" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                    PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnRowDataBound="GvPaymentVoucher_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk" OnCheckedChanged="chk_CheckedChanged" runat="server" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvoiceno" Text='<%# Eval("SupInvNo") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvoicedate" runat="server" Text='<%# Eval("SupInvDate").ToString().TrimEnd("0:0".ToCharArray()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtinvoiceamt" Width="110px" ReadOnly="true" runat="server" Text='<%# Eval("GradTotal") %>'></asp:TextBox>
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
                        <div class="row">
                            <div class="col-8"></div>

                             <div class="col-3">
                                <asp:Label ID="lblgrandtotal" runat="server" Text="Grand Total :"></asp:Label>
                                <asp:TextBox ID="txtgrandtotal" CssClass="form-control" AutoComplete="off" runat="server"></asp:TextBox>
                            </div>


                            <div class="col-md-4">
                                <asp:Label ID="lbl_total_amt" runat="server" class="control-label col-sm-6">Total Amount (In Words) :<span class="spncls"></span></asp:Label><br />
                                <asp:Label ID="lbl_total_amt_Value" class="control-label col-sm-6 font-weight-bold" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="hfTotal" runat="server" />
                            </div>


                           
                            <div class="col-1"></div>
                        </div>

                        <div class="row m-2">
                            <div class="col-4"></div>
                            <div class="col-2">
                                <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary form-control" OnClick="btnsave_Click" Text="Save" />
                            </div>
                            <div class="col-2">
                                <asp:Button ID="btncancel" runat="server" CssClass="btn btn-primary form-control" OnClick="btncancel_Click" Text="Cancel" />
                            </div>
                            <div class="col-4"></div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnsave" />
                <asp:AsyncPostBackTrigger ControlID="btncancel" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>

