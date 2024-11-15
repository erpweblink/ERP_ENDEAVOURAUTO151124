<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="OutwardEntry.aspx.cs" Inherits="Reception_OutwardEntry" %>

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
                window.location.href = "../Admin/OutwardEntryList.aspx";
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
            margin-left: 13px;
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

        .Cust {
            width: 408px;
            margin-left: 11px;
        }

        .custlbl {
            margin-left: 11px;
        }

        .panel1 {
            background-color: lavender;
            position: fixed;
            z-index: 10002;
            left: 557.5px;
            top: 199px;
            width: 500px;
            height: 202px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Outward Entry</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblJobNo" runat="server" class="control-label col-sm-6">Job No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtJobNo" OnTextChanged="txtJobNo_TextChanged" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please fill Job No" ControlToValidate="txtJobNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetjobList" TargetControlID="txtJobNo" runat="server">
                            </asp:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6">

                            <asp:Label ID="lblDateout" runat="server" class="control-label col-sm-6">Date Out :<span class="spncls"></span></asp:Label>


                            <asp:TextBox runat="server" CssClass="form-control " ID="txtDateout" AutoPostBack="True" TextMode="Date" />&nbsp;&nbsp;
                           
                            <br />
                        </div>
                        <div class="col-md-6">
                            <%--<div class="row">
                                <div class="col-md-8">--%>

                            <asp:Label ID="lblCustName" runat="server" class="control-label col-sm-6">Customer Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtcustomer" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Customer Name" ControlToValidate="txtcustomer" ForeColor="Red"></asp:RequiredFieldValidator>



                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblMaterName" runat="server" class="control-label col-sm-6">Product Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtmaterName" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Product Name" ControlToValidate="txtmaterName" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblModelNo" runat="server" class="control-label col-sm-6">Model No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtModelNo" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Model No" ControlToValidate="txtModelNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblSerialNo" runat="server" class="control-label col-sm-6">Serial No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtSrNo" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Serial No" ControlToValidate="txtSrNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                             <asp:Label ID="lblReturnDate" runat="server" class="control-label col-sm-6">Return Date 1 :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control " ID="txtReturnDate" AutoPostBack="True" TextMode="Date" />&nbsp;&nbsp;                          
                            <br />                   
                            <br />
                        </div>

                          <div class="col-md-6">
                            <asp:Label ID="Label3" runat="server" class="control-label col-sm-6"> Return Date 2 :<span class="spncls"></span></asp:Label> <%--Repeated date change--%>
                            <asp:TextBox runat="server" Type="Date" class="form-control" ID="txtrepeateddate" />
                            <br />
                        </div>


                        <div class="col-md-6">
                            <asp:Label ID="lblreturnCustStatus" runat="server" class="control-label col-sm-6 ">Dispatch Date :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" Type="Date" class="form-control" ID="txtdispatchdate" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Select Dispatch Date " ControlToValidate="txtdispatchdate" ForeColor="Red"></asp:RequiredFieldValidator>

                            <%-- <asp:TextBox runat="server" class="form-control" ID="txtreturnCustStatus" />--%>
                            <%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please fill Customer Status" ControlToValidate="txtreturnCustStatus" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                        <div class="col-md-6">
                                <div class="row">
                                <asp:Label ID="lblrepaidstatus" runat="server" class="control-label col-sm-6 custlbl">Return Repair/Unrepair Status :<span class="spncls"></span></asp:Label>
                                <asp:DropDownList ID="DropDownrepaid" runat="server" CssClass="form-control Cust " AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="Select"> </asp:ListItem>
                                </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                              <asp:LinkButton ID="lnkproduct" runat="server" CssClass="lnk " CausesValidation="false" OnClick="lnkproduct_Click">+ADD</asp:LinkButton>
                                                                 <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please fill Status" ControlToValidate="DropDownrepaid" ForeColor="Red"></asp:RequiredFieldValidator>--%>

                            </div>
                            <br />
                        </div>
                        <div class="col-md-6">
                        <asp:Label ID="Label2" runat="server" class="control-label col-sm-6" for="cust">Repeated Job No :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtrepeatedno" />
                            <br />
                        </div>
                      
                        <div class="col-md-6">
                            <asp:Label ID="lblcourier" runat="server" class="control-label col-sm-6" for="cust">Courier Details :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtcourier" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">(Docket Details) :</label><br />
                            <asp:FileUpload ID="FileUpload" CssClass="form-control" runat="server" />
                            <asp:Label ID="lblPath" runat="server" Text=""></asp:Label>
                        </div>
                         
                        <br />
                          <div class="col-md-6">
                            <asp:Label ID="ddlagainstby" runat="server" class="control-label col-sm-4">Against By :<span class="spncls"></span></asp:Label>
                            <asp:DropDownList runat="server" class="form-control" ID="ddlservicetype">
                                <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="Challan" Text="Challan"></asp:ListItem>
                                <asp:ListItem Value="Bill" Text="Bill"></asp:ListItem>
                                <asp:ListItem Value="Cash" Text="Cash"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-6">
                           
                        </div>


                    </div>
                    <br />
                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btnSubmit_Click"  ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " Text="Cancel"  CausesValidation="False"  OnClick="btnCancel_Click"></asp:Button>
                 <asp:HiddenField runat="server" ID="hidden" /> 
            </div></center>
                </div>
            </div>

        </div>

        <asp:Button ID="btnprof" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="modelprofile" runat="server" TargetControlID="btnprof"
            PopupControlID="PopupAddDetail" OkControlID="Button2" />
        <asp:Panel ID="PopupAddDetail" runat="server" class="w3-panel w3-white panel1">

            <h4 style="background-color: #0755A1; color: white; font-weight: 300">Repairing Status</h4>
            <div class="col-md-10">
                <asp:Label ID="Label1" runat="server" class="control-label col-sm-6">Repairing Status :<span class="spncls">*</span></asp:Label>
                <asp:TextBox runat="server" class="form-control" ID="txtstatus" />

                <br />
            </div>
            <center>   <div class="col-md-6">  
              <asp:Button  ID="btnsave" runat="server" class="btn btn-primary  " Text="Save" OnClick="btnsave_Click"  CausesValidation="False"></asp:Button>
                 
             <asp:Button  ID="Button2" runat="server" class="btn btn-primary  " Text="Close"  CausesValidation="False"  ></asp:Button>
                 <asp:HiddenField runat="server" ID="HiddenField1" /> 
            </div></center>


        </asp:Panel>

    </form>
</asp:Content>

