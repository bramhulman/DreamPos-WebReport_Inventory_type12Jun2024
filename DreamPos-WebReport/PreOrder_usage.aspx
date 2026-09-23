<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="PreOrder_usage.aspx.vb" Inherits="DreamPos_WebReport.PreOrder_usage" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
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
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            function ShowProgress() {
                setTimeout(function () {
                    var modal = $('<div />');
                    modal.addClass("modal");
                    $('body').append(modal);
                    var loading = $(".loading");
                    loading.show();
                }, 200);
            }

            function HideProgress() {
                $('.loading').fadeOut(300);
                $('.modal').remove();
            }

            // Sembunyikan loading saat page load selesai
            $(window).on('load', HideProgress);

            // Untuk UpdatePanel biasa (AsyncPostBack)
            if (prm) {
                prm.add_endRequest(HideProgress);
            }

            function startExport() {
                ShowProgress();
                // Sembunyikan otomatis setelah 8 detik (cukup untuk download + safety)
                setTimeout(function () {
                    HideProgress();
                }, 8000);
            }

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
            function scrollToUsage() {
                document.getElementById("<%= usagestockdiv.ClientID %>")
                    .scrollIntoView({ behavior: "smooth" });
            }

            function InitDatePicker() {
                $("#<%= txtFrom.ClientID %>").datepicker({
                    dateFormat: "dd M yy",
                    changeMonth: true,
                    changeYear: true
                });
                $("#<%= txtTo.ClientID %>").datepicker({
                    dateFormat: "dd M yy",
                    changeMonth: true,
                    changeYear: true
                });
            }

            $(function () {
                InitDatePicker();
                // Re-initialize on UpdatePanel partial postback
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitDatePicker);
            });
        </script>

        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0 text-dark">Pre-Order</h1>
                    </div><!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Pre-Order</li>
                        </ol>
                    </div><!-- /.col -->
                </div><!-- /.row -->
            </div><!-- /.container-fluid -->
        </div>
        <div class="col-md-12" style="background-color:white;">

            <!-- /.content-header -->
            <%-- <h2>User Access</h2>--%>
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
                <div class="row">
                    <div class="form-horizontal" id="formload" style="width:100%">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>


                                <div class="card-body" style="overflow:scroll;">
                                    <div runat="server" id="usagestockdiv" class="form-horizontal"
                                        style="margin:20px 10px; width:98%;">
                                        <div class="row mb-3 align-items-end">

                                            <div class="col-12 col-md-auto mb-2 mb-md-0">
                                                <label class="mb-1 font-weight-bold">Usage Stock From</label>
                                            </div>

                                            <div class="col-12 col-md-3 mb-2 mb-md-0">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="far fa-calendar-alt"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox runat="server" ID="txtFrom" CssClass="form-control"
                                                        AutoPostBack="false" style="text-align:center;width:15%;" />
                                                </div>
                                            </div>

                                            <div class="col-12 col-md-auto mb-2 mb-md-0">
                                                <label class="mb-1 font-weight-bold">To</label>
                                            </div>

                                            <div class="col-12 col-md-3 mb-2 mb-md-0">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">
                                                            <i class="far fa-calendar-alt"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox runat="server" ID="txtTo" CssClass="form-control"
                                                        AutoPostBack="false" style="text-align:center;width:15%;" />
                                                </div>
                                            </div>

                                            <div class="col-6 col-md-auto mb-2 mb-md-0">
                                                <asp:Button ID="btnFilter" runat="server" Text="Filter"
                                                    CssClass="btn btn-primary btn-block" />
                                            </div>

                                            <div class="col-6 col-md-auto mb-2 mb-md-0">
                                                <asp:Button ID="btnExportUsage" runat="server" Text="Export to Excel"
                                                    CssClass="btn btn-info btn-block"
                                                    style="font-weight:600; border-color:gray;" Visible="false"
                                                    OnClick="btnExportUsage_Click"
                                                    OnClientClick="startExport(); return true;" />
                                            </div>

                                        </div>


                                        <asp:DataGrid runat="server" ID="dg1" HeaderStyle-backcolor="#4b6c9e"
                                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"
                                            HeaderStyle-BorderColor="White" HeaderStyle-HorizontalAlign="Center"
                                            AllowSorting="true" AlternatingItemStyle-BackColor="#ccffff"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke"
                                            Width="100%" CssClass="jsgrid" AllowPaging="true" PageSize="31"
                                            OnPageIndexChanged="dg1_PageIndexChanged" PagerStyle-Mode="NumericPages"
                                            PagerStyle-Visible="false" PagerStyle-Position="Bottom"
                                            PagerStyle-HorizontalAlign="Center" PagerStyle-NextPageText="Next"
                                            PagerStyle-PrevPageText="Prev">
                                        </asp:DataGrid>
                                    </div>




                                    <table>
                                        <tr>
                                            <td style="text-align:center">
                                                <table style="width:40%" border="0" runat="server" id="usagepage"
                                                    visible="false">
                                                    <tr>
                                                        <td style="text-align:center">
                                                            <asp:button runat="server" text="First" id="btnFirst">
                                                            </asp:button>

                                                        </td>
                                                        <td style="text-align:center">

                                                            <asp:button runat="server" text="Prev" id="btnPrev">
                                                            </asp:button>
                                                        </td>
                                                        <td style="text-align:center">
                                                            <asp:Label runat="server" ID="pagelbl" Text=" Page "
                                                                Font-Bold="True"
                                                                style="display:inline-block; width:108px;">
                                                            </asp:Label>
                                                        </td>
                                                        <td style="text-align:center">
                                                            <asp:button runat="server" text="Next" id="btnNext">
                                                            </asp:button>

                                                        </td>
                                                        <td style="text-align:center">
                                                            <asp:button runat="server" text="Last" id="btnLast">
                                                            </asp:button>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExportUsage" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
        </div>
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>