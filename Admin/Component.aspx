<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Component.aspx.cs" Inherits="Reception_Component" %>

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
                window.location.href = "../Admin/ComponentList.aspx";
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
                    <h5 class="m-0 font-weight-bold text-primary">Component Master</h5>
                </div>
                <div class="card-body">
                    <div runat="server" id="divform">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lblCompname" runat="server" class="control-label col-sm-6" for="cust">Component Name :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtCompName" name="Customer" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please Fill Component Name." ControlToValidate="txtCompName" ForeColor="Red"></asp:RequiredFieldValidator>
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
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lblhsc" runat="server" class="control-label col-sm-6" for="cust">HSN/SAC Code :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txthsc" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill HSN/SAC Code." ControlToValidate="txthsc" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblunit" class="control-label col-sm-6" runat="server">Unit :</asp:Label>
                                <asp:DropDownList ID="DropDownListUnits" runat="server" class="form-control">
                                    <asp:ListItem Value="" Text="Select Unit"></asp:ListItem>
                                </asp:DropDownList><br />
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lbltax" runat="server" class="control-label col-sm-6">Tax % :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txttax" onkeypress="return isNumberKey(event)" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill Tax" ControlToValidate="txttax" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblrate" class="control-label col-sm-6" runat="server">Rate :<span class="spncls">*</span></asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtrate" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please fill Componant Rate" ControlToValidate="txtrate" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>

                        </div>
                        <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary" Text="Submit"  OnClick="btnSubmit_Click" ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary btncancel " Text="Cancel"  CausesValidation="False" OnClick="btnCancel_Click"></asp:Button>
            </div></center>
                        <br />
                    </div>

                    <%--comented code start--%>



<%--                    <hr style="background-color: #000066;" />
                    <div class="col-lg-12 header">
                        <center>
                        <h5>Component List</h5></center>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:TextBox runat="server" class="form-control txtsear" ID="txtSearch" name="Search" placeholder="Search Component Name...." />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCompList" TargetControlID="txtSearch" runat="server">
                            </asp:AutoCompleteExtender>

                        </div>
                        <div class="col-md-4">
                            <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch " CausesValidation="False" OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary lnksearch " OnClick="lnkrefresh_Click" CausesValidation="false"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                        </div>
                        <div class="col-md-4">


                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" class="form-control active1 " OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" Width="150px">
                                <asp:ListItem Value="All" Text="All"></asp:ListItem>
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">DeActive</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div style="width: 100%; padding: 20px; overflow: scroll;" class="table-responsive">
                        <asp:GridView ID="gv_Comp" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Comp_RowCommand" OnRowDataBound="gv_Comp_RowDataBound" PageSize="5" AllowPaging="true" OnPageIndexChanging="gv_Comp_PageIndexChanging" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Component Name">

                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HSN/SAC">

                                    <ItemTemplate>
                                        <asp:Label ID="lblhsn" runat="server" Text='<%# Eval("HSN") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax">

                                    <ItemTemplate>
                                        <asp:Label ID="lbltax" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit">

                                    <ItemTemplate>
                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Units") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">

                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Active">

                                    <ItemTemplate>
                                        <asp:Label ID="lblIsStatus" runat="server" Text='<%# Eval("IsStatus") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("Compid") %>' CommandName="RowEdit" CausesValidation="False"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>


                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Compid") %>' CommandName="RowDelete" CausesValidation="False"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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



                   <%-- comnented code end--%>

                    <asp:HiddenField runat="server" ID="hidden" />

                </div>
            </div>
        </div>
    </form>


</asp:Content>

