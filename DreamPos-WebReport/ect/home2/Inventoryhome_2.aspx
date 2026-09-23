<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Inventoryhome_2.aspx.vb" Inherits="DreamPos_WebReport.Inventoryhome_2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

</asp:Content>




<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    <style type="text/css">
        .style1 {
    color: #FFFFFF;
    background: #3a4f63;
}
        .bgtablelavender
        {
            background-color: #F08080;
        }
        .bgtablewhite
        {
            background-color: #fff;
        }
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
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>--%>
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
        //$(function () {
        //    if (document.getElementById('hfposted').value != '1') {
        //        if (confirm("Do you want to Acknowledge?")) {
        //            document.getElementById('hidField').value = "Yes";
        //        } else {
        //            document.getElementById('hidField').value = "No";
        //        }
        //        document.forms[0].submit();
        //    }
        //});
          //function Confirm() {
          //    var confirm_value = document.createElement("INPUT");
          //    confirm_value.type = "hidden";
          //    confirm_value.name = "confirm_value";
          //    if (confirm("Please run Opening Stock Process?")) {
          //        confirm_value.value = "Yes";
          //    } else {
          //        confirm_value.value = "No";
          //    }
          //    document.forms[0].appendChild(confirm_value);
          //}
    </script>
    <script type="text/javascript">
    function DisableButton() {
        document.getElementById("<%=RadButton3.ClientID %>").disabled = true;
    }
    window.onbeforeunload = DisableButton;
</script>
    <%-- <asp:HiddenField ID="hidField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfposted" Value="0" runat="server" />--%>
    <%--<asp:Button ID="btnDummy" runat="server" Text="Edit" Style="display: none;" />--%>
    <button type="button" runat="server" data-toggle="modal" id="btnshowmdlprocess" data-target="#ModalProcess" style="display: none;"></button>
    <section class="content-header" style="padding:5px.5rem;">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-4">
            <h1>
                <asp:Label ID="lblTitle" runat="server" Text="Inventory Stock"></asp:Label></h1>
          </div>
             <div class="col-sm-4">
            <h3>
                <%--<asp:Label ID="alert1" runat="server" Text="Need to Calculate Usage" ForeColor="Red" Visible="False"></asp:Label>--%>
                <asp:TextBox id="alert1" runat="server" CssClass="form-control" style="Z-INDEX: 101; LEFT: 36px; OVERFLOW: hidden; POSITION: absolute; color:red; " TextMode="MultiLine" width="100%" ReadOnly="True" Font-Bold="True" BackColor="#CCCCCC" Font-Size="Large"></asp:TextBox>
            </h3>

          </div>
          <div class="col-sm-4">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="/inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Inventory Stock</li>
            </ol>
          </div>
        </div>
      </div><!-- /.container-fluid -->
    </section>
    <div class="card">
        <div class="card-header" style="padding: .5rem 1.25rem;">
              <div class="row">
          <div class="col-lg-3 col-md-3">
             <div class="form-horizontal" id="formload1">
                   <div class="card-body" style="padding: .5rem 1.25rem;">
                  <div class="form-group row" style="margin-bottom: 0.1rem;">
                    <asp:Label runat="server" AssociatedControlID="ddlOutlet" CssClass="col-sm-4 col-form-label" Text="Cost Centre: " AutoPostBack="True"></asp:Label>
                    <div class="col-sm-6">
                        <div class="input-group">
                      
                        <asp:DropDownList ID="ddlOutlet" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged"
                    EnableViewState="True" ViewStateMode="Enabled">
                            <asp:ListItem Value="0">----Select----</asp:ListItem>
                        </asp:DropDownList>
                        
                            </div>
                    </div>
                  </div>
                  
                </div>
                <!-- /.card-body -->
              </div>
           
          </div>
<div class="col-lg-3 col-md-3">
             <div class="form-horizontal" id="formload2">
                   <div class="card-body" style="padding: .5rem 1.25rem;">
                  <div class="form-group row" style="margin-bottom: 0.1rem;">
                    <asp:Label runat="server" AssociatedControlID="ddlGrp" CssClass="col-sm-4 col-form-label" Text="Stock Group: "></asp:Label>
                    <div class="col-sm-6">
                        <div class="input-group">
                      
                        <asp:DropDownList ID="ddlGrp" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="ddlGrp_SelectedIndexChanged"
                    EnableViewState="True" ViewStateMode="Enabled">
                             <asp:ListItem Value="0">----Select----</asp:ListItem>
                        </asp:DropDownList>
                        
                            </div>
                    </div>
                  </div>
                  
                </div>
                <!-- /.card-body -->
              </div>
           
          </div>
                  <div class="col-lg-3 col-md-3">
             <div class="form-horizontal" id="formload3">
                   <div class="card-body" style="padding: .5rem 1.25rem;">
                  <div class="form-group row" style="margin-bottom: 0.1rem;">
                    <asp:Label runat="server" AssociatedControlID="ddlStatus" CssClass="col-sm-3 col-form-label" Text="Status: "></asp:Label>
                    <div class="col-sm-6">
                        <div class="input-group">
                      <asp:DropDownList ID="DdlStatus" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True"
                        EnableViewState="True" ViewStateMode="Enabled">
                        <asp:ListItem Value="0">----Select----</asp:ListItem>
                        <asp:ListItem Value="1">No Warning</asp:ListItem>
                        <asp:ListItem Value="2">Warning</asp:ListItem>
                    </asp:DropDownList>	
                        
                            </div>
                    </div>
                  </div>
                  
                </div>
                <!-- /.card-body -->
              </div>
           
          </div>
                    <div class="col-lg-3 col-md-3">
             <div class="form-horizontal" id="formload4">
                   <div class="card-body" style="padding: .5rem 1.25rem;">
                  <div class="form-group row" style="margin-bottom: 0.1rem;">
                    <asp:Label runat="server" AssociatedControlID="txtsearch" CssClass="col-sm-3 col-form-label" Text="Name: "></asp:Label>
                    <div class="col-sm-8">
                        <div class="input-group">
                      <asp:TextBox id="txtsearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>	
                        <span class="input-group-btn input-space">
                    <button runat="server" title="search" id="btnsrc" onserverclick="btnsearch" class="btn btn-default" type="button"><i class="fas fa-search"></i></button>
               </span>

                            </div>
                    </div>
                  </div>
                  
                </div>
                <!-- /.card-body -->
              </div>
           
          </div>
          <!-- ./col -->
          

                 </div>
          <%--<h3 class="card-title">Report Menu</h3>--%>
<%--<asp:Label runat="server" ID="secondtitle" Text="Inventory Stock" Font-Size="Large" Font-Bold="True"></asp:Label>--%>
          <%--<div class="card-tools">
            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
              <i class="fas fa-times"></i></button>
          </div>--%>
        </div>
        <div class="card-body" style="overflow:scroll;">
         <div class="row">
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
        
    <div class="table-responsive">
        <table id="tblEmpDetails" align="left" style="border-style: solid;" width="100%" class="menutab table table-bordered">
            <thead class="style1">
                <tr role="row">
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #FFFFFF;">Cost Centre</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #FFFFFF;">Item</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #FFFFFF;">Total Stock</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #FFFFFF;">Cost</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #FFFFFF;">Total</b>
                    </th>
                    <th class="sorting_asc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending">
                        <b style="color: #FFFFFF;">Stock Limit</b>
                    </th>
                   

                </tr>
            </thead>

            <asp:ListView ID="lstViewDetails" OnItemCommand="lstView_ItemCommand" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
                <ItemTemplate>
                    <tr id="Tr1" runat="server" class='<%# GetBackground(Eval("stat")) %>'>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("ccname")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("stkname")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("ttlqty", "{0:0,0.0}") + " " + Eval("Unit2").ToString  %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("Cost", "{0:0,0.00}")%>'></asp:Label>
                    </td>
                          <td>
                        <%--<asp:Label ID="Label1" runat="server" Text=' <%# Eval("Cost", "{0:#,0}") * Eval("ttlqty", "{0:#,0}")%>'></asp:Label>--%>
                              <asp:Label ID="Label5" runat="server" Text=' <%# Eval("Total", "{0:0,0.00}")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text=' <%# Eval("stklimit", "{0:#,0}")%>'></asp:Label>
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
                    <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewDetails" PageSize="50">
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
        </div>

       
        

        <!-- /.card-body -->
        <div class="card-footer">
          <%--Report Menu--%>
        </div>
        <!-- /.card-footer-->
      </div>

      

    
     <!-- Modal proses -->
     <asp:UpdatePanel ID="UpdatePanel2" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalProcess" tabindex="-1" role="dialog" aria-labelledby="ModalProcess" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalProcessLabel"><asp:label runat="server" id="openlabel" Text="There is no Opening Stock !!!"></asp:label> </h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>

      </div>

        <div class="row">
        <div class="form-horizontal" id="formload" runat="server" style="width:100%;">
                   <div class="card-body">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label text-nowrap" Text="Do you want to Calculate now?"></asp:Label>
                 
                     
                   
                    </div>
                  </div>

                </div>
             
     

    </div>

      <div class="modal-body">
 
      </div>
      <div class="modal-footer" style="align-items:center; align-content:center">
        <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">NO</button>--%>
       <%-- <button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit2_Click">Select</button>
          <button type="button" id="btnAll2" runat="server" class="btn btn-primary" onserverclick="btnAll2_Click">SelectAll</button>--%>
          <button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit2_Click" id="RadButton3">YES</button>
          <%--<button type="button" class="btn btn-primary" runat="server" onserverclick="btnAll3_ServerClick">SelectAll</button>--%>
      </div>
    </div>
  </div>
</div>

                </ContentTemplate>
         </asp:UpdatePanel>
    <!-- Modal proses -->
     <script>
        function OnKeyPress(e) {
            if (e.keyCode == "27") {
                $find("ModalProcess").hide();
                   
                    
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
        /*Update 21 Februari 2020*/

        function openModalProcess() {
            $('#ModalProcess').modal({ show: true });
            //document.getElementById("txtsearch2").focus();
        }
        
        </script>
</asp:Content>

