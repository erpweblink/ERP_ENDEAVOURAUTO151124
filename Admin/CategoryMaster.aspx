<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="CategoryMaster.aspx.cs" Inherits="Admin_CategoryMaster" %>

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
                window.location.href = "../Admin/CategoryList.aspx";
            })
        };
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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>

        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Category Master</h5>
                </div>
                <div class="card-body">
                    <div runat="server" id="divform">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lblCategoryname" runat="server" class="control-label col-sm-6" for="cust">Category Name :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtCategoryName" name="Customer" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please Fill Category Name." ControlToValidate="txtCategoryName" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblisstatus" class="control-label col-sm-6" runat="server">Is Active :</asp:Label>
                                <asp:DropDownList ID="DropDownListisActive" runat="server" class="form-control" AutoPostBack="true">
                                    <asp:ListItem>Yes</asp:ListItem>
                                    <asp:ListItem>No</asp:ListItem>
                                </asp:DropDownList><br />
                            </div>

                        </div>
                       
                        <center>
                            <div class="col-md-6">
                                <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
                                <asp:Button ID="btnCancel" runat="server" class="btn btn-primary btncancel " Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click"></asp:Button>
                            </div>
                        </center>
                        <br />
                    </div>

                    <asp:HiddenField runat="server" ID="hidden" />

                </div>
            </div>
        </div>
            
    </form>


</asp:Content>

