<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="PurchaseOrderList.aspx.cs" Inherits="Admin_PurchaseOrderList"  EnableEventValidation="false"%>

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
            font-size: 16px;
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
                <h5 class="m-0 font-weight-bold text-primary">Vendor P.O List</h5>
            </div>
            <%--<div class="card-header">
                    <h5></h5>
                </div>--%>
                <hr />
                <div class="card-body">
                    <div class="row text-center">
                        <div class="row">
                        <div class="col-md-2">
                            <asp:Label ID="lbl_pono" runat="server" Text="Po. No." CssClass="control-label col-sm-6  lbljob lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_pono_search" CssClass="form-control txtjob" placeholder="Search Po. No." runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetPoList" TargetControlID="txt_pono_search" runat="server">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-2 mt-top">
                            <asp:Label ID="lbl_vendoename" runat="server" Text="Vendor Name" CssClass="control-label col-sm-6 lblcust lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_Vendername_search" CssClass="form-control txtcust " placeholder="Search Vendor" runat="server" onkeypress="return character(event)"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetVendorList" TargetControlID="txt_Vendername_search" runat="server">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-2 mt-top">
                            <asp:Label ID="lbl_formdate" runat="server" Text="Form Date" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_form_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2 mt-top">
                            <asp:Label ID="lbl_todate" runat="server" Text="To Date" CssClass="control-label col-sm-6 lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_to_podate_search" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-xs-7 col-7 mt-top">
                            <br />
                            <%--<asp:Label ID="Label1" runat="server" Text="" CssClass="control-label col-sm-6"></asp:Label><br />--%>
                            <asp:LinkButton ID="btn_search" OnClick="btn_search_Click1" runat="server" CssClass="btn btn-primary lnksearchvendorpo"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkBtn_rfresh" runat="server" CssClass="btn btn-primary" OnClick="lnkBtn_rfresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>

                        </div>
                        <div class="col-md-2 col-xs-5 col-5">
                            <br />
                            <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreate" Text="Create" OnClick="btncreate_Click"></asp:Button>
                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary btncreate" Text="Export"  OnClick="btnExport_Click"></asp:Button>
                        </div>
                    </div>
                    <div class="table-responsive text-center" style="width: 100%; padding: 20px;">
                        <asp:GridView ID="GvPurchaseOrderList" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" OnRowDataBound="GvPurchaseOrderList_RowDataBound" OnRowCommand="GvPurchaseOrderList_RowCommand" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="GvPurchaseOrderList_PageIndexChanging">
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
                                <asp:TemplateField HeaderText="Po. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Po. Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PoDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblpodate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ref No.">
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
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblQuDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkedit" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>'><i class='far fa-edit' style='font-size: 26px'></i></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" OnClick="btn_delete_Click" ToolTip="Delete" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
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
                        <asp:HiddenField ID="hdId" runat="server"></asp:HiddenField>

                        <%--   sorted Grid started--%>
                        <asp:GridView ID="sortedGv" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%" OnRowDataBound="GvPurchaseOrderList_RowDataBound" OnRowCommand="GvPurchaseOrderList_RowCommand" PagerStyle-CssClass="paging" PageSize="10" OnPageIndexChanging="sortedGv_PageIndexChanging">
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
                                <asp:TemplateField HeaderText="Po. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Po. Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PoDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblpodate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ref No.">
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
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblQuDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkedit" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>'><i class='far fa-edit' style='font-size: 26px'></i></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" OnClick="btn_delete_Click" ToolTip="Delete" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
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

                        <%--                        started Grid End--%>



<%--                        Export Grid--%>
                          <asp:GridView ID="GVExportExcel" runat="server" AutoGenerateColumns="false" CssClass="grid" AllowPaging="true" Width="100%"   PagerStyle-CssClass="paging" >
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
                                <asp:TemplateField HeaderText="Po. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Po. Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PoDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblpodate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ref No.">
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
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblQuDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                           <%--     <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkedit" CommandName="RowEdit" CommandArgument='<%# Eval("Id") %>'><i class='far fa-edit' style='font-size: 26px'></i></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" OnClick="btn_delete_Click" ToolTip="Delete" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
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
                </div>
            </div>
        </div>
        <%-- </div>--%>
    </form>

</asp:Content>

