<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_Calculate.aspx.vb" Inherits="DreamPos_WebReport.Pages_Calculate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
     <style>.input-group>.form-control{ width: 800px; }</style>
    <style type="text/css">
           .loading
        {
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
    
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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

  </script>
 
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Calculate Inventory</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
              <li class="breadcrumb-item active">Calculate Inventory</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>
    <div class="row">
      
        <%--<div class="form-inline" id="formload" runat="server" style="margin:0 20px; width:100%;background: #007bff;padding:0.8rem 0.8rem">--%>
             <%--<div class="form-group mx-sm-3 mt-mb-1">
                    <asp:label for="txtDate" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">Date:</b></asp:label>
                    <asp:TextBox style="text-align:center;" runat="server" id="txtDate" Class="form-control" AutoPostBack="false" />
                          <ajaxToolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate" 
                            Format="dd MMM yyyy"></ajaxToolkit:CalendarExtender>  
             
                </div>--%>
                   <%--<div class="form-group mx-sm-3 mt-mb-1">
                    <asp:label for="txtDate" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">To:</b></asp:label>
                    <asp:TextBox style="text-align:center;" runat="server" id="txtTo" Class="form-control" AutoPostBack="false" />
                          <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo" 
                            Format="dd MMM yyyy"></ajaxToolkit:CalendarExtender>  
             
                </div>  --%>
            
             
<%--                <div class="form-group mx-sm-3 mt-mb-1">
                       <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
                          <asp:Label runat="server">Calculate</asp:Label>
                        </button>
                    
                  
                      </div>--%>
                 
              <%--  </div>--%>
             
              
              <div class="form-horizontal" id="formload" style="width:100%;">
                 
                   <div class="card-body">
                        <div class="form-group row col-md-12">
                    <asp:label for="ddlCC" runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4" Text="Cost Centre: "></asp:label>
                 <div class="input-group col-md-3">
                     <asp:DropDownList ID="ddlCC" runat="server" CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="true">
                     </asp:DropDownList>
                  </div>
               </div>

               <div class="form-group row col-md-12">
                    <asp:label for="ddlStock" runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label5" Text="Stock Item: "></asp:label>
                 <div class="input-group col-md-3">
                     <asp:DropDownList ID="ddlStock" runat="server" CssClass="form-control" AutoPostBack="false" AppendDataBoundItems="true">
                     </asp:DropDownList>
                  </div>
               </div>
               
               <div class="form-group row col-md-12">
           
                   <asp:label for="txtDate" runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="From: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     
                      <asp:TextBox style="text-align:left; width:auto;" runat="server" ID="txtDate" Class="form-control" AutoPostBack="false" />
                          <ajaxToolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate" 
                            Format="dd MMM yyyy"></ajaxToolkit:CalendarExtender>                               
                  </div>             
                  </div>  
            
            
        </div>         
<%--     <div class="form-group row col-md-12">
        
                   <asp:label for="txtto" runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1" Text="To: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     
                      <asp:TextBox style="text-align:left; width:auto;" runat="server" ID="txtTo" Class="form-control" AutoPostBack="false" />
                          <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTo" 
                            Format="dd MMM yyyy"></ajaxToolkit:CalendarExtender>                               
                  </div>             
                  </div>  
            
            
        </div>     --%> 
               <div class="form-group row col-md-12">
                    <asp:label runat="server" for="btnSearch" CssClass="col-sm-1 col-form-label text-nowrap"  ID="Label2"></asp:label>
                 <div class="col-md-10">
                    <button runat="server" id="btncalcusage" type="submit" onserverclick="btncalcusage_Click" class="btn btn-primary" title="Calculate Usage" style="font-weight: 600; border-color:gray; width:120px; margin-right: 15px;">
                        <asp:Label runat="server">Calc Usage</asp:Label>
                    </button>
                    
                    <div runat="server" id="recalbtn" style="display:inline-block;">
                        <button runat="server" id="btnrecal" Visible="true" onserverclick="btnrecal_ServerClick" class="btn btn-primary" title="Re Calculate" style="font-weight: 600; border-color:gray; width:130px; margin-right: 5px;">
                            <asp:Label runat="server">Calc Full Stock</asp:Label>
                        </button>
                        <button runat="server" id="btncalccost" Visible="true" onserverclick="btncalccost_ServerClick" class="btn btn-success" title="Calculate Moving Average Cost" style="font-weight: 600; border-color:gray; width:130px; margin-right: 5px;">
                            <asp:Label runat="server">Calc Avg Cost</asp:Label>
                        </button>
                        <button runat="server" id="btnSyncCost" Visible="true" onserverclick="btnSyncCost_ServerClick" class="btn btn-warning" title="Sync Cost from WH" style="font-weight: 600; border-color:gray; width:130px; margin-right: 5px;">
                            <asp:Label runat="server">Sync Cost</asp:Label>
                        </button>
                        <a href="Inventoryhome.aspx" class="btn btn-outline-secondary" style="font-weight: 600; border-color:gray; width:100px;" title="Back to Home">
                            <i class="fas fa-home mr-1"></i> Home
                        </a>
                    </div>
                 </div>
                        
                       
                   </div> 
                       </div>
                  </div>
                      
             
             
               <div class="loading" style="text-align:center">
    Loading... Please wait...<br />
    <br />
    <img src="/images/loader.gif" alt="" />
</div>

    </div>

  


      <script>
       
          if ($(window).width() >= 960) {
              document.getElementById("formload").style.width = "100%";
          } else {
              document.getElementById("formload").style.width = "80%";

          }
      
       
    </script>
</asp:Content>
