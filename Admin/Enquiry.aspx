<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Enquiry.aspx.cs" Inherits="Admin_Enquiry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                    window.location.href = "../Admin/EnquiryList.aspx";
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
    </style>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("divdtls").offsetTop;
            window.scrollTo(0, target);
        }
    </script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <!-- Boostrap DatePciker JS  -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src='https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/jquery-ui.min.js' type='text/javascript'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="row">
                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                        <h5 class="m-0 font-weight-bold text-primary">Enquiry Master</h5>
                    </div>
                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between" style="margin-left: 66%;">
                        <asp:Button ID="btncreate1" runat="server" class="btn btn-primary btncreate" CausesValidation="False" Text="Enquiry List" OnClick="btncreate1_Click"></asp:Button>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row m-0">
                        <div class="col-md-6">
                            <asp:Label ID="lblCustname" runat="server" class="control-label col-sm-6" for="cust">Customer name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox ID="txtCustName" OnTextChanged="txtCustName_TextChanged" AutoPostBack="true" runat="server" class="form-control txtcustomer" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please fill Customer Name" ControlToValidate="txtCustName" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCompanyList"
                                TargetControlID="txtCustName" Enabled="true">
                            </asp:AutoCompleteExtender>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkBtmUpdate" runat="server" CssClass="lnk " OnClick="lnkBtmUpdate_Click" CausesValidation="false" Visible="false">+EDIT</asp:LinkButton>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblstatecode" class="control-label col-sm-6" runat="server">State Code :<span class="spncls">*</span></asp:Label>
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorState" runat="server" ControlToValidate="DropDownListcustomer" InitialValue="" ErrorMessage="Please select a state." ForeColor="Red" />
                        </div>
                    </div>
                    <div class="row m-0">
                        <div class="col-md-6">
                            <asp:Label ID="lbladd1" class="control-label col-sm-6" runat="server">Address Line 1 :<span class="spncls"></span></asp:Label>
                            <asp:TextBox TextMode="MultiLine" runat="server" type="Text" class="form-control" ID="txtAddresline1" name="Address1" />
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatoradd" runat="server" ErrorMessage="Please fill Address" ControlToValidate="txtAddresline1" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblarea" class="control-label col-sm-6" runat="server">Area :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtarea" name="Area" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorarea" runat="server" ErrorMessage="Please fill Area" ControlToValidate="txtarea" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <br />
                    <div class="row m-0">
                        <div class="col-md-6">
                            <asp:Label ID="lbltemail" class="control-label " runat="server">Email :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" type="Email" class="form-control" ID="txttemail" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please fill Email" ControlToValidate="txttemail" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblmob" class="control-label col-sm-6" runat="server">Mobile No :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtMobileNo" name="MobileNo" MaxLength="10" MinLength="10" onkeypress="return isNumberKey(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Mobile Number" ControlToValidate="txtMobileNo" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-2" visible="false">
                            <asp:Label ID="lblisstatus" class="control-label col-sm-6" runat="server" Visible="false">Is Active :<span class="spncls">*</span></asp:Label>
                            <asp:DropDownList ID="DropDownListisActive" runat="server" class="form-control" Visible="false">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row m-0">
                        <div class="col-md-3">
                            <asp:Label ID="lblpostal" class="control-label col-sm-6" runat="server">Postal Code :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtPostalCode" name="PostalCode"
                                onkeypress="return isNumberKey(event)" MaxLength="6" />
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorpostal" runat="server" ErrorMessage="Please fill Postal Code" ControlToValidate="txtPostalCode" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-5">
                            <asp:Label ID="lblcity" class="control-label col-sm-6" runat="server">City :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtcity" name="City" onkeypress="return character(event)" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorCity" runat="server" ErrorMessage="Please fill City" ControlToValidate="txtcity" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblcountry" class="control-label col-sm-6" runat="server">Country :<span class="spncls"></span> </asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtcountry" name="country" onkeypress="return character(event)" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Country" ControlToValidate="txtcountry" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                </div>
                <hr />
                <h5 class="m-0 font-weight-bold">Product Information</h5>
                <div class="table-responsive">
                    <div class="row m-2" id="divdtls">
                        <div class="col-md-5">
                            <asp:Label ID="lblproduct" runat="server" class="control-label col-sm-6 custlbl">Product Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtproductname" name="Search" placeholder="Search Product" AutoPostBack="true" onkeypress="return character(event)" OnTextChanged="txtproductname_TextChanged" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetProductList" TargetControlID="txtproductname" runat="server">
                            </asp:AutoCompleteExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Product Name" ControlToValidate="txtproductname" ForeColor="Red"></asp:RequiredFieldValidator>
                             <asp:Label ID="lblProduct1" runat="server" class="control-label col-sm-6 custlbl" Visible="false"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label3" class="control-label " runat="server">Product Image :<span class="spncls"></span></asp:Label>
                            <asp:FileUpload ID="FileUpload" CssClass="form-control" runat="server" />
                            <asp:Label ID="lblPath" runat="server" Text=""></asp:Label>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please fill Email" ControlToValidate="txttemail" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-3">
                            <asp:Image ID="imgProduct" runat="server" CssClass="img-responsive" Width="190px" Height="110px" Style="border: 1px solid #acacac;" Visible="false" />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label4" runat="server" class="control-label col-sm-4">Service Type :<span class="spncls">*</span></asp:Label>
                            <asp:DropDownList runat="server" class="form-control" ID="ddlservicetype">
                                <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                                <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                                <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                            </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldServiceType" runat="server" ControlToValidate="ddlservicetype" InitialValue="" ErrorMessage="Please select product service type." ForeColor="Red" />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Lable2" class="control-label " runat="server">Other information :<span class="spncls"></span></asp:Label>
                            <asp:TextBox TextMode="MultiLine" runat="server" class="form-control" ID="txtotherinfo" name="Customer" />
                        </div>
                    </div>
                </div>
            </div>
            <center>
                <div class="col-md-6">
                    <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary " Text="Save" OnClick="btnSubmit_Click"></asp:Button>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnCancel" runat="server" class="btn btn-primary cancelbutton" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False"></asp:Button>
                </div>
            </center>
            <asp:HiddenField runat="server" ID="hidden" />
        </div>
    </form>
</asp:Content>
