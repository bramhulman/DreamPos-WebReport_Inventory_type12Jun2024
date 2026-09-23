<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_ReceipeEdit.aspx.vb" Inherits="DreamPos_WebReport.Pages_ReceipeEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
      <style>.input-group>.form-control{ width: 400px }
          .form-row {
              margin-bottom: 1rem;
          }
      </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <%-- DataTables + Buttons CDN --%>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap4.min.css"/>
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.bootstrap4.min.css"/>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/dataTables.bootstrap4.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.bootstrap4.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
      <script type="text/javascript">
          function ShowProgress() {
              setTimeout(function () {
                  var modal = $('<div />');
                  modal.addClass("modal");
                  $('body').append(modal);
                  var loading = $(".loading");
                  loading.show();
                  var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                  var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                  loading.css({ top: top, left: left });
              }, 200);
          }
          $('form').live("submit", function () {
              ShowProgress();
          });
          $(document).ready(function () {
              $('.btn').click(function () {
                  if (Page_IsValid) {
                      $('#spinner').show();
                  }
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
            <h1 class="m-0 text-dark">Receipe Edit</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Receipe Edit</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;">
       
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>
        <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="" src="/images/loader.gif" />
        </div>
    </div>
         <div class="row">
               <div class="col-lg-6 col-md-6">
        <div class="form-horizontal" id="formload">
                   <div class="card-body">
                        <div class="form-row">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="lblcode" Text="Code: "></asp:label>
           <div class="input-group col-sm-5">
                     
                      <asp:TextBox style="text-align:left;" runat="server" ID="txtNo" AutoPostBack="true" OnTextChanged="txtNo_TextChanged" Class="form-control" MaxLength="10"/>
                           <asp:label runat="server" ID="errNo" Visible="false" Style="color:red;"></asp:label>                         
                  </div>  
            
            
        </div>
                    <div class="form-row">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label2" Text="Name: "></asp:label>
           <div class="input-group col-md-5">
                     
                      <asp:TextBox style="text-align:left;" runat="server" ID="txtName" Class="form-control" MaxLength="15"/>
                                   
                  </div>  
            
            
        </div>    

                      <%-- <div class="form-row">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label6" Text="Type: "></asp:label>
           <div class="input-group col-md-5">
                     
                     <asp:DropDownList ID="DdlType" style="padding: 0.375rem 0.75rem;" runat="server" AppendDataBoundItems="True" AutoPostBack="True" Class="form-control"
                    EnableViewState="True" ViewStateMode="Enabled">
                    <asp:ListItem Value="0">--Select Type--</asp:ListItem>
                    <asp:ListItem Value="1">Combo</asp:ListItem>
                    <asp:ListItem Value="2">Stock Usage</asp:ListItem>
                 
                </asp:DropDownList>                                
                  </div>  
            
            
        </div>  --%>

                   <div class="form-row" runat="server" id="cost1" Style="display:none" >
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label1" Text="Cost: "></asp:label>
           <div class="input-group col-md-5">
                     
                      <asp:TextBox style="text-align:right; width:800pt;padding: 0.375rem 0.75rem" runat="server" ID="txtCost" Class="form-control" Enabled="false"/>
                                   
                  </div>  
            
            
        </div>         
             
        <div class="form-row">
                   <asp:label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" ID="Label3" Text="Remarks: "></asp:label>
           <div class="input-group col-md-5">
                     
                      <asp:TextBox style="text-align:left;" runat="server" ID="txtRemarks" Class="form-control"  TextMode="MultiLine" MaxLength="50"/>
                                   
                  </div>  
            
            
        </div>         

               
             
     

    </div>

   </div>
        </div>
       
         <div class="col-lg-6 col-md-6">
         <div class="form-horizontal" style="margin:0 20px; width:100%;overflow:scroll;">
          <asp:ListView ID="lstViewCost" runat="server" OnItemCommand="lstViewCost_ItemCommand"
                OnPagePropertiesChanging="OnPagecostChanging">
              <LayoutTemplate>
    <div class="table-responsive">
        <table id="tblCostDetails" align="left" style="border-style: solid; width:100%; margin-bottom:0;" class="menutab table table-bordered">
            <thead>
                <tr role="row">
                   <%-- <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Item No</b>
                    </th>--%>
                    <%--<th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Item Name</b>
                    </th>--%>
                                        <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Cost Centre</b>
                    </th>
                  
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Cost</b>
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

                   
                   <%-- <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("Name")%>'></asp:Label>
                    </td>--%>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                    </td>
                 
                    <td>
                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("Cost2", "{0:0,0.00}")%>'></asp:Label>
                    </td>
         
                    <td style="width: 8%; text-align: center">
                        
                        <%--<asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                            CommandArgument='<%#Eval("ccid").ToString%>' runat="server" ImageUrl="/images/edit.png" />--%>
                        <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                            ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("ccID").ToString + ";" + Eval("ccname").ToString %>' runat="server"
                            ImageUrl="~/images/delete.png"></asp:ImageButton>
                    </td>




                    </tr>
                </ItemTemplate>
            </asp:ListView>
        <%--</table>

        <br />--%>
    </div>
    <div class="table-responsive" style="margin:0 20px; width:100%;">
        <table class="table" style="margin-bottom: 0;">
            <tr>
                <td>
                    <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                    <asp:DataPager ID="lstViewCostPager1" runat="server" PagedControlID="lstViewCost" PageSize="5">
                        <Fields>
                            <asp:NumericPagerField ButtonType="Link" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </div>
        
         </div>
         </div>
  <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10" style="margin: 0 20px;">
                            <table>
                                <tr>
                                    <td>
                                        <button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnaddcc" data-target="#ModalItem" style="font-weight: 600; border-color:gray; width:108px;">
                                        Add CC
                                    </button>
                                    </td>
                                    <td>
                                        
                                         <button type="button" runat="server" id="btnInsert" onserverclick="btnInsert_Click"  class="btn btn-primary" style="font-weight: 600; border-color:gray; width:108px;">Add</button>
                                    </td>
                                    <td>
                                        
                                         <button type="button" runat="server" id="btnUpdate" onserverclick="btnUpdate_Click"  class="btn btn-primary" style="font-weight: 600; border-color:gray; width:108px;" Visible="false">Update</button>
                                    </td>
                                    <td>
                                         <button type="button" runat="server" id="btnCancel" onserverclick="btnCancel_Click"  class="btn btn-primary" style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" >Cancel</button>
                                    </td>
                                </tr>
                            </table>
                           
                        </div>
                   </div>       

        <hr style="width:100%;border: 1px solid #3a4f63;margin-top: 0rem;margin-bottom: 0rem;"/> 
     <div class="content-header"  style="padding:0;">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Receipe Detail</h1>
          </div><!-- /.col -->
         
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
   <div class="row">
          <div class="form-inline" id="Div1" runat="server" style="margin:0 20px; width:100%;background: #3a4f63;padding:0.2rem 0.8rem">
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
                        <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" class="btn btn-mini" title="Search" style="background:#FFFFFF;color:#3a4f63;">
                            <i class="fas fa-search"></i>
                        </button>
                        </div>
                    <div class="btn-group" role="group" style="margin:0.5rem">
                        <button type="button" runat="server" id="btnnew" onserverclick="btnnew_ServerClick" class="btn btn-primary" title="new" style="background:#FFFFFF;color:#3a4f63;">
                           <asp:Label runat="server">New</asp:Label>
                        </button>
                        </div>
                        <%-- Tombol Export Excel dari DataTables akan di-inject di sini --%>
                        <div id="dtExportContainer" style="margin:0.5rem; display:inline-block;"></div>
                      </div>
                  
           

                  

 


                    
                     
                  

                </div>
        </div>
        <div class="card-body" style="overflow:scroll;">
            <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
          <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
              <LayoutTemplate>
    <div class="table-responsive">
        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%" class="menutab table table-bordered">
            <thead>
                <tr role="row">
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Tipe</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Name</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Qty</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Unit</b>
                    </th>
                     <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Loss factor</b>
                    </th>
                    <th runat="server" id="thCostUnit3" visible="false" class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Cost/Unit3</b>
                    </th>
                    <th runat="server" id="thTotalCost" visible="false" class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Total Cost</b>
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
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("TypeName")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("dtlName")%>'></asp:Label>
                    </td>
                
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("Qty")%>'></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("unit")%>'></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="Label5" runat="server" Text=' <%# Eval("Lossf")%>'></asp:Label>
                    </td>
                    <td runat="server" visible='<%# modulepriv() = 0 %>' style="text-align:right">
                        <asp:Label ID="lblCostUnit3" runat="server" Text='<%# Eval("cost_per_unit3", "{0:0,0.00}") %>'></asp:Label>
                    </td>
                    <td runat="server" visible='<%# modulepriv() = 0 %>' style="text-align:right">
                        <asp:Label ID="lblTotalCost" runat="server" Text='<%# Eval("total_cost", "{0:0,0.00}") %>'></asp:Label>
                    </td>
                    <td style="width: 8%; text-align: center">
                        <%--//==== Here we have used CommandName property to distinguish which button is 
                        clicked and we have passed our primary key to CommandArgument property. ====//--%>
                        <asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                            CommandArgument='<%#Eval("id").ToString + ";" + Eval("tipe").ToString + ";" + Eval("unit").ToString%>' runat="server" ImageUrl="/images/edit.png" />
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
    <div class="table-responsive" style="margin:0 20px; width:100%;">
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
    </div>
        
         </div>
            </div>

        <!-- Modal CC -->
     <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalStoreLabel">Select CC</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>

        <div class="row">
        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                   <div class="card-body">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="txtSearchcc" CssClass="col-sm-2 col-form-label text-nowrap" Text="Cost Centre: "></asp:Label>
                 
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
                    <asp:TextBox id="txtSearchcc" style="width:auto;" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                    

                    <button runat="server" id="btnSearchcc" onserverclick="Searchcc_Click" class="btn btn-mini" title="Search">
                    <i class="fas fa-search"></i>
                    </button>
                             <button runat="server" id="btnClearSearchcc" onserverclick="btnClearSearch_Click" class="btn btn-mini" title="Clear Search">
                    <i class="fas fa-times"></i>
                    </button>
        
                  
        </div>
      </div>
                   
                    </div>
                  </div>

                </div>
             
     

    </div>

      <div class="modal-body">
       <div class="table-responsive">
  <table class="table">
   <thead>
    <tr>
      <th>check</th>
      <th scope="col" hidden="hidden">ID</th>
        
      <th scope="col">Cost Centre</th>
        <%--<th scope="col">CC Stock</th>--%>
    </tr>
  </thead>
     <tbody>
         
      <asp:ListView ID="LsvStk" runat="server">
            <ItemTemplate>
                <tr>
                <td>
                                    <asp:CheckBox ID="CheckBox" runat="server"></asp:CheckBox>
                                </td>
                                <td hidden="hidden">
                                    <asp:Label ID="lblID" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                    <td>
                                    <asp:Label ID="lblName" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                </td>
                                <%--<td >
                                    <asp:Label ID="Label1" runat="server" Text=' <%# Eval("stock")%>'></asp:Label>
                </td>--%>
                    </tr>
            </ItemTemplate>
         </asp:ListView>

    
     </tbody>
  </table>
</div>
      </div>

      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
          <button type="button" class="btn btn-primary" runat="server" onserverclick="RadButton1_ServerClick" id="RadButton1">Add</button>
          <%--<button id="btnAll1" type="button" runat="server" class="btn btn-primary" onserverclick="btnAll1_ServerClick">SelectAll</button>--%>
        <%--<button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit_Click">Select</button>
          <button type="button" id="btnAll" runat="server" class="btn btn-primary" onserverclick="btnAll_Click">SelectAll</button>--%>
      </div>
    </div>
  </div>
</div>

                </ContentTemplate>
         </asp:UpdatePanel>
        <%--modalcc--%>

    </div>
     
    
          <script>
              function OnKeyPress(e) {
                  if (e.keyCode == "27") {
                      $find("ModalItem").hide();
                  }
              }

              function openModalcc() {
                  $('#ModalItem').modal({ show: true });
              }

            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
            } else {
                document.getElementById("formload").style.width = "80%";
            }

            // =============================================
            // DataTables + Export Excel untuk tblEmpDetails
            // =============================================
            $(document).ready(function () {

                // Hitung jumlah kolom yang visible (kolom Action selalu kolom terakhir)
                // Kolom: Tipe, Name, Qty, Unit, Loss Factor, [Cost/Unit3], [Total Cost], Action
                // Kita exclude kolom Action (index terakhir) dari export

                if ($('#tblEmpDetails').length > 0) {

                    // Deteksi total kolom dari header
                    var totalCols = $('#tblEmpDetails thead tr th').length;
                    // Index kolom Action = kolom terakhir (0-based)
                    var actionColIdx = totalCols - 1;

                    var table = $('#tblEmpDetails').DataTable({
                        paging: false,          // Paginasi sudah ditangani ASP.NET DataPager
                        searching: false,       // Search sudah ditangani server-side
                        ordering: false,        // Tidak perlu sort ulang
                        info: false,            // Sembunyikan info "Showing 1 to N"
                        bAutoWidth: false,
                        dom: 'Bt',              // B = Buttons, t = table only
                        buttons: [
                            {
                                extend: 'excelHtml5',
                                text: '<i class="fas fa-file-excel" style="margin-right:5px;"></i> Export Excel',
                                className: 'btn btn-success btn-sm',
                                title: 'Receipe Detail',
                                filename: 'ReceipeDetail_' + new Date().toISOString().slice(0,10),
                                exportOptions: {
                                    columns: function (idx, data, node) {
                                        // Export semua kolom KECUALI kolom Action (index terakhir)
                                        return idx !== actionColIdx;
                                    }
                                },
                                customize: function (xlsx) {
                                    // Styling header kolom di Excel
                                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                    $('row:first c', sheet).attr('s', '2'); // bold header
                                }
                            }
                        ]
                    });

                    // Pindahkan tombol DataTables ke area toolbar (sebelah tombol New)
                    table.buttons().container().appendTo('#dtExportContainer');
                }
            });
    </script>
</asp:Content>
