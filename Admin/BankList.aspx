<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="BankList.aspx.cs" Inherits="Admin_BankList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
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

        .active1 {
            float: right;
            margin-right: 80px
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

        .lnksearch {
        }

        .btnlnkgrid {
            width: 150px;
        }
    </style>
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/BankList.aspx";
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Bank List</h5>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <asp:TextBox runat="server" class="form-control txtsear" ID="txtSearch" name="Search" placeholder="Bank Name" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetBankList" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch " OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary lnksearch lnkrefreshtab" ><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>
                    <%--<div class="col-md-3 col-xs-7 col-7">
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" class="form-control active2 " Width="150px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Value="All" Text="All"></asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">DeActive</asp:ListItem>
                        </asp:DropDownList>
                    </div>--%>
                     <div class="col-md-3 col-xs-7 col-7">
                       
                    </div>
                    <div class="col-md-4 col-xs-5 col-5">
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreate" Text="Create" OnClick="btncreate_Click"></asp:Button>
                    </div>

                </div>


                <div style="width: 100%; padding: 20px;">
                    <div class="table-responsive">
                        <asp:GridView ID="GVBank" runat="server" OnRowDataBound="GVBank_RowDataBound" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                               PageSize="10" OnRowCommand="GVBank_RowCommand" AllowPaging="true" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbankname" runat="server" Text='<%# Eval("BankName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblaccountno" runat="server" Text='<%# Eval("AccounNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IFSC Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblifsccode" runat="server" Text='<%# Eval("IFSCCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbranchname" runat="server" Text='<%# Eval("BranchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="btnlnkgrid">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("id") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

