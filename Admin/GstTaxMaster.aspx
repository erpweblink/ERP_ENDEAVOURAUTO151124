<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="GstTaxMaster.aspx.cs" Inherits="Admin_GstTaxMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
                window.location.href = "../Admin/GSTTaxList.aspx";
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
   .paging
        {
        }
         
        .paging a
        {
            background-color: #0755A1;
            padding: 1px 7px;
            text-decoration: none;
            border: 1px solid #0755A1;
        }
         
        .paging a:hover
        {
            background-color: #E1FFEF;
            color: white;
            border: 1px solid #47417c;
        }
         
        .paging span
        {
            background-color: #0755A1;
            padding: 1px 7px;
            color: white;
            border: 1px solid #0755A1;
        }
         
        tr.paging
        {
            background: none !important;
        }
         
        tr.paging tr
        {
            background: none !important;
        }
        tr.paging td
        {
            border: none;
        }
</style>
    <style>
        .spncls{
            color:red;
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
        .header
        {
            background-color:#0755a1;
            color:white;
            font-weight:bolder;
        }
        .active1{
            float:right;
            margin-right:42px;
       
    </style>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("customerpo").offsetTop;
            window.scrollTo(0, target);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form runat="server">
     <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>

     <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
               <h5 class="m-0 font-weight-bold text-primary">GST Tax</h5>
            </div>
            <div class="card-body">

                 <div class="row">
                    <div class="col-md-6">
                        <asp:label  id="lbltaxname" runat="server" class="control-label col-sm-6" for="cust">Tax Name :<span class="spncls">*</span></asp:label>
                        <asp:TextBox runat="server"   class="form-control" id="txttaxname"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please Fill Tax Name."  ControlToValidate="txttaxname" ForeColor="Red"></asp:RequiredFieldValidator>            
                        <br />
                     </div>
                     
                      <div class="col-md-6">
                        <asp:label  id="lbltax" runat="server" class="control-label col-sm-6" for="cust">Tax(%) :<span class="spncls">*</span></asp:label>
                        <asp:TextBox runat="server"   class="form-control" id="txttax" onkeypress="return isNumberKey(event)"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Fill Tax."  ControlToValidate="txttax" ForeColor="Red"></asp:RequiredFieldValidator>            
                        <br />
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <asp:label  id="lblgroupunder" runat="server" class="control-label col-sm-6" for="cust">Group Under :<span class="spncls">*</span></asp:label>
                        <asp:TextBox runat="server"   class="form-control" id="txtgroupunder"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Fill Group Under."  ControlToValidate="txtgroupunder" ForeColor="Red"></asp:RequiredFieldValidator>            
                        <br />
                     </div>
                    <div class="col-md-6">
                        <asp:label  id="lblprintdescription" runat="server" class="control-label col-sm-6" for="cust">Print Description:<span class="spncls"></span></asp:label>
                        <asp:TextBox runat="server"   class="form-control" id="txtprintdiscription"/>
<%--                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Fill Tax Name."  ControlToValidate="txtCompName" ForeColor="Red"></asp:RequiredFieldValidator>            --%>
                        <br />
                     </div>
                    </div>
                 <div class="row">
                    <div class="col-md-6">
                        <asp:label  id="lbltaxtype" runat="server" class="control-label col-sm-6">Tax Type :<span class="spncls"></span></asp:label>
                        <asp:DropDownList runat="server" ID="ddltaxtype" CssClass="form-control">
                            <asp:ListItem Text="select"></asp:ListItem>
                            <asp:ListItem Text="test"></asp:ListItem>
                            <asp:ListItem Text="test1"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                     </div>
                     <div class="col-md-6">
                        <asp:label  id="lblonvalue" runat="server" class="control-label col-sm-6">On Value :<span class="spncls"></span></asp:label>
                        <asp:DropDownList runat="server" ID="ddlonvalue" CssClass="form-control">
                            <asp:ListItem Text="select"></asp:ListItem>
                            <asp:ListItem Text="Before GSt Tax"></asp:ListItem> 
                            <asp:ListItem Text="test1"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                     </div>
                     </div>
                  <div class="col-md-6">
                        <asp:Label id="lblisstatus" class="control-label col-sm-6" runat="server">Is Active :</asp:Label>
                         <asp:DropDownList ID="DropDownListisActive" runat="server" class="form-control" AutoPostBack="true" >
                             <asp:ListItem>Yes</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:DropDownList><br />
                    </div>

                 <center>   
                     <div class="row">
                         <div class="col-4">

                         </div>
                         <div class="col-md-1 col-xs-6 col-6">
                             <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                         </div>
                         <div class="col-md-1 col-xs-6 col-6">
                             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary btncancel " Text="Cancel"  CausesValidation="False"  OnClick="btnCancel_Click"></asp:Button>
                         </div>
                     </div>
                     </center>
                <br />
             <%--   <hr style="background-color:#000066;" />
                    <div class="col-lg-12 header" >
                        <center>
                        <h5>GST Tax List</h5></center>
                    </div>
                <div class="row">
                 <div class="col-md-4">
                        <asp:TextBox runat="server" class="form-control txtsear" ID="txtname" name="Search" placeholder="Tax Name" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCompList" TargetControlID="txtname" runat="server">
                        </asp:AutoCompleteExtender>

                    </div>
                     <div class="col-md-4">
                      <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch mt-top" OnClick="lnkBtnsearch_Click" CausesValidation="False"  ><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                      <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary lnksearch mt-top" OnClick="lnkrefresh_Click"  CausesValidation="false"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-4">
                       
                   
                         <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" class="form-control active1 "  Width="150px" OnTextChanged="ddlStatus_TextChanged">
                              <asp:ListItem Value="All" Text="All"></asp:ListItem>
                              <asp:ListItem Value="1">Active</asp:ListItem>
                              <asp:ListItem Value="0">DeActive</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>--%>
               <%--  <div style="width: 100%; overflow:scroll;" class="table-responsive">
                    <asp:GridView ID="gv_gsttax" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                           PageSize="5" AllowPaging="true"  PagerStyle-CssClass="paging" OnRowDataBound="gv_gsttax_RowDataBound" OnRowCommand="gv_gsttax_RowCommand" OnPageIndexChanging="gv_gsttax_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No.">

                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Name">

                                <ItemTemplate>
                                    <asp:Label ID="lblTaxname" runat="server" Text='<%# Eval("TaxName") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Tax">

                                <ItemTemplate>
                                    <asp:Label ID="lbltax" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Group Under">

                                <ItemTemplate>
                                    <asp:Label ID="lblgroupunder" runat="server" Text='<%# Eval("GroupUnder") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Tax Type">

                                <ItemTemplate>
                                    <asp:Label ID="lbltaxtype" runat="server" Text='<%# Eval("TaxType") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="On value">

                                <ItemTemplate>
                                    <asp:Label ID="lblonvalue" runat="server" Text='<%# Eval("Onvalue") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Status">

                                <ItemTemplate>
                                    <asp:Label ID="lblIsStatus" runat="server" Text='<%# Eval("isactive") %>'></asp:Label>
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
                </div>--%>
                 <asp:HiddenField runat="server" ID="hidden" /> 
                </div>
                </div>
         </div>
        </form>
</asp:Content>

