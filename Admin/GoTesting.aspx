<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="GoTesting.aspx.cs" Inherits="Reception_PoPupTesting" %>

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
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Testing Product</h5>
                </div>

                <div class="card-body">
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label ID="lbltagno" runat="server" class="control-label col-sm-6">Tag No :</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txttagno" ReadOnly="true" />

                            <br />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblproductname" runat="server" class="control-label col-sm-6">Product Name :</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtproductname" ReadOnly="true" />

                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblTestingDate" runat="server" class="control-label col-sm-6">Testing Date :</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txttestingdate" />

                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblEngiName" runat="server" class="control-label col-sm-6">Engineer Name :</asp:Label>
                            <%--<asp:TextBox runat="server" class="form-control" ID="txtEngiName" />--%>

                            <asp:DropDownList ID="txtEngiName" runat="server" class="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblCompList" runat="server" class="control-label col-sm-6 lblcomp">Component List :</asp:Label>
                            <asp:TextBox runat="server" class="form-control txtsear mt-top" ID="txtcomponent" name="Search" placeholder="Search Component" onkeypress="return character(event)" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetComponetList" TargetControlID="txtcomponent" runat="server">
                            </asp:AutoCompleteExtender>
                            <%-- <asp:DropDownList ID="DropDownListComp" runat="server" class="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Select Component"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblQuantityComp" runat="server" class="control-label col-sm-6 lblcomp">Quantity :</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtQuantityComp" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please fill Quantity" ControlToValidate="txtQuantityComp" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:LinkButton ID="lnkbtnAdd" runat="server" Font-Bold="true" OnClick="lnkbtnAdd_Click" CommandArgument='<%# Eval("JobNo") %>'>+ADD</asp:LinkButton>
                        </div>
                        <br />
                        <br />

                    </div>

                    <br />
                    <div class="col-md-6">
                        <asp:Label ID="lblreportedto" runat="server" class="control-label col-sm-6">Reported To :</asp:Label>

                        <asp:TextBox runat="server" class="form-control" ID="txtreportedto" />
                    </div>
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
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="lblStatus" runat="server" class="control-label col-sm-6">Status:</asp:Label>
                            <%--          <asp:TextBox runat="server" class="form-control" ID="txtStatus" />--%>
                            <asp:DropDownList runat="server" class="form-control" ID="txtStatus">
                                <asp:ListItem Value="Repaired" Text="Repaired"></asp:ListItem>
                                <asp:ListItem Value="Not Repaired" Text="Not Repaired"></asp:ListItem>
                                <asp:ListItem Value="Tested" Text="Tested"></asp:ListItem>
                            </asp:DropDownList>

                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblRemark" runat="server" class="control-label col-sm-6">Remark:</asp:Label>
                            <asp:TextBox runat="server" class="form-control" ID="txtRemark" />

                            <br />
                        </div>
                    </div>

                    <center>   <div class="col-md-6">  
              <asp:Button  ID="btnSubmit" runat="server" class="btn btn-primary col-sm-3 " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button  ID="btnCancel" runat="server" class="btn btn-primary col-sm-3 " Text="Cancel"  CausesValidation="False"  OnClick="btnCancel_Click" ></asp:Button>
                 <asp:HiddenField runat="server" ID="hidden" /> 
            </div></center>

                </div>

            </div>



        </div>
    </form>
</asp:Content>

