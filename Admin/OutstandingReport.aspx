<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="OutstandingReport.aspx.cs" Inherits="Admin_OutstandingReport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <style type="text/css">
        .header {
            width: 100%;
            margin: 0 auto;
            height: 10px;
        }

        .header {
            background-color: #0755a1; /*#646464;*/
            font-family: Arial;
            color: White;
            border: none 0px transparent;
            height: 25px;
            text-align: center;
            font-size: 16px;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 14px;
            color: #000;
            min-height: 25px;
            text-align: left;
            border: none 0px transparent;
        }

        .btn {
            font-weight: 600;
            font-size: 12px;
            width: 150px;
        }

        h4 {
            font-size: 18px;
            font-family: verdana;
            line-height: 25px;
        }

        .gvhead {
            text-align: center;
            color: #d44805;
        }

        .hrstyle {
            background-color: #af8508;
        }

        /*.mydatagrid a:hover {
            background-color: #000;
            color: #fff;
        }*/

        .mydatagrid span {
            color: #000;
            padding: 0px 0px 0px 0px;
        }


        .mydatagrid td {
            padding: 0px;
            font-size: 13px;
        }

        .mydatagrid th {
            padding: 0px;
            font-size: 20px;
            background-color: #e6c150;
        }

        .BackgroundColor th {
            background-color: #1a2c2e !important;
        }

        .mydatagrid tr:hover {
            background-color: #e8d086;
        }

        .mybtn {
            padding: 4px;
            font-size: 14px;
            width: 60%;
        }

        .abc {
            padding-left: 4%;
        }

        .btn1 {
            background-color: #ffc107;
        }
    </style>

    <style type="text/css">
        .cal_Theme1 .ajax__calendar_container {
            background-color: #DEF1F4;
            border: solid 1px #77D5F7;
        }

        .adv {
            margin-top: 9px;
        }

        .cal_Theme1 .ajax__calendar_header {
            background-color: #ffffff;
            margin-bottom: 4px;
        }

        .cal_Theme1 .ajax__calendar_title,
        .cal_Theme1 .ajax__calendar_next,
        .cal_Theme1 .ajax__calendar_prev {
            color: #004080;
            padding-top: 3px;
        }

        .cal_Theme1 .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px #77D5F7;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            color: #004080;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            text-align: center;
        }

        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_day,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_month,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_year,
        .cal_Theme1 .ajax__calendar_active {
            color: #004080;
            font-weight: bold;
            background-color: #DEF1F4;
        }

        .cal_Theme1 .ajax__calendar_today {
            font-weight: bold;
            font-size: 10px;
        }

        .cal_Theme1 .ajax__calendar_other,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_today,
        .cal_Theme1 .ajax_calendar_hover .ajax_calendar_title {
            color: #bbbbbb;
        }

        .ajax__calendar_body {
            height: 158px !important;
            width: 220px !important;
            position: relative;
            overflow: hidden;
            margin: 0 0 0 -5px !important;
        }

        .ajax__calendar_container {
            padding: 4px;
            cursor: default;
            width: 220px !important;
            font-size: 11px;
            text-align: center;
            font-family: tahoma,verdana,helvetica;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            font-size: 14px;
            text-align: center;
        }

        .ajax__calendar_day {
            height: 22px !important;
            width: 27px !important;
            text-align: right;
            padding: 0 14px !important;
            cursor: pointer;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            margin-left: 12px !important;
            color: #004080;
        }

        .ajax__calendar_year {
            height: 50px !important;
            width: 51px !important;
            font-weight: bold;
            text-align: center;
            cursor: pointer;
            overflow: hidden;
            color: #004080;
        }

        .ajax__calendar_month {
            height: 50px !important;
            width: 51px !important;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
            overflow: hidden;
            color: #004080;
        }

        .grid tr:hover {
            background-color: #d4f0fa;
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
        }

        .listItem {
            color: #191919;
        }

        .uppercase {
            text-transform: uppercase;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
            font-weight: 600;
        }

        .pcoded[theme-layout="vertical"][vertical-nav-type="expanded"] .pcoded-header .pcoded-left-header, .pcoded[theme-layout="vertical"][vertical-nav-type="expanded"] .pcoded-navbar {
            width: 210px;
        }
    </style>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="../Scripts/jquery-ui-1.11.4.js"></script>

    <script type="text/javascript">
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: "dd-mm-yy",
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <style type="text/css">
        .img1 {
            vertical-align: unset !important;
            border-style: none;
        }

            .img1:hover {
                opacity: 0.75;
            }
    </style>
    <style>
        .error-message {
            color: red;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container abc">
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="header">
                        <h4>OUTSTANDING REPORT</h4>
                    </div>
                    <br />
                    <br />


                    <div class="row">
                        <div class="col-2">
                            <asp:Label ID="Label1" runat="server" Text="TYPE :"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txtoutstandingtype" Text="SALE" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4">
                            <asp:Label ID="Label2" runat="server" Text="CUSTOMER NAME :"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txtcustomername" runat="server" CssClass="form-control uppercase" OnTextChanged="txtcustomername_TextChanged" ValidationGroup="CustomerValidation"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtcustomername" ValidationGroup="CustomerValidation" ErrorMessage="Customer name is required" CssClass="error-message"></asp:RequiredFieldValidator>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtendersale" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerlist" TargetControlID="txtcustomername" runat="server">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>
                        <div class="col-3">
                            <asp:Label ID="Label3" runat="server" Text="FROM DATE :"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txtFromDt" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" CssClass="cal_Theme1" TargetControlID="txtFromDt" Format="dd-MM-yyyy" runat="server"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-3">
                            <asp:Label ID="Label4" runat="server" Text="TO DATE :"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txttoDt" AutoPostBack="true" runat="server" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" CssClass="cal_Theme1" TargetControlID="txttoDt" Format="dd-MM-yyyy" runat="server"></asp:CalendarExtender>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-12">
                        <div class="row">
                            <asp:LinkButton ID="btnsearch" OnClick="btnsearch_Click" runat="server" CssClass="btn btn-info btn-block btn-lg" Text="Search" ValidationGroup="CustomerValidation" />&nbsp;&nbsp
                            <asp:LinkButton ID="btnDownloadExcel" OnClick="btnDownloadExcel_Click1" runat="server" Width="169px" CssClass="btn btn-success" Visible="true" CausesValidation="False"><i class="fa"> &#xf1c3;</i> &nbsp;Export To Excel</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="btnDownloadpdf" OnClick="btnDownloadpdf_Click" runat="server" Width="169px" CssClass="btn btn-danger" Visible="false"><i class="fa"> &#xf1c1;</i> &nbsp;Export To PDF</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                           <asp:LinkButton ID="btnrefresh" OnClick="btnrefresh_Click" runat="server" Width="45px" CssClass="btn btn-info" Visible="false"><i class="fa"> &#xf021;</i> &nbsp;</asp:LinkButton>
                            <%--<asp:LinkButton ID="btnsearch" OnClick="btnsearch_Click" runat="server" CssClass="btn btn-info btn-block btn-lg" Text="Search" />--%>
                        </div>
                </div>
                <br />
                <div id="iframe" runat="server" visible="false">
                    <iframe id="ifrRight6" runat="server" enableviewstate="false" scrolling="auto" style="width: 100%; -ms-zoom: 0.75; height: 685px;"></iframe>
                </div>

                <%--   Gridview bind --%>
                <div id="DvGrid" runat="server">

                    <asp:GridView ID="GVOutstandingRpt" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                        PageSize="10" AllowPaging="true" PagerStyle-CssClass="paging">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice No.">

                                <ItemTemplate>
                                    <asp:Label ID="lblinvoiceno" runat="server" Text='<%# Eval("INVOICENO") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="98px">
                                <ItemTemplate>
                                    <asp:Label ID="lblinvoicedate" runat="server" Text='<%# Eval("INVOICEDATE","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payable Amt">
                                <ItemTemplate>
                                    <asp:Label ID="lblpaybleamt" runat="server" Text='<%# Eval("PayableAmt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recived Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblrecivedamt" runat="server" Text='<%# Eval("RecivedAmt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pending Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblpendingamt" runat="server" Text='<%# Eval("PendingAmt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Days">
                                <ItemTemplate>
                                    <asp:Label ID="lbldays" runat="server" Text='<%# Eval("Days") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
                </div>
                <%--   Gridview bind --%>

                </div>


             <div id="dvexcell" runat="server" visible="false">

                    <asp:GridView ID="GvExcell" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" >
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice No.">

                                <ItemTemplate>
                                    <asp:Label ID="lblinvoiceno" runat="server" Text='<%# Eval("INVOICENO") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="98px">
                                <ItemTemplate>
                                    <asp:Label ID="lblinvoicedate" runat="server" Text='<%# Eval("INVOICEDATE","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payable Amt">
                                <ItemTemplate>
                                    <asp:Label ID="lblpaybleamt" runat="server" Text='<%# Eval("PayableAmt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recived Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblrecivedamt" runat="server" Text='<%# Eval("RecivedAmt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pending Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblpendingamt" runat="server" Text='<%# Eval("PendingAmt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Days">
                                <ItemTemplate>
                                    <asp:Label ID="lbldays" runat="server" Text='<%# Eval("Days") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>


            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID ="btnDownloadExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>

