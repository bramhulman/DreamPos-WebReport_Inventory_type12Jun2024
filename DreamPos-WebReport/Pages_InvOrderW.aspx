<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvOrderW.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvOrderW" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
        <style type="text/css">
            .displayaction {}
            .nodisplay { display: none; }
            .mycheckbox input[type="checkbox"] { margin-right: 10px; }
            [disabled] { color: #933; background-color: #ffc; }
            .delivery-number-cell { max-width: 220px; white-space: normal; line-height: 1.35; }
            .delivery-number-list { display: inline-block; color: #263544; word-break: break-word; }

            /* Modern Toolbar Styles */
            .inv-who-page { margin: 0 20px; width: auto; }
            .inv-who-toolbar {
                background: #3a4f63; padding: 0.8rem 1rem; display: flex; align-items: center;
                gap: 1rem; flex-wrap: wrap; border-radius: 4px; margin-bottom: 1rem;
            }
            .inv-who-filter { display: flex; align-items: center; color: #FFFFFF; }
            .inv-who-filter label { margin: 0; margin-right: 0.5rem; white-space: nowrap; }
            .inv-who-filter select, .inv-who-filter input { width: auto; min-width: 150px; }
            .inv-who-filter-name { margin-left: auto; }
            #dtLengthWrap label { margin-bottom: 0; font-weight: bold; }
            .inv-who-filter-actions { display: flex; gap: 0.5rem; }
            .btn-search-who {
                background: #FFFFFF; color: #3a4f63; border: none; padding: 0.375rem 0.75rem;
                border-radius: 4px; transition: all 0.2s;
            }
            .btn-search-who:hover { background: #f8f9fa; opacity: 0.9; }
            .inv-who-table-wrap {
                background: white; padding: 1rem; border-radius: 4px; box-shadow: 0 1px 3px rgba(0,0,0,0.1);
            }
            .clickable-row:hover {
                background-color: #f5f5f5 !important; transition: background-color 0.15s ease-in-out;
            }
            @media (max-width: 768px) {
                .inv-who-toolbar { flex-direction: column; align-items: stretch; }
                .inv-who-filter { width: 100%; }
                .inv-who-filter select, .inv-who-filter input { flex: 1; }
                .inv-who-filter-name { margin-left: 0; width: 100%; justify-content: flex-end; }
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
                    "dom": 'rt<"d-flex justify-content-between align-items-center mt-3"ip><"clear">'
                });
                
                var lengthHtml = '<div class="inv-who-filter" id="dtLengthWrap">' +
                                 '<label for="dtLengthSelect"><b>Show:</b></label>' +
                                 '<select id="dtLengthSelect" name="dtLengthSelect" class="form-control" style="width: auto; margin: 0 0.5rem; flex: 0 0 auto;">' +
                                 '<option value="10">10</option>' +
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
                
                lengthSel.insertBefore('.inv-who-filter-name');

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
                            <h1 class="m-0 text-dark">Order/ Receive Stock To Warehouse</h1>
                        </div><!-- /.col -->
                        <div class="col-sm-6">
                            <ol class="breadcrumb float-sm-right">
                                <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                <li class="breadcrumb-item active">Order/ Receive Stock To Warehouse</li>
                            </ol>
                        </div><!-- /.col -->
                    </div><!-- /.row -->
                </div><!-- /.container-fluid -->
            </div>
            <!-- /.content-header -->
            <%-- <h2>User Access</h2>--%>
                <div id="spinner" class="modal" style="display: none">
                    <div class="center">
                        <img alt="" src="/images/loader.gif" />
                    </div>
                </div>
            <div class="inv-who-page">
                <div id="formload" runat="server" class="inv-who-toolbar">
                    <div class="inv-who-filter">
                        <label for="<%= DdlType.ClientID %>"><b>Type:</b></label>
                        <asp:DropDownList ID="DdlType" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            CssClass="form-control" EnableViewState="True" ViewStateMode="Enabled">
                            <asp:ListItem Value="1">--Order--</asp:ListItem>
                            <asp:ListItem Value="2">--Delivery--</asp:ListItem>
                            <asp:ListItem Value="3">--Finish--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="inv-who-filter inv-who-filter-name">
                        <label for="<%= txtsearch.ClientID %>"><b>Name:</b></label>
                        <asp:TextBox ID="txtsearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                    </div>

                    <div class="inv-who-filter-actions">
                        <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" type="button" class="btn btn-search-who" title="Search">
                            <i class="fas fa-search"></i>
                        </button>
                        <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click" class="btn btn-search-who" title="New" style="font-weight:bold;">
                            <asp:Label runat="server">New</asp:Label>
                        </button>
                    </div>
                </div>


                <div class="inv-who-table-wrap">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server">
                        <LayoutTemplate>
                            <div class="table-responsive">
                                <table id="tblEmpDetails" width="100%" class="menutab table table-bordered table-sm table-hover mb-0">
                                        <thead>
                                            <tr role="row">
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">ID</b>
                                                </th>

                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">DO NO</b>
                                                </th>

                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">From</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">To</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Status</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Date</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Expected Delivery</b>
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
                                <tr style="cursor:pointer;" class="clickable-row" onclick="if(!$(event.target).closest('a, button, input, img').length) { $(this).find('input[id*=\'imgBtnEdit\']').click(); }">
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>

                                <td class="delivery-number-cell">
                                    <asp:Label ID="Label1" runat="server" CssClass="delivery-number-list" Text=' <%# Eval("DeliveryNumbers")%>'></asp:Label>
                                </td>

                                <td>
                                    <asp:Label ID="Label3" runat="server" Text=' <%# Eval("ccFromNm")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text=' <%# Eval("ccToNm")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text=' <%# Eval("STATUS")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text=' <%# Eval("statdate")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text=' <%# Eval("exdeldate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                </td>
                                <td style="width: 8%; text-align: center">
                                        <asp:ImageButton Style="margin-right:0.5rem;" ID="imgBtnEdit" CommandName="Edt"
                                            ToolTip="Edit a record."
                                            CommandArgument='<%#Eval("ID").ToString + ";" + Eval("ccTo").ToString + ";" + Eval("ccFrom").ToString + ";" + Eval("statdate").ToString + ";" + Eval("Notes").ToString + ";" + Eval("createddate").ToString + ";" + Eval("deldate").ToString + ";" + Eval("rcvdate").ToString + ";" + Eval("DelID").ToString %>'
                                            runat="server" ImageUrl="images/edit.png">
                                        </asp:ImageButton>
                                        <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="Delete a record."
                                            OnClientClick="Confirm()" class='<%# GetBackground() %>' ID="ImageButton2"
                                            Visible='true' CommandName="Del"
                                            CommandArgument='<%#Eval("ID").tostring+ ";" +Eval("ccTo").ToString%>'
                                            runat="server" ImageUrl="~/images/delete.png"></asp:ImageButton>
                                        <asp:ImageButton BackColor="Black" ToolTip="Print PDF."
                                            OnClientClick="SendEmail()" class='<%# Getprintbg() %>' ID="imgEmail"
                                            CommandName="Send" CommandArgument='<%#Eval("ID").ToString + ";" + Eval("DelID").ToString + ";" + Eval("ccTo").ToString %>' runat="server"
                                            ImageUrl="images/print.png"></asp:ImageButton>

                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                </div>
            </div>
    </asp:Content>
