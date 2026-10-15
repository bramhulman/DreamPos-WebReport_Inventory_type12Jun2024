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
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Recipe Detail Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Recipe Detail Edit</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;">

    <!-- /.content-header -->


    <div class="row">
        <div class="form-horizontal inventory-entry-form" id="formload">
                   <div class="card-body">
<div id="recipeDetailError" runat="server" visible="false" class="alert alert-danger" role="alert"></div>

                   <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="Label1" Text="Type: " AssociatedControlID="ddlTipe"></asp:label>
           <div class="col-sm-9">
                     <asp:DropDownList ID="ddlTipe" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">--Select Type--</asp:ListItem>
                    <asp:ListItem Value="1">  Inventory  </asp:ListItem>
                    <asp:ListItem Value="2">  Receipe  </asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipe"
                    ErrorMessage="Please select a type." ForeColor="Red" InitialValue="0" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>
                  </div>


        </div>
                       <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="Label7" Text="Item: " AssociatedControlID="ddlItem"></asp:label>
           <div class="col-sm-9">
                     <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlItem"
                    ErrorMessage="Please select an item." ForeColor="Red" InitialValue="0" Display="Dynamic" CssClass="field-validation"></asp:RequiredFieldValidator>
                  </div>


        </div>




                        <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="Label13" Text="Qty: " AssociatedControlID="txtQty"></asp:label>
           <div class="col-sm-9">
                    <div class="input-group">
                      <asp:TextBox style="text-align:right;" runat="server" ID="txtQty" Text = "0" Class="form-control" inputmode="decimal" />
                      <div class="input-group-append">
                        <asp:Label ID="lblQty" runat="server" CssClass="input-group-text" Text="unit"></asp:Label>
                      </div>
                    </div>

                  </div>
                            </div>

                    <div class="form-group row">
                   <asp:label runat="server" CssClass="col-sm-3 col-form-label" ID="Label2" Text="Loss Factor (%): " AssociatedControlID="txtLosF"></asp:label>
           <div class="col-sm-9">

                      <asp:TextBox style="text-align:right;" runat="server" ID="txtLosF" Text = "0" Class="form-control" inputmode="decimal" />


                  </div>
                            </div>





               <div class="form-group row">
                    <div class="col-sm-9 offset-sm-3">
                            <div class="entry-actions"><button type="button" runat="server" class="btn btn-primary" onserverclick="btnInsert_Click" id="btnInsert" style="font-weight: 600;">
                                            Add
                                              </button>



                                          <button type="button" runat="server" class="btn btn-primary" onserverclick="btnUpdate_Click" id="btnUpdate" style="font-weight: 600;" visible="false">
                                            Update
                                              </button>




                                        <button type="button" runat="server" class="btn btn-outline-secondary" onserverclick="btnCancel_Click" id="btnCancel" style="font-weight: 600;" CausesValidation="false">
                                            Cancel
                                              </button></div>

                        </div>
                   </div>



    </div>

   </div>
        </div>

    </div>



</asp:Content>
