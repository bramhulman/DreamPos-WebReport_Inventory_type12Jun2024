<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_InvClosingStock.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvClosingStock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    <style type="text/css">
        .displayaction
        {
        	
           
        }
        .nodisplay
        {
            display: none;
        }
        .mycheckbox input[type="checkbox"] 
{ 
    margin-right: 10px; 
}
       [disabled] { /* Text and background colour, medium red on light yellow */
color:#933;
background-color:#ffc;
}
    </style>
      <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
<link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
    
    <script type="text/javascript">
        <%--$(document).ready(function () {

            $("#<%= txtDate.ClientID  %>").datepicker();

        });--%>
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
            $("#tblEmpDetails").DataTable();
        });
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
    </script>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Closing Stock</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Closing Stock</li>
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
      
        <div class="form-inline" id="formload" runat="server" style="margin:0 20px; width:100%;background: #3a4f63;padding:0.2rem 0.8rem">
             <div class="form-group mx-sm-3 mt-mb-1">
                    <asp:label for="txtDate" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">Date:</b></asp:label>
                    <asp:TextBox style="text-align:center;" runat="server" id="txtDate" Class="form-control" AutoPostBack="false" />
                          <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>  
                 <%--<div class="form-check" style="margin-left:10px;">
                                <asp:CheckBox ID="chkDate" runat="server" AutoPostBack="true" OnCheckedChanged="chkDate_CheckedChanged" style="color: #FFFFFF;" class="form-check-input" type="checkbox" />
                              
                                <label class="form-check-label" for="flexCheckChecked">
                               <b style="color: #FFFFFF;margin-right:0.5rem;"> ByDate</b>
                                </label>
                            </div> --%>
                </div>
                    
             <div class="form-group mx-sm-3 mt-mb-1">
                    <asp:label AssociatedControlID="txtsearch" runat="server">Nama:</asp:Label>
                    <asp:TextBox id="txtsearch" runat="server" Width="300px" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                </div>
             
                <div class="form-group mx-sm-3 mt-mb-1">
                         <div class="btn-group" role="group" style="margin:0.5rem" >
                        <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" class="btn btn-mini inventory-search-button" title="Search" style="background:#FFFFFF;color:#3a4f63;" aria-label="Cari nama"><i class="fas fa-search" aria-hidden="true"></i></button>
                        </div>

                     <%--untuk cc--%>
                    
                       <asp:label for="ddlOutlet" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">Cost Centre: </b></asp:label>
                   <asp:DropDownList ID="ddlOutlet" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged"
                    EnableViewState="True" ViewStateMode="Enabled">
                            <asp:ListItem Value="0">----Select CC----</asp:ListItem>
                        </asp:DropDownList>

                    <div class="btn-group" role="group" style="margin:0.5rem">
                        <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click" class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
                           <asp:Label runat="server">New</asp:Label>
                        </button>
                        </div>
                  
                      </div>
                 
           

                  


                   
                    
                  

                </div>
             
     

    </div>


     <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
          <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
              <LayoutTemplate>
    <div class="table-responsive">
        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%" class="menutab table table-bordered">
            <thead>
                <tr role="row">
                  <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Stok Name</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Cost Centre</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Closing Waste</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Lost Stock</b>
                    </th>
                      <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Closing Stock</b>
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
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("stkname")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("cwasteqty", "{0:0.000}") + " " + Eval("Unit2").ToString  %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("ttlqty", "{0:#,0}") - Eval("cwasteqty", "{0:#,0}")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text=' <%# Eval("varianceqty", "{0:#,0}")%>'></asp:Label>
                    </td>
                    <td class='<%# GetBackground() %>'>
                        <asp:ImageButton ID="imgBtnEdit" CssClass="btn2" CommandName="Edt" ToolTip="Edit a record."
                            CommandArgument='<%#Eval("id").ToString + ";" + Eval("ccid").ToString %>' runat="server" ImageUrl="images/edit.png">
                            <%--CommandArgument='<%#Eval("ID")%>' runat="server" ImageUrl="images/edit.png">--%>
                        </asp:ImageButton>
                        <asp:ImageButton ToolTip="Delete a record." CssClass="btn2" OnClientClick="Confirm()"
                            ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("id").ToString %>' runat="server"
                            ImageUrl="images/delete.png"></asp:ImageButton>
                    </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        <%--</table>--%>

        <br />
    </div>
    <div class="table-responsive" style="margin:0 20px; width:100%;">
        <table class="table">
            <tr>
                <td>
                    <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                    <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="15">
                        <Fields>
                            <asp:NumericPagerField ButtonType="Link" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </div>
        <div style="font-family: Calibri;margin: 0 20px;">
    <asp:Label ID="lblError" runat="server" Text='' ForeColor = "Red" Font-Size ="Medium" ></asp:Label>
            <asp:Label ID="lblnoitem" runat="server" Text="0"  Visible = "false"></asp:Label>
    
    </div>
         </div>
   
      <script>
       
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "50%";
                
               }
      
       
    </script>
</asp:Content>
