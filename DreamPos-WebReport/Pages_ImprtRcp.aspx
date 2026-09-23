<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteinventoryHome.Master" CodeBehind="Pages_ImprtRcp.aspx.vb" Inherits="DreamPos_WebReport.Pages_ImprtRcp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
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
              }, 2);
          }
          $('form').live("submit", function () {
              ShowProgress();
          });
          //$(document).ready(function () {
          //    $('.btn').click(function () {
          //        if (Page_IsValid) {
          //            $('#spinner').show();
          //        }
          //    });
          //});
       
    </script>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Export/Import Receipe</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Export/Import Receipe</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;">
       
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>
        <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="" src="/images/loader.gif" />
        </div>
    </div>
    <div class="row">
        <div class="form-horizontal" id="formload">
                   <div class="card-body">
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblfile" Text="File: "></asp:label>
           <div class="input-group col-md-3">
                     <asp:FileUpload ID="FileUpLoad1" AlternateText="You cannot upload files" 
                    runat="server" Class="form-control"
                    style="font-weight: 700; color: #000000; background-color: #FFFFFF; Width:max-content;" />
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Fileupload1"
                    ErrorMessage="Please Select excel files." ForeColor="Red"></asp:RequiredFieldValidator>
                                
                  </div>  
            
            
        </div>
                <%--    <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="File Name: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtEx" Class="form-control"/>
              
                                                       
                  </div>  
            
            
        </div>    --%>
                  <%--  <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1" Text="Date: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <asp:TextBox style="text-align:center;" runat="server" id="txtDate" Class="form-control" AutoPostBack="false" />
                          <ajaxtoolkit:CalendarExtender ID="cleFrom" runat="server" TargetControlID="txtDate" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>  
              
                                                       
                  </div>  
            
            
        </div>      --%>
                   <%--    
                      <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4" Text="Import for: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:DropDownList ID="ddladjustnow" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="false" EnableViewState="True" ViewStateMode="Enabled">
                           <asp:ListItem Value="1">Current Stock</asp:ListItem>
                           <asp:ListItem Value="2">Opening Stock</asp:ListItem>
                    </asp:DropDownList>
                                        
                  </div>  --%>
            
            
        </div>  
             <%--           
              <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="Cc Name: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <asp:DropDownList ID="ddlCCName" runat="server" AppendDataBoundItems="True" 
                        AutoPostBack="false" EnableViewState="True" ViewStateMode="Enabled">
                           <asp:ListItem Value="0">----Select----</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCCName"
                ErrorMessage="Cost Centre Must Be Choice" InitialValue="0" style="color: #FF0000"></asp:RequiredFieldValidator>                        
                  </div>  
            
            
        </div>--%>     
        

               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                            <table>
                                <tr>
                                    <td>
                                        
                                         <asp:Button runat="server" ID="btnImport" OnClick="btnImport_Click" Text="Import" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                                    </td>
                                   <%-- <td>
                                        
                                         <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Sample" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false"/>
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
     <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
    </div>
     
    
          <script>
       
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "80%";
                
               }
      
       
    </script>
</asp:Content>
