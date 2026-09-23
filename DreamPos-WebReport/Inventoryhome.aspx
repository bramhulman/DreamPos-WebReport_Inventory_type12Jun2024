<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Inventoryhome.aspx.vb" Inherits="DreamPos_WebReport.Inventoryhome" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
            <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.dataTables.min.css" />
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            <script>
                $(document).on('submit', 'form', function () {
                    ShowProgress();
                });
            </script>
        </asp:Content>




        <asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
            <asp:ScriptManager runat="server">

            </asp:ScriptManager>
            <style type="text/css">
                /* ========================================
               GLOBAL TABLE STYLES
            ======================================== */
                .style1 {
                    color: #fff;
                    background: #3a4f63;
                }

                .hidden-col {
                    display: none !important;
                }

                .text-end {
                    text-align: right !important;
                }

                .dataTables_processing {
                    display: none !important;
                }

                /* ========================================
               LOADING MODAL
            ======================================== */
                .modal {
                    position: fixed;
                    top: 0;
                    left: 0;
                    background-color: black;
                    z-index: 99;
                    opacity: 0.8;
                    min-height: 100%;
                    width: 100%;
                }

                .loading {
                    font-family: Arial;
                    font-size: 10pt;
                    border: 5px solid #67CFF5;
                    width: 200px;
                    height: 100px;
                    display: none;
                    position: fixed;
                    background-color: #fff;
                    z-index: 999;
                }

                /* ========================================
               GROUP HEADER (FINAL CLEAN VERSION)
            ======================================== */
                .group-row-hidden {
                    display: none !important;
                }

                .group-header {
                    background: #f0f4ff !important;
                    font-weight: bold;
                    border-left: 6px solid #4361ee !important;
                    cursor: pointer;
                    user-select: none;
                    transition: all 0.2s;
                }

                .group-header:hover {
                    background: #e1ecff !important;
                }

                .group-header i {
                    transition: transform 0.2s ease;
                    display: inline-block;
                }

                .group-header td {
                    padding: 12px 14px !important;
                }

                .group-header.group-collapsed i {
                    transform: rotate(0deg);
                }

                .group-header .group-title {
                    font-size: 16px;
                }

                .group-header .group-summary {
                    font-size: 13px;
                    color: #333;
                    margin-left: 28px;
                }

                .group-collapsed {
                    opacity: 0.6;
                }

                /* Smooth animation for collapse */
                .animate-slide {
                    transition: all 0.25s ease-out;
                }

                /* ========================================
               GROUP ITEM ROWS
            ======================================== */
                .group-item td {
                    background-color: #f8f9fa !important;
                }

                .group-item.warning-row {
                    background-color: #f08080 !important;
                }

                /* ========================================
               STOCK STATUS COLORS (Unified)
            ======================================== */
                .table-success {
                    background-color: #d9f7d9 !important;
                }

                .table-warning {
                    background-color: #fff3cd !important;
                }

                .table-danger {
                    background-color: #f8d7da !important;
                }

                /* Semua row item default hidden */
                .group-item {
                    display: none !important;
                }

                /* RESTORE ORIGINAL HEADER FOR THIS PAGE */
                #<%=gvHome.ClientID %> thead th {
                    background-color: SteelBlue !important;
                    color: white !important;
                }
                .hidden-col {
                    display: none !important;
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
                        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                        loading.css({ top: top, left: left });
                    }, 200);
                }

            </script>
            <script type="text/javascript">
                function DisableButton() {
                    document.getElementById("<%=RadButton3.ClientID %>").disabled = true;
                }

                window.onbeforeunload = DisableButton;
            </script>

            <%-- <asp:HiddenField ID="hidField" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hfposted" Value="0" runat="server" />--%>
            <%--<asp:Button ID="btnDummy" runat="server" Text="Edit" Style="display: none;" />--%>
            <button type="button" runat="server" data-toggle="modal" id="btnshowmdlprocess" data-target="#ModalProcess"
                style="display: none;"></button>
            <section class="content-header">
                <div class="container-fluid">
                    <div class="row mb-3 align-items-center">
                        <div class="col-sm-6">
                            <h1 class="m-0 text-dark" style="font-weight: 700; color: #04274f !important;">
                                <asp:Label ID="lblTitle" runat="server" Text="Inventory Stock"></asp:Label>
                            </h1>
                        </div>
                        <div class="col-sm-6">
                            <ol class="breadcrumb float-sm-right bg-transparent p-0 m-0">
                                <li class="breadcrumb-item"><a href="/inventoryHome.aspx"
                                        style="color: #007bff;">Home</a></li>
                                <li class="breadcrumb-item active">Inventory Stock</li>
                            </ol>
                        </div>
                    </div>

                    <!-- MODERN STATUS CARD -->
                    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="row">
                        <div class="col-md-12">
                            <div class="card shadow-sm border-left-primary" style="border-left: 5px solid #007bff;">
                                <div
                                    class="card-body p-3 d-flex align-items-center justify-content-between bg-white rounded">
                                    <div style="flex-grow: 1; margin-left: 20px;">
                                        <asp:Label ID="alert1" runat="server"
                                            CssClass="d-block font-weight-bold text-danger text-lg" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                </div><!-- /.container-fluid -->
            </section>

            <div class="card shadow rounded-lg border-0">


                <div class="card-body" style="background-color: #f8f9fa;">

                    <div class="table-responsive bg-white p-3 rounded shadow-sm">
                        <asp:UpdatePanel ID="upCrudGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!-- === FILTER ROW MODERN === -->
                                <div class="row mb-4 bg-light p-3 rounded border">
                                    <div class="col-md-3">
                                        <label class="form-label fw-bold text-primary"><i
                                                class="fas fa-store-alt mr-1"></i>Cost Centre</label>
                                        <asp:DropDownList ID="ddlCcFilter" runat="server"
                                            CssClass="form-control form-control-sm shadow-sm" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label fw-bold text-primary"><i
                                                class="fas fa-layer-group mr-1"></i>Stock Group</label>
                                        <asp:DropDownList ID="ddlGrpFilter" runat="server"
                                            CssClass="form-control form-control-sm shadow-sm" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label fw-bold text-primary"><i class="fas fa-filter mr-1"></i>Status</label>
                                        <select id="ddlStatusFilter" class="form-control form-control-sm shadow-sm">
                                            <option value="">-- All Status --</option>
                                            <option value="normal">Normal (Safe)</option>
                                            <option value="warning">Warning (Low)</option>
                                            <option value="critical">Critical (Very Low)</option>
                                            <option value="outofstock">Out of Stock</option>
                                        </select>
                                    </div>
                                    <div class="col-md-3 d-flex align-items-end" style="padding-bottom: 2px;">
                                        <button type="button" id="btnToggleGroup" class="btn btn-secondary btn-sm shadow-sm">
                                            <i class="fas fa-object-group mr-1"></i> <span>Enable Grouping</span>
                                        </button>
                                    </div>
                                </div>




                                <div class="table-responsive">
                                    <asp:GridView ID="gvHome" runat="server" Width="99%" AutoGenerateColumns="False" EnableViewState="false"
                                        DataKeyNames="custid,ccid,stkid,ccname,stkname,sgname,ttlqty,ttlqty_with_unit,cost,Total,stkLimit,unit2,stat"
                                        CssClass="table table-striped table-hover table-bordered"
                                        HeaderStyle-BackColor="SteelBlue" HeaderStyle-ForeColor="White"
                                        ShowHeaderWhenEmpty="True"
                                        OnPreRender="gvHome_PreRender">

                                        <Columns>

                                            <asp:BoundField DataField="ccname" HeaderText="Cost Center"
                                                SortExpression="ccname" />

                                            <asp:BoundField DataField="stkname" HeaderText="Item"
                                                SortExpression="stkname" />

                                            <asp:BoundField DataField="sgname" HeaderText="Stock Group"
                                                SortExpression="sgname" />

                                            <asp:BoundField DataField="ttlqty_with_unit" HeaderText="Total Stock"
                                                SortExpression="ttlqty" ItemStyle-HorizontalAlign="Right" />

                                            <asp:BoundField DataField="ttlqty" HeaderText="ttlqty_raw"
                                                ItemStyle-CssClass="hidden-col" HeaderStyle-CssClass="hidden-col" HtmlEncode="false" />

                                            <asp:BoundField DataField="cost" HeaderText="Cost" SortExpression="cost"
                                                ItemStyle-HorizontalAlign="Right" />

                                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"
                                                ItemStyle-HorizontalAlign="Right" />

                                            <asp:BoundField DataField="stkLimit" HeaderText="Stock Limit"
                                                SortExpression="stkLimit" ItemStyle-HorizontalAlign="Right" />


                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <!-- /.card-body -->
                        <div class="card-footer">
                            <%--Report Menu--%>
                        </div>
                        <!-- /.card-footer-->
                    </div>




                    <!-- Modal proses -->
                    <asp:UpdatePanel ID="UpdatePanel2" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>


                            <div class="modal fade" id="ModalProcess" tabindex="-1" role="dialog"
                                aria-labelledby="ModalProcess" aria-hidden="true" data-backdrop="false"
                                style="opacity:1;z-index:1050;">
                                <div class="modal-dialog modal-dialog-scrollable" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="ModalProcessLabel">
                                                <asp:label runat="server" id="openlabel"
                                                    Text="There is no Opening Stock !!!"></asp:label>
                                            </h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span>
                                            </button>

                                        </div>

                                        <div class="row">
                                            <div class="form-horizontal" id="formload" runat="server"
                                                style="width:100%;">
                                                <div class="card-body">



                                                    <div class="form-group row">
                                                        <asp:Label runat="server"
                                                            CssClass="col-sm-2 col-form-label text-nowrap"
                                                            Text="Do you want to Calculate now?"></asp:Label>



                                                    </div>
                                                </div>

                                            </div>



                                        </div>

                                        <div class="modal-body">

                                        </div>
                                        <div class="modal-footer" style="align-items:center; align-content:center">
                                            <%--<button type="button" class="btn btn-secondary"
                                                data-dismiss="modal">NO</button>--%>
                                                <%-- <button type="button" class="btn btn-primary" runat="server"
                                                    onserverclick="btnSubmit2_Click">Select</button>
                                                    <button type="button" id="btnAll2" runat="server"
                                                        class="btn btn-primary"
                                                        onserverclick="btnAll2_Click">SelectAll</button>--%>
                                                    <button type="button" class="btn btn-primary" runat="server"
                                                        onserverclick="btnSubmit2_Click" id="RadButton3">YES</button>
                                                    <%--<button type="button" class="btn btn-primary" runat="server"
                                                        onserverclick="btnAll3_ServerClick">SelectAll</button>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!-- Modal proses -->
                    <script>
                        function OnKeyPress(e) {
                            if (e.keyCode == "27") {
                                $find("ModalProcess").hide();


                            }
                        }

                        /*Update 21 Februari 2020*/
                        function showPopup() {
                            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
                            modalPopupBehavior.show();
                        }
                        function hidepopup() {
                            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
                            modalPopupBehavior.hide();
                        }
                        /*Update 21 Februari 2020*/

                        function openModalProcess() {
                            $('#ModalProcess').modal({ show: true });
                            //document.getElementById("txtsearch2").focus();
                        }

                    </script>
                    <!-- 4. DataTables Buttons (Untuk Export, Opsional) -->
                    <script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
                    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
                    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
                    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
                    <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
                    <script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>
                    <!-- DataTables Responsive (Modern Mobile Support) -->
                    <script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>

                    <!-- pace -->
                    <script>
                        // Matikan jvectormap kalau conflict
                        if (typeof $.fn.vectorMap !== 'undefined') {
                            $.fn.vectorMap = null;
                        }
                    </script>

                    <script type="text/javascript">
                        var table = null;
                        var collapsedGroups = {};     // GLOBAL — jangan di dalam function!
                        var defaultCollapse = true;   // semua grup default collapsed
                        // VARIABEL GLOBAL UNTUK MENYIMPAN FILTER GRUP
                        var currentGroupFilter = null;
                        var enableGrouping = false;

                        // FUNGSI FILTER GRUP (DI LUAR SEMUA FUNCTION!)
                        function groupFilterFunction(settings, data, dataIndex) {
                            if (settings.nTable !== table.table().node()) return true;
                            if (!currentGroupFilter) return true;
                            return data[2] === currentGroupFilter;
                        }

                        function parseNumber(value) {
                            if (typeof value === 'string') {
                                value = value.replace(/\./g, '').replace(',', '.');
                                return parseFloat(value) || 0;
                            } else if (typeof value === 'number') {
                                return value;
                            } else {
                                return 0;
                            }
                        }
                        // FUNGSI BARU — KHUSUS UNTUK sum() plugin
                        function cleanNumber(str) {
                            if (!str) return 0;
                            return parseFloat(String(str).replace(/[^\d.-]/g, '')) || 0;
                        }

                        function initializeDataTable() {
                            var tableId = '#<%= gvHome.ClientID %>';

                            if ($.fn.DataTable.isDataTable(tableId)) {
                                $(tableId).DataTable().destroy();
                            }
                            // PLUGIN sum() — VERSI TERBAIK & BENAR UNTUK FORMAT INDONESIA
                            jQuery.fn.dataTable.Api.register('sum()', function () {
                                return this.flatten().reduce(function (a, b) {
                                    var x = typeof a === 'string' ? cleanNumber(a) : a;
                                    var y = typeof b === 'string' ? cleanNumber(b) : b;
                                    return x + y;
                                }, 0);
                            });
                            $.fn.dataTable.ext.type.detect.unshift(function (d) {
                                if (typeof d === 'string' && d.match(/^-?\d{1,3}(\.\d{3})*(,\d+)?$/)) {
                                    return 'indonumber';
                                }
                                return null;
                            });
                            $.fn.dataTable.ext.type.order['indonumber-pre'] = function (d) {
                                if (!d) return 0;

                                // Hapus . (thousand), ubah , menjadi .
                                return parseFloat(d.replace(/\./g, '').replace(',', '.')) || 0;
                            };

                            setTimeout(function () {
                                table = $(tableId).DataTable({
                                    destroy: true,
                                    paging: true,
                                    searching: true,
                                    info: true,
                                    pageLength: 25,
                                    lengthMenu: [[25, 50, 100, 200, -1], [25, 50, 100, 200, "All"]],
                                    dom: 'l<"clear">frtip', // agar info tetap muncul
                                    orderFixed: { pre: [2, 'asc'] },
                                    order: [[2, 'asc']], // PASTIKAN urut berdasarkan Stock Group!
                                    responsive: true,
                                    deferRender: true,
                                    processing: false,
                                    searchDelay: 500,
                                    columnDefs: [
                                        { targets: 1, responsivePriority: 1 }, // Item Name (Prioritas 1: selalu tampil)
                                        { targets: 3, responsivePriority: 2, className: 'text-end' }, // Total Stock (Prioritas 2: tampil di mobile)
                                        { targets: 0, responsivePriority: 3 }, // Cost Center
                                        { targets: 6, responsivePriority: 4, type: 'indonumber', className: 'text-end' }, // Total Value
                                        { targets: 5, responsivePriority: 5, type: 'indonumber', className: 'text-end' }, // Cost
                                        { targets: 2, responsivePriority: 6 }, // Stock Group
                                        { targets: 7, responsivePriority: 7, type: 'indonumber', className: 'text-end' }, // Stock Limit
                                        { targets: 4, visible: false, searchable: false, responsivePriority: 100 } // ttlqty_raw hidden
                                    ],
                                    // Fix info "NaN to NaN" saat show all
                                    drawCallback: function () {
                                        var api = this.api();
                                        var info = api.page.info();
                                        var text = '';
                                        if (info.length === -1) {
                                             text = 'Showing all ' + info.recordsDisplay + ' entries';
                                        } else {
                                            var start = info.recordsDisplay === 0 ? 0 : info.start + 1;
                                            text = 'Showing ' + start + ' to ' + info.end + ' of ' + info.recordsDisplay + ' entries';
                                        }
                                        if (info.recordsTotal !== info.recordsDisplay) {
                                            text += ' (filtered from ' + info.recordsTotal + ' total entries)';
                                        }
                                        $(api.table().container()).find('.dataTables_info').html(text);
                                    }
                                });

                                // FILTERS (tetap sama)
                                $('#<%= ddlCcFilter.ClientID %>').off('change').on('change', function () {
                                    table.column(0).search(this.value ? '^' + this.value + '$' : '', true, false).draw();
                                });


                                // GANTI KODE FILTER STOCK GROUP JADI INI
                                $('#<%= ddlGrpFilter.ClientID %>').off('change').on('change', function () {
                                    currentGroupFilter = this.value || null;

                                    // Reset filter bawaan
                                    table.search('').columns().search('');

                                    // Hapus semua custom filter dulu
                                    $.fn.dataTable.ext.search = $.fn.dataTable.ext.search.filter(function (fn) {
                                        return fn !== groupFilterFunction;
                                    });

                                    // Tambah filter grup jika ada pilihan
                                    if (currentGroupFilter) {
                                        $.fn.dataTable.ext.search.push(groupFilterFunction);
                                    }

                                    table.draw();
                                });
                                $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
                                    if (!table || settings.nTable !== table.table().node()) return true;
                                    var sel = $('#ddlStatusFilter').val();
                                    if (!sel) return true;
                                    var rawQty = data[3] || "";
                                    var qty = parseFloat(rawQty.replace(/[^0-9.-]/g, "")) || 0;
                                    var lim = parseFloat(data[7]) || 0;
                                    var st = (lim <= 0) ? "nolimit"
                                        : (qty <= 0) ? "outofstock"
                                            : (qty <= lim * 0.3) ? "critical"
                                                : (qty <= lim * 0.7) ? "warning"
                                                    : "normal";
                                    return st === sel;
                                });
                                $('#ddlStatusFilter').off('change').on('change', function () { table.draw(); });
                                $('#btnResetFilter').off('click').on('click', function () {
                                    $('#<%= ddlCcFilter.ClientID %>').val('');
                                    $('#<%= ddlGrpFilter.ClientID %>').val('');
                                    $('#ddlStatusFilter').val('');

                                    currentGroupFilter = null;
                                    // Reset semua custom filter
                                    $.fn.dataTable.ext.search = [];
                                    table.search('').columns().search('').draw();
                                });

                                $('#btnToggleGroup').off('click').on('click', function () {
                                    var $btn = $(this);
                                    enableGrouping = !enableGrouping;
                                    
                                    // Ubah UI seketika
                                    if (enableGrouping) {
                                        $btn.find('span').text('Disable Grouping');
                                        $btn.removeClass('btn-secondary').addClass('btn-primary');
                                        // Paksa show all jika grouping agar semua grup terlihat
                                        table.page.len(-1);
                                    } else {
                                        $btn.find('span').text('Enable Grouping');
                                        $btn.removeClass('btn-primary').addClass('btn-secondary');
                                    }
                                    
                                    // Tunda proses drawing yang berat agar tombol berubah warna dulu
                                    setTimeout(function() {
                                        table.draw();
                                    }, 50);
                                });

                                table.on('draw', function () {
                                    var api = table;
                                    var colspan = api.columns(':visible').count();
                                    var body = $(api.table().body());

                                    // HAPUS SEMUA HEADER LAMA
                                    body.find('.group-header').remove();

                                    if (!enableGrouping) {
                                        api.rows({ page: 'current' }).every(function () {
                                            $(this.node()).removeClass('group-row-hidden');
                                        });
                                        return; // Keluar, jangan jalankan grouping
                                    }

                                    // Paksa halaman menampilkan semua data jika grouping aktif
                                    if (api.page.len() !== -1) {
                                        api.page.len(-1).draw();
                                        return;
                                    }

                                    // AMBIL DATA YANG SUDAH DIFILTER SEKALI SAJA (CEPAT & AKURAT!)
                                    var filteredData = api.rows({ search: 'applied' }).data().toArray();

                                    var lastGroup = null;

                                    var searchTerm = api.search().trim();
                                    if (window.lastSearchTerm !== searchTerm) {
                                        collapsedGroups = {}; 
                                        window.lastSearchTerm = searchTerm;
                                    }

                                    api.rows({ page: 'current' }).every(function () {
                                        var data = this.data();
                                        var group = data[2];
                                        if (!group) return;

                                        // Set expand automatically if searching, unless explicitly collapsed
                                        if (searchTerm !== '' && collapsedGroups[group] === undefined) {
                                            collapsedGroups[group] = false; 
                                        }

                                        if (group !== lastGroup) {
                                            // HITUNG TOTAL DARI filteredData (SEMUA DATA YANG DIFILTER!)
                                            var groupData = filteredData.filter(r => r[2] === group);
                                            var itemCount = groupData.length;

                                            // TOTAL STOCK: dari hidden column ttlqty_raw (index 4)
                                            var totalStock = groupData.reduce(function(sum, r) { 
                                                return sum + (parseFloat(r[4]) || 0); 
                                            }, 0);

                                            var totalValue = groupData.reduce(function (sum, r) {
                                                return sum + parseNumber(r[6]);
                                            }, 0);

                                            var isCollapsed = (collapsedGroups[group] !== undefined) ? collapsedGroups[group] : defaultCollapse;

                                            var $header = $(`
                            <tr class="group-header ${isCollapsed ? 'group-collapsed' : ''}" data-group="${group}">
                                <td colspan="${colspan}" style="background:#f0f4ff; padding:14px 16px; border-left:6px solid #4361ee; cursor:pointer; font-weight:bold; user-select:none;">
                                    <i class="fas ${isCollapsed ? 'fa-chevron-right' : 'fa-chevron-down'} me-3" style="font-size:16px; width:20px;"></i>
                                    <strong style="font-size:16px;">${group}</strong>
                                    <span class="text-muted ms-3">
                                        (${itemCount} items)
                                        &bull; Stock: <strong class="text-primary">${totalStock.toLocaleString('id-ID', { minimumFractionDigits: 0, maximumFractionDigits: 4 })}</strong>
                                        &bull; Value: <strong class="text-danger">Rp ${totalValue.toLocaleString('id-ID')}</strong>
                                    </span>
                                </td>
                            </tr>
                        `);

                                            var node = this.node();
                                            if (node) {
                                                $(node).before($header);
                                            }
                                            lastGroup = group;
                                        }

                                        // TAMPILKAN ATAU SEMBUNYIKAN BARIS
                                        var shouldShow = (collapsedGroups[group] !== undefined) ? !collapsedGroups[group] : !defaultCollapse;
                                        $(this.node()).toggleClass('group-row-hidden', !shouldShow);
                                    });
                                });

                                // KLIK HEADER — TOGGLE SEMUA
                                $(document).off('click', '.group-header').on('click', '.group-header', function () {
                                    var group = $(this).data('group');
                                    collapsedGroups[group] = !(collapsedGroups[group] ?? defaultCollapse);

                                    var isCollapsed = collapsedGroups[group];

                                    // Update semua header yang grupnya sama, tanpa menggunakan selector CSS rumit
                                    $(table.table().body()).find('.group-header').filter(function () {
                                        return $(this).data('group') === group;
                                    }).toggleClass('group-collapsed', isCollapsed)
                                      .find('i').toggleClass('fa-chevron-right fa-chevron-down');

                                    // Update semua baris dari grup ini
                                    table.rows().every(function () {
                                        if (this.data() && this.data()[2] === group) {
                                            $(this.node()).toggleClass('group-row-hidden', isCollapsed);
                                        }
                                    });
                                });

                            }, 600);
                        }

                        $(document).ready(function () {
                            initializeDataTable();
                        });
                        function pageLoad() {
                            initializeDataTable();
                        }
                    </script>

            <!-- ========================================================
                 FLOATING COST ANOMALY BUBBLE NOTIFICATION
            ======================================================== -->
            <div id="costAnomalyContainer" class="cost-anomaly-container" style="display:none;">
                <!-- Minimized Floating Action Button/Pill (pulsing alert) -->
                <button type="button" id="btnAnomalyBadge" class="btn btn-danger btn-anomaly-floating shadow-lg" onclick="toggleAnomalyCard(true)">
                    <i class="fas fa-exclamation-triangle mr-1"></i>
                    <span id="lblAnomalyCountBadge">0</span> Cost Anomalies
                </button>

                <!-- Full Anomaly Detail Card (Displayed by default until closed) -->
                <div id="cardAnomalyBox" class="card shadow-lg border-danger card-anomaly-box">
                    <div class="card-header bg-danger text-white d-flex align-items-center justify-content-between p-2">
                        <div class="d-flex align-items-center">
                            <i class="fas fa-exclamation-triangle fa-lg mr-2"></i>
                            <strong>Cost Anomaly Warning (<span id="lblAnomalyCount">0</span>)</strong>
                        </div>
                        <button type="button" class="btn btn-sm btn-outline-light py-0 px-2 font-weight-bold" onclick="toggleAnomalyCard(false)" title="Dismiss Temporarily">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                    <div class="card-body p-2" style="max-height: 280px; overflow-y: auto;">
                        <p class="small text-muted mb-2">
                            Detected transactions with significant cost variance (&ge;50%) pending normalization:
                        </p>
                        <div class="table-responsive">
                            <table class="table table-sm table-bordered table-striped mb-1" style="font-size: 11px;">
                                <thead class="bg-light">
                                    <tr>
                                        <th style="white-space:nowrap;">Date</th>
                                        <th style="white-space:nowrap;">Outlet</th>
                                        <th>Item</th>
                                        <th style="white-space:nowrap;">Type</th>
                                        <th class="text-right" style="white-space:nowrap;">Trans Cost</th>
                                        <th class="text-right" style="white-space:nowrap;">Baseline</th>
                                        <th class="text-right" style="white-space:nowrap;">Diff</th>
                                    </tr>
                                </thead>
                                <tbody id="tblAnomalyBody">
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="card-footer bg-light p-2 d-flex justify-content-between align-items-center">
                        <div>
                            <asp:LinkButton ID="btnAutoRecalculateAnomalies" runat="server" OnClick="btnAutoRecalculateAnomalies_Click" CssClass="btn btn-xs btn-success font-weight-bold text-white shadow-sm mr-2" style="font-size: 11px; padding: 4px 10px;" OnClientClick="return confirmAutoRecalculate();">
                                <i class="fas fa-bolt mr-1"></i> Auto-Recalculate (Max 20)
                            </asp:LinkButton>
                        </div>
                        <a href="Pages_Calculate.aspx" class="btn btn-xs btn-outline-primary font-weight-bold" style="font-size: 11px; padding: 4px 8px;" title="Open Manual Recalculate Page">
                            <i class="fas fa-calculator mr-1"></i> Open Page
                        </a>
                    </div>
                </div>
            </div>

            <style type="text/css">
                .cost-anomaly-container {
                    position: fixed;
                    bottom: 25px;
                    right: 25px;
                    z-index: 1050;
                    max-width: 720px;
                    width: 95vw;
                    font-family: inherit;
                }
                .card-anomaly-box {
                    border-radius: 10px;
                    overflow: hidden;
                    margin-bottom: 0;
                    box-shadow: 0 10px 30px rgba(220, 53, 69, 0.45) !important;
                }
                .btn-anomaly-floating {
                    border-radius: 25px;
                    padding: 8px 18px;
                    font-weight: 600;
                    box-shadow: 0 4px 15px rgba(220, 53, 69, 0.45);
                    animation: pulseAnomaly 2s infinite;
                    cursor: pointer;
                    display: none;
                }
                @keyframes pulseAnomaly {
                    0% { transform: scale(1); }
                    50% { transform: scale(1.06); }
                    100% { transform: scale(1); }
                }
            </style>

            <script type="text/javascript">
                function initAnomalyBubble(data) {
                    if (!data || data.length === 0) return;
                    $('#lblAnomalyCount').text(data.length);
                    $('#lblAnomalyCountBadge').text(data.length);

                    var html = '';
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        var costVal = parseFloat(item.cost || 0).toLocaleString('id-ID', { maximumFractionDigits: 2 });
                        var baseVal = parseFloat(item.baseline_cost || 0).toLocaleString('id-ID', { maximumFractionDigits: 2 });
                        var diffVal = parseFloat(item.pct_diff || 0);
                        var badgeCls = diffVal > 0 ? 'badge-danger' : 'badge-warning';
                        var diffText = (diffVal > 0 ? '+' : '') + diffVal.toFixed(1) + '%';

                        html += '<tr>' +
                            '<td style="white-space:nowrap;">' + (item.tgl || '') + '</td>' +
                            '<td style="white-space:nowrap;"><strong>' + (item.Cc_name || '') + '</strong></td>' +
                            '<td style="max-width:180px; word-break:break-word;">' + (item.stk_name || '') + '</td>' +
                            '<td style="white-space:nowrap;"><span class="badge badge-secondary" style="font-size: 10px;">' + (item.type_desc || '') + '</span></td>' +
                            '<td class="text-right font-weight-bold text-danger" style="white-space:nowrap;">Rp ' + costVal + '</td>' +
                            '<td class="text-right text-muted" style="white-space:nowrap;">Rp ' + baseVal + '</td>' +
                            '<td class="text-right" style="white-space:nowrap;"><span class="badge ' + badgeCls + '">' + diffText + '</span></td>' +
                            '</tr>';
                    }
                    $('#tblAnomalyBody').html(html);
                    $('#costAnomalyContainer').show();

                    var isDismissed = sessionStorage.getItem('cost_anomaly_closed');
                    if (isDismissed === '1') {
                        $('#cardAnomalyBox').hide();
                        $('#btnAnomalyBadge').show();
                    } else {
                        $('#cardAnomalyBox').show();
                        $('#btnAnomalyBadge').hide();
                    }
                }

                function toggleAnomalyCard(showCard) {
                    if (showCard) {
                        $('#btnAnomalyBadge').fadeOut(150, function () {
                            $('#cardAnomalyBox').fadeIn(200);
                        });
                        sessionStorage.removeItem('cost_anomaly_closed');
                    } else {
                        $('#cardAnomalyBox').fadeOut(150, function () {
                            $('#btnAnomalyBadge').fadeIn(200);
                        });
                        sessionStorage.setItem('cost_anomaly_closed', '1');
                    }
                }

                function confirmAutoRecalculate() {
                    var count = $('#lblAnomalyCount').text() || 'several';
                    var confirmMsg = "The system will normalize batch cost anomalies and recalculate Moving Average Cost from D-1 of the earliest anomaly date to today for up to " + count + " transactions.\n\nDo you want to proceed now?";
                    if (confirm(confirmMsg)) {
                        if ($('#spinner').length) {
                            $('#spinner').show();
                        }
                        return true;
                    }
                    return false;
                }
            </script>

        </asp:Content>