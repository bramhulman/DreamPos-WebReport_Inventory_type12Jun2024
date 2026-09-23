<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
  CodeBehind="Pages_StockOpnameEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_StockOpnameEdit" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    <%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
      <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
          .input-group>.form-control {
            width: 800px;
          }

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
          <script type="text/javascript">
            function DisableButton() {
              document.getElementById("<%=btnInsert.ClientID %>").disabled = true;
            }
            window.onbeforeunload = DisableButton;
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
                <h1 class="m-0 text-dark">
                  <asp:label runat="server" ID="lblh1">Stock Opname Edit</asp:label>
                </h1>
              </div><!-- /.col -->
              <div class="col-sm-6">
                <ol class="breadcrumb float-sm-right">
                  <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                  <li class="breadcrumb-item active">
                    <asp:label runat="server" ID="lblli1">Stock Opname Edit</asp:label>
                  </li>
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
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label7"
                      Text="Cost Centre: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:DropDownList ID="DdlTo" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                        <asp:ListItem Value="0">----Select----</asp:ListItem>
                      </asp:DropDownList>
                      <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="DdlTo" ErrorMessage="Cost Centre Must Be Choice" InitialValue="0"
                        style="color: #FF0000"></asp:RequiredFieldValidator>--%>

                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label8" Text="Item: ">
                    </asp:label>
                    <div class="input-group col-md-3">

                      <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                        AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                        <asp:ListItem Value="0">----Select----</asp:ListItem>
                      </asp:DropDownList>
                      <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="DdlItem" ErrorMessage="Item Must Be Choice" InitialValue="0"
                        style="color: #FF0000"></asp:RequiredFieldValidator>--%>

                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="for: ">
                    </asp:label>
                    <div class="input-group col-md-3">

                      <asp:DropDownList ID="ddladjustnow" runat="server" AppendDataBoundItems="True"
                        CssClass="form-control" AutoPostBack="True" EnableViewState="True" ViewStateMode="Enabled">
                        <%--<asp:ListItem Value="1">Current Stock</asp:ListItem>--%>
                          <asp:ListItem Value="2">Opening Stock</asp:ListItem>
                      </asp:DropDownList>

                    </div>


                  </div>
                  <div class="form-group row col-md-12" style="display:none;">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblStkno"
                      Text="Stkno: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:center;" runat="server" ID="txtstkNo" Class="form-control" />

                    </div>


                  </div>

                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblinstock"
                      Text="In Stock: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right;width:revert;" ReadOnly="true" runat="server"
                        AutoPostBack="True" ID="txtinstock" Class="form-control" />

                      <asp:label runat="server" ID="lblunit22" Text="" Style="padding:6px;" CssClass="align-middle"
                        ForeColor="Black"></asp:label>



                    </div>


                  </div>

                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4"
                      Text="Unit 1: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right; width:revert;" runat="server" ID="txtUnit1"
                        OnTextChanged="txtUnit1_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" />
                      <asp:label runat="server" ID="lblUnit1" Text="Unit1" Style="padding:6px;" CssClass="align-middle"
                        ForeColor="Black"></asp:label>

                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label9"
                      Text="Unit 2: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right; width:revert;" runat="server" ID="txtUnit2"
                        OnTextChanged="txtUnit2_TextChanged" AutoPostBack="True"
                        onkeydown="return (!(event.keyCode!=32);" Class="form-control" />
                      <asp:label runat="server" ID="lblUnit2" Text="Unit2" Style="padding:6px;" CssClass="align-middle"
                        ForeColor="Black"></asp:label>


                    </div>


                  </div>

                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label11"
                      Text="Factor: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right; width:800px;padding: 0.375rem 0.75rem;" runat="server"
                        onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True" ID="txtLosF"
                        Enabled="false" Text="1" Class="form-control" />


                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1"
                      Text="Total Actual: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right; width:800px;padding: 0.375rem 0.75rem;" runat="server"
                        onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True"
                        ID="txtTotalStk" Enabled="false" Text="1" Class="form-control" />


                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label5"
                      Text="Cost U1: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right; width:revert;" runat="server"
                        onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True"
                        ID="txtCostu1" Text="1" Class="form-control" />
                      <asp:label runat="server" ID="lblcostu1" Text="Unit1" Style="padding:6px;" CssClass="align-middle"
                        ForeColor="Black"></asp:label>

                    </div>


                  </div>
                  <div class="form-group row col-md-12">
                    <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2"
                      Text="Cost U2: "></asp:label>
                    <div class="input-group col-md-3">

                      <asp:TextBox style="text-align:right; width:revert;" runat="server"
                        onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" AutoPostBack="True" ID="txtCost"
                        Text="1" Class="form-control" />
                      <asp:label runat="server" ID="lblcostu2" Text="Unit2" Style="padding:6px;" CssClass="align-middle"
                        ForeColor="Black"></asp:label>

                    </div>


                  </div>




                  <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                      <table>
                        <tr>
                          <td>
                            <%--<asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add"
                              CssClass="btn btn-primary"
                              Style="font-weight: 600; border-color:gray; width:108px;" />--%>
                            <button runat="server" id="btnInsert" onserverclick="btnInsert_Click"
                              class="btn btn-primary"
                              style="font-weight: 600; border-color:gray; width:108px;">Add</button>
                          </td>
                          <td>
                            <%--<asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update"
                              CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;"
                              Visible="false" />--%>
                            <button runat="server" id="btnUpdate" onserverclick="btnUpdate_Click"
                              class="btn btn-primary" style="font-weight: 600; border-color:gray; width:108px;"
                              Visible="false">Update</button>
                          </td>
                          <td>
                            <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel"
                              CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;"
                              CausesValidation="false" />
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