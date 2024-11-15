<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProfilePage.aspx.cs" Inherits="Admin_ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form runat="server">
     <div class="card-body">

      <center>   <asp:Image runat="server" Height="200px" Width="200px" />
          
      </center>


                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblname" runat="server" class="control-label col-sm-6" for="cust">Name</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtName"  onkeypress="return character(event)"/>
                            
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblemail" runat="server" class="control-label col-sm-6" for="cust">Email:</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtemail" TextMode="Email" />
                            
                            <br />
                        </div>
         </div> 
         <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblpassword" runat="server" class="control-label col-sm-6">Password</asp:Label>
                            
                            <asp:TextBox runat="server" class="form-control " ID="Password" type="password"></asp:TextBox>
                             <input id="cbShowHidePassword" type="checkbox"  onclick="ShowHidePassword();"  />
                           Show Password
                           <br /><br />
                            </div>
                      <div class="col-md-6">
                            <asp:Label ID="lblisstatus" class="control-label col-sm-6" runat="server">Is Active:</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtstatus"  onkeypress="return character(event)" ReadOnly="true"/>      <br />
                        </div>
                        
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblmobileNo" runat="server" class="control-label col-sm-6" for="cust">Mobile Number</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtmobileNo" MinLength="10" MaxLength="11" onkeypress="return isNumberKey(event)"/>
                           
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblrole" runat="server" class="control-label col-sm-6" for="cust">Role</asp:Label>
                             <asp:TextBox runat="server" class="form-control" ID="txtRole"  onkeypress="return character(event)" ReadOnly="true"/>
                            
                            <%-- <asp:TextBox runat="server"   class="form-control" id="txtrole" />--%>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please fill Role" ControlToValidate="ddlRole" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                    </div>
                    
                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit"    ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " Text="Cancel"  CausesValidation="False"  ></asp:Button>

            </div></center>
            </div>
    </form>
</asp:Content>

