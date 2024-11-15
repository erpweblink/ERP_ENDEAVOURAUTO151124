<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Expenses.aspx.cs" Inherits="Admin_Expenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <style>
        .spncls {
            color: red;
        }

        .txtcustomer {
        }
    </style>
    <script type='text/javascript'>

        function scrollToElement() {
            var target = document.getElementById("divdtls").offsetTop;
            window.scrollTo(0, target);
        }
    </script>
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/Expenses.aspx";
            })
        };
    </script>
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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="col-lg-12">
            <asp:HiddenField ID="hidden" runat="server" />
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row alige-items-center justify-content-beetween">
                    <h5 class="m-0 font-weight-bold text-primary">Office Expenses</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblexpensestype" runat="server" CssClass="control-table col-sm-6" For="cust" Text="Expenses Type"></asp:Label>
                            <asp:TextBox ID="textexpensestype" CssClass="form-control" runat="server"></asp:TextBox>

                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblbalacesheet" runat="server" CssClass="contro-table col-sm-6" For="cust" Text="Balance Sheet"></asp:Label>
                            <asp:TextBox ID="textbalancesheet" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblnarration" runat="server" CssClass="control-table col-sm-6" For="cust" Text="Narration"></asp:Label>
                            <asp:TextBox ID="textnarration" CssClass="form-control" runat="server"></asp:TextBox>

                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lbldate" runat="server" CssClass="contro-table col-sm-6" For="cust" Text="Date"></asp:Label>
                            <asp:TextBox ID="textdate" CssClass="form-control " TextMode="Date" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblamount" runat="server" CssClass="contro-table col-sm-6" For="cust" Text="Amount"></asp:Label>
                            <asp:TextBox ID="textamount" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnadd" CssClass="btn btn-primary form-control" OnClick="btnadd_Click" runat="server" Text="Save" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btncancel" CssClass="btn btn-primary form-control" OnClick="btncancel_Click" runat="server" Text="Cancel" />
                        </div>
                        <div class="col-4">
                        </div>
                    </div>
                    <div style="width: 100%; padding: 20px;">
                        <asp:GridView ID="gv_officeexpenses" runat="server" AutoGenerateColumns="false" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging" OnRowCommand="gv_officeexpenses_RowCommand" OnPageIndexChanging="gv_officeexpenses_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expensive Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpensivetype" Text='<%# Eval("ExpensiveType") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BalanceSheet">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbalancesheet" Text='<%# Eval("BalanceSheet") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Narration">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnarration" Text='<%# Eval("Narration") %>' runat="server"> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldate" Text='<%# Eval("Date") %>' runat="server"> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblamount" Text='<%# Eval("Amount") %>' runat="server"> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="btnlnkgrid">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("Id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                    <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("Id") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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


