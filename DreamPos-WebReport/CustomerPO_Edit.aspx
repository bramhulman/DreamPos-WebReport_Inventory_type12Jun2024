<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="CustomerPO_Edit.aspx.vb" Inherits="DreamPos_WebReport.CustomerPO_Edit" %>
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
            <h1 class="m-0 text-dark">Customer Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Customer Edit</li>
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
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="lblname" Text="Name: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox runat="server" ID="txtName" Class="form-control" MaxLength="35"/>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtname"
                    ErrorMessage="Name Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>             
                  </div>  
            
            
        </div>
                    <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label2" Text="Email: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox runat="server" ID="txtEmail" Class="form-control" MaxLength="30"/>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>
                                                       
                  </div>  
            
            
        </div>    
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label3" Text="Address: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox runat="server" ID="txtAddress" Class="form-control" MaxLength="100" TextMode="MultiLine"/>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress"
                    ErrorMessage="Address Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label4" Text="Phone: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox runat="server" ID="txtPhone" Class="form-control" MaxLength="15" />
               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPhone"
                    ErrorMessage="Phone Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>
                                                       
                  </div>   
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label9" Text="City: "></asp:label>
           <div class="input-group col-md-6">
                     
                     <asp:TextBox runat="server" ID="txtCity" Class="form-control" MaxLength="25" />
               <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCity"
                    ErrorMessage="City Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>
                                              
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label10" Text="State: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox runat="server" ID="txtState" Class="form-control" MaxLength="25" />
                
                                                       
                  </div>  
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label11" Text="Postal Code: "></asp:label>
           <div class="input-group col-md-6">
                     
                       
                      <asp:TextBox runat="server" ID="txtCpos" Class="form-control" MaxLength="15" />
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label12" Text="Notes: "></asp:label>
           <div class="input-group col-md-6">
                     
                    <asp:TextBox runat="server" ID="txtNotes" Class="form-control" TextMode="MultiLine" MaxLength="48" />
                       
                                                       
                  </div>  
            
            
        </div>  
                    
                        
                        
                     
             
        

               <div class="form-group row">
                   <div Class="col-md-2"></div>
                    <div class="col-md-offset-2 col-md-10">
                            <table>
                                <tr>
                                    <td>
                                        
                                         <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                                    </td>
                                    <td>
                                        
                                         <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" Visible="false" />
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
     
    </div>
     
    
          <script>
       
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "80%";
                
               }
      
       
    </script>
</asp:Content>
