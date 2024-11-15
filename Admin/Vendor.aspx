<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Vendor.aspx.cs" Inherits="Reception_Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href ="../Admin/VendorList.aspx";
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
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("divdtls").offsetTop;
            window.scrollTo(0, target);
        }
    </script>
  <style>
        .spncls{
            color:red;
        }
    </style>
           <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <form runat="server">
    <div class="col-lg-12">
        <div class="card shadow-sm mb-4">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
              <center> <h5 id="vendorlbl" runat="server" class="m-0 font-weight-bold text-primary"> Vendor Master </h5></center> 
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <label class="control-label " for="cust">Vendor name :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server"   class="form-control" id="txtvendorName" name="Vendor" onkeypress="return character(event)"/>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please fill Vendor Name"  ControlToValidate="txtvendorName" ForeColor="Red"></asp:RequiredFieldValidator>            
<br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="gst">GST No. :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtgstno" name="GSTNo" MaxLength="15"/>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill GSTNo"  ControlToValidate="txtgstno" ForeColor="Red"></asp:RequiredFieldValidator>            
                    </div>
                    <div class="col-md-6">
                      

                        <label class="control-label " for="state">State Code :</label>
<%--                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtstatecode" name="StateCode"/><br />--%>
                            <asp:DropDownList ID="DropDownListvendor" runat="server" class="form-control" >
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
                        </asp:DropDownList><br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="pan">PAN No. :</label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtpanno" name="PanNo" MaxLength="10"/><br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="Add1">Address Line 1 :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtAddresline1" name="Address1"/>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidatoradd" runat="server" ErrorMessage="Please fill Address"  ControlToValidate="txtAddresline1" ForeColor="Red"></asp:RequiredFieldValidator>            

                        <br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label" for="Add2">Address Line 2 :</label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtadreLine2" name="Address2"/><br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="Add3">Address Line 3 :</label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtadreLine3" name="Address3"/><br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="area">Area :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtarea" name="Area"/>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidatorarea" runat="server" ErrorMessage="Please fill Area"  ControlToValidate="txtarea" ForeColor="Red"></asp:RequiredFieldValidator>            

                        <br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="city">City :</label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtcity" name="City" onkeypress="return character(event)"/>
                        
                        <br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label " for="country">Country :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtcountry" name="country" onkeypress="return character(event)"/>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Country"  ControlToValidate="txtcountry" ForeColor="Red"></asp:RequiredFieldValidator>            
                        <br />
                    </div>
                     <div class="col-md-6">
                        <label class="control-label " for="PostalCode">Postal Code :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtPostalCode" name="PostalCode" onkeypress="return isNumberKey(event)" MaxLength="6" />
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Postal Code"  ControlToValidate="txtPostalCode" ForeColor="Red"></asp:RequiredFieldValidator>            

                         <br />
                    </div>
                    <div class="col-md-6">
                        <label class="control-label col-sm-6" for="MobileNo">Mobile No :<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtMobileNo" name="MobileNo" MaxLength="11"  onkeypress="return isNumberKey(event)"/>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Mobile Number"  ControlToValidate="txtMobileNo" ForeColor="Red"></asp:RequiredFieldValidator>            

                        <br />
                    </div>
                    <%-- <div class="col-md-6">
                        <label class="control-label col-sm-6" for="Email">Email:<span class="spncls">*</span></label>
                        <asp:TextBox runat="server" type="Email" class="form-control" id="txtEmail" name="Email"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Email"  ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>            


                         <br />
                    </div>--%>
                     <div class="col-md-6">
                        <label class="control-label " for="IsActive">Is Active :</label>
                         <asp:DropDownList ID="DropDownListisActive" runat="server" class="form-control">
                             <asp:ListItem>Yes</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:DropDownList><br />
<%--                        <asp:TextBox runat="server" type="Text" class="form-control" id="txtIsActive" name="IsActive"/><br />--%>
                    </div>
                    
                     
                    </div>
                    <hr />
                            <h5 class="m-0 font-weight-bold">Contact Detils</h5>
                           <br />
                            <div class="row">
                      <div class="row" id="divdtls">
                      <div class="col-md-3">
                        <asp:Label id="lblcontactperson" class="control-label" runat="server">Contact Person :<span class="spncls"></span></asp:Label>
                         <asp:Textbox  runat="server" class="form-control " id="txtcustomername" /> 
              <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Email"  ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>            --%>
                </div>
                      <div class="col-md-3">
                        <asp:Label id="lblpersonno" class="control-label " runat="server">Contact Person No. :<span class="spncls"></span></asp:Label>
                            <asp:Textbox  runat="server" class="form-control " MaxLength="11" id="txtcustomernameno" onkeypress="return isNumberKey(event)" />  
              <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Email"  ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>            --%>
                </div>
                                <div class="col-md-3">
                        <asp:Label id="lbltemail" class="control-label " runat="server" >Email :<span class="spncls"></span></asp:Label>
                        <asp:TextBox runat="server"  class="form-control" id="txttemail"  TextMode="Email"/>
              <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Email"  ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>            --%>
                </div>
                                 <div class="col-md-2">
                        <asp:Label id="lbldesignation" class="control-label " runat="server">Designation :<span class="spncls"></span></asp:Label>
                        <asp:TextBox runat="server"  class="form-control" id="txtdesignation"  />
              <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Email"  ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>            --%>
                </div>
            <div class="col-md-1 text-center">
                <br />
<asp:Button ID="btnadd" runat="server" CssClass="btn btn-primary " Text="Add"  CausesValidation="false" OnClick="btnadd_Click" />                        </div>  
                <%-- <table class="table-responsive col-md-12" >  
                  
                    <tr class="col-md-12">  
                        <td class="col-md-5">Contact Person</td>  
                        <td class="col-md-5">Contact Person No</td>  
                       
                    </tr>  
                 
                 
                    <tr  class="col-md-12">  
                         <td class="col-md-5">  
                            <asp:Textbox  runat="server" class="form-control " id="txtcustomername" /></td>  
                        <td class="col-md-5">  
                            <asp:Textbox  runat="server" class="form-control " MaxLength="11" id="txtcustomernameno" onkeypress="return isNumberKey(event)" /></td>  
                        <td>  
                           
                          <td class="col-md-2">
                              <asp:Button ID="btnadd" runat="server" CssClass="btn btn-primary " Text="Add"  CausesValidation="false" OnClick="btnadd_Click" />
                        </td>  
                    </tr>  
              
            </table>--%>  
                         </div>
                         </div>
                </br>
                         <div style="width: 100%;">  <div class="table-responsive">
                    <asp:GridView ID="gv_vendorcontact" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                         PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No.">

                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Contact Person">

                                <ItemTemplate>
                                    <asp:Label ID="lblCustomername" Text='<%# Eval("colFirst") %>' runat="server" ></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Contact Person No.">

                                <ItemTemplate>
                                    <asp:Label ID="lblCustomernameno" runat="server"  Text='<%# Eval("colsecond") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Email">

                                <ItemTemplate>
                                    <asp:Label ID="lblemailperson" runat="server"  Text='<%# Eval("colemail") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Designation">

                                <ItemTemplate>
                                    <asp:Label ID="lbldesignation" runat="server"  Text='<%# Eval("coldesignation") %>'></asp:Label>
                                </ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">

                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CausesValidation="false" OnClick="lnkbtnDelete_Click"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                        </asp:GridView></div>
                  </div>
                </div><br /><center>
                     <div class="col-md-6">
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary " Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False"></asp:Button>
                 <asp:HiddenField runat="server" ID="hidden" /> 

                         </div></center>
               
            </div>
        </div>
        
    </div>
        </form>

</asp:Content>

