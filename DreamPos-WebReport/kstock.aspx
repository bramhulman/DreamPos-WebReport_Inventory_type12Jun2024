<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteReport.Master" CodeBehind="kstock.aspx.vb"
  Inherits="DreamPos_WebReport.kstock" %>

  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <title runat="server" id="reporttitle">
          <%= ddlRpt.Text.ToString & " " & txtFrom.Text & " - " & txtTo.Text %>
        </title>
      <!-- DataTables Buttons CSS -->
      <link rel="stylesheet" href="plugins/datatables-buttons/css/buttons.bootstrap4.min.css" />
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
      <!-- Manual Include of Libraries (Matching reportviewer_3.aspx pattern for robustness) -->
      <!-- jQuery -->


      <!-- DataTables Buttons & Dependencies -->
      <script src="plugins/jszip/jszip.min.js"></script>
      <script src="plugins/pdfmake/pdfmake.min.js"></script>
      <script src="plugins/pdfmake/vfs_fonts.js"></script>
      <script src="plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
      <script src="plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
      <script src="plugins/datatables-buttons/js/buttons.html5.min.js"></script>
      <script src="plugins/datatables-buttons/js/buttons.print.min.js"></script>
      <script src="plugins/datatables-buttons/js/buttons.colVis.min.js"></script>

      <script type="text/javascript">
        // PageRequestManager to handle UpdatePanel postbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
          createDataTable();
        });

        $(function () {
          createDataTable();
        });

        function createDataTable() {
          if ($.fn.DataTable.isDataTable('#<%= dg1.ClientID %>')) {
            $('#<%= dg1.ClientID %>').DataTable().destroy();
          }

          // Helper: convert Indonesian number format (1.316,00) to US English (1316.00)
          function toExportNumber(data) {
            if (data === null || data === undefined) return "";
            var clean = typeof data === 'string' ? data : data.toString();
            clean = $('<div>').html(clean).text().trim(); // strip HTML tags
            // Match Indonesian number: digits with optional dot-thousands and comma-decimal
            if (/^-?(\d{1,3}(\.\d{3})*|\d+)(,\d+)?$/.test(clean)) {
              var noThousands = clean.replace(/\./g, '');   // remove thousand sep (dots)
              var withDot    = noThousands.replace(',', '.'); // decimal comma → dot
              if (!isNaN(withDot) && withDot !== '') {
                return withDot;
              }
            }
            return clean;
          }

          var exportOpts = {
            format: {
              body: function (data, row, column, node) {
                return toExportNumber(data);
              }
            }
          };

          $("#<%= dg1.ClientID %>").DataTable({
            "destroy": true,
            "responsive": true,
            "lengthChange": true,
            "autoWidth": false,
            "paging": true,
            "searching": true,
            "ordering": true,
            "order": [],
            "info": true,
            "dom": '<"row"<"col-md-6"B><"col-md-6"f>>rtip',
            "buttons": [
              {
                extend: 'copy',
                text: 'Copy',
                className: 'btn btn-light btn-sm border',
                exportOptions: exportOpts
              },
              {
                extend: 'csv',
                text: 'CSV',
                className: 'btn btn-light btn-sm border',
                exportOptions: exportOpts
              },
              {
                extend: 'excel',
                text: 'Excel',
                className: 'btn btn-light btn-sm border',
                exportOptions: exportOpts
              },
              {
                extend: 'pdf',
                text: 'PDF',
                orientation: 'landscape',
                className: 'btn btn-light btn-sm border',
                exportOptions: exportOpts
              },
              {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-light btn-sm border',
                exportOptions: exportOpts
              },
              {
                extend: 'colvis',
                text: 'Column visibility',
                className: 'btn btn-light btn-sm border'
              }
            ],
            "initComplete": function () {
              var btns = $('.dt-button');
              btns.removeClass('btn-secondary');
            }
          });
        }
        function OnKeyPress(e) {
          if (e.keyCode == "27") {
            $find("ModalBrand").hide();
            $find("ModalCity").hide();
            $find("ModalCc").hide();
            $find("ModalStock").hide();

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

        function openModalBrand() {
          $('#ModalBrand').modal({ show: true });
          //document.getElementById("txtsearch2").focus();
        }
        function openModalCity() {
          $('#ModalCity').modal({ show: true });
          //document.getElementById("txtsearch3").focus();
        }
        function openModalCc() {
          $('#ModalCc').modal({ show: true });
          //document.getElementById("txtsearch").focus();
        }
        function openModalStock() {
          $('#ModalStock').modal({ show: true });
          //document.getElementById("txtsearch").focus();
        }
      </script>
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="LeftContentBar" runat="server">
      <asp:ScriptManager runat="server"></asp:ScriptManager>
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
        $(document).on("submit", "form", function () {
          ShowProgress();
        });
        //untuk dropdownlist
        $(document).ready(function () {

          ddlOptionGrouping();

          //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ddlOptionGrouping);

          function ddlOptionGrouping() {
            /* Add Option Groups to asp:DropdownList */
            var $select = $('#<%= ddlRpt.ClientID %>');
            var optGroup;
            $('#<%= ddlRpt.ClientID %> option').each(function () {
              if ($(this).val() == '<') {
                /* Opener */
                optGroup = $('<optGroup>').attr('label', $(this).text());
              } else if ($(this).val() == '>') {
                /* Closer */
                $('</optGroup>').appendTo(optGroup);
                optGroup.appendTo($select);
                optGroup = null;
              } else {
                /* Normal Item */
                let $option = $('<option>' + $(this).text() + '</option>').attr('value', $(this).val());
                if (this.hasAttribute('selected')) {
                  $option.attr('selected', $(this).attr('selected'));
                }
                if (optGroup) {
                  $option.appendTo(optGroup);
                } else {
                  $option.appendTo($select);
                }
              }
              $(this).remove();
            });
          }
        });
        //akhir untuk dropdowngroup

        // AUTO LOGOUT SCRIPT (10 Minutes)
        var idleTime = 0;
        $(document).ready(function () {
          //Increment the idle time counter every minute.
          var idleInterval = setInterval(timerIncrement, 60000); // 1 minute

          //Zero the idle timer on mouse movement.
          $(this).mousemove(function (e) {
            idleTime = 0;
          });
          $(this).keypress(function (e) {
            idleTime = 0;
          });
        });

        function timerIncrement() {
          idleTime = idleTime + 1;
          if (idleTime >= 10) { // 10 minutes
            window.location.href = 'Default.aspx';
          }
        }
      </script>
      <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
        <!-- Report Selection -->
        <li class="nav-item">
          <div class="px-3 py-2">
            <label class="small text-uppercase text-muted mb-1"
              style="font-size: 0.75rem; letter-spacing: 0.5px;">Select Report</label>
            <asp:DropDownList ID="ddlRpt" runat="server"
              CssClass="form-control form-control-sm shadow-sm border-secondary" AppendDataBoundItems="True"
              AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
              <asp:ListItem Value="Kartu Stock">Kartu Stock</asp:ListItem>
              <asp:ListItem Value="Running Cost">Running Cost</asp:ListItem>
              <asp:ListItem Value="All Trans">All Trans</asp:ListItem>
            </asp:DropDownList>
          </div>
        </li>



        <!-- Cc Selection -->
        <li class="nav-item">
          <table style="width:100%">
            <tr>
              <td rowspan="2" style="width:40%; text-align:center">
                <asp:Label runat="server" AssociatedControlID="btnshowmdlstore" Text="Cc: " ForeColor="Black">
                </asp:Label>
              </td>
              <td style="width:60%; margin-left:10px; text-align:center;">
                <button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshowmdlstore"
                  data-target="#ModalCc" style="padding:1px 4px;">
                  Get Cc
                </button>
              </td>
            </tr>
            <tr>
              <td style="text-align:center;">
                <asp:Label ID="lblCcNameData" runat="server" Text="All Cc" Textalign="center" ForeColor="Black"
                  Visible="false"></asp:Label>
              </td>
            </tr>
            <tr>
              <td rowspan="2" style="width:40%; text-align:center">
                <asp:Label runat="server" AssociatedControlID="btnshowmdlstock" Text="Stock: " ForeColor="Black">
                </asp:Label>
              </td>
              <td style="width:60%; margin-left:10px; text-align:center;">
                <button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshowmdlstock"
                  data-target="#ModalStock" style="padding:1px 4px;">
                  Get Stock
                </button>
              </td>
            </tr>
            <tr>
              <td style="text-align:center;">
                <asp:Label ID="lblStockNameData" runat="server" Text="All Stock" Textalign="center" ForeColor="Black"
                  Visible="false"></asp:Label>
              </td>
            </tr>
          </table>
        </li>

        <!-- Item Parameter (Hidden by default) -->
        <li class="nav-item" id="paramitem" style="display:none" runat="server">
          <div class="px-3 pb-2">
            <div class="d-flex justify-content-between align-items-center mb-1">
              <label class="text-dark mb-0" style="font-weight: 500;">Item:</label>
              <asp:button runat="server" class="btn btn-outline-secondary btn-xs shadow-sm" id="btnItem"
                Text="Get Store" style="min-width: 90px;"></asp:button>
            </div>
            <asp:Label ID="LblNmItem" runat="server" Text="All Item"
              CssClass="d-block text-muted small text-right italic"></asp:Label>
          </div>
        </li>

        <!-- Date From -->
        <li class="nav-item">
          <div class="px-3 pb-2" id="calFrom" runat="server">
            <asp:Label CssClass="small text-muted text-uppercase mb-1" runat="server" ID="lblparamfrom"
              style="font-size: 0.7rem;" Text="Date From"></asp:Label>
            <div class="input-group input-group-sm">
              <div class="input-group-prepend">
                <span class="input-group-text border-0 bg-light text-muted"><i class="far fa-calendar-alt"></i></span>
              </div>
              <asp:TextBox runat="server" ID="txtFrom" CssClass="form-control border-0 text-center font-weight-bold"
                AutoPostBack="false" style="color: #495057;" />
              <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFrom"
                Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
            </div>
          </div>
        </li>

        <!-- Date To -->
        <li class="nav-item">
          <div class="px-3 pb-3" id="paramto" runat="server">
            <asp:Label CssClass="small text-muted text-uppercase mb-1" runat="server" ID="lblparamto"
              style="font-size: 0.7rem;" Text="Date To"></asp:Label>
            <div class="input-group input-group-sm">
              <div class="input-group-prepend">
                <span class="input-group-text border-0 bg-light text-muted"><i class="far fa-calendar-alt"
                    runat="server" id="iconparamto"></i></span>
              </div>
              <asp:TextBox runat="server" ID="txtTo" CssClass="form-control border-0 text-center font-weight-bold"
                AutoPostBack="false" style="color: #495057;" />
              <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo"
                Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
            </div>
          </div>
        </li>

        <!-- Action Button -->
        <li class="nav-item px-3 mt-3 mb-4">
          <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click1" Text="GENERATE REPORT"
            CssClass="btn btn-primary btn-block font-weight-bold shadow elevation-2 text-uppercase"
            style="letter-spacing: 0.8px;" />
        </li>

        <!-- Hidden Exports & Labels -->
        <li class="nav-item d-none">
          <asp:Button Text="Export CSV" ID="btnExport" runat="server" OnClick="btnExport_Click"
            CssClass="btn btn-primary" UseSubmitBehavior="false" Visible="false" />
          <asp:Button Text="Export Excel" ID="btnExport2" runat="server" OnClick="btnExport2_Click"
            CssClass="btn btn-primary" UseSubmitBehavior="false" Visible="false" />
          <asp:Label ID="lblnobrand" runat="server" Text="0" Visible="false"></asp:Label>
          <asp:Label ID="lblnoCity" runat="server" Text="0" Visible="false"></asp:Label>
          <asp:Label ID="lblNoOutlet" runat="server" Text="0" Visible="false"></asp:Label>
          <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>
          <asp:Label ID="lblnostock" runat="server" Text="0" Visible="false"></asp:Label>
        </li>
      </ul>
      <div class="loading" style="text-align:center">
        Loading... Please wait...<br />
        <br />
        <img src="/images/loader.gif" alt="" />
      </div>
    </asp:Content>
    <asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
      <section class="content-header">
        <div class="container-fluid">
          <div class="row mb-2">
            <div class="col-sm-6">
              <h1>Report</h1>
            </div>
            <asp:Label ID="lblMsg" runat="server" Visible="false">
            </asp:Label>
            <div class="col-sm-6">
              <ol class="breadcrumb float-sm-right">
                <li class="breadcrumb-item"><a href="/Inventoryhome.aspx">Home</a></li>
                <li class="breadcrumb-item active">Report</li>
              </ol>
            </div>
          </div>
        </div><!-- /.container-fluid -->
      </section>
      <div class="card">
        <div class="card-header">
          <%--<h3 class="card-title">Report Menu</h3>--%>
            <asp:Label runat="server" ID="ReportName" Text="Report Menu" Font-Size="Large" Font-Bold="True"></asp:Label>
            <div class="card-tools">
              <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip"
                title="Collapse">
                <i class="fas fa-minus"></i></button>
              <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                <i class="fas fa-times"></i></button>
            </div>
        </div>
        <div class="card-body" style="overflow:scroll;">
          <asp:GridView runat="server" ID="dg1" HeaderStyle-backcolor="#4b6c9e" HeaderStyle-ForeColor="White"
            HeaderStyle-Font-Bold="true" HeaderStyle-BorderColor="White" HeaderStyle-HorizontalAlign="Center"
            AllowSorting="true" AlternatingItemStyle-BackColor="#ccffff" ItemStyle-HorizontalAlign="Center"
            ItemStyle-BackColor="WhiteSmoke" Width="100%" CssClass="table table-striped table-bordered table-hover"
            AutoGenerateColumns="false" GridLines="None" UseAccessibleHeader="true">
          </asp:GridView>
        </div>




        <!-- /.card-body -->
        <div class="card-footer">
          <%--Report Menu--%>
        </div>
        <!-- /.card-footer-->
      </div>

      <!-- Modal Brand -->
      <%-- <asp:UpdatePanel ID="UpdatePanel2" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


          <div class="modal fade" id="ModalBrand" tabindex="-1" role="dialog" aria-labelledby="ModalBrand"
            aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
            <div class="modal-dialog modal-dialog-scrollable" role="document">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title" id="ModalBrandLabel">Select Brand</h5>
                  <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>

                </div>

                <div class="row">
                  <div class="form-horizontal" id="formload" runat="server" style="width:100%;">
                    <div class="card-body">




                      <div class="form-group row">
                        <asp:Label runat="server" AssociatedControlID="txtSearch2"
                          CssClass="col-sm-2 col-form-label text-nowrap" Text="Brand: "></asp:Label>

                        <div class="input-group-append">
                          <div class="input-group input-group-sm">
                            <asp:TextBox id="txtSearch2" runat="server" CssClass="form-control" placeholder="Search"
                              type="search" aria-label="Search"></asp:TextBox>

                            <button runat="server" id="btnSearch2" onserverclick="btnSearch2_ServerClick"
                              class="btn btn-mini" title="Search">
                              <i class="fas fa-search"></i>
                            </button>


                          </div>
                        </div>

                      </div>
                    </div>

                  </div>



                </div>

                <div class="modal-body">
                  <div class="table-responsive">
                    <table class="table">
                      <thead>
                        <tr>
                          <th scope="col">
                            <asp:CheckBox ID="Checkbox_allbrand" runat="server" ToolTip="SelectAll"
                              onclick="javascript:fnSelectAll();" OnCheckedChanged="Checkbox_allbrand_CheckedChanged1">
                            </asp:CheckBox>
                          </th>
                          <th scope="col" hidden="hidden">ID</th>
                          <th scope="col">Brand</th>
                        </tr>
                      </thead>
                      <tbody>

                        <asp:ListView ID="ListViewBrand" runat="server">
                          <ItemTemplate>
                            <tr>
                              <td>
                                <asp:CheckBox ID="CheckBox2" runat="server"></asp:CheckBox>
                              </td>
                              <td hidden="hidden">
                                <asp:Label ID="LblIdBrand" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                              </td>
                              <td>
                                <asp:Label ID="lblBrand2" runat="server" Text=' <%# Eval("nama")%>'></asp:Label>
                              </td>
                            </tr>
                          </ItemTemplate>
                        </asp:ListView>


                      </tbody>
                    </table>
                  </div>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                  <%-- <button type="button" class="btn btn-primary" runat="server"
                    onserverclick="btnSubmit2_Click">Select</button>
                    <button type="button" id="btnAll2" runat="server" class="btn btn-primary"
                      onserverclick="btnAll2_Click">SelectAll</button>--%>
                    <%--<button type="button" class="btn btn-primary" runat="server"
                      onserverclick="RadButton3_ServerClick" id="RadButton3">Select</button>--%>
                      <%--<button type="button" class="btn btn-primary" runat="server"
                        onserverclick="btnAll3_ServerClick">SelectAll</button>--%>
                        <%--< /div>
                </div>
              </div>
            </div>

        </ContentTemplate>
        </asp:UpdatePanel>--%>
        <!-- Modal Brand -->

        <!-- Modal City -->
        <%-- <asp:UpdatePanel ID="UpdatePanel4" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
          <ContentTemplate>


            <div class="modal fade" id="ModalCity" tabindex="-1" role="dialog" aria-labelledby="ModalCity"
              aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
              <div class="modal-dialog modal-dialog-scrollable" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="ModalCityLabel">Select City</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                  </div>

                  <div class="row">
                    <div class="form-horizontal" id="Div1" runat="server" style="width:100%;">
                      <div class="card-body">




                        <div class="form-group row">
                          <asp:Label runat="server" AssociatedControlID="txtSearch3"
                            CssClass="col-sm-2 col-form-label text-nowrap" Text="City: "></asp:Label>

                          <div class="input-group-append">
                            <div class="input-group input-group-sm">
                              <asp:TextBox id="txtSearch3" runat="server" CssClass="form-control" placeholder="Search"
                                type="search" aria-label="Search"></asp:TextBox>


                              <button runat="server" id="btnSearch3" onserverclick="btnSearch3_ServerClick"
                                class="btn btn-mini" title="Search">
                                <i class="fas fa-search"></i>
                              </button>


                            </div>
                          </div>

                        </div>
                      </div>

                    </div>



                  </div>

                  <div class="modal-body">
                    <div class="table-responsive">
                      <table class="table">
                        <thead>
                          <tr>
                            <th scope="col">
                              <asp:CheckBox ID="Checkbox_allcity" runat="server" ToolTip="SelectAll"
                                onclick="javascript:fnSelectAllCity();"
                                OnCheckedChanged="Checkbox_allcity_CheckedChanged1"></asp:CheckBox>
                            </th>
                            <th scope="col" hidden="hidden">ID</th>
                            <th scope="col">City</th>
                          </tr>
                        </thead>
                        <tbody>

                          <asp:ListView ID="ListViewCity" runat="server">
                            <ItemTemplate>
                              <tr>
                                <td>
                                  <asp:CheckBox ID="CheckBox3" runat="server"></asp:CheckBox>
                                </td>
                                <td hidden="hidden">
                                  <asp:Label ID="LblIdCity" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                                <td>
                                  <asp:Label ID="lblCity2" runat="server" Text=' <%# Eval("kota")%>'></asp:Label>
                                </td>
                              </tr>
                            </ItemTemplate>
                          </asp:ListView>


                        </tbody>
                      </table>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary" runat="server"
                      onserverclick="btnSubmit3_Click">Select</button>
                      <button type="button" id="btnAll3" runat="server" class="btn btn-primary"
                        onserverclick="btnAll3_Click">SelectAll</button>--%>
                      <%--<button type="button" class="btn btn-primary" runat="server"
                        onserverclick="RadButton2_ServerClick" id="RadButton2">Select</button>--%>
                        <%--<button type="button" class="btn btn-primary" runat="server"
                          onserverclick="btnAll2_ServerClick">SelectAll</button>--%>
                          <%--< /div>
                  </div>
                </div>
              </div>

          </ContentTemplate>
          </asp:UpdatePanel>--%>
          <!-- Modal City -->

          <!-- Modal CC -->
          <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>


              <div class="modal fade" id="ModalCc" tabindex="-1" role="dialog" aria-labelledby="ModalCc"
                aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                <div class="modal-dialog modal-dialog-scrollable" role="document">
                  <div class="modal-content">
                    <div class="modal-header">
                      <h5 class="modal-title" id="ModalCcLabel">Select Cc</h5>
                      <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                      </button>
                    </div>

                    <div class="row">
                      <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                        <div class="card-body">




                          <div class="form-group row">
                            <asp:Label runat="server" AssociatedControlID="txtSearch"
                              CssClass="col-sm-2 col-form-label text-nowrap" Text="Cc: "></asp:Label>

                            <div class="input-group-append">
                              <div class="input-group input-group-sm">
                                <asp:TextBox id="txtSearch" runat="server" CssClass="form-control" placeholder="Search"
                                  type="search" aria-label="Search"></asp:TextBox>


                                <button runat="server" id="btnSearch" onserverclick="btnSearch_ServerClick"
                                  class="btn btn-mini" title="Search">
                                  <i class="fas fa-search"></i>
                                </button>
                                <button runat="server" id="btnClear" onserverclick="btnClear_ServerClick"
                                  class="btn btn-mini" title="Clear Search">
                                  <i class="fas fa-times"></i>
                                </button>


                              </div>
                            </div>

                          </div>
                        </div>

                      </div>



                    </div>

                    <div class="modal-body">
                      <div class="table-responsive">
                        <table class="table">
                          <thead>
                            <tr>
                              <th scope="col">
                                <asp:CheckBox ID="Checkbox_allstore" runat="server" ToolTip="SelectAll"
                                  onclick="javascript:fnSelectAllStore();"
                                  OnCheckedChanged="Checkbox_allstore_CheckedChanged1"></asp:CheckBox>
                              </th>
                              <%--<th scope="col" hidden="hidden">ID</th>--%>
                                <th scope="col" hidden="hidden">Olnumber</th>
                                <th scope="col">Stock</th>
                            </tr>
                          </thead>
                          <tbody>

                            <asp:ListView ID="LsvStk" runat="server">
                              <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                              </LayoutTemplate>
                              <ItemTemplate>
                                <tr>
                                  <td>
                                    <asp:CheckBox ID="CheckBox" runat="server"></asp:CheckBox>
                                  </td>
                                  <%--<td hidden="hidden">
                                    <asp:Label ID="lblID" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>--%>
                                    <td hidden="hidden">
                                      <asp:Label ID="lblkode" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>
                                    <td>
                                      <asp:Label ID="lblName" runat="server" Text=' <%# Eval("Name")%>'></asp:Label>
                                    </td>
                                </tr>
                              </ItemTemplate>
                            </asp:ListView>


                          </tbody>
                        </table>
                      </div>
                    </div>
                    <div class="modal-footer">
                      <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                      <button type="button" class="btn btn-primary" runat="server"
                        onserverclick="RadButton1_ServerClick" id="RadButton1">Select</button>
                      <%--<button id="btnAll1" type="button" runat="server" class="btn btn-primary"
                        onserverclick="btnAll1_ServerClick">SelectAll</button>--%>
                        <%--<button type="button" class="btn btn-primary" runat="server"
                          onserverclick="btnSubmit_Click">Select</button>
                          <button type="button" id="btnAll" runat="server" class="btn btn-primary"
                            onserverclick="btnAll_Click">SelectAll</button>--%>
                    </div>
                  </div>
                </div>
              </div>

            </ContentTemplate>
          </asp:UpdatePanel>
          <!-- Modal CC -->

          <!-- Modal Stock -->
          <asp:UpdatePanel ID="UpdatePanelStock" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>


              <div class="modal fade" id="ModalStock" tabindex="-1" role="dialog" aria-labelledby="ModalStock"
                aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
                <div class="modal-dialog modal-dialog-scrollable" role="document">
                  <div class="modal-content">
                    <div class="modal-header">
                      <h5 class="modal-title" id="ModalStockLabel">Select Stock</h5>
                      <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                      </button>
                    </div>

                    <div class="row">
                      <div class="form-horizontal" id="Div3" runat="server" style="width:100%;">
                        <div class="card-body">




                          <div class="form-group row">
                            <asp:Label runat="server" AssociatedControlID="txtSearchStock"
                              CssClass="col-sm-2 col-form-label text-nowrap" Text="Stock: "></asp:Label>

                            <div class="input-group-append">
                              <div class="input-group input-group-sm">
                                <asp:TextBox id="txtSearchStock" runat="server" CssClass="form-control"
                                  placeholder="Search" type="search" aria-label="Search"></asp:TextBox>


                                <button runat="server" id="BtnSearchStock" onserverclick="BtnSearchStock_ServerClick"
                                  class="btn btn-mini" title="Search">
                                  <i class="fas fa-search"></i>
                                </button>
                                <button runat="server" id="btnClearStock" onserverclick="btnClearStock_ServerClick"
                                  class="btn btn-mini" title="Clear Search">
                                  <i class="fas fa-times"></i>
                                </button>


                              </div>
                            </div>

                          </div>
                        </div>

                      </div>



                    </div>

                    <div class="modal-body">
                      <div class="table-responsive">
                        <table class="table">
                          <thead>
                            <tr>
                              <th scope="col">
                                <asp:CheckBox ID="CheckboxAllStock" runat="server" ToolTip="SelectAll"
                                  onclick="javascript:fnSelectAllStock();"
                                  OnCheckedChanged="CheckboxAllStock_CheckedChanged"></asp:CheckBox>
                              </th>
                              <%--<th scope="col" hidden="hidden">ID</th>--%>
                                <th scope="col" hidden="hidden">StockNo</th>
                                <th scope="col">Stock</th>
                            </tr>
                          </thead>
                          <tbody>

                            <asp:ListView ID="LsvStock" runat="server">
                              <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                              </LayoutTemplate>
                              <ItemTemplate>
                                <tr>
                                  <td>
                                    <asp:CheckBox ID="CheckBoxStock" runat="server"></asp:CheckBox>
                                  </td>
                                  <%--<td hidden="hidden">
                                    <asp:Label ID="lblID" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>--%>
                                    <td hidden="hidden">
                                      <asp:Label ID="lblkodestock" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>
                                    <td>
                                      <asp:Label ID="lblNamestock" runat="server" Text=' <%# Eval("Name")%>'>
                                      </asp:Label>
                                    </td>
                                </tr>
                              </ItemTemplate>
                            </asp:ListView>


                          </tbody>
                        </table>
                      </div>
                    </div>
                    <div class="modal-footer">
                      <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                      <button type="button" class="btn btn-primary" runat="server"
                        onserverclick="RadButtonStock_ServerClick" id="RadButtonStock">Select</button>
                      <%--<button id="btnAll1" type="button" runat="server" class="btn btn-primary"
                        onserverclick="btnAll1_ServerClick">SelectAll</button>--%>
                        <%--<button type="button" class="btn btn-primary" runat="server"
                          onserverclick="btnSubmit_Click">Select</button>
                          <button type="button" id="btnAll" runat="server" class="btn btn-primary"
                            onserverclick="btnAll_Click">SelectAll</button>--%>
                    </div>
                  </div>
                </div>
              </div>

            </ContentTemplate>
          </asp:UpdatePanel>
          <!-- Modal stock -->


    </asp:Content>