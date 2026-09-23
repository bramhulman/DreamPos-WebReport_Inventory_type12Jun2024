<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
  CodeBehind="Pages_UsageList.aspx.vb" Inherits="DreamPos_WebReport.Pages_UsageList" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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
        $(window).on('load', HideProgress);
      </script>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
      <asp:ScriptManager runat="server"></asp:ScriptManager>

      <div class="content-header">
        <div class="container-fluid">
          <div class="row mb-2">
            <div class="col-sm-6">
              <h1 class="m-0 text-dark">Usage List</h1>
            </div>
            <div class="col-sm-6">
              <ol class="breadcrumb float-sm-right">
                <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                <li class="breadcrumb-item active">Usage List</li>
              </ol>
            </div>
          </div>
        </div>
      </div>

      <div class="card-body" style="overflow:scroll;">

        <div runat="server" id="usagestockdiv" class="form-horizontal" style="margin:20px 10px; width:98%;">
          <asp:Label runat="server" ID="lblusage" Text="Usage Stock" Visible="false">

          </asp:Label>
          <asp:Button ID="btnExportUsage" runat="server" Text="Export to Excel" CssClass="btn btn-info"
            style="font-weight: 600; border-color:gray; width:200px; margin:5px;" Visible="false"
            OnClick="btnExportUsage_Click" OnClientClick="startExport(); return true;" />
          <asp:DataGrid runat="server" ID="dg1" HeaderStyle-backcolor="#4b6c9e" HeaderStyle-ForeColor="White"
            HeaderStyle-Font-Bold="true" HeaderStyle-BorderColor="White" HeaderStyle-HorizontalAlign="Center"
            AllowSorting="true" AlternatingItemStyle-BackColor="#ccffff" ItemStyle-HorizontalAlign="Center"
            ItemStyle-BackColor="WhiteSmoke" Width="100%" CssClass="jsgrid" AllowPaging="true" PageSize="31"
            OnPageIndexChanged="dg1_PageIndexChanged" PagerStyle-Mode="NumericPages" PagerStyle-Visible="false"
            PagerStyle-Position="Bottom" PagerStyle-HorizontalAlign="Center" PagerStyle-NextPageText="Next"
            PagerStyle-PrevPageText="Prev">
          </asp:DataGrid>
        </div>




        <table>
          <tr>
            <td style="text-align:center">
              <table style="width:40%" border="0" runat="server" id="usagepage" visible="false">
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
                    <asp:Label runat="server" ID="pagelbl" Text=" Page " Font-Bold="True"
                      style="display:inline-block; width:108px;"></asp:Label>
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

      <div class="loading">
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

    </asp:Content>