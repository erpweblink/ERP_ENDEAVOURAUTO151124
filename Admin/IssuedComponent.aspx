<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="IssuedComponent.aspx.cs" Inherits="Admin_IssuedComponent" %>

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
                debugger;
                if (flg == '1') {
                    window.location.href = "../Admin/BankList.aspx";
                }
                else {
                    window.location.href = "../Admin/InwardEntry.aspx";
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

    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

    <script>
        function allowAlphabets(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode <= 90 && charCode >= 65) || (charCode <= 122 && charCode >= 97 || charCode == 8)) {

                return true;
            }
            return false;
        }
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
            margin-right: 41px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="card shadow-sm mb-4">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Issued Component</h5>
            </div>
            <div class="card-body">
                <div runat="server" id="divform">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblcustmoername" runat="server" class="control-label col-sm-6" for="cust">Customer Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtcustomername" name="Customer Name" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please Customer Name" ControlToValidate="txtcustomername" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GeCustomerList" TargetControlID="txtcustomername" runat="server">
                            </asp:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lbljobno" runat="server" class="control-label col-sm-6" for="cust">Job No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtjobno" OnTextChanged="txtjobno_TextChanged" AutoPostBack="true" name="Jobno" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Job No." ControlToValidate="txtjobno" ForeColor="Red"></asp:RequiredFieldValidator>
                              <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GeJobnoList" TargetControlID="txtjobno" runat="server">
                            </asp:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblproductname" runat="server" class="control-label col-sm-6" for="cust">Product Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtproductname" name="product" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Product Name" ControlToValidate="txtproductname" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblengeername" runat="server" class="control-label col-sm-6" for="cust">Engineer Name:<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtengeername" name="Customer" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Engineer Name" ControlToValidate="txtengeername" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                         <div class="col-md-6">
                            <asp:Label ID="lblCompList" runat="server" class="control-label col-sm-6 lblcomp">Component List :</asp:Label>
                            <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtcomponent" name="Search" placeholder="Search Component" onkeypress="return character(event)" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetComponetList" TargetControlID="txtcomponent" runat="server">
                            </asp:AutoCompleteExtender>                       
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblQuantityComp" runat="server" class="control-label col-sm-6 lblcomp">Quantity :</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtQuantityComp" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Quantity" ControlToValidate="txtQuantityComp" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                          <div class="col-md-1">
                            <br />
                            <asp:LinkButton ID="lnkbtnAdd" OnClick="lnkbtnAdd_Click" runat="server" Font-Bold="true" CommandArgument='<%# Eval("JobNo") %>'>+ADD</asp:LinkButton>
                        </div>
                    </div>

                          <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_Comp" runat="server" AutoGenerateColumns="false" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comp Id" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompid" runat="server" Text='<%# Eval("CompId") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component Name">
                                        <ItemTemplate>

                                            <asp:Label ID="lblCompnamee" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>

                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>

                                            <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("CompId") %>' ><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

                        <br />
                    </div>
                     
                    <div class="row">
                        <div class="col-5">
                        </div>
                        <div class="col-md-6">
                            <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary" Text="Submit"></asp:Button>
                            &nbsp;&nbsp; 
                        <asp:Button ID="btnCancel" runat="server" class="btn btn-primary btncancel" Text="Cancel " CausesValidation="False"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hidden" />
        </div>
    </form>
</asp:Content>

