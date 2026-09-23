<%@ Page Title="Supplier Stock Mapping" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_SupplierStkSetting.aspx.vb" Inherits="DreamPos_WebReport.Pages_SupplierStkSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Select2 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <!-- DataTables CSS -->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/dataTables.bootstrap4.min.css" />
    
    <style type="text/css">
        .card-custom {
            border: none;
            border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
            margin-bottom: 30px;
            background: #ffffff;
            transition: all 0.3s ease;
        }
        .card-custom:hover {
            box-shadow: 0 6px 25px rgba(0,0,0,0.12);
        }
        .header-gradient {
            background: linear-gradient(135deg, #2c3e50 0%, #3498db 100%);
            color: white;
            border-top-left-radius: 12px;
            border-top-right-radius: 12px;
            padding: 18px 24px;
        }
        .header-gradient h3 {
            margin: 0;
            font-size: 1.25rem;
            font-weight: 600;
        }
        .nav-tabs-custom {
            border-bottom: 2px solid #e9ecef;
            margin-bottom: 20px;
        }
        .nav-tabs-custom .nav-link {
            border: none;
            color: #6c757d;
            font-weight: 600;
            padding: 12px 20px;
            transition: all 0.3s ease;
            position: relative;
        }
        .nav-tabs-custom .nav-link.active {
            color: #3498db;
            background: transparent;
        }
        .nav-tabs-custom .nav-link.active::after {
            content: '';
            position: absolute;
            bottom: -2px;
            left: 0;
            right: 0;
            height: 3px;
            background: #3498db;
            border-radius: 3px;
        }
        .select2-container .select2-selection--single {
            height: 38px !important;
            border: 1px solid #ced4da !important;
            border-radius: 6px !important;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 36px !important;
            color: #2c3e50 !important;
            padding-left: 15px !important;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 36px !important;
        }
        .btn-premium {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white !important;
            border: none;
            border-radius: 6px;
            padding: 8px 20px;
            font-weight: 600;
            box-shadow: 0 4px 15px rgba(52, 152, 219, 0.3);
            transition: all 0.3s ease;
        }
        .btn-premium:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(52, 152, 219, 0.4);
        }
        .btn-danger-custom {
            background: #ff7675;
            color: white !important;
            border: none;
            border-radius: 6px;
            padding: 6px 12px;
            font-weight: 500;
            transition: all 0.3s ease;
        }
        .btn-danger-custom:hover {
            background: #d63031;
            transform: scale(1.05);
        }
        .list-container {
            max-height: 400px;
            overflow-y: auto;
            border: 1px solid #e9ecef;
            border-radius: 6px;
            padding: 10px;
            background: #f8f9fa;
        }
        
        /* Pagination custom styles */
        .dataTables_wrapper .row { align-items: center; }
        div.dataTables_wrapper div.dataTables_paginate { margin: 0; white-space: nowrap; text-align: right; }
        .pagination { margin: 0 !important; }

        .custom-control-label {
            cursor: pointer;
            font-weight: 500;
            color: #2c3e50;
        }
        .custom-switch .custom-control-label::before {
            height: 1.5rem;
            width: 2.75rem;
            border-radius: 1rem;
        }
        .custom-switch .custom-control-label::after {
            width: calc(1.5rem - 4px);
            height: calc(1.5rem - 4px);
            border-radius: 1rem;
        }
        .custom-switch .custom-control-input:checked ~ .custom-control-label::before {
            background-color: #2ecc71;
            border-color: #2ecc71;
        }
        .search-box-list {
            border-radius: 20px;
            border: 1px solid #ced4da;
            padding: 8px 16px;
            margin-bottom: 12px;
            font-size: 0.9rem;
        }

        /* --- jQuery DataTables Pagination Layout Correction --- */
        #tblMappings_wrapper .pagination {
            display: flex !important;
            flex-wrap: nowrap !important;
            justify-content: flex-end !important;
            padding-left: 0 !important;
            list-style: none !important;
            border-radius: 0.25rem !important;
            width: auto !important;
            margin: 0 !important;
        }
        #tblMappings_wrapper .pagination .paginate_button {
            padding: 0 !important;
            margin: 0 !important;
            border: none !important;
            background: none !important;
            background-color: transparent !important;
            box-shadow: none !important;
        }
        #tblMappings_wrapper .pagination .page-item {
            display: inline-block !important;
            width: auto !important;
            flex: none !important;
            margin: 0 !important;
        }
        #tblMappings_wrapper .pagination .page-item .page-link {
            position: relative !important;
            display: block !important;
            padding: 0.35rem 0.65rem !important;
            margin-left: -1px !important;
            line-height: 1.25 !important;
            color: #007bff !important;
            background-color: #fff !important;
            border: 1px solid #dee2e6 !important;
            width: auto !important;
            flex: none !important;
        }
        #tblMappings_wrapper .pagination .page-item:first-child .page-link {
            margin-left: 0 !important;
            border-top-left-radius: 0.25rem !important;
            border-bottom-left-radius: 0.25rem !important;
        }
        #tblMappings_wrapper .pagination .page-item:last-child .page-link {
            border-top-right-radius: 0.25rem !important;
            border-bottom-right-radius: 0.25rem !important;
        }
        #tblMappings_wrapper .pagination .page-item.active .page-link {
            z-index: 3 !important;
            color: #fff !important;
            background-color: #007bff !important;
            border-color: #007bff !important;
        }
        #tblMappings_wrapper .pagination .page-item.disabled .page-link {
            color: #6c757d !important;
            pointer-events: none !important;
            cursor: auto !important;
            background-color: #fff !important;
            border-color: #dee2e6 !important;
        }
        #tblMappings_wrapper .dataTables_paginate {
            margin: 0 !important;
            white-space: nowrap !important;
            text-align: right !important;
            display: flex !important;
            justify-content: flex-end !important;
        }
        #tblMappings_wrapper .dataTables_info {
            white-space: nowrap !important;
            padding-top: 0.65em !important;
            color: #2c3e50 !important;
            font-weight: 500 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <!-- Script Manager -->
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <!-- Content Header -->
    <div class="content-header">
        <div class="container-fluid">
            <div class="row mb-3">
                <div class="col-sm-6">
                    <h1 class="m-0 text-dark" style="font-weight: 700; color: #2c3e50;">
                        <i class="fas fa-handshake mr-2" style="color: #3498db;"></i>Supplier Stock Relationship
                    </h1>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item active">Supplier Stock Mapping</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <!-- Alert Messages -->
    <div style="margin: 0 20px;">
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3" Font-Bold="true"></asp:Label>
    </div>

    <div class="container-fluid" style="padding: 0 20px;">
        <div class="row">
            <!-- Left Panel: Interactive Configurator -->
            <div class="col-lg-5">
                <div class="card card-custom">
                    <div class="header-gradient">
                        <h3><i class="fas fa-sliders-h mr-2"></i>Relationship Configurator</h3>
                    </div>
                    <div class="card-body">
                        <!-- Navigation Tabs -->
                        <ul class="nav nav-tabs nav-tabs-custom" id="configTabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="by-supplier-tab" data-toggle="tab" href="#by-supplier" role="tab" aria-controls="by-supplier" aria-selected="true">
                                    <i class="fas fa-truck mr-1"></i>By Supplier
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="by-stock-tab" data-toggle="tab" href="#by-stock" role="tab" aria-controls="by-stock" aria-selected="false">
                                    <i class="fas fa-boxes mr-1"></i>By Stock Item
                                </a>
                            </li>
                        </ul>

                        <!-- Tab Content -->
                        <div class="tab-content" id="configTabContent">
                            <!-- Tab: By Supplier -->
                            <div class="tab-pane fade show active" id="by-supplier" role="tabpanel" aria-labelledby="by-supplier-tab">
                                <asp:UpdatePanel ID="upBySupplier" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-group mb-4">
                                            <label class="font-weight-bold" style="color: #2c3e50;">Select Supplier:</label>
                                            <asp:DropDownList ID="ddlSupplierSearch" runat="server" CssClass="form-control select2-search" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplierSearch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="mb-2 d-flex justify-content-between align-items-center">
                                            <label class="font-weight-bold" style="color: #2c3e50; margin: 0;">Map Stock Items:</label>
                                            <span class="badge badge-info" id="cntItems">0 selected</span>
                                        </div>

                                        <input type="text" class="form-control search-box-list" id="searchItems" placeholder="Search stock items..." onkeyup="filterList('searchItems', 'itemList')" />

                                        <div class="list-container" id="itemList">
                                            <asp:CheckBoxList ID="cblStockItems" runat="server" CssClass="custom-control custom-switch custom-checkbox-list" RepeatLayout="UnorderedList">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="mt-4 text-right">
                                            <asp:Button ID="btnSaveBySupplier" runat="server" Text="Save Supplier Mappings" CssClass="btn btn-premium btnstyle" OnClick="btnSaveBySupplier_Click" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierSearch" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                            <!-- Tab: By Stock Item -->
                            <div class="tab-pane fade" id="by-stock" role="tabpanel" aria-labelledby="by-stock-tab">
                                <asp:UpdatePanel ID="upByStock" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-group mb-4">
                                            <label class="font-weight-bold" style="color: #2c3e50;">Select Stock Item:</label>
                                            <asp:DropDownList ID="ddlStockSearch" runat="server" CssClass="form-control select2-search" AutoPostBack="true" OnSelectedIndexChanged="ddlStockSearch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="mb-2 d-flex justify-content-between align-items-center">
                                            <label class="font-weight-bold" style="color: #2c3e50; margin: 0;">Map Suppliers:</label>
                                            <span class="badge badge-info" id="cntSuppliers">0 selected</span>
                                        </div>

                                        <input type="text" class="form-control search-box-list" id="searchSuppliers" placeholder="Search suppliers..." onkeyup="filterList('searchSuppliers', 'supplierList')" />

                                        <div class="list-container" id="supplierList">
                                            <asp:CheckBoxList ID="cblSuppliers" runat="server" CssClass="custom-control custom-switch custom-checkbox-list" RepeatLayout="UnorderedList">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="mt-4 text-right">
                                            <asp:Button ID="btnSaveByStock" runat="server" Text="Save Item Mappings" CssClass="btn btn-premium btnstyle" OnClick="btnSaveByStock_Click" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlStockSearch" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Right Panel: DataTables Mapping Grid -->
            <div class="col-lg-7">
                <div class="card card-custom">
                    <div class="header-gradient" style="background: linear-gradient(135deg, #1abc9c 0%, #2ecc71 100%);">
                        <h3><i class="fas fa-clipboard-list mr-2"></i>Active Relationships Overview</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <table id="tblMappings" class="table table-striped table-bordered" style="width: 100%;">
                                        <thead>
                                            <tr style="background-color: #2c3e50; color: white;">
                                                <th>Supplier Name</th>
                                                <th>Stock Code</th>
                                                <th>Stock Item Name</th>
                                                <th style="text-align: center; width: 80px;">Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptMappings" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="font-weight: 600; color: #2c3e50;"><%# Eval("supplier_name") %></td>
                                                        <td><span class="badge badge-secondary" style="font-size: 0.85rem;"><%# Eval("stk_number") %></span></td>
                                                        <td><%# Eval("stk_name") %></td>
                                                        <td style="text-align: center;">
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn-danger-custom btn btn-sm"
                                                                CommandName="DeleteMapping"
                                                                CommandArgument='<%# Eval("supplier_id") & ";" & Eval("stk_id") %>'
                                                                OnClientClick="return confirm('Are you sure you want to delete this relationship?');">
                                                                <i class="fas fa-trash-alt"></i>
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Spinner Loader -->
    <div id="spinner" class="loading" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); z-index: 9999; background: #ffffff; padding: 30px 40px; border-radius: 12px; box-shadow: 0 15px 35px rgba(0,0,0,0.2); text-align: center;">
        <div style="font-size: 3rem; color: #3498db; margin-bottom: 15px;">
            <i class="fas fa-circle-notch fa-spin"></i>
        </div>
        <div style="font-family: 'Segoe UI', sans-serif; font-size: 1.2rem; color: #333; font-weight: 600;">Processing...</div>
    </div>

    <!-- Scripts -->
    <!-- Select2 JS -->
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <!-- DataTables JS -->
    <script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.11.5/js/dataTables.bootstrap4.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            initializePlugins();

            // Intercept async postback to reinitialize plugins
            if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    initializePlugins();
                    $('#spinner').hide();
                });
            }

            $('.btnstyle').click(function () {
                $('#spinner').show();
            });
        });

        function initializePlugins() {
            // Initialize Select2 dropdowns
            $('.select2-search').select2({
                placeholder: "Select...",
                allowClear: true,
                width: '100%'
            });

            // Initialize jQuery DataTables
            if ($.fn.DataTable.isDataTable('#tblMappings')) {
                $('#tblMappings').DataTable().destroy();
            }

            $('#tblMappings').DataTable({
                "pageLength": 10,
                "ordering": true,
                "responsive": true,
                "dom": '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"row align-items-center mt-2"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
                "language": {
                    "search": "Filter Mappings:",
                    "lengthMenu": "Show _MENU_ records",
                    "info": "Displaying _START_ to _END_ of _TOTAL_ mappings",
                    "paginate": {
                        "first": "First",
                        "last": "Last",
                        "next": "Next >",
                        "previous": "< Prev"
                    }
                }
            });

            // Format switch inputs to style nicely
            $('.custom-checkbox-list li').each(function () {
                var container = $(this);
                container.addClass('custom-control custom-switch mb-2');
                var checkbox = container.find('input[type="checkbox"]');
                var label = container.find('label');

                checkbox.addClass('custom-control-input');
                var uniqueId = checkbox.attr('id');
                label.attr('for', uniqueId);
                label.addClass('custom-control-label pl-2');
            });

            updateCounts();
        }

        // Live filtration search for list containers
        function filterList(searchId, listId) {
            var input = document.getElementById(searchId);
            var filter = input.value.toUpperCase();
            var list = document.getElementById(listId);
            var li = list.getElementsByTagName("li");

            for (var i = 0; i < li.length; i++) {
                var label = li[i].getElementsByTagName("label")[0];
                if (label) {
                    var txtValue = label.textContent || label.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        li[i].style.display = "";
                    } else {
                        li[i].style.display = "none";
                    }
                }
            }
        }

        function updateCounts() {
            var selectedItems = $('#itemList input[type="checkbox"]:checked').length;
            $('#cntItems').text(selectedItems + " selected");

            var selectedSuppliers = $('#supplierList input[type="checkbox"]:checked').length;
            $('#cntSuppliers').text(selectedSuppliers + " selected");
        }

        // Listen for switch changes
        $(document).on('change', '.custom-checkbox-list input[type="checkbox"]', function () {
            updateCounts();
        });
    </script>
</asp:Content>
