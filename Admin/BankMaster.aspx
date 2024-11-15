<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="BankMaster.aspx.cs" Inherits="Admin_BankMaster" %>

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
                    <h5 class="m-0 font-weight-bold text-primary">Bank Master</h5>
                </div>
                <div class="card-body">
                    <div class="row m-0">
                        <div class="col-md-6">
                            <asp:Label ID="lblbankname" runat="server" class="control-label col-sm-6" for="cust">Bank Name:<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control txtcustomer" ID="txtbankname" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Bank Name" ControlToValidate="txtbankname" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblaccountno" class="control-label col-sm-6" runat="server">Account No. :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtaccountno" name="txtaccountno" MaxLength="15" /><br />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblifsccode" class="control-label col-sm-6" runat="server">IFSC Code :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtifsccode" name="txtifsccode" MaxLength="10" /><br />
                        </div>
                    </div>
                    <div class="row m-0">

                        <div class="col-md-6">
                            <asp:Label ID="lblbranchname" class="control-label col-sm-6" runat="server">Branch Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtbranchname" name="Address1" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatoradd" runat="server" ErrorMessage="Branch Name" ControlToValidate="txtbranchname" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row m-2">
                        <div class="col-md-6">
                            <asp:Label ID="lblremark" class="control-label col-sm-6" runat="server">Remark :</asp:Label>
                            <asp:TextBox runat="server" type="Text" class="form-control" ID="txtremark" name="txtremark" />
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

