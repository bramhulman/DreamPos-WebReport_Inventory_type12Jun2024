<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
  CodeBehind="Pages_StkGroupEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_StkGroupEdit" %>
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
            var loading = $(".loading");
            if (loading.length) loading.show();
        }
        $(document).ready(function () {
            $('.btn-action-save').click(function () {
                if (typeof Page_IsValid === 'undefined' || Page_IsValid) {
                    ShowProgress();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>

    <!-- Loading Spinner -->
    <div id="spinner" class="loading">
        <div style="font-size: 3rem; color: #007bff; margin-bottom: 15px;">
            <i class="fas fa-circle-notch fa-spin"></i>
        </div>
        <div style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 1.25rem; color: #333; font-weight: 600; margin-bottom: 5px;">
            Processing...
        </div>
        <div style="font-family: 'Segoe UI', sans-serif; font-size: 0.9rem; color: #6c757d;">
            Please wait a moment.
        </div>
    </div>

    <!-- Content Header -->
    <div class="content-header py-2">
        <div class="container-fluid">
            <div class="row align-items-center mb-1">
                <div class="col-sm-6">
                    <h4 class="m-0 text-dark font-weight-bold">
                        <i class="fas fa-layer-group text-primary mr-2"></i>Stock Group Edit
                    </h4>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right mb-0 py-0" style="background:transparent;">
                        <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                        <li class="breadcrumb-item"><a href="Pages_StkGroup.aspx">Stock Group</a></li>
                        <li class="breadcrumb-item active">Stock Group Edit</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid mb-4">
        <div class="row justify-content-center">
            <div class="col-lg-8 col-md-10">
                <div class="card card-form" id="formload">
                    <div class="card-header">
                        <i class="fas fa-edit mr-2"></i>Stock Group Information
                    </div>
                    <div class="card-body p-4">
                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="lblcode" Text="Code:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNo" CssClass="form-control" placeholder="Enter group code..." Enabled="True" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNo"
                                    ErrorMessage="Code must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row align-items-center">
                            <asp:Label runat="server" CssClass="col-sm-3 col-form-label text-sm-right form-label-custom" ID="Label2" Text="Name:"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtName" CssClass="form-control" placeholder="Enter group name..." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="Name must be filled." ForeColor="Red" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Action Buttons -->
                        <div class="form-group row mt-4 pt-3 border-top">
                            <div class="col-sm-8 offset-sm-3 d-flex flex-wrap gap-2">
                                <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Save"
                                    CssClass="btn btn-primary font-weight-bold px-4 mr-2 btn-action-save" />
                                <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update"
                                    CssClass="btn btn-info font-weight-bold px-4 mr-2 btn-action-save" Visible="false" />
                                <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel"
                                    CssClass="btn btn-secondary font-weight-bold px-4" CausesValidation="false" />
                            </div>
                        </div>

                        <div runat="server" id="prevemail"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
