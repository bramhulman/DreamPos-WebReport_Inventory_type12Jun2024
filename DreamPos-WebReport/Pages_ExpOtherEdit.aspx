<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ExpOtherEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_ExpOtherEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">

     </asp:ScriptManager>

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
            <h1 class="m-0 text-dark">Misc Expense Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Misc Expense Edit</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;">

    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>

    <div class="row">
        <div class="form-horizontal inventory-entry-form" id="formload">
                   <div class="card-body">
                       <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="lblOutlet" Text="CC:" AssociatedControlID="DdlOutlet"></asp:label>
           <div class="col-sm-9">

                      <asp:DropDownList ID="DdlOutlet" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                           <%--<asp:ListItem Value="0">----Select----</asp:ListItem>--%>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlOutlet"
                ErrorMessage="Outlet Must Be Choice" InitialValue="0" style="color: #FF0000" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>

                  </div>


        </div>

                        <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="lblDate" Text="Date: " AssociatedControlID="Busidate"></asp:label>
           <div class="col-sm-9">
                <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                      <asp:TextBox style="text-align:left; width:auto;padding: 0.375rem 0.75rem;" runat="server" id="Busidate" Class="form-control" AutoPostBack="false"/>
                          <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="Busidate"
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                    </div>
                  </div>


        </div>

                        <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="lblName" Text="Name: " AssociatedControlID="txtName"></asp:label>
           <div class="col-sm-9">

                      <asp:TextBox style="text-align:left;" runat="server" ID="txtName" Class="form-control" />

                  </div>


        </div>
                       <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="lblSubtotal" Text="Subtotal: " AssociatedControlID="txtSubtotal"></asp:label>
           <div class="col-sm-9">

                      <asp:TextBox style="text-align:right;" runat="server" ID="txtSubtotal" text ="0"
                        onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" Class="form-control" />
               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSubtotal"
                    ErrorMessage="Subtotal Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>--%>

                  </div>


        </div>




               <div class="form-group row">
                    <div class="col-sm-9 offset-sm-3">
                            <div class="entry-actions"><asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600;" />



                                         <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600;" Visible="false" />


                                         <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-outline-secondary" Style="font-weight: 600;" CausesValidation="false" /></div>

                        </div>
                   </div>



    </div>

   </div>
        </div>

    </div>



</asp:Content>
