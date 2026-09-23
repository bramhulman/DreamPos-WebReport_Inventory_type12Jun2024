<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_Invclswaste.aspx.vb" Inherits="DreamPos_WebReport.Pages_Invclswaste" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    <style>.input-group>.form-control{ width: 800px; }</style>
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <%--<script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>--%>
    <script src="js/jquery.ui.core.js" type="text/javascript"></script>
    <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="js/jquery.ui.button.js" type="text/javascript"></script>
    <script src="js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script> 
    <script src="js/jquery.ui.combobox.js" type="text/javascript"></script> 
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
            <h1 class="m-0 text-dark">Closing Waste</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Closing Waste</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;">
       
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>

    <div class="row">
        <div class="form-horizontal" id="formload">
                   <div class="card-body">
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label7" Text="CC: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:DropDownList ID="DdlTo" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                           <asp:ListItem Value="0">----Select----</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlTo"
                ErrorMessage="Cost Centre Must Be Choice" InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label8" Text="Stock Name: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                           <asp:ListItem Value="0">----Select----</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DdlItem"
                ErrorMessage="Item Must Be Choice" InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                       
                  </div>  
            
            
        </div> 

                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label6" Text="for: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:DropDownList ID="ddlclosing" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                           <asp:ListItem Value="0">Closing yesterday</asp:ListItem>
                           <asp:ListItem Value="1">Closing today</asp:ListItem>
                    </asp:DropDownList>
                                        
                  </div>  
            
            
        </div> 

                        <div class="form-group row col-md-12" style="display:none;">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblStkno" Text="Stkno: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtstkNo" Class="form-control"/>
                                                    
                  </div>  
            
            
        </div>
                    
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4" Text="Unit 1: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert;" runat="server" ID="txtUnit1" OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" />
                <asp:label runat="server" ID="lblUnit1" Text="Unit1" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                                                       
                  </div>  
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label9" Text="Unit 2: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert;" runat="server" ID="txtUnit2" OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" />
               <asp:label runat="server" ID="lblUnit2" Text="Unit2" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                 
                                                       
                  </div>  
            
            
        </div>  
                       
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label11" Text="Factor: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width : 800px; padding: 0.375rem 0.75rem" runat="server" onkeydown="return (!(event.keyCode!=32);" AutoPostBack="True" ID="txtLosF" Enabled="false" Text = "1" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div>  
                     <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1" Text="Total Waste: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width : 800px;padding: 0.375rem 0.75rem" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True" ID="txtTotalStk" Enabled="false" Text = "1" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div> 
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="Cost: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert;" runat="server" ID="txtcost" OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" readonly="true" />
               <asp:label runat="server" ID="lblcost" Text="Unit" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                 
                                                       
                  </div>  
            
            
        </div>    
                       
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label5" Text="Total Cost: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:800px;padding: 0.375rem 0.75rem;" runat="server" AutoPostBack="True" ID="txtTotalCost" Enabled="false" Text = "0" Class="form-control" />
               <%-- <asp:label runat="server" ID="lblTotalCost" Text="Unit" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>--%> 
                                                       
                  </div>  
            
            
        </div> 
                        
               <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Current Stock: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right;width:revert;padding: 0.375rem 0.75rem;" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True" ID="txtStock" readonly="true" Text = "1" Class="form-control" />
                <asp:label runat="server" ID="lblStock" Text="Unit" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label> 
                                                       
                  </div>  
            
            
        </div>             
                     
             
        

               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                            
                                       <button type="button" runat="server" class="btn btn-primary" id="btnInsert" onserverclick="btnInsert_Click" style="font-weight: 600; border-color:gray; width:108px;">
                            Add
                        </button>
                                         <%--<asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />--%>
                                   
                                         <button type="button" runat="server" class="btn btn-primary" id="btnUpdate" onserverclick="btnUpdate_ServerClick" style="font-weight: 600; border-color:gray; width:108px;" Visible="false" >
                                            Update
                                        </button>
                                         <%--<asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" Visible="false" />--%>
                                    <button type="button" runat="server" class="btn btn-primary" id="btnCancel" onserverclick="btnCancel_ServerClick" style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" >
                                            Cancel
                                        </button>
                                         <%--<asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" />--%>
                                    
                           
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
                document.getElementById("formload").style.width = "80%";
                
               }
      
       
    </script>
</asp:Content>
