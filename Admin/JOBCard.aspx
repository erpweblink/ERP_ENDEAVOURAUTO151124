<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="JOBCard.aspx.cs" Inherits="Admin_JOBCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function HideLabel(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 3000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "../Admin/JOBcardList.aspx";
            })
        };
    </script>

    <%--New Script dropdown--%>
    <script type="text/javascript">
     
        <%--let selectedEngineers = [];

        function addEngineerToList() {
            const dropdown = document.getElementById('<%= txtengineername.ClientID %>');
            const selectedEngineer = dropdown.options[dropdown.selectedIndex].text;

            if (!selectedEngineers.includes(selectedEngineer) && selectedEngineer !== "Select Engineer") {
                selectedEngineers.push(selectedEngineer);
                updateEngineerDisplay();
                updateHiddenField();
            }
        }

        function updateEngineerDisplay() {
            const container = document.getElementById('<%= selectedEngineersContainer.ClientID %>');
            if (!container) return;

            container.innerHTML = ''; 

            selectedEngineers.forEach((engineer, index) => {
                const engineerElement = document.createElement('span');
                engineerElement.innerText = engineer;
                engineerElement.classList.add('badge', 'badge-primary', 'm-1', 'p-2');
                engineerElement.style.cursor = 'pointer';
                engineerElement.onclick = () => removeEngineer(index);

                const closeIcon = document.createElement('span');
                closeIcon.innerHTML = '&times;';
                closeIcon.style.marginLeft = '8px';
                closeIcon.style.cursor = 'pointer';

                engineerElement.appendChild(closeIcon);
                container.appendChild(engineerElement);
            });
        }

        function removeEngineer(index) {
            selectedEngineers.splice(index, 1); 
            updateEngineerDisplay();
            updateHiddenField();
        }

        function updateHiddenField() {
            const hiddenField = document.getElementById('<%= hiddenSelectedEngineers.ClientID %>');
            hiddenField.value = selectedEngineers.join(", ");
        }--%>

       
    let selectedEngineers = [];

    // Function to initialize saved engineers on page load
    window.onload = function() {
        const hiddenField = document.getElementById('<%= hiddenSelectedEngineers.ClientID %>');
        if (hiddenField.value) {
            selectedEngineers = hiddenField.value.split(", ");
            updateEngineerDisplay();  // Display saved engineers immediately on load
        }
    };

    function addEngineerToList() {
        const dropdown = document.getElementById('<%= txtengineername.ClientID %>');
        const selectedEngineer = dropdown.options[dropdown.selectedIndex].text;

        if (!selectedEngineers.includes(selectedEngineer) && selectedEngineer !== "Select Engineer") {
            selectedEngineers.push(selectedEngineer);
            updateEngineerDisplay();
            updateHiddenField();
        }
    }

    function updateEngineerDisplay() {
        const container = document.getElementById('<%= selectedEngineersContainer.ClientID %>');
        if (!container) return;

        container.innerHTML = ''; 

        selectedEngineers.forEach((engineer, index) => {
            const engineerElement = document.createElement('span');
            engineerElement.innerText = engineer;
            engineerElement.classList.add('badge', 'badge-primary', 'm-1', 'p-2');
            engineerElement.style.cursor = 'pointer';
            engineerElement.onclick = () => removeEngineer(index);

            const closeIcon = document.createElement('span');
            closeIcon.innerHTML = '&times;';
            closeIcon.style.marginLeft = '8px';
            closeIcon.style.cursor = 'pointer';

            engineerElement.appendChild(closeIcon);
            container.appendChild(engineerElement);
        });
    }

    function removeEngineer(index) {
        selectedEngineers.splice(index, 1); 
        updateEngineerDisplay();
        updateHiddenField();
    }

    function updateHiddenField() {
        const hiddenField = document.getElementById('<%= hiddenSelectedEngineers.ClientID %>');
        hiddenField.value = selectedEngineers.join(", ");
    }


         <%-- Accept Uppercase,Lowercase & Numbers --%>
        function character(event) {
            var charCode = event.which ? event.which : event.keyCode;

            if ((charCode >= 48 && charCode <= 57) || // Numbers 0-9
                (charCode >= 65 && charCode <= 90) || // Uppercase A-Z
                (charCode >= 97 && charCode <= 122) || // Lowercase a-z
                charCode === 32 || // Space
                charCode === 46) { // Dot (.)
                return true;
            }

            return false;
        }

        <%-- Automatic outward date show --%>
        window.onload = function () {
            const dateTextBox = document.getElementById('<%= txtoutwardate.ClientID %>');
            const today = new Date();
            const formattedDate = today.toISOString().split('T')[0]; // Format: DD-MM-YYYY
            dateTextBox.value = formattedDate;
        };
    </script>


    <style>
        .spncls {
            color: red;
        }
    </style>
    <!--char-->
    <script>
        //function character(e) {
        //    isIE = document.all ? 1 : 0
        //    keyEntry = !isIE ? e.which : event.keyCode;
        //    if (((keyEntry >= '65') && (keyEntry <= '90')) || ((keyEntry >= '97') && (keyEntry <= '122')) || (keyEntry == '46') || (keyEntry == '32') || keyEntry == '45')

        //        return true;

        //    else {
        //        return false;
        //    }
        //}
    </script>
    <style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
        <div class="col-lg-12" style="overflow: scroll;">
            <div class="card shadow-sm mb-4">
                <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h5 class="m-0 font-weight-bold text-primary">Job Card</h5>
                </div>
                <div class="card-body">

                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label runat="server" ID="jobno" Text="" class="control-label col-sm-6">Job Card No. :
                                <span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" ID="txtjobno" class="form-control" OnTextChanged="txtjobno_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please enter Job No." ControlToValidate="txtjobno" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetjobList" TargetControlID="txtjobno" runat="server">
                            </asp:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-3">
                            <asp:Label runat="server" ID="lblrepeatno" Text="" class="control-label col-sm-6">Repeated No. :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" ID="txtrepeatedNo" class="form-control"></asp:TextBox>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblitemdescription" Text="" class="control-label col-sm-6">Product Description :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" ID="txtitemdescription" class="form-control" onkeypress="return character(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter Item Description." ControlToValidate="txtitemdescription" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblmodelno" Text="" class="control-label col-sm-6">Model No. :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" ID="txtmodelno" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter Model No." ControlToValidate="txtmodelno" ForeColor="Red"></asp:RequiredFieldValidator><br />
                        </div>
                        <%--<div class="col-md-6">
                            <asp:Label runat="server" ID="lblengineername" Text="" class="control-label col-sm-6">Engineer Name :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" ID="txtengineername" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter Engineer Name" ControlToValidate="txtengineername" ForeColor="Red"></asp:RequiredFieldValidator><br />
                        </div>--%>

                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblinwarddate" Text="" class="control-label col-sm-6">Inward Date :<span class="spncls">*</span></asp:Label>
                            <asp:TextBox runat="server" ID="txtinwarddate" class="form-control" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter Inward Date." ControlToValidate="txtinwarddate" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                        <%-- <div class="col-md-6">
                            <asp:Label ID="lblEngiName" runat="server" class="control-label col-sm-6">Engineer Name 1 <span class="spncls">*</span>:</asp:Label>
                            <asp:DropDownList ID="txtengineername" runat="server" class="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please enter Engineer Name" ControlToValidate="txtengineername" ForeColor="Red"></asp:RequiredFieldValidator><br />
                            <br />
                        </div>--%>

                       

                         <%-- New Code  --%>
                        <%--<div class="col-md-6">
                            <asp:Label ID="lblEngiName" runat="server" class="control-label col-sm-6">
                                Engineer Name :<span class="spncls">*</span>:
                            </asp:Label>

                            <!-- Container for Displaying Selected Engineers -->
                            <div id="selectedEngineersContainer" runat="server" class="form-control"
                                style="height: auto; min-height: 38px; padding: 5px; border: 1px solid #ced4da; background-color: #e9ecef;">
                            </div>

                            <asp:HiddenField ID="hiddenSelectedEngineers" runat="server" />

                            <!-- Dropdown for Selecting Engineers -->
                            <asp:DropDownList ID="txtengineername" runat="server" class="form-control" AppendDataBoundItems="true" 
                                onchange="addEngineerToList()">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>                              
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please enter Engineer Name" ControlToValidate="txtengineername" ForeColor="Red">
                            </asp:RequiredFieldValidator><br />
                            <br />
                        </div>--%>

                        <div class="col-md-6">
                            <asp:Label ID="lblEngiName" runat="server" class="control-label col-sm-6">
                                Engineer Name :<span class="spncls">*</span>:
                            </asp:Label>

                            <!-- Container for Displaying Selected Engineers -->
                            <div id="selectedEngineersContainer" runat="server" class="form-control"
                                style="height: auto; min-height: 38px; padding: 5px; border: 1px solid #ced4da; background-color: #e9ecef;">
                            </div>

                            <asp:HiddenField ID="hiddenSelectedEngineers" runat="server" />

                            <!-- Dropdown for Selecting Engineers -->
                            <asp:DropDownList ID="txtengineername" runat="server" class="form-control" AppendDataBoundItems="true" 
                                onchange="addEngineerToList()">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>                              
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please enter Engineer Name" 
                                ControlToValidate="txtengineername" ForeColor="Red">
                            </asp:RequiredFieldValidator><br />
                            <br />
                        </div>



                        <%--<div class="col-md-6">
                            <asp:Label ID="Label1" runat="server" class="control-label col-sm-6">Engineer Name 2 :</asp:Label>
                            <asp:DropDownList ID="txtengineername2" runat="server" class="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label2" runat="server" class="control-label col-sm-6">Engineer Name 3 :</asp:Label>
                            <asp:DropDownList ID="txtengineername3" runat="server" class="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="Label3" runat="server" class="control-label col-sm-6">Engineer Name 4 :</asp:Label>
                            <asp:DropDownList ID="txtengineername4" runat="server" class="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Select Engineer"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>--%>


                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lbloutward" Text="" class="control-label col-sm-6">Job Assing Date :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" ID="txtoutwardate" class="form-control" TextMode="Date"></asp:TextBox>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblstatus" Text="" class="control-label col-sm-6">Status :<span class="spncls"></span></asp:Label>
                            <%--  <asp:TextBox runat="server" ID="txtstatus" class="form-control"></asp:TextBox>--%>
                            <asp:DropDownList runat="server" class="form-control" ID="txtstatus">
                                <asp:ListItem Value="Repaired" Text="Repaired"></asp:ListItem>
                                <asp:ListItem Value="Not Repaired" Text="Not Repaired"></asp:ListItem>
                                <asp:ListItem Value="Tested" Text="Tested"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblreparingdate" Text="" class="control-label col-sm-6">Reparing Date :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" ID="txtreparingdate" class="form-control" TextMode="Date"></asp:TextBox>
                            <br />
                        </div>

                        <%-- New added --%>
                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblmotortrial" Text="" class="control-label col-sm-6">Motor Trial Description :<%--<span class="spncls">*</span>--%></asp:Label>
                            <asp:TextBox runat="server" ID="txtmotortrial" class="form-control" onkeypress="return character(event)"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter Motor Trial Description." ControlToValidate="txtmotortrial" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblmotorrating" Text="" class="control-label col-sm-6">Motor Rating Description :<%--<span class="spncls">*</span>--%></asp:Label>
                            <asp:TextBox runat="server" ID="txtmotorrating" class="form-control" onkeypress="return character(event)"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please enter Motor Rating Description." ControlToValidate="txtmotorrating" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblmotorcurrent" Text="" class="control-label col-sm-6">Motor Current Description :<%--<span class="spncls">*</span>--%></asp:Label>
                            <asp:TextBox runat="server" ID="txtmotorcurrent" class="form-control" onkeypress="return character(event)"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please enter Motor Current Description." ControlToValidate="txtmotorcurrent" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lbltrialtimedate" Text="" class="control-label col-sm-6">Trial Date :<span class="spncls"></span></asp:Label>
                            <asp:TextBox runat="server" ID="txttrialtimedate" class="form-control" TextMode="Date"></asp:TextBox>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblkeypadtrail" class="control-label col-sm-6" runat="server">Keypad Trail :</asp:Label>
                            <asp:DropDownList ID="DropDownListkeypadtrail" runat="server" class="form-control" AutoPostBack="true">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblanologtrail" class="control-label col-sm-6" runat="server">Anolog(Terminal) Trail :</asp:Label>
                            <asp:DropDownList ID="DropDownListanologtrail" runat="server" class="form-control" AutoPostBack="true">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblfancleaning" class="control-label col-sm-6" runat="server">Drive Cleaning & Fan Cleaning :</asp:Label>
                            <asp:DropDownList ID="DropDownListfancleaning" runat="server" class="form-control" AutoPostBack="true">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblparameterorignal" class="control-label col-sm-6" runat="server">Parameter Orignal Setting :</asp:Label>
                            <asp:DropDownList ID="DropDownListparameterorignal" runat="server" class="form-control" AutoPostBack="true">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblpackingsop" class="control-label col-sm-6" runat="server">Drive Cleaned & Packing SOP :</asp:Label>
                            <asp:DropDownList ID="DropDownListpackingsop" runat="server" class="form-control" AutoPostBack="true">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>
                        <%-- New added end --%>



                        <div style="width: 100%; padding: 20px;">
                            <div class="table-responsive">
                                <asp:GridView ID="gvparticular" runat="server" CssClass="table table-striped table-bordered nowrap"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." ItemStyle-Width="68" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Particular">
                                            <ItemTemplate>
                                                <asp:Label ID="lblparticular1" runat="server" Text='<%# Eval("Particular") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="630" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtstatus1" TextMode="MultiLine" Width="630px" Height="34px" CssClass="form-control" Text='<%# Eval("Statusjob") %>'></asp:TextBox>
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
                    <center>
                        <h5 style="font-weight: bold">VFD/SERVO OBSERVATION/TEST POINTS</h5>
                    </center>
                    <div style="width: 100%; padding: 20px;">
                        <div class="table-responsive">
                            <asp:GridView ID="gv_jobcardgrid2" runat="server" CssClass="table table-striped table-bordered nowrap"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." ItemStyle-Width="68" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label ID="lblparticular2" runat="server" Text='<%# Eval("Particular") %>'></asp:Label>
                                            <%-- <%# Container.DataItem %>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="630" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtstatus2" Height="34px" TextMode="MultiLine" Width="630px" CssClass="form-control" Text='<%# Eval("Status") %>'></asp:TextBox>
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

                <center>
                    <div class="col-md-6">
                        <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary  " Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                        &nbsp;&nbsp;        &nbsp;&nbsp;     &nbsp;&nbsp;     &nbsp;&nbsp; 
             <asp:Button ID="btnCancel" runat="server" class="btn btn-primary " Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click"></asp:Button>

                    </div>
                </center>
            </div>
            <asp:HiddenField runat="server" ID="hidden" />
        </div>
        </div>
    </form>
</asp:Content>

