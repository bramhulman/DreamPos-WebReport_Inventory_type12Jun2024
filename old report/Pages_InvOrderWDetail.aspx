<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="Pages_InvOrderWDetail.aspx.vb" Inherits="DreamPos_WebReport.Pages_InvOrderWDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
    <style>.input-group>.form-control{ width: 800px; }</style>
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
    <script type="text/javascript">
    function DisableButton() {
        document.getElementById("<%=btnFinish.ClientID %>").disabled = true;
    }
    window.onbeforeunload = DisableButton;
</script>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Order Stock To Warehouse</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Order Stock To Warehouse</li>
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
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblorderno" Text="Order No: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox runat="server" ID="txtID" Class="form-control" MaxLength="30" enabled="false" Style="width:800px; padding: 0.375rem 0.75rem;"/>
                       <%--<asp:label runat="server" ID="lblStatus" Text="" Style="padding:6px;" CssClass="align-middle"></asp:label>--%>                             
                  </div>  
            
            
        </div>
                    <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label2" Text="warehouse/Ho: "></asp:label>
           <div class="input-group col-md-3">
                     
                     <asp:DropDownList ID="ddlWare" OnSelectedIndexChanged="ddlWare_SelectedIndexChanged" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="true" EnableViewState="True" ViewStateMode="Enabled">
                         <asp:ListItem Value="0">----Select----</asp:ListItem>
                     </asp:DropDownList>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlWare"
                    ErrorMessage="Please Choice warehouse" ForeColor="Red"></asp:RequiredFieldValidator>
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
              <div class="form-group row col-md-12" runat="server" id="Createdate" visible="false">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label4" Text="Create Date: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID ="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                      <asp:TextBox style="text-align:center; width:auto" runat="server" ID="txtcreatedate" Class="form-control" AutoPostBack="false" />
                          <ajaxtoolkit:CalendarExtender ID="clecreatedate" runat="server" TargetControlID="txtcreatedate" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>             
                  </div>  
            
            
        </div>    
        <div class="form-group row col-md-12" runat="server" id="Deliverydate" visible="false">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label3" Text="Delivery Date: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID ="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                      <asp:TextBox style="text-align:center; width:auto" runat="server" ID="txtexpD" Class="form-control" AutoPostBack="false" />
                          <ajaxtoolkit:CalendarExtender ID="cle" runat="server" TargetControlID="txtexpD" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>             
                  </div>  
            
            
        </div>   
                       
                        <div class="form-group row col-md-12" runat="server" id="receivedate" visible="false">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="Label5" Text="Receive Date: "></asp:label>
           <div class="input-group col-md-3">
                     
                       <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID ="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                      <asp:TextBox style="text-align:center; width:auto" runat="server" ID="txtrcvdate" Class="form-control" AutoPostBack="false" />
                          <ajaxtoolkit:CalendarExtender ID="clereceivedate" runat="server" TargetControlID="txtrcvdate" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>             
                  </div>  
            
            
        </div>          
                       
        <div class="form-group row col-md-12">
                   <asp:label runat="server" CssClass="col-sm-1 col-form-label text-nowrap" ID="lblNotes" Text="Notes: "></asp:label>
           <div class="input-group col-md-3">
                     
                      <asp:TextBox style="text-align:left;" runat="server" ID="txtNotes" Class="form-control" idth="400" TextMode="MultiLine" MaxLength="95" Height="100"/>
                                                
                  </div>  
            
            
        </div>
               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                        <button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshow" data-target="#ModalItem" style="font-weight: 600; border-color:gray; width:108px;">
                            Add Item
                        </button>
                          <button type="button" runat="server" class="btn btn-primary" id="btnSetAll" onserverclick="btnSetAll_Click" style="font-weight: 600; border-color:gray; width:108px;">
                            stk Limit
                        </button>
                         <button type="button" runat="server" class="btn btn-primary" id="btnFinish" onserverclick="btnFinish_Click" style="font-weight: 600; border-color:gray; width:108px;">
                            Save Finish
                        </button>
                         <button type="button" runat="server" class="btn btn-primary" id="btnBack" onserverclick="btnBack_Click" style="font-weight: 600; border-color:gray; width:108px;">
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
         <div class="form-horizontal" style="margin:0 20px; width:100%;">
             <asp:GridView ID="DgvTransfer" runat="server" Width="100%" AutoGenerateColumns="false"
                        OnRowDataBound="RowDataBound" OnRowDeleting="DgvTransfer_RowDeleting" OnRowUpdating="DgvTransfer_RowUpdating"
                        OnRowCancelingEdit="DgvTransfer_RowCancelingEdit" OnRowEditing="DgvTransfer_RowEditing"
                        HeaderStyle-BackColor="#4D4D4D" HeaderStyle-ForeColor="White" CellPadding="5">
                        <Columns>


                              <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("wID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblIEditD" runat="server" Text='<%# Eval("wID") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStkID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStkEditID" runat="server" Text='<%# Eval("stk_id") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Number" HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStkno" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStkEditno" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditName" runat="server" Text='<%# Eval("stk_name") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Stock" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stockcc", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditStock" runat="server" Text='<%# Eval("Stockcc", "{0:0.00}") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Stock Wh" HeaderStyle-Width="30px" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStockwh" runat="server" Text='<%# Eval("stockwh", "{0:0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditStockwh" runat="server" Text='<%# Eval("stockwh", "{0:0.00}") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Limit" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:Label ID="lbllimit" runat="server" Text='<%# Eval("stklimit", "{0:0.##}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditlimit" runat="server" Text='<%# Eval("stklimit", "{0:0.##}") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Order" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <%--Bram 20Jun22 diganti hanya u2--%>
                                    <%--<asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("U1", "{0:0.##}") %>'></asp:Label>--%>
                                    <asp:Label ID="lblUnit1" runat="server" Text='<%# Eval("totalU2", "{0:0.##}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--Bram 20Jun22 diganti hanya u2--%>
                                  <%--  <asp:TextBox ID="txtEditUnit1" BackColor="Aqua" runat="server" Text='<%# Eval("U1", "{0:0.##}") %>'
                                        Width="60px" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>--%>
                                     <asp:TextBox ID="txtEditUnit2" BackColor="Aqua" runat="server" Text='<%# Eval("totalU2") %>' MaxLength="10"
                                        Width="60px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                      <%--Bram 20Jun22 diganti hanya u2--%>
                                    <asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U2_nm") %>'></asp:Label>
                                    <%--<asp:Label ID="lblU1Nm" runat="server" Text='<%# Eval("U1_nm") %>'></asp:Label>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                      <%--Bram 20Jun22 diganti hanya u2--%>
                                     <asp:Label ID="lblEditU1Nm" runat="server" Text='<%# Eval("U2_nm") %>'></asp:Label>
                                   <%-- <asp:Label ID="lblEditU1Nm" runat="server" Text='<%# Eval("U1_nm") %>'></asp:Label>--%>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" ImageUrl="~/Images/edit.png" />
                                    <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" ImageUrl="~/Images/Delete.png"
                                        OnClientClick="Confirm()" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" ImageUrl="~/Images/icon-update.png"
                                        Width="16px" Height="16px" />
                                    <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/icon-Cancel.png"
                                        Width="16px" Height="16px" />
                                </EditItemTemplate>
                               
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
        
        </div>
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
                    <asp:TextBox id="txtSearch" style="width:auto;" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                    

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
        <th scope="col">CC Stock</th>
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
                                    <asp:Label ID="lblName" runat="server" Text=' <%# Eval("name")%>'></asp:Label>
                </td>
                                <td >
                                    <asp:Label ID="Label1" runat="server" Text=' <%# Eval("stock")%>'></asp:Label>
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
        <div style="font-family: Calibri; ">
    <asp:Label ID="lblError" runat="server" Text='' ForeColor = "Red" Font-Size ="Medium" ></asp:Label>
            <asp:Label ID="lblnoitem" runat="server" Text="0"  Visible = "false"></asp:Label>
    
    </div>
    </div>
     
    
          <script>
       
      
            function OnKeyPress(e) {
                if (e.keyCode == "27") {
                    $find("ModalItem").hide();
                   

                }
            }

            function openModalItem() {
                $('#ModalItem').modal({ show: true });
                //document.getElementById("txtsearch").focus();
            }

            if ($(window).width() >= 960) {
                document.getElementById("formload").style.width = "100%";
            } else {
                document.getElementById("formload").style.width = "80%";

            }
    </script>
</asp:Content>
