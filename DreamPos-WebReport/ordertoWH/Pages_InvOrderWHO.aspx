<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_InvOrderWHO.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvOrderWHO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <style type="text/css">
        .displayaction { }
        .nodisplay { display: none; }
        .mycheckbox input[type="checkbox"] { margin-right: 10px; }
        [disabled] {
            color: #933;
            background-color: #ffc;
        }

        .inv-who-page {
            padding: 0 15px 1rem;
        }

        #<%= formload.ClientID %> {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 0.65rem 1rem;
            width: 100%;
            max-width: 100%;
            margin: 0 0 1rem 0;
            padding: 0.65rem 1rem;
            background: #3a4f63;
            border-radius: 6px;
            box-sizing: border-box;
        }

        .inv-who-filter {
            display: flex;
            align-items: center;
            flex: 1 1 180px;
            min-width: 0;
            margin: 0;
        }

        .inv-who-filter-name {
            flex: 2 1 220px;
        }

        .inv-who-filter label,
        .inv-who-filter b {
            color: #fff;
            margin: 0 0.5rem 0 0;
            white-space: nowrap;
            font-size: 0.9rem;
        }

        .inv-who-filter .form-control,
        .inv-who-filter select {
            flex: 1 1 auto;
            min-width: 0;
            max-width: 100%;
            height: auto !important;
            line-height: 1.5;
            padding: 10px 12px;
            overflow: visible;
        }

        #<%= formload.ClientID %> select.form-control {
            -webkit-appearance: menulist;
            appearance: auto;
        }

        .inv-who-filter-actions {
            display: flex;
            align-items: center;
            flex: 0 0 auto;
            margin-left: auto;
        }

        .inv-who-filter-actions .btn-search-who {
            background: #fff;
            color: #3a4f63;
            border: none;
            height: auto;
            min-width: 42px;
            padding: 8px 12px;
            border-radius: 4px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
        }

        .inv-who-filter-actions .btn-search-who:hover {
            background: #e8eef3;
        }

        .inv-who-table-wrap {
            width: 100%;
            margin: 0;
        }

        #tblEmpDetails thead th {
            background: #b8d4e8;
            color: #414a4c;
            vertical-align: middle;
            text-align: center;
            white-space: nowrap;
            font-size: 0.85rem;
            padding: 0.5rem 0.4rem;
        }

        #tblEmpDetails tbody td {
            vertical-align: middle;
            font-size: 0.9rem;
        }

        #tblEmpDetails tbody td:last-child {
            text-align: center;
            white-space: nowrap;
        }

        .inv-who-pager {
            margin-top: 0.5rem;
        }

        @media (max-width: 991px) {
            .inv-who-filter {
                flex: 1 1 100%;
            }
            .inv-who-filter-actions {
                margin-left: 0;
                width: 100%;
                justify-content: flex-end;
            }
        }
    </style>
      <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
<link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />--%>
    
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
        //$(function () {
        //    $("#tblEmpDetails").DataTable();
        //});
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
          function SendEmail2() {
              var confirm_value = document.createElement("INPUT");
              confirm_value.type = "hidden";
              confirm_value.name = "confirm_value";
              if (confirm("Do you want to Show DO File?")) {
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
            <h1 class="m-0 text-dark">Order Stock From Store</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Order Stock From Store</li>
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
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Order</asp:ListItem>
                    <asp:ListItem Value="2">Delivery</asp:ListItem>
                    <asp:ListItem Value="3">Receive</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="inv-who-filter">
                <label for="<%= DdlCc.ClientID %>"><b>CC:</b></label>
                <asp:DropDownList ID="DdlCc" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    CssClass="form-control" EnableViewState="True" ViewStateMode="Enabled">
                    <asp:ListItem Value="0">All</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="inv-who-filter inv-who-filter-name">
                <label for="<%= txtsearch.ClientID %>"><b>Name:</b></label>
                <asp:TextBox ID="txtsearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
            </div>
            <div class="inv-who-filter-actions">
                <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" type="button"
                    class="btn btn-search-who" title="Search">
                    <i class="fas fa-search"></i>
                </button>
            </div>
        </div>

        <div class="inv-who-table-wrap">
          <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
              <LayoutTemplate>
    <div class="table-responsive">
        <table id="tblEmpDetails" width="100%" class="menutab table table-bordered table-sm table-hover mb-0">
            <thead>
                <tr role="row">
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">ID</b>
                    </th>

                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">DO NO</b>
                    </th>

                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">From</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">To</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Status</b>
                    </th>
                      <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Date</b>
                    </th>
                      <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Expected Delivery</b>
                    </th>
                     
                   <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
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
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                    </td>

                                    <td>
                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("delId")%>'></asp:Label>
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
                        <%--//==== Here we have used CommandName property to distinguish which button is 
                        clicked and we have passed our primary key to CommandArgument property. ====//--%>
                        <asp:ImageButton Style="margin-right:0.5rem;" ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                            CommandArgument='<%#Eval("ID").ToString + ";" + Eval("ccTo").ToString + ";" + Eval("ccFrom").ToString + ";" + Eval("statdate").ToString + ";" + Eval("DelID").ToString + ";" + Eval("STATUS").ToString + ";" + Eval("Notes").ToString + ";" + Eval("createddate").ToString + ";" + Eval("deldate").ToString + ";" + Eval("rcvdate").ToString %>' runat="server" ImageUrl="images/edit.png">
                       </asp:ImageButton>
                         <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="Delete a record." OnClientClick="Confirm()" class= '<%# GetBackground() %>'
                            ID="ImageButton2" Visible ='true' CommandName="Del" CommandArgument='<%#Eval("ID").tostring+ ";" +Eval("ccTo").Tostring%>' runat="server"
                            ImageUrl="~/images/delete.png"></asp:ImageButton>
                          <asp:ImageButton BackColor="Black"  ToolTip="Print PDF." CssClass="btn2" OnClientClick="SendEmail()"
                            ID="imgEmail" CommandName="Send" CommandArgument='<%#Eval("ID")+ ";"%>' runat="server"
                            ImageUrl="images/print.png"></asp:ImageButton>
                         <asp:ImageButton Style="margin-right:0.5rem;"  ToolTip="Print Delivery Order."  OnClientClick="SendEmail2()" class= '<%# GetBackgroundDO() %>'
                            ID="ImageButton1" Visible ='true' CommandName="Send2" CommandArgument='<%#Eval("ID")+ ";"%>' runat="server"
                            ImageUrl="images/list.png"></asp:ImageButton>
                       
                    </td>




                    </tr>
                </ItemTemplate>
            </asp:ListView>
        <%--</table>--%>

        <div class="inv-who-pager d-flex align-items-center flex-wrap">
            <asp:Label runat="server" Font-Bold="True" CssClass="mr-2 mb-0">Page:</asp:Label>
            <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="15">
                <Fields>
                    <asp:NumericPagerField ButtonType="Link" />
                </Fields>
            </asp:DataPager>
        </div>
        </div>
    </div>
</asp:Content>
