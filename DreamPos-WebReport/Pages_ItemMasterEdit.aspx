<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_ItemMasterEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_ItemMasterEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        </asp:Content>
        <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
            <asp:ScriptManager runat="server">

            </asp:ScriptManager>
            <style>
                .item-master-form { width: 100%; }
                .item-master-form .card-body { padding: 20px 16px; }
                .item-master-form .form-row {
                    display: grid;
                    grid-template-columns: 140px minmax(0, 1fr);
                    gap: 0 16px;
                    margin: 0 0 18px;
                    align-items: start;
                }
                .item-master-form .form-row > * {
                    width: 100%; max-width: 100%;
                    padding-left: 0; padding-right: 0;
                }
                .item-master-form .col-form-label {
                    white-space: normal !important;
                    padding-top: 10px;
                }
                .item-master-form .input-group { display: block; }
                .item-master-form .form-control {
                    width: 100% !important; min-width: 0;
                    height: 44px; border-radius: 4px;
                }
                .item-master-form .form-check {
                    display: flex; align-items: center; min-height: 44px;
                }
                .item-master-form .select2-container { width: 100% !important; }
                .item-master-form .select2-selection--single {
                    height: 44px; border: 1px solid #b8c6d4; border-radius: 4px;
                }
                .item-master-form .select2-selection--single .select2-selection__rendered {
                    line-height: 42px; padding-left: 12px; padding-right: 32px;
                }
                .item-master-form .select2-selection--single .select2-selection__arrow {
                    height: 42px; right: 7px;
                }
                .item-master-form .select2-container--focus .select2-selection,
                .item-master-form .select2-container--open .select2-selection {
                    border-color: #2374b9;
                    box-shadow: 0 0 0 3px rgba(35,116,185,.18);
                }
                .item-master-form .field-validation { font-size: 13px; }
                .item-master-actions {
                    display: flex; flex-wrap: wrap; gap: 10px;
                    margin-left: 156px; padding-top: 16px;
                    border-top: 1px solid #dce3e9;
                }
                .item-master-actions .btn {
                    min-height: 44px; min-width: 104px; border-radius: 4px;
                }
                @media (max-width: 575.98px) {
                    .item-master-form .card-body { padding: 16px 0; }
                    .item-master-form .form-row {
                        grid-template-columns: minmax(0, 1fr);
                        gap: 6px; margin-bottom: 16px;
                    }
                    .item-master-form .col-form-label { padding-top: 0; padding-bottom: 0; }
                    .item-master-actions { margin-left: 0; }
                }
            </style>
            <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
            <%--<script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>--%>
                <script src="js/jquery.ui.core.js" type="text/javascript"></script>
                <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
                <script src="js/jquery.ui.button.js" type="text/javascript"></script>
                <script src="js/jquery.ui.position.js" type="text/javascript"></script>
                <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script>
                <script src="js/jquery.ui.combobox.js" type="text/javascript"></script>
                <script type="text/javascript"
                    src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

                <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
                <script type="text/javascript">
                    $(document).ready(function () {
                        $("#<%= ddlItem.ClientID %>").select2({ width: "100%" });
                    });
                </script>
                <link href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" rel="stylesheet"
                    type="text/css" />
                <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.min.js"
                    type="text/javascript"></script>


                <script type="text/javascript">
                    function ShowProgress() {
                        setTimeout(function () {
                            var modal = $('<div />');
                            modal.addClass("modal");
                            $('body').append(modal);
                            var loading = $(".loading");
                            loading.show();
                            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                            loading.css({ top: top, left: left });
                        }, 200);
                    }
                    $(document).on("submit", "form", function () {
                        ShowProgress();
                    });
                    $(document).ready(function () {
                        $('.btn').click(function () {
                            if (Page_IsValid) {
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
                                <h1 class="m-0 text-dark">Item Master Edit</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Item Master Edit</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <div class="col-md-12" style="background-color:white;">

                    <!-- /.content-header -->
                    <%-- <h2>User Access</h2>--%>

                        <div class="row">
                            <div class="col-xl-6 col-12">
                                <div class="form-horizontal item-master-form" id="formload">
                                    <div class="card-body">
                                        <div class="form-row">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                                ID="lblitemno" Text="Item No: "></asp:label>
                                            <div class="input-group col-md-8">

                                                <asp:TextBox runat="server" ID="txtNo" Class="form-control"
                                                    style="width:800px;" MaxLength="8" />

                                            </div>


                                        </div>
                                        <div class="form-row">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                                ID="Label2" Text="Item Name: "></asp:label>
                                            <div class="input-group col-md-8">

                                                <asp:TextBox runat="server" ID="txtName" Class="form-control"
                                                    MaxLength="25" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                    ControlToValidate="txtName" ErrorMessage="Item Name Must Be Fill."
                                                    ForeColor="Red" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>

                                            </div>


                                        </div>
                                        <div class="form-row">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                                ID="Label4" Text="Major Group: "></asp:label>
                                            <div class="input-group col-md-8">

                                                <asp:DropDownList ID="ddlmajorgroup" runat="server"
                                                    CssClass="form-control" style="width:400px;" Class="form-control"
                                                    AppendDataBoundItems="True" EnableViewState="True"
                                                    ViewStateMode="Enabled" Enabled="False">
                                                    <asp:ListItem Value="0">----None----</asp:ListItem>
                                                </asp:DropDownList>


                                            </div>


                                        </div>
                                        <div class="form-row">
                                            <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                                ID="Label9" Text="Item Group: "></asp:label>
                                            <div class="input-group col-md-8">

                                                <asp:DropDownList ID="ddlitemgroup" runat="server"
                                                    CssClass="form-control" style="width:400px;" Class="form-control"
                                                    AppendDataBoundItems="True" EnableViewState="True"
                                                    ViewStateMode="Enabled" Enabled="False">
                                                    <asp:ListItem Value="0">----None----</asp:ListItem>
                                                </asp:DropDownList>


                                            </div>


                                        </div>
                                        <%-- <div class="form-group row col-md-12">
                                            <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                                ID="Label9" Text="CC: "></asp:label>
                                            <div class="input-group col-md-3">
                                                <asp:DropDownList ID="ddlcc"
                                                    OnSelectedIndexChanged="ddlcc_SelectedIndexChanged" runat="server"
                                                    CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                                                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                    ControlToValidate="ddlcc" ErrorMessage="Please Choice Cc"
                                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>


                                    </div> --%>
                                    <div class="form-row" style="display:none">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label3" Text="Price: "></asp:label>
                                        <div class="input-group col-md-8">

                                            <asp:TextBox style="text-align:right;" runat="server" ID="txtPrice"
                                                onkeypress="return this.value.length<=15"
                                                onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                Class="form-control" />
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txtPrice" ErrorMessage="Price Must Be Fill."
                                                ForeColor="Red"></asp:RequiredFieldValidator>--%>

                                        </div>


                                    </div>
                                    <%-- <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap"
                                            ID="Label4" Text="Cost: "></asp:label>
                                        <div class="input-group col-md-3">

                                            <asp:TextBox
                                                style="text-align:right;width:800px; padding: 0.375rem 0.75rem;"
                                                runat="server" ID="txtCost" Enabled="false" Class="form-control" />


                                        </div>


                                </div> --%>
                                <div class="form-row">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label5"
                                        Text="Link: "></asp:label>
                                    <div class="form-check input-group col-md-8">

                                        <asp:CheckBox ID="chklink" runat="server" AutoPostBack="True"
                                            OnCheckedChanged="chkbatch_CheckedChanged"
                                            cssclass="form-control-check-input" type="checkbox" />
                                        <asp:label runat="server" CssClass="form-check-label" for="chklink" ID="Label1"
                                            Text="Yes" style="padding-left:10px;"></asp:label>
                                    </div>


                                </div>
                                <div class="form-row">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label6"
                                        Text="Type: "></asp:label>
                                    <div class="input-group col-md-8">

                                        <asp:DropDownList ID="DdlType" Enabled="false"
                                            style="width:800px;padding: 0.375rem 0.75rem;" runat="server"
                                            AppendDataBoundItems="True" AutoPostBack="True" Class="form-control"
                                            EnableViewState="True" ViewStateMode="Enabled">
                                            <asp:ListItem Value="0">--Select Type--</asp:ListItem>
                                            <asp:ListItem Value="1">Inventory Stock</asp:ListItem>
                                            <asp:ListItem Value="2">Inventory Receipe</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>


                                </div>
                                <div class="form-row">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label7"
                                        Text="Stock/Rcp: "></asp:label>
                                    <div class="input-group col-md-8">

                                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control"
                                            style="width:400px;" Class="form-control" EnableViewState="True"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Value="0">----Select----</asp:ListItem>
                                        </asp:DropDownList>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlItem" runat="server"
                                            ControlToValidate="ddlItem" ErrorMessage="Stock/Rcp Must Be Choice."
                                            ForeColor="Red" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>

                                    </div>


                                </div>
                                <div class="form-row">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label8"
                                        Text="Factor: "></asp:label>
                                    <div class="input-group col-md-8">

                                        <asp:TextBox style="text-align:right;" runat="server" ID="txtFactor"
                                            Class="form-control" text="0" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="txtFactor" ErrorMessage="Factor Must Be Fill."
                                            ForeColor="Red" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>

                                    </div>


                                </div>
                                <div class="form-row">
                                    <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                        ID="Label10" Text="Usage: "></asp:label>
                                    <div class="form-check input-group col-md-8">

                                        <asp:CheckBox ID="Chkusage" runat="server" AutoPostBack="True"
                                            OnCheckedChanged="Chkusage_CheckedChanged"
                                            cssclass="form-control-check-input" type="checkbox" />
                                        <asp:label runat="server" CssClass="form-check-label" for="Chkusage"
                                            ID="Label11" Text="Yes" style="padding-left:10px;"></asp:label>
                                    </div>


                                </div>


<div class="form-group">
<div>
<div class="item-master-actions"><button type="button" runat="server" class="btn btn-outline-primary"
                                                        data-toggle="modal" id="btnaddcc" data-target="#ModalItem"
                                                        style="font-weight: 600;">
                                                        Add CC
                                                    </button>



                                                    <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click"
                                                        Text="Add" CssClass="btn btn-primary"
                                                        style="font-weight: 600;" />



                                                    <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click"
                                                        Text="Update" CssClass="btn btn-primary"
                                                        style="font-weight: 600;"
                                                        Visible="false" />


                                                    <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click"
                                                        Text="Cancel" CssClass="btn btn-outline-secondary"
                                                        style="font-weight: 600;"
                                                        CausesValidation="false" /></div>

                                    </div>
                                </div>



                            </div>

                        </div>
                </div>

                <div class="col-lg-6 col-md-6">
                    <div class="form-horizontal" style="margin:0 20px; width:100%;overflow:scroll;">
<asp:ListView ID="lstViewCost" runat="server" OnItemCommand="lstViewCost_ItemCommand"
                            OnPagePropertiesChanging="OnPagecostChanging">
                            <LayoutTemplate>
                                <div class="table-responsive">
                                    <table id="tblCostDetails" align="left"
                                        style="border-style: solid; width:100%; margin-bottom:0;"
                                        class="menutab table table-bordered">
                                        <thead>
                                            <tr role="row">
                                                <%-- <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Item No</b>
                                                    </th>--%>
                                                    <%--<th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Item Name</b>
                                                        </th>--%>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Cost Centre</b>
                                                        </th>

                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Cost</b>
                                                        </th>
                                                        <th class="sorting_asc" tabindex="0" Width="20%"
                                                            aria-controls="example2" rowspan="1" colspan="1"
                                                            aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Add Cost</b>
                                                        </th>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Action</b>
                                                        </th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </div>
                            </LayoutTemplate>


                            <ItemTemplate>


                                <%-- <td>
                                    <asp:Label ID="Label3" runat="server" Text=' <%# Eval("Name")%>'></asp:Label>
                                    </td>--%>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("Cost2", "{0:0.00}")%>'>
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Textbox ID="taddcost" runat="server"
                                            Text=' <%# Eval("addcost", "{0:0.00}")%>' BorderWidth="0" Enabled="false">
                                        </asp:Textbox>
                                    </td>
                                    <td style="width: 8%; text-align: center">

                                        <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                                            CommandArgument='<%#Eval("ccid").ToString%>' runat="server"
                                            ImageUrl="/images/edit.png" />
                                        <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                                            ID="imgBtnDelete" CommandName="Del"
                                            CommandArgument='<%#Eval("ccID").ToString + ";" + Eval("ccname").ToString %>'
                                            runat="server" ImageUrl="~/images/delete.png"></asp:ImageButton>
                                        <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="save"
                                            CommandArgument='<%#Eval("ccID").ToString + ";" + Eval("ccname").ToString %>'
                                            visible="false" ImageUrl="~/Images/icon-update.png" Width="16px"
                                            Height="16px" />
                                    </td>




                                    </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <%--< /table>

                            <br />--%>
                    </div>
                    <div class="table-responsive" style="margin:0 20px; width:100%;">
                        <table class="table" style="margin-bottom: 0;">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                                    <asp:DataPager ID="lstViewCostPager1" runat="server" PagedControlID="lstViewCost"
                                        PageSize="5">
                                        <Fields>
                                            <asp:NumericPagerField ButtonType="Link" />
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>

                </div>

                <!-- Modal CC -->
                <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem"
                            aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                            <div class="modal-dialog modal-dialog-scrollable" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="ModalStoreLabel">Select CC</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>

                                    <div class="row">
                                        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                                            <div class="card-body">




                                                <div class="form-group row">
                                                    <asp:Label runat="server" AssociatedControlID="txtSearchcc"
                                                        CssClass="col-sm-2 col-form-label text-nowrap" Text="CC: ">
                                                    </asp:Label>

                                                    <div class="input-group-append">
                                                        <div class="input-group input-group-sm">
                                                            <asp:TextBox id="txtSearchcc" style="width:auto;"
                                                                runat="server" CssClass="form-control"
                                                                placeholder="Search" type="search" aria-label="Search">
                                                            </asp:TextBox>


                                                            <button runat="server" id="btnSearchcc"
                                                                onserverclick="Searchcc_Click" class="btn btn-mini"
                                                                title="Search">
                                                                <i class="fas fa-search"></i>
                                                            </button>
                                                            <button runat="server" id="btnClearSearchcc"
                                                                onserverclick="btnClearSearch_Click"
                                                                class="btn btn-mini" title="Clear Search">
                                                                <i class="fas fa-times"></i>
                                                            </button>


                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>



                                    </div>

                                    <div class="modal-body">
                                        <div class="table-responsive">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <%--<th>check</th>--%>
                                                            <th scope="col">
                                                                <asp:CheckBox ID="Checkbox_allcc" runat="server"
                                                                    ToolTip="SelectAll"
                                                                    onclick="javascript:fnSelectAllCC();"
                                                                    OnCheckedChanged="Checkbox_allcc_CheckedChanged">
                                                                </asp:CheckBox>
                                                            </th>
                                                            <th scope="col" hidden="hidden">ID</th>

                                                            <th scope="col">Cost Centre</th>
                                                            <%--<th scope="col">CC Stock</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>

                                                    <asp:ListView ID="LsvStk" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="CheckBox" runat="server">
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td hidden="hidden">
                                                                    <asp:Label ID="lblID" runat="server"
                                                                        Text=' <%# Eval("Id")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server"
                                                                        Text=' <%# Eval("ccname")%>'></asp:Label>
                                                                </td>
                                                                <%--<td>
                                                                    <asp:Label ID="Label1" runat="server"
                                                                        Text=' <%# Eval("stock")%>'></asp:Label>
                                                                    </td>--%>
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
                <%--modalcc--%>

                    </div>


                    <script>
                        function OnKeyPress(e) {
                            if (e.keyCode == "27") {
                                $find("ModalItem").hide();


                            }
                        }

                        function openModalcc() {
                            $('#ModalItem').modal({ show: true });
                            //document.getElementById("txtsearch").focus();
                        }


                    </script>
        </asp:Content>
