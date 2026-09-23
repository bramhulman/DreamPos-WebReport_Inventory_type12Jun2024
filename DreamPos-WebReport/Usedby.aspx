<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Usedby.aspx.vb" Inherits="DreamPos_WebReport.Usedby" %>
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

            /* Professional Table Header Design */
            .menutab thead th {
                background-color: #3a4f63 !important;
                color: #ffffff !important;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                font-size: 13px !important;
                font-weight: 600 !important;
                text-transform: uppercase;
                letter-spacing: 0.5px;
                text-align: center !important;
                padding: 15px 25px 15px 15px !important;
                border: 1px solid #2d3e4e !important;
                border-bottom: 3px solid #1a252f !important;
                vertical-align: middle !important;
            }

            .menutab tbody td.bg-recipe {
                background-color: #f4f9fc !important;
            }

            .menutab tbody td.bg-produced {
                background-color: #f2fbf9 !important;
            }

            .menutab tbody tr:hover td.bg-recipe {
                background-color: #e5f0fa !important;
            }

            .menutab tbody tr:hover td.bg-produced {
                background-color: #e0f2ef !important;
            }

            .menutab tbody td {
                padding: 10px 15px !important;
                vertical-align: middle !important;
                font-size: 14px !important;
                color: #333333 !important;
            }

            .menutab tbody tr:hover td {
                background-color: #f1f3f5 !important;
            }

            /* Custom White Sort Icons for DataTables */
            table.dataTable thead .sorting {
                background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMCIgaGVpZ2h0PSIxOSI+PHBhdGggZD0iTTUgMEwyIDEwaDZsLTMtMTB6TTUgMTlsMy0xMEgybDMgMTB6IiBmaWxsPSJyZ2JhKDI1NSwyNTUsMjU1LDAuMykiLz48L3N2Zz4=') !important;
            }

            table.dataTable thead .sorting_asc {
                background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMCIgaGVpZ2h0PSIxOSI+PHBhdGggZD0iTTUgMEwyIDEwaDZsLTMtMTB6IiBmaWxsPSIjZmZmIi8+PHBhdGggZD0iTTUgMTlsMy0xMEgybDMgMTB6IiBmaWxsPSJyZ2JhKDI1NSwyNTUsMjU1LDAuMykiLz48L3N2Zz4=') !important;
            }

            table.dataTable thead .sorting_desc {
                background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMCIgaGVpZ2h0PSIxOSI+PHBhdGggZD0iTTUgMEwyIDEwaDZsLTMtMTB6IiBmaWxsPSJyZ2JhKDI1NSwyNTUsMjU1LDAuMykiLz48cGF0aCBkPSJNNSAxOWwzLTEwSDJsMyAxMHoiIGZpbGw9IiNmZmYiLz48L3N2Zz4=') !important;
            }

            /* Show/Hide Columns (ColVis) Button Styling */
            div.dt-button-collection .dt-button.buttons-columnVisibility {
                background: #f8f9fa !important;
                color: #6c757d !important;
                border: 1px solid #dee2e6 !important;
                box-shadow: none !important;
                background-image: none !important;
                font-weight: normal !important;
                padding: 8px 12px !important;
                margin-bottom: 2px !important;
                text-align: left !important;
            }

            div.dt-button-collection .dt-button.buttons-columnVisibility:hover {
                background: #e2e6ea !important;
                color: #495057 !important;
            }

            /* Active (Shown) Columns */
            div.dt-button-collection .dt-button.buttons-columnVisibility.active {
                background: #3a4f63 !important;
                color: #ffffff !important;
                border: 1px solid #2d3e4e !important;
                font-weight: 600 !important;
            }

            div.dt-button-collection .dt-button.buttons-columnVisibility.active:hover {
                background: #2d3e4e !important;
                color: #ffffff !important;
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
        <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.colVis.min.js"></script>
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
            $(function () {
                $("#tblEmpDetails").DataTable({
                    "lengthMenu": [[5, 15, 25, 50, -1], [5, 15, 25, 50, "All"]],
                    "dom": "<'row'<'col-sm-4'l><'col-sm-4 text-center'B><'col-sm-4'f>>" +
                        "<'row'<'col-sm-12'tr>>" +
                        "<'row'<'col-sm-5'i><'col-sm-7'p>>",
                    "buttons": [
                        { extend: 'colvis', text: 'Show/Hide Columns' },
                        { extend: 'excelHtml5', title: 'Used By List' },
                        { extend: 'pdfHtml5', title: 'Used By List', orientation: 'landscape', pageSize: 'LEGAL' },
                        { extend: 'print', title: 'Used By List' }
                    ]
                });
            });

        </script>

        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0 text-dark">Used By</h1>
                    </div><!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Used By</li>
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
        <div class="row">
            <div class="form-inline" id="formload" runat="server" style="margin:0 20px; width:100%;">
            </div>
        </div>


        <div class="row">
            <div class="form-horizontal" style="margin:0 20px; width:100%;">
                <asp:ListView ID="lstViewDetails" runat="server">
                    <LayoutTemplate>
                        <div class="table-responsive">
                            <table id="tblEmpDetails" align="left" width="100%"
                                class="menutab table table-bordered table-striped table-hover">
                                <thead>
                                    <tr role="row">
                                        <th>Root Stock Number</th>
                                        <th>Root Stock Name</th>
                                        <th>Recipe L1 Code</th>
                                        <th>Recipe L1 Name</th>
                                        <th>Produced L1 Number</th>
                                        <th>Produced L1 Name</th>
                                        <th>Recipe L2 Code</th>
                                        <th>Recipe L2 Name</th>
                                        <th>Produced L2 Number</th>
                                        <th>Produced L2 Name</th>
                                        <th>Recipe L3 Code</th>
                                        <th>Recipe L3 Name</th>
                                        <th>Produced L3 Number</th>
                                        <th>Produced L3 Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </div>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("RootStockNumber")%>
                            </td>
                            <td>
                                <%# Eval("RootStockName")%>
                            </td>
                            <td class="bg-recipe">
                                <%# Eval("RecipeLevel1_Code")%>
                            </td>
                            <td class="bg-recipe">
                                <%# Eval("RecipeLevel1_Name")%>
                            </td>
                            <td class="bg-produced">
                                <%# Eval("ProducedStockLevel1_Number")%>
                            </td>
                            <td class="bg-produced">
                                <%# Eval("ProducedStockLevel1_Name")%>
                            </td>
                            <td class="bg-recipe">
                                <%# Eval("RecipeLevel2_Code")%>
                            </td>
                            <td class="bg-recipe">
                                <%# Eval("RecipeLevel2_Name")%>
                            </td>
                            <td class="bg-produced">
                                <%# Eval("ProducedStockLevel2_Number")%>
                            </td>
                            <td class="bg-produced">
                                <%# Eval("ProducedStockLevel2_Name")%>
                            </td>
                            <td class="bg-recipe">
                                <%# Eval("RecipeLevel3_Code")%>
                            </td>
                            <td class="bg-recipe">
                                <%# Eval("RecipeLevel3_Name")%>
                            </td>
                            <td class="bg-produced">
                                <%# Eval("ProducedStockLevel3_Number")%>
                            </td>
                            <td class="bg-produced">
                                <%# Eval("ProducedStockLevel3_Name")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <br />
            </div>

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