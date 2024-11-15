<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Evalution.aspx.cs" Inherits="Reception_Evalution" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11;
        }


        .btncreate {
            float: right;
            margin-right: 27px;
        }

        .txtsear {
            margin-left: -105px;
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

        .dateser {
            margin-left: 34px;
        }

        .comp {
            width: 300px;
            margin-left: 13px;
        }

        .lblcomp {
            margin-left: 13px;
        }

        .spncls {
            color: red;
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
                window.location.href = "../Admin/Evalution.aspx";
            })
        };
    </script>
    <script type="text/javascript">
        function ShowPopup(title, body) {
            debugger;
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Testing Product</h5>
            </div>
            <hr />
            <div class="card-body">
                <div class="row">
                    <%-- <div class="col-md-1">
                      
                    </div>--%>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Job No. :"></asp:Label>
                        <asp:TextBox ID="txtJobNo" class="form-control txtjob " placeholder="Search Job No." runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJOBNOList" TargetControlID="txtJobNo" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <%-- <div class="col-md-2 mt-top">
                       
                    </div>--%>
                    <div class="col-md-2">
                        <asp:Label ID="lblCustomer" runat="server" Text="Customer Name :"></asp:Label>
                        <asp:TextBox ID="txtcustomername" class="form-control txtcustomer" placeholder="Customer Name" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtcustomername" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>


                    <div class="col-md-2">
                        <asp:Label ID="lblproduct" runat="server" Text="Product :"></asp:Label>
                        <asp:TextBox ID="txtproduct" class="form-control txtcustomer" placeholder="Product" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetProductList" TargetControlID="txtproduct" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblstatus" runat="server" Text="Status :"></asp:Label>
                        <asp:TextBox ID="txtstatus" class="form-control txtcustomer" placeholder="Status" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetStatusList" TargetControlID="txtstatus" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <%-- -----------------date filter------------------%>

                    <div class="col-md-2">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                    </div>

                    <div class="row">
                    </div>


                    <div class="col-md-3 ddlevalution mt-top ">
                        <asp:DropDownList ID="ddlEvalution" runat="server" Style="margin-top: 10px;" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEvalution_SelectedIndexChanged">
                            <asp:ListItem Value="Select" Text="Select"></asp:ListItem>
                            <asp:ListItem Value="All" Text="All"></asp:ListItem>
                            <asp:ListItem Value="1">Tested List</asp:ListItem>
                            <asp:ListItem Value="0">Pending List</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2 mt-top ">
                        <asp:LinkButton ID="btn_search" OnClick="btn_search_Click" runat="server" Style="margin-top: 10px;" CssClass="btn btn-primary "><i class="fa fa-search"  style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtn_rfresh" runat="server" Style="margin-top: 10px;" CssClass="btn btn-primary" OnClick="lnkBtn_rfresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-2 mt-top ">
                        <asp:Button ID="Button1" runat="server" Style="margin-top: 10px;" class="btn btn-primary" Text="Export to Excel" OnClick="lnkbtn_ExportExcel_Click"></asp:Button>
                    </div>

                </div>
                <br />
                <div class="row">
                    <div class="col-md-7"></div>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" BackColor="Red" Width="20px" Height="20px" Enabled="false"></asp:TextBox><asp:Label runat="server" Text="Testing Pending."></asp:Label>
                    </div>
                    <div class="col-md-3 mt-top">
                        <asp:TextBox runat="server" BackColor="green" Width="20px" Height="20px" Enabled="false"></asp:TextBox><asp:Label runat="server" Text="Testing Completed."></asp:Label>
                    </div>
                </div>

                </br>
                
                <div style="width: 100%;">

                    <div runat="server" id="dgvgridview">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_Evalution" runat="server" CellPadding="3" Width="100%" AutoGenerateColumns="false"
                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Evalution_RowCommand" OnPageIndexChanging="gv_Evalution_PageIndexChanging"
                                OnRowDataBound="gv_Evalution_RowDataBound" PageSize="5" PagerStyle-CssClass="paging" AllowPaging="true">


                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SubCustomer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCustomer" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Entry Date">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("EntryDate")).ToString("dd/MM/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engineer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblenginame" runat="server" Text='<%# Eval("EngiName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            <asp:Label ID="lbliscompleted" runat="server" Text='<%# Eval("isCompleted") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkbtnTotesting" ToolTip="Go Testing" CommandArgument='<%# Eval("JobNo") %>' CommandName="selectdata">
                                              <i class="fa fa-sign-in" style="font-size:24px"></i>
                                            </asp:LinkButton>
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



                            <%--    <asp:GridView ID="gv_Evalution" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                PageSize="5" AllowPaging="true" PagerStyle-CssClass="paging" OnRowCommand="gv_Evalution_RowCommand" OnPageIndexChanging="gv_Evalution_PageIndexChanging"   OnRowDataBound="gv_Evalution_RowDataBound" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubCustomer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCustomer" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Entry Date">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("EntryDate")).ToString("dd/MM/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engineer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblenginame" runat="server" Text='<%# Eval("EngiName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            <asp:Label ID="lbliscompleted" runat="server" Text='<%# Eval("isCompleted") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkbtnTotesting" ToolTip="Go Testing" CommandArgument='<%# Eval("JobNo") %>' CommandName="selectdata">
                                              <i class="fa fa-sign-in" style="font-size:24px"></i>
                                            </asp:LinkButton>
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

                            </asp:GridView>--%>
                        </div>
                    </div>


                </div>

                <%-- Filter Grid start--%>

                <div style="width: 100%;">
                    <div runat="server" id="divgridfilter">
                        <div class="table-responsive">
                            <asp:GridView ID="Grid_Filter" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%"
                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" OnRowCommand="Grid_Filter_RowCommand" OnPageIndexChanging="Grid_Filter_PageIndexChanging"
                                OnRowDataBound="gv_Evalution_RowDataBound" PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SubCustomer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCustomer" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entry Date">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("EntryDate")).ToString("dd/MM/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engineer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbEngiName" runat="server" Text='<%# Eval("EngiName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            <asp:Label ID="lbliscompleted" runat="server" Text='<%# Eval("isCompleted") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkbtnTotesting" ToolTip="Go Testing" CommandArgument='<%# Eval("JobNo") %>' CommandName="selectdata">
                                              <i class="fa fa-sign-in" style="font-size:24px"></i>
                                            </asp:LinkButton>
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

                <%-- Filter Grid End--%>

                <%--    SortedGridStart--%>
                <div style="width: 100%;">
                    <div runat="server" id="div1">
                        <div class="table-responsive">
                            <asp:GridView ID="SortGvEvaluations" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%"
                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" OnRowCommand="Grid_Filter_RowCommand" OnPageIndexChanging="SortGvEvaluations_PageIndexChanging"
                                OnRowDataBound="gv_Evalution_RowDataBound" PageSize="5" AllowPaging="true" PagerStyle-CssClass="paging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SubCustomer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCustomer" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entry Date">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("EntryDate")).ToString("dd/MM/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engineer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbEngiName" runat="server" Text='<%# Eval("EngiName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            <asp:Label ID="lbliscompleted" runat="server" Text='<%# Eval("isCompleted") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkbtnTotesting" ToolTip="Go Testing" CommandArgument='<%# Eval("JobNo") %>' CommandName="selectdata">
                                              <i class="fa fa-sign-in" style="font-size:24px"></i>
                                            </asp:LinkButton>
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
                <%--   SortedGridend--%>


                <%--Export to Excel Grid Start--%>
                <div style="width: 100%;">
                    <div runat="server" id="div2">
                        <div class="table-responsive">
                            <asp:GridView ID="ExportToExcelGrid" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%"
                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" PagerStyle-CssClass="paging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SubCustomer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCustomer" runat="server" Text='<%# Eval("Subcustomer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entry Date">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("EntryDate")).ToString("dd/MM/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engineer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbEngiName" runat="server" Text='<%# Eval("EngiName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSriNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            <asp:Label ID="lbliscompleted" runat="server" Text='<%# Eval("isCompleted") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Created By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("id") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkbtnTotesting" ToolTip="Go Testing" CommandArgument='<%# Eval("JobNo") %>' CommandName="selectdata">
                                              <i class="fa fa-sign-in" style="font-size:24px"></i>
                                            </asp:LinkButton>
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
                <%--Export To Excel Grid End--%>
            </div>
        </div>
    </form>
</asp:Content>

