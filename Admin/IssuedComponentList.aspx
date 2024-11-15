<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="IssuedComponentList.aspx.cs" Inherits="Admin_IssuedComponentList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }

        .completionList {
            border: solid 1px Gray;
            border-radius: 5px;
            margin: 0px;
            padding: 3px;
            7 height: 200px;
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
                window.location.href = "";
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>

        <div class="col-lg-12 card">
               
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">IssuedComponentList</h5>
            </div>
             <div class="col-md-2">                    
                        <asp:Button ID="Button1" runat="server" class="btn btn-primary btncreatequata" OnClick="Button1_Click" Text="Create" ></asp:Button>
                    </div>
            <br />
            <div class="row">
                    <%-- -----------------date filter------------------%>

                    <div class="col-md-2">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                    </div>
                 <div class="col-md-2 col-xs-3 col-3" style="margin-top: 24px">
                        <asp:LinkButton ID="lnkBtnsearch" OnClick="lnkBtnsearch_Click" runat="server" CssClass="btn btn-primary txtsear" ><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtn_rfresh" runat="server" CssClass="btn btn-primary"  OnClick="lnkBtn_rfresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>

                    <%-----------------------------%>
            </div>
            <hr />
          
            <div class="card-body">

                <div style="width: 100%;">
                    <div class="table-responsive">
                        <asp:GridView ID="GV_IssuedComponent" runat="server" OnRowDataBound="GV_IssuedComponent_RowDataBound" DataKeyNames="JobNo,CustomerName" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging" AllowPaging="true" PageSize="15" OnPageIndexChanging="GV_IssuedComponent_PageIndexChanging" OnRowCommand="GV_IssuedComponent_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20" HeaderText="View">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="CompName" HeaderText="Component Name" />
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Quantity" HeaderText="Quantity" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcustomername" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblproductname" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engineer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEngineerName" runat="server" Text='<%# Eval("EngineerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                         <asp:Label ID="lblissuedate" runat="server" Text='<%# Eval("IssuedDate", "{0:dd-MM-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                             <asp:TemplateField HeaderText="Action" ItemStyle-Width="150px" Visible="false">
                                    <ItemTemplate>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit Component list" CommandName="RowEdit" CommandArgument='<%# Eval("JobNo") %>'><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete Component list" CommandArgument='<%# Container.DataItemIndex %>'  CommandName="RowDelete" AutoPostBack="true" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp; 
                                       <%-- <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView" CommandArgument='<%# Eval("Quotation_no") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>--%>
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
                </div>
            </div>
        </div>
    </form>
</asp:Content>

