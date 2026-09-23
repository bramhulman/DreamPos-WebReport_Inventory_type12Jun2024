<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_StkGroup.aspx.vb" Inherits="DreamPos_WebReport.Pages_StkGroup" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
        <style type="text/css">
            .mycheckbox input[type="checkbox"] {
                margin-right: 10px;
            }
        </style>
        <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
            <link type="text/css" rel="stylesheet"
                href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />--%>
            <%--<link type="text/css" rel="stylesheet"
                href="https://cdn.datatables.net/1.10.9/css/dataTables.bootstrap.min.css" />--%>
            <%-- <link type="text/css" rel="stylesheet"
                href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
            <link type="text/css" rel="stylesheet"
                href="https://cdn.datatables.net/responsive/1.0.7/css/responsive.bootstrap.min.css" />--%>
            <%-- <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
                <script type="text/javascript"
                    src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
                <script type="text/javascript"
                    src="https://cdn.datatables.net/responsive/1.0.7/js/dataTables.responsive.min.js"></script>
                <script type="text/javascript"
                    src="https://cdn.datatables.net/1.10.9/js/dataTables.bootstrap.min.js"></script>
                <script type="text/javascript"
                    src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>--%>

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
                    //$(function () {
                    //    $("#tblEmpDetails").DataTable();
                    //});
                </script>

                <!-- Content Header (Page header) -->
                <div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0 text-dark">Stock group</h1>
                            </div><!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                                    <li class="breadcrumb-item active">Stock group</li>
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
                        <div class="form-inline" id="Div1" runat="server"
                            style="margin:0 20px; width:100%;background: #3a4f63;padding:0.2rem 0.8rem">
                            <div class="form-group mx-sm-3 mt-mb-1">
                                <asp:label for="txtsearch" runat="server"> <b
                                        style="color: #FFFFFF;margin-right:0.5rem;">Name:</b></asp:label>
                                <asp:TextBox id="txtsearch" runat="server" Width="300px" CssClass="form-control"
                                    placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                            </div>
                            <%-- <div class="form-group mx-sm-3 mt-mb-1">
                                <asp:label for="chklink" runat="server"> <b
                                        style="color: #FFFFFF;margin-right:0.5rem;">By Link:</b></asp:label>
                                <div class="form-check">
                                    <asp:CheckBox ID="chklink" runat="server" AutoPostBack="false"
                                        style="color: #FFFFFF;" class="form-check-input" type="checkbox" />

                                    <label class="form-check-label" for="flexCheckChecked">
                                        <b style="color: #FFFFFF;margin-right:0.5rem;"> ALLDATA</b>
                                    </label>
                                </div>
                        </div>--%>
                        <div class="form-group mx-sm-3 mt-mb-1">
                            <div class="btn-group" role="group" style="margin:0.5rem">
                                <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-mini"
                                    title="Search" style="background:#FFFFFF;color:#3a4f63;">
                                    <i class="fas fa-search"></i>
                                </button>
                            </div>
                            <div class="btn-group" role="group" style="margin:0.5rem">
                                <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click"
                                    class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
                                    <asp:Label runat="server">New</asp:Label>
                                </button>
                            </div>
                            <%-- <div class="btn-group" role="group" style="margin:0.5rem">
                                <button type="button" runat="server" id="btnExportExcel"
                                    onserverclick="btnExportExcel_Click" class="btn btn-primary" title="Export / Import"
                                    style="background:#FFFFFF;color:#3a4f63;">
                                    <asp:Label runat="server">Export / Import Excel</asp:Label>
                                </button>
                        </div>--%>
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
                                                        <b style="color: #414a4c">Code</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Name</b>
                                                    </th>
                                                    <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                        rowspan="1" colspan="1" aria-sort="ascending"
                                                        aria-label="Rendering engine: activate to sort column descending">
                                                        <b style="color: #414a4c">Cust Name</b>
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

                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Code")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("Name")%>'></asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("Custname")%>'>
                                        </asp:Label>
                                    </td>

                                    <td style="width: 8%; text-align: center">
                                        <%-- //====Here we have used CommandName property to distinguish which button is
                                            clicked and we have passed our primary key to CommandArgument
                                            property.====//--%>
                                            <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                                                CommandArgument='<%#Eval("id") %>' runat="server"
                                                ImageUrl="/images/edit.png" />
                                            <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                                                ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("id") %>'
                                                runat="server" ImageUrl="~/images/delete.png"></asp:ImageButton>
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




                        <script>

                            if ($(window).width() >= 960) {
                                document.getElementById('<%= Div1.ClientID %>').style.width = "100%";
                            } else {
                                document.getElementById('<%= Div1.ClientID %>').style.width = "50%";

                            }


                        </script>
    </asp:Content>