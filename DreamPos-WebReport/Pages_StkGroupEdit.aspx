<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
  CodeBehind="Pages_StkGroupEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_StkGroupEdit" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
      <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <style type="text/css">
          .modal {
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
          }

          .loading {
            position: fixed;
            top: 50% !important;
            left: 50% !important;
            transform: translate(-50%, -50%);
            z-index: 9999;
            background: #ffffff;
            padding: 30px 40px;
            border-radius: 12px;
            box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);
            text-align: center;
            min-width: 300px;
            display: none;
            border: none;
          }
        </style>
        <script type="text/javascript">
          function ShowProgress() {
            setTimeout(function () {
              var modal = $('<div />');
              modal.addClass("modal");
              $('body').append(modal);
              var loading = $(".loading");
              loading.show();
            }, 200);
          }
          $('form').live("submit", function () {
            ShowProgress();
          });
          $(document).ready(function () {
            $('.btn').click(function () {
              if (Page_IsValid) {
                ShowProgress();
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

        <div id="spinner" class="loading">
          <div style="font-size: 3rem; color: #007bff; margin-bottom: 15px;">
            <i class="fas fa-circle-notch fa-spin"></i>
          </div>
          <div
            style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 1.25rem; color: #333; font-weight: 600; margin-bottom: 5px;">
            Processing...
          </div>
          <div style="font-family: 'Segoe UI', sans-serif; font-size: 0.9rem; color: #6c757d;">
            Please wait a moment.
          </div>
        </div>

        <!-- Content Header (Page header) -->
        <div class="content-header">
          <div class="container-fluid">
            <div class="row mb-2">
              <div class="col-sm-6">
                <h1 class="m-0 text-dark">Stock Group Edit</h1>
              </div><!-- /.col -->
              <div class="col-sm-6">
                <ol class="breadcrumb float-sm-right">
                  <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                  <li class="breadcrumb-item active">Stock Group Edit</li>
                </ol>
              </div><!-- /.col -->
            </div><!-- /.row -->
          </div><!-- /.container-fluid -->
        </div>
        <div class="col-md-12" style="background-color:white;">

          <!-- /.content-header -->
          <%-- <h2>User Access</h2>--%>

            <div class="row">
              <div class="form-horizontal" id="formload">
                <div class="card-body">
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblcode" Text="Code: ">
                    </asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="width:800px;" runat="server" ID="txtNo" Class="form-control" Enabled="True" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNo"
                        ErrorMessage="Code Must Be Fill." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Name: ">
                    </asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="width:800px;" runat="server" ID="txtName" Class="form-control" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtName" ErrorMessage="Name Must Be Fill." ForeColor="Red">
                      </asp:RequiredFieldValidator>

                    </div>


                  </div>




                  <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                      <table>
                        <tr>
                          <td>

                            <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add"
                              CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                          </td>
                          <td>

                            <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update"
                              CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;"
                              Visible="false" />
                          </td>
                          <td>
                            <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel"
                              CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;"
                              CausesValidation="false" />
                          </td>
                          <%--<td>

                            <asp:Button runat="server" ID="btntestemail" OnClick="btntestemail_Click" Text="testemail"
                              CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                            </td>
                            <td>

                              <asp:Button runat="server" ID="btnprvemail" OnClick="btnprvemail_Click" Text="prvemail"
                                CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                            </td>--%>
                        </tr>
                      </table>
                      <div runat="server" id="prevemail"></div>

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