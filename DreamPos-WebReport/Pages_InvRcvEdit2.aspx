<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_InvRcvEdit2.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvRcvEdit2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
     <style>
         .input-group>.form-control{ width: 800px; }
         .btn-info {
    color: #fff;
     background-color:#fff; 
    border-color: #17a2b8;
    box-shadow: none;
}
     </style>
    <style type="text/css">
    /*.modal
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
    }*/
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
          //$(document).ready(function () {
          //    $('.btn').click(function () {
          //        if (Page_IsValid) {
          //            $('#spinner').show();
          //        }
          //    });
          //});
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
            <h1 class="m-0 text-dark">Order /Receive Detail</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Order /Receive Detail</li>
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
        <div class="form-horizontal" id="formload">
                   <div class="card-body">
                        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblorderno" Text="Receive No: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:left;" runat="server" ID="txtLinkid" Class="form-control" MaxLength="30" ReadOnly="true"/>
                       <%--<asp:label runat="server" ID="lblStatus" Text="" Style="padding:6px;" CssClass="align-middle"></asp:label>--%>                             
                  </div>  
            
            
        </div>
                         <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4" Text="DO: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:left;" runat="server" ID="txtDO" Class="form-control" MaxLength="30"/>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDO"
                    ErrorMessage="Fill Do Number" ForeColor="Red"></asp:RequiredFieldValidator>                           
                  </div>  
            
            
        </div>
                    <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="Supplier: "></asp:label>
           <div class="input-group col-md-3">
                     
                     <asp:DropDownList ID="DdlSup" OnSelectedIndexChanged="DdlSup_SelectedIndexChanged" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlSup"
                    ErrorMessage="Please Choice supplier" ForeColor="Red"></asp:RequiredFieldValidator>
                  </div>  
            
            
        </div>    
                   <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label1" Text="CC: "></asp:label>
           <div class="input-group col-md-3">
                     <asp:DropDownList ID="ddlFrom" OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlFrom"
                    ErrorMessage="Please Choice Cc" ForeColor="Red"></asp:RequiredFieldValidator>         
                  </div>  
            
            
        </div>         
             
        <div class="form-group row col-md-12" runat="server"  id="expdel">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="Exp Date: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <div class="input-group">
                     <%-- <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>--%>
                     <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID ="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                      <asp:TextBox style="text-align:left; width:auto;" runat="server" ID="txtExp" Class="form-control" AutoPostBack="false" />
                          <ajaxtoolkit:CalendarExtender ID="cleExp" runat="server" TargetControlID="txtExp" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>             
                  </div>  
            
            
        </div>         
        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblNotes" Text="Notes: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtNotes" Class="form-control" idth="400" TextMode="MultiLine" MaxLength="150" Height="100"/>
                                                
                  </div>  
            
            
        </div>
               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                        <button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshow" data-target="#ModalItem" style="font-weight: 600; border-color:gray; width:108px;">
                            Add Item
                        </button>
                         <button type="button" runat="server" class="btn btn-primary" id="btnSave" onserverclick="btnSave_Click" style="font-weight: 600; border-color:gray; width:108px;">
                            Order
                        </button>
                        <button type="button" runat="server" class="btn btn-primary" id="btnFinish" onserverclick="btnFinish_Click" style="font-weight: 600; border-color:gray; width:108px;">
                            Receive
                        </button>
                         <button type="button" runat="server" class="btn btn-primary" id="btnBack" causesvalidation="false" onserverclick="btnBack_ServerClick" style="font-weight: 600; border-color:gray; width:108px;">
                            Back
                        </button>
                       <%-- <button type="button" runat="server" class="btn btn-primary" id="btntest1" style="font-weight: 600; border-color:gray; width:108px;">
                            Add test
                        </button>--%>
                            <%--<table>
                                <tr>
                                    <td>
                                        <asp:Button type="button" runat="server" ID="btnShow2" Text="Add Item" CssClass="btn btn-primary" data-target="#ModalItem" data-toggle="modal" Style="font-weight: 600; border-color:gray; width:108px;" />
                                        
                                    </td>
                                    <td>
                                        
                                         <asp:Button type="button" runat="server" ID="btnFinish" Text="Save Finish" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                                    </td>
                                    <td>
                                         <asp:Button type="button" runat="server" ID="btnBack" Text="Back" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>--%>
                           
                        </div>
                   </div>       
             
     

    </div>

   </div>
        </div>
       

        <div class="card-body" style="overflow:scroll;">
            <div class="row">
                 <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always" style="width:100%;">
            <ContentTemplate>
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
             <asp:GridView ID="DgvTransfer" runat="server" Width="100%" AutoGenerateColumns="false" DataKeyNames="stk_id"
                         OnRowCommand="DgvTransfer_RowCommand" OnRowEditing="DgvTransfer_RowEditing" OnRowDeleting="DgvTransfer_RowDeleting"
                         CssClass="table table-striped"
                        HeaderStyle-BackColor="#4D4D4D" HeaderStyle-ForeColor="White" CellPadding="5">
                 <%--  OnPreRender="DgvTransfer_PreRender" OnRowEditing="DgvTransfer_RowEditing" OnRowDataBound="RowDataBound" OnRowDeleting="DgvTransfer_RowDeleting" OnRowUpdating="DgvTransfer_RowUpdating" OnRowCancelingEdit="DgvTransfer_RowCancelingEdit"--%>
                        <Columns>

                             <asp:TemplateField HeaderText="Action" HeaderStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("stk_id")%>' ImageUrl="~/Images/edit.png" ControlStyle-CssClass="btn btn-info" ButtonType="Button" />

                                    <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("stk_id")%>' ImageUrl="~/Images/Delete.png" ControlStyle-CssClass="btn btn-info" ButtonType="Button" />
                                    <%--OnClientClick="Confirm()"--%>
                                  <%--  <button runat="server" id="imgbtndetails" onserverclick="imgbtndetails_Click" class="btn btn-mini" commandname="details" title="Details">
                    <i class="fas fa-search"></i>
                    </button>--%>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" ImageUrl="~/Images/icon-update.png"
                                        Width="16px" Height="16px" />
                                    <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/icon-Cancel.png"
                                        Width="16px" Height="16px" />
                                </EditItemTemplate>--%>
                                <%--<FooterTemplate>
                           <asp:LinkButton ID="lbtnAdd" runat="server" CommandName="ADD" Text="Add" Width="80px"></asp:LinkButton> 
                        </FooterTemplate>--%>
                            </asp:TemplateField>

                              <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                            <asp:TextBox ID="txtAddID" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStkID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStkEditID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                   <%-- <asp:Label ID="lblName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>--%>
                                    <asp:LinkButton ID ="lnkstkname" runat ="server" CommandArgument='<%#Eval("stk_id")%>' CommandName ="detail" Text ='<%#Eval("stk_name") %>'></asp:LinkButton>  
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:Label ID="lblEditName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>
                                </EditItemTemplate>--%>
                                <%--<FooterTemplate>
                            <asp:TextBox ID="txtAddName" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit1" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("U1","{0:#,0}") %>'></asp:Label>--%>
                                    <asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("U1") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>--%>
                                    <%--<asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server" Text='<%# Eval("U1","{0:#,0}") %>'
                                        Width="40px" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>--%>
                                    <%--    <asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server" MaxLength="10" Text='<%# Eval("U1") %>'></asp:TextBox>
                                </EditItemTemplate>--%>
                                <%-- <FooterTemplate>
                            <asp:TextBox ID="txtAddUnit1" runat="server" Width="80px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Nm" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U1_nm") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditU1Nm" runat="server" Text='<%# Eval("U1_nm") %>'></asp:Label>
                                </EditItemTemplate>
                                <%-- <FooterTemplate>
                           
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit 2" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblUnit2" runat="server" Text='<%# Eval("U2","{0:#,0}") %>'></asp:Label>--%>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("U2") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:TextBox ID="txtEditUnit2" BackColor="Aqua" runat="server"  MaxLength="10" Text='<%# Eval("U2") %>'></asp:TextBox>
                                </EditItemTemplate>--%>
                                <%-- <FooterTemplate>
                            <asp:TextBox ID="txtAddUnit2" runat="server" Width="80px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Nm" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lblU2Nm" runat="server" Text='<%# Eval("U2_nm") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditU2Nm" runat="server" Text='<%# Eval("U2_nm") %>'></asp:Label>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                          
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Factor U1-U2" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblfactor" runat="server" Text='<%# Eval("factor1") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditfactor" runat="server" Text='<%# Eval("factor1") %>'
                                        Width="60px"></asp:Label>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                            <asp:TextBox ID="txtAddfactor" runat="server" Width="80px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TotalReceive (Unit2)" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblttlStock" runat="server" Text='<%# Eval("ttlStock") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblttlStock" runat="server" Text='<%# Eval("ttlStock") %>'
                                        Width="60px"></asp:Label>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                            <asp:TextBox ID="txtttlStock" runat="server" Width="80px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost/Unit1" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblU1Cost" runat="server" Text='<%# Eval("CostU1", "{0:#,0}") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:TextBox ID="txtEditU1Cost" BackColor="Aqua" runat="server"  MaxLength="10" Text='<%# Eval("CostU1", "{0:#,0}") %>'
                                        Width="60px" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
                                </EditItemTemplate>--%>
                          
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost/Unit2" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCost" runat="server" Text='<%# Eval("Cost", "{0:#,0}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txtEditCost" runat="server" Text='<%# Eval("Cost", "{0:#,0}") %>'
                                        Width="60px" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:Label>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                            <asp:TextBox ID="txtttlStock" runat="server" Width="80px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Cost" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblttlCost" runat="server" Text='<%# Eval("ttlCost", "{0:#,0}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblttlCost" runat="server" Text='<%# Eval("ttlCost", "{0:#,0}") %>'
                                        Width="60px"></asp:Label>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                            <asp:TextBox ID="txtttlStock" runat="server" Width="80px"></asp:TextBox>
                        </FooterTemplate>--%>
                            </asp:TemplateField>

                           

                        </Columns>
                    </asp:GridView>
        
        </div>
                </ContentTemplate>
                     </asp:UpdatePanel>
   <%-- <div class="table-responsive" style="margin:0 20px; width:100%;">
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
            </div>
        <!-- Modal Item -->
     <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalItem" tabindex="-1" role="dialog" aria-labelledby="ModalItem" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalStoreLabel">Select Item</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>

        <div class="row">
        <div class="form-horizontal" id="Div2" runat="server" style="width:100%;">
                   <div class="card-body">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="txtSearch" CssClass="col-sm-2 col-form-label text-nowrap" Text="Item: "></asp:Label>
                 
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
                    <asp:TextBox style="width:unset;" id="txtSearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                    

                    <button runat="server" id="btnSearch" onserverclick="btnSearch_Click" class="btn btn-mini" title="Search">
                    <i class="fas fa-search"></i>
                    </button>
                             <button runat="server" id="btnClear" onserverclick="btnClearSearch_Click" class="btn btn-mini" title="Clear Search">
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
        
      <th scope="col">Stock Name</th>
        <th scope="col">Batch Stock</th>
    </tr>
  </thead>
     <tbody>
         
      <asp:ListView ID="LsvStk" runat="server">
            <ItemTemplate>
                <tr>
                <td>
                                    <asp:CheckBox ID="CheckBox1" runat="server"></asp:CheckBox>
                                </td>
                                <td hidden="hidden">
                                    <asp:Label ID="lblID" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                    <td>
                                    <asp:Label ID="lblName" runat="server" Text=' <%# Eval("name")%>'></asp:Label>
                </td>
                                <td >
                                    <asp:Label ID="Label1" runat="server" Text=' <%# Eval("Batch")%>'></asp:Label>
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
    <!-- Modal Store -->

        <%--tambahan 22 nov2021--%> 
        <!-- Modal details -->
     <asp:UpdatePanel ID="updetails" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                 <!-- Modal -->
          <div class="modal fade" id="detailModal" tabindex="-1" role="dialog" aria-labelledby="detailModal" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="detailModalLabel">Details</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
                        <div class="modal-body">
                            <asp:DetailsView ID="DetailsView1" 
                                    runat="server" CssClass="table table-bordered table-hover"
                                        BackColor="White" ForeColor="Black"
                                        FieldHeaderStyle-Wrap="false"
                                        FieldHeaderStyle-Font-Bold="true"
                                        FieldHeaderStyle-BackColor="LavenderBlush"
                                        FieldHeaderStyle-ForeColor="Black" 
                                        BorderStyle="Groove" AutoGenerateRows="False">
                                        <Fields>
                                            <asp:BoundField DataField="stk_name" 
                                            HeaderText="Stock Name" />
                                            <asp:BoundField DataField="U1" 
                                            HeaderText="U1" />
                                            <asp:BoundField DataField="U1_nm" 
                                            HeaderText="U1_nm" />
                                            <asp:BoundField DataField="U2" 
                                            HeaderText="U2" />
                                            <asp:BoundField DataField="U2_nm" 
                                            HeaderText="U2_nm" />
                                            <asp:BoundField DataField="factor1" 
                                            HeaderText="factor1" />
                                            <asp:BoundField DataField="ttlStock" 
                                            HeaderText="ttlStock" />
                                            <asp:BoundField DataField="CostU1" 
                                            HeaderText="CostU1" />
                                            <asp:BoundField DataField="cost" 
                                            HeaderText="cost" />
                                             <asp:BoundField DataField="ttlCost" 
                                            HeaderText="ttlCost" />
                                        </Fields>
                                    </asp:DetailsView>
                            </div>
                       </div>
                    </div>
                </div>

                </ContentTemplate>
         <Triggers>
    <asp:AsyncPostBackTrigger ControlID="DgvTransfer"  EventName="RowCommand" />  
</Triggers>
         </asp:UpdatePanel>
    <!-- Modal details -->

         <!-- Modal edit -->
        <asp:UpdatePanel ID="UpdateEdit" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                 
              <div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModal" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="editModalLabel">Update</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
                        <div class="modal-body">
                                    <asp:HiddenField ID="HfUpdateID" 
                                    runat="server" />
                                    <table class="table">
                                        <tr>
                                            <td>Stock Name :</td>
                                            <td>
                                                <asp:label ID="lblstk_name" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
					<tr>
                                            <td>U1 :</td>
                                            <td>
                                                <asp:TextBox ID="txtU1" 
                                                runat="server" Width="100%"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>U1_nm :</td>
                                            <td>
                                                <asp:label ID="lblU1_nm" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
                                        <tr>
                                            <td>U2 :</td>
                                            <td>
                                                <asp:TextBox ID="txtU2" 
                                                runat="server" Width="100%"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>U2_nm :</td>
                                            <td>
                                                <asp:label ID="lblU2_nm" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
                                        <tr>
                                            <td>factor1 :</td>
                                            <td>
                                                <asp:label ID="lblfactor1" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
                                        <tr>
                                            <td>ttlStock :</td>
                                            <td>
                                                <asp:label ID="lblttlStock" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
                                        <tr>
                                            <td>CostU1 :</td>
                                            <td>
                                                <asp:TextBox ID="txtCostU1" 
                                                runat="server" Width="100%"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Cost :</td>
                                            <td>
                                                <asp:label ID="lblCost" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
                                        <tr>
                                            <td>ttlCost :</td>
                                            <td>
                                                <asp:label ID="lblttlCost" 
                                                runat="server" Width="100%"></asp:label></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="modal-footer">
                                    <%--<label id="lblResult" Visible="false" runat="server"></label>--%>
                                    <asp:Label ID="lblResult" 
                                    Visible="false" runat="server"></asp:Label>
                                    <asp:button id="btnSave2" type="button"
                                    runat="server" Text="Update"
                                    class="btn btn-primary" 
                                    ValidationGroup="ValidationGroupUpdate" onclick ="btnSave2_Click"/>
                                    <button class="btn btn-secondary" type="button"
                                    data-dismiss="modal">Close</button>

                                </div>
        </div>
      </div>
                  </div>
                </ContentTemplate>
         <Triggers>
    <asp:AsyncPostBackTrigger ControlID="DgvTransfer"  EventName="RowCommand" />  
             <asp:AsyncPostBackTrigger ControlID="btnSave2" EventName="click" />
</Triggers>
         </asp:UpdatePanel>
            <!-- /.modal edit-->

          <!-- Modal confirm -->
         <asp:UpdatePanel ID="upconfirm" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                 <!-- Modal -->
          <div class="modal fade" id="confirmModal" tabindex="-1" role="dialog" aria-labelledby="confirmModal" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="confirmModalLabel">Confirm</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
                        <div class="modal-body">
                            <h5>Do you want to Delete data?</h5>
                            </div>
        <div class="modal-footer">
                                   
                                    <asp:button id="btnok" type="button"
                                    runat="server" Text="Yes"
                                    class="btn btn-primary" 
                                    ValidationGroup="ValidationGroupUpdate" onclick ="btnok_Click"/>
                                    <button class="btn btn-secondary" type="button"
                                    data-dismiss="modal">No</button>

                                </div>
                       </div>
                    </div>
                </div>

                </ContentTemplate>
         <Triggers>
    <asp:AsyncPostBackTrigger ControlID="DgvTransfer"  EventName="RowCommand" />  
</Triggers>
         </asp:UpdatePanel>
            <!-- /.modal confirm-->

        <div style="font-family: Calibri;margin: 0 20px;">
    <asp:Label ID="lblError" runat="server" Text='' ForeColor = "Red" Font-Size ="Medium" ></asp:Label>
            <asp:Label ID="lblnoitem" runat="server" Text="0"  Visible = "false"></asp:Label>
    
    </div>

    </div>
     
    
           <script type="text/javascript">
              $('#detailModal').on('hidden.bs.modal', function () {
                  location.reload();
              })

              $('#editModal').on('hidden.bs.modal', function () {
                  
                  location.reload();
              })

      
            function OnKeyPress(e) {
                if (e.keyCode == "27") {
                    $find("ModalItem").hide();
                    $find("detailModal").hide();
                    $find("editModal").hide();
                    $find("confirmModal").hide();
                }
            }

            function openModalItem() {
                $('#ModalItem').modal({ show: true });
                //document.getElementById("txtsearch").focus();
            }

            function opendetailmodal() {
                $('#detailModal').modal({ show: true });
                //document.getElementById("txtsearch").focus();
            }
            function openeditmodal() {
                $('#editModal').modal({ show: true });
                //document.getElementById("txtsearch").focus();
            }
            function openconfirmmodal() {
                $('#confirmModal').modal({ show: true });
                //document.getElementById("txtsearch").focus();
            }

            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
            } else {
                document.getElementById("formload").style.width = "80%";

            }
    </script>
</asp:Content>
