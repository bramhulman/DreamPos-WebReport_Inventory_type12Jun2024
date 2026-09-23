<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteHome.Master" CodeBehind="Outlet.aspx.vb" Inherits="DreamPos_WebReport.Outlet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
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
    </script>

      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Outlet</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
              <li class="breadcrumb-item active">Outlet</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>

    <div class="row">
        <div class="form-horizontal" id="formload" runat="server">
                   <div class="card-body" style="width:400px;">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="txtsearch" CssClass="col-sm-2 col-form-label text-nowrap" Text="Outlet: "></asp:Label>
                    <div class="col-md-10">
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
<asp:TextBox id="txtsearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                

                    <button runat="server" id="btnsrc" onserverclick="btnsearch" class="btn btn-mini" title="Search">
    <i class="fas fa-search"></i>
</button>

                 
            
                  
        </div>
      </div>
                        </div>
                    </div>
                  </div>

                </div>
             
     

    </div>


     <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
        
    <div class="table-responsive">
        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%" class="menutab table table-bordered">
            <thead>
                <tr role="row">
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Kode</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Nama</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Pos Version</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Expire Date</b>
                    </th>
                   <%--<th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #414a4c">Action</b>
                    </th>--%>

                </tr>
            </thead>

            <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
                <ItemTemplate>

                    <td>
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("Kode")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("Nama")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("PosVer")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("eDate1")%>'></asp:Label>
                    </td>
                    <%--<td style="width: 8%; text-align: center">
                        <%--//==== Here we have used CommandName property to distinguish which button is 
                        clicked and we have passed our primary key to CommandArgument property. ====//--%>
                        <%--<asp:ImageButton ID="imgBtnEdit" CommandName="Edt" ToolTip="Edit a record."
                            CommandArgument='<%#Eval("Id") %>' runat="server" ImageUrl="/images/edit.png" />--%>
                        <%--<asp:ImageButton ToolTip="Delete a record." OnClientClick="Confirm()"
                            ID="imgBtnDelete" CommandName="Del" CommandArgument='<%#Eval("Id") %>' runat="server"
                            ImageUrl="~/images/delete.png"></asp:ImageButton>
                    </td>--%>




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
       

     </div>
      <script>
       
            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
                } else {
                document.getElementById("formload").style.width = "50%";
                
               }
      
       
    </script>
</asp:Content>
