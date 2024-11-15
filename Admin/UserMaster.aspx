<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="UserMaster.aspx.cs" Inherits="Admin_UserMaster" %>

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
                window.location.href = "../Admin/UserMaster.aspx";
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
    <script type="text/javascript">
        function ShowHidePassword() {
            var txt = $('#<%=Password.ClientID%>');
            if (txt.prop("type") == "password") {
                txt.prop("type", "text");
                $("label[for='cbShowHidePassword']").text("Hide Password");
            }
            else {
                txt.prop("type", "password");
                $("label[for='cbShowHidePassword']").text("Show Password");
            }

        }
    </script>
    <!--password-->
    <script type="text/javascript">
        function ShowHidePasswordD() {
            var txt = $('#<%=txtconpassword.ClientID%>');
            if (txt.prop("type") == "password") {
                txt.prop("type", "text");
                $("label[for='cbShowHidePassword2']").text("Hide Password");
            }
            else {
                txt.prop("type", "password");
                $("label[for='cbShowHidePassword2']").text("Show Password");
            }

        }
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
                    <h5 class="m-0 font-weight-bold text-primary">User Master</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblname" runat="server" class="control-label col-sm-6" for="cust">User Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtName" onkeypress="return character(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill  Name" ControlToValidate="txtName" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblemail" runat="server" class="control-label col-sm-6" for="cust">Email :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtemail" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Email" ControlToValidate="txtemail" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblpassword" runat="server" class="control-label col-sm-6" for="cust">Password :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control " ID="Password" type="password"></asp:TextBox>
                            <input id="cbShowHidePassword" type="checkbox" onclick="ShowHidePassword();" />
                            Show Password
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill  Password" ControlToValidate="Password" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblconpassword" runat="server" class="control-label col-sm-6" for="cust">Confirm Password :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtconpassword" type="password" />
                            <input id="cbShowHidePassword2" type="checkbox" onclick="ShowHidePasswordD();" />
                            Show Password
                            <asp:CompareValidator ID="CompareValidator1" runat="server"
                                ControlToCompare="Password" ControlToValidate="txtconpassword"
                                ErrorMessage="Password does not match" ForeColor="Red"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Confirm Password" ControlToValidate="txtconpassword" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblmobileNo" runat="server" class="control-label col-sm-6" for="cust">Mobile Number :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtmobileNo" MinLength="10" MaxLength="11"
                                onkeypress="return isNumberKey(event)" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please fill  Mobile Number" ControlToValidate="txtmobileNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblrole" runat="server" class="control-label col-sm-6" for="cust">Department :<span class="spncls">*</span></asp:Label>

                            <asp:DropDownList ID="ddlRole" runat="server" class="form-control" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Value="" Text="Select Role">
                                </asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                    </div>
                    <div class="row">
          
                        <div class="col-md-6">
                            <asp:Label ID="lblisstatus" class="control-label col-sm-6" runat="server">Is Active :</asp:Label>
                            <asp:DropDownList ID="DropDownListisActive" runat="server" class="form-control" AutoPostBack="true">

                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList><br />
                        </div>


<%--                         <div class="col-md-6">
                            <asp:Label ID="lblcompanyname" runat="server" class="control-label col-sm-6" for="cust">Role :<span class="spncls">*</span></asp:Label>

                            <asp:DropDownList ID="ddlcompany" runat="server" class="form-control" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Value="Select Company" Text="----Select Company----"></asp:ListItem>
                            <asp:ListItem Value="ENDEAVOUR AUTOMATION" Text="ENDEAVOUR AUTOMATION"></asp:ListItem>
                            <asp:ListItem Value="SCHNEIDER ELECTRIC INDIA PVT.LTD." Text="SCHNEIDER ELECTRIC INDIA PVT.LTD."></asp:ListItem>      
                            </asp:DropDownList>
                            <br />
                        </div>--%>

                    </div>
                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary " Text="Submit"   OnClick="btnSubmit_Click" ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary btncancel " Text="Cancel"  CausesValidation="False" OnClick="btnCancel_Click" ></asp:Button>

            </div></center>
                  <%--  <br />
                    <hr style="background-color: #000066;" />
                    <div class="col-lg-12 header">
                        <center>
                        <h5>User List</h5></center>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:TextBox runat="server" class="form-control txtsear" ID="txtSearch" name="Search" placeholder="Search User Name...." onkeypress="return character(event)" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetUserList" TargetControlID="txtSearch" runat="server">
                            </asp:AutoCompleteExtender>

                        </div>
                        <div class="col-md-4">
                            <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch " CausesValidation="False" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary lnksearch" OnClick="lnkrefresh_Click" CausesValidation="false"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                        </div>
                        <div class="col-md-4">


                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" class="form-control active1 " Width="150px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Value="All" Text="All"></asp:ListItem>
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">DeActive</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>--%>
                  <%--  <div style="width: 100%; padding: 20px; overflow: scroll;">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_user" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                OnRowCommand="gv_user_RowCommand" OnRowDataBound="gv_user_RowDataBound" OnPageIndexChanging="gv_user_PageIndexChanging" AllowPaging="true" PageSize="5" PagerStyle-CssClass="paging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">

                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email">

                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Password">

                                        <ItemTemplate>
                                            <asp:Label ID="lblpass" runat="server" Text='<%# Eval("pass") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mobile No.">

                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileno" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role">

                                        <ItemTemplate>
                                            <asp:Label ID="lblrole" runat="server" Text='<%# Eval("role") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("IsActive") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("Id") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>


                                            &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                    </div>--%>



                    <asp:HiddenField runat="server" ID="hidden" />

                </div>
            </div>
        </div>
    </form>


</asp:Content>

