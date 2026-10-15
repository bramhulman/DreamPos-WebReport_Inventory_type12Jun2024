<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_OwnEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_OwnEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">

     </asp:ScriptManager>
   <style></style>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Own Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Own Edit</li>
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
                   <asp:label AssociatedControlID="txtName" runat="server" CssClass="col-sm-3 col-form-label" ID="lblitemno" Text="Name: "></asp:label>
           <div class="col-sm-9">

                      <asp:TextBox style="padding: 0.375rem 0.75rem;" runat="server" ID="txtName" Class="form-control" MaxLength="15"/>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtName"
                    ErrorMessage="Cost Centre Name Must Be Fill." ForeColor="Red" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>
                  </div>


        </div>

                      <%-- <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="Label3" Text="Outlet: "></asp:label>


        </div>  --%>
                        <div class="form-group row">
                   <asp:label AssociatedControlID="txtMarkup" runat="server" CssClass="col-sm-3 col-form-label" ID="lblmarkup" Text="Mark Up: "></asp:label>
           <div class="col-sm-9">
                <asp:TextBox style="padding: 0.375rem 0.75rem;" ID="txtMarkup" runat="server" Class="form-control" ></asp:TextBox>

                  </div>


        </div>









               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                            <div class="entry-actions">

                                         <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600;" />



                                         <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600;" Visible="false" />


                                         <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-outline-secondary" Style="font-weight: 600;" CausesValidation="false" />
                                    </div>

                        </div>
                   </div>



    </div>

   </div>
        </div>

    </div>



</asp:Content>
