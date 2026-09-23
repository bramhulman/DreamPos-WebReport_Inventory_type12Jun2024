<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ListWaste.aspx.vb" Inherits="DreamPos_WebReport.Pages_ListWaste" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    
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
       
    </script>

      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">
                <asp:Label ID="lblTitle" runat="server" Text="List Opname"></asp:Label>
            </h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">List Opname</li>
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
  
    <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>


<div class="row">

    <div class="form-horizontal" style="margin:0 20px; width:96%;">
        
                     <div class="table-responsive" style="margin:0 20px; width:97%;">
                  <asp:UpdatePanel ID="upCrudGrid" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:GridView ID="gvOpname" runat="server" Width="99%" 

        AutoGenerateColumns="false" DataKeyNames="invdate,ccid,stkid,custid,Stkno,stkname,ccname,opencost,openqty,cost,ttlqty,bclose,closeqty,closecost,varianceqty,factor1,unit1,unit2, createddate,trtype, type"
            HeaderStyle-BackColor="SteelBlue" HeaderStyle-ForeColor="White" 
            OnPreRender="gvOpname_PreRender"

            OnRowCommand="gvOpname_RowCommand"

            CssClass="table table-striped">
            <EmptyDataTemplate>
                <asp:Image ID="Image0" 

                ImageUrl="~/Images/yellow-warning.gif" 

                AlternateText="No Image" runat="server" 

                Visible="false" />No Data Found.
            </EmptyDataTemplate>
            <Columns>
                <asp:ButtonField CommandName="editRecord" ControlStyle-CssClass="btn btn-info" 
                    ButtonType="Button" Text="Edit" HeaderText="" ItemStyle-CssClass="text-center" ItemStyle-Width="4%">
     <ControlStyle CssClass="btn btn-info"></ControlStyle>
 </asp:ButtonField>
             <asp:BoundField DataField="invdate" SortExpression="invdate" DataFormatString="{0:dd MMM yyyy}" HeaderText="invdate" />
                <%--<asp:BoundField DataField="custid" SortExpression="custid" HeaderText="custid" />
                <asp:BoundField DataField="stkid" SortExpression="stkid" HeaderText="stkid" />--%>
                 <asp:BoundField DataField="ccname" SortExpression="ccname" HeaderText="cc name" />
                <%--<asp:BoundField DataField="createddate" SortExpression="createddate" HeaderText="createddate" />--%>
<asp:BoundField DataField="stkno" SortExpression="stkno" HeaderText="stkno" />
                <asp:BoundField DataField="stkname" SortExpression="stkname" HeaderText="Stock Name" />
                 <%-- <asp:BoundField DataField="trtype" SortExpression="trtype" HeaderText="trtype" />
                <asp:BoundField DataField="ccid" SortExpression="ccid" HeaderText="ccid" />--%>
                  <asp:BoundField DataField="type" SortExpression="type" HeaderText="type" />
            <%--    <asp:BoundField DataField="opencost" SortExpression="opencost" HeaderText="opencost" />--%>
              <%--  <asp:BoundField DataField="openqty" SortExpression="openqty" HeaderText="openqty" />--%>
                <asp:BoundField DataField="ttlqty" DataFormatString="{0:#,0}" SortExpression="ttlqty" HeaderText="ttlqty" />
                <asp:BoundField DataField="unit2" DataFormatString="{0:#,0}" SortExpression="unit2" HeaderText="unit2" />
                <asp:BoundField DataField="cost" DataFormatString="{0:#,0}" SortExpression="cost" HeaderText="cost" />
               <%-- <asp:BoundField DataField="bclose" SortExpression="bclose" HeaderText="bclose" />--%>
                <asp:BoundField DataField="closeqty" DataFormatString="{0:#,0}" SortExpression="closeqty" HeaderText="closeqty" />
                <asp:BoundField DataField="closecost" DataFormatString="{0:#,0}" SortExpression="closecost" HeaderText="closecost" />
               <asp:BoundField DataField="varianceqty" DataFormatString="{0:#,0}" SortExpression="varianceqty" HeaderText="varianceqty" />
                 <%--<asp:BoundField DataField="unit2" SortExpression="unit2" HeaderText="unit2" />--%>
                
              
              
                <%--<asp:TemplateField HeaderText="invdate">
                    <ItemTemplate>
                        <%# Eval("invdate") %>
                    </ItemTemplate>
                </asp:TemplateField> --%> 
                   </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
                 </div>
    </div>

             <!-- Modal Edit -->
    <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
                    <div class="modal fade" id="editModal" 

                    tabindex="-1" role="dialog" 

                    aria-labelledby="editModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-xl">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="editModalLabel">Edit Record</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                    <i class="fa fa-window-close" aria-hidden="true"></i>
                </button>
                                </div>
            
                                        <div class="modal-body">
                                            <asp:HiddenField ID="HfUpdatecc" runat="server" />
                                            <asp:HiddenField ID="HfUpdatestock" runat="server" />
                                            <asp:HiddenField ID="Hffactor1" runat="server" />
                                            <asp:HiddenField ID="Hfstkno" runat="server" />
                                            <asp:HiddenField ID="HfUpdatedate" runat="server" />
                                            <table class="table">
                                                 <tr>
                                                      <td>Cost Centre : </td>
                                                      <td>
                                                          <asp:TextBox ID="txtcostcentre" Enabled="false" runat="server" Width="100%" />
                                                      <td>
                                                  </tr>
                                                <tr>
                                                    <td>Stock : </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStockNameUpdate" 

                                                        runat="server" Enabled="false" Width="100%" />
                                                        <%--<asp:RequiredFieldValidator 

                                                            ID="valNameUpdate" 

                                                        ControlToValidate="txtNameUpdate" 

                                                        EnableClientScript="False" Display="Dynamic"

                                                            Text="<br />* Name is required" 

                                                            Font-Bold="true" ForeColor="Red" 

                                                            runat="server"

                                                            ValidationGroup="ValidationGroupUpdate" />--%>
                                                    </td>
                               
                                                </tr>
                                                <tr>
                                                    <td>cost : </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcost" Enabled="false" runat="server" Width="100%" />
                                                    <td>
                                                </tr>
                                                <tr>
                                                    <td>opening cost : </td>
                                                    <td>
                                                        <asp:TextBox ID="txtclosecost" runat="server" Width="100%" />
                                                    <td>
                                                </tr>
                                                    <tr>
                                                    <td>total qty : </td>
                                                    <td>
                                                        <asp:TextBox ID="txtttlqty" runat="server" Width="75%" Enabled="false" />
                                                        <asp:label ID="lblttlu2" runat="server" Width="20%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>unit1 qty : </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcloseqtyu1" runat="server" Width="75%" />
                                                        <asp:label ID="lblunit1" runat="server" Width="20%" />
                                                    </td>
                                                                
                                                </tr>
                                                <tr>
                                                <td>unit2 qty : </td>
                                                <td>
                                                    <asp:TextBox ID="txtcloseqtyu2" runat="server" Width="75%" />
                                                           <asp:label ID="lblunit2" runat="server" Width="20%" />
                                                </td>
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
                                        </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                    <!-- /.modal Edit-->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvOpname" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
     
         
    <%--<div class="table-responsive" style="margin:0 20px; width:100%;">
        <table class="table">
            <tr>
                <td>
                    <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                    <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="15">
                        <Fields>
                            <asp:NumericPagerField ButtonType="Link" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </div>--%>
        
         </div>
       
                             



<!-- 4. DataTables Buttons (Untuk Export, Opsional) -->
<script src="https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.7.1/js/buttons.print.min.js"></script>


    <script>

        //if ($(window).width() >= 960) {
        //    document.getElementById("formload").style.width = "100%";
        //} else {
        //    document.getElementById("formload").style.width = "80%";

        //}

        
    </script>

  <!-- pace -->
 <script type="text/javascript">
     $(document).ready(function () {
         $('#<%= gvOpname.ClientID %>').dataTable({
             "pageLength": 10,
             "lengthMenu": [5, 10, 25, 50, 100],
             "order": [[1, "asc"]],
             destroy: true,
             paging: true,
             searching: true,
             stateSave: false,      // jangan simpan state ke localStorage
             responsive: false,     // kalau tabel fixed layout
             autoWidth: false,      // hindari perhitungan otomatis lebar kolom
             deferRender: true,     // render baris hanya saat dibutuhkan (lebih cepat)
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
    
      
 </script>
</asp:Content>
