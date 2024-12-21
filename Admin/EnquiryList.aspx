<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="EnquiryList.aspx.cs" Inherits="Admin_EnquirPage_EnquiryPage" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
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

        .panelinward {
            border: 1px solid darkgray;
            width: 70%;
            padding: 20px !important;
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

        .lnksearch {
        }

        .btnlnkgrid {
            width: 150px;
        }
    </style>
    <script>
        // Nikhil Taks of Enquiry Master
        function HideLabel(msg, flg, url) {
            Swal.fire({
                icon: 'success',
                html: `<div style="display: flex; align-items: center; color: red;">
                            <span style="font-size: 24px; margin-right: 10px; color: red;">&#9888;</span>
                            <span style="color: red;">${msg}</span>
                        </div>`,
                timer: 5000,
                showCancelButton: false,
                showConfirmButton: true
            }).then(function () {
                if (url) {
                    window.location.href = url;
                } else if (flg == '1') {
                    window.location.href = "../ Admin / EnquiryList.aspx";
                }
            });
        }

        //function HideLabel(msg) {
        //    Swal.fire({
        //        icon: 'success',
        //        text: msg,
        //        timer: 3000,
        //        showCancelButton: false,
        //        showConfirmButton: false
        //    }).then(function () {
        //        window.location.href = "../Admin/EnquiryList.aspx";
        //    })
        //};
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
                    <h5 class="m-0 font-weight-bold text-primary">Enquiry List</h5>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <asp:TextBox runat="server" class="form-control txtsear" ID="txtSearch" name="Search" placeholder="Customer Name" onkeypress="return character(event)" />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList" TargetControlID="txtSearch" runat="server">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="lnkBtnsearch" runat="server" CssClass="btn btn-primary lnksearch " OnClick="lnkBtnsearch_Click"><i class="fa fa-search" style="font-size:24px"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkrefresh" runat="server" CssClass="btn btn-primary lnksearch lnkrefreshtab" OnClick="lnkrefresh_Click"><i class="fa fa-refresh" style="font-size:24px"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-3 col-xs-7 col-7">
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" class="form-control active2 " Width="150px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <%--<asp:ListItem Value="All" Text="All"></asp:ListItem>--%>
                            <asp:ListItem Value="1" Text="Pending"></asp:ListItem>
                            <asp:ListItem Value="0">Completed</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 col-xs-5 col-5">
                        <asp:Button ID="btncreate" runat="server" class="btn btn-primary btncreate" Text="Create" OnClick="btncreate_Click"></asp:Button>
                    </div>

                </div>


                <div style="width: 100%; padding: 20px;">
                    <div class="table-responsive">
                        <asp:GridView ID="gv_Customer" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            OnRowCommand="gv_Customer_RowCommand" OnPageIndexChanging="gv_Customer_PageIndexChanging" OnRowDataBound="gv_Customer_RowDataBound" PageSize="15" AllowPaging="true" PagerStyle-CssClass="paging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("Createddate")).ToString("dd/MM/yyyy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name/ Product Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        <br />
                                        <br />
                                        <label id="lblHeading"><b>Product Name</b></label>
                                        <br />
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProdName") %>' BackColor="Yellow"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Area">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArea" runat="server" Text='<%# Eval("Area") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="City">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCity" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Email ID">
                                    <ItemTemplate>
                                        <asp:Label ID="txttemail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Mobile No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobNo" runat="server" Text='<%# Eval("MobNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Info">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkshow" runat="server" CommandArgument='<%#Eval("EnquiryId")%>' OnClick="lnkshow_Click"><i class="fa fa-info-circle" style="font-size:30px;color:#0755A1"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="btnlnkgrid">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEdit" ToolTip="Edit" CommandArgument='<%# Eval("EnquiryId") %>' CommandName="RowEdit"><i class="fa fa-edit" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("EnquiryId") %>' CommandName="RowDelete"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
                                        &nbsp;&nbsp;  
                                        <asp:LinkButton runat="server" ID="LinkButton1" Visible='<%# Eval("IsStatus").ToString() == "Open" ? false : true %>' CommandName="CreateJobCard" CommandArgument='<%# Eval("EnquiryId") %>' ToolTip="Create JobCard"><i class="fa fa-arrow-circle-right" style="font-size:24px;color:green;"></i></asp:LinkButton>
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

                        <asp:Button ID="btnprof" runat="server" Style="display: none" />
                        <asp:ModalPopupExtender ID="modelprofile" runat="server" TargetControlID="btnprof"
                            PopupControlID="PopupAddDetail" OkControlID="Closepopdetail" />
                        <asp:Panel ID="PopupAddDetail" runat="server" class="w3-panel w3-white panelinward" GroupingText="Product Details" Direction="LeftToRight" Wrap="true">
                            <div class="row btnclose">
                                <div class="col-md-11">
                                </div>
                                <div class="col-md-1">
                                    <asp:LinkButton ID="Closepopdetail" runat="server"><i class="fa fa-close" style="font-size:24px;color:red;"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div class="row">
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label ID="lblJobNo" runat="server" class="control-label lbl">Job No:</asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblJNum" runat="server" class="control-label "></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblStat" runat="server" class="control-label lbl">Status :</asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblStatus" runat="server" class="control-label "></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIscomp" runat="server" class="control-label lbl">Completion :</asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIsCompleted" runat="server" class="control-label "></asp:Label>
                                </div>
                            </div>
                            <br ID ="br1" runat="server" Visible="false"/>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblproName" runat="server" class="control-label lbl">Product Name :</asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblproductNa" runat="server" class="control-label "></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblServTy" runat="server" class="control-label lbl">Service Type :</asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblServiceType" runat="server" class="control-label "></asp:Label>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="row">
                                <div class="col-md-3 ">
                                    <asp:Label ID="lblOtInfo" runat="server" class="control-label  lbl ">Other Information :</asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblOtherInfo" runat="server" class="control-label "></asp:Label>
                                </div>

                                <div class="col-md-3">
                                    <asp:Label ID="lblProdImg" runat="server" class="control-label lbl">Product Image : </asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Image ID="lblProductImg" runat="server" ImageUrl='<%# Eval("ProductImage") %>' Width="190px" Height="110px" Style="border: 1px solid #acacac;" />
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
