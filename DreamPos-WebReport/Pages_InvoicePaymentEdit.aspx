<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvoicePaymentEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvoicePaymentEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>
                <style>
                    .input-group>.form-control {
                        width: 800px;
                    }
                </style>
                <script type="text/javascript">
                    function ShowProgress() {
                        setTimeout(function () {
                            var modal = $('<div />');
                            modal.addClass("modal");
                            $('body').append(modal);
                            var loading = $(".loading");
                            if (loading.length > 0) {
                                loading.show();
                                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                                loading.css({ top: top, left: left });
                            }
                        }, 200);
                    }
                    $('form').submit(function () {
                        ShowProgress();
                    });
                    $(document).ready(function () {
                        $('.btn').click(function () {
                            if (typeof Page_IsValid == 'undefined' || Page_IsValid) {
                                $('#spinner').show();
                            }
                        });
                    });
                    function Confirm() {
                        var confirm_value = document.createElement("INPUT");
                        confirm_value.type = "hidden";
                        confirm_value.name = "confirm_value";
                        if (confirm("Do you want to Delete data?")) {
                            confirm_value.value = "Yes";
                        } else {
                            confirm_value.value = "No";
                        }
                        document.forms[0].appendChild(confirm_value);
                    }
                </script>
                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Inventory Invoice</h1>
                                <asp:Label ID="lblMsg" runat="server" Visible="false">
                                </asp:Label>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Store Invoice</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color:white;">

                    <!-- /.content-header -->
                    <%-- <h2>User Access</h2>--%>

                        <div class="row">
                            <div class="form-horizontal" id="formload">
                                <div class="card-body">
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblCC" Text="Cost Centre:"></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:DropDownList ID="DdlCc" runat="server" AppendDataBoundItems="True"
                                                CssClass="form-control" AutoPostBack="True" EnableViewState="True"
                                                ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="DdlSup" ErrorMessage="Supplier Must Be Choice"
                                                InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                --%>

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblpaymentno" Text="Payment No: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="" runat="server" ID="txtpaymentno" MaxLength="18"
                                                OnTextChanged="txtpaymentno_TextChanged" Class="form-control" />
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="txtinvoiceno" ErrorMessage="Number Must Be fill"
                                                InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                --%>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblDate" Text="Payment Date: "></asp:label>
                                        <div class="input-group col-md-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="far fa-calendar-alt"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox style="width:auto;" runat="server" id="txtpaymentdate"
                                                    Class="form-control" AutoPostBack="false" />
                                                <ajaxtoolkit:CalendarExtender ID="cle" runat="server"
                                                    TargetControlID="txtpaymentdate" Format="dd MMM yyyy HH:mm:ss">
                                                </ajaxtoolkit:CalendarExtender>
                                            </div>

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblInvoiceTotal" Text="Invoice Total: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:right;" runat="server" ID="txtinvoicetot"
                                                ReadOnly="true"
                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                Class="form-control" />
                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="txtinvoicetot"
                                                ErrorMessage="Invoice Total Must Be Fill." ForeColor="Red">
                                                </asp:RequiredFieldValidator>--%>

                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="lblNotes" Text="Notes: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="text-align:left;" runat="server" ID="txtnotes"
                                                TextMode="MultiLine" MaxLength="100" Class="form-control" />

                                        </div>


                                    </div>





                                    <div class="form-group row">
                                        <div Class="col-md-2"></div>
                                        <div class="col-md-offset-2 col-md-10">
                                            <button type="button" runat="server" class="btn btn-primary"
                                                data-toggle="modal" id="InsertButton" data-target="#ModalItem"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Add
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btnSave"
                                                onserverclick="btnSave_Click"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Save
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btncancel"
                                                onserverclick="btnCancel_Click"
                                                style="font-weight: 600; border-color:gray; width:108px;"
                                                causesvalidation="false">
                                                Cancel
                                            </button>


                                        </div>
                                    </div>



                                </div>

                            </div>
                        </div>
                        <div class="card-body" style="overflow:scroll;">
                            <div class="row">
                                <div class="form-horizontal" style="margin:0 20px; width:100%;">
                                    <asp:GridView ID="DgvRcv" runat="server" Width="100%" AutoGenerateColumns="false"
                                        OnRowDataBound="RowDataBound" OnRowDeleting="DgvRcv_RowDeleting"
                                        OnRowUpdating="DgvRcv_RowUpdating" OnRowCancelingEdit="DgvRcv_RowCancelingEdit"
                                        OnRowEditing="DgvRcv_RowEditing" HeaderStyle-BackColor="#4D4D4D"
                                        HeaderStyle-ForeColor="White" CellPadding="5">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="14%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditID" runat="server" Text='<%# Eval("id") %>'>
                                                    </asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtAddID" runat="server" Width="100px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ccfrom" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblccfrom" runat="server"
                                                        Text='<%# Eval("ccfrom") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditccfrom" runat="server"
                                                        Text='<%# Eval("ccfrom") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ccto" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblccto" runat="server" Text='<%# Eval("ccto") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditccto" runat="server"
                                                        Text='<%# Eval("ccto") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cctonm" HeaderStyle-Width="14%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCctonm" runat="server"
                                                        Text='<%# Eval("cctonm") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditcctonm" runat="server"
                                                        Text='<%# Eval("cctonm") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtAddID" runat="server" Width="100px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocTot" runat="server"
                                                        Text='<%# Eval("DocTot", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditDocTot" runat="server"
                                                        Text='<%# Eval("DocTot", "{0:#,0}") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtAddName" runat="server" Width="100px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Received" HeaderStyle-Width="18%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrcvdate" runat="server"
                                                        Text='<%# Eval("Rcvdate", "{0:dd MMM yyyy}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblrcvdate" runat="server"
                                                        Text='<%# Eval("Rcvdate", "{0:dd MMM yyyy}") %>'>
                                                    </asp:Label>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>

                                                    </FooterTemplate>--%>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="18%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server"
                                                        Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditStatus" runat="server"
                                                        Text='<%# Eval("Status") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:TextBox ID="txtAddUnit1" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
                                                <ItemTemplate>

                                                    <asp:ImageButton ID="imgbtnDelete" runat="server"
                                                        CommandName="Delete" ImageUrl="~/Images/Delete.png"
                                                        OnClientClick="Confirm()" />
                                                </ItemTemplate>
                                                <%--<EditItemTemplate>

                                                    <asp:ImageButton ID="imgbtnCancel" runat="server"
                                                        CommandName="Cancel" ImageUrl="~/Images/delete.png" Width="16px"
                                                        Height="16px" />
                                                    </EditItemTemplate>--%>
                                                    <%--<FooterTemplate>
                                                        <asp:LinkButton ID="lbtnAdd" runat="server" CommandName="ADD"
                                                            Text="Add" Width="80px">
                                                        </asp:LinkButton>
                                                        </FooterTemplate>--%>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </div>
                                <%-- <div class="table-responsive" style="margin:0 20px; width:100%;">
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                                                <asp:DataPager ID="lvDataPager1" runat="server"
                                                    PagedControlID="lstViewDetails" PageSize="15">
                                                    <Fields>
                                                        <asp:NumericPagerField ButtonType="Link" />
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                    </table>
                            </div>--%>

                        </div>
                </div>
                <!-- Modal Item -->
                <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem"
                    aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                    <div class="modal-dialog modal-dialog-scrollable" role="document">
                        <div class="modal-content">
                            <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="ModalStoreLabel">Select Invoice</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>

                                    <div class="row">
                                        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                                            <div class="card-body">
                                                <div class="form-group row" style="padding: 0 1rem;">
                                                    <asp:Label runat="server" AssociatedControlID="txtSearch"
                                                        CssClass="col-sm-2 col-form-label text-nowrap" Text="Item: ">
                                                    </asp:Label>

                                                    <div class="col-sm-10">
                                                        <div class="input-group">
                                                            <asp:TextBox id="txtSearch" runat="server"
                                                                Style="width:50%;" CssClass="form-control"
                                                                placeholder="Search" type="search" aria-label="Search">
                                                            </asp:TextBox>
                                                            <div class="input-group-btn">
                                                                <button runat="server" id="btnSearch"
                                                                    onserverclick="btnSearch_ServerClick"
                                                                    class="btn btn-default" title="Search"
                                                                    style="margin-left: -2px; border-radius: 0;">
                                                                    <i class="fas fa-search"></i>
                                                                </button>
                                                                <button runat="server" id="btnClear"
                                                                    onserverclick="btnClearSearch_Click"
                                                                    class="btn btn-danger" title="Clear Search"
                                                                    style="border-radius: 0 4px 4px 0;">
                                                                    <i class="fas fa-times"></i>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="modal-body">
                                        <div class="table-responsive" style="max-height: 400px; overflow: auto;">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th>check</th>
                                                        <th scope="col">ID</th>

                                                        <th scope="col" id="ccheader" runat="server" visible="false">CC
                                                        </th>
                                                        <th scope="col">CostCentre</th>
                                                        <th scope="col">Total</th>
                                                        <th scope="col">Receive Date</th>

                                                    </tr>
                                                </thead>
                                                <tbody>

                                                    <asp:ListView ID="LsvInvRcv" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server">
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server"
                                                                        Text=' <%# Eval("Id")%>'></asp:Label>
                                                                </td>
                                                                <td id="cc_col" runat="server" hidden="true">
                                                                    <asp:Label ID="lblName" runat="server"
                                                                        Text=' <%# Eval("CCto")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server"
                                                                        Text=' <%# Eval("cctonm")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label2" runat="server"
                                                                        Text=' <%# Eval("DocTot")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server"
                                                                        Text=' <%# Eval("rcvDate")%>'></asp:Label>
                                                                </td>

                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>


                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary"
                                            data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary" runat="server"
                                            onserverclick="RadButton1_ServerClick" id="RadButton1">Add</button>
                                        <%--<button id="btnAll1" type="button" runat="server" class="btn btn-primary"
                                            onserverclick="btnAll1_ServerClick">SelectAll</button>--%>
                                            <%--<button type="button" class="btn btn-primary" runat="server"
                                                onserverclick="btnSubmit_Click">Select</button>
                                                <button type="button" id="btnAll" runat="server" class="btn btn-primary"
                                                    onserverclick="btnAll_Click">SelectAll</button>--%>
                                    </div>
                        </div>
                    </div>
                </div>

                </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Modal Store -->
                <div style="font-family: Calibri; ">
                    <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>

                </div>
                </div>


                <script>

                    if ($(window).width() >= 960) {
                        document.getElementById("formload").style.width = "100%";
                    } else {
                        document.getElementById("formload").style.width = "80%";

                    }
                    function OnKeyPress(e) {
                        if (e.keyCode == "27") {
                            $find("ModalItem").hide();


                        }
                    }

                    function openModalItem() {
                        $('#ModalItem').modal({ show: true });
                        //document.getElementById("txtsearch").focus();
                    }

                    if ($(window).width() >= 960) {
                        document.getElementById("formload").style.width = "100%";
                    } else {
                        document.getElementById("formload").style.width = "80%";

                    }

                </script>
            </asp:Content>