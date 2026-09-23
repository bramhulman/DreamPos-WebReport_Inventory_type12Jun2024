<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="User_Edit.aspx.vb" Inherits="DreamPos_WebReport.User_Edit" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
        <%--<script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>--%>
            <script src="js/jquery.ui.core.js" type="text/javascript"></script>
            <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
            <script src="js/jquery.ui.button.js" type="text/javascript"></script>
            <script src="js/jquery.ui.position.js" type="text/javascript"></script>
            <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script>
            <script src="js/jquery.ui.combobox.js" type="text/javascript"></script>
            <script type="text/javascript"
                src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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
            <script type="text/javascript">
                function Confirm() {
                    var confirm_value = document.createElement("INPUT");
                    confirm_value.type = "hidden";
                    confirm_value.name = "confirm_value";
                    if (confirm("Do you want to Delete data?")) {
                        confirm_value.value = "Yes";
                    } else {
                        confirm_value.value = "No";
                    }
                    document.forms[0].appendChild(confirm_value);

                }
                function Confirm2() {
                    var confirm_value = document.createElement("INPUT");
                    confirm_value.type = "hidden";
                    confirm_value.name = "confirm2_value";
                    if (confirm("Do you want to Update data?")) {
                        confirm_value.value = "Yes";
                    } else {
                        confirm_value.value = "No";
                    }
                    document.forms[0].appendChild(confirm_value);

                }
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
        <style>
            .form-control {
                width: 40%;
            }

            /* just to help visualise the layout: */

            label {
                padding: 0.5em;
            }


            /* to ensure visibility of the :focus indicator should the client
               be using keyboard navigation: */

            :focus-visible {
                outline-width: 2px;
                outline-style: solid;
                outline-color: #f90;
                outline-offset: 0.1em;
            }
        </style>
        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0 text-dark">User Edit</h1>
                    </div>
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                    <!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">User Edit</li>
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

                            <div class="form-group row">
                                <asp:Label runat="server" AssociatedControlID="txtusr"
                                    CssClass="col-sm-2 control-label text-xl-right" Text="User"></asp:Label>
                                <div class="col-md-10">
                                    <%--<asp:label ID="lblnumber" runat="server" CssClass="form-control">--%>
                                        <asp:textbox ID="txtusr" runat="server" CssClass="form-control">
                                        </asp:textbox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label runat="server" AssociatedControlID="txtpsw"
                                    CssClass="col-sm-2 control-label text-xl-right" Text="Password"></asp:Label>
                                <div class="col-md-10">
                                    <%--<asp:label ID="lblnumber" runat="server" CssClass="form-control">--%>
                                        <asp:textbox ID="txtpsw" runat="server" Text="*******" CssClass="form-control">

                                        </asp:textbox>
                                        <input id="txtshapsw" runat="server" type="hidden" />
                                </div>
                            </div>


                            <%-- modulepriv --%>
                                <div class="form-group row">
                                    <asp:Label runat="server" AssociatedControlID="ddlmodulepriv"
                                        CssClass="col-sm-2 control-label text-xl-right" Text="Module Priv"></asp:Label>
                                    <div class="col-md-10">
                                        <asp:dropdownlist ID="ddlmodulepriv" runat="server" CssClass="form-control"
                                            OnSelectedIndexChanged="ddlmodulepriv_SelectedIndexChanged"
                                            AppendDataBoundItems="false" AutoPostBack="True" EnableViewState="True"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Value="-1">None</asp:ListItem>
                                            <asp:ListItem Value="0">Admin</asp:ListItem>
                                            <asp:ListItem Value="1">User</asp:ListItem>
                                        </asp:dropdownlist>


                                    </div>
                                </div>
                                <%-- modulepriv --%>
                                    <%-- rptPriv --%>
                                        <div class="form-group row">
                                            <asp:Label runat="server" AssociatedControlID="ddlrptPriv"
                                                CssClass="col-sm-2 control-label text-xl-right" Text="Rpt Priv">
                                            </asp:Label>
                                            <div class="col-md-10">
                                                <asp:dropdownlist ID="ddlrptPriv" runat="server" CssClass="form-control"
                                                    OnSelectedIndexChanged="ddlrptPriv_SelectedIndexChanged"
                                                    AppendDataBoundItems="false" AutoPostBack="True"
                                                    EnableViewState="True" ViewStateMode="Enabled">
                                                    <asp:ListItem Value="-1">None</asp:ListItem>
                                                    <asp:ListItem Value="0">NonRevenue</asp:ListItem>
                                                    <asp:ListItem Value="1">Revenue</asp:ListItem>
                                                </asp:dropdownlist>

                                            </div>
                                        </div>
                                        <%-- rptPriv --%>
                                            <div class="form-group row">
                                                <asp:Label runat="server" AssociatedControlID="txtemail"
                                                    CssClass="col-sm-2 control-label text-xl-right" Text="Email">
                                                </asp:Label>
                                                <div class="col-md-10">
                                                    <%--<asp:label ID="lblnumber" runat="server"
                                                        CssClass="form-control">--%>
                                                        <asp:textbox ID="txtemail" runat="server" Text=""
                                                            CssClass="form-control">
                                                        </asp:textbox>

                                                </div>
                                            </div>
                                            <%-- Modul --%>
                                                <div class="form-group row">
                                                    <asp:Label runat="server" AssociatedControlID="ddlModul"
                                                        CssClass="col-sm-2 control-label text-xl-right" Text="Modul">
                                                    </asp:Label>
                                                    <div class="col-md-10">
                                                        <asp:dropdownlist ID="ddlModul" runat="server"
                                                            CssClass="form-control" AppendDataBoundItems="false"
                                                            AutoPostBack="True" EnableViewState="True"
                                                            ViewStateMode="Enabled">
                                                            <asp:ListItem Value="-1">None</asp:ListItem>
                                                            <asp:ListItem Value="1">Web Report</asp:ListItem>
                                                            <asp:ListItem Value="2">CRM</asp:ListItem>
                                                            <asp:ListItem Value="3">CRM & Report</asp:ListItem>
                                                        </asp:dropdownlist>

                                                    </div>
                                                </div>
                                                <%-- Modul --%>
                                                    <%-- Rptf --%>
                                                        <div class="form-group row">
                                                            <asp:Label runat="server" AssociatedControlID="ddlRptf"
                                                                CssClass="col-sm-2 control-label text-xl-right"
                                                                Text="Reportf"></asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:dropdownlist ID="ddlRptf" runat="server"
                                                                    CssClass="form-control" AppendDataBoundItems="false"
                                                                    AutoPostBack="True" EnableViewState="True"
                                                                    ViewStateMode="Enabled">
                                                                    <asp:ListItem Value="-1">None</asp:ListItem>
                                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                                </asp:dropdownlist>

                                                            </div>
                                                        </div>
                                                        <%-- Rptf --%>

                                                            <%-- Option --%>
                                                                <div class="form-group row">
                                                                    <asp:Label runat="server"
                                                                        AssociatedControlID="lvwOption"
                                                                        CssClass="col-sm-2 control-label text-xl-right"
                                                                        Text="Option"></asp:Label>
                                                                    <div class="col-md-10"
                                                                        style="width: 50%;height:100%;text-align:center; flex:inherit;">

                                                                        <asp:ListView ID="lvwOption" runat="server">
                                                                            <LayoutTemplate>
                                                                                <table id="tblitem_xdetail"
                                                                                    class="table table-bordered">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th
                                                                                                style="text-align:center;">
                                                                                                <asp:CheckBox
                                                                                                    ID="cbxOption"
                                                                                                    OnCheckedChanged="cbxOption_Clicked"
                                                                                                    AutoPostBack="true"
                                                                                                    ToolTip="Select All"
                                                                                                    runat="server" />
                                                                                            </th>
                                                                                            <th
                                                                                                style="text-align:center;display:none;">
                                                                                                <b>ID</b>
                                                                                            </th>
                                                                                            <th
                                                                                                style="text-align:center;">
                                                                                                <b>Item Option</b>
                                                                                            </th>

                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tr id="itemPlaceholder"
                                                                                        runat="server">
                                                                                    </tr>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td style="width:5%">
                                                                                        <asp:CheckBox
                                                                                            ID="cbxOptionDetail"
                                                                                            runat="server">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                    <td style="display:none">
                                                                                        <asp:Label ID="lblDetailID"
                                                                                            runat="server"
                                                                                            Text=' <%# Eval("id")%>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblDetailName"
                                                                                            runat="server"
                                                                                            Text=' <%# Eval("name")%>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                </tr>

                                                                            </ItemTemplate>

                                                                        </asp:ListView>


                                                                    </div>
                                                                </div>
                                                                <%-- Option --%>


                        </div>


                        <div class="form-group row">
                            <div class="col-sm-2"></div>
                            <div class="col-md-offset-2 col-md-10">
                                <table>
                                    <tr>
                                        <%--<td>

                                            <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click"
                                                Text="Add" CssClass="btn btn-primary"
                                                Style="font-weight: 600; border-color:gray; width:108px;" />
                                            </td>--%>
                                            <td>

                                                <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click"
                                                    Text="Update" CssClass="btn btn-primary"
                                                    Style="font-weight: 600; border-color:gray; width:108px;"
                                                    Visible="false" />
                                            </td>
                                            <td style="width:10%;"></td>
                                            <td>
                                                <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click"
                                                    Text="Cancel" CssClass="btn btn-primary"
                                                    Style="font-weight: 600; border-color:gray; width:108px;"
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