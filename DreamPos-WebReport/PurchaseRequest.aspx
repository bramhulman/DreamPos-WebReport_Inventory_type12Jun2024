<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="PurchaseRequest.aspx.vb" Inherits="DreamPos_WebReport.PurchaseRequest" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
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
        </style>
        <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
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
                    if (confirm("Do you want to Reject data?")) {
                        confirm_value.value = "Yes";
                    } else {
                        confirm_value.value = "No";
                    }
                    document.forms[0].appendChild(confirm_value);
                }
                //$(function () {
                //    $("#tblEmpDetails").DataTable();
                //});
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
                            <h1 class="m-0 text-dark">Purchase Request</h1>
                        </div><!-- /.col -->
                        <div class="col-sm-6">
                            <ol class="breadcrumb float-sm-right">
                                <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                <li class="breadcrumb-item active">Purchase Request</li>
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
                        <div class="form-group mx-sm-3 mt-mb-1">
                            <asp:label for="DdlType" runat="server"> <b
                                    style="color: #FFFFFF;margin-right:0.5rem;">Type:</b></asp:label>
                            <asp:DropDownList ID="DdlType" runat="server" AppendDataBoundItems="True"
                                AutoPostBack="True" style="width:300px; text-align:center;" EnableViewState="True"
                                ViewStateMode="Enabled">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                                <asp:ListItem Value="1">--REQUEST--</asp:ListItem>
                                <asp:ListItem Value="2">--APPROVED--</asp:ListItem>
                                <asp:ListItem Value="3">--REJECTED--</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div class="form-group mx-sm-3 mt-mb-1">
                            <asp:label AssociatedControlID="txtsearch" runat="server">Nama:</asp:Label>
                            <asp:TextBox id="txtsearch" runat="server" Width="300px" CssClass="form-control"
                                placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                        </div>

                        <div class="form-group mx-sm-3 mt-mb-1">
                            <div class="btn-group" role="group" style="margin:0.5rem">
                                <button runat="server" id="btnSearch" onserverclick="btnSearch_Click"
                                    class="btn btn-mini inventory-search-button" title="Search" style="background:#FFFFFF;color:#3a4f63;" aria-label="Cari nama"><i class="fas fa-search" aria-hidden="true"></i></button>
                            </div>
                            <div class="btn-group" role="group" style="margin:0.5rem">
                                <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click"
                                    class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
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
                                                    aria-label="Rendering engine: activate to sort column descending" style="display:none;">
                                                    <b style="color: #414a4c">ID</b>
                                                </th>

                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Number</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">PR Date</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Required Date</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">CC</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Request by</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Status</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Approved by</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Approved Date</b>
                                                </th>
                                               <%-- <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Remarks</b>
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

                                <td style="display:none;">
                                    <asp:Label ID="Label2" runat="server" Text=' <%# Eval("pr_id")%>'></asp:Label>
                                </td>

                                <td>
                                    <asp:Label ID="Label1" runat="server" Text=' <%# Eval("pr_no")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text=' <%# Eval("prdate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text=' <%# Eval("requireddate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text=' <%# Eval("request_by")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text=' <%# Eval("stat")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text=' <%# Eval("appBy")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text=' <%# Eval("aprDate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                </td>
<%--                                <td>
                                    <asp:Label ID="Label6" runat="server" Text=' <%# Eval("appBy")%>'></asp:Label>
                                </td>--%>
                                <td style="width: 8%; text-align: center">
                                    <%-- //====Here we have used CommandName property to distinguish which button is
                                        clicked and we have passed our primary key to CommandArgument
                                        property.====//--%>
                                        <asp:ImageButton Style="margin-right:0.5rem;" ID="imgBtnEdit" CommandName="Edt"
                                            ToolTip="Edit a record."
                                            CommandArgument='<%#Eval("pr_id").ToString + ";" + Eval("pr_no").ToString + ";" + Eval("prdate").ToString + ";" + Eval("requireddate").ToString + ";" + Eval("CC").ToString + ";" + Eval("request_by").ToString + ";" + Eval("stat").ToString %>'
                                            runat="server" ImageUrl="images/edit.png">
                                        </asp:ImageButton>
                                        <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="Reject a record."
                                            OnClientClick="Confirm()" class='<%# GetBackground() %>' ID="ImageButton2"
                                            Visible='true' CommandName="Del"
                                            CommandArgument='<%#Eval("pr_id").ToString + ";" + Eval("cc").ToString%>'
                                            runat="server" ImageUrl="~/images/cancel.png"></asp:ImageButton>
                                        <asp:ImageButton BackColor="Black" ToolTip="Print PDF."
                                            OnClientClick="SendEmail()" class='<%# Getprintbg() %>' ID="imgEmail"
                                            CommandName="Send" CommandArgument='<%#Eval("pr_id").ToString + ";" + Eval("pr_no").ToString + ";" + Eval("prdate").ToString + ";" + Eval("requireddate").ToString + ";" + Eval("cc").ToString + ";" + Eval("ccname").ToString + ";" + Eval("request_by").ToString + ";" + Eval("stat").ToString %>' runat="server"
                                            ImageUrl="images/print.png"></asp:ImageButton>

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
                                    <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails"
                                        PageSize="15">
                                        <Fields>
                                            <asp:NumericPagerField ButtonType="Link" />
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>

                    <script>

                        if ($(window).width() >= 960) {
                            $("#<%= formload.ClientID %>").css("width", "100%");
                        } else {
                            $("#<%= formload.ClientID %>").css("width", "50%");

                        }


                    </script>
    </asp:Content>