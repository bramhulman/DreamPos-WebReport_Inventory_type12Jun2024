<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ReceipeDtlEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_ReceipeDtlEdit" %>
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
            <h1 class="m-0 text-dark">Receipt Detail Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Receipt Detail Edit</li>
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
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1" Text="Type: "></asp:label>
           <div class="input-group col-md-3">
                     <asp:DropDownList ID="ddlTipe" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">--Select Type--</asp:ListItem>
                    <asp:ListItem Value="1">  Inventory  </asp:ListItem>
                    <asp:ListItem Value="2">  Receipe  </asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipe"
                    ErrorMessage="Please Choice type" ForeColor="Red"></asp:RequiredFieldValidator>         
                  </div>  
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label7" Text="Item: "></asp:label>
           <div class="input-group col-md-3">
                     <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlItem"
                    ErrorMessage="Please Choice item" ForeColor="Red"></asp:RequiredFieldValidator>         
                  </div>  
            
            
        </div>   
             
                      
                       
                       
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label13" Text="Qty: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right; width:revert;" runat="server" ID="txtQty" Text = "0" Class="form-control" 
                          onkeydown = "return (!(event.keyCode!=32);" />
             <asp:Label ID="lblQty" runat ="server" Style="padding:6px;" CssClass="align-middle" Text="unit" ForeColor="Black"></asp:Label>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtQty"
                    ErrorMessage="Qty Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>   --%>                             
                  </div>  
                            </div>
                         
                    <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Loss Factor (%): "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:right;" runat="server" ID="txtLosF" Text = "0" Class="form-control" onkeydown = "return (!(event.keyCode!=32);" />
               
                                                       
                  </div>  
                            </div>
                        
                     
             
        

               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                            <table>
                                <tr>
                                    <td>
                                          <button type="button" runat="server" class="btn btn-primary" onserverclick="btnInsert_Click" data-toggle="modal" id="btnInsert" style="font-weight: 600; border-color:gray; width:108px;">
                                            Add
                                              </button>
                                         <%--<asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />--%>
                                    </td>
                                    <td>
                                          <button type="button" runat="server" class="btn btn-primary" onserverclick="btnUpdate_Click" data-toggle="modal" id="btnUpdate" style="font-weight: 600; border-color:gray; width:108px;" visible="false">
                                            Update
                                              </button>
                                         <%--<asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" Visible="false" />--%>
                                    </td>
                             <%--         <td>
                                         <asp:Button runat="server" ID="btnlimit" OnClick="btnlimit_Click" Text="set Limit" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                                    </td>--%>
                                    <td>
                                        <button type="button" runat="server" class="btn btn-primary" onserverclick="btnCancel_Click" data-toggle="modal" id="btnCancel" style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false">
                                            Cancel
                                              </button>
                                         <%--<asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" />--%>
                                    </td>
                                </tr>
                            </table>
                           
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
