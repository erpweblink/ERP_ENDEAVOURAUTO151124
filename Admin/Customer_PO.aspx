<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Customer_PO.aspx.cs" Inherits="Admin_Customer_PO" %>

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
                window.location.href = "../Admin/CustomerPO_List.aspx";
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
                window.location.href = "../Admin/CustomerPO_List.aspx";
            })
        };
    </script>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("customerpo").offsetTop;
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
                    <h5 class="m-0 font-weight-bold text-primary" id="headerreport" runat="server">Customer PO</h5>
                </div>
                <hr />
                <div class="card-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnID" runat="server" />
                            <asp:HiddenField ID="hdnPoProductTot" runat="server" />
                            <asp:HiddenField ID="taxhidden" runat="server" />
                            <div class="row">
                                  <div class="col-md-6">
                                    <asp:Label ID="Label4" runat="server" class="control-label col-sm-4">Against By :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" OnTextChanged="ddlagainstby_TextChanged" AutoPostBack="true" ID="ddlagainstby">
                                        <asp:ListItem Value="--Select--" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="Direct" Text="Direct"></asp:ListItem>
                                        <asp:ListItem Value="Order" Text="Order"></asp:ListItem>                                
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlservicetype">
                                        <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                                        <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                                        <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_Customer_name" runat="server" class="control-label col-sm-6" for="cust">Customer Name :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_Customer_name" OnTextChanged="txt_Customer_name_TextChanged" AutoPostBack="true" onkeypress="return character(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please Enter Customer Name" ControlToValidate="txt_Customer_name" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GeCustomerList" TargetControlID="txt_Customer_name" runat="server">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblsubcust" runat="server" class="control-label col-sm-6" for="cust">Sub Customer :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtsubcust" AutoPostBack="true" onkeypress="return character(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblQuotationno" runat="server" class="control-label col-sm-6" for="cust">Quotation No. :<span class="spncls">*</span></asp:Label>
                                    <asp:DropDownList ID="ddlquotationno" OnTextChanged="ddlquotationno_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="" Text="Select Quotation No.">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtinvno" runat="server" OnTextChanged="txtinvno_TextChanged"  AutoPostBack="true" CssClass="form-control"></asp:TextBox>--%>
                                </div>
                                <div class="col-md-3" runat="server" visible="false">
                                    <asp:Label ID="lbl_job_no" runat="server" class="control-label col-sm-6" for="cust">Job No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_job_no" runat="server" OnTextChanged="txt_job_no_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GeJobNoList" TargetControlID="txt_job_no" runat="server">
                                    </asp:AutoCompleteExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please Enter Job Number" ControlToValidate="txt_job_no" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lbl_ref_no" runat="server" class="control-label col-sm-6" for="cust">Ref. No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_ref_no" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lbl_po_no" runat="server" class="control-label col-sm-6" for="cust">P.O. No. :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_po_no" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter PO Number" ControlToValidate="txt_po_no" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lbl_po_date" runat="server" Text="" Class="control-label col-sm-6" for="cust">P.O. Date :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_po_date" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Po Date" ControlToValidate="txt_po_date" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="col-6" id="chk" runat="server">
                                        <asp:CheckBox ID="check_addresss" OnCheckedChanged="check_address_CheckedChanged" AutoPostBack="true" runat="server" />
                                        <asp:Label ID="Label3" runat="server" Text=""><b>Copy Address</b></asp:Label>
                                    </div>
                                    <asp:Label ID="lblshippingaddr" runat="server" Text="" Class="control-label col-sm-6">Shipping Address :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txtShippingAddr" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please Enter Shipping Address" ControlToValidate="txtShippingAddr" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                </div>
                                <%--</div>--%>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_delivery_address" runat="server" Text="" Class="control-label col-sm-6">Address :<span class="spncls">*</span></asp:Label>
                                    <asp:TextBox ID="txt_delivery_address" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Address" ControlToValidate="txt_delivery_address" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_kind_att" runat="server" Text="" Class="control-label col-sm-6" for="cust">Kind Att. :<span class="spncls"></span></asp:Label>
                                    <asp:DropDownList ID="txt_kind_att" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="" Text="Select Kind Att."></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Kind Att." ControlToValidate="txt_kind_att" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_gst_no" runat="server" class="control-label col-sm-6">GST No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_gst_no" MaxLength="15" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_mobile_no" runat="server" class="control-label col-sm-6">Mobile No. :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_mobile_no" onkeypress="return isNumberKey(event)" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter Mobile No" ControlToValidate="txt_mobile_no" ForeColor="Red" SetFocusOnError="false"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label2" runat="server" class="control-label col-sm-6">State Code :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txtstatecode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_pay_tern" runat="server" class="control-label col-sm-6">Pay Terms :<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_pay_term" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">(Attach P.O.) :</label><br />
                                    <asp:FileUpload ID="FileUpload" CssClass="form-control" runat="server" />
                                    <asp:Label ID="lblPath" runat="server" Text=""></asp:Label>
                                </div>



                                <%--</div>--%>
                                <div class="col-md-6">
                                    <asp:Label ID="lbl_email_id" runat="server" class="control-label col-sm-6">Email ID :<span class="spncls"></span></asp:Label>
                                    <div class="table-responsive">
                                        <asp:GridView ID="Grd_MAIL" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                            AllowPaging="true" PagerStyle-CssClass="paging" DataKeyNames="id">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No.">
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
                            <h5 class="m-0 font-weight-bold">Components Details</h5>
                            <br />
                            <div class="row" id="tbl" runat="server">
                                <div class="col-md-12">
                                    <center>
                                    <table class="table-responsive" >
                                        <tr>
                                            <td style="width: 10px;">Job No.</td>
                                            <td style="width: 10px;">Product</td>
                                            <td style="width: 10px;">Component</td>
                                 <%--           <td style="width: 10px;">Description</td>--%>
                                            <td style="width: 10px;">Print Description</td>
                                            <td style="width: 10px;">HSN/SAC</td>
                                            <td style="width: 10px;">Rate</td>
                                            <td style="width: 10px;">Unit</td>
                                            <td style="width: 10px;">Quntity</td>
                                            <td style="width: 10px;">Tax(%)</td>
                                            <td style="width: 10px;">Discount(%)</td>
                                            <td style="width: 10px;">Total_Amount</td>
                                        </tr>
                                        <tr>
                                         <%--     <td>
                                                <asp:TextBox ID="txt_jobno" runat="server" Width="100px"></asp:TextBox>
                                            </td>--%>
                                            <td>
                                        <asp:DropDownList ID="txt_jobno" OnTextChanged="txt_jobno_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server" Width="150px">
                                        <asp:ListItem Value="" Text="Select Job No.">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                            </td>
                                             <td>
                                                <asp:TextBox ID="txtpoduct" runat="server" AutoPostBack="true" Width="230px"></asp:TextBox>
                                             <td>
                                                <asp:TextBox ID="txt_discription" runat="server" OnTextChanged="txt_discription_TextChanged" AutoPostBack="true" Width="230px"></asp:TextBox>
                                                 <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetDescriptionList" TargetControlID="txt_discription" runat="server">
                                        </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_printdescription" runat="server"  AutoPostBack="true" Width="230px"  TextMode="MultiLine"></asp:TextBox>                                            
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_hsn" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_rate" AutoPostBack="true" OnTextChanged="txt_rate_TextChanged" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                             </td>
                                               <td>
                                                <asp:TextBox ID="txt_unit" onkeypress="return character(event)" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_quntity" AutoPostBack="true" OnTextChanged="txt_quntity_TextChanged" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                           <%--  <td>
                                                <asp:TextBox ID="txt_quntity_Tbl" OnTextChanged="txt_quntity_Tbl_TextChanged" AutoPostBack="true" runat="server" Width="100px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>--%>
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
                                                <asp:Button ID="btn_add_more" CausesValidation="false" OnClick="btn_add_more_Click" runat="server" Text="Add More" CssClass="btn btn-primary" />
                                            </td>
                                        </tr>
                                    </table>
                                        </center>
                                </div>
                            </div>
                            <hr />
                            <div class="table-responsive text-center">
                                <asp:GridView ID="quatationgrid" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="false" Width="100%" OnRowEditing="quatationgrid_RowEditing" OnRowDataBound="quatationgrid_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_sr_no" ReadOnly="true" runat="server" Text='<%# Container.DataItemIndex+1%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No.">
                                            <ItemTemplate>
                                                <asp:Label ID="LblJobNo" ReadOnly="true" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Jobno_grd" runat="server" Width="150" Text='<%# Eval("JobNo") %>'></asp:TextBox>
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
                                        <asp:TemplateField HeaderText="Component">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_discription_grd" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_discription_grddd" runat="server" Width="150" Text='<%# Eval("CompName") %>'></asp:TextBox>
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

                                        <%--   <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_discription_grd" ReadOnly="true" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                            </ItemTemplate>
                                              <EditItemTemplate>
                                                <asp:TextBox ID="txt_discription_grd"  runat="server" Width="150"  Text='<%# Eval("CompName") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="HSN / SAC">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_hsn_grd" ReadOnly="true" runat="server" Text='<%# Eval("HSN") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_hsn_grd" runat="server" Width="150" Text='<%# Eval("HSN") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_rate_grd" ReadOnly="true" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_rate_Quogrd" OnTextChanged="txt_rate_Quogrd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_unit_grd" ReadOnly="true" runat="server" Text='<%# Eval("Units") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_unit_grd" onkeypress="return isNumberKey(event)" runat="server" Width="150" Text='<%# Eval("Units") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quntity">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_quntity_grd" ReadOnly="true" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_quntity_Quogrd" OnTextChanged="txt_quntity_Quogrd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="150" AutoPostBack="true" Text='<%# Eval("Qty") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tax">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_tax_grd" ReadOnly="true" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_tax_quogrd" OnTextChanged="txt_tax_quogrd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="120" AutoPostBack="true" Text='<%# Eval("Tax") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount %">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_discount_grd" ReadOnly="true" runat="server" Text='<%# Eval("Disc_per") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_discount_Quogrd" OnTextChanged="txt_discount_Quogrd_TextChanged" onkeypress="return isNumberKey(event)" runat="server" Width="100" AutoPostBack="true" Text='<%# Eval("Disc_per") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_total_amount_grd" ReadOnly="true" runat="server" Text='<%# Eval("total") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_total_amount_Quogrd" ReadOnly="true" runat="server" Width="160" Text='<%# Eval("total") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" OnClick="lnkbtnDelete_Click" CausesValidation="false"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="gv_Qupdate" Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " OnClick="gv_Qupdate_Click" runat="server"></asp:LinkButton><br />
                                                &nbsp;
                                                <asp:LinkButton ID="gv_cancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " OnClick="gv_cancel_Click" runat="server"></asp:LinkButton>
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
                            <br />
                            <div class="row" id="customerpo">
                                <div class="table-responsive text-center">
                                    <asp:GridView ID="gvPurchaseRecord" runat="server" AutoGenerateColumns="false" OnRowEditing="gvPurchaseRecord_RowEditing" OnRowDataBound="gvPurchaseRecord_RowDataBound" CssClass="grid" AllowPaging="false" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_sr_no" ReadOnly="true" runat="server" Text='<%# Container.DataItemIndex+1%>'></asp:Label>
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
                                            <%--           //New added by shubham--%>
                                            <asp:TemplateField HeaderText="Product">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblproduct" ReadOnly="true" runat="server" Text='<%# Eval("MateName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtmatename" TextMode="multiline" Width="200px" Height="100px" runat="server" Text='<%# Eval("MateName") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <%--       End --%>

                                            <asp:TemplateField HeaderText="Component">
                                                <%--           <asp:TemplateField HeaderText="Description">--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_discription_grd" runat="server" Text='<%# Eval("Discription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_discription_grdd" runat="server" Width="150" Text='<%# Eval("Discription") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <%--Added  By  shubham--%>
                                            <asp:TemplateField HeaderText="Print Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprintdescription" ReadOnly="true" runat="server" Text='<%# Eval("PrintDescription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_printdescription_grd" runat="server" Width="150" Text='<%# Eval("PrintDescription") %>'></asp:TextBox>
                                                </EditItemTemplate>

                                            </asp:TemplateField>
                                            <%--End  By  shubham--%>
                                            <asp:TemplateField HeaderText="HSN / SAC">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_hsn_grd" ReadOnly="true" runat="server" Text='<%# Eval("HSN/SAC") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_hsn_grdd" runat="server" Width="150" Text='<%# Eval("HSN/SAC") %>'></asp:TextBox>
                                                </EditItemTemplate>
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
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_unit_grdd" runat="server" Width="150" Text='<%# Eval("Unit") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quntity">
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
                                                    <asp:Label ID="lbl_total_amount_grd" ReadOnly="true" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_total_amount_grd" ReadOnly="true" runat="server" Width="160" Text='<%# Eval("Total_Amount") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_edit" CausesValidation="false" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="gv_update" Text="Update" CausesValidation="false" CssClass="btn btn-primary btn-sm " OnClick="gv_update_Click" runat="server"></asp:LinkButton><br />
                                                    &nbsp;
                                                <asp:LinkButton ID="gv_cancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary btn-sm " OnClick="gv_cancel_Click" runat="server"></asp:LinkButton>
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

                            <br />
                            <div class=" form-group row">
                                <div class="col-md-6">
                                </div>

                                <div class="col-md-4" style="text-align: end">
                                    <asp:Label ID="lbl_total" runat="server" Text="Subtotal"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_total" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>
                                <div class="col-md-6"></div>
                                <div class="col-md-4" style="text-align: end">
                                    <asp:Label ID="lbl_cgst_amt" runat="server" CssClass="lbl" Text="CGST Amount"></asp:Label>
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
                                    <asp:Label ID="lbl_sgst_amt" runat="server" Text="SGST Amount"></asp:Label>
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
                                    <asp:Label ID="lbl_igst" runat="server" Text="IGST Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_igst_amt" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>

                                <div class="col-md-6"></div>
                                <div class="col-md-4" style="text-align: end">
                                    <asp:Label ID="lbl_round_off" runat="server" Text="Round Off"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txt_round_off" OnTextChanged="txt_round_off_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox><br />
                                </div>

                                <div class="col-md-6"></div>
                                <div class="col-md-4" style="text-align: end">
                                    <asp:Label ID="lbl_grand_total" runat="server" Text="Grand Total"></asp:Label>
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
                                    <asp:TextBox ID="txt_term_1" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Term" ControlToValidate="txt_term_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_condition_1" runat="server" class="control-label col-sm-6" for="cust">Condition 1<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_condition_1" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_term_2" runat="server" class="control-label col-sm-6" for="cust">Term 2<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_term_2" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Term" ForeColor="Red" ControlToValidate="txt_term_2" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lbl_condition_2" runat="server" class="control-label col-sm-6" for="cust">Condition 2<span class="spncls"></span></asp:Label>
                                    <asp:TextBox ID="txt_condition_2" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Enter Condition" ControlToValidate="txt_condition_2" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <br />
                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btn_save" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btn_save_Click"   ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " OnClick="btnCancel_Click"  CausesValidation="False"  Text="Cancel"   ></asp:Button>
                               <asp:HiddenField runat="server" ID="hidden" /> 
                        <asp:HiddenField runat="server" ID="statecodehidden" /> 
                         
            </div></center>
                </div>
            </div>



        </div>

    </form>
</asp:Content>

