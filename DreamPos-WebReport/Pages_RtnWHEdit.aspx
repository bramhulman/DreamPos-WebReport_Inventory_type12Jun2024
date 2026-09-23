<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_RtnWHEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_RtnWHEdit" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
                <style>
                    .input-group>.form-control {
                        width: 800px;
                    }
                </style>
                <style type="text/css">
                    .modal {
                        position: fixed;
                        top: 0;
                        left: 0;
                        background-color: black;
                        z-index: 99;
                        opacity: 0.8;
                        filter: alpha(opacity=80);
                        -moz-opacity: 0.8;
                        min-height: 100%;
                        width: 100%;
                    }

                    .loading {
                        position: fixed;
                        top: 50% !important;
                        left: 50% !important;
                        transform: translate(-50%, -50%);
                        z-index: 9999;
                        background: #ffffff;
                        padding: 30px 40px;
                        border-radius: 12px;
                        box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);
                        text-align: center;
                        min-width: 300px;
                        display: none;
                        border: none;
                    }
                </style>
                <script type="text/javascript">
                    function ShowProgress() {
                        setTimeout(function () {
                            var modal = $('<div />');
                            modal.addClass("modal");
                            $('body').append(modal);
                            var loading = $(".loading");
                            loading.show();
                        }, 200);
                    }
                    // Updated to use .on() instead of .live() (deprecated)
                    // Updated to use .on() instead of .live() (deprecated)
                    $(document).on("submit", "form", function (e) {
                        // Don't show progress for async postbacks from inside the modal (like search)
                        var target = e.target;
                        if ($(document.activeElement).attr('id') == "<%= btnSearch.ClientID %>" || $(document.activeElement).attr('id') == "<%= txtSearch.ClientID %>") {
                            return;
                        }
                        // Also check if we are in an update panel or if it's a specific control causing it
                        ShowProgress();
                    });
                    //$(document).ready(function () {
                    //    $('.btn').click(function () {
                    //        if (Page_IsValid) {
                    //            $('#spinner').show();
                    //        }
                    //    });
                    //});
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
            </asp:Content>
            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            </asp:Content>
            <asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <asp:ScriptManager runat="server">

                </asp:ScriptManager>

                <div id="spinner" class="loading">
                    <div style="font-size: 3rem; color: #007bff; margin-bottom: 15px;">
                        <i class="fas fa-circle-notch fa-spin"></i>
                    </div>
                    <div
                        style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 1.25rem; color: #333; font-weight: 600; margin-bottom: 5px;">
                        Processing...
                    </div>
                    <div style="font-family: 'Segoe UI', sans-serif; font-size: 0.9rem; color: #6c757d;">
                        Please wait a moment.
                    </div>
                </div>

                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Return to Warehouse Edit</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Return to Warehouse Edit</li>
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
                                            ID="lblorderno" Text="ID Master: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:TextBox style="padding: 0.375rem 0.75rem" runat="server" ID="txtLinkid"
                                                Class="form-control" MaxLength="30" ReadOnly="true" />
                                            <%--<asp:label runat="server" ID="lblStatus" Text="" Style="padding:6px;"
                                                CssClass="align-middle"></asp:label> --%>
                                        </div>


                                    </div>
                                    <div class="form-group row col-md-12">
                                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap"
                                            ID="Label2" Text="From: "></asp:label>
                                        <div class="input-group col-md-6">

                                            <asp:DropDownList ID="ddlFrom"
                                                OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged" runat="server"
                                                CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true"
                                                EnableViewState="True" ViewStateMode="Enabled">
                                                <asp:ListItem Value="0">----Select----</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="ddlFrom" ErrorMessage="Please Choice "
                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>


                                    </div>

                                    <div class="form-group row">
                                        <div Class="col-md-2"></div>
                                        <div class="col-md-offset-2 col-md-10">
                                            <button type="button" runat="server" class="btn btn-primary"
                                                data-toggle="modal" id="btnshow" data-target="#ModalItem"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Add Item
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btnSave"
                                                onserverclick="btnSave_Click"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Save Item
                                            </button>
                                            <button type="button" runat="server" class="btn btn-primary" id="btnBack"
                                                onserverclick="btnBack_Click"
                                                style="font-weight: 600; border-color:gray; width:108px;">
                                                Back
                                            </button>
                                            <%-- <button type="button" runat="server" class="btn btn-primary"
                                                id="btntest1" style="font-weight: 600; border-color:gray; width:108px;">
                                                Add test
                                                </button>--%>
                                                <%--<table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button type="button" runat="server" ID="btnShow2"
                                                                Text="Add Item" CssClass="btn btn-primary"
                                                                data-target="#ModalItem" data-toggle="modal"
                                                                Style="font-weight: 600; border-color:gray; width:108px;" />

                                                        </td>
                                                        <td>

                                                            <asp:Button type="button" runat="server" ID="btnFinish"
                                                                Text="Save Finish" CssClass="btn btn-primary"
                                                                Style="font-weight: 600; border-color:gray; width:108px;" />
                                                        </td>
                                                        <td>
                                                            <asp:Button type="button" runat="server" ID="btnBack"
                                                                Text="Back" CssClass="btn btn-primary"
                                                                Style="font-weight: 600; border-color:gray; width:108px;"
                                                                CausesValidation="false" />
                                                        </td>
                                                    </tr>
                                                    </table>--%>

                                        </div>
                                    </div>



                                </div>

                            </div>
                        </div>


                        <div class="card-body" style="overflow:scroll;">
                            <div class="row">
                                <div class="form-horizontal" style="margin:0 20px; width:100%;">
                                    <asp:GridView ID="DgvTransfer" runat="server" Width="100%"
                                        AutoGenerateColumns="false" OnRowDataBound="RowDataBound"
                                        OnRowDeleting="DgvTransfer_RowDeleting" OnRowUpdating="DgvTransfer_RowUpdating"
                                        OnRowCancelingEdit="DgvTransfer_RowCancelingEdit"
                                        OnRowEditing="DgvTransfer_RowEditing" HeaderStyle-BackColor="#4D4D4D"
                                        HeaderStyle-ForeColor="White" CellPadding="5">
                                        <Columns>


                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblIEditID" runat="server" Text='<%# Eval("ID") %>'>
                                                    </asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStkID" runat="server"
                                                        Text='<%# Eval("stk_id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblStkEditID" runat="server"
                                                        Text='<%# Eval("stk_id") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server"
                                                        Text='<%# Eval("stk_name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditName" runat="server"
                                                        Text='<%# Eval("stk_name") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="cc" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCc" runat="server" Text='<%# Eval("Cc") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditCc" runat="server" Text='<%# Eval("Cc") %>'>
                                                    </asp:Label>
                                                </EditItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTo" runat="server" Text='<%# Eval("Cc_Name") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DdlTo" BackColor="Aqua" runat="server"
                                                        Width="100%">
                                                        <%--<asp:ListItem Value="0"> </asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:DropDownList ID="DdlTo" runat="server"
                                                        AppendDataBoundItems="True" AutoPostBack="True"
                                                        EnableViewState="True" ViewStateMode="Enabled">
                                                        <asp:ListItem Value="0"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit 1" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("U1") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server"
                                                        Text='<%# Eval("U1") %>' Width="60px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:TextBox ID="txtAddUnit1" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U1_nm") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditU1Nm" runat="server"
                                                        Text='<%# Eval("U1_nm") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>

                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit 2" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnit2" runat="server" Text='<%# Eval("U2") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditUnit2" BackColor="Aqua" runat="server"
                                                        Text='<%# Eval("U2") %>' Width="60px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:TextBox ID="txtAddUnit2" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblU2Nm" runat="server" Text='<%# Eval("U2_nm") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditU2Nm" runat="server"
                                                        Text='<%# Eval("U2_nm") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>

                                                    </FooterTemplate>--%>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCost" runat="server"
                                                        Text='<%# Eval("Cost", "{0:#,0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="txtEditcost" runat="server"
                                                        Text='<%# Eval("Cost", "{0:#,0}") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <%-- <FooterTemplate>
                                                    <asp:TextBox ID="txtAddUnit2" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Factor" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfactor" runat="server"
                                                        Text='<%# Eval("factor1") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditfactor" runat="server"
                                                        Text='<%# Eval("factor1") %>' Width="60px"></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtAddfactor" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Return" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblttlStock" runat="server"
                                                        Text='<%# Eval("ttlStock") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEdittlStock" runat="server"
                                                        Text='<%# Eval("ttlStock") %>' Width="60px"></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtttlStock" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current Stock" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEditStock" runat="server"
                                                        Text='<%# Eval("Stock") %>' Width="60px"></asp:Label>
                                                </EditItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:TextBox ID="txtttlStock" runat="server" Width="80px">
                                                    </asp:TextBox>
                                                    </FooterTemplate>--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                                        ImageUrl="~/Images/edit.png" />
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server"
                                                        CommandName="Delete" ImageUrl="~/Images/Delete.png"
                                                        OnClientClick="Confirm()" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="imgbtnUpdate" runat="server"
                                                        CommandName="Update" ImageUrl="~/Images/icon-update.png"
                                                        Width="16px" Height="16px" />
                                                    <asp:ImageButton ID="imgbtnCancel" runat="server"
                                                        CommandName="Cancel" ImageUrl="~/Images/icon-Cancel.png"
                                                        Width="16px" Height="16px" />
                                                </EditItemTemplate>

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
                <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem"
                            aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                            <div class="modal-dialog modal-dialog-scrollable" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="ModalStoreLabel">Select Item</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>

                                    <div class="row">
                                        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                                            <div class="card-body">



                                                <div class="form-group row">
                                                    <asp:Label runat="server" AssociatedControlID="txtSearch"
                                                        CssClass="col-sm-2 col-form-label text-nowrap" Text="Item: ">
                                                    </asp:Label>

                                                    <div class="col-sm-10">
                                                        <div class="input-group">
                                                            <asp:TextBox id="txtSearch" runat="server"
                                                                Style="width:70%;" CssClass="form-control"
                                                                placeholder="Search" type="search" aria-label="Search"
                                                                OnTextChanged="txtSearch_TextChanged">
                                                            </asp:TextBox>
                                                            <div class="input-group-append">
                                                                <button runat="server" id="btnSearch"
                                                                    onserverclick="btnSearch_Click"
                                                                    class="btn btn-primary" title="Search">
                                                                    <i class="fas fa-search"></i>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>



                                    </div>

                                    <div class="modal-body">
                                        <div class="table-responsive">
                                            <table class="table table-hover table-bordered">
                                                <thead class="thead-light">
                                                    <tr>
                                                        <th style="width: 50px;">Check</th>
                                                        <th scope="col" hidden="hidden">ID</th>
                                                        <th scope="col">Name</th>
                                                        <th scope="col" style="width: 100px;">Stock</th>
                                                    </tr>
                                                </thead>
                                                <tbody>

                                                    <asp:ListView ID="LsvStk" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="text-center">
                                                                    <asp:CheckBox ID="CheckBox" runat="server">
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td hidden="hidden">
                                                                    <asp:Label ID="lblID" runat="server"
                                                                        Text=' <%# Eval("Id")%>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server"
                                                                        Text=' <%# Eval("name")%>'></asp:Label>
                                                                </td>
                                                                <td class="text-right">
                                                                    <asp:Label ID="lblStock" runat="server"
                                                                        Text='<%# Eval("stock") %>'></asp:Label>
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
                    function debounce(func, wait, immediate) {
                        var timeout;
                        return function () {
                            var context = this, args = arguments;
                            var later = function () {
                                timeout = null;
                                if (!immediate) func.apply(context, args);
                            };
                            var callNow = immediate && !timeout;
                            clearTimeout(timeout);
                            timeout = setTimeout(later, wait);
                            if (callNow) func.apply(context, args);
                        };
                    };

                    function OnKeyPress(e) {
                        if (e.keyCode == "27") {
                            $find("ModalItem").hide();
                        }
                    }

                    function openModalItem() {
                        $('#ModalItem').modal({ show: true });
                        attachSearchEvents();
                    }

                    function attachSearchEvents() {
                        var $field = $('#<%= txtSearch.ClientID %>');

                        // Unbind existing events to prevent duplicates
                        $field.off('keyup keydown');

                        $field.on('keyup', debounce(function (e) {
                            if (e.keyCode !== 13) { // Ignore Enter key in keyup
                                __doPostBack('<%= txtSearch.UniqueID %>', '');
                            }
                        }, 500));

                        $field.on('keydown', function (e) {
                            if (e.which === 13) {
                                e.preventDefault();
                                __doPostBack('<%= txtSearch.UniqueID %>', '');
                                return false;
                            }
                        });

                        // Set focus to end of text
                        var val = $field.val();
                        $field.focus().val("").val(val);
                    }

                    // Ensures events are re-attached after UpdatePanel refresh
                    function pageLoad() {
                        attachSearchEvents();

                        // Also ensure it attaches when modal is shown (for the first time)
                        $('#ModalItem').on('shown.bs.modal', function () {
                            attachSearchEvents();
                            $('#<%= txtSearch.ClientID %>').focus();
                        });
                    }


                    // Re-attach events after UpdatePanel refresh
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        if ($('#ModalItem').hasClass('show')) {
                            attachSearchEvents();
                        }
                    });

                    if ($(window).width() >= 960) {
                        document.getElementById("formload").style.width = "100%";
                    } else {
                        document.getElementById("formload").style.width = "80%";

                    }
                </script>
            </asp:Content>