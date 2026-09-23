<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_Supplier.aspx.vb" Inherits="DreamPos_WebReport.Pages_Supplier" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <style type="text/css">
          .displayaction
        {
        	
           
        }
        .nodisplay
        {
            display: none;
        }
        .mycheckbox input[type="checkbox"] 
        { 
            margin-right: 10px; 
        }
        #dtExcelBtnContainer .dt-button {
            background: #FFFFFF !important;
            color: #3a4f63 !important;
            border: 1px solid transparent !important;
            background-image: none !important;
            vertical-align: top !important;
        }
        #dtExcelBtnContainer .dt-button:hover {
            background: #e2e6ea !important;
            color: #3a4f63 !important;
        }
    </style>
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.24/css/jquery.dataTables.min.css" />
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/buttons/1.7.0/css/buttons.dataTables.min.css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.0/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.7.0/js/buttons.html5.min.js"></script>
    
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
                dom: 'rtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export Excel',
                        className: 'btn btn-primary',
                        exportOptions: {
                            columns: [ 0, 1, 2, 3, 4, 5 ]
                        }
                    }
                ]
            });
            
            $('#dtExcelBtnContainer').append(table.buttons().container().find('button'));

            $("[id$='txtsearch']").on('keyup change', function () {
                table.search(this.value).draw();
            });
            
            $("[id$='btnsrc']").on('click', function(e){
                e.preventDefault();
                table.search($("[id$='txtsearch']").val()).draw();
            });
        });
    </script>

      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Supplier</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Supplier</li>
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
         <div class="form-inline" id="formload" runat="server" style="margin:0 20px; width:100%;background: #3a4f63;padding:0.2rem 0.8rem">
                  <div class="form-group mx-sm-3 mt-mb-1">
                    <asp:label for="txtsearch" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">Name:</b></asp:label>
                    <asp:TextBox id="txtsearch" runat="server" Width="300px" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                </div>
               <%-- <div class="form-group mx-sm-3 mt-mb-1">
                    <asp:label for="chklink" runat="server"> <b style="color: #FFFFFF;margin-right:0.5rem;">By Link:</b></asp:label>
                            <div class="form-check">
                                <asp:CheckBox ID="chklink" runat="server" AutoPostBack="false" style="color: #FFFFFF;" class="form-check-input" type="checkbox"/>
                              
                                <label class="form-check-label" for="flexCheckChecked">
                               <b style="color: #FFFFFF;margin-right:0.5rem;"> ALLDATA</b>
                                </label>
                            </div>
                </div>--%>
                <div class="form-group mx-sm-3 mt-mb-1">
                         <div class="btn-group" role="group" style="margin:0.5rem" >
                        <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-mini" title="Search" style="background:#FFFFFF;color:#3a4f63;">
                            <i class="fas fa-search"></i>
                        </button>
                        </div>
                    <div class="btn-group" role="group" style="margin:0.5rem">
                        <button type="button" runat="server" id="btnnew" onserverclick="btnNew_Click" class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
                           <asp:Label runat="server">New</asp:Label>
                        </button>
                        </div>
                    <div class="btn-group" role="group" style="margin:0.5rem">
                        <button type="button" runat="server" id="btnExportExcel" onserverclick="btnExportExcel_Click" class="btn btn-primary" title="Import Excel" style="background:#FFFFFF;color:#3a4f63;">
                           <asp:Label runat="server">Import Excel</asp:Label>
                        </button>
                    </div>
                    <div class="btn-group" role="group" style="margin:0.5rem" id="dtExcelBtnContainer">
                    </div>
                      </div>
                 
           

                  


                   
                    
                  

                </div>
             
     

    </div>
    <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>


     <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
          <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server">
              <LayoutTemplate>
    <div class="table-responsive">
        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%" class="menutab table table-bordered">
            <thead>
                <tr role="row">
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Name</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Email</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Address</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Phone</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Customer</b>
                    </th>
                     <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Notes</b>
                    </th>
                    
                   <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
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
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("name")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("email")%>'></asp:Label>
                    </td>
                    
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("address")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text=' <%# Eval("phone")%>'></asp:Label>
                    </td>
                       <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("Custname")%>'></asp:Label>
                    </td>
                      <td>
                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("Notes")%>'></asp:Label>
                    </td>
                    <td style="width: 8%; text-align: center">
                        <%--//==== Here we have used CommandName property to distinguish which button is 
                        clicked and we have passed our primary key to CommandArgument property. ====//--%>
                        <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record." Visible="true" class='<%# Getdisplay() %>'
                            CommandArgument='<%#Eval("id") %>' runat="server" ImageUrl="/images/edit.png" />
                        <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                            ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("id") %>' runat="server"
                            ImageUrl="~/images/delete.png"></asp:ImageButton>
                    </td>




                    </tr>
                </ItemTemplate>
            </asp:ListView>
        <%--</table>--%>

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
