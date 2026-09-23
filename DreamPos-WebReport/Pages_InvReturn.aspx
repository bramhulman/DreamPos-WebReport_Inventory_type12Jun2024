<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvReturn.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvReturn" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        </asp:Content>
        <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
            <asp:ScriptManager runat="server">

            </asp:ScriptManager>
            <style type="text/css">
                .displayaction {}

                .nodisplay {
                    display: none;
                }

                .mycheckbox input[type="checkbox"] {
                    margin-right: 10px;
                }

                [disabled] {
                    /* Text and background colour, medium red on light yellow */
                    color: #933;
                    background-color: #ffc;
                }

                .dataTables_wrapper .row {
                    display: flex;
                    align-items: center;
                }

                .dataTables_length {
                    margin-right: 15px;
                }

                #dateWrapper,
                #newWrapper {
                    display: flex;
                    align-items: center;
                    margin-right: 15px;
                }

                .dataTables_filter {
                    display: flex;
                    align-items: center;
                    justify-content: flex-end;
                }

                .dataTables_filter label {
                    display: flex;
                    align-items: center;
                    margin-bottom: 0;
                }
            </style>
            <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js">
                </script>
                <link type="text/css" rel="stylesheet"
                    href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />--%>

                <script type="text/javascript">
                    $(document).ready(function () {
                        $('.btnstyle').click(function () {
                            $('#spinner').show();
                        });
                    });
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

                    $(function () {
                        var table = $("#tblEmpDetails").DataTable({
                            "initComplete": function () {
                                var dateWrapper = $("#dateWrapper");
                                var newWrapper = $("#newWrapper");
                                var lengthContainer = $(".dataTables_length");
                                var filterContainer = $(".dataTables_filter");

                                // Prepend New button then Date filter so they appear: Date | New | Search
                                filterContainer.prepend(newWrapper);
                                filterContainer.prepend(dateWrapper);

                                // Apply Flexbox to filter container for alignment
                                filterContainer.css({
                                    "display": "flex",
                                    "align-items": "center",
                                    "justify-content": "flex-end",
                                    "flex-wrap": "nowrap",
                                    "gap": "10px"
                                });

                                // Style Date Wrapper
                                dateWrapper.css({ "display": "flex", "align-items": "center", "margin": "0" });
                                dateWrapper.find("label").css({ "margin-right": "5px", "margin-bottom": "0", "white-space": "nowrap" });
                                dateWrapper.find("input").css({ "height": "30px", "padding": "5px", "display": "inline-block", "width": "auto" });

                                // Style New Wrapper
                                newWrapper.css({ "display": "flex", "align-items": "center", "margin": "0" });
                                newWrapper.find("button").css({ "height": "30px", "padding": "0 15px", "line-height": "1", "display": "flex", "align-items": "center" });

                                // Style Search Label/Input (DataTables standard)
                                var searchLabel = filterContainer.find("label");
                                searchLabel.css({ "margin-bottom": "0", "display": "flex", "align-items": "center", "white-space": "nowrap" });
                                searchLabel.find("input").css({ "height": "30px", "margin-left": "5px", "display": "inline-block", "width": "auto" });

                                // Ensure label color matches theme
                                dateWrapper.find("label b").css("color", "inherit");

                                // Hide the original formload container if empty or just safer to hide
                                document.getElementById('<%= formload.ClientID %>').style.display = 'none';
                            }
                        });
                    });
                </script>
                <script type="text/javascript">
                    function SendEmail() {
                        var confirm_value = document.createElement("INPUT");
                        confirm_value.type = "hidden";
                        confirm_value.name = "confirm_value";
                        if (confirm("Do you want to Show PDF File?")) {
                            confirm_value.value = "Yes";
                        } else {
                            confirm_value.value = "No";
                        }
                        document.forms[0].appendChild(confirm_value);
                    }
                </script>
                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Inventory Return</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Inventory Return</li>
                                </ol>
                            </div><!-- /.col -->
                        </div><!-- /.row -->
                    </div><!-- /.container-fluid -->
                </div>
                <!-- /.content-header -->
                <%-- <h2>User Access</h2>--%>
                    <div id="spinner" class="modal" style="display: none">
                        <div class="center">
                            <img alt="" src="/images/loader.gif" />
                        </div>
                    </div>
                    <div class="row">

                        <div class="form-inline" id="formload" runat="server"
                            style="margin:0 20px; width:100%;background: #3a4f63;padding:0.2rem 0.8rem">
                            <div class="form-group mx-sm-3 mt-mb-1" id="dateWrapper">
                                <asp:label for="txtDate" runat="server"> <b
                                        style="margin-right:0.5rem; color: #FFFFFF;">Date:</b>
                                </asp:label>
                                <asp:TextBox style="text-align:center;" runat="server" id="txtDate" Class="form-control"
                                    AutoPostBack="false" />
                                <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate"
                                    Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                            </div>

                            <div class="form-group mx-sm-3 mt-mb-1" style="display:none;">
                                <asp:label for="txtsearch" runat="server"> <b
                                        style="color: #FFFFFF;margin-right:0.5rem;">Name:</b></asp:label>
                                <asp:TextBox id="txtsearch" runat="server" Width="300px" CssClass="form-control"
                                    placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                            </div>

                            <div class="form-group mx-sm-3 mt-mb-1">
                                <div class="btn-group" role="group" style="margin:0.5rem; display:none;">
                                    <button runat="server" id="btnSearch" onserverclick="btnSearch_Click"
                                        class="btn btn-mini" title="Search" style="background:#FFFFFF;color:#3a4f63;">
                                        <i class="fas fa-search"></i>
                                    </button>
                                </div>
                                <div class="btn-group" role="group" style="margin:0.5rem" id="newWrapper">
                                    <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click"
                                        class="btn btn-primary" title="new">
                                        <asp:Label runat="server">New</asp:Label>
                                    </button>
                                </div>

                            </div>





                        </div>



                    </div>


                    <div class="row">
                        <div class="form-horizontal" style="margin:0 20px; width:100%;">
                            <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                                OnPagePropertiesChanging="OnPagePropertiesChanging">
                                <LayoutTemplate>
                                    <div class="table-responsive">
                                        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%"
                                            class="menutab table table-bordered">
                                            <thead>
                                                <tr role="row">
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Id</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Supplier</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Date</b>
                                                    </th>
                                                    <%--<th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Total</b>
                                                        </th>--%>
                                                        <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                            rowspan="1" colspan="1" aria-sort="ascending"
                                                            aria-label="Rendering engine: activate to sort column descending">
                                                            <b style="color: #414a4c">Action</b>
                                                        </th>

                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                </LayoutTemplate>


                                <ItemTemplate>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("SupName")%>'></asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("CreateD")%>'></asp:Label>
                                    </td>
                                    <%-- <td>
                                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Doctot","{0:#,0}")%>'>
                                        </asp:Label>
                                        </td>--%>
                                        <td>
                                            <asp:ImageButton ID="imgBtnEdit" CssClass="btn2" CommandName="Edt"
                                                ToolTip="Edit a record."
                                                CommandArgument='<%#Eval("ID").ToString + ";" + Eval("Cc").ToString + ";" + Eval("Supid").ToString + ";" + Eval("Do").ToString + ";" + Eval("InvDate").ToString %>'
                                                runat="server" ImageUrl="images/edit.png">
                                            </asp:ImageButton>
                                            <asp:ImageButton ToolTip="Delete a record."
                                                class='<%# GetBackground2(Eval("id")) %>' OnClientClick="Confirm()"
                                                ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("ID")%>'
                                                runat="server" ImageUrl="images/delete.png"></asp:ImageButton>
                                        </td>
                                        </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            <%--< /table>--%>

                                <br />
                        </div>
                        <div class="table-responsive" style="margin:0 20px; width:100%; display:none;">
                            <table class="table">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="font-family: Calibri;margin: 0 20px;">
                            <asp:Label ID="lblError" runat="server" Text='' ForeColor="Red" Font-Size="Medium">
                            </asp:Label>
                            <asp:Label ID="lblnoitem" runat="server" Text="0" Visible="false"></asp:Label>

                        </div>
                    </div>

                    <script>

                        if ($(window).width() >= 960) {
                            document.getElementById('<%= formload.ClientID %>').style.width = "100%";
                        } else {
                            document.getElementById('<%= formload.ClientID %>').style.width = "50%";

                        }


                    </script>
        </asp:Content>