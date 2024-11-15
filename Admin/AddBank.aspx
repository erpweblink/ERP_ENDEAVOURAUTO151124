<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AddBank.aspx.cs" Inherits="Admin_AddBnak" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="card shadow-sm mb-4">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Bank Master</h5>
            </div>
            <div class="card-body">
                 <div runat="server" id="divform">
                <div class="row">
                    <div class="col-md-6">
                        <asp:Label ID="lblbankname" runat="server" class="control-label col-sm-6" for="cust">Bank Name :<span class="spncls">*</span></asp:Label>
                        <asp:TextBox runat="server" class="form-control" ID="txtbankname" name="Customer" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please fill Bank Name" ControlToValidate="txtbankname" ForeColor="Red"></asp:RequiredFieldValidator>
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblaccountno" runat="server" class="control-label col-sm-6" for="cust">Account No. :<span class="spncls">*</span></asp:Label>
                        <asp:TextBox runat="server" class="form-control" ID="txtaccountno" onkeypress="return isNumberKey(event)" name="Customer" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Account No." ControlToValidate="txtaccountno" ForeColor="Red"></asp:RequiredFieldValidator>
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblifsccode" runat="server" class="control-label col-sm-6" for="cust">IFSC Code :<span class="spncls">*</span></asp:Label>
                        <asp:TextBox runat="server" class="form-control" ID="txtifsccode" name="Customer" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill IFSC Number" ControlToValidate="txtifsccode" ForeColor="Red"></asp:RequiredFieldValidator>
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblbranchname" runat="server" class="control-label col-sm-6" for="cust">Branch Name:<span class="spncls">*</span></asp:Label>
                        <asp:TextBox runat="server" class="form-control" ID="txtbranchname" name="Customer" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Branch Name" ControlToValidate="txtbranchname" ForeColor="Red"></asp:RequiredFieldValidator>
                        <br />
                    </div>
                     <div class="col-md-6">
                        <asp:Label ID="lblremark" runat="server" class="control-label col-sm-6" for="cust">Remark :<span class="spncls"></span></asp:Label>
                        <asp:TextBox runat="server" class="form-control" ID="txtremark" name="Customer" TextMode="MultiLine" />
                        <br />
                    </div>
                  
                </div>
                <center>  
                        <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary" OnClick="btnSubmit_Click" Text="Submit" ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary btncancel " OnClick="btnCancel_Click" Text="Cancel "  CausesValidation="False"  ></asp:Button>
            </div></center>
                     </div>
                <%--<div id="divText" runat="server" class="div-to-hide">
                    <br />
                    <hr style="background-color: #000066;" />
                    <div class="col-lg-12 header">
                        <center>
                        <h5>Product List</h5></center>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:TextBox runat="server" class="form-control txtsear" ID="txtSearch" name="Search" placeholder="Search Product Name..." />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetProdList" TargetControlID="txtSearch" runat="server">
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
                    </div>
                    <div style="width: 100%; padding: 20px; overflow: scroll;">
                        <asp:GridView ID="gv_Prod" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Prod_RowCommand" OnRowDataBound="gv_Prod_RowDataBound" PagerStyle-CssClass="paging" PageSize="5" OnPageIndexChanging="gv_Prod_PageIndexChanging" AllowPaging="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProdName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Technical Specification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTechSeci" runat="server" Text='<%# Eval("TechSpeci") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModel" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Serial No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblserialNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Active">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsStatus" runat="server" Text='<%# Eval("IsStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("Prodid") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Prodid") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
            </div>
            <asp:HiddenField runat="server" ID="hidden" />
        </div>
    </form>
</asp:Content>

