<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master"
    CodeBehind="Pages_InvoicePayment.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvoicePayment" %>
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
        </style>
        <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
        <link type="text/css" rel="stylesheet"
            href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />

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
                $("#tblEmpDetails").DataTable();
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
                if (confirm("Do you want to Show DO File?")) {
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
                        <h1 class="m-0 text-dark">Order Stock From Store</h1>
                    </div><!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Order Stock From Store</li>
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
                    <div class="form-group mx-sm-2 mt-mb-1">
                        <asp:label for="txtDate" runat="server"> <b
                                style="color: #FFFFFF;margin-right:0.5rem;">From:</b></asp:label>
                        <asp:TextBox style="text-align:center;" runat="server" id="txtDate" Class="form-control"
                            AutoPostBack="true" />
                        <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtDate"
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>

                    </div>
                    <div class="form-group mx-sm-2 mt-mb-1">
                        <asp:label for="txtDateto" runat="server"> <b
                                style="color: #FFFFFF;margin-right:0.5rem;">to:</b></asp:label>
                        <asp:TextBox style="text-align:center;" runat="server" id="txtDateto" Class="form-control"
                            AutoPostBack="true" />
                        <ajaxtoolkit:CalendarExtender ID="cleto" runat="server" TargetControlID="txtDateto"
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>

                    </div>
                    <div class="form-group mx-sm-2 mt-mb-1">
                        <asp:label for="DdlType" runat="server"> <b
                                style="color: #FFFFFF;margin-right:0.5rem;">Type:</b></asp:label>
                        <asp:DropDownList ID="DdlType" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            style="width:150px; text-align:center;" EnableViewState="True" ViewStateMode="Enabled">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="3">Received</asp:ListItem>
                            <asp:ListItem Value="4">Paid</asp:ListItem>

                        </asp:DropDownList>

                    </div>
                    <div class="form-group mx-sm-2 mt-mb-1">
                        <asp:label for="DdlCc" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">CC:</b>
                        </asp:label>
                        <asp:DropDownList ID="DdlCc" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            style="width:150px; text-align:center;" EnableViewState="True" ViewStateMode="Enabled">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>

                    </div>


                    <div class="form-group mx-sm-2 mt-mb-1">

                        <div class="btn-group" role="group" style="margin:0.1rem">
                            <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click"
                                class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
                                <asp:Label runat="server">Payment</asp:Label>
                            </button>
                        </div>

                        <%-- </div>
                            <div class="form-group mx-sm-3 mt-mb-1">--%>
                                <div class="btn-group" role="group" style="margin:0.5rem">
                                    <button type="button" runat="server" id="btnExportExcel"
                                        onserverclick="btnExportExcel_Click" class="btn btn-primary" title="Export"
                                        style="background:#FFFFFF;color:#3a4f63;">
                                        <asp:Label runat="server">Export Excel</asp:Label>
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
                                                    <b style="color: #414a4c">Cost Centre</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Status</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Payment</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Receive Date</b>
                                                </th>
                                                <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Invoice Total</b>
                                                </th>

                                                <%-- <th class="sorting_asc" tabindex="0" aria-controls="example2"
                                                    rowspan="1" colspan="1" aria-sort="ascending"
                                                    aria-label="Rendering engine: activate to sort column descending">
                                                    <b style="color: #414a4c">Action</b>
                                                    </th>--%>

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
                                    <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                                <%-- <td>
                                    <asp:Label ID="Label3" runat="server" Text=' <%# Eval("ccFromNm")%>'></asp:Label>
                                    </td>--%>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("ccToNm")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text=' <%# Eval("STATUS")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("payno")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server"
                                            Text=' <%# Eval("rcvdate", "{0:dd MMM yyyy}")%>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("Doctot", "{0:0.##}")%>'>
                                        </asp:Label>
                                    </td>
                                    <%-- <td style="width: 8%; text-align: center">

                                        <asp:ImageButton Style="margin-right:0.5rem;" ID="imgBtnEdit" CommandName="Edt"
                                            ToolTip="Edit a record."
                                            CommandArgument='<%#Eval("ID").ToString + ";" + Eval("ccTo").ToString + ";" + Eval("ccFrom").ToString + ";" + Eval("exDelDate").ToString + ";" + Eval("DelID").ToString + ";" + Eval("STATUS").ToString + ";" + Eval("Notes").ToString %>'
                                            runat="server" ImageUrl="images/edit.png">
                                        </asp:ImageButton>
                                        <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="Delete a record."
                                            OnClientClick="Confirm()" class='<%# GetBackground() %>' ID="ImageButton2"
                                            Visible='true' CommandName="Del"
                                            CommandArgument='<%#Eval("ID").tostring+ ";" +Eval("ccTo").Tostring%>'
                                            runat="server" ImageUrl="~/images/delete.png"></asp:ImageButton>
                                        <asp:ImageButton BackColor="Black" ToolTip="Print PDF." CssClass="btn2"
                                            OnClientClick="SendEmail()" ID="imgEmail" CommandName="Send"
                                            CommandArgument='<%#Eval("ID")+ ";"%>' runat="server"
                                            ImageUrl="images/print.png"></asp:ImageButton>
                                        <asp:ImageButton Style="margin-right:0.5rem;" ToolTip="Print Delivery Order."
                                            OnClientClick="SendEmail2()" class='<%# GetBackgroundDO() %>'
                                            ID="ImageButton1" Visible='true' CommandName="Send2"
                                            CommandArgument='<%#Eval("ID")+ ";"%>' runat="server"
                                            ImageUrl="images/list.png"></asp:ImageButton>

                                        </td>--%>




                                        </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <%--< /table>--%>

                            <br />
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