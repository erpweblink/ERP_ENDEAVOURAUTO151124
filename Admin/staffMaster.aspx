<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="staffMaster.aspx.cs" Inherits="Admin_staffMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style>
        .spncls {
            color: red;
        }

        .btncreate {
            float: right;
            margin-right: 18px;
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

        .header {
            background-color: #0755a1;
            color: white;
            font-weight: bolder;
        }

        .active1 {
            float: right;
            margin-right: 42px;
        }
    </style>
    <style type="text/css">
        .paging {
        }

            .paging a {
                background-color: #0755A1;
                padding: 1px 7px;
                text-decoration: none;
                border: 1px solid #0755A1;
            }

                .paging a:hover {
                    background-color: #E1FFEF;
                    color: white;
                    border: 1px solid #47417c;
                }

            .paging span {
                background-color: #0755A1;
                padding: 1px 7px;
                color: white;
                border: 1px solid #0755A1;
            }

        tr.paging {
            background: none !important;
        }

            tr.paging tr {
                background: none !important;
            }

            tr.paging td {
                border: none;
            }
    </style>
    <script>
        function HideLabe(msg) {

            swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/StaffList.aspx";
            })
        };
    </script>

    <!--char-->
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">

        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>


        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Staff Master</h5>
                </div>
                <div class="card-body">
                    <%--   <asp:UpdatePanel runat="server" ID="updatepanel">

            <ContentTemplate>--%>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblname" runat="server" class="control-label col-sm-6" for="cust">Employee Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control txtcustomer" ID="txtName" onkeypress="return character(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill  Employee Name" ControlToValidate="txtName" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblemail" runat="server" class="control-label col-sm-6" for="cust">Email ID :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtemail" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Email" ControlToValidate="txtemail" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblAddress" runat="server" class="control-label col-sm-6">Current Address :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtAddress" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill  Adress" ControlToValidate="txtAddress" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblpermantaddres" runat="server" class="control-label col-sm-6">Permanent Address :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtpermantaddres" TextMode="MultiLine" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please fill  Adress" ControlToValidate="txtAddress" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-md-6">
                            <asp:Label ID="lblMobno" runat="server" class="control-label col-sm-6" for="cust">Mobile / Tel No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtmob" MaxLength="11" onkeypress="return isNumberKey(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Mobile Number" ControlToValidate="txtmob" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblDesignation" runat="server" class="control-label col-sm-6">Designation :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtDesignation" onkeypress="return character(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Please fillDesignation" ControlToValidate="txtDesignation" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblsalary" runat="server" class="control-label col-sm-6" for="cust">Salary :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtsalary" onkeypress="return isNumberKey(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please fill Salary Amount" ControlToValidate="txtsalary" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblsalaryAC" runat="server" class="control-label col-sm-6" for="cust">Salary AC :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtsalaryAC" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please fill Mobile Number" ControlToValidate="txtsalaryAC" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                    </div>
                    <div class="row" style="margin-left: 3px">
                        <div class="col-md-3">
                            <asp:Label ID="lbltimeIn" runat="server" class="control-label col-md-6">Time In :<span class="spncls">*</span></asp:Label><div class="row">
                                <asp:TextBox runat="server" class="form-control" ID="txtTimeIn" TextMode="Time" Width="130px" />
                                <asp:DropDownList runat="server" class="form-control" ID="ddltimein" Width="110px">
                                    <asp:ListItem>AM</asp:ListItem>
                                    <asp:ListItem>PM</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">

                            <asp:Label ID="lblTimeOut" runat="server" class="control-label">Time Out :<span class="spncls">*</span></asp:Label>
                            <div class="row">
                                <asp:TextBox runat="server" class="form-control" ID="txtTimeout" TextMode="Time" Width="130px" />
                                <asp:DropDownList runat="server" class="form-control" ID="ddltimeout" Width="110px">
                                    <asp:ListItem>AM</asp:ListItem>
                                    <asp:ListItem>PM</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%-- <div class="col-md-6">
                                    <asp:Label ID="lbltimeIn" runat="server" class="control-label col-md-6">Time In<span class="spncls">*</span></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblTimeOut" runat="server" class="control-label">Time Out<span class="spncls">*</span></asp:Label> 
                                    <div class="row" style="margin-left:3px" >
                                        
                                     

                                       
                                            <asp:TextBox runat="server" class="form-control" ID="txtTimeIn" type="time" Width="130px" />&nbsp;
                           <asp:DropDownList runat="server" class="form-control" ID="ddltimein" Width="110px">
                               <asp:ListItem>AM</asp:ListItem>
                               <asp:ListItem>PM</asp:ListItem>
                           </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                        
                                       
                                        <br />

                                        
                                        
                                            <asp:TextBox runat="server" class="form-control" ID="txtTimeout" type="time" Width="130px" />&nbsp;
                           <asp:DropDownList runat="server" class="form-control" ID="ddltimeout" Width="110px">
                               <asp:ListItem>AM</asp:ListItem>
                               <asp:ListItem>PM</asp:ListItem>
                           </asp:DropDownList>
                                       
                                    </div>

                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please fill Time In" ControlToValidate="txtTimeIn" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please fill Time Out" ControlToValidate="txttimeout" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <br />
                                </div>--%>
                        <div class="col-md-6">
                            <asp:Label ID="lblIsActive" runat="server" class="control-label col-sm-6">Is Active :<span class="spncls"></span></asp:Label>

                            <asp:DropDownList ID="ddlisActive" runat="server" class="form-control" AutoPostBack="true">

                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList><br />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lbldateJoining" runat="server" class="control-label col-sm-6">Date of Joining :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtdateJoining" TextMode="Date" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Please fill Date OF Joining" ControlToValidate="txtdateJoining" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblLeavingdate" runat="server" class="control-label col-sm-6">Leaving Date :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtLeavingdate" TextMode="Date" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please fill Date OF Joining" ControlToValidate="txtdateJoining" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblCommisionAC" runat="server" class="control-label col-sm-6">Blood Group :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtCommisionAC" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please fill Blood Group" ControlToValidate="txtCommisionAC" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblRefBy" runat="server" class="control-label col-sm-6">Ref By :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtRefBy" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please fill Commission AC" ControlToValidate="txtTimeIn" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-md-6">
                            <asp:Label ID="lblExtraInfo" runat="server" class="control-label col-sm-6">Extra Information :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtExtraInfo" onkeypress="return character(event)" />
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Please fillDesignation" ControlToValidate="txtDesignation" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>
                    </div>


                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Please fill Designation" ControlToValidate="txtIsActive" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    <br />

                    <%--</div>--%>
                  
                    <center>   
                        <div class="row">
                            <div class="col-4">

                            </div>
                        <div class="col-md-1 col-xs-6 col-6"> 
                       
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary" Text="Submit"  OnClick="btnSubmit_Click" ></asp:Button>
            </div>

                        <div class="col-md-1 col-xs-6 col-6"> 
             <asp:Button  ID="Button2" runat="server" class="btn btn-primary btncancel" Text="Cancel"  CausesValidation="False"   OnClick="btnCancel_Click"></asp:Button>

            </div>
</div>
                    </center>
                    </ContentTemplate>
        </asp:UpdatePanel>
                </div>
                <asp:HiddenField runat="server" ID="hidden" />
            </div>

        </div>
    </form>
</asp:Content>

