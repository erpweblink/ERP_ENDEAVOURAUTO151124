<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="InwardEntry.aspx.cs" Inherits="Reception_InwardEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <!-- Boostrap DatePciker JS  -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src='https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/jquery-ui.min.js' type='text/javascript'></script>
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/InwardEntryList.aspx";
            })
        };
    </script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <style>
        .Cust {
            width: 408px;
            margin-left: 11px;
        }

        .lnk {
            font-weight: bolder;
            font-size: large;
        }

        .spncls {
            color: red;
        }

        .cls {
            width: 400px;
            margin-left: -3px;
        }

        .custlbl {
            margin-left: 11px;
        }

        @media only screen and (max-width: 767px) {
            .cls {
                width: 100%;
                margin-left: -3px;
            }
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Inward Entry</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblJobNo" runat="server" class="control-label col-sm-6" for="cust">Job No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtJobNo" ReadOnly="true" />
                            <br />
                        </div>
                        <div class="col-md-6">

                            <asp:Label ID="lblDateIn" runat="server" class="control-label col-sm-6" for="cust">Date In :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control myDate cls" ID="txtDateIn" OnTextChanged="txtDateIn_TextChanged" AutoPostBack="True" TextMode="Date" />&nbsp;&nbsp;                         
                            <br />
                        </div>
                        <div class="col-md-6">

                            <asp:Label ID="lblCustName" runat="server" class="control-label col-sm-6 custlbl">Customer Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtcustomername" name="Search" placeholder="Search Customer" onkeypress="return character(event)" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtcustomername" runat="server">
                            </asp:AutoCompleteExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Customer Name" ControlToValidate="txtcustomername" ForeColor="Red"></asp:RequiredFieldValidator>

                            <%-- <asp:DropDownList ID="DropDownCust" runat="server" class="form-control Cust inwardcust" AppendDataBoundItems="True">
                                    <asp:ListItem Value="" Text="Select Customer">

                                    </asp:ListItem>
                                </asp:DropDownList>--%>

                                &nbsp;&nbsp;<asp:LinkButton ID="lnkBtmNew" runat="server" CssClass="lnk " OnClick="lnkBtmNew_Click" CausesValidation="false">+ADD</asp:LinkButton>

                            <br />
                        </div>

                        <div class="col-md-6">

                            <asp:Label ID="lblproduct" runat="server" class="control-label col-sm-6 custlbl">Product Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtproductname" name="Search" placeholder="Search Product" OnTextChanged="txtproductname_TextChanged" AutoPostBack="true" onkeypress="return character(event)" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetProductList" TargetControlID="txtproductname" runat="server">
                            </asp:AutoCompleteExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Product Name" ControlToValidate="txtproductname" ForeColor="Red"></asp:RequiredFieldValidator>

                            <%-- <asp:DropDownList ID="ddlproduct" runat="server" class="form-control Cust inwardcust" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlproduct_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="" Text="Select Product">
                                    </asp:ListItem>
                                </asp:DropDownList>--%>

                                &nbsp;&nbsp;<asp:LinkButton ID="lnkproduct" runat="server" CssClass="lnk " CausesValidation="false" OnClick="lnkproduct_Click">+ADD</asp:LinkButton>

                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblsubcust" runat="server" class="control-label col-sm-6" for="cust">Sub Customer :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtsubcust" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Serial No" ControlToValidate="txtSrNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label5" runat="server" class="control-label col-sm-6" for="cust">Services :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtservices" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Serial No" ControlToValidate="txtSrNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label6" runat="server" class="control-label col-sm-6" for="cust">Customer Challan No./RGP No. :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtcustomerno" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Serial No" ControlToValidate="txtSrNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblbranch" runat="server" class="control-label col-sm-6" for="cust">Branch :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtbranch" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Serial No" ControlToValidate="txtSrNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>


                        <div class="col-md-6">
                            <asp:Label ID="lblSerialNo" runat="server" class="control-label col-sm-6" for="cust">Serial No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtSrNo" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Serial No" ControlToValidate="txtSrNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblModelNo" runat="server" class="control-label col-sm-6" for="cust">Model No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtModelNo" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Model No" ControlToValidate="txtModelNo" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblMateriStatus" runat="server" class="control-label col-sm-6" for="cust">Status :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtMateriStatus" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Status" ControlToValidate="txtMateriStatus" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblfinalstatus" runat="server" class="control-label col-sm-6" for="cust">Final Status :<span ></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtfinalstatus" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please fill Material Status" ControlToValidate="txtMateriStatus" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblproductfaulty" runat="server" class="control-label col-sm-6" for="cust">Product Fault :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtproductfaulty" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Product fault " ControlToValidate="txtproductfaulty" ForeColor="Red"></asp:RequiredFieldValidator>--%>

                            <%--  <asp:SqlDataSource ID="SqlDataSourceTest" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommand="SELECT [empcode], [name] FROM [employees]"></asp:SqlDataSource>--%>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please fill Employee Name" ControlToValidate="DropDownListtest" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label1" runat="server" class="control-label col-sm-6" for="cust">Repeated Job No. :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtrepeatedno" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label2" runat="server" class="control-label col-sm-6">Repeated Date :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" Type="Date" class="form-control" ID="txtrepeateddate" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblotherinfo" runat="server" class="control-label col-sm-6" for="cust">Other Information :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtotherinfo" name="Customer" />
                            <%--TextMode="MultiLine"--%>
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">(Product Img) :</label><br />
                            <asp:FileUpload ID="FileUpload" CssClass="form-control" runat="server" />
                            <asp:Label ID="lblPath" runat="server" Text=""></asp:Label>
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type :<span class="spncls"></span></asp:Label>
                            <asp:DropDownList runat="server" class="form-control" ID="ddlservicetype">
                                <asp:ListItem Value="--Select--" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                                <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                                <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label3" runat="server" class="control-label col-sm-6" Visible="false">Repeated Job No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtrepetedjob" Visible="false" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label4" runat="server" class="control-label col-sm-6" Visible="false">Date :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" TextMode="Date" class="form-control" ID="txtdate" Visible="false" />
                            <br />
                        </div>

                    </div>
                    <center>
                        <div class="row ">
                            <div class="col-4">

                            </div>
                            <div class="col-md-1 col-xs-6 col-6">
                                <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary btnsubmitinward" Text="Submit"   OnClick="btnSubmit_Click"></asp:Button>
                            </div>
                            <div class="col-md-1 col-xs-6 col-6">
                                <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary btncancel" Text="Cancel"  CausesValidation="False" OnClick="btnCancel_Click" ></asp:Button>
                            </div>
                            <asp:HiddenField runat="server" ID="hidden" /> 
                        </div>                      
                       </center>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

