<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DeliveryChallan.aspx.cs" Inherits="Admin_DeliveryChallan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function HideLabe(msg) {
            swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/DeliveryChallanList.aspx";
            })
        };
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
        }

        .listItem {
            color: #191919;
            font-size: 16px;
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
            margin-right: 42px;
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
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("deliverychallan").offsetTop;
            window.scrollTo(0, target);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary" id="headerreport" runat="server">Delivery Challan</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="update" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-5">
                                    <asp:Label ID="lblCustomerName" runat="server" class="control-label col-sm-6">Customer Name :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" OnTextChanged="txtcustomerName_TextChanged" AutoPostBack="true" ID="txtcustomerName"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtcustomerName" runat="server">
                                    </asp:AutoCompleteExtender>

                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Customer Name" ControlToValidate="txtcustomerName" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>

                                <div class="col-md-2" visible="false" runat="server">
                                    <asp:Label ID="lbljobno" runat="server" class="control-label col-sm-6">Job No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtjobNo" OnTextChanged="txtjobNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetjobList" TargetControlID="txtjobNo" runat="server">
                                    </asp:AutoCompleteExtender>

                                    <%--   <asp:DropDownList runat="server" class="form-control" ID="txtjobNo" AutoPostBack="true"  OnSelectedIndexChanged="txtjobNo_SelectedIndexChanged">
                                        <asp:ListItem Value="" Text="Select Job No"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorQuoDate" runat="server" ErrorMessage="Please Enter Jobno" ControlToValidate="txtjobNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                </div>
                                <%--   <div class="col-md-3">
                                        <asp:Label ID="lbl_Invoice_no" runat="server" class="control-label col-sm-6" for="cust">Invoice No. :<span class="spncls">*</span></asp:Label>
                                        <asp:TextBox ID="txt_InvoiceNo" AutoPostBack="true" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Enter InvoiceNo" ControlToValidate="txt_InvoiceNo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>--%>

                                <%--                                <div class="col-md-2">
                                    <asp:Label ID="lblInvoiceno" runat="server" class="control-label col-sm-6">Invoice No. :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" ID="txt_InvoiceNo" AutoPostBack="true" OnSelectedIndexChanged="txt_InvoiceNo_SelectedIndexChanged">
                                        <asp:ListItem Value="" Text="Select InvoiceNo "></asp:ListItem>
                                    </asp:DropDownList>
                                </div>--%>
                                <%--      <div class="col-md-2">
                                    <asp:Label ID="lblIndate" runat="server" class="control-label col-sm-6">Invoice Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtinvoicedate" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Invoice Date" ControlToValidate="txtinvoicedate" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>--%>
                                <br />

                                <div class="col-md-3">
                                    <asp:Label ID="lbldocTag" runat="server" class="control-label col-sm-6">Doc.Tag :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="Txtdoctag" ReadOnly="true"></asp:TextBox>
                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator_QuoDate" runat="server" ErrorMessage="Please Enter Quotation Date" ControlToValidate="Txtdoctag" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblchallanNo" runat="server" class="control-label col-sm-6">Challan No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txtchallanNo"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblDate" runat="server" class="control-label col-sm-6">Challan Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtchallanDate" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Challan Date" ControlToValidate="txtchallanDate" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>

                                <div class="col-md-3">
                                    <asp:Label ID="lblLRNo" runat="server" class="control-label col-sm-6">L.R. No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtLRNo"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblLRDate" runat="server" class="control-label col-sm-6">L.R. Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtLRdate" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter L R Date" ControlToValidate="txtLRdate" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="Label9" runat="server" class="control-label col-sm-6">Type :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddltype">
                                        <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                                        <asp:ListItem Value="Regular" Text="Regular"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblOrderNo" runat="server" class="control-label col-sm-6">Order No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtorderNo"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblOrderDate" runat="server" class="control-label col-sm-6">Order Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtorderDate" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter Order No Date" ControlToValidate="txtorderDate" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>

                                <div class="col-md-3">
                                    <asp:Label ID="lblpoNo" runat="server" class="control-label col-sm-6">PO. No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtpono"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblpoDate" runat="server" class="control-label col-sm-6">PO. Date :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtpodate" TextMode="Date"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Order No Date" ControlToValidate="txtpodate" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>



                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblcrdays" runat="server" class="control-label col-sm-6">Cr. Days :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtcrdays"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Cr.Days" ControlToValidate="txtcrdays" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lblDelivery" runat="server" class="control-label col-sm-6">Delivery :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtdelivery"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator_QuoDate" runat="server" ErrorMessage="Please Enter Quotation Date" ControlToValidate="txtdelivery" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                            </div>
                            <div class="row">
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblMobileNo" runat="server" class="control-label col-sm-6">Mobile No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtMobileNo" MaxLength="11" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Mobile No" ControlToValidate="txtMobileNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblAddress" runat="server" class="control-label col-sm-6">Address :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtAddress"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Address" ControlToValidate="txtAddress" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lblkintatt" runat="server" class="control-label col-sm-6">Kind Att :<span class="spncls"></span></asp:Label>
                                    <asp:DropDownList ID="ddlkintperson" runat="server" class="form-control">
                                        <asp:ListItem Value="0" Text="select Kind Att"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- <asp:TextBox runat="server" class="form-control"  ID="txtKittAtt"   onkeypress="return character(event)" ></asp:TextBox>--%>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Enter Mobile No" ControlToValidate="txtMobileNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblGstNo" runat="server" class="control-label col-sm-6">GST No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtGstNo"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Enter Gst No:" ControlToValidate="txtGstNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <%--<asp:Label ID="lblemail" runat="server" class="control-label col-sm-6">Email:<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtEmail" TextMode="Email"></asp:TextBox>--%>

                                    <asp:Label ID="Lbl_mail" Visible="false" runat="server" class="control-label col-sm-6">Mail ID :<span class="spncls"></span></asp:Label>
                                    <div class="row" id="maildiv" runat="server">
                                        <table class=" col-md-12">
                                            <tr>
                                                <td class="col-11">
                                                    <asp:TextBox ID="TXtMailtbl" TextMode="Email" class="form-control" runat="server" Visible="false"></asp:TextBox>

                                                </td>

                                                <td class="col-1">
                                                    <asp:LinkButton runat="server" ID="Lnkbtn_addmail" CausesValidation="false" Visible="false" ToolTip="Add Mail ID" OnClick="Lnkbtn_addmail_Click"><i class="far fa-plus-square" aria-hidden="true" style="font-size: 24px"></i></asp:LinkButton>
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

                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkmail" runat="server" />
                                                            <%--                                                <asp:LinkButton runat="server" ID="lnkMAILDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"  CausesValidation="false" OnClick="lnkMAILDelete_Click"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>--%>
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
                                    <div class="col-md-12">
                                        <asp:Label ID="Label2" runat="server" class="control-label col-sm-6">Refrence :<span class="spncls"></span></asp:Label>
                                        <asp:TextBox runat="server" class="form-control" ID="txtRefrence"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Please Enter Status" ControlToValidate="txtstatus" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                        <br />
                                    </div>
                                    <br />
                                </div>
                                <br />



                                <div class="col-md-6">
                                    <asp:Label ID="lblnaration" runat="server" class="control-label col-sm-6">Narration :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtnarration"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Please Enter Status" ControlToValidate="txtstatus" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblstatus" runat="server" class="control-label col-sm-6">Status :<span class="spncls"></span></asp:Label>
                                    <%--<asp:TextBox runat="server" class="form-control" ID="txtstatus"></asp:TextBox>--%>
                                    <asp:DropDownList runat="server" class="form-control" ID="txtstatus">
                                        <asp:ListItem Value="--Select--" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="Repaired" Text="Repaired"></asp:ListItem>
                                        <asp:ListItem Value="Not Repaired" Text="Not Repaired"></asp:ListItem>
                                        <asp:ListItem Value="Repeat" Text="Repeat"></asp:ListItem>
                                        <asp:ListItem Value="Tested" Text="Tested"></asp:ListItem>
                                    </asp:DropDownList>


                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblstatecode" runat="server" class="control-label col-sm-6">State Code :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtstatecode"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Enter Narration" ControlToValidate="txtnarration" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>




                            </div>

                            <hr />
                            <h5 class="m-0 font-weight-bold">Componant Details</h5>
                            <br />
                            <%--<asp:UpdatePanel ID="update" runat="server"><ContentTemplate>--%>
                            <div class="row" id="componantdetails" runat="server">
                                <div class="col-md-12">
                                    <table class="table-responsive">
                                        <tr>
                                            <td style="width: 10px;">Job NO.</td>
                                            <td style="width: 10px;">Product</td>
                                            <td style="width: 10px;">Component</td>
                                            <td style="width: 10px;">Print Descriptions</td>
                                            <td style="width: 10px;">HSN/SAC</td>
                                            <td style="width: 10px;">Rate</td>
                                            <td style="width: 10px;">Unit</td>
                                            <td style="width: 10px;">Quantity</td>
                                            <%--<td style="width: 10px;">Total</td>--%>
                                            <td style="width: 10px;">Tax(%)</td>
                                            <td style="width: 10px;">Discount(%)</td>
                                            <td style="width: 10px;">Total Amount</td>
                                            <%--<td style="width: 10px;">Discriptions</td>--%>
                                            <td style="width: 10px;" visible="false" runat="server">Print Desc.</td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:DropDownList ID="txt_jobno" OnTextChanged="txt_jobno_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server" Width="150px">
                                                    <asp:ListItem Value="" Text="Select Job No.">
                                                    </asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtpoduct" runat="server" AutoPostBack="true" Width="230px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_discription_Tbl" runat="server" AutoPostBack="true" Width="230px" OnTextChanged="txt_discription_Tbl_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetDescriptionList" TargetControlID="txt_discription_Tbl" runat="server">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtprintdesc" runat="server" Width="230px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_hsn_Tbl" runat="server" Width="100px"></asp:TextBox>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txt_rate_Tbl" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)" OnTextChanged="txt_rate_Tbl_TextChanged"></asp:TextBox>

                                            </td>

                                            <td>
                                                <asp:TextBox ID="txt_unit_Tbl" onkeypress="return character(event)" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_quntity_Tbl" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)" OnTextChanged="txt_quntity_Tbl_TextChanged"></asp:TextBox>
                                            </td>
                                            <%--  <td>
                                                <asp:TextBox ID="txt_Total_Tbl" onkeypress="return isNumberKey(event)" ReadOnly="true" runat="server" Width="100px"></asp:TextBox>
                                            </td>--%>

                                            <td>
                                                <asp:TextBox ID="txt_tax_Tbl" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)" OnTextChanged="txt_tax_Tbl_TextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_discount_Tbl" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)" OnTextChanged="txt_discount_Tbl_TextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_total_amount_Tbl" ReadOnly="true" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <%-- <td>
                                                <asp:TextBox ID="txtprintdesc" runat="server" Width="100px"></asp:TextBox>
                                            </td>--%>
                                            <td>
                                                <%--<asp:Button ID="btn_add_more_Tbl" CausesValidation="false" runat="server" Text="Add More" CssClass="btn btn-facebook" OnClick="btn_add_more_Tbl_Click" />--%>
                                                <asp:LinkButton runat="server" ID="btn_add_more_Tbl" CausesValidation="false" ToolTip="Add Component" OnClick="btn_add_more_Tbl_Click" Text="Add" CssClass="btn  btn-primary"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <hr />
                            <%-- </ContentTemplate></asp:UpdatePanel>--%>
                            <div class="row" id="deliverychallan">
                                <div class="col-md-12">

                                    <div class="table-responsive text-center">

                                        <asp:GridView ID="dgvProductDtl" runat="server" AllowPaging="false" CssClass="table" AutoGenerateColumns="false"
                                            OnRowEditing="dgvProductDtl_RowEditing" OnRowDataBound="dgvProductDtl_RowDataBound" Visible="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">

                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSRNO" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  Added by shubham --%>
                                                <asp:TemplateField HeaderText="Product">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblproduct" ReadOnly="true" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
                                                    </ItemTemplate>

                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtmatename" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("MateName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%-- End--%>
                                                <asp:TemplateField HeaderText="Componet">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Description" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Description" TextMode="multiline" Width="200px" Height="100px" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <%--  Added by shubham --%>
                                                <asp:TemplateField HeaderText="Print Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txt_printdescription_grd" ReadOnly="true" runat="server" Text='<%# Eval("printdescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="textprintdescription_grd" TextMode="multiline" Width="200px" Height="100px" ReadOnly="true" runat="server" Text='<%# Eval("printdescription") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--  End--%>

                                                <asp:TemplateField HeaderText="HSN/SAC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txt_HSN_SAC" ReadOnly="true" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:Label>
                                                        <%--<asp:TextBox ID="txt_HSN_SAC" ReadOnly="true" Width="90px" runat="server" Text='<%# Eval("HSN") %>'></asp:TextBox>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Rate" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRate" OnTextChanged="txtRate_TextChanged" Width="80px" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Qty.">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_quntity_grd" ReadOnly="true" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" onkeypress="return isNumberKey(event)" Text='<%# Eval("Quantity") %>' Width="60px" runat="server" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Quantity" ControlToValidate="txtQuantity" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtUnit" ReadOnly="true" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                        <%--  <asp:TextBox ID="txtUnit" runat="server" Width="60px" Text='<%# Eval("Units") %>' AutoPostBack="true"></asp:TextBox>--%>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Total" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTotal" ReadOnly="true" Text='<%# Eval("Total") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Tax (%)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Tax" ReadOnly="true" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtTax" onkeypress="return isNumberKey(event)" Width="60px" runat="server" Text='<%# Eval("Tax") %>' AutoPostBack="true" OnTextChanged="txtTax_TextChanged"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Disc.(%)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Discount" ReadOnly="true" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Discount" Width="60px" onkeypress="return isNumberKey(event)" runat="server" AutoPostBack="true" Text='<%# Eval("Discount") %>' OnTextChanged="txt_Discount_TextChanged"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Total Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalPrice" ReadOnly="true" runat="server" Text='<%# Eval("Total Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--                                                <asp:TemplateField HeaderText="Print Description">
                                                 <%--   <ItemTemplate>
                                                        <asp:Label ID="lblprintDescription" ReadOnly="true" runat="server" Text='<%# Eval("Print Description") %>'></asp:Label>
                                                    </ItemTemplate>--%>
                                                <%--     </asp:TemplateField>--%>

                                                <%-- <asp:TemplateField HeaderText="Action" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkbtnEdit" Width="10px"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                                &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" Width="10px" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="gv_update" Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " runat="server" OnClick="gv_update_Click"></asp:LinkButton>&nbsp
                                                <asp:LinkButton ID="gv_cancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server" OnClick="gv_cancel_Click"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <HeaderStyle BackColor="#0755a1" ForeColor="White" Font-Bold="true" />

                                        </asp:GridView>

                                        <br />

                                        <asp:GridView ID="dgvProductDtl1" runat="server" AllowPaging="false" CssClass="table" AutoGenerateColumns="false" OnRowDataBound="dgvProductDtl1_RowDataBound" OnRowEditing="dgvProductDtl1_RowEditing" Visible="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">

                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSRNO" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Job No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJob" ReadOnly="true" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Jobno_grdd" onkeypress="return isNumberKey(event)" runat="server" Width="150" Text='<%# Eval("JobNo") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <%-- //New added by shubham--%>
                                                <asp:TemplateField HeaderText="Product">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblproduct" ReadOnly="true" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtmatename" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("MateName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--       End --%>
                                                <asp:TemplateField HeaderText="Componet">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Description" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Description" TextMode="multiline" Width="200px" Height="100px" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--  Added by shubham --%>
                                                <asp:TemplateField HeaderText="Print Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lblprintdescription_grd" ReadOnly="true" runat="server" Text='<%# Eval("printdescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_printdescription_grd" TextMode="multiline" Width="200px" Height="100px" ReadOnly="true" runat="server" Text='<%# Eval("printdescription") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--  End--%>
                                                <asp:TemplateField HeaderText="HSN/SAC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHSN_SAC" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:Label>
                                                        <%--<asp:TextBox ID="txt_HSN_SAC" ReadOnly="true" Width="90px" runat="server" Text='<%# Eval("HSN") %>'></asp:TextBox>--%>
                                                    </ItemTemplate>

                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_HSN_SAC" TextMode="MultiLine" Width="200px" Height="100px" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:TextBox>
                                                    </EditItemTemplate>

                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Rate" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRate" OnTextChanged="txtRate_TextChanged" Width="80px" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Qty.">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_quntity_grd" ReadOnly="true" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" onkeypress="return isNumberKey(event)" Text='<%# Eval("Quantity") %>' Width="60px" runat="server" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Quantity" ControlToValidate="txtQuantity" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" ReadOnly="true" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                        <%--  <asp:TextBox ID="txtUnit" runat="server" Width="60px" Text='<%# Eval("Units") %>' AutoPostBack="true"></asp:TextBox>--%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtUnit" Width="80px" runat="server" Text='<%# Eval("Unit") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--     <asp:TemplateField HeaderText="Total" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTotal" ReadOnly="true" Text='<%# Eval("Total") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Tax (%)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Tax" ReadOnly="true" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtTax" onkeypress="return isNumberKey(event)" Width="60px" runat="server" Text='<%# Eval("Tax") %>' AutoPostBack="true" OnTextChanged="txtTax_TextChanged"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Disc.(%)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Discount" ReadOnly="true" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Discount" Width="60px" onkeypress="return isNumberKey(event)" runat="server" AutoPostBack="true" Text='<%# Eval("Discount") %>' OnTextChanged="txt_Discount_TextChanged"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Total Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalPrice" ReadOnly="true" runat="server" Text='<%# Eval("Total Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Print Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprintDescription" ReadOnly="true" runat="server" Text='<%# Eval("Print Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <%-- <asp:TemplateField HeaderText="Action" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkbtnEdit" Width="10px"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                                &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" Width="10px" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="gv_update1" Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " runat="server" OnClick="gv_update1_Click"></asp:LinkButton>&nbsp
                                                <asp:LinkButton ID="gv_cancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server" OnClick="gv_cancel_Click"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <HeaderStyle BackColor="#0755a1" ForeColor="White" Font-Bold="true" />

                                        </asp:GridView>

                                        <br />

                                        <div class="row">
                                            <div class="table-responsive text-center">
                                                <asp:GridView ID="grd_getDTLS" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="false" Width="100%" OnRowDataBound="grd_getDTLS_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txt_sr_no_GET" ReadOnly="true" runat="server" Text='<%# Container.DataItemIndex+1%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Job No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txt_Jobno_grdd" ReadOnly="true" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txt_Jobno_grdd" runat="server" Width="150" Text='<%# Eval("JobNo") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <%-- //New added by shubham--%>
                                                        <asp:TemplateField HeaderText="Product">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblproduct" ReadOnly="true" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtmatename" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("MateName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--       End --%>


                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txt_discription_GET" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <%--  Added by shubham --%>
                                                        <asp:TemplateField HeaderText="Print Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Lblprintdescription_grd" ReadOnly="true" runat="server" Text='<%# Eval("printdescription") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txt_printdescription_grd" TextMode="multiline" Width="200px" Height="100px" ReadOnly="true" runat="server" Text='<%# Eval("printdescription") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--  End--%>

                                                        <asp:TemplateField HeaderText="HSN / SAC">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txt_hsn_GET" ReadOnly="true" runat="server" Text='<%# Eval("Hsn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_rate_GET" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                            </ItemTemplate>
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
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Tax">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_tax_GET" ReadOnly="true" runat="server" Text='<%# Eval("TaxPercenteage") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Discount %">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_discount_GET" ReadOnly="true" runat="server" Text='<%# Eval("DiscountPercentage") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Total Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_total_amount_GET" ReadOnly="true" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdn_GETTTL" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" CausesValidation="false"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                                            </div>
                                        </div>



                                    </div>
                                </div>
                            </div>
                            <br />

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <br />
                                            <br />
                                            <br />

                                            <center>
                                        <div class="col-md-12">
                                            <asp:Label ID="lbl_total_amt" runat="server" class="control-label col-sm-6">Total Amount (In Words) :<span class="spncls"></span></asp:Label><br />
                                            <asp:Label ID="lbl_total_amt_Value" class="control-label col-sm-6 font-weight-bold" runat="server" Text=""></asp:Label>
                                             <asp:HiddenField ID="hfTotal" runat="server" />
                                        </div>
                                            </center>

                                        </div>
                                        <div class="col-md-6" style="text-align: right">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lbl_Subtotal" runat="server" class="control-label col-sm-6">Subtotal :<span class="spncls"></span></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txt_Subtotal"></asp:TextBox><br />
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lbl_cgst9" runat="server" class="control-label col-sm-6">CGST % on Subtotal :<span class="spncls"></span></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txt_cgst9"></asp:TextBox><br />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lbl_sgst9" runat="server" class="control-label col-sm-6">SGST % on Subtotal :<span class="spncls"></span></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txt_sgst9"></asp:TextBox><br />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label1" runat="server" class="control-label col-sm-6">IGST % on Subtotal :<span class="spncls"></span></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txtigst"></asp:TextBox><br />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lbl_grandTotal" runat="server" class="control-label col-sm-6">Grand Total :<span class="spncls"></span></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txt_grandTotal"></asp:TextBox><br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please Enter Component Info." ControlToValidate="txt_grandTotal" ForeColor="Red"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                        </div>
                                        <hr />
                                        <h5 class="m-0 font-weight-bold">Terms & Condition</h5>
                                        <br />
                                        <br />
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_1" runat="server" class="control-label col-sm-6" for="cust">Term 1<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_1" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please Enter Term" ControlToValidate="txt_term_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>

                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_1" runat="server" class="control-label col-sm-6" for="cust">Condition 1<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_1" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>

                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_2" runat="server" class="control-label col-sm-6" for="cust">Term 2<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_2" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Please Enter Term" ForeColor="Red" ControlToValidate="txt_term_2" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>

                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_2" runat="server" class="control-label col-sm-6" for="cust">Condition 2<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_2" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_2" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>

                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_3" runat="server" class="control-label col-sm-6" for="cust">Term 3 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_3" runat="server" CssClass="form-control"></asp:TextBox><br />
                                            </div>

                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_3" runat="server" class="control-label col-sm-6" for="cust">Condition 3 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_3" runat="server" CssClass="form-control"></asp:TextBox>
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

                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <center> 
                                   <div class="col-md-12">
                                          <div class="row">

                            <div class="col-md-4">
                            </div>
                            <div class="col-md-4" visible="false" id="mailcheck" runat="server">
                                <center>
                                <asp:CheckBox ID="ChkSendQuotation" runat="server"></asp:CheckBox>
                                &nbsp;
                                    <asp:Label runat="server" Text="Send Challan Invoice on Mail ID"></asp:Label>
                                                               <br />
                                     </center>
                            </div>
                            <div class="col-md-1">
                            </div>

                            <div class="col-md-4">
                            </div>

                        </div>
                                 <div class="col-md-6">  
                            <br />
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
         &nbsp;&nbsp;    
                                     <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 "  CausesValidation="False"  Text="Cancel"  OnClick="btnCancel_Click"  ></asp:Button>
                               <asp:HiddenField runat="server" ID="hidden" /> 

            </div>

                                   </div></center>


                </div>

            </div>
        </div>





    </form>

</asp:Content>

