<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_limitStkEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_limitStkEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    <style>.input-group>.form-control{ width: 800px }</style>
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
            <h1 class="m-0 text-dark">Limit Stock Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Limit Stock Edit</li>
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
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="lblitemno" Text="Number: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox style="padding: 0.375rem 0.75rem;width:800px;" runat="server" ID="txtNo" Class="form-control" Enabled="False" MaxLength="15"/>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNo"
                    ErrorMessage="No Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>             
                  </div>  
            
            
        </div>
                   <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label1" Text="Name: "></asp:label>
           <div class="input-group col-md-6">
                     <asp:DropDownList ID="ddlstok" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlstok"
                    ErrorMessage="Please Choice stock" ForeColor="Red"></asp:RequiredFieldValidator>         
                  </div>  
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label7" Text="CC: "></asp:label>
           <div class="input-group col-md-6">
                     <asp:DropDownList ID="ddlcc" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlcc"
                    ErrorMessage="Please Choice Cc" ForeColor="Red"></asp:RequiredFieldValidator>         
                  </div>  
            
            
        </div>   
                       <%--<div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="Stock Group: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:DropDownList ID="DdlGrpStk" runat="server" AppendDataBoundItems="True" 
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DdlGrpStk"
                ErrorMessage="Choose Stock Group" InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4" Text="Unit 1: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtUnit1" MaxLength="5" Text = "pcs" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div>  
                       <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label9" Text="Unit 2: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtUnit2" MaxLength="5" Text = "pcs" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label10" Text="Unit 3: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtUnit3" MaxLength="5" Text = "pcs" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div> --%> 
                       
                        <%--<div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label12" Text="Factor 2: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" OnTextChanged="txtFac2_TextChanged" AutoPostBack="True" ID="txtFac2" Text = "1" Class="form-control" />
               
                                                       
                  </div>  
            
            
        </div>  --%>
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label13" Text="Stock Limit: "></asp:label>
           <div class="input-group col-md-6">
                     
                      <asp:TextBox style="text-align:right;" runat="server" ID="txtStkLimit" Text = "0" Class="form-control" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" />
             
                                                       
                  </div>  
                            </div>
                         
                       <%--<div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label5" Text="Batch: "></asp:label>
           <div class="form-check input-group col-md-3">
                     
                      <asp:CheckBox ID="chkbatch" runat="server" AutoPostBack="True" OnCheckedChanged="chkbatch_CheckedChanged" cssclass="form-check-input" type="checkbox"  />
                                              <asp:label runat="server" CssClass="form-check-label" for="chklink" ID="Label1" Text="Yes" ></asp:label>          
                  </div>  
            
            
        </div>  
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label6" Text="Recipe Link: "></asp:label>
           <div class="input-group col-md-3">
                     
                     <asp:DropDownList ID="DdlLink" Enabled ="false" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled">
                   <asp:ListItem Value="0">--Select Receipe--</asp:ListItem>
                 
                </asp:DropDownList>                                
                  </div>  
            
            
        </div>  --%>
                        
                     
             
        

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
                             <%--         <td>
                                         <asp:Button runat="server" ID="btnlimit" OnClick="btnlimit_Click" Text="set Limit" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                                    </td>--%>
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
