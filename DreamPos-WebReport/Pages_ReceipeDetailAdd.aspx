<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ReceipeDetailAdd.aspx.vb" Inherits="DreamPos_WebReport.Pages_ReceipeDetailAdd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <%--<style type="text/css">
    thead, tbody { display: block; width:100%;}

tbody {
    height: 480px;       /* Just for the demo          */
    overflow-y: auto;    /* Trigger vertical scroll    */
    overflow-x: hidden;  /* Hide the horizontal scroll */
}
</style>--%>
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    <style>.input-group>.form-control{ width: 800px; }</style>
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
          $(document).ready(function () {
              $('.btn').click(function () {
                  if (Page_IsValid) {
                      $('#spinner').show();
                  }
              });
          });
       
    </script>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Receipt Detail Add</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Receipt Detail Add</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;height: 100%; width: 100%;">
       
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>
        <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="" src="/images/loader.gif" />
        </div>
    </div>
    <div class="row">
          

                
        <div class="form-horizontal" id="formload" style="text-align:left; padding:0 10%;">
                   <div class="card-body">
                       
 <div class="form-group row col-md-6">
                   <%--<asp:label style="margin-right:2rem;" runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Type: " AssociatedControlID="ddlTipe"></asp:label>--%>
                 <asp:label runat="server" Style="margin-right: 2rem;" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Type: " AssociatedControlID="ddlTipe"></asp:label>
     <div class="input-group col-md-6">
         <asp:DropDownList ID="ddlTipe" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="form-control" style="width:600px; padding: 0.375rem 0.75rem;"  
                    EnableViewState="True" ViewStateMode="Enabled">
                    <asp:ListItem Value="0">--Select Type--</asp:ListItem>
                    <asp:ListItem Value="1">--Inventory--</asp:ListItem>
                    <asp:ListItem Value="2">--Receipe--</asp:ListItem>
                </asp:DropDownList>

                    
         </div>
                 
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTipe"
                    ErrorMessage="Tipe Must Be Choice." ForeColor="Red"></asp:RequiredFieldValidator>            
                  </div>  
               <div class="form-group row col-md-6">
                   <asp:Label Style="margin-right: 2rem;" runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="Search: " AssociatedControlID="txtSearch"></asp:Label>
                    <div class="input-group col-md-6">
                       <asp:TextBox style="text-align:left; width:70%; padding: 0.375rem 0.75rem;" runat="server" ID="txtSearch" Class="form-control"/>
                       <span class="input-group-addon" >
                            <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" class="btn btn-mini" title="Search" style="background:#FFFFFF">
                            <i class="fas fa-search"></i>
                        </button>   
                       </span>
                       </div>  
                    
                            
                  </div>  
   </div>
        </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-md-6" style="text-align:center; padding:0 10%;">
                <div class="table-responsive table-bordered"  style="overflow-y:auto; height:300px;">
  <table class="table">
   <thead>
    <tr style="background-color: #3a4f63;">
      <th><b style="color: #FFFFFF">Check</b></th>
      <th runat="server" visible="false" id="thid" style="color: #FFFFFF">ID</th>
      <th><b style="color: #FFFFFF">Name</b></th>
    </tr>
  </thead>
     <tbody>
         
      <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server" OnPagePropertiesChanging="OnPagePropertiesChanging">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server"></asp:CheckBox>
                    </td>
                    <td runat="server" id="tdid" visible="false">
                        <asp:Label ID="lblID" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                    </td>
                    <td runat="server" style="text-align: left;">
                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("name")%>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
         </asp:ListView>

    
     </tbody>
  </table>
</div>
              </div>

         <div class="form-horizontal col-lg-6 col-md-6" style="overflow:scroll;" id="formedit">
                   <div class="card-body">
                       
 <div class="form-group row col-md-6">
                   <asp:label style="margin-right:4rem;" runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label1" Text="Qty: " AssociatedControlID="txtQty"></asp:label>
                 <div class="input-group col-md-6">
                         <asp:TextBox style="text-align:right;" runat="server" ID="txtQty" Class="form-control" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);"/>
                 
                     </div>                  
      </div>  
               <div class="form-group row col-md-6">
                   <asp:label style="margin-right:4rem;" runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label4" Text="Loss Factor (%): " AssociatedControlID="txtLosF"></asp:label>
                  <div class="input-group col-md-6">
                      <asp:TextBox style="text-align:right;" runat="server" ID="txtLosF" Class="form-control" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);"/>
                    </div>
                                                 
                  </div>  
                       <div class="form-group row">
                    <div class="btn-group" role="group" style="margin:0.5rem" >
                        <%--<asp:button onserverclick="btnInsert_Click" ID="btnInsert" runat="server" type="button" class="btn btn-primary" title="insert"style="background:#FFFFFF;color:#3a4f63;"/>--%>
                         <button runat="server" id="btnInsert" onserverclick="btnInsert_ServerClick" class="btn btn-primary" title="Insert" style="background:#FFFFFF;color:#3a4f63;">
                            Insert
                        </button>  
                        </div>
                    <div class="btn-group" role="group" style="margin:0.5rem">
                         <%--<asp:button onserverclick="btnUpdate_Click" ID="btnUpdate" runat="server" type="button" class="btn btn-primary" title="Update"style="background:#FFFFFF;color:#3a4f63;" visible="false"/>--%>
                        <button runat="server" id="btnUpdate" onserverclick="btnUpdate_Click" class="btn btn-primary" title="Update" style="background:#FFFFFF;color:#3a4f63;" visible="false">
                           Update
                        </button>     
                    </div>
                            <div class="btn-group" role="group" style="margin:0.5rem" >
                        <%--<asp:button onserverclick="btnCancel_Click" id="btnCancel" runat="server" type="button" class="btn btn-primary" title="insert"style="background:#FFFFFF;color:#3a4f63;" causesvalidation="false"/>--%>
                        <button runat="server" id="btnCancel" onserverclick="btnCancel_ServerClick" class="btn btn-primary" title="Cancel" style="background:#FFFFFF;color:#3a4f63;" causesvalidation="false">
                           Cancel
                        </button>    
                            </div>
                  </div>  
   </div>
        </div>

           </div>
     
    </div>
     
    
          <script>
       
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "94%";
                
               }
      
       
    </script>
</asp:Content>
