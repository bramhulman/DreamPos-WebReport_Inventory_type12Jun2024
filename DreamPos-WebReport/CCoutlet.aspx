<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="CCoutlet.aspx.vb" Inherits="DreamPos_WebReport.CCoutlet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>     
    <style type="text/css">
    .modal
    {
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
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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
            <h1 class="m-0 text-dark">CC Supplier</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
              <li class="breadcrumb-item active">CC Supplier</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>

    <div class="row">
        <div class="form-horizontal" id="formload">
                   <div class="card-body">
                  
               <div class="form-group row">
            <asp:Label runat="server" AssociatedControlID="ddlCC" CssClass="col-sm-2 col-form-label" Text="CC: "></asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlCC" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled" CssClass="form-control">
                </asp:DropDownList>

            </div>
        </div>
        <div class="form-group row">
            <asp:Label runat="server" AssociatedControlID="btnshowmdlstore" CssClass="col-sm-2 col-form-label" Text="Access: "></asp:Label>
            <div class="col-md-10">

                <div class="input-group">
                      
                        <span class="input-group-btn">
                            <button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshowmdlstore" data-target="#ModalSupplier" style="padding:1px 4px; margin-left:10px;">

                                         Get Supplier
                                         </button>
                            
                            <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Style="font-weight: 500; margin-left:15px; border-color: gray;"
                                Text="Add" CssClass="btn btn-default" />
                            <asp:Label ID="lblNoOutlet" runat="server" Text="0"  Visible = "false"></asp:Label>
                        </span>
                        
                            </div>

                </div>
        </div>
 <%--<div class="form-group row">
            <asp:Label runat="server" AssociatedControlID="ddlActive" CssClass="col-sm-2 col-form-label" Text="Active: "></asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlActive" runat="server" AutoPostBack="true" onchange="Confirm2()" AppendDataBoundItems="True" EnableViewState="True" ViewStateMode="Enabled" CssClass="form-control">
                    <asp:ListItem>active</asp:ListItem>
                    <asp:ListItem>nonactive</asp:ListItem>
                </asp:DropDownList>

            </div>
        </div>--%>
<%--<div class="form-group row">
      <asp:Label runat="server" AssociatedControlID="txtsearch" CssClass="col-sm-2 col-form-label text-nowrap" Text="Outlet: "></asp:Label>
                    <div class="col-md-10">
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
<asp:TextBox id="txtsrc_outxuser" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                

                    <button runat="server" id="btnsrc" onserverclick="btnsrc_outxuser" class="btn btn-mini" title="Search">
    <i class="fas fa-search"></i>
</button>

                 
            
                  
        </div>
      </div>
                        </div>

    </div>--%>
                </div>
             
     

    </div>

   </div>

     <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
             <div>
        <%= GeName()%>

                 <div class="row float-sm-right" style="width:40%;">
      <asp:Label runat="server" AssociatedControlID="txtsearch" CssClass="col-sm-2 col-form-label text-nowrap" Text="Outlet: "></asp:Label>
                    <div class="col-md-10">
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
<asp:TextBox id="txtsrc_outxuser" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                

                    <button runat="server" id="Button1" onserverclick="btnsrc_suppXcc" class="btn btn-mini" title="Search">
    <i class="fas fa-search"></i>
</button>

                 
            
                  
        </div>
      </div>
                        </div>

    </div>

    </div>
             
    <div class="table-responsive">
        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%" class="menutab table table-bordered">
            <thead>
                <tr role="row">
                    
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Nama</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Alamat</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Telp</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Email</b>
                    </th>
                   <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Action</b>
                    </th>

                </tr>
            </thead>

            <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
                <ItemTemplate>

                    <%--<td>
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Kode")%>'></asp:Label>
                    </td>--%>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("Name")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("address")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("phone")%>'></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("email")%>'></asp:Label>
                    </td>
                    <td style="width: 8%; text-align: center">
                        <%--//==== Here we have used CommandName property to distinguish which button is 
                        clicked and we have passed our primary key to CommandArgument property. ====//--%>
                        <%--<asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                            CommandArgument='<%#Eval("Id") %>' runat="server" ImageUrl="/images/edit.png" />--%>
                        <asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                            ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("Id") %>' runat="server"
                            ImageUrl="~/images/delete.png"></asp:ImageButton>
                    </td>




                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </table>

        <br />
    </div>
    <div class="table-responsive">
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
       
           <!-- Modal Supplier -->
     <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalSupplier" tabindex="-1" role="dialog" aria-labelledby="ModalSupplier" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalSupplierLabel">Select Supplier</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>

        <div class="row">
        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                   <div class="card-body">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="txtSearch" CssClass="col-sm-2 col-form-label text-nowrap" Text="Outlet: "></asp:Label>
                 
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
                    <asp:TextBox id="txtSearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                

                    <button runat="server" id="btnSearch" class="btn btn-mini" title="Search">
                    <i class="fas fa-search"></i>
                    </button>
                                <button runat="server" id="btnClearSupplier" onserverclick="btnClearSupplier_ServerClick" class="btn btn-mini" title="Clear Search">
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
</div>

                </ContentTemplate>
         </asp:UpdatePanel>
    <!-- Modal Supplier -->
       
     </div>
    
          <script>
              function OnKeyPress(e) {
                  if (e.keyCode == "27") {
                      
                      $find("ModalSUpplier").hide();
                      $find("ModalStock").hide();

                  }
              }

              /*Update 21 Februari 2020*/
              function showPopup() {
                  var modalPopupBehavior = $find('programmaticModalPopupBehavior');
                  modalPopupBehavior.show();
              }
              function hidepopup() {
                  var modalPopupBehavior = $find('programmaticModalPopupBehavior');
                  modalPopupBehavior.hide();
              }
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "80%";
                
            }

              function openModalSupplier() {
                  $('#ModalSupplier').modal({ show: true });
            }
              function openModalStock() {
                  $('#ModalStock').modal({ show: true });
                  //document.getElementById("txtsearch").focus();
              }
          </script>
</asp:Content>
