<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_InvBatchEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvBatchEdit" %>
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
        <style type="text/css">
    /*.modal
    {
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
    }*/
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
</style>
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
    <script type="text/javascript">
    function DisableButton() {
        document.getElementById("<%=btnInsert.ClientID %>").disabled = true;
    }
    window.onbeforeunload = DisableButton;
</script>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Batch Process Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Batch Process Edit</li>
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
                     
                      <asp:DropDownList ID="DdlTo" runat="server" AppendDataBoundItems="True" 
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled" CssClass="form-control">
                           <asp:ListItem Value="0">----Select----</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlTo"
                ErrorMessage="Cost Centre Must Be Choice" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label8" Text="Item: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="True" CssClass="form-control" 
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                           <asp:ListItem Value="0">----Select----</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DdlItem"
                ErrorMessage="Item Must Be Choice" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                       
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
                     
                      <asp:TextBox style="text-align:right; width:revert" runat="server" ID="txtUnit1" OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" />
                <asp:label runat="server" ID="lblUnit1" Text="Unit1" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                                                       
                  </div>  
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label9" Text="Unit 2: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert" runat="server" ID="txtUnit2" OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" />
               <asp:label runat="server" ID="lblUnit2" Text="Unit2" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                 
                                                       
                  </div>  
            
            
        </div>  
                       
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label11" Text="Factor: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:800px;padding: 0.375rem 0.75rem;" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True" ID="txtLosF" Enabled="false" Text = "1" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div>  
                     <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1" Text="Total Product: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert;padding: 0.375rem 0.75rem;" runat="server" onkeydown="return (!(event.keyCode!=32);" AutoPostBack="True" ID="txtTotalStk" Text = "1" Class="form-control" />
               <asp:label runat="server" ID="lbltotal" Text="" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                 
                                                       
                  </div>  
            
            
        </div>    
               <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Current Stock: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert;padding: 0.375rem 0.75rem;" runat="server" onkeydown="return (!(event.keyCode!=32);" AutoPostBack="True" ID="txtStock" Text = "1" Class="form-control" />
               <asp:label runat="server" ID="lblcurrent" Text="" Style="padding:6px;" CssClass="align-middle" ForeColor="Black"></asp:label>  
                                                
                  </div>  
               <asp:TextBox Visible="false" runat="server" ID="txtddate"/>    
            
        </div>             
                     
             
        

               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                            <table>
                                <tr>
                                    <td>
                                        <button type="submit" runat="server" class="btn btn-primary" id="btnInsert" onserverclick="btnInsert_Click" style="font-weight: 600; border-color:gray; width:108px;">
    Add
</button>
                                         <%--<asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />--%>
                                    </td>
                                    <td>
                                        
                                         <asp:Button runat="server" ID="btnUpdate" UseSubmitBehavior="true" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" Visible="false" />
                                    </td>
                                    <td>
                                         <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                           
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
