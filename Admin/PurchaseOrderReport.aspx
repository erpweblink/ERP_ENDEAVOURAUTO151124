<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="PurchaseOrderReport.aspx.cs" Inherits="Admin_PurchaseOrderReport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/PurchaseOrder.aspx";
            })
        };
    </script>
    <style>
        .lbljob {
            margin-left: 20px;
        }

        .lblcust {
            margin-left: 10px;
        }

        .txtjob {
            margin-left: 22px;
        }

        .txtcust {
            margin-left: 10px;
        }

        .btncreate {
            float: right;
            margin-left: 24px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 900;
        }

        .auto-style1 {
            margin-left: 11;
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
            font-size: 12px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 900;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">
            <%--<div class="card">--%>
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Vendor P.O. Report</h5>
            </div>
            <hr />
            <%--<div class="card-header">
                    <h5></h5>
                </div>--%>

            <div class="card-body">
                <div class="row text-center">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:Label ID="lbl_pono" runat="server" Text="P.O. No. :" CssClass="control-label col-sm-6 lbljob lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_pono_search" CssClass="form-control txtjob" placeholder="P.O. No." runat="server"></asp:TextBox>

                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetPONoList" TargetControlID="txt_pono_search" runat="server">
                            </asp:AutoCompleteExtender>

                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lbl_vendoename" runat="server" Text="Vendor Name :" CssClass="control-label col-sm-6 lblcust lblvendorpo "></asp:Label>
                            <asp:TextBox ID="txt_Vendername_search" CssClass="form-control txtcust " placeholder="Vendor Name" runat="server" onkeypress="return character(event)"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetVendorList" TargetControlID="txt_Vendername_search" runat="server">
                            </asp:AutoCompleteExtender>
                        </div>

                        <div class="col-md-3">
                            <asp:Label ID="lbl_formdate" runat="server" Text="Form Date :" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_form_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lbl_todate" runat="server" Text="To Date :" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_to_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <br />
                            <%--<asp:Label ID="Label1" runat="server" Text="" CssClass="control-label col-sm-6"></asp:Label><br />--%>
                            <asp:LinkButton ID="btn_search" runat="server" CssClass="btn btn-primary lnksearchvendorpo" OnClick="btn_search_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkBtn_rfresh" runat="server" CssClass="btn btn-primary" OnClick="lnkBtn_rfresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                                 <asp:Button ID="btnExportExcel" OnClick="btnExportExcel_Click" style="margin-top: 10px" CssClass="btn btn-primary" runat="server" Text="Export Excel" />
                            </div>              
                    </div>
     
                    <div class="table-responsive text-center" style="width: 100%; padding: 20px;">
                        <asp:GridView ID="GvPurchaseOrderList" runat="server" AutoGenerateColumns="false" CssClass="grid" Width="100%" OnRowCommand="GvPurchaseOrderList_RowCommand">
                            <%--PageSize="10" PagerStyle-CssClass="paging" AllowPaging="true" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vendor Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Vendorname" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="P.O. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="P.O. Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>

                                        <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Eval("PoDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ref. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mobile No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToDateTime( Eval("CreatedOn","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>
                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedOn","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandName="ShowReport" CommandArgument='<%# Eval("Pono")%>' Visible="false">
                                            <i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
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


                        <%--Export grid start--%>

                        <asp:GridView ID="sortedgv" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging" OnRowCommand="GvPurchaseOrderList_RowCommand" PageSize="10" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vendor Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Vendorname" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="P.O. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="P.O. Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbl_PoDate" runat="server" Text='<%# Convert.ToDateTime( Eval("PoDate","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>

                                        <asp:Label ID="lbl_PoDate" runat="server" Text='<%# Eval("PoDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ref. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Refno" runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mobile No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lBL_Mobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToDateTime( Eval("CreatedOn","{0:d}")).ToString("dd/MM/yyyy") %>'></asp:Label>--%>
                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedOn","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandName="ShowReport" CommandArgument='<%# Eval("Pono")%>' Visible="false">
                                            <i class="fas fa-eye" style="font-size:24px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
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

                        <%--Export grid end--%>

                        <asp:HiddenField ID="hdId" runat="server"></asp:HiddenField>
                    </div>
                </div>
            </div>
        </div>
        <%-- </div>--%>
    </form>

</asp:Content>

