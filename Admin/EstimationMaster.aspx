<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="EstimationMaster.aspx.cs" Inherits="Admin_EstimationMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <!-- Boostrap DatePciker JS  -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src='https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/jquery-ui.min.js' type='text/javascript'></script>
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/EstimationList.aspx";
            })
        };
    </script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <style>
        .Cust {
            width: 408px;
            margin-left: 11px;
        }

        .lnk {
            font-weight: bolder;
            font-size: large;
        }

        .spncls {
            color: red;
        }

        .cls {
            width: 400px;
            margin-left: -3px;
        }

        .bold-text {
            font-weight: bold;
        }

        .custlbl {
            margin-left: 11px;
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
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js" type="text/javascript"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $(".status").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'EstimationMaster.aspx/GetstatusList',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[0],
                                    val: item.split('-')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $(this).parent().find("input[type=hidden]").val(i.item.val);
                },
                minLength: 1
            }).focus(function () {
                $(this).autocomplete("search");
            });
        });
    </script>

    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script>
        function allowAlphabets(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode <= 90 && charCode >= 65) || (charCode <= 122 && charCode >= 97 || charCode == 8)) {

                return true;
            }
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">

        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Estimation</h5>
                </div>

                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblJobNo" runat="server" class="control-label col-sm-6">Job No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtJobNo" AutoPostBack="true" OnTextChanged="txtJobNo_TextChanged" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Job No." ControlToValidate="txtJobNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetjobList" TargetControlID="txtJobNo" runat="server">
                            </asp:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6" runat="server" id="divCustName">            
                            <asp:Label ID="lblCustName" runat="server" class="control-label col-sm-6">Customer Name:<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtcustname" ReadOnly="true" onkeypress="return character(event)" />
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="Please fill Customer Name" ControlToValidate="txtcustname" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>

                              <div class="col-md-6">
                            <asp:Label ID="Label1" runat="server" class="control-label col-sm-6">Product Name :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtproduct"/>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label2" runat="server" class="control-label col-sm-6">Model No. :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtmodelno"/>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblFinalstatus" runat="server" class="control-label col-sm-6">Final Status :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtFinalStatus" onkeypress="return character(event)" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblcopmstatus" runat="server" class="control-label col-sm-6">Component Status:<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtcompstatus" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblcomponentcost" runat="server" class="control-label col-sm-6">Component Cost :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" onkeypress="return isNumberKey(event)" AutoPostBack="true" OnTextChanged="txtcomponentcost_TextChanged" class="form-control" ID="txtcomponentcost" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblsitevisitchargesr" runat="server" class="control-label col-sm-6">Site Visit Charges :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control"  AutoPostBack="true" OnTextChanged="txtsitevisitcharges_TextChanged" onkeypress="return isNumberKey(event)" ID="txtsitevisitcharges" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblothercharges" runat="server" class="control-label col-sm-6">Other Charges :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" OnTextChanged="txtothercharges_TextChanged" AutoPostBack="true" class="form-control" onkeypress="return isNumberKey(event)" ID="txtothercharges" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblFinalcost" runat="server" class="control-label col-sm-6">Final Cost :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" onkeypress="return isNumberKey(event)" class="form-control" ID="txtfinalcost" />
                            <br />
                        </div>

                        <div class="col-md-6" runat="server" id="divtxtestimatedquo">
                            <asp:Label ID="lblestimatedquo" runat="server" class="control-label col-sm-6">Quotation Amount :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtestimatedquo" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblDateIn" runat="server" class="control-label col-sm-6" for="cust">Component Received Date :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control myDate cls" ID="txtcomprecdate" AutoPostBack="True" TextMode="Date" />&nbsp;&nbsp;   
                             <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="Please fill Component Received Date" ControlToValidate="txtcomprecdate" ForeColor="Red"></asp:RequiredFieldValidator> --%>
                            <br />
                        </div>

                    </div>
                    <div style="width: 100%;">
                        <asp:GridView ID="gv_estimation" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            PageSize="30" AllowPaging="true" PagerStyle-CssClass="paging" OnRowDataBound="gv_estimation_RowDataBound" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Component Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btntotal" runat="server" class="btn btn-primary" Text="Total" OnClick="btntotal_Click" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblqty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblqtytotal" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor1header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor1" runat="server" CssClass="form-control" OnTextChanged="txtvendor1_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal1" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor2header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor2" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal2" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor3header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor3" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal3" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor4header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor4" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal4" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor5header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor5" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal5" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetstatusList" TargetControlID="txtstatus" runat="server">
                                        </asp:AutoCompleteExtender>
                                    </ItemTemplate>

                                </asp:TemplateField>

                            </Columns>


                            <FooterStyle BackColor="White" ForeColor="#000066" HorizontalAlign="left" />
                            <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="center" />
                            <RowStyle ForeColor="#000066" HorizontalAlign="left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    </div>
                    <div style="width: 100%;">
                        <asp:GridView ID="gv_Updateestimat" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            PageSize="30" AllowPaging="true" PagerStyle-CssClass="paging" OnRowDataBound="gv_Updateestimat_RowDataBound1" ShowFooter="true" UseAccessibleHeader="true">
                            <Columns>
                                <asp:TemplateField HeaderText="SrNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Component Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btntotal" runat="server" class="btn btn-primary" Text="Total" OnClick="btntotal_Click1" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblqty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblqtytotal" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor1header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor1" runat="server" CssClass="form-control"
                                            Text='<%# Eval("vendor1Rate") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal1" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor2header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor2" runat="server" CssClass="form-control" Text='<%# Eval("vendor2Rate") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal2" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor3header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor3" runat="server" CssClass="form-control" Text='<%# Eval("vendor3Rate") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal3" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor4header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor4" runat="server" CssClass="form-control" Text='<%# Eval("vendor4Rate") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal4" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:DropDownList runat="server" ID="txtvendor5header" CssClass="form-control" AppendDataBoundItems="false">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvendor5" runat="server" CssClass="form-control" Text='<%# Eval("vendor5Rate") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblvendortotal5" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" AutoPostBack="true" Text='<%# Eval("status") %>'></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                            CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                            CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetstatusList" TargetControlID="txtstatus" runat="server">
                                        </asp:AutoCompleteExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" HorizontalAlign="left" />
                            <HeaderStyle BackColor="#0755a1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="left" />
                            <RowStyle ForeColor="#000066" HorizontalAlign="left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                        <%-- <div class="col-md-6" id="totalcomcost">
                            <asp:Label ID="lbl" runat="server" Text="Total Cost of Component"></asp:Label>
                            <asp:TextBox ID="txtcostofcomponet" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                        </div>--%>
                    </div>
                    <br />
                    <div class="row">

                        <div class="col-9">
                        </div>

                        <br />
                        <%--<asp:Label ID="lblTotal" class="spncls" runat="server"></asp:Label>--%>
                       <asp:Label ID="lblTotal" CssClass="bold-text spncls" runat="server"></asp:Label>
                        <br />
                    </div>


                    <asp:HiddenField ID="header1" runat="server" />
                    <asp:HiddenField ID="header2" runat="server" />
                    <asp:HiddenField ID="header3" runat="server" />
                    <asp:HiddenField ID="header4" runat="server" />
                    <asp:HiddenField ID="header5" runat="server" />
                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " Text="Cancel"  CausesValidation="False"  OnClick="btnCancel_Click"></asp:Button>

            </div></center>
                </div>
                <asp:HiddenField runat="server" ID="hidden" />
            </div>
        </div>
    </form>
</asp:Content>

