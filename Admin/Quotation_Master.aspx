<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Quotation_Master.aspx.cs" Inherits="Admin_Quotation_Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
            font-size: 12px;
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
            margin-right: 42px;
    </style>


    <!-- CSS only -->

    <%-- <!-- JavaScript Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>--%>

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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
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
                window.location.href = "../Admin/QuotationList.aspx";
            })
        };
    </script>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("Quatationgrid").offsetTop;
            window.scrollTo(0, target);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnID1" runat="server" />
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary" id="headerreport" runat="server">Quotation</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="update" runat="server">
                        <ContentTemplate>
                            <div class="row">

                                <%-- <div class="col-md-3">
                                    <asp:Label ID="lblquotagn" runat="server" class="control-label col-sm-6">Quotation Against :<span class="spncls"></span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlquotagainst" OnTextChanged="ddlquotagainst_TextChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="--- Select ---"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Job No."></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Manual"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>--%>

                                <%-- <div class="col-md-3">
                                    <asp:Label ID="lbl_JobNo" runat="server" class="control-label col-sm-6">Job No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txtJobNo"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter Job No." ControlToValidate="txtJobNo" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>--%>

                                <div class="col-md-3">
                                    <asp:Label ID="lblQuo_No" runat="server" class="control-label col-sm-6">Quotation No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txt_Quo_No" ReadOnly="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Select Quotation No." ControlToValidate="txt_Quo_No" ForeColor="Red"></asp:RequiredFieldValidator>

                                    <asp:HiddenField ID="hdnID" runat="server" />
                                    <br />
                                </div>

                                <div class="col-md-3">
                                    <asp:Label ID="Label15" runat="server" class="control-label col-sm-6">Against By :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" ID="ddlagainstby">
                                        <%--<asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>--%>
                                        <asp:ListItem Value="JobNo" Text="JobNo"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlservicetype">
                                        <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                                        <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                                        <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                                        <asp:ListItem Value="Testing" Text="Testing"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row">

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_Comp_name" runat="server" class="control-label col-sm-6">Customer Name :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" OnTextChanged="txt_Comp_name_TextChanged" AutoPostBack="true" onkeypress="return character(event)" class="form-control" ID="txt_Comp_name"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txt_Comp_name" runat="server">
                                    </asp:AutoCompleteExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_compname" runat="server" ErrorMessage="Please Enter Company Name" ControlToValidate="txt_Comp_name" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>
                                <br />
                                <div class="col-md-6">
                                    <asp:Label ID="lblsubcust" runat="server" class="control-label col-sm-6">Sub Customer :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlsubcustomer">
                                        <asp:ListItem Value="" Text="Select Sub Customer"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:DropDownList ID="ddlsubcustomer" class="control-label col-sm-6" AutoPostBack="true" runat="server" AppendDataBoundItems="True">                                    
                                    </asp:DropDownList>--%>
                                    <%--<asp:TextBox runat="server" AutoPostBack="true" onkeypress="return character(event)" class="form-control" ID="txtsubcustomer"></asp:TextBox>--%>
                                </div>
                                <br />

                                <div class="col-md-6">
                                    <asp:Label ID="LblQuo_Date" runat="server" class="control-label col-sm-6">Quotation Date. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" TextMode="Date" AutoPostBack="true" ID="Txt_Quo_Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_QuoDate" runat="server" ErrorMessage="Please Enter Quotation Date" ControlToValidate="Txt_Quo_Date" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_Expirydate" runat="server" class="control-label col-sm-6">Expiry Date. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" TextMode="Date" AutoPostBack="true" ID="txtexpirydate"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Expiry Date" ControlToValidate="txtexpirydate" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>

                            </div>
                            <div class="row">

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_address" runat="server" class="control-label col-sm-6">Address :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txt_Address" TextMode="Multiline" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_mobile" runat="server" class="control-label col-sm-6">Mobile No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" onkeypress="return isNumberKey(event)" MaxLength="11" ID="txt_Mobile" ReadOnly="true"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator_Mobile" runat="server" ErrorMessage="Please Enter Mobile No." ControlToValidate="txt_Mobile" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_Phone_No" runat="server" class="control-label col-sm-6">Phone No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" onkeypress="return isNumberKey(event)" MaxLength="13" class="form-control" ID="txt_Phoneno" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_GST" runat="server" class="control-label col-sm-6">GST No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txt_GST" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_kind_att" runat="server" class="control-label col-sm-6">Kind Att. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txt_kind_att"></asp:TextBox>
                                    <%--   <asp:DropDownList runat="server" class="form-control" ID="txt_kind_att">
                                        <asp:ListItem Value="" Text="select Kind Att"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_Statecode" runat="server" class="control-label col-sm-6">State Code :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox runat="server" class="form-control" ID="txt_state" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="Lbl_mail" runat="server" class="control-label col-sm-6">Mail ID :<span class="spncls">*</span></asp:Label>
                                    <%--<asp:LinkButton ID="lnkbtnAdd" runat="server" Font-Bold="true" CausesValidation="false" CommandArgument='<%#Eval("Custid") %>' OnClick="lnkbtnAdd_Click">+Add Mail</asp:LinkButton>--%>
                                    <%-- New added by Shubham Patl--%>
                                    <asp:LinkButton ID="lnkproduct" runat="server" CssClass="lnk " CausesValidation="false" OnClick="lnkproduct_Click">+Add Mail</asp:LinkButton>
                                    <%--End--%>
                                    <div class="row" id="maildiv" runat="server">
                                        <table class=" col-md-12">
                                            <tr>
                                                <td class="col-11">
                                                    <asp:TextBox ID="TXtMailtbl" TextMode="Email" class="form-control" Visible="false" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="col-1">
                                                    <asp:LinkButton runat="server" ID="Lnkbtn_addmail" Visible="false" CausesValidation="false" ToolTip="Add Mail ID"><i class="far fa-plus-square" aria-hidden="true" style="font-size: 24px"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:HiddenField ID="hiddencustomerID" runat="server" />
                                    <div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="Grd_MAIL" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                                AllowPaging="true" PagerStyle-CssClass="paging" OnRowDataBound="Grd_MAIL_RowDataBound" DataKeyNames="id">
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
                                                            <asp:Label ID="lbldesignation" Text='<%# Eval("designation") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select">
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
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label13" runat="server" class="control-label col-sm-6">Job No. :<span class="spncls">*</span></asp:Label>
                                    <div class="table-responsive">
                                        <asp:GridView ID="grdjobno" runat="server" AutoGenerateColumns="False" DataKeyNames="Jobno" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="grdjobno_RowDataBound" OnPageIndexChanging="grdjobno_PageIndexChanging" RowStyle-HorizontalAlign="Center"
                                            AllowPaging="true" PagerStyle-CssClass="paging">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsrno" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Job No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbljobno" Text='<%# Eval("Jobno") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblproduct" Text='<%# Eval("MateName") %>' runat="server"></asp:Label>
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
                                        <%--   <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please select at least one record."
                                            ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>--%>

                                        <br />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <h5 class="m-0 font-weight-bold">Component Details</h5>
                            <br />

                            <div class="row" id="componantdetails" runat="server">
                                <div class="col-md-12">
                                    <table class="table-responsive">
                                        <tr>
                                            <td style="width: 10px;">Job No.</td>
                                            <td style="width: 10px;">Product</td>
                                            <td style="width: 10px;">Component</td>
                                            <td style="width: 10px">Print Description</td>


                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddljobno" OnTextChanged="ddljobno_TextChanged" AutoPostBack="true" Width="230px" runat="server" AppendDataBoundItems="True" class="form-control">

                                                    <asp:ListItem Value="" Text="Select Job No.">

                                                    </asp:ListItem>
                                                </asp:DropDownList>

                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtpoduct" runat="server" AutoPostBack="true" Width="230px" class="form-control"></asp:TextBox>

                                            </td>

                                            <td>

                                                <asp:TextBox ID="txt_discription_Tbl" runat="server" OnTextChanged="txt_discription_Tbl_TextChanged" AutoPostBack="true" Width="230px" class="form-control"></asp:TextBox>

                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetDescriptionList" TargetControlID="txt_discription_Tbl" runat="server">
                                                </asp:AutoCompleteExtender>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtadddescription" runat="server" AutoPostBack="true" Width="230px" TextMode="MultiLine" Style="height: 37px;" class="form-control"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="width: 10px;">HSN/SAC</td>
                                            <td style="width: 10px;">Rate</td>
                                            <td style="width: 10px;">Unit</td>
                                            <td style="width: 10px;">Quantity</td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txt_hsn_Tbl" runat="server" Width="230px" class="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_rate_Tbl" AutoPostBack="true" OnTextChanged="txt_rate_Tbl_TextChanged" runat="server" Width="230px" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_unit_Tbl" onkeypress="return character(event)" runat="server" Width="230px" class="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_quntity_Tbl" OnTextChanged="txt_quntity_Tbl_TextChanged" AutoPostBack="true" runat="server" Width="230px" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="width: 10px;">Tax(%)</td>
                                            <td style="width: 10px;">Discount(%)</td>
                                            <td style="width: 10px;" runat="server" visible="false">Total Amount</td>
                                            <td style="width: 10px;">Total</td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txt_tax_Tbl" AutoPostBack="true" OnTextChanged="txt_tax_Tbl_TextChanged" runat="server" Width="230px" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_discount_Tbl" AutoPostBack="true" OnTextChanged="txt_discount_Tbl_TextChanged" runat="server" Width="230px" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>
                                            </td>
                                            <td id="tdl" runat="server" visible="false">
                                                <asp:TextBox ID="txt_total_amount_Tbl" ReadOnly="true" runat="server" Width="230px" class="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Total_Tbl" onkeypress="return isNumberKey(event)" ReadOnly="true" runat="server" Width="230px" class="form-control"></asp:TextBox>
                                            </td>
                                            <td style="text-align: center">

                                                <asp:LinkButton runat="server" ID="btn_add_more_Tbl" CausesValidation="false" ToolTip="Add Component" Width="100px" CssClass="btn btn-facebook" OnClick="btn_add_more_Tbl_Click1" Text="Add More" class="form-control"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>



                            <hr />
                            <div class="row" id="Quatationgrid">
                                <div class="col-md-12">
                                    <div class="table-responsive text-center">

                                        <asp:GridView ID="dgvProductDtl" runat="server" AllowPaging="false" CssClass="table" AutoGenerateColumns="false"
                                            OnRowEditing="dgvProductDtl_RowEditing" OnRowUpdated="dgvProductDtl_RowUpdated" OnRowDataBound="dgvProductDtl_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SN">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSRNO" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- New Codee for Job Days Count --%>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbljobDaysCount" ReadOnly="true" Text='<%# Eval("DaysSinceCreated") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- End  --%>

                                                <asp:TemplateField HeaderText="Job No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbljobno" ReadOnly="true" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtjobno" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("JobNo") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Product">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblproduct" ReadOnly="true" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtmatename" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("MateName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Component">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Description" ReadOnly="true" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Description" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Print Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_AddDescription" ReadOnly="true" runat="server" Text='<%# Eval("AddDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_AddDescription" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("AddDescription") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="HSN / SAC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblhsnsac" ReadOnly="true" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_HSN_SAC" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Rate" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRate" Width="80px" OnTextChanged="txtRate_TextChanged" AutoPostBack="true" ReadOnly="false" runat="server" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" ReadOnly="true" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtunit" Width="80px" AutoPostBack="true" ReadOnly="false" runat="server" Text='<%# Eval("Unit") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_quntity_grd" ReadOnly="true" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" onkeypress="return isNumberKey(event)" Text='<%# Eval("Quantity") %>' Width="60px" runat="server" OnTextChanged="txtQuantity_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Quantity" ControlToValidate="txtQuantity" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tax (%)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Tax" ReadOnly="true" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtTax" onkeypress="return isNumberKey(event)" OnTextChanged="txtTax_TextChanged" Width="60px" runat="server" Text='<%# Eval("Tax") %>' AutoPostBack="true"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Disc.(%)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Discount" ReadOnly="true" runat="server" Text='<%# Eval("Discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Discount" Width="60px" onkeypress="return isNumberKey(event)" OnTextChanged="txt_Discountt_TextChanged" runat="server" AutoPostBack="true" Text='<%# Eval("Discount") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Price" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalPrice" ReadOnly="true" runat="server" Text='<%# Eval("Total Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTotal" ReadOnly="true" Text='<%# Eval("Total") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>&nbsp;                                                 
                                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" OnClick="lnkbtnDelete_Click" CausesValidation="false"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="gv_update" Text="Update" CausesValidation="false" OnClick="gv_update_Click" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="gv_cancel" CausesValidation="false" OnClick="gv_cancel_Click" Text="Cancel" CssClass="btn btn-primary btn-sm " runat="server"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0755a1" ForeColor="White" Font-Bold="true" />
                                        </asp:GridView>
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
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
                                                    <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="txt_Subtotal"></asp:Label><br />
                                                </div>
                                            </div>
                                            <asp:Panel ID="taxPanel1" runat="server">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbl_cgst9" runat="server" class="control-label col-sm-6">CGST 9% Amount :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="txt_cgst9" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbl_sgst9" runat="server" class="control-label col-sm-6">SGST 9% Amount :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="txt_sgst9" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbligst" runat="server" class="control-label col-sm-6">IGST 18% Subtotal :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="txtigst" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="taxpanel2" runat="server">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label1" runat="server" class="control-label col-sm-6">CGST 6% Amount :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="Label2" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" class="control-label col-sm-6">SGST 6% Amount :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="Label4" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label5" runat="server" class="control-label col-sm-6">IGST 12% Subtotal :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="Label6" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="txtpanel3" runat="server">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" class="control-label col-sm-6">CGST 14% Amount :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="Label8" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" class="control-label col-sm-6">SGST 14% Amount :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="Label10" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" class="control-label col-sm-6">IGST 28% Subtotal :<span class="spncls"></span></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" class="control-label col-sm-6" ReadOnly="true" ID="Label12" Text="0"></asp:Label><br />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lbl_grandTotal" runat="server" class="control-label col-sm-6">Grand Total :<span class="spncls"></span></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox runat="server" class="form-control" ReadOnly="true" ID="txt_grandTotal"></asp:TextBox><br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Component Info." ControlToValidate="txt_grandTotal" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <hr />
                                        <h5 class="m-0 font-weight-bold">Terms & Condition</h5>
                                        <br />
                                        <br />
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_1" runat="server" class="control-label col-sm-6" for="cust">Term : 1<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_1" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Term" ControlToValidate="txt_term_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_1" runat="server" class="control-label col-sm-6" for="cust">Condition : 1<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_1" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_2" runat="server" class="control-label col-sm-6" for="cust">Term : 2<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_2" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Term" ForeColor="Red" ControlToValidate="txt_term_2" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_2" runat="server" class="control-label col-sm-6" for="cust">Condition : 2<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_2" runat="server" CssClass="form-control"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_2" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_3" runat="server" class="control-label col-sm-6" for="cust">Term : 3 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_3" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_3" runat="server" class="control-label col-sm-6" for="cust">Condition : 3 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_3" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_4" runat="server" class="control-label col-sm-6" for="cust">Term 4<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_4" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_4" runat="server" class="control-label col-sm-6" for="cust">Condition : 4 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_4" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_5" runat="server" class="control-label col-sm-6" for="cust">Term 5<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_5" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_5" runat="server" class="control-label col-sm-6" for="cust">Condition : 5 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_5" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_term_6" runat="server" class="control-label col-sm-6" for="cust">Term 6<span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_term_6" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lbl_condition_6" runat="server" class="control-label col-sm-6" for="cust">Condition : 6 <span class="spncls"></span></asp:Label>
                                                <asp:TextBox ID="txt_condition_6" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
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
                                    <asp:Label runat="server" Text="Send Quotation on Mail ID"></asp:Label>
                                    <br />
                                </center>
                            </div>
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <center>
                            <div class="col-md-6">
                                <br />
                                <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                &nbsp;&nbsp;        &nbsp;&nbsp;       
             <asp:Button ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " OnClick="btnCancel_Click" CausesValidation="False" Text="Cancel"></asp:Button>
                                &nbsp;&nbsp;        &nbsp;&nbsp; 
             <%--<asp:Button  ID="btnsendpo" runat="server" class="btn btn-primary col-sm-3" OnClick="btnsendpo_Click" CausesValidation="False"  Text="Send PO"   ></asp:Button>--%>

                                <asp:HiddenField runat="server" ID="hidden" />
                            </div>
                        </center>
                    </div>
                </div>

            </div>
        </div>

        <%--New Code by Shubham Patil--%>
        <asp:Button ID="btnprof" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="modelprofile" runat="server" TargetControlID="btnprof"
            PopupControlID="PopupAddDetail" OkControlID="Button2" />
        <asp:Panel ID="PopupAddDetail" runat="server" class="w3-panel w3-white panel1" Style="background-color: #f8f9fa; border: 1px solid #ccc; border-radius: 8px; padding: 30px; width: 600px;">

            <h4 style="background-color: #0755A1; color: white; font-weight: 300; padding: 10px; border-radius: 5px; text-align: center;">Add Mail </h4>
            <div class="row">
                <div class="col-md-6" style="margin-bottom: 20px;">
                    <asp:Label ID="Label18" runat="server" class="control-label" Style="font-weight: bold; display: block;">Contact Person Name: <span class="spncls">*</span></asp:Label>
                    <asp:TextBox runat="server" class="form-control" ID="txtcontactname" Style="width: 100%; height: 40px; border-radius: 4px; font-size: 16px;" />
                </div>

                <div class="col-md-6" style="margin-bottom: 20px;">
                    <asp:Label ID="Label19" runat="server" class="control-label" Style="font-weight: bold; display: block;">Mobile No.: <span class="spncls">*</span></asp:Label>
                    <asp:TextBox runat="server" class="form-control" ID="txtmobile" Style="width: 100%; height: 40px; border-radius: 4px; font-size: 16px;" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6" style="margin-bottom: 20px;">
                    <asp:Label ID="Label16" runat="server" class="control-label" Style="font-weight: bold; display: block;">Email: <span class="spncls">*</span></asp:Label>
                    <asp:TextBox runat="server" class="form-control" ID="txtemail" Style="width: 100%; height: 40px; border-radius: 4px; font-size: 16px;" />
                </div>

                <div class="col-md-6" style="margin-bottom: 20px;">
                    <asp:Label ID="Label17" runat="server" class="control-label" Style="font-weight: bold; display: block;">Designation: <span class="spncls">*</span></asp:Label>
                    <asp:TextBox runat="server" class="form-control" ID="txtdesination" Style="width: 100%; height: 40px; border-radius: 4px; font-size: 16px;" />
                </div>
            </div>
            <div class="col-md-12" style="text-align: center; margin-top: 15px;">
                <asp:Button ID="btnsave" runat="server" class="btn btn-primary"
                    Text="Save" OnClick="btnsave_Click" CausesValidation="False"
                    Style="margin-right: 10px; padding: 10px 20px; font-size: 16px;" />
                <asp:Button ID="Button2" runat="server" class="btn btn-secondary"
                    Text="Close" CausesValidation="False"
                    Style="padding: 10px 20px; font-size: 16px;" />
                <asp:HiddenField runat="server" ID="HiddenField1" />
            </div>

        </asp:Panel>
        <%--End--%>
    </form>

</asp:Content>

