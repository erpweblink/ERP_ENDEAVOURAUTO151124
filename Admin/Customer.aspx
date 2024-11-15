<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Reception_Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function HideLabel(msg, flg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                if (flg == '1') {
                    window.location.href = "../Admin/CustomerList.aspx";
                }
                else if (flg == '0') {
                    window.location.href = "../Admin/InwardEntry.aspx";
                }
                else {
                    window.location.href = "../Admin/Quotation_Master.aspx";
                }
            })
        };
    </script>
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
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Customer Master</h5>
                </div>
                <div class="card-body">
                    <div class="row m-0">
                        <div class="col-md-6">
                            <asp:Label ID="lblCustname" runat="server" class="control-label col-sm-6" for="cust">Customer name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control txtcustomer" ID="txtCustName" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please fill Customer Name" ControlToValidate="txtCustName" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblgstno" class="control-label col-sm-6" runat="server">GST No. :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtgstno" name="GSTNo" MaxLength="15" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill GST No." ControlToValidate="txtgstno" ForeColor="Red"></asp:RequiredFieldValidator>--%>

                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblpan" class="control-label col-sm-6" runat="server">PAN No. :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtpanno" name="PanNo" MaxLength="10" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill PAN No." ControlToValidate="txtpanno" ForeColor="Red"></asp:RequiredFieldValidator>--%>

                        </div>
                    </div>
                    <div class="row m-0">
                        <div class="col-md-6">
                            <asp:Label ID="lblstatecode" class="control-label col-sm-6" runat="server">State Code :</asp:Label>
                            <asp:DropDownList ID="DropDownListcustomer" runat="server" class="form-control">
                                <asp:ListItem Value="" Text="Select State"></asp:ListItem>
                                <asp:ListItem Text="1 JAMMU AND KASHMIR"></asp:ListItem>
                                <asp:ListItem Text="2 HIMACHAL PRADESH"></asp:ListItem>
                                <asp:ListItem Text="3 PUNJAB"></asp:ListItem>
                                <asp:ListItem Text="4 CHANDIGARH"></asp:ListItem>
                                <asp:ListItem Text="5 UTTARAKHAND"></asp:ListItem>
                                <asp:ListItem Text="6 HARYANA"></asp:ListItem>
                                <asp:ListItem Text="7 DELHI"></asp:ListItem>
                                <asp:ListItem Text="8 RAJASTHAN"></asp:ListItem>
                                <asp:ListItem Text="9 UTTAR PRADESH"></asp:ListItem>
                                <asp:ListItem Text="10 BIHAR"></asp:ListItem>
                                <asp:ListItem Text="11 SIKKIM"></asp:ListItem>
                                <asp:ListItem Text="12 ARUNACHAL PRADESH"></asp:ListItem>
                                <asp:ListItem Text="13 NAGALAND"></asp:ListItem>
                                <asp:ListItem Text="14 MANIPUR"></asp:ListItem>
                                <asp:ListItem Text="15 MIZORAM"></asp:ListItem>
                                <asp:ListItem Text="16 TRIPURA"></asp:ListItem>
                                <asp:ListItem Text="17 MEGHLAYA"></asp:ListItem>
                                <asp:ListItem Text="18 ASSAM"></asp:ListItem>
                                <asp:ListItem Text="19 WEST BENGAL"></asp:ListItem>
                                <asp:ListItem Text="20 JHARKHAND"></asp:ListItem>
                                <asp:ListItem Text="21 ODISHA"></asp:ListItem>
                                <asp:ListItem Text="22 CHATTISGARH"></asp:ListItem>
                                <asp:ListItem Text="23 MADHYA PRADESH"></asp:ListItem>
                                <asp:ListItem Text="24 GUJARAT"></asp:ListItem>
                                <asp:ListItem Text="25 DAMAN AND DIU"></asp:ListItem>
                                <asp:ListItem Text="26 DADRA AND NAGAR HAVELI"></asp:ListItem>
                                <asp:ListItem Text="27 MAHARASHTRA"></asp:ListItem>
                                <asp:ListItem Text="28 ANDHRA PRADESH (OLD)"></asp:ListItem>
                                <asp:ListItem Text="29 KARNATAKA"></asp:ListItem>
                                <asp:ListItem Text="30 GOA"></asp:ListItem>
                                <asp:ListItem Text="31 LAKSHWADEEP"></asp:ListItem>
                                <asp:ListItem Text="32 KERALA"></asp:ListItem>
                                <asp:ListItem Text="33 TAMIL NADU"></asp:ListItem>
                                <asp:ListItem Text="34 PUDUCHERRY"></asp:ListItem>
                                <asp:ListItem Text="35 ANDAMAN AND NICOBAR ISLANDS"></asp:ListItem>
                                <asp:ListItem Text="36 TELANGANA"></asp:ListItem>
                                <asp:ListItem Text="37 ANDHRA PRADESH (NEW)"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lbladd1" class="control-label col-sm-6" runat="server">Address Line 1 :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtAddresline1" name="Address1" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatoradd" runat="server" ErrorMessage="Please fill Address" ControlToValidate="txtAddresline1" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row m-2">
                        <div class="col-md-6">
                            <asp:Label ID="lbladd2" class="control-label col-sm-6" runat="server">Address Line 2 :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtadreLine2" name="Address2" />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lbladd3" class="control-label col-sm-6" runat="server">Address Line 3 :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtadreLine3" name="Address3" />
                        </div>
                    </div>
                    <div class="row m-3">
                        <div class="col-md-6">
                            <asp:Label ID="lblarea" class="control-label col-sm-6" runat="server">Area :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtarea" name="Area" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorarea" runat="server" ErrorMessage="Please fill Area" ControlToValidate="txtarea" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblcity" class="control-label col-sm-6" runat="server">City :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtcity" name="City" onkeypress="return character(event)" />
                        </div>
                    </div>
                    <div class="row m-2">
                        <div class="col-md-6">
                            <asp:Label ID="lblcountry" class="control-label col-sm-6" runat="server">Country :<span class="spncls">*</span> </asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtcountry" name="country" onkeypress="return character(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Country" ControlToValidate="txtcountry" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblpostal" class="control-label col-sm-6" runat="server">Postal Code :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtPostalCode" name="PostalCode"
                                onkeypress="return isNumberKey(event)" MaxLength="6" />
                            <%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorpostal" runat="server" ErrorMessage="Please fill Postal Code" ControlToValidate="txtPostalCode" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="row m-2">
                        <div class="col-md-6">
                            <asp:Label ID="lblmob" class="control-label col-sm-6" runat="server">Mobile No :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtMobileNo" name="MobileNo" MaxLength="11" MinLength="10" onkeypress="return isNumberKey(event)" />
                            <%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Mobile Number" ControlToValidate="txtMobileNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblisstatus" class="control-label col-sm-6" runat="server">Is Active :</asp:Label>
                            <asp:DropDownList ID="DropDownListisActive" runat="server" class="form-control">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <hr />
                    <h5 class="m-0 font-weight-bold">Contact Details</h5>
                    <div class="table-responsive">
                        <div class="row m-2" id="divdtls">
                            <div class="col-md-3">
                                <asp:Label ID="lblcontactperson" class="control-label " runat="server">Contact Person :<span class="spncls"></span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtcustomername" />
<%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Contact Person" ControlToValidate="txtcustomername" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblpersonno" class="control-label " runat="server">Contact Person No. :<span class="spncls"></span></asp:Label>
                                <asp:TextBox runat="server" class="form-control " MaxLength="11" ID="txtcustomernameno" onkeypress="return isNumberKey(event)" /></td>  
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lbltemail" class="control-label " runat="server">Email :<span class="spncls"></span></asp:Label>
                                <asp:TextBox runat="server" type="Email" class="form-control" ID="txttemail" TextMode="Email" />
<%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please fill Email" ControlToValidate="txttemail" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lbldesignation" class="control-label " runat="server">Designation :<span class="spncls"></span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtdesignation" />
                            </div>
                            <div class="col-md-1">
                                <br />
                                <asp:Button ID="btnadd" runat="server" CssClass="btn btn-primary " Text="Add" OnClick="btnadd_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                    <div style="width: 100%; padding: 20px;">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_Customercontact" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact Person">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomername" Text='<%# Eval("colFirst") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact Person No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomernameno" runat="server" Text='<%# Eval("colsecond") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email">
                                        <ItemTemplate>
                                            <asp:Label ID="lblemailperson" runat="server" Text='<%# Eval("colemail") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesignation" runat="server" Text='<%# Eval("coldesignation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" OnClick="lnkbtnDelete_Click" CausesValidation="false"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                <center>
                <div class="col-md-6">
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary cancelbutton" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False"></asp:Button>
             </div>
                </center>
                <asp:HiddenField runat="server" ID="hidden" />
            </div>
        </div>
        </div>
      
    </form>
</asp:Content>

