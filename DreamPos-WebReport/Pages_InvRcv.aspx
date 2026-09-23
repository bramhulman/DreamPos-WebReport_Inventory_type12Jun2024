<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvRcv.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvRcv" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        </asp:Content>
        <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
            <asp:ScriptManager runat="server">

            </asp:ScriptManager>
            <style type="text/css">
                @media (max-width: 450px) {
                    .form-inline .form-check {
                        display: inline;
                    }
                }

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

                /* Ensure children of dateWrapper like form-groups display inline-flex */
                #dateWrapper .form-group {
                    display: flex;
                    align-items: center;
                    margin-bottom: 0;
                    margin-right: 10px;
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
                
                #spinner {
                    position: fixed;
                    z-index: 99999;
                    height: 100%;
                    width: 100%;
                    top: 0;
                    left: 0;
                    background-color: rgba(0, 0, 0, 0.4); 
                }

                #spinner .center {
                    position: absolute;
                    top: 50%;
                    left: 50%;
                    transform: translate(-50%, -50%);
                }

                #spinner .center img {
                    width: 100px; 
                    height: 100px;
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
                            $('#spinner').show(); // Tampilkan loading
                        } else {
                            confirm_value.value = "No";
                        }
                        document.forms[0].appendChild(confirm_value);
                    }
                    $(function () {
                        var table = $("#tblEmpDetails").DataTable({
                            "order": [],
                            "initComplete": function () {
                                var dateWrapper = $("#dateWrapper");
                                var newWrapper = $("#newWrapper");
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

                                // Style New Wrapper
                                newWrapper.css({ "display": "flex", "align-items": "center", "margin": "0" });
                                newWrapper.find("button").css({ "height": "30px", "padding": "0 15px", "line-height": "1", "display": "flex", "align-items": "center" });

                                // Style Search Label/Input
                                var searchLabel = filterContainer.find("label");
                                searchLabel.css({ "margin-bottom": "0", "display": "flex", "align-items": "center", "white-space": "nowrap" });
                                searchLabel.find("input").css({ "height": "30px", "margin-left": "5px", "display": "inline-block", "width": "auto" });

                                // Hide the original formload container
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
                    function SendEmail2() {
                        var confirm_value = document.createElement("INPUT");
                        confirm_value.type = "hidden";
                        confirm_value.name = "confirm_value";
                        if (confirm("Do you want to Print PO Unit1?")) {
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
                            <div class="col-sm-4">
                                <h1 class="m-0 text-dark">Order/ Receive Supplier</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-5" style="display:flex; align-items:center; padding:8px 15px;">
                                    <asp:TextBox id="alert1" runat="server" CssClass="form-control"
                                        style="max-height:130px; overflow-y:auto; overflow-x:hidden; resize:none; color:#721c24; background-color:#f8d7da; border:1px solid #f5c6cb; border-radius:8px; padding:8px 12px; box-shadow:0 4px 6px rgba(0,0,0,0.1); font-weight:bold; font-size:14px; line-height:1.5; width:100%; display:block;"
                                        TextMode="MultiLine" Rows="5" width="100%" ReadOnly="True" Font-Bold="True"
                                        Font-Size="Small"></asp:TextBox>
                            </div>

                            <div class="col-sm-3">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Order/ Receive Supplier</li>
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
                            style="margin:0 20px; width:100%;background: #3a4f63;padding:0.8rem">

                            <div id="dateWrapper">
                                <div class="form-group mx-sm-3 mt-mb-1">
                                    <asp:label for="txtDate" runat="server"> <b style="margin-right:0.5rem;">From:</b>
                                    </asp:label>
                                    <asp:TextBox style="text-align:center; width:140px;" runat="server" id="txtDate"
                                        Class="form-control" AutoPostBack="true" />
                                    <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate"
                                        Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                                </div>
                                <div class="form-group mx-sm-3 mt-mb-1">
                                    <asp:label for="txttoDate" runat="server"> <b style="margin-right:0.5rem;">To:</b>
                                    </asp:label>
                                    <asp:TextBox style="text-align:center; width:140px;" runat="server" id="txttoDate"
                                        Class="form-control" AutoPostBack="true" />
                                    <ajaxtoolkit:CalendarExtender ID="cleto" runat="server" TargetControlID="txttoDate"
                                        Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                                </div>
                            </div>

                            <div id="newWrapper">
                                <div class="btn-group" role="group" style="margin:0.5rem">
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
                            <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server">
                                <LayoutTemplate>
                                    <div class="table-responsive">
                                        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%"
                                            class="menutab table table-bordered">
                                            <thead>
                                                <tr role="row">
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">ID</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">CC Name</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Supplier</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">DO</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Status</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">ACC</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Date</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Total</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Email</b>
                                                    </th>
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
                                    <tr style="cursor:pointer;" class="clickable-row" onclick="if(!$(event.target).closest('a, button, input, img').length) { $(this).find('input[id*=\'imgBtnEdit\']').click(); }">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("CCName")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("SupName")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text=' <%# Eval("DO")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("Status")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("acc2")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text=' <%# Eval("statdate")%>'>
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text=' <%# Eval("Doctot", "{0:#,0}")%>'>
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("Email")%>'></asp:Label>
                                    </td>
                                    <td style="width: 8%; text-align: center">
                                        <%-- //====Here we have used CommandName property to distinguish which button is
                                            clicked and we have passed our primary key to CommandArgument
                                            property.====//--%>
                                            <asp:ImageButton Style="margin-right:0.5rem;" ID="imgBtnEdit"
                                                CommandName="Edt" ToolTip="Edit a record."
                                                CommandArgument='<%#Eval("ID").ToString + ";" + Eval("Cc").ToString + ";" + Eval("Supid").ToString + ";" + Eval("Do").ToString + ";" + Eval("flag").ToString + ";" + Eval("CreatedDate").ToString + ";" + Eval("invdate").ToString + ";" + Eval("acc2").ToString + ";" + Eval("exDelDate").ToString + ";" + Eval("p_tax").ToString + ";" + Eval("n_tax").ToString%>'
                                                runat="server" ImageUrl="images/edit.png">
                                            </asp:ImageButton>
                                            <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="Delete a record."
                                                OnClientClick="Confirm()" ID="ImageButton2" Visible='true'
                                                CommandName="Del"
                                                CommandArgument='<%#Eval("ID") + ";" + Eval("flag").ToString + ";" + Eval("CreatedDate").ToString + ";" + Eval("exDelDate").ToString%>'
                                                runat="server" ImageUrl="~/images/delete.png"
                                                Class='<%# GetBackground2(Eval("id")) %>'></asp:ImageButton>
                                            <asp:ImageButton BackColor="Black" ToolTip="Print PDF." CssClass="btn2"
                                                OnClientClick="SendEmail()" ID="imgEmail" CommandName="Send"
                                                CommandArgument='<%#Eval("ID")+ ";"%>' runat="server"
                                                ImageUrl="images/print.png"></asp:ImageButton>
                                            <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="PO Unit1."
                                                OnClientClick="SendEmail2()" ID="ImageButton1" Visible='true'
                                                CommandName="Send2" CommandArgument='<%#Eval("ID") + ";"%>'
                                                runat="server" Class='<%# GetBackgroundu1(Eval("id")) %>'
                                                ImageUrl="images/list.png"></asp:ImageButton>
                                    </td>




                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            <%--< /table>--%>

                                <br />
                        </div>
                        <div class="table-responsive" style="margin:0 20px; width:100%;">
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
                            document.getElementById("<%= formload.ClientID %>").style.width = "100%";
                        } else {
                            document.getElementById("<%= formload.ClientID %>").style.width = "50%";

                        }


                    </script>
        </asp:Content>