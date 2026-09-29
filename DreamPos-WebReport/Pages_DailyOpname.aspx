<%@ Page Title="Daily Stock Opname" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_DailyOpname.aspx.vb" Inherits="DreamPos_WebReport.Pages_DailyOpname" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="shortcut icon" href="data:image/x-icon;," type="image/x-icon" />
    <link rel="icon" href="data:image/x-icon;," type="image/x-icon" />
    <style type="text/css">
        .content-header {
            padding: 6px 0.5rem !important;
        }
        .content-header h4 {
            font-size: 1.2rem;
            margin-bottom: 0;
        }
        .info-box.stat-card {
            min-height: 48px !important;
            margin-bottom: 8px !important;
            padding: 4px 8px !important;
            border-radius: 6px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            transition: transform 0.15s;
        }
        .info-box.stat-card:hover {
            transform: translateY(-1px);
        }
        .info-box.stat-card .info-box-icon {
            width: 38px !important;
            height: 38px !important;
            font-size: 1rem !important;
            border-radius: 4px !important;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .info-box.stat-card .info-box-content {
            padding: 0 6px !important;
            line-height: 1.1 !important;
            display: flex;
            flex-direction: column;
            justify-content: center;
        }
        .info-box.stat-card .info-box-text {
            font-size: 0.72rem !important;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            margin-bottom: 1px;
        }
        .info-box.stat-card .info-box-number {
            font-size: 1.05rem !important;
            line-height: 1 !important;
        }
        .filter-card {
            background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
            color: white;
            border-radius: 6px;
            padding: 8px 12px;
            margin-bottom: 8px;
            box-shadow: 0 3px 5px rgba(0,0,0,0.06);
        }
        .filter-card label {
            font-size: 0.72rem;
            margin-bottom: 2px;
        }
        .table-responsive-opname {
            max-height: 620px;
            overflow-x: auto !important;
            overflow-y: auto !important;
            position: relative;
            border: 1px solid #ced4da;
            background: #ffffff;
        }
        .table-opname {
            min-width: 1420px;
            width: 100% !important;
            border-collapse: separate !important;
            border-spacing: 0;
            font-size: 0.81rem;
        }
        .table-opname thead th {
            position: sticky;
            top: 0;
            z-index: 15;
            background-color: #1e3a5f !important;
            color: #ffffff !important;
            font-weight: 700 !important;
            font-size: 0.82rem !important;
            text-align: center;
            vertical-align: middle !important;
            white-space: nowrap;
            padding: 6px 6px !important;
            border: 1px solid #142841 !important;
            letter-spacing: 0.3px;
        }
        .table-opname td {
            vertical-align: middle !important;
            padding: 3px 6px !important;
            line-height: 1.25;
            border: 1px solid #dee2e6 !important;
            background-color: #ffffff;
            white-space: nowrap;
        }

        /* Freeze 3 Columns on the Left (CODE, Inventory_Stock_Name, CCName) */
        .col-sticky-1 {
            position: sticky !important;
            left: 0px !important;
            width: 85px !important;
            min-width: 85px !important;
            max-width: 85px !important;
        }
        .col-sticky-2 {
            position: sticky !important;
            left: 85px !important;
            width: 220px !important;
            min-width: 220px !important;
            max-width: 220px !important;
        }
        .col-sticky-3 {
            position: sticky !important;
            left: 305px !important;
            width: 120px !important;
            min-width: 120px !important;
            max-width: 120px !important;
            border-right: 3px solid #78909c !important;
            box-shadow: 4px 0 6px -2px rgba(0,0,0,0.18);
        }

        /* Top-Left Header Intersection (keeps headers on top of both vertical & horizontal scroll) */
        .table-opname thead th.col-sticky-1,
        .table-opname thead th.col-sticky-2,
        .table-opname thead th.col-sticky-3 {
            z-index: 30 !important;
            background-color: #152c48 !important;
            color: #ffffff !important;
        }

        /* Body Sticky Left Columns */
        .table-opname tbody td.col-sticky-1,
        .table-opname tbody td.col-sticky-2,
        .table-opname tbody td.col-sticky-3 {
            z-index: 10 !important;
            background-color: #f8fafc;
        }

        /* Row hover on body */
        .table-opname tbody tr:hover td {
            background-color: #eaf2f8 !important;
        }
        .table-opname tbody tr:hover td.col-sticky-1,
        .table-opname tbody tr:hover td.col-sticky-2,
        .table-opname tbody tr:hover td.col-sticky-3 {
            background-color: #e2eef7 !important;
        }

        .opname-col-header {
            background-color: #173860 !important;
            border-left: 2px solid #ffc107 !important;
            border-right: 2px solid #ffc107 !important;
            color: #ffeb3b !important;
        }
        .opname-input-cell {
            background-color: #fffdf5 !important;
            border-left: 2px solid #ffc107 !important;
            border-right: 2px solid #ffc107 !important;
        }
        .opname-input {
            width: 80px;
            height: 24px !important;
            font-size: 0.82rem !important;
            font-weight: bold;
            color: #1a5276;
            background-color: #fffde7;
            border: 1px solid #f9a825;
            border-radius: 3px;
            padding: 1px 4px !important;
            text-align: right;
            transition: all 0.2s ease-in-out;
            display: inline-block;
        }
        .opname-input:focus {
            background-color: #ffffff;
            border-color: #007bff;
            box-shadow: 0 0 4px rgba(0, 123, 255, 0.4);
            outline: none;
        }
        /* Button batalkan opname ('X') */
        .btn-cancel-opname {
            width: 22px;
            height: 22px;
            padding: 0;
            font-size: 0.72rem;
            line-height: 20px;
            text-align: center;
            border-radius: 50%;
            border: 1px solid #dc3545;
            color: #dc3545;
            background: #ffffff;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
        }
        .btn-cancel-opname:hover {
            background-color: #dc3545;
            color: #ffffff;
            transform: scale(1.12);
            box-shadow: 0 2px 4px rgba(220, 53, 69, 0.3);
        }
        .save-status-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 18px;
            height: 18px;
            font-size: 0.82rem;
            flex-shrink: 0;
        }
        .opname-input.saving {
            background-color: #fff8e1 !important;
            border-color: #ffc107 !important;
        }
        .opname-input.saved {
            background-color: #e8f5e9 !important;
            border-color: #28a745 !important;
        }
        /* Custom floating toast notification */
        .opname-toast {
            position: fixed;
            bottom: 25px;
            right: 25px;
            padding: 8px 16px;
            border-radius: 4px;
            z-index: 10500;
            font-size: 0.85rem;
            font-weight: 500;
            color: #fff;
            box-shadow: 0 4px 10px rgba(0,0,0,0.25);
            opacity: 0;
            transform: translateY(10px);
            transition: all 0.25s ease;
            pointer-events: none;
        }
        .opname-toast.show {
            opacity: 1;
            transform: translateY(0);
        }
        .var-positive {
            color: #d9534f;
            font-weight: bold;
        }
        .var-negative {
            color: #f0ad4e;
            font-weight: bold;
        }
        .var-zero {
            color: #28a745;
            font-weight: bold;
        }
        #spinner.modal {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(255, 255, 255, 0.7);
            z-index: 9999;
            text-align: center;
        }
        #spinner .center {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hfOpnameJson" runat="server" />

    <!-- Loading Spinner -->
    <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="Loading..." src="images/loader.gif" />
            <div class="font-weight-bold text-primary mt-2">Memproses data opname...</div>
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-1">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-clipboard-check text-primary mr-2"></i>Daily Stock Opname
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background: transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_StockOpname.aspx">Opname</a></li>
                        <li class="breadcrumb-item active">Daily Opname</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <!-- Alert Notification Panel -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show shadow-sm py-1 px-3 mb-2" role="alert">
            <asp:Label ID="lblAlertMsg" runat="server"></asp:Label>
            <button type="button" class="close py-1" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </asp:Panel>

        <!-- KPI / Summary Cards -->
        <div class="row mb-2">
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white">
                    <span class="info-box-icon bg-info elevation-1"><i class="fas fa-boxes"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Total Stock Items</span>
                        <span class="info-box-number text-dark font-weight-bold" id="lblStatTotal" runat="server">0</span>
                    </div>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white">
                    <span class="info-box-icon bg-success elevation-1"><i class="fas fa-check-circle"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Sudah Di-Opname</span>
                        <span class="info-box-number text-success font-weight-bold" id="lblStatOpnamed" runat="server">0</span>
                    </div>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white">
                    <span class="info-box-icon bg-warning elevation-1"><i class="fas fa-hourglass-half text-white"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Belum Di-Opname</span>
                        <span class="info-box-number text-warning font-weight-bold" id="lblStatPending" runat="server">0</span>
                    </div>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white">
                    <span class="info-box-icon bg-danger elevation-1"><i class="fas fa-exclamation-triangle"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Items Selisih / Variance</span>
                        <span class="info-box-number text-danger font-weight-bold" id="lblStatVariance" runat="server">0</span>
                    </div>
                </div>
            </div>
        </div>

        <!-- Filter & Control Bar -->
        <div class="filter-card">
            <div class="row align-items-end">
                <!-- Date Filter (Auto H-1) -->
                <div class="col-12 col-sm-6 col-md-3 col-lg-2 mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="far fa-calendar-alt mr-1"></i>Tanggal Opname:
                    </label>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control font-weight-bold text-center" AutoPostBack="false"></asp:TextBox>
                        <div class="input-group-append">
                            <span class="input-group-text bg-light px-2"><i class="fas fa-calendar"></i></span>
                        </div>
                        <ajaxToolkit:CalendarExtender ID="calExt" runat="server" TargetControlID="txtDate" Format="dd MMM yyyy" />
                    </div>
                </div>

                <!-- Cost Centre / Outlet Dropdown -->
                <div class="col-12 col-sm-6 col-md-3 col-lg-3 mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="fas fa-store mr-1"></i>Cost Centre / Outlet:
                    </label>
                    <asp:DropDownList ID="ddlCc" runat="server" CssClass="form-control form-control-sm font-weight-bold"></asp:DropDownList>
                </div>

                <!-- Stock Search Filter -->
                <div class="col-12 col-md-3 col-lg-4 mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="fas fa-search mr-1"></i>Cari Kode / Nama Barang:
                    </label>
                    <input type="text" id="txtQuickSearch" class="form-control form-control-sm" placeholder="Ketik untuk filter cepat..." />
                </div>

                <!-- Actions: Filter Button (Lup) + Excel + CSV in 1 line -->
                <div class="col-12 col-md-auto ml-md-auto text-md-right mt-2 mt-md-0">
                    <div class="d-flex align-items-center justify-content-md-end" style="gap: 5px; flex-wrap: nowrap;">
                        <asp:LinkButton ID="btnFilter" runat="server" OnClick="btnFilter_Click" 
                            CssClass="btn btn-primary btn-sm px-3 shadow-sm font-weight-bold" ToolTip="Tampilkan Data">
                            <i class="fas fa-search"></i>
                        </asp:LinkButton>
                        <asp:Button ID="btnExportExcel" runat="server" Text="Excel" OnClick="btnExportExcel_Click" 
                            CssClass="btn btn-success btn-sm px-2 shadow-sm font-weight-bold" />
                        <asp:Button ID="btnExportCsv" runat="server" Text="CSV" OnClick="btnExportCsv_Click" 
                            CssClass="btn btn-info btn-sm px-2 shadow-sm font-weight-bold" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Main Data Card -->
        <div class="card card-outline card-primary shadow-sm mb-3">
            <div class="card-header bg-white d-flex justify-content-between align-items-center py-1 px-3">
                <h6 class="card-title font-weight-bold text-dark mb-0">
                    <i class="fas fa-table mr-2 text-primary"></i>Data Stock Opname Harian
                    <span class="badge badge-secondary ml-2" id="badgeRowCount" runat="server">0 Rows</span>
                </h6>
                <div class="card-tools">
                    <asp:Button ID="btnSubmitTop" runat="server" Text="SIMPAN OPNAME" OnClick="btnSubmit_Click" 
                        CssClass="btn btn-warning btn-sm font-weight-bold shadow-sm text-dark px-3 py-1 btn-submit-opname" 
                        OnClientClick="return confirmSubmit();" />
                    <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
                </div>
            </div>

            <div class="card-body p-0">
                <div class="table-responsive table-responsive-opname">
                    <table class="table table-bordered table-striped table-hover table-opname mb-0" id="tblOpname">
                        <thead>
                            <tr>
                                <th class="col-sticky-1">CODE</th>
                                <th class="col-sticky-2" style="text-align: left;">Inventory_Stock_Name</th>
                                <th class="col-sticky-3">CCName</th>
                                <th style="width: 75px;">OpenQty</th>
                                <th style="width: 75px;">RcvQty</th>
                                <th style="width: 70px;">TinQty</th>
                                <th style="width: 70px;">ToutQty</th>
                                <th style="width: 75px;">RWasteQTY</th>
                                <th style="width: 70px;">BPinQty</th>
                                <th style="width: 70px;">BPoutQty</th>
                                <th style="width: 75px;">UsageQty</th>
                                <th style="width: 80px;">EndingQty</th>
                                <th class="opname-col-header" style="width: 140px;">
                                    <i class="fas fa-edit mr-1"></i>Opname
                                </th>
                                <th style="width: 85px; background-color: #234b75 !important;">Variance</th>
                                <th style="width: 60px;">Unit</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptOpname" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <tr class="opname-row" 
                                        data-stkid='<%# Eval("stkid") %>' 
                                        data-ccid='<%# Eval("ccid") %>' 
                                        data-totalqty='<%# Eval("TotalQty") %>'>

                                        <!-- [CODE] (Freeze Left 1) -->
                                        <td class="col-sticky-1 font-weight-bold text-center text-muted"><%# Eval("CODE") %></td>

                                        <!-- [Inventory_Stock_Name] (Freeze Left 2) -->
                                        <td class="col-sticky-2 font-weight-bold text-dark"><%# Eval("Inventory_Stock_Name") %></td>

                                        <!-- [CCName] (Freeze Left 3) -->
                                        <td class="col-sticky-3 text-center"><span class="badge badge-light border"><%# Eval("CCName") %></span></td>

                                        <!-- [OpenQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("OpenQty")) %></td>

                                        <!-- [RcvQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("RcvQty")) %></td>

                                        <!-- [TinQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("TinQty")) %></td>

                                        <!-- [ToutQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("ToutQty")) %></td>

                                        <!-- [RWasteQTY] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("RWasteQTY")) %></td>

                                        <!-- [BPinQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("BPinQty")) %></td>

                                        <!-- [BPoutQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("BPoutQty")) %></td>

                                        <!-- [UsageQty] -->
                                        <td class="text-right"><%# FormatNumberQty(Eval("UsageQty")) %></td>

                                        <!-- [EndingQty] -->
                                        <td class="text-right font-weight-bold ending-qty-cell"><%# FormatNumberQty(Eval("EndingQty")) %></td>

                                        <!-- [Opname (Input)] -->
                                        <td class="text-center opname-input-cell">
                                            <div class="d-flex align-items-center justify-content-center" style="gap: 4px;">
                                                <input type="text" class="form-control form-control-sm text-right opname-input" 
                                                    style="width: 72px; font-weight: 600; padding: 2px 4px; height: 26px; border-radius: 3px;"
                                                    data-stkid='<%# Eval("stkid") %>' 
                                                    data-ccid='<%# Eval("ccid") %>' 
                                                    data-orig='<%# If(Eval("bClose") = True, FormatInputQty(Eval("closeQty")), "") %>' 
                                                    value='<%# If(Eval("bClose") = True, FormatInputQty(Eval("closeQty")), "") %>' 
                                                    placeholder="0" autocomplete="off" />
                                                <button type="button" class="btn btn-cancel-opname" 
                                                    data-toggle="tooltip" data-placement="top" title="batalkan opname"
                                                    style='<%# If(Eval("bClose") = True AndAlso Val(Eval("closeQty").ToString()) > 0, "", "display:none;") %>'>
                                                    <i class="fas fa-times"></i>
                                                </button>
                                                <span class="save-status-icon" style="display:none;"></span>
                                            </div>
                                        </td>

                                        <!-- [Variance (Formula)] -->
                                        <td class="text-right font-weight-bold variance-cell <%# GetVarianceCss(Eval("bClose"), Eval("Variance")) %>">
                                            <%# If(Eval("bClose") = True, FormatNumberQty(Eval("Variance")), "-") %>
                                        </td>

                                        <!-- [Unit] -->
                                        <td class="text-center text-muted font-italic"><%# Eval("Unit") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr id="trNoData" runat="server" visible='<%# rptOpname.Items.Count = 0 %>'>
                                        <td colspan="15" class="text-center py-4 text-muted">
                                            <i class="fas fa-inbox fa-3x mb-2 text-secondary"></i><br />
                                            Tidak ada data stock opname untuk tanggal dan Cost Centre yang dipilih.
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="card-footer bg-light d-flex justify-content-between align-items-center py-1 px-3">
                <span class="small text-muted font-italic" style="font-size: 0.75rem;">
                    <i class="fas fa-info-circle mr-1"></i>Perhitungan: <b>Variance = Total Qty - Opname</b>. Nilai Opname akan otomatis disimpan menjadi <b>closeqty (H-1)</b> dan <b>openqty (Hari Berjalan)</b>.
                </span>
                <asp:Button ID="btnSubmitBottom" runat="server" Text="SIMPAN SEMUA PERUBAHAN" OnClick="btnSubmit_Click" 
                    CssClass="btn btn-warning btn-sm font-weight-bold shadow-sm px-3 text-dark btn-submit-opname" 
                    OnClientClick="return confirmSubmit();" />
            </div>
        </div>
    </div>

    <!-- Client-Side Scripts for Instant Calculation, Auto-Save, Cancel, Search, and UX -->
    <script type="text/javascript">
        function recalculateRow($input) {
            var $row = $input.closest('tr');
            var rawTotal = $row.attr('data-totalqty') || '0';
            var totalQty = parseFloat(rawTotal.replace(/,/g, '')) || 0;
            var valStr = $input.val().trim();
            var $varCell = $row.find('.variance-cell');

            if (valStr === '') {
                $varCell.text('-');
                $varCell.removeClass('var-zero var-positive var-negative');
                return;
            }

            var opnameVal = parseFloat(valStr.replace(/,/g, '')) || 0;
            var variance = totalQty - opnameVal;

            // Format to Indonesian decimal
            var formattedVar = variance.toLocaleString('id-ID', { minimumFractionDigits: 0, maximumFractionDigits: 4 });
            $varCell.text(formattedVar);

            $varCell.removeClass('var-zero var-positive var-negative');
            if (Math.abs(variance) < 0.00001) {
                $varCell.addClass('var-zero');
            } else if (variance > 0) {
                $varCell.addClass('var-positive');
            } else {
                $varCell.addClass('var-negative');
            }
        }

        // Floating toast notification
        function showToast(message, type) {
            var bg = type === 'success' ? '#28a745' : (type === 'danger' ? '#dc3545' : '#17a2b8');
            var $toast = $('<div class="opname-toast shadow">' + message + '</div>');
            $toast.css('background-color', bg);
            $('body').append($toast);
            setTimeout(function () { $toast.addClass('show'); }, 10);
            setTimeout(function () {
                $toast.removeClass('show');
                setTimeout(function () { $toast.remove(); }, 250);
            }, 2000);
        }

        // Live calculation on user typing
        $(document).on('input keyup', '.opname-input', function () {
            $(this).attr('data-modified', 'true');
            recalculateRow($(this));
        });

        // Auto-save per row on blur / change (saat user ganti baris)
        $(document).on('change', '.opname-input', function () {
            var $input = $(this);
            var val = $input.val().trim();
            var origVal = $input.attr('data-orig') || '';

            if (val !== origVal) {
                saveRowAjax($input, val);
            }
        });

        // Enter key navigation: blur & auto-save current row, lalu pindah kursor ke baris berikutnya
        $(document).on('keydown', '.opname-input', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                $(this).blur();
                var $currentRow = $(this).closest('tr');
                var $nextRow = $currentRow.nextAll('tr:visible').first();
                if ($nextRow.length > 0) {
                    var $nextInput = $nextRow.find('.opname-input');
                    if ($nextInput.length > 0) {
                        $nextInput.focus().select();
                    }
                }
            }
        });

        // AJAX single-row save
        function saveRowAjax($input, val) {
            var $row = $input.closest('tr');
            var $cancelBtn = $row.find('.btn-cancel-opname');
            var $statusIcon = $row.find('.save-status-icon');
            var stkId = $input.attr('data-stkid');
            var ccId = $input.attr('data-ccid');
            var targetDate = $('#<%= txtDate.ClientID %>').val();
            var stkName = $row.find('.col-sticky-2').text().trim();

            $input.addClass('saving');
            $statusIcon.html('<i class="fas fa-spinner fa-spin text-primary"></i>').show();

            $.ajax({
                type: 'POST',
                url: 'Pages_DailyOpname.aspx?action=save_row',
                data: {
                    stkId: stkId,
                    ccId: ccId,
                    val: val,
                    date: targetDate
                },
                dataType: 'json',
                success: function (res) {
                    $input.removeClass('saving');
                    if (res && res.success) {
                        $input.attr('data-orig', val);
                        $input.removeAttr('data-modified');
                        $input.addClass('saved');
                        setTimeout(function () { $input.removeClass('saved'); }, 1200);

                        $statusIcon.html('<i class="fas fa-check text-success"></i>').show();
                        setTimeout(function () { $statusIcon.fadeOut(200); }, 1500);

                        var numVal = parseFloat((val || '').replace(/,/g, '')) || 0;
                        if (numVal > 0) {
                            $cancelBtn.show();
                            $cancelBtn.tooltip({ boundary: 'window' });
                        } else {
                            $cancelBtn.hide();
                        }

                        showToast('<i class="fas fa-check mr-1"></i> ' + stkName + ' (' + val + ') tersimpan', 'success');
                    } else {
                        $statusIcon.html('<i class="fas fa-exclamation-triangle text-danger"></i>').show();
                        showToast('<i class="fas fa-exclamation mr-1"></i> Gagal: ' + (res.message || 'Error'), 'danger');
                    }
                },
                error: function () {
                    $input.removeClass('saving');
                    $statusIcon.html('<i class="fas fa-times text-danger"></i>').show();
                    showToast('<i class="fas fa-times mr-1"></i> Gagal koneksi saat menyimpan', 'danger');
                }
            });
        }

        // Cancel opname on button 'X' click (nilai kembali '0')
        $(document).on('click', '.btn-cancel-opname', function (e) {
            e.preventDefault();
            e.stopPropagation();

            var $btn = $(this);
            var $row = $btn.closest('tr');
            var $input = $row.find('.opname-input');
            var $statusIcon = $row.find('.save-status-icon');
            var stkId = $input.attr('data-stkid');
            var ccId = $input.attr('data-ccid');
            var targetDate = $('#<%= txtDate.ClientID %>').val();
            var stkName = $row.find('.col-sticky-2').text().trim();

            if (!confirm('Batalkan opname untuk ' + stkName + '?\n\nNilai opname akan dikembalikan menjadi 0.')) {
                return;
            }

            $btn.tooltip('hide');
            $btn.hide();
            $statusIcon.html('<i class="fas fa-spinner fa-spin text-muted"></i>').show();

            $.ajax({
                type: 'POST',
                url: 'Pages_DailyOpname.aspx?action=cancel_row',
                data: {
                    stkId: stkId,
                    ccId: ccId,
                    date: targetDate
                },
                dataType: 'json',
                success: function (res) {
                    if (res && res.success) {
                        $input.val('0');
                        $input.attr('data-orig', '0');
                        $input.removeAttr('data-modified');
                        recalculateRow($input);

                        $statusIcon.html('<i class="fas fa-check text-info"></i>').show();
                        setTimeout(function () { $statusIcon.fadeOut(200); }, 1500);

                        showToast('<i class="fas fa-undo mr-1"></i> Opname ' + stkName + ' dibatalkan (0)', 'info');
                    } else {
                        $btn.show();
                        $statusIcon.html('<i class="fas fa-exclamation-triangle text-danger"></i>').show();
                        showToast('<i class="fas fa-exclamation mr-1"></i> Gagal: ' + (res.message || 'Error'), 'danger');
                    }
                },
                error: function () {
                    $btn.show();
                    $statusIcon.html('<i class="fas fa-times text-danger"></i>').show();
                    showToast('<i class="fas fa-times mr-1"></i> Gagal koneksi saat membatalkan', 'danger');
                }
            });
        });

        // Initialize tooltips with custom bubble message and fast search
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip({ boundary: 'window' });

            $('#txtQuickSearch').on('keyup', function () {
                var value = $(this).val().toLowerCase();
                $('#tblOpname tbody tr.opname-row').filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                });
            });
        });

        // Optional confirmation before bulk submit
        function confirmSubmit() {
            var modifiedItems = [];
            $('.opname-input').each(function () {
                var isModified = $(this).attr('data-modified') === 'true';
                var val = $(this).val().trim();
                var origVal = $(this).attr('data-orig') || '';
                if ((isModified || val !== origVal) && val !== '') {
                    var stkId = parseInt($(this).attr('data-stkid')) || 0;
                    var ccId = parseInt($(this).attr('data-ccid')) || 0;
                    if (stkId > 0 && ccId > 0) {
                        modifiedItems.push({ stkId: stkId, ccId: ccId, val: val });
                    }
                }
            });

            if (modifiedItems.length === 0) {
                alert('Peringatan: Belum ada baris kuantitas opname yang diedit atau diubah.');
                return false;
            }

            var res = confirm('Apakah Anda yakin ingin menyimpan ' + modifiedItems.length + ' item Stock Opname yang telah diubah?\n\nNilai akan menjadi CloseQty untuk H-1 dan OpenQty untuk Hari Berjalan.');
            if (res) {
                var hf = document.getElementById('<%= hfOpnameJson.ClientID %>');
                if (hf) {
                    hf.value = JSON.stringify(modifiedItems);
                }
                $('#spinner').show();
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
