<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="UserList.aspx.vb" Inherits="DreamPos_WebReport.UserList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
        <asp:ScriptManager runat="server">

    </asp:ScriptManager>     
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

      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Item Master Update</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
              <li class="breadcrumb-item active">Item Master Update</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>
<div class="row" style="padding:5pt;">
         <div class="col-md-5" id="formload" runat="server" style="margin-left:30pt">
                   <%--<div class="card-body" style="width:100%; justify-content:center; display:flex; align-items:center">
                <div style="width:100%; justify-content:center; display:flex; align-items:center">--%>
                 <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="col-sm-2 btn btn-info" Style="font-weight: 700;padding-right:10px;" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnExport" runat="server" Text="EXPORT" OnClick="btnExport_Click" CssClass="col-sm-2 btn btn-info" Style="font-weight: 700;padding-right:10px;"></asp:Button>
             <asp:FileUpload ID="FileUpLoad1" AlternateText="You cannot upload files" CssClass="col-sm-4"
                    runat="server" onchange="readURL(this);"
                    style="font-weight: 700; color: #000000; background-color: #FFFFFF;padding-right:10px;" />   
             <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" CssClass="col-sm-2 btn btn-info" Style="font-weight: 700;padding-right:10px;" />
             <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="true">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
      </asp:PlaceHolder>
             <%--</div>--%>
            
<%-- notes --%>
                    <%--   <div class="form-group row col-md-12">
          <asp:Label runat="server" AssociatedControlID="txtNote" CssClass="col-sm-2 col-form-label" Text="Note"></asp:Label>
          <div class="col-md-10">
              <asp:Textbox ID="txtNote" runat="server" CssClass="form-control">
              </asp:Textbox>
              
          </div>
      </div>--%>
                       <%-- Form Type --%>
     <%--                     <div class="form-group row col-md-12">
            <asp:Label runat="server" AssociatedControlID="ddlFormType" CssClass="col-sm-3 col-form-label" Text="Form Type"></asp:Label>
            <div class="col-md-9">
                <asp:DropDownList ID="ddlFormType" runat="server" AutoPostBack="true" AppendDataBoundItems="True" EnableViewState="True" ViewStateMode="Enabled" CssClass="form-control" OnSelectedIndexChanged="ddlFormType_SelectedIndexChanged">
                        <asp:ListItem Text="Select Type" Value="0"></asp:ListItem>
                        <asp:ListItem Text="New" Value="1"></asp:ListItem>
                    <asp:ListItem Text="List Update" Value="2"></asp:ListItem>                 
                </asp:DropDownList>

            </div>
        </div>--%>
                                  

                 <%--</div>--%>
             
     

    </div>      
         

   </div>

     <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:96%;">
             <div id="searchForm" runat="server">
    <%--    <%= GeName()%>--%>

                 <%--<div class="row float-sm-right" style="width:40%;">
                     
      <asp:Label runat="server" AssociatedControlID="txtsrc_outxuser" CssClass="col-sm-2 col-form-label text-nowrap" Text="Name: "></asp:Label>
                    <div class="col-md-8">
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
<asp:TextBox id="txtsrc_outxuser" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                

                    <button runat="server" id="btnSearch" onserverclick="btnsrc_outxuser"  class="btn btn-mini" title="Search">
    <i class="fas fa-search"></i>
</button>
        </div>
      </div>
                        </div>

    </div>--%>
             
    </div>
             <div class="table-responsive" style="margin:0 20px; width:97%;">
                  <asp:UpdatePanel ID="upCrudGrid" runat="server">
    <ContentTemplate>
        <asp:GridView ID="gvItemMaster" runat="server" 

        AutoGenerateColumns="false" DataKeyNames="itemmasterid,itemname,itemnumber,itemgroupid,majorgroupid"
        ShowHeaderWhenEmpty="True"
            OnPreRender="gvItemMaster_PreRender"

            OnRowCommand="gvItemMaster_RowCommand"

            CssClass="table table-striped">
            <Columns>
                <asp:ButtonField CommandName="editRecord" 

 ControlStyle-CssClass="btn btn-info"

 ButtonType="Button" Text="Edit" 

 HeaderText="" ItemStyle-CssClass="text-center" ItemStyle-Width="4%">
     <ControlStyle CssClass="btn btn-info"></ControlStyle>
 </asp:ButtonField>
<asp:ButtonField CommandName="addstore" 

ControlStyle-CssClass="btn btn-info"

ButtonType="Button" Text="ADD Store" 

HeaderText="" ItemStyle-CssClass="text-center" ItemStyle-Width="4%">
    <ControlStyle CssClass="btn btn-info"></ControlStyle>
</asp:ButtonField>
                <asp:BoundField DataField="itemnumber" SortExpression="itemnumber" HeaderText="item number" />
                 <asp:BoundField DataField="itemname" SortExpression="itemname" HeaderText="item name" />
                <%--<asp:BoundField DataField="storenumber" SortExpression="storenumber" HeaderText="store number" />
                <asp:BoundField DataField="storename" SortExpression="storename" HeaderText="Store" />--%>
                <asp:BoundField DataField="itemgroupname" SortExpression="itemgroupname" HeaderText="Item Group" />
                <asp:BoundField DataField="majorgroupname" SortExpression="majorgroupname" HeaderText="Major Group" />
                <asp:BoundField DataField="Barcode" SortExpression="Barcode" HeaderText="Barcode" />
                
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
                 </div>
            
         </div>
      
               

          <!-- Modal Store -->
     <%--<asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>

               
<div class="modal fade" id="ModalStore" tabindex="-1" role="dialog" aria-labelledby="ModalStore" aria-hidden="true" data-backdrop="false" style="opacity:1;">
  <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
        
      <div class="modal-header">
        <h5 class="modal-title" id="ModalStoreLabel">Select Store</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>

        <div class="row">
        <div class="form-horizontal" id="Div2" runat="server" style="width:97%;">
                   <div class="card-body">
                  
               <div class="form-group row float-sm-right" style="width:100%;">
                    <asp:Label runat="server" AssociatedControlID="txtSearch" CssClass="col-sm-3 col-form-label text-nowrap" Text="Outlet: "></asp:Label>
                 
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
                    <asp:TextBox id="txtSearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                

                    <button runat="server" id="btnsrcoutlet" class="btn btn-mini" title="Search" onclick="btnsrcoutlet_click">
                    <i class="fas fa-search"></i>
                    </button>
        
                  
        </div>
      </div>
                   
                    </div>
                  </div>

                </div>
             
     

    </div>

      <div class="modal-body">
       <div class="table-responsive">
           <asp:HiddenField ID="HfUpdateitemID" runat="server" />
  <table class="table">
   <thead>
    <tr>
      <th scope="col"> <asp:CheckBox ID="Checkbox_allstore" runat="server" ToolTip="SelectAll" OnCheckedChanged="Checkbox_allstore_CheckedChanged"></asp:CheckBox></th>
      <%--<th scope="col" hidden="hidden">ID</th>--%>
        <th scope="col" hidden="hidden">Olnumber</th>
      <th scope="col">Store</th>
    </tr>
  </thead>
     <tbody>
         
      <asp:ListView ID="LsvStk" runat="server">
            <ItemTemplate>
                <tr>
                <td>
                                    <asp:CheckBox ID="CheckBox" runat="server"></asp:CheckBox>
                                </td>
                                <%--<td hidden="hidden">
                                    <asp:Label ID="lblID" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>--%>
                    <td hidden="hidden">
                                    <asp:Label ID="lblkode" runat="server" Text=' <%# Eval("id")%>'></asp:Label>
                </td>
                                <td >
                                    <asp:Label ID="lblName" runat="server" Text=' <%# Eval("name")%>'></asp:Label>
                </td>
                    </tr>
            </ItemTemplate>
         </asp:ListView>

    
     </tbody>
  </table>
</div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
          <button type="button" class="btn btn-primary" runat="server" id="RadButton1" onserverclick="RadButton1_ServerClick">Select</button>
          <%--<button id="btnAll1" type="button" runat="server" class="btn btn-primary" onserverclick="btnAll1_ServerClick" >SelectAll</button>--%>
        <%--<button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit_Click">Select</button>
          <button type="button" id="btnAll" runat="server" class="btn btn-primary" onserverclick="btnAll_Click">SelectAll</button>--%>
      </div>
              
    </div>
  </div>
                       </ContentTemplate>
</asp:UpdatePanel>
</div>

                <%--</ContentTemplate>
         </asp:UpdatePanel>--%>
    <!-- Modal Store -->

         <!-- Modal Edit -->
<%--<div id="editModal" class="modal hide fade" 

tabindex="-1" role="dialog" 

aria-labelledby="editModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="editModalLabel">Edit Record</h3>
                <button type="button" class="close" 

                data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <asp:UpdatePanel ID="upEdit" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <asp:HiddenField ID="HfUpdateitemID" 

                        runat="server" />
                        <table class="table">
                            <tr>
                                <td>Item Name : </td>
                                <td>
                                    <asp:TextBox ID="txtNameUpdate" 

                                    runat="server" Width="100%" />
                                    <asp:RequiredFieldValidator 

                                        ID="valNameUpdate" 

                                    ControlToValidate="txtNameUpdate" 

                                    EnableClientScript="False" Display="Dynamic"

                                        Text="<br />* Name is required" 

                                        Font-Bold="true" ForeColor="Red" 

                                        runat="server"

                                        ValidationGroup="ValidationGroupUpdate" />
                                </td>
                                
                                <td>
                                    <asp:Label ID="lbloldname" runat="server" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td>
                                      <asp:TextBox ID="TxtnumberUpdate" runat="server" Width="100%" />
                                </td>
                            </tr>
                                    <tr>
                                    <td>Item Group : </td>
                                    <td>
                                        <asp:dropdownlist ID="ddlItemgroupUpdate" runat="server" BorderWidth="0"></asp:dropdownlist>
                                    </td>
                                    <td>
                                </tr>
                            <tr>
                                <td>Major Group : </td>
                                <td>
                                    <asp:dropdownlist ID="ddlmjgroupUpdate" runat="server" BorderWidth="0"></asp:dropdownlist>
                                </td>
                                <td>
                            </tr>
                                  
                        </table>
                        
                    </div>
                    <div class="modal-footer">
                        <asp:Label ID="lblResult" 

                        Visible="false" runat="server"></asp:Label>
                        <asp:Button ID="btnSave" 

                        runat="server" Text="Update" 

                        CssClass="btn btn-info" 

                        ValidationGroup="ValidationGroupUpdate" 

                               OnClick="btnUpdate_Click" />
                        <button class="btn btn-info" 

                        data-dismiss="modal" 

                        aria-hidden="true">Close</button>

                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvItemMaster" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>--%>
<!-- /.modal Edit-->

<asp:Label ID="lblMessage" runat="server" />
                  <!-- Modal Add -->
<%--<div id="addModal" class="modal hide fade" 

tabindex="-1" role="dialog" 

aria-labelledby="addModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="addModalLabel">Add New Record</h3>
                <button type="button" class="close" 

                data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>Item Name : </td>
                                <td>
                                    <asp:TextBox ID="txtNameAdd" runat="server" Width="100%" />
                                    <asp:RequiredFieldValidator ID="valNameAdd" ControlToValidate="txtNameAdd" 
                                        EnableClientScript="False" Display="Dynamic" Text="<br />* Item Name is required" 
                                        Font-Bold="true" ForeColor="Red" runat="server" ValidationGroup="ValidationGroupAdd" />
                                 </td>
                                
                            </tr>
                            <tr>
                                <td>Major Group : </td>
                                <td>
                                    <asp:dropdownlist ID="ddlmjgroupAdd" runat="server" BorderWidth="0"></asp:dropdownlist>
                                </td>
                                <td>
                            </tr>
                                    <tr>
                                <td>Item Group : </td>
                                <td>
                                    <asp:dropdownlist ID="ddlItemgroupAdd" runat="server" BorderWidth="0"></asp:dropdownlist>
                                </td>
                                <td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                     <asp:Button ID="btnAddRecord" runat="server" Text="Add" CssClass="btn btn-info" ValidationGroup="ValidationGroupAdd" OnClick="btnAddRecord_Click" />
                         <button class="btn btn-info" 

                        data-dismiss="modal" 

                        aria-hidden="true">Close</button>

                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
        
    </div>
    
</div>--%>

<!-- /.Modal Add -->

         <!-- Modal Delete -->
<div id="deleteModal" class="modal hide fade" 

tabindex="-1" role="dialog" 

aria-labelledby="delModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h3 id="delModalLabel">Delete Record</h3>
                <button type="button" class="close" 

                data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        Are you sure you want to delete the record
                        <asp:Label ID="lblFirstNameDelete" 

                        runat="server"></asp:Label>
                        ?
                        <asp:HiddenField ID="HfDeleteID" 

                        runat="server" />
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnDelete" 

                        runat="server" Text="Delete" 

                        CssClass="btn btn-info" />
                        <%--OnClick="btnDelete_Click"--%> 
                        <button class="btn btn-info" 

                        data-dismiss="modal" 

                        aria-hidden="true">Cancel</button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnDelete" 

                    EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal Delete -->

     </div>
    
          <script>
       
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "80%";
                
            }

            function openModalStore() {
                $('#ModalStore').modal({ show: true });
            }
       
          </script>
                         <!-- jQuery and Bootstrap JS files -->
<script src="Content/js/jquery-3.3.1.min.js"></script>
<script src="Content/js/popper.min.js"></script>
<script src="Content/js/bootstrap.min.js"></script>

<!-- Datatables-->
<script src="Content/datatables.net/js/jquery.dataTables.min.js"></script>
<script src="Content/datatables.net-buttons-bs4/js/dataTables.bootstrap4.min.js"></script>


  <!-- pace -->
 <script type="text/javascript">
     $(document).ready(function () {
         $('[id*=gvItemMaster]').dataTable({
             "aLengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
             "iDisplayLength": 5,
             "order": [[3, "asc"]],
             stateSave: true,
             stateSaveCallback: function (settings, data) {
                 localStorage.setItem
                     ('DataTables_' + settings.sInstance, JSON.stringify(data));
             },
             stateLoadCallback: function (settings) {
                 return JSON.parse
                     (localStorage.getItem('DataTables_' + settings.sInstance));
             }
         });
     });

     //The scripts below to restore the page changed by command
     $('#detailModal').on('hidden.bs.modal', function () {
         location.reload();
     })

     $('#editModal').on('hidden.bs.modal', function () {
         location.reload();
     })

     $('#addModal').on('hidden.bs.modal', function () {
         location.reload();
     })
     $('#deleteModal').on('hidden.bs.modal', function () {
         location.reload();
     })
     $('#ModalStore').on('hidden.bs.modal', function () {
         location.reload();
     })
 </script>
</asp:Content>
