<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="QuotationList.aspx.cs" Inherits="Admin_QuotationList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .auto-style1 {
            margin-left: 11px;
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

        /*Bell Icon Css*/
        @keyframes bellBounce {
            0% {
                transform: translateX(0);
            }

            25% {
                transform: translateX(-5px);
            }

            50% {
                transform: translateX(5px);
            }

            75% {
                transform: translateX(-5px);
            }

            100% {
                transform: translateX(0);
            }
        }

        .bell-bounce {
            animation: bellBounce 1s ease-in-out infinite;
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

    <%-- New script Nikhil --%>

    <script type="text/javascript">

        var selectedJobNos = [];

        function toggleJobSelection(checkbox) {
            var jobNo = checkbox.closest('tr').querySelector('[data-jobno]').innerText;

            if (checkbox.checked) {
                if (!selectedJobNos.includes(jobNo)) {
                    selectedJobNos.push(jobNo);
                }
            } else {
                var index = selectedJobNos.indexOf(jobNo);
                if (index !== -1) {
                    selectedJobNos.splice(index, 1);
                }
            }
        }

        function validateJobSelection() {
            if (selectedJobNos.length === 0) {
                alert('Please select a Job to be closed...');
                return false;
            } else {
                if (confirm('Are you sure you want to close selected Jobs?')) {
                    sendJobNosToServer(selectedJobNos);
                }
            }
            return false;
        }
        function sendJobNosToServer(jobNos) {
            fetch('QuotationList.aspx/ProcessJobNos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ jobNos: jobNos }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.d) {
                        alert('Job closed successfully.');
                    } else {
                        alert('Failed to process job numbers.');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while processing job numbers.');
                });
        }
        function ReloadPage() {
            window.location.href = "../Admin/QuotationList.aspx";
        }
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12 card">
            <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h5 class="m-0 font-weight-bold text-primary">Quotation List</h5>

                <asp:LinkButton ID="lnkshow" runat="server" CommandArgument='20' OnClick="lnkshow_Click">
                    <i class="fas fa-bell" style="font-size: 30px; color: #0755A1"><span class="badge bg-danger" id="notificationCount" runat="server">
                        <asp:Label ID="lblcount" runat="server" Text="0"></asp:Label>
                    </span></i>
                </asp:LinkButton>
            </div>

            <hr />
            <div class="card-body">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lbljobno" runat="server" Text="Quotation No. :" CssClass="lblcust"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsearjobqua" ID="txtquotation" name="Search" placeholder=" Quotation No." />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetJobList" TargetControlID="txtquotation" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2 mt-top">
                        <asp:Label ID="Label1" runat="server" Text="Company Name :" CssClass="lblcust"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtsearcomqua" ID="txtSearch" name="Search" placeholder=" Company Name" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>

                    <div class="col-md-2 mt-top">
                        <asp:Label ID="Label2" runat="server" Text="Quotation Date :" CssClass="lblquatation"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtdate" ID="txtDateSearch" name="Search" placeholder=" Enter Date" TextMode="Date" />
                    </div>

                    <%-- <div class="col-md-2 mt-top">
                        <asp:Label ID="Label3" runat="server" Text="Expiry Date" CssClass="lblexpirydate"></asp:Label>
                        <asp:TextBox runat="server" class="form-control txtdate" ID="txtexpirydate" name="Search" placeholder=" Expiry Date" TextMode="Date" />
                    </div>--%>

                    <div class="col-md-2 mt-top">
                        <br />
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary " OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size: 24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtn_rfresh" runat="server" CssClass="btn btn-primary " OnClick="lnkBtn_rfresh_Click"><i class="fa fa-refresh" style="font-size: 24px"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-4 mt-top">
                        <br />
                        <asp:LinkButton ID="btncreate" runat="server" CssClass="btn btn-primary " Text="Create" OnClick="btncreate_Click"></asp:LinkButton>
                        <asp:LinkButton ID="btnexportexcel" runat="server" CssClass="btn btn-primary " Text="Export to Excel" OnClick="btnexportexcel_Click"></asp:LinkButton>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblfromdate" runat="server" class="control-label col-sm-6">From Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchfrom" name="Search" TextMode="Date" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lbltodate" runat="server" class="control-label col-sm-6">To Date </asp:Label>
                        <asp:TextBox runat="server" class="form-control " ID="txtDateSearchto" name="Search" TextMode="Date" />
                    </div>

                    <div class="col-md-3">
                        <asp:Label ID="Label14" runat="server" class="control-label col-sm-4">Service Type :<span class="spncls"></span></asp:Label>
                        <asp:DropDownList runat="server" class="form-control" OnTextChanged="ddlservicetype_TextChanged" AutoPostBack="true" ID="ddlservicetype">
                            <asp:ListItem Value="Service" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                            <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                            <asp:ListItem Value="Reparing" Text="Reparing"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                </br>
                <div style="width: 100%;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_Quot_List" runat="server" OnRowDataBound="gv_Quot_List_RowDataBound" AutoGenerateColumns="False" DataKeyNames="Quotation_no" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Quot_List_RowCommand" ShowFooter="True">
                            <%--PagerStyle-CssClass="paging" AllowPaging="true" PageSize="15" OnPageIndexChanging="gv_Quot_List_PageIndexChanging"--%>
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20" HeaderText="View">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <%--<asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>--%>
                                                    <%--<asp:BoundField ItemStyle-Width="150px" DataField="Id" HeaderText="Sr. No." />--%>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                </Columns>
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="JobStatus" HeaderText="Status" />
                                                </Columns>
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="JobDaysCount" HeaderText="Completed Days" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--///////--%>
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
                                <%-- <asp:TemplateField HeaderText="JOB No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Quo.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsubcustomer" runat="server" Text='<%# Eval("SubCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblkindAtt" runat="server" Text='<%# Eval("kind_Att") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quatation Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Quotation_Date", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblQuDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("ExpiryDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblExpDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblCreDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreateuser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Count Days">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("CountDays") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Agains By">
                                    <ItemTemplate>
                                        <asp:Label ID="Againstby" runat="server" Text='<%# Eval("Againstby") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <%--                                        <asp:LinkButton runat="server" ID="lnkCreateQua" ToolTip="Create Quotation" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowQuotation"><i class="fa fa-clipboard" style="font-size:24px"></i></i></asp:LinkButton>--%>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit Quotation" CommandName="RowEdit" CommandArgument='<%# Eval("Quotation_no") %>'><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete Quotation" CommandArgument='<%# Eval("ID") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView" CommandArgument='<%# Eval("Quotation_no") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>
                                        <%--&nbsp;&nbsp; 
                                        <asp:LinkButton runat="server" ID="lnkbtnsendpo" ToolTip="Send PO" CommandName="SendPO" CommandArgument='<%# Eval("Quotation_no") %>'><i class="fa fa-paper-plane"  style="font-size: 26px; color:black; "></i></i></asp:LinkButton>--%>
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

                        <%--   Sorted Grid start--%>

                        <asp:GridView ID="sortedgv" runat="server" OnRowDataBound="gv_Quot_List_RowDataBound" AutoGenerateColumns="False" DataKeyNames="Quotation_no" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Quot_List_RowCommand" ShowFooter="True">
                            <%--PagerStyle-CssClass="paging" AllowPaging="true" PageSize="15" OnPageIndexChanging="sortedgv_PageIndexChanging"--%>
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20" HeaderText="View">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="Panel1" runat="server" Style="display: none">
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <%--<asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>--%>
                                                    <%--<asp:BoundField ItemStyle-Width="150px" DataField="Id" HeaderText="Sr. No." />--%>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--///////--%>
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
                                <%-- <asp:TemplateField HeaderText="JOB No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Quo.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuNo" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsubcustomer" runat="server" Text='<%# Eval("SubCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblkindAtt" runat="server" Text='<%# Eval("kind_Att") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotalAmt" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quatation Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Quotation_Date", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblQuDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("ExpiryDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblExpDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="lblCreDate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreateuser" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Count Days">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldaycount" runat="server" Text='<%# Eval("days") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton runat="server" ID="lnkCreateQua" ToolTip="Create Quotation" CommandArgument='<%# Eval("JobNo") %>' CommandName="RowQuotation"><i class="fa fa-clipboard" style="font-size:24px"></i></i></asp:LinkButton>--%>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit Quotation" CommandName="RowEdit" CommandArgument='<%# Eval("Quotation_no") %>'><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete Quotation" CommandArgument='<%# Eval("ID") %>' CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton runat="server" ID="lnkBtn_View" ToolTip="View Quotation PDF" CommandName="RowView" CommandArgument='<%# Eval("Quotation_no") %>'><i class="fas fa-file-pdf"  style="font-size: 26px; color:red; "></i></i></asp:LinkButton>
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
                        <%--Sorted Grid End--%>


                        <%--Export to excel grid start--%>
                        <asp:GridView ID="ExportGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="Quotation_no" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="15">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20" HeaderText="View" Visible="false">
                                    <ItemTemplate>
                                        <img alt="" style="cursor: pointer" src="img/plus.png" />
                                        <asp:Panel ID="Panel2" runat="server" Style="display: none">
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <%--<asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>--%>
                                                    <%--<asp:BoundField ItemStyle-Width="150px" DataField="Id" HeaderText="Sr. No." />--%>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="Jobno" HeaderText="Job No." />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--///////--%>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label16" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="JOB No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Quo.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("Quotation_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="Label20" runat="server" Text='<%# Eval("SubCustomer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="kind Att.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("kind_Att") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amt.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label22" runat="server" Text='<%# Eval("AllTotal_price") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quatation Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Quotation_Date", "{0:dd-MM-yyyy}") %>' runat="server" ID="Label23" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("ExpiryDate", "{0:dd-MM-yyyy}") %>' runat="server" ID="Label24" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %>' runat="server" ID="Label25" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="Label26" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Count Days">
                                    <ItemTemplate>
                                        <asp:Label ID="Label27" runat="server" Text='<%# Eval("days") %>'></asp:Label>
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
                        <%--Export to excel grid end--%>

                        <%-- New Code for PopUP --%>

                        <asp:Button ID="btnprof" runat="server" Style="display: none" />
                        <asp:ModalPopupExtender ID="modelprofile" runat="server" TargetControlID="btnprof"
                            PopupControlID="PopupAddDetail" OkControlID="Closepopdetail" BackgroundCssClass="modalBackground" />

                        <asp:Panel ID="PopupAddDetail" runat="server" class="w3-panel w3-white panelinward m-0 font-weight-bold text-primary"
                            Direction="LeftToRight" Wrap="true" Style="display: none;">

                            <div class="d-flex justify-content-between align-items-center">

                                <h5 class="m-0 font-weight-bold text-primary">Pending Job Numbers</h5>


                                <asp:LinkButton ID="Closepopdetail" runat="server" class="btn-close" OnClientClick="ReloadPage()">
                                    <i class="fa fa-close" style="font-size:24px;color:red;"></i>
                                </asp:LinkButton>
                            </div>
                            <br />
                            <div class="row">
                                <div class="table-container" style="height: 300px; overflow-y: auto;">
                                    <asp:GridView ID="gv_EstimationList" runat="server" AutoGenerateColumns="False"
                                        CellPadding="3" CssClass="custom-grid" AllowPaging="false" OnRowDataBound="gv_EstimationList_RowDataBound"
                                        HeaderStyle-HorizontalAlign="Center" OnRowCommand="gv_EstimationList_RowCommand" RowStyle-HorizontalAlign="Center">

                                        <Columns>
                                            <%--New Code--%>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-Width="20">
                                                <ItemTemplate>
                                                    <img alt="" style="cursor: pointer" src="img/plus.png" />
                                                    <asp:Panel ID="PenJobs" runat="server" Style="display: none">
                                                        <asp:GridView ID="penJobDetails" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" OnClick="toggleJobSelection(this);" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Job No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblJobNo" HeaderText="Job No." runat="server" Text='<%# Eval("JobNo") %>'
                                                                            data-jobno='<%# Eval("JobNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField ItemStyle-Width="109px" DataField="CreatedDate" HeaderText="Created Date"
                                                                    SortExpression="CreatedDate" DataFormatString="{0:dd-MM-yyyy}" />
                                                                <asp:TemplateField HeaderText="Job Days Count">
                                                                    <ItemTemplate>
                                                                        <asp:Label Style="justify-content: center; display: flex" ID="lblDaysSinceCreated" runat="server"
                                                                            Text='<%# Eval("DaysSinceCreated") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <asp:Panel ID="CreatedDate" runat="server" Style="display: none">
                                                        <asp:GridView ID="CreatedDateDetails" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                            <Columns>
                                                                <asp:BoundField ItemStyle-Width="150px" DataField="JobNo" HeaderText="Job No." />
                                                                <asp:BoundField ItemStyle-Width="150px" DataField="CreatedDate" HeaderText="Created Date"
                                                                    SortExpression="CreatedDate" DataFormatString="{0:dd-MM-yyyy}" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>




                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label28" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <HeaderStyle Width="491px" />
                                                <ItemStyle Width="491px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Count">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label29" runat="server" Text='<%# Eval("JobCount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnCorrect" ToolTip="Correct"
                                                        CommandArgument='<%# Eval("CustName") %>' CommandName="RowCorrect"
                                                        CausesValidation="False" OnClick="lnkbtnCorrect_Click">
                                                        <i class="fa fa-check" style="font-size:24px; color:green"></i>
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                       <asp:LinkButton runat="server" ID="lnkbtnClose"
                                                           ToolTip="Close" OnClientClick="return validateJobSelection();" CommandArgument='0'
                                                           CommandName="RowClose" CausesValidation="False">
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
        </div>
    </form>
</asp:Content>
