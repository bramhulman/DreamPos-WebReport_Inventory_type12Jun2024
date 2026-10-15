<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_ItemMaster.aspx.vb" Inherits="DreamPos_WebReport.Pages_ItemMaster" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
        <asp:ScriptManager runat="server">

        </asp:ScriptManager>
        <style type="text/css">
            .mycheckbox input[type="checkbox"] {
                margin-right: 10px;
            }

            .loading {
                font-family: Arial;
                font-size: 10pt;
                border: 5px solid #67CFF5;
                width: 200px;
                height: 100px;
                display: none;
                position: fixed;
                background-color: White;
                z-index: 999;
            }
        </style>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
        <link type="text/css" rel="stylesheet"
            href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
        <link type="text/css" rel="stylesheet"
            href="https://cdn.datatables.net/buttons/1.5.1/css/buttons.dataTables.min.css" />
        <script type="text/javascript"
            src="https://cdn.datatables.net/buttons/1.5.1/js/dataTables.buttons.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
        <script type="text/javascript"
            src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.32/pdfmake.min.js"></script>
        <script type="text/javascript"
            src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.32/vfs_fonts.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.html5.min.js"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.print.min.js"></script>
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
            $('form').live("submit", function () {
                ShowProgress();
            });
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
                $("#tblEmpDetails").DataTable({
                    "lengthMenu": [[15, 25, 50, -1], [15, 25, 50, "All"]],
                    "dom": "<'row'<'col-sm-4'l><'col-sm-4 text-center'B><'col-sm-4'f>>" +
                        "<'row'<'col-sm-12'tr>>" +
                        "<'row'<'col-sm-5'i><'col-sm-7'p>>",
                    "buttons": [
                        { extend: 'excelHtml5', title: 'Item Master List', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] } },
                        { extend: 'pdfHtml5', title: 'Item Master List', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] } },
                        { extend: 'print', title: 'Item Master List', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] } }
                    ]
                });
            });

        </script>

        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0 text-dark">Item Master</h1>
                    </div><!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Item Master</li>
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
            <div class="row">

                <div class="form-inline" id="formload" runat="server"
                    style="margin:0 20px; width:100%;background: #3a4f63;padding:0.2rem 0.8rem">
                    <div class="form-group mx-sm-3 mt-mb-1" style="display:none;">
                        <asp:label AssociatedControlID="txtsearch" runat="server">Nama:</asp:Label>
                        <asp:TextBox id="txtsearch" runat="server" Width="300px" CssClass="form-control"
                            placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                    </div>
                    <div class="form-group mx-sm-3 mt-mb-1">
                        <asp:label for="chklink" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">By
                                Link:</b></asp:label>
                        <div class="form-check">
                            <asp:CheckBox ID="chklink" runat="server" AutoPostBack="true" style="color: #FFFFFF;"
                                class="form-check-input" OnCheckedChanged="chklink_CheckedChanged" type="checkbox" />

                            <label class="form-check-label" for="flexCheckChecked">
                                <b style="color: #FFFFFF;margin-right:0.5rem;"> ALLDATA</b>
                            </label>
                        </div>
                    </div>
                    <div class="form-group mx-sm-3 mt-mb-1">
                        <div class="btn-group" role="group" style="margin:0.5rem; display:none;">
                            <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-mini inventory-search-button"
                                title="Search" style="background:#FFFFFF;color:#3a4f63;" aria-label="Cari nama"><i class="fas fa-search" aria-hidden="true"></i></button>
                        </div>
                        <div class="btn-group" role="group" style="margin:0.5rem">
                            <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click"
                                class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;"
                                visible="False">
                                <asp:Label runat="server">New</asp:Label>
                            </button>
                        </div>
                        <%-- <div class="btn-group" role="group" style="margin:0.5rem">
                            <button type="button" runat="server" id="btnExportExcel"
                                onserverclick="btnExportExcel_Click" class="btn btn-primary" title="Export / Import"
                                style="background:#FFFFFF;color:#3a4f63;">
                                <asp:Label runat="server">Export / Import Excel</asp:Label>
                            </button>
                    </div>--%>
                    <div class="btn-group" role="group" style="margin:0.5rem">
                        <button type="button" runat="server" id="btnImport" onserverclick="btnImport_Click"
                            class="btn btn-primary" title="Import" style="background:#FFFFFF;color:#3a4f63;"
                            visible="False">
                            <asp:Label runat="server">Import Excel</asp:Label>
                        </button>
                        <button type="button" runat="server" id="btnExport" onserverclick="btnExport_Click"
                            class="btn btn-primary" title="Export"
                            style="background:#FFFFFF;color:#3a4f63; display:none;">
                            <asp:Label runat="server">Export</asp:Label>
                        </button>
                    </div>
                </div>










            </div>



            </div>


            <div class="row">
                <div class="form-horizontal" style="margin:0 20px; width:100%;">
                    <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" OnItemDataBound="lstViewDetails_ItemDataBound" runat="server">
                        <LayoutTemplate>
                            <div class="table-responsive">
                                <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%"
                                    class="menutab table table-bordered">
                                    <thead>
                                        <tr role="row">
                                            <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1"
                                                colspan="1" aria-sort="ascending"
                                                aria-label="Rendering engine: activate to sort column descending">
                                                <b style="color: #414a4c">Item No</b>
                                            </th>
                                            <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1"
                                                colspan="1" aria-sort="ascending"
                                                aria-label="Rendering engine: activate to sort column descending">
                                                <b style="color: #414a4c">Item Name</b>
                                            </th>
                                            <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1"
                                                colspan="1" aria-sort="ascending"
                                                aria-label="Rendering engine: activate to sort column descending">
                                                <b style="color: #414a4c">Item Group</b>
                                            </th>
                                            <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1"
                                                colspan="1" aria-sort="ascending"
                                                aria-label="Rendering engine: activate to sort column descending">
                                                <b style="color: #414a4c">Major Group</b>
                                            </th>
                                            <%-- <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                rowspan="1" colspan="1" aria-sort="ascending"
                                                aria-label="Rendering engine: activate to sort column descending">
                                                <b style="color: #414a4c">Cost Centre</b>
                                                </th>--%>
                                                <%--<th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Price</b>
                                                    </th>--%>
                                                    <%-- <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Cost</b>
                                                        </th>--%>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Link</b>
                                                        </th>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Type</b>
                                                        </th>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Item Name</b>
                                                        </th>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Usage</b>
                                                        </th>
                                                        <th class="sorting_asc" id="thAction" runat="server" tabindex="0" aria-controls="example2"
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

                            <td>
                                <asp:Label ID="Label2" runat="server" Text=' <%# Eval("itemNo")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text=' <%# Eval("ItemName")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text=' <%# Eval("ignm")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text=' <%# Eval("mgnm")%>'></asp:Label>
                            </td>
                            <%-- <td>
                                <asp:Label ID="Label8" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                                </td>--%>
                                <%-- <td>
                                    <asp:Label ID="Label4" runat="server" Text=' <%# Eval("Price", "{0:#,0}")%>'>
                                    </asp:Label>
                                    </td>--%>
                                    <%-- <td>
                                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("Cost2", "{0:#,0}")%>'>
                                        </asp:Label>
                                        </td>--%>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text=' <%# Eval("linkname")%>'>
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text=' <%# Eval("Typename")%>'>
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text=' <%# Eval("dtlname")%>'>
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text=' <%# Eval("xusage")%>'>
                                            </asp:Label>
                                        </td>
                                        <td id="tdAction" runat="server" style="width: 8%; text-align: center">
                                            <%-- //====Here we have used CommandName property to distinguish which
                                                button is clicked and we have passed our primary key to CommandArgument
                                                property.====//--%>
                                                <asp:ImageButton ID="imgBtnEdit" CommandName="Edt"
                                                    ToolTip="Edit a record."
                                                    CommandArgument='<%#Eval("itemNo").ToString%>' runat="server"
                                                    ImageUrl="/images/edit.png" />
                                                <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                                                    ID="imgBtnDelete" CommandName="Del"
                                                    CommandArgument='<%#Eval("itemNo") %>' runat="server"
                                                    ImageUrl="~/images/delete.png"></asp:ImageButton>
                                        </td>




                                        </tr>
                        </ItemTemplate>
                    </asp:ListView>
                    <%--< /table>--%>

                        <br />
                </div>
                <%-- DataPager removed because pagination is now native to DataTables --%>

                    <div class="loading" style="text-align:center">
                        Loading... Please wait...<br />
                        <br />
                        <img src="/images/loader.gif" alt="" />
                    </div>

            </div>



            <script>

                if ($(window).width() >= 960) {
                    var fl = document.getElementById("<%= formload.ClientID %>");
                    if (fl) fl.style.width = "100%";
                } else {
                    var fl = document.getElementById("<%= formload.ClientID %>");
                    if (fl) fl.style.width = "50%";

                }


            </script>
    </asp:Content>