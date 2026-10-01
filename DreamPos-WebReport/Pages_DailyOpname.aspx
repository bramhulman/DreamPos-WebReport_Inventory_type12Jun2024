<%@ Page Title="Daily Stock Opname" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_DailyOpname.aspx.vb" Inherits="DreamPos_WebReport.Pages_DailyOpname" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="shortcut icon" href="data:image/x-icon;," type="image/x-icon" />

    <%-- DataTables CSS --%>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.7/css/dataTables.bootstrap4.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.bootstrap4.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.bootstrap4.min.css" />

    <style type="text/css">
        /* ── Content Header ── */
        .content-header { padding: 6px 0.5rem !important; }
        .content-header h4 { font-size: 1.2rem; margin-bottom: 0; }

        /* ── KPI Cards ── */
        .info-box.stat-card {
            min-height: 48px !important;
            margin-bottom: 8px !important;
            padding: 4px 8px !important;
            border-radius: 6px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            transition: transform 0.15s;
        }
        .info-box.stat-card:hover { transform: translateY(-1px); }
        .info-box.stat-card .info-box-icon {
            width: 38px !important; height: 38px !important;
            font-size: 1rem !important; border-radius: 4px !important;
            display: flex; align-items: center; justify-content: center;
        }
        .info-box.stat-card .info-box-content {
            padding: 0 6px !important; line-height: 1.1 !important;
            display: flex; flex-direction: column; justify-content: center;
        }
        .info-box.stat-card .info-box-text {
            font-size: 0.72rem !important; text-transform: uppercase;
            letter-spacing: 0.3px; margin-bottom: 1px;
        }
        .info-box.stat-card .info-box-number { font-size: 1.05rem !important; line-height: 1 !important; }

        /* ── Filter Bar ── */
        .filter-card {
            background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
            color: white; border-radius: 6px;
            padding: 8px 12px; margin-bottom: 8px;
            box-shadow: 0 3px 5px rgba(0,0,0,0.06);
        }
        .filter-card label { font-size: 0.72rem; margin-bottom: 2px; }

        /* ── KPI Stat Card Active & Hover ── */
        .stat-card {
            transition: all 0.2s ease-in-out;
            border: 2px solid transparent;
        }
        .stat-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 10px rgba(0,0,0,0.12) !important;
        }
        .stat-card.active-filter {
            border: 2px solid #007bff !important;
            box-shadow: 0 0 10px rgba(0,123,255,0.4) !important;
            background-color: #f0f7ff !important;
        }
        #cardStatVariance.active-filter {
            border: 2px solid #dc3545 !important;
            box-shadow: 0 0 10px rgba(220,53,69,0.4) !important;
            background-color: #fff5f5 !important;
        }

        /* ── DataTables pagination & info ── */
        #tblOpname_wrapper .dataTables_info,
        #tblOpname_wrapper .dataTables_paginate {
            padding: 6px 12px;
            font-size: 0.82rem;
        }

        /* ── Table & Header Styling ── */
        .table-responsive-opname {
            overflow-x: auto !important;
            border: 1px solid #ced4da;
            background: #ffffff;
            margin-bottom: 0;
        }
        .table-opname {
            font-size: 0.81rem;
            width: 100% !important;
            min-width: 1350px;
        }
        .table-opname thead th {
            background-color: #1e3a5f !important;
            color: #ffffff !important;
            font-weight: 700 !important;
            font-size: 0.8rem !important;
            text-align: center;
            vertical-align: middle !important;
            white-space: nowrap;
            padding: 7px 6px !important;
            border: 1px solid #142841 !important;
        }
        .table-opname td {
            vertical-align: middle !important;
            padding: 3px 6px !important;
            line-height: 1.25;
            white-space: nowrap;
        }
        /* opname input column highlight */
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
            width: 78px; height: 24px !important;
            font-size: 0.82rem !important; font-weight: bold;
            color: #1a5276; background-color: #fffde7;
            border: 1px solid #f9a825; border-radius: 3px;
            padding: 1px 4px !important; text-align: right;
            transition: all 0.2s ease-in-out; display: inline-block;
        }
        .opname-input:focus {
            background-color: #ffffff; border-color: #007bff;
            box-shadow: 0 0 4px rgba(0,123,255,0.4); outline: none;
        }
        .opname-input.saving { background-color: #fff8e1 !important; border-color: #ffc107 !important; }
        .opname-input.saved  { background-color: #e8f5e9 !important; border-color: #28a745 !important; }

        /* cancel opname button */
        .btn-cancel-opname {
            width: 22px; height: 22px; padding: 0;
            font-size: 0.72rem; line-height: 20px; text-align: center;
            border-radius: 50%; border: 1px solid #dc3545;
            color: #dc3545; background: #ffffff; cursor: pointer;
            transition: all 0.2s ease-in-out;
            display: inline-flex; align-items: center; justify-content: center; flex-shrink: 0;
        }
        .btn-cancel-opname:hover {
            background-color: #dc3545; color: #ffffff;
            transform: scale(1.12); box-shadow: 0 2px 4px rgba(220,53,69,0.3);
        }
        .save-status-icon {
            display: inline-flex; align-items: center; justify-content: center;
            width: 18px; height: 18px; font-size: 0.82rem; flex-shrink: 0;
        }

        /* variance */
        .var-positive { color: #d9534f; font-weight: bold; }
        .var-negative { color: #f0ad4e; font-weight: bold; }
        .var-zero     { color: #28a745; font-weight: bold; }

        /* floating toast */
        .opname-toast {
            position: fixed; bottom: 25px; right: 25px;
            padding: 8px 16px; border-radius: 4px; z-index: 10500;
            font-size: 0.85rem; font-weight: 500; color: #fff;
            box-shadow: 0 4px 10px rgba(0,0,0,0.25);
            opacity: 0; transform: translateY(10px);
            transition: all 0.25s ease; pointer-events: none;
        }
        .opname-toast.show { opacity: 1; transform: translateY(0); }

        /* loading spinner */
        #spinner.modal {
            position: fixed; top: 0; left: 0;
            width: 100%; height: 100%;
            background-color: rgba(255,255,255,0.7);
            z-index: 9999; text-align: center;
        }
        #spinner .center {
            position: absolute; top: 50%; left: 50%;
            transform: translate(-50%,-50%);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hfOpnameJson" runat="server" />

    <%-- Loading Spinner --%>
    <div id="spinner" class="modal" style="display:none">
        <div class="center">
            <img alt="Loading..." src="images/loader.gif" />
            <div class="font-weight-bold text-primary mt-2">Memproses data opname...</div>
        </div>
    </div>

    <%-- Content Header --%>
    <div class="content-header py-1">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-clipboard-check text-primary mr-2"></i>Daily Stock Opname
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_StockOpname.aspx">Opname</a></li>
                        <li class="breadcrumb-item active">Daily Opname</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">

        <%-- Alert --%>
        <asp:Panel ID="pnlAlert" runat="server" Visible="false"
            CssClass="alert alert-dismissible fade show shadow-sm py-1 px-3 mb-2" role="alert">
            <asp:Label ID="lblAlertMsg" runat="server"></asp:Label>
            <button type="button" class="close py-1" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </asp:Panel>

        <%-- KPI Cards (Bisa diklik untuk quick filter) --%>
        <div class="row mb-2">
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white" id="cardStatTotal" style="cursor:pointer;" title="Klik untuk reset filter (tampilkan semua barang)">
                    <span class="info-box-icon bg-info elevation-1"><i class="fas fa-boxes"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Total Stock Items</span>
                        <span class="info-box-number text-dark font-weight-bold" id="lblStatTotal" runat="server">0</span>
                    </div>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white" id="cardStatOpnamed" style="cursor:pointer;" title="Klik untuk filter: hanya barang yang sudah di-opname">
                    <span class="info-box-icon bg-success elevation-1"><i class="fas fa-check-circle"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Sudah Di-Opname</span>
                        <span class="info-box-number text-success font-weight-bold" id="lblStatOpnamed" runat="server">0</span>
                    </div>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white" id="cardStatPending" style="cursor:pointer;" title="Klik untuk filter: hanya barang yang belum di-opname">
                    <span class="info-box-icon bg-warning elevation-1"><i class="fas fa-hourglass-half text-white"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-muted">Belum Di-Opname</span>
                        <span class="info-box-number text-warning font-weight-bold" id="lblStatPending" runat="server">0</span>
                    </div>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="info-box stat-card bg-white" id="cardStatVariance" style="cursor:pointer; border:1px solid #ffc107;" title="Klik untuk filter: HANYA barang yang ada selisih (Variance <> 0)">
                    <span class="info-box-icon bg-danger elevation-1"><i class="fas fa-exclamation-triangle"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text text-danger font-weight-bold">Items Variance</span>
                        <span class="info-box-number text-danger font-weight-bold" id="lblStatVariance" runat="server">0</span>
                    </div>
                </div>
            </div>
        </div>

        <%-- Filter Bar --%>
        <div class="filter-card">
            <div class="row align-items-end">
                <%-- Date --%>
                <div class="col-12 col-sm-6 col-md-3 col-lg-2 mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="far fa-calendar-alt mr-1"></i>Tanggal Opname:
                    </label>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control font-weight-bold text-center" AutoPostBack="false" style="height: 31px; font-size: 0.82rem;"></asp:TextBox>
                        <div class="input-group-append" style="<%= If(IsAdminOrSupport(), "cursor:pointer;", "cursor:not-allowed; opacity:0.75;") %>">
                            <span class="input-group-text bg-light px-2" style="height: 31px;">
                                <i class="fas <%= If(IsAdminOrSupport(), "fa-calendar text-primary", "fa-lock text-secondary") %>"
                                   title="<%= If(IsAdminOrSupport(), "Klik untuk pilih tanggal", "Tanggal opname dikunci (H-1). Hanya user support/admin yang dapat mengubah data sebelum kemarin.") %>"></i>
                            </span>
                        </div>
                        <ajaxToolkit:CalendarExtender ID="calExt" runat="server" TargetControlID="txtDate" Format="dd MMM yyyy" OnClientDateSelectionChanged="onDateSelected" />
                    </div>
                </div>

                <%-- Cost Centre --%>
                <div class="col-12 col-sm-6 col-md-3 col-lg-3 mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="fas fa-store mr-1"></i>Cost Centre / Outlet:
                    </label>
                    <asp:DropDownList ID="ddlCc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCc_SelectedIndexChanged"
                        CssClass="form-control form-control-sm font-weight-bold" style="height: 31px; font-size: 0.82rem; padding: 2px 8px;"></asp:DropDownList>
                </div>

                <%-- Quick Search (feeds DataTables built-in search) --%>
                <div class="col-12 col-md-3 col-lg-3 mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="fas fa-search mr-1"></i>Cari Kode / Nama Barang:
                    </label>
                    <div class="input-group input-group-sm">
                        <input type="text" id="txtQuickSearch" class="form-control form-control-sm"
                            placeholder="Menyiapkan data tabel..." autocomplete="off" disabled="disabled"
                            style="height: 31px; font-size: 0.82rem;" />
                        <div class="input-group-append">
                            <span class="input-group-text bg-white px-2" id="searchStatusIcon" style="height: 31px; width: 34px; justify-content: center;" title="Menyiapkan tabel...">
                                <i class="fas fa-spinner fa-spin text-warning"></i>
                            </span>
                        </div>
                    </div>
                </div>

                <%-- Tampilkan ... baris (DataTables length) --%>
                <div class="col-auto mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 font-weight-bold">
                        <i class="fas fa-list-ol mr-1"></i>Tampilkan:
                    </label>
                    <div class="d-flex align-items-center">
                        <select id="customPageLength" class="form-control form-control-sm font-weight-bold text-center" style="height: 31px; width: 80px; font-size: 0.82rem; padding: 2px 6px;">
                            <option value="10">10</option>
                            <option value="25" selected="selected">25</option>
                            <option value="50">50</option>
                            <option value="100">100</option>
                            <option value="-1">Semua</option>
                        </select>
                        <span class="small text-light ml-1 font-weight-bold">baris</span>
                    </div>
                </div>

                <%-- Action Buttons (Lup, Excel, CSV) --%>
                <div class="col-auto ml-auto text-right mb-2 mb-md-0">
                    <label class="small text-uppercase text-light mb-1 d-block font-weight-bold" style="visibility: hidden;">Aksi</label>
                    <div class="d-flex align-items-center justify-content-end" style="gap:5px; flex-wrap:nowrap;">
                        <asp:LinkButton ID="btnFilter" runat="server" OnClick="btnFilter_Click"
                            CssClass="btn btn-primary btn-sm px-3 shadow-sm font-weight-bold" style="height: 31px; display: inline-flex; align-items: center;" ToolTip="Tampilkan Data">
                            <i class="fas fa-search"></i>
                        </asp:LinkButton>
                        <button type="button" id="btnExportDtExcel" class="btn btn-success btn-sm px-2 shadow-sm font-weight-bold" style="height: 31px; display: inline-flex; align-items: center;" title="Export Excel">
                            <i class="fas fa-file-excel mr-1"></i>Excel
                        </button>
                        <button type="button" id="btnExportDtCsv" class="btn btn-info btn-sm px-2 shadow-sm font-weight-bold" style="height: 31px; display: inline-flex; align-items: center;" title="Export CSV">
                            <i class="fas fa-file-csv mr-1"></i>CSV
                        </button>

                        <%-- Hidden Server Export Buttons (fallback) --%>
                        <asp:Button ID="btnExportExcel" runat="server" Text="Excel" OnClick="btnExportExcel_Click" style="display:none;" />
                        <asp:Button ID="btnExportCsv" runat="server" Text="CSV" OnClick="btnExportCsv_Click" style="display:none;" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Main Data Card --%>
        <div class="card card-outline card-primary shadow-sm mb-3">
            <div class="card-header bg-white d-flex justify-content-between align-items-center py-2 px-3">
                <h6 class="card-title font-weight-bold text-dark mb-0">
                    <i class="fas fa-table mr-2 text-primary"></i>Data Stock Opname Harian
                    <span class="badge badge-secondary ml-2" id="badgeRowCount" runat="server">0 Rows</span>
                    <span class="badge badge-danger ml-1" id="badgeFilterVariance" style="display:none; cursor:pointer;" title="Klik untuk reset filter (tampilkan semua data)">
                        <i class="fas fa-filter mr-1"></i>Hanya Variance &times;
                    </span>
                    <span class="badge badge-warning ml-1" id="badgeLoadingStatus"><i class="fas fa-spinner fa-spin mr-1"></i>Menyiapkan tabel...</span>
                </h6>
            </div>

            <div class="card-body p-0">
                <div class="table-responsive table-responsive-opname">
                    <table class="table table-bordered table-striped table-hover table-opname mb-0" id="tblOpname" style="width:100%">
                    <thead>
                        <tr>
                            <th>CODE</th>
                            <th style="text-align:left; min-width:200px;">Inventory Stock Name</th>
                            <th>CC Name</th>
                            <th>Open Qty</th>
                            <th>Rcv Qty</th>
                            <th>Tin Qty</th>
                            <th>Tout Qty</th>
                            <th>R.Waste</th>
                            <th>BP In</th>
                            <th>BP Out</th>
                            <th>Usage</th>
                            <th>Ending Qty</th>
                            <th class="opname-col-header"><i class="fas fa-edit mr-1"></i>Opname</th>
                            <th style="background-color:#234b75 !important; color:#fff !important;">Variance</th>
                            <th>Unit</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptOpname" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <tr class="opname-row"
                                    data-stkid='<%# Eval("stkid") %>'
                                    data-ccid='<%# Eval("ccid") %>'
                                    data-totalqty='<%# Eval("TotalQty") %>'
                                    data-bclose='<%# If(IsOpnamed(Eval("bClose")), "1", "0") %>'
                                    data-variance='<%# Eval("Variance") %>'
                                    data-hasvariance='<%# If(HasVariance(Eval("bClose"), Eval("Variance")), "1", "0") %>'>

                                    <td class="font-weight-bold text-center text-muted"><%# Eval("CODE") %></td>
                                    <td class="font-weight-bold text-dark"><%# Eval("Inventory_Stock_Name") %></td>
                                    <td class="text-center"><span class="badge badge-light border"><%# Eval("CCName") %></span></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("OpenQty")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("RcvQty")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("TinQty")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("ToutQty")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("RWasteQTY")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("BPinQty")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("BPoutQty")) %></td>
                                    <td class="text-right"><%# FormatNumberQty(Eval("UsageQty")) %></td>
                                    <td class="text-right font-weight-bold"><%# FormatNumberQty(Eval("EndingQty")) %></td>

                                    <%-- Opname Input --%>
                                    <td class="text-center opname-input-cell">
                                        <div class="d-flex align-items-center justify-content-center" style="gap:3px;">
                                            <input type="text" class="opname-input"
                                                data-stkid='<%# Eval("stkid") %>'
                                                data-ccid='<%# Eval("ccid") %>'
                                                data-orig='<%# GetOpnameVal(Eval("bClose"), Eval("closeQty")) %>'
                                                value='<%# GetOpnameVal(Eval("bClose"), Eval("closeQty")) %>'
                                                placeholder="0" autocomplete="off" />
                                            <button type="button" class="btn-cancel-opname"
                                                data-toggle="tooltip" data-placement="top" title="Batalkan opname"
                                                style='<%# If(IsOpnamed(Eval("bClose")) AndAlso Val(Eval("closeQty").ToString()) > 0, "", "display:none;") %>'>
                                                <i class="fas fa-times"></i>
                                            </button>
                                            <span class="save-status-icon" style="display:none;"></span>
                                        </div>
                                    </td>

                                    <%-- Variance --%>
                                    <td class="text-right font-weight-bold variance-cell <%# GetVarianceCss(Eval("bClose"), Eval("Variance")) %>"
                                        data-order='<%# If(IsOpnamed(Eval("bClose")), Eval("Variance"), "999999999") %>'>
                                        <%# GetVarianceDisplay(Eval("bClose"), Eval("Variance")) %>
                                    </td>

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

            <div class="card-footer bg-light py-2 px-3">
                <span class="small text-muted font-italic" style="font-size:0.75rem;">
                    <i class="fas fa-info-circle mr-1"></i>
                    <b>Variance = Total Qty - Opname</b>. Nilai opname otomatis tersimpan saat berpindah baris / tekan Enter.
                </span>
                <asp:Button ID="btnSubmitBottom" runat="server" Text="SIMPAN SEMUA PERUBAHAN"
                    OnClick="btnSubmit_Click" style="display:none;" />
            </div>
        </div>

    </div><%-- /.container-fluid --%>

    <%-- ═══════════════════ SCRIPTS ═══════════════════ --%>

    <%-- DataTables JS & plugins --%>
    <script src="https://cdn.datatables.net/1.13.7/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.7/js/dataTables.bootstrap4.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.bootstrap4.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>
    <script src="https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"></script>
    <script src="https://cdn.datatables.net/responsive/2.5.0/js/responsive.bootstrap4.min.js"></script>
    <script type="text/javascript">
        var dtOpname = null;

        /* ── DataTables initialization ─────────────────────────────── */
        $(document).ready(function () {

            setTimeout(function () {
                dtOpname = $('#tblOpname').DataTable({
                    paging: true,
                    pageLength: 25,
                    deferRender: true,             // Sangat penting untuk ribuan baris agar browser cepat
                    ordering: true,
                    order: [],                     // Mengikuti urutan SQL server langsung tanpa re-sort berat di JS
                    searching: true,               // internal search engine (fed by txtQuickSearch)
                    info: true,
                    autoWidth: false,
                    responsive: false,
                // Disable column ordering for opname input; Variance is sortable numeric
                columnDefs: [
                    { targets: [12], orderable: false, searchable: false },  // Opname input non-orderable
                    { targets: [13], type: 'num' },                          // Variance is numeric & sortable
                    { targets: '_all', className: 'dt-body-center' }
                ],
                // DOM: HANYA TABEL (t) LALU DI BAWAHNYA INFO (i) DAN PAGINATION (p)
                dom: "<'row'<'col-sm-12'tr>>" +
                     "<'row py-2 px-3 align-items-center'<'col-sm-6 text-muted'i><'col-sm-6'p>>",
                buttons: [
                    {
                        extend: 'excelHtml5',
                        title: 'Daily Opname',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14],  // skip Opname input col
                            format: {
                                body: function (data, row, column, node) {
                                    return $('<div>').html(data).text().trim();
                                }
                            }
                        }
                    },
                    {
                        extend: 'csvHtml5',
                        title: 'Daily Opname',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14],
                            format: {
                                body: function (data, row, column, node) {
                                    return $('<div>').html(data).text().trim();
                                }
                            }
                        }
                    }
                ],
                language: {
                    search: 'Cari:',
                    lengthMenu: 'Tampilkan _MENU_ baris',
                    info: 'Menampilkan _START_ s/d _END_ dari _TOTAL_ baris',
                    infoEmpty: 'Tidak ada data',
                    infoFiltered: '(difilter dari _MAX_ total baris)',
                    zeroRecords: 'Tidak ada data yang cocok',
                    paginate: {
                        first: '<i class="fas fa-angle-double-left"></i>',
                        last: '<i class="fas fa-angle-double-right"></i>',
                        previous: '<i class="fas fa-angle-left"></i>',
                        next: '<i class="fas fa-angle-right"></i>'
                    }
                },
                initComplete: function (settings, json) {
                    // Ketika DataTables 100% selesai dimuat & siap dicari:
                    $('#txtQuickSearch').prop('disabled', false)
                                        .attr('placeholder', 'Ketik untuk filter cepat...');
                    $('#searchStatusIcon').html('<i class="fas fa-search text-muted"></i>')
                                          .attr('title', 'Tabel siap dicari');
                    $('#badgeLoadingStatus').removeClass('badge-warning')
                                            .addClass('badge-success')
                                            .html('<i class="fas fa-check mr-1"></i>Siap')
                                            .fadeOut(2500);
                }
            });
            }, 50);

            /* ── Tampilkan spinner saat filter / reload data ── */
            $('#<%= btnFilter.ClientID %>').on('click', function () {
                $('#spinner').show();
            });

            /* ── Hubungkan tombol Export atas (Excel & CSV) ke DataTables export ── */
            $('#btnExportDtExcel').on('click', function () {
                if (dtOpname) {
                    dtOpname.button('.buttons-excel').trigger();
                }
            });

            $('#btnExportDtCsv').on('click', function () {
                if (dtOpname) {
                    dtOpname.button('.buttons-csv').trigger();
                }
            });

            /* ── Hubungkan dropdown Tampilkan ... baris di filter bar ke DataTables ── */
            $('#customPageLength').on('change', function () {
                if (dtOpname) {
                    var len = parseInt($(this).val());
                    dtOpname.page.len(len).draw();
                }
            });

            /* ── Quick Filter: KPI card click handlers ── */
            $('#cardStatVariance').on('click', function () { setTableFilter('variance'); });
            $('#cardStatOpnamed').on('click', function () { setTableFilter('opnamed'); });
            $('#cardStatPending').on('click', function () { setTableFilter('pending'); });
            $('#cardStatTotal').on('click', function () { setTableFilter('all'); });
            $('#badgeFilterVariance').on('click', function () { setTableFilter('all'); });

            /* ── Dropdown Cost Centre responsif: tampilkan spinner saat berubah ── */
            $('#<%= ddlCc.ClientID %>').on('change', function () {
                $('#spinner').show();
            });

            /* ── Pipe txtQuickSearch → DataTables search engine dengan debounce & indikator loading ── */
            var searchDebounceTimer = null;
            $('#txtQuickSearch').on('input keyup', function () {
                var searchVal = $(this).val();
                clearTimeout(searchDebounceTimer);

                // Indikasi visual: icon berubah menjadi spinner saat sedang memproses pencarian
                $('#searchStatusIcon').html('<i class="fas fa-spinner fa-spin text-primary"></i>');

                searchDebounceTimer = setTimeout(function () {
                    if (dtOpname) {
                        dtOpname.search(searchVal).draw();
                    }
                    // Kembalikan ke icon search setelah tabel selesai difilter
                    $('#searchStatusIcon').html('<i class="fas fa-search text-muted"></i>');
                }, 250);
            });
        });

        /* ── Auto-submit saat admin/support memilih tanggal dari kalender ── */
        function onDateSelected(sender, args) {
            $('#spinner').show();
            setTimeout(function () {
                var btn = document.getElementById('<%= btnFilter.ClientID %>');
                if (btn) btn.click();
            }, 100);
        }

        <% If Not IsAdminOrSupport() Then %>
        $(document).ready(function () {
            $('#<%= txtDate.ClientID %>').prop('readonly', true).on('keydown paste focus mousedown', function (e) {
                e.preventDefault();
                $(this).blur();
            });
        });
        <% End If %>

        /* ══════════════════════════════════════════════════════════ */
        /* ── Custom DataTables Filter Engine ─────────────────────── */
        var activeFilterType = 'all'; // 'all', 'variance', 'opnamed', 'pending'

        $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
            if (settings.sTableId !== 'tblOpname') return true;
            if (!activeFilterType || activeFilterType === 'all') return true;

            var rowData = settings.aoData[dataIndex];
            var rowNode = rowData ? rowData.nTr : null;
            var $row = rowNode ? $(rowNode) : null;

            var bClose = false;
            var hasVariance = false;

            if ($row && $row.length > 0) {
                bClose = $row.attr('data-bclose') === '1';
                hasVariance = $row.attr('data-hasvariance') === '1';
            } else {
                var varText = (data[13] || '').trim();
                bClose = varText !== '-';
                hasVariance = bClose && varText !== '0,00' && varText !== '0';
            }

            if (activeFilterType === 'variance') {
                return bClose && hasVariance;
            } else if (activeFilterType === 'opnamed') {
                return bClose;
            } else if (activeFilterType === 'pending') {
                return !bClose;
            }
            return true;
        });

        function setTableFilter(type) {
            if (activeFilterType === type) {
                activeFilterType = 'all';
            } else {
                activeFilterType = type;
            }

            $('.stat-card').removeClass('active-filter');
            $('#badgeFilterVariance').hide();

            if (activeFilterType === 'variance') {
                $('#cardStatVariance').addClass('active-filter');
                $('#badgeFilterVariance').show();
            } else if (activeFilterType === 'opnamed') {
                $('#cardStatOpnamed').addClass('active-filter');
            } else if (activeFilterType === 'pending') {
                $('#cardStatPending').addClass('active-filter');
            } else {
                $('#cardStatTotal').addClass('active-filter');
                setTimeout(function () { $('#cardStatTotal').removeClass('active-filter'); }, 1000);
            }

            if (dtOpname) {
                dtOpname.draw();
            }
        }

        /* ══════════════════════════════════════════════════════════ */
        /* ── Live Variance Recalculation ─────────────────────────── */
        function recalculateRow($input) {
            var $row = $input.closest('tr');
            var rawTotal = $row.attr('data-totalqty') || '0';
            var totalQty = parseFloat(rawTotal.replace(/,/g, '')) || 0;
            var valStr = $input.val().trim();
            var $varCell = $row.find('.variance-cell');

            if (valStr === '') {
                $varCell.text('-');
                $varCell.attr('data-order', '999999999');
                $varCell.removeClass('var-zero var-positive var-negative');
                $row.attr('data-bclose', '0').attr('data-hasvariance', '0').attr('data-variance', '');
                return;
            }
            var opnameVal = parseFloat(valStr.replace(/,/g, '')) || 0;
            var variance = totalQty - opnameVal;
            var formatted = variance.toLocaleString('id-ID', { minimumFractionDigits: 0, maximumFractionDigits: 4 });
            $varCell.text(formatted);
            $varCell.attr('data-order', variance);
            $varCell.removeClass('var-zero var-positive var-negative');
            var hasVar = Math.abs(variance) >= 0.0001;
            if (!hasVar)                           $varCell.addClass('var-zero');
            else if (variance > 0)                 $varCell.addClass('var-positive');
            else                                   $varCell.addClass('var-negative');

            $row.attr('data-bclose', '1')
                .attr('data-hasvariance', hasVar ? '1' : '0')
                .attr('data-variance', variance);
        }

        /* ── Floating Toast ────────────────────────────────────────── */
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

        /* ── Input events ─────────────────────────────────────────── */
        $(document).on('input keyup', '.opname-input', function () {
            $(this).attr('data-modified', 'true');
            recalculateRow($(this));
        });

        $(document).on('change', '.opname-input', function () {
            var $input = $(this);
            var val = $input.val().trim();
            var origVal = $input.attr('data-orig') || '';
            if (val !== origVal) {
                saveRowAjax($input, val);
            }
        });

        // Enter → save current, move to next visible row
        $(document).on('keydown', '.opname-input', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                $(this).blur();
                var $nextRow = $(this).closest('tr').nextAll('tr.opname-row:visible').first();
                if ($nextRow.length > 0) {
                    var $ni = $nextRow.find('.opname-input');
                    if ($ni.length > 0) $ni.focus().select();
                }
            }
        });

        /* ── AJAX single-row save ─────────────────────────────────── */
        function saveRowAjax($input, val) {
            var $row       = $input.closest('tr');
            var $cancelBtn = $row.find('.btn-cancel-opname');
            var $statusIcon= $row.find('.save-status-icon');
            var stkId      = $input.attr('data-stkid');
            var ccId       = $input.attr('data-ccid');
            var targetDate = $('#<%= txtDate.ClientID %>').val();
            var stkName    = $row.find('td:eq(1)').text().trim();

            $input.addClass('saving');
            $statusIcon.html('<i class="fas fa-spinner fa-spin text-primary"></i>').show();

            $.ajax({
                type: 'POST',
                url: 'Pages_DailyOpname.aspx?action=save_row',
                data: { stkId: stkId, ccId: ccId, val: val, date: targetDate },
                dataType: 'json',
                success: function (res) {
                    $input.removeClass('saving');
                    if (res && res.success) {
                        $input.attr('data-orig', val).removeAttr('data-modified');
                        $input.addClass('saved');
                        setTimeout(function () { $input.removeClass('saved'); }, 1200);
                        $statusIcon.html('<i class="fas fa-check text-success"></i>').show();
                        setTimeout(function () { $statusIcon.fadeOut(200); }, 1500);
                        var numVal = parseFloat((val || '').replace(/,/g, '')) || 0;
                        if (numVal > 0) { $cancelBtn.show(); $cancelBtn.tooltip({ boundary: 'window' }); }
                        else { $cancelBtn.hide(); }
                        $row.attr('data-bclose', '1');
                        showToast('<i class="fas fa-check mr-1"></i>' + stkName + ' (' + val + ') tersimpan', 'success');
                    } else {
                        $statusIcon.html('<i class="fas fa-exclamation-triangle text-danger"></i>').show();
                        showToast('<i class="fas fa-exclamation mr-1"></i>Gagal: ' + (res.message || 'Error'), 'danger');
                    }
                },
                error: function () {
                    $input.removeClass('saving');
                    $statusIcon.html('<i class="fas fa-times text-danger"></i>').show();
                    showToast('<i class="fas fa-times mr-1"></i>Gagal koneksi saat menyimpan', 'danger');
                }
            });
        }

        /* ── Cancel opname (X button) ─────────────────────────────── */
        $(document).on('click', '.btn-cancel-opname', function (e) {
            e.preventDefault(); e.stopPropagation();
            var $btn        = $(this);
            var $row        = $btn.closest('tr');
            var $input      = $row.find('.opname-input');
            var $statusIcon = $row.find('.save-status-icon');
            var stkId       = $input.attr('data-stkid');
            var ccId        = $input.attr('data-ccid');
            var targetDate  = $('#<%= txtDate.ClientID %>').val();
            var stkName     = $row.find('td:eq(1)').text().trim();

            if (!confirm('Batalkan opname untuk ' + stkName + '?\n\nNilai opname akan dikembalikan menjadi 0.')) return;
            $btn.tooltip('hide').hide();
            $statusIcon.html('<i class="fas fa-spinner fa-spin text-muted"></i>').show();

            $.ajax({
                type: 'POST',
                url: 'Pages_DailyOpname.aspx?action=cancel_row',
                data: { stkId: stkId, ccId: ccId, date: targetDate },
                dataType: 'json',
                success: function (res) {
                    if (res && res.success) {
                        $input.val('0').attr('data-orig', '0').removeAttr('data-modified');
                        recalculateRow($input);
                        $statusIcon.html('<i class="fas fa-check text-info"></i>').show();
                        setTimeout(function () { $statusIcon.fadeOut(200); }, 1500);
                        showToast('<i class="fas fa-undo mr-1"></i>Opname ' + stkName + ' dibatalkan (0)', 'info');
                    } else {
                        $btn.show();
                        $statusIcon.html('<i class="fas fa-exclamation-triangle text-danger"></i>').show();
                        showToast('<i class="fas fa-exclamation mr-1"></i>Gagal: ' + (res.message || 'Error'), 'danger');
                    }
                },
                error: function () {
                    $btn.show();
                    $statusIcon.html('<i class="fas fa-times text-danger"></i>').show();
                    showToast('<i class="fas fa-times mr-1"></i>Gagal koneksi saat membatalkan', 'danger');
                }
            });
        });

        /* ── Bulk Submit with JSON payload ────────────────────────── */
        function confirmSubmit() {
            var modifiedItems = [];
            $('.opname-input').each(function () {
                var isModified = $(this).attr('data-modified') === 'true';
                var val        = $(this).val().trim();
                var origVal    = $(this).attr('data-orig') || '';
                if ((isModified || val !== origVal) && val !== '') {
                    var stkId = parseInt($(this).attr('data-stkid')) || 0;
                    var ccId  = parseInt($(this).attr('data-ccid'))  || 0;
                    if (stkId > 0 && ccId > 0) modifiedItems.push({ stkId: stkId, ccId: ccId, val: val });
                }
            });

            if (modifiedItems.length === 0) {
                alert('Peringatan: Belum ada baris kuantitas opname yang diedit atau diubah.');
                return false;
            }
            var res = confirm('Apakah Anda yakin ingin menyimpan ' + modifiedItems.length +
                ' item Stock Opname yang telah diubah?\n\n' +
                'Nilai akan menjadi CloseQty H-1 dan OpenQty Hari Berjalan.');
            if (res) {
                var hf = document.getElementById('<%= hfOpnameJson.ClientID %>');
                if (hf) hf.value = JSON.stringify(modifiedItems);
                $('#spinner').show();
                return true;
            }
            return false;
        }
    </script>

</asp:Content>
