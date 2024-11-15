<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="UAuthorization.aspx.cs" Inherits="Admin_UAuthorization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="http://code.jquery.com/ui/1.11.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .reqcls {
            color: red;
            font-weight: 600;
            font-size: 14px;
        }

        .bg-primary {
            background-color: #0755a1 !important;
        }


        .lbl {
            font-weight: 600;
        }

        .imagepic {
            width: 50px;
            height: 150px;
            border-radius: 60%;
        }

        .grid {
            width: 98% !important;
            margin: 0 auto;
        }

            .grid td {
                padding: 1px !important;
                text-align: center !important;
                font-size: 12px;
                font-weight: 600;
            }

            .grid th {
                background: #868e96 !important;
                height: 30px;
                color: #fff;
            }

        .errspan {
            float: right;
            margin-right: 6px;
            margin-top: -25px;
            position: relative;
            z-index: 2;
            color: black;
        }

        #Role input[type="checkbox"] {
            margin-right: 10px;
        }

        .GridPager a, .GridPager span {
            display: block;
            height: 25px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }

        .ma {
            margin-left: 29px;
        }

        .lbluser {
            margin-left: 27px;
        }
    </style>
    <style>
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
                window.location.href = "../Admin/UserAuthoration.aspx";
            })
        };
    </script>
    <script>
        function selectAll(checked) {

            var isChecked = $("#<%= gvUserAuthorization.ClientID %> input[id*='chkselectall']").prop('checked') ? true : false;
            if (isChecked) {
                $('input:checkbox[name$=chkCreate] tr').each(
                    function () {
                        $(this).attr('checked', 'checked');
                    });
            }
            else {
                //$('input:checkbox[name$=chkEmployee]').each(
                //    function () {
                //        $(this).removeAttr('checked');
                //    });
            }
        }
    </script>



    <script language="javascript" type="text/javascript">
        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById('DivHeaderRow');
                var DivMC = document.getElementById('DivMainContent');
                var DivFR = document.getElementById('DivFooterRow');

                //*** Set divheaderRow Properties ****
                DivHR.style.height = headerHeight + 'px';
                DivHR.style.width = (parseInt(width) - 16) + 'px';
                DivHR.style.position = 'relative';
                DivHR.style.top = '0px';
                DivHR.style.zIndex = '10';
                DivHR.style.verticalAlign = 'top';

                //*** Set divMainContent Properties ****
                DivMC.style.width = width + 'px';
                DivMC.style.height = height + 'px';
                DivMC.style.position = 'relative';
                DivMC.style.top = -headerHeight + 'px';
                DivMC.style.zIndex = '1';

                //*** Set divFooterRow Properties ****
                DivFR.style.width = (parseInt(width) - 16) + 'px';
                DivFR.style.position = 'relative';
                DivFR.style.top = -headerHeight + 'px';
                DivFR.style.verticalAlign = 'top';
                DivFR.style.paddingtop = '2px';

                //****Copy Header in divHeaderRow****
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }
        function OnScrollDiv(Scrollablediv) {
            document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
            document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form runat="server">

        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <%--//////////--%>
                <div class="page-wrapper">
                    <div class="page-body">
                        <div class="row">
                            <div class="col-md-7">
                                <div class="page-header-breadcrumb">
                                    <div style="float: left; font-size: 15px;">
                                        <span><i class="feather icon-home"></i>&nbsp; User Authorization</span>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="container py-3">
                            <div class="card">
                                <div class="card-header bg-primary text-uppercase text-white">
                                    <h5><i class="fa fa-user-plus"></i>User Authorization</h5>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-1">
                                        <asp:Label ID="Label1" runat="server" Text="Role :"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlrole" runat="server" AutoPostBack="true" class="form-control active1 " Width="170px" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlrole_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Label ID="Label2" runat="server" Text="User :"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddluser" runat="server" AutoPostBack="true" class="form-control active1" Width="170px" AppendDataBoundItems="true" OnSelectedIndexChanged="ddluser_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <%--user Authorization grid START--%>
                                <div id="GridDiv" runat="server">
                                    <div class="col-lg-12">
                                        <div class="mb-4">
                                            <div id="DivRoot">
                                                <asp:GridView ID="gvUserAuthorization" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No records found" DataKeyNames="ID"
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" HorizontalAlign="Center" BackColor="#0755a1" OnRowDataBound="gvUserAuthorization_RowDataBound" OnRowCommand="gvUserAuthorization_RowCommand" AllowPaging="false" PagerStyle-CssClass="paging" PageSize="10">
                                                    <AlternatingRowStyle BackColor="White" VerticalAlign="Middle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Width="50px" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="text-center" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Page Name" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMenuName" runat="server" Text='<%# Eval("MenuName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Menu Name" Visible="false" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("PageName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Menu ID" HeaderStyle-CssClass="text-center" Visible="false">

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMenuId" runat="server" Text='<%# Eval("MenuID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="text-center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Role" HeaderStyle-CssClass="text-center" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrole" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="text-center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkPages" readonly="true" AutoPostBack="true" runat="server" name="chk" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="text-center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="View" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkPagesView" readonly="true" AutoPostBack="true" runat="server" name="chk" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="text-center" />
                                                        </asp:TemplateField>
                                                    </Columns>

                                                    <EditRowStyle BackColor="#2461BF" HorizontalAlign="Center" />
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                                    <HeaderStyle BackColor="#212529" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#eaeaea " HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#0755a1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>
                                                <div id="DivFooterRow" style="overflow: hidden">
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-dark col-sm-3 " Text="Submit"  OnClick="btnSubmit_Click"  ></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-dark col-sm-3 " Text="Cancel"  CausesValidation="False" OnClick="btnCancel_Click" ></asp:Button>

            </div></center>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--//////////--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>

