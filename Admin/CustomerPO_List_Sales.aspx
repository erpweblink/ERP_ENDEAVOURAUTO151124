<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="CustomerPO_List_Sales.aspx.cs" Inherits="Admin_CustomerPO_List_Sales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                window.location.href = "../Admin/CustomerPO_list.aspx";
                })
        };
    </script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "img/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "img/plus.png");
            $(this).closest("tr").next().remove();
        });
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
            font-size: 12px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 900;
        }

        /*New CSS*/
        .panelinward {
            border: 1px solid darkgray;
            width: 55%;
            padding: 20px !important;
            background-color: whitesmoke;
            left: 378px !important;
        }

        .floa {
            float: right;
        }

        .lbl {
            font-weight: bold;
        }

        .btnclose {
            margin-top: -2%;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">
            <%--<div class="card">--%>
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Customer P.O. List (Sales)</h5>

                 <asp:LinkButton ID="lnkshow" runat="server" CommandArgument='20' OnClick="lnkshow_Click">
                     <%--Pending Job No.--%><i class="fas fa-bell" style="font-size: 30px; color: #0755A1">
                             <span class="badge bg-danger" id="notificationCount" runat="server">
                                <asp:Label ID="lblquatation" runat="server" Text="0"></asp:Label>
                             </span>
                          </i>
                 </asp:LinkButton>

            </div>
            <hr />
            <div class="card-body">
                <div class="row text-center">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:Label ID="lbljobno" runat="server" Text="Customer Name" CssClass="lblcustomer"></asp:Label>
                            <asp:TextBox ID="txtJobno" CssClass="form-control jobtxtcustomer " placeholder="Customer Name" runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtJobno" runat="server">
                            </asp:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lbl_pono" runat="server" Text="P.O. Number" CssClass="control-label col-sm-6 lblcust lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txt_pono_search" CssClass="form-control txtcust" placeholder="P.O. No." runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetPOList" TargetControlID="txt_pono_search" runat="server">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lbljob" runat="server" Text="Job No." CssClass="control-label col-sm-6 lblcust lblvendorpo"></asp:Label>
                            <asp:TextBox ID="txtjob" CssClass="form-control txtcust" placeholder="Job. No." runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetPOList" TargetControlID="txtjob" runat="server">
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
                            <asp:LinkButton ID="btn_search" runat="server" OnClick="btn_search_Click" CssClass="btn btn-primary lnksearchvendorpo"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkBtn_rfresh" runat="server" OnClick="lnkBtn_rfresh_Click" CssClass="btn btn-primary"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                        </div>

                        <div class="col-md-3">
                            <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type <span class="spncls"></span></asp:Label>
                            <asp:DropDownList runat="server" class="form-control" OnTextChanged="ddlservicetype_TextChanged" AutoPostBack="true" ID="ddlservicetype">
                                <asp:ListItem Value="Service" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                                <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                                <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2 col-xs-5 col-5">
                            <br />
                            <asp:Button ID="btncreate" runat="server" class="btn btn-primary " Text="Create" OnClick="btncreate_Click"></asp:Button>
                            <asp:Button ID="btnexporttoexcel" runat="server" class="btn btn-primary " Text="Export" OnClick="btnexporttoexcel_Click"></asp:Button>
                        </div>



                    </div>
                    <div class="table-responsive text-center" style="width: 100%; padding: 20px;">
                        <asp:GridView ID="GvCustomerpoList" runat="server" AutoGenerateColumns="false" CssClass="grid" DataKeyNames="Quotationno"  Width="100%" OnRowCommand="GvCustomerpoList_RowCommand" 
                            OnRowDataBound="GvCustomerpoList_RowDataBound" ShowFooter="True">
                            <%--AllowPaging="true" PageSize="10" OnPageIndexChanging="GvCustomerpoList_PageIndexChanging" PagerStyle-CssClass="paging"--%>
                            <Columns>
                            <%--    <asp:TemplateField HeaderStyle-Width="20" HeaderText="View">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDetailss" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quotation_no" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" Visible="false" runat="server" Text='<%# Eval("Quotationno") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>

                                        <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_subcustomer" runat="server" Text='<%# Eval("SubCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Job No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P.O. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P.O. Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PoDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblpodate" />
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
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblcreteddate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Count days">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcountdays" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Type">
                                     <ItemTemplate>
                                         <asp:Label ID="lbltype" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                     </ItemTemplate>
                                 </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField Visible="false" HeaderText="QuotationNO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblquotation" runat="server" Text='<%# Eval("Quotationno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <%-- <a href="<%#"Customer_PO.aspx?Id="+Eval("Id") %>"><i class='far fa-edit' style='font-size: 26px'></i></a>--%>
                                        <asp:LinkButton ID="btn_edit" runat="server" CommandName="Rowedit" CommandArgument='<%#Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" ToolTip="Delete" CommandName="RowDelete" CommandArgument='<%# Eval("Id") %>' OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnsendpo" runat="server" CommandName="SendINV" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Send Invoice">
                                          <i class="fa fa-paper-plane" style="font-size:26px;color:black"></i>
                                        </asp:LinkButton>
                                        <%--<asp:LinkButton runat="server" ID="lnkbtnsendpo" ToolTip="Send PO" CommandName="SendINV"><i class="fa fa-paper-plane"  style="font-size: 26px; color:black; "></i></i></asp:LinkButton>--%>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblFooterTotalAmt" runat="server" Text="Total Amt:" Font-Bold="True"></asp:Label>
                                    </FooterTemplate>
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
                    </div>

                    <%--   SortedGrid start--%>
                    <div class="table-responsive text-center" style="width: 100%; padding: 20px;">
                        <asp:GridView ID="GvSorted" runat="server" AutoGenerateColumns="false" CssClass="grid" DataKeyNames="Quotationno" Width="100%" OnRowCommand="GvCustomerpoList_RowCommand" OnRowDataBound="GvCustomerpoList_RowDataBound" ShowFooter="True">
                            <%--AllowPaging="true" PageSize="10" PagerStyle-CssClass="paging" OnPageIndexChanging="GvSorted_PageIndexChanging"--%>
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20" HeaderText="View">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDetailss" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Quotation_no" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" Visible="false" runat="server" Text='<%# Eval("Quotationno") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>


                                        <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_subcustomer" runat="server" Text='<%# Eval("SubCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Job No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P.O. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P.O. Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PoDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblpodate" />
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
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblcreteddate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Count days">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcountdays" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <%-- <a href="<%#"Customer_PO.aspx?Id="+Eval("Id") %>"><i class='far fa-edit' style='font-size: 26px'></i></a>--%>
                                        <asp:LinkButton ID="btn_edit" runat="server" CommandName="Rowedit" CommandArgument='<%#Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" ToolTip="Delete" CommandName="RowDelete" CommandArgument='<%# Eval("Id") %>' OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblFooterTotalAmt" runat="server" Text="Total Amt:" Font-Bold="True"></asp:Label>
                                    </FooterTemplate>
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
                        <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                    </div>
                    <%--   sorted Grid end--%>

                    <%--Export to Excel Grid Start--%>

                    <div class="table-responsive text-center" style="width: 100%; padding: 20px;">
                        <asp:GridView ID="GridExportExcel" runat="server" AutoGenerateColumns="false" CssClass="grid" DataKeyNames="Quotationno" Width="100%" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20" HeaderText="View" Visible="false">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDetailss" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_srno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Quotation_no" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" Visible="false" runat="server" Text='<%# Eval("Quotationno") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>


                                        <asp:Label ID="lbl_Customername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_subcustomer" runat="server" Text='<%# Eval("SubCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Job No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_JobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P.O. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Pono" runat="server" Text='<%# Eval("Pono") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P.O. Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PoDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblpodate" />
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
                                        <asp:Label ID="lblcreatedby" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblcreteddate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Count days">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcountdays" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotal" runat="server" Text='<%# Eval("GrandTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_View" runat="server" CommandName="RowPrint" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate Pdf"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_Viewpopdf" runat="server" CommandName="RowViewPO" CommandArgument='<%# Eval("Id") %>' ToolTip="Genrate PO"><i class="fas fa-file-pdf"  style="font-size:26px;color:red"></i></asp:LinkButton>
                                        <%-- <a href="<%#"Customer_PO.aspx?Id="+Eval("Id") %>"><i class='far fa-edit' style='font-size: 26px'></i></a>--%>
                                        <asp:LinkButton ID="btn_edit" runat="server" CommandName="Rowedit" CommandArgument='<%#Eval("Id") %>' ToolTip="Edit"><i class="far fa-edit"  style="font-size:26px"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btn_delete" runat="server" Text="" ToolTip="Delete" CommandName="RowDelete" CommandArgument='<%# Eval("Id") %>' OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash" aria-hidden="true" style="font-size:26px"></i></asp:LinkButton>
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
                        <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
                    </div>

                    <%--Export to Excel Grid End--%>

                     <%-- New Code for PopUP Shubham Patil --%>

                     <asp:Button ID="btnprof" runat="server" Style="display: none" />
                     <asp:ModalPopupExtender ID="modelprofile" runat="server" TargetControlID="btnprof"
                         PopupControlID="PopupAddDetail" OkControlID="Closepopdetail" BackgroundCssClass="modalBackground" />

                     <asp:Panel ID="PopupAddDetail" runat="server" class="w3-panel w3-white panelinward m-0 font-weight-bold text-primary"
                         Direction="LeftToRight" Wrap="true" Style="display: none;">

                         <div class="d-flex justify-content-between align-items-center">

                             <h5 class="m-0 font-weight-bold text-primary">Quatation Pending List </h5>


                             <asp:LinkButton ID="Closepopdetail" runat="server" class="btn-close">
                                 <i class="fa fa-close" style="font-size:24px;color:red;"></i>
                             </asp:LinkButton>
                         </div>
                         <br />
                         <div class="row">
                             <div class="table-container" style="height: 300px; overflow-y: auto;">
                                 <asp:GridView ID="gv_EstimationList" runat="server" AutoGenerateColumns="False"  OnRowCommand="gv_EstimationList_RowCommand" 
                                     CellPadding="3" CssClass="custom-grid" AllowPaging="false"
                                     HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">

                                     <Columns>

                                         <asp:TemplateField HeaderText="Sr. No.">
                                             <ItemTemplate>
                                                 <asp:Label ID="Label28" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Quotation No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblquotationNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Customer Name">
                                             <HeaderStyle Width="491px" />
                                             <ItemStyle Width="491px" />
                                             <ItemTemplate>
                                                 <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Quotation Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblquotation" runat="server" 
                                                           Text='<%# Eval("Quotation_Date", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Count">
                                             <ItemTemplate>
                                                 <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("Days_Completed") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Action">
                                             <ItemTemplate>
                                                <%--<asp:LinkButton runat="server" ID="lnkbtnCorrect" ToolTip="Correct"
                                                    CommandArgument='<%# Eval("Quotation_no") %>' CommandName="RowCorrect" OnClientClick="databinding"
                                                    CausesValidation="False" 
                                                    >
                                                    <i class="fa fa-check" style="font-size:24px; color:green"></i>
                                                </asp:LinkButton>--%>
                                                 <asp:LinkButton runat="server" ID="lnkbtnCorrect" ToolTip="Correct"
                                                     CommandArgument='<%# Eval("Quotation_no") %>' CommandName="RowCorrect"
                                                     CausesValidation="False" OnClick="lnkbtnCorrect_Click">
                                                     <i class="fa fa-check" style="font-size:24px; color:green"></i>
                                                 </asp:LinkButton>

                                                 &nbsp;&nbsp;
                                                    <%-- <asp:LinkButton runat="server" ID="lnkbtnClose" ToolTip="Close"
                                                         OnClientClick="Javascript:return confirm('Are you sure to Close?')"
                                                         CommandArgument='0' CommandName="RowClose"
                                                         CausesValidation="False">
                                                         <i class="fa fa-times" style="font-size:24px; color:red"></i>
                                                     </asp:LinkButton>--%>
                                                 <asp:LinkButton runat="server" ID="lnkbtnClose" ToolTip="Close"
                                                    OnClientClick="return confirm('Are you sure you want to close this entry?');"
                                                    CommandArgument='<%# Eval("ID") %>' 
                                                    CommandName="CloseQuotation"
                                                    CausesValidation="False">
                                                    <i class="fa fa-times" style="font-size:24px; color:red"></i>
                                                </asp:LinkButton>

                                             </ItemTemplate>

                                         </asp:TemplateField>
                                     </Columns>
                                     <FooterStyle BackColor="White" ForeColor="#000066" />
                                     <RowStyle ForeColor="#000066" />
                                     <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                 </asp:GridView>
                             </div>
                         </div>
                     </asp:Panel>
                     <%-- Old code --%>


                </div>
            </div>
        </div>

    </form>

</asp:Content>

