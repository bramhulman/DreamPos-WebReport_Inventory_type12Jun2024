<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="CustomerPO_Edit.aspx.vb" Inherits="DreamPos_WebReport.CustomerPO_Edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/inventory-mobile.css" />
    <style type="text/css">
        .card-form {
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            border: 1px solid #dee2e6;
        }
        .card-form .card-header {
            background-color: #1e3a5f;
            color: #ffffff;
            font-weight: 600;
            border-top-left-radius: 7px;
            border-top-right-radius: 7px;
            padding: 12px 20px;
        }
        .form-label-custom {
            font-weight: 600;
            color: #2c3e50;
            padding-top: 6px;
        }
        .validation-error {
            display: block;
            font-size: 0.8rem;
            margin-top: 3px;
        }
        #spinner.modal {
            position: fixed; top: 0; left: 0;
            width: 100%; height: 100%;
            background-color: rgba(255,255,255,0.75);
            z-index: 9999; text-align: center;
        }
        #spinner .center {
            position: absolute; top: 50%; left: 50%;
            transform: translate(-50%,-50%);
        }
        #spinner img { width: 64px; height: 64px; }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.btn-save-action').click(function () {
                if (typeof Page_IsValid === 'undefined' || Page_IsValid) {
                    $('#spinner').show();
                }
            });
        });
        function DisableButton() {
            var btn = document.getElementById("<%=btnInsert.ClientID %>");
            if (btn) btn.disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <!-- Loading Spinner -->
    <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="Loading..." src="/images/loader.gif" />
            <div class="font-weight-bold text-primary mt-2">Processing...</div>
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-2">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-user-edit text-primary mr-2"></i>Customer Edit
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_CustomerPO.aspx">Customer PO</a></li>
                        <li class="breadcrumb-item active">Customer Edit</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid mb-4">
        <div class="row justify-content-center">
            <div class="col-lg-9 col-md-11">
                <div class="card card-form" id="formload">
                    <div class="card-header">
                        <i class="fas fa-id-card mr-2"></i>Customer Details
                    </div>
                    <div class="card-body p-4">
                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="lblname" Text="Name:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtName" CssClass="form-control" MaxLength="35" placeholder="Enter customer name..." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="Name must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label2" Text="Email:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" MaxLength="30" placeholder="Enter email address..." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Email must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label3" Text="Address:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtAddress" CssClass="form-control" MaxLength="100" TextMode="MultiLine" Rows="2" placeholder="Enter street address..." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress"
                                    ErrorMessage="Address must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label4" Text="Phone:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtPhone" CssClass="form-control" MaxLength="15" placeholder="Enter phone number..." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPhone"
                                    ErrorMessage="Phone must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label9" Text="City:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtCity" CssClass="form-control" MaxLength="25" placeholder="Enter city..." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCity"
                                    ErrorMessage="City must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label10" Text="State:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtState" CssClass="form-control" MaxLength="25" placeholder="Enter state/province..." />
                            </div>
                        </div>

                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label11" Text="Postal Code:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtCpos" CssClass="form-control" MaxLength="15" placeholder="Enter postal code..." />
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label12" Text="Notes:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNotes" CssClass="form-control" TextMode="MultiLine" Rows="2" MaxLength="48" placeholder="Enter notes..." />
                            </div>
                        </div>

                        <!-- Action Buttons -->
                        <div class="form-group row mt-4 pt-3 border-top">
                            <div class="col-sm-8 offset-sm-3 d-flex flex-wrap gap-2">
                                <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Save" CssClass="btn btn-primary font-weight-bold px-4 mr-2 btn-save-action" />
                                <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-info font-weight-bold px-4 mr-2 btn-save-action" Visible="false" />
                                <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-secondary font-weight-bold px-4" CausesValidation="false" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
