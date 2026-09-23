<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvTransfer.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvTransfer" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        </asp:Content>
        <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
            <asp:ScriptManager runat="server">

            </asp:ScriptManager>
            <style type="text/css">
                .displayaction {}

                .nodisplay {
                    display: none;
                }

                .mycheckbox input[type="checkbox"] {
                    margin-right: 10px;
                }

                [disabled] {
                    /* Text and background colour, medium red on light yellow */
                    color: #933;
                    background-color: #ffc;
                }

                .inv-trf-page {
                    padding: 0 15px;
                }
                .inv-trf-toolbar {
                    display: flex;
                    align-items: center;
                    background: #3a4f63;
                    padding: 0.8rem 1rem;
                    gap: 1rem;
                    flex-wrap: wrap;
                    border-radius: 4px;
                    margin-bottom: 1rem;
                }
                .inv-trf-filter {
                    display: flex;
                    align-items: center;
                    color: #FFFFFF;
                }
                .inv-trf-filter label {
                    margin: 0 0.5rem 0 0;
                    white-space: nowrap;
                    font-weight: bold;
                }
                .inv-trf-filter select, .inv-trf-filter input {
                    width: auto;
                    min-width: 130px;
                }
                .inv-trf-filter-name {
                    margin-left: auto;
                }
                .inv-trf-filter-actions {
                    display: flex;
                    gap: 0.5rem;
                }
                .btn-trf {
                    background: #FFFFFF;
                    color: #3a4f63;
                    border: none;
                    padding: 0.375rem 0.75rem;
                    border-radius: 4px;
                    font-weight: bold;
                    transition: all 0.2s;
                }
                .btn-trf:hover {
                    background: #f8f9fa;
                    opacity: 0.9;
                }
                .inv-trf-table-wrap {
                    background: white;
                    padding: 1rem;
                    border-radius: 4px;
                    box-shadow: 0 1px 3px rgba(0,0,0,0.1);
                }
                .clickable-row:hover {
                    background-color: #f5f5f5 !important;
                    transition: background-color 0.15s ease-in-out;
                }
                @media (max-width: 768px) {
                    .inv-trf-toolbar { flex-direction: column; align-items: stretch; }
                    .inv-trf-filter { width: 100%; }
                    .inv-trf-filter select, .inv-trf-filter input { flex: 1; }
                    .inv-trf-filter-name { margin-left: 0; width: 100%; }
                }
            </style>
            
            <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
            <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />

            <script type="text/javascript">
                $(document).ready(function () {
                    $('.btnstyle').click(function () {
                        $('#spinner').show();
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

                $(function () {
                    var table = $("#tblEmpDetails").DataTable({
                        "dom": 'rt<"d-flex justify-content-between align-items-center mt-3"ip><"clear">',
                        "pageLength": 15,
                        "order": [[1, "desc"]]
                    });
                    
                    var lengthHtml = '<div class="inv-trf-filter" id="dtLengthWrap">' +
                                     '<label for="dtLengthSelect"><b>Show:</b></label>' +
                                     '<select id="dtLengthSelect" name="dtLengthSelect" class="form-control" style="width: auto; margin: 0 0.5rem; flex: 0 0 auto;">' +
                                     '<option value="15">15</option>' +
                                     '<option value="25">25</option>' +
                                     '<option value="50">50</option>' +
                                     '<option value="-1">All</option>' +
                                     '</select>' +
                                     '<span><b>entries</b></span>' +
                                     '</div>';
                    
                    var lengthSel = $(lengthHtml);
                    lengthSel.find('select').on('change', function() {
                        table.page.len($(this).val()).draw();
                    });
                    
                    lengthSel.insertBefore('.inv-trf-filter-name');

                    $('#<%= txtsearch.ClientID %>').on('keyup change clear', function () {
                        table.search(this.value).draw();
                    });
                });
            </script>
            <script type="text/javascript">
                function SendEmail() {
                    var confirm_value = document.createElement("INPUT");
                    confirm_value.type = "hidden";
                    confirm_value.name = "confirm_value";
                    if (confirm("Do you want to Show PDF File?")) {
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
                            <h1 class="m-0 text-dark">Inventory Transfer</h1>
                        </div><!-- /.col -->
                        <div class="col-sm-6">
                            <ol class="breadcrumb float-sm-right">
                                <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                <li class="breadcrumb-item active">Inventory Transfer</li>
                            </ol>
                        </div><!-- /.col -->
                    </div><!-- /.row -->
                </div><!-- /.container-fluid -->
            </div>
            <!-- /.content-header -->
            <div id="spinner" class="modal" style="display: none">
                <div class="center">
                    <img alt="" src="/images/loader.gif" />
                </div>
            </div>

            <div class="inv-trf-page">
                <div class="inv-trf-toolbar" id="formload" runat="server">
                    <div class="inv-trf-filter">
                        <label for="<%= txtDate.ClientID %>">From:</label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" style="text-align:center; width:130px;" ReadOnly="true" />
                        <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate" Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                    </div>

                    <div class="inv-trf-filter">
                        <label for="<%= txtToDate.ClientID %>">To:</label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" style="text-align:center; width:130px;" ReadOnly="true" />
                        <ajaxtoolkit:CalendarExtender ID="cleTo" runat="server" TargetControlID="txtToDate" Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                    </div>

                    <div class="inv-trf-filter">
                        <label for="<%= ddlCCFrom.ClientID %>">Cost Centre:</label>
                        <asp:DropDownList ID="ddlCCFrom" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">-- All Cost Centre --</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="inv-trf-filter inv-trf-filter-name">
                        <label for="<%= txtsearch.ClientID %>">Search:</label>
                        <asp:TextBox ID="txtsearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                    </div>

                    <div class="inv-trf-filter-actions">
                        <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" type="button" class="btn btn-trf" title="Search">
                            <i class="fas fa-search"></i>
                        </button>
                        <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click" class="btn btn-trf" title="New">
                            <asp:Label runat="server">New</asp:Label>
                        </button>
                    </div>
                </div>

                <div class="inv-trf-table-wrap">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server">
                        <LayoutTemplate>
                            <div class="table-responsive">
                                <table id="tblEmpDetails" width="100%" class="menutab table table-bordered table-sm table-hover mb-0">
                                    <thead>
                                        <tr role="row">
                                            <th class="sorting_asc" style="width: 140px;"><b style="color: #414a4c">Transfer ID</b></th>
                                            <th style="width: 110px;"><b style="color: #414a4c">Date</b></th>
                                            <th><b style="color: #414a4c">From (Cost Centre)</b></th>
                                            <th><b style="color: #414a4c">To (Cost Centre)</b></th>
                                            <th style="text-align:center; width: 110px;"><b style="color: #414a4c">Items</b></th>
                                            <th style="text-align:right; width: 100px;"><b style="color: #414a4c">Total Qty</b></th>
                                            <th style="text-align:center; width: 80px;"><b style="color: #414a4c">Action</b></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr style="cursor:pointer;" class="clickable-row" onclick="if(!$(event.target).closest('a, button, input, img').length) { $(this).find('input[id*=\'imgBtnEdit\']').click(); }">
                                <td>
                                    <span style="font-weight:600; color:#2c3e50;"><asp:Label ID="Label5" runat="server" Text='<%# Eval("id")%>'></asp:Label></span>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("createD")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("ccname")%>'></asp:Label>
                                </td>
                                <td>
                                    <span class="badge badge-light" style="font-size: 0.9rem; font-weight: normal;"><%# Eval("ccToName")%></span>
                                </td>
                                <td style="text-align:center;">
                                    <span class="badge badge-info" style="font-size: 0.85rem;"><%# Eval("totalItems")%> item(s)</span>
                                </td>
                                <td style="text-align:right;">
                                    <b><%# Eval("totalQty", "{0:#,0.##}")%></b>
                                </td>
                                <td style="text-align:center; white-space:nowrap;">
                                    <asp:ImageButton ID="imgBtnEdit" CssClass="btn2" CommandName="Edt"
                                        ToolTip="Edit transfer"
                                        CommandArgument='<%#Eval("ID").ToString + ";" + Eval("ccfrom").ToString %>'
                                        runat="server" ImageUrl="images/edit.png">
                                    </asp:ImageButton>
                                    <asp:ImageButton Style="margin-left:0.5rem;" ToolTip="Delete transfer"
                                        OnClientClick="Confirm()" Visible="true" ID="imgBtnDelete" CommandName="Del"
                                        CommandArgument='<%#Eval("ID")%>' runat="server"
                                        class='<%# GetBackground(Eval("id")) %>' ImageUrl="images/delete.png">
                                    </asp:ImageButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                <div style="font-family: Calibri; margin-top: 10px;">
                    <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>
                </div>
            </div>
        </asp:Content>