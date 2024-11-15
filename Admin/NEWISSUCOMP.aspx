<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="NEWISSUCOMP.aspx.cs" Inherits="Reception_PoPupTesting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        .btncreate {
            float: right;
            margin-right: 27px;
        }

        .txtsear {
            margin-left: 22px;
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
    </style>
    <%--<script type="text/javascript">
        function openModal() {
            $('#exampleModalCenter').modal('show');
        }
    </script>--%>
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/IssuedComponentList.aspx";
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
        <%-- <div class="col-lg-12 card">--%>
        <div class="card shadow-sm mb-4">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Issued Component</h5>
                </div>
                <div class="card-body">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lblTestingDate" runat="server" class="control-label col-sm-6">Customer Name :</asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtcustomername" placeholder="Customer Name" OnTextChanged="txtcustomername_TextChanged" AutoPostBack="true" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorcustname" runat="server" ErrorMessage="Please Fill Customer Name" ControlToValidate="txtcustomername" ForeColor="Red" ValidationGroup="submitdetails"></asp:RequiredFieldValidator>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GeCustomerList" TargetControlID="txtcustomername" runat="server">
                                </asp:AutoCompleteExtender>
                                <%--       <br />--%>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lbljobno" runat="server" class="control-label col-sm-6" for="cust">Job No. :<span class="spncls"></span></asp:Label>
                                <%--    <asp:TextBox runat="server" class="form-control" ID="txttagno" OnTextChanged="txtjobno_TextChanged" AutoPostBack="true" name="Jobno" placeholder="Job No." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Job No." ControlToValidate="txttagno" ForeColor="Red" ValidationGroup="submitdetails"></asp:RequiredFieldValidator>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GeJobnoList" TargetControlID="txttagno" runat="server">
                                </asp:AutoCompleteExtender>--%>
                                <asp:DropDownList runat="server" class="form-control" ID="txttagno" AutoPostBack="true" OnSelectedIndexChanged="txttagno_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="Select Job No"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill Job No." ControlToValidate="txttagno" ForeColor="Red" ValidationGroup="submitdetails"></asp:RequiredFieldValidator>

                                <%--       <br />--%>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblproductname" runat="server" class="control-label col-sm-6">Product Name :</asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtproductname" placeholder="Product Name" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Fill Product Name" ControlToValidate="txtproductname" ForeColor="Red" ValidationGroup="submitdetails"></asp:RequiredFieldValidator>
                                <%--        <br />--%>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblEngiName" runat="server" class="control-label col-sm-6">Engineer Name :</asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtEngiName" placeholder="Engineer Name" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please  Fill Engineer Name" ControlToValidate="txtproductname" ForeColor="Red" ValidationGroup="submitdetails"></asp:RequiredFieldValidator>

                            </div>
                                 <div class="col-md-6">
                                <asp:Label ID="lblissueddate" runat="server" class="control-label col-sm-6">Issued Date :</asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtissudedate" TextMode="Date" placeholder="Issued Date" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please  Fill Issued Date" ControlToValidate="txtproductname" ForeColor="Red" ValidationGroup="submitdetails"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-8">
                                <asp:Label ID="lblCompList" runat="server" class="control-label col-sm-3 lblcomp">Component List :</asp:Label>
                                <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtcomponent" name="Search" placeholder="Select Component" onkeypress="return character(event)" />
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetComponetList" TargetControlID="txtcomponent" runat="server">
                                </asp:AutoCompleteExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Fill Component List" ControlToValidate="txtcomponent" ForeColor="Red" ValidationGroup="AddComQunatity"></asp:RequiredFieldValidator>
                            </div>
                            <%--        <br />--%>
                            <div class="col-md-2">
                                <asp:Label ID="lblQuantityComp" runat="server" class="control-label col-sm-2 lblcomp">Quantity :</asp:Label>
                                <asp:TextBox runat="server" class="form-control" ID="txtQuantityComp" placeholder="Quantity" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Quantity" ControlToValidate="txtQuantityComp" ForeColor="Red" ValidationGroup="AddComQunatity"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-2">
                                <%--        <br />--%>
                                <asp:LinkButton ID="lnkbtnAdd" runat="server" Font-Bold="true" OnClick="lnkbtnAdd_Click" CommandArgument='<%# Eval("JobNo") %>' ValidationGroup="AddComQunatity">+ADD</asp:LinkButton>
                            </div>
                            <br />
                            <br />
                        </div>

                    </div>
                    <br />
                    <br />
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_Comp" runat="server" AutoGenerateColumns="false" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="gv_Comp_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comp Id" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompid" runat="server" Text='<%# Eval("CompId") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompnamee" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>

                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>

                                            <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("CompId") %>' OnClick="lnkbtnDelete_Click"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                        <br />
                    </div>


                    <%-- <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="GVRepairing" runat="server" AutoGenerateColumns="false" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comp Id" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompid" runat="server" Text='<%# Eval("CompId") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompnamee" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>

                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>

                                            <asp:LinkButton runat="server" ID="lnkbtnDelete" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("CompId") %>' OnClick="lnkbtnDelete_Click"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                        <br />
                    </div>--%>


                    <div class="col-md-12" id="Divcompnew" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="Gv_CompNew" runat="server" AutoGenerateColumns="false" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comp Id" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblcompid" runat="server" Text='<%# Eval("CompId") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompnamee" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>

                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>

                                            <asp:LinkButton runat="server" ID="lnkbtnDeletenew" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%# Eval("CompId") %>' OnClick="lnkbtnDeletenew_Click"><i class="fa fa-trash-o" style="font-size:24px"></i></asp:LinkButton>
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
                        <br />
                    </div>


                    <div class="col-md-12" id="DvUpdatecomp" runat="server" visible="false">
                        <div class="table-responsive">
                            <asp:GridView ID="GVUpdatecomp" runat="server" AutoGenerateColumns="false" CellPadding="3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" OnRowCommand="GVUpdatecomp_RowCommand" DataKeyNames="CompId,JobNo,CompName,Quantity">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobno" runat="server" Text='<%# Eval("JobNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comp Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcompid" runat="server" Text='<%# Eval("CompId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompnamee" runat="server" Text='<%# Eval("CompName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkbtnDeleteUpcomp" ToolTip="Delete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')"  CommandArgument='<%# Container.DataItemIndex %>'  CommandName="Deletecomp"><i class="fa fa-trash-o" style="font-size:24px" ></i></asp:LinkButton>
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
                        <br />
                    </div>

                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btnSubmit_Click"  ValidationGroup="submitdetails" CausesValidation="true"></asp:Button>
          <%--      &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; --%>
               <asp:Button  ID="btnUpdate" runat="server" class="btn btn-primary col-sm-3 " Text="Update"  OnClick="btnUpdate_Click"   ValidationGroup="submitdetails" CausesValidation="true" Visible="false"></asp:Button>
<%--                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp;--%> 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " Text="Cancel"  CausesValidation="False"  OnClick="btnCancel_Click" ></asp:Button>
                 <asp:HiddenField runat="server" ID="hidden" /> 
            </div></center>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

