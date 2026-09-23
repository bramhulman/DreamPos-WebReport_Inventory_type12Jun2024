<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteInventoryHome.Master" CodeBehind="User_Edit.aspx.vb" Inherits="HQWeb.User_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <%--<script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>--%>
    <script src="js/jquery.ui.core.js" type="text/javascript"></script>
    <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="js/jquery.ui.button.js" type="text/javascript"></script>
    <script src="js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script> 
    <script src="js/jquery.ui.combobox.js" type="text/javascript"></script> 
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <%--<script type="text/javascript">
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#<%=Image1.ClientID %>').attr('src', e.target.result);
                };

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>--%>
  <script type="text/javascript">
      function togglePanel(checkbox) {
          var panel = document.getElementById('<%= DetailInputs.ClientID %>');
          if (checkbox.checked) {
              panel.style.display = "block";
          } else {
              panel.style.display = "none";
          }
      }
  </script>
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

      </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager runat="server">
                
     </asp:ScriptManager>
   <style>
       .form-control{ 
           width: 40%;
         }
       /* just to help visualise the layout: */

            label {
                            padding: 0.5em;
            }


            /* to ensure visibility of the :focus indicator should the client
               be using keyboard navigation: */

            :focus-visible {
              outline-width: 2px;
              outline-style: solid;
              outline-color: #f90;
              outline-offset: 0.1em;
            }

   </style>
      <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Store Group Edit</h1>
          </div>
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            <!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="inventoryHome.aspx">Home</a></li>
              <li class="breadcrumb-item active">Store Group Edit</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <div class="col-md-12"  style="background-color:white;">
       
    <!-- /.content-header -->
   <%-- <h2>User Access</h2>--%>

    <div class="row">
        <div class="form-horizontal" id="formload">
                   <div class="card-body">

                              <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtnumber" CssClass="col-sm-2 control-label text-xl-right" Text="Number"></asp:Label>
    <div class="col-md-10">
        <%--<asp:label ID="lblnumber" runat="server" CssClass="form-control">--%>
            <asp:textbox ID="txtnumber" runat="server" CssClass="form-control">
        </asp:textbox>
        
    </div>
</div>

                      <%-- nama --%>
             <div class="form-group row">
          <asp:Label runat="server" AssociatedControlID="txtnama" CssClass="col-sm-2 control-label text-xl-right" Text="Nama"></asp:Label>
          <div class="col-md-10">
              <asp:Textbox ID="txtnama" runat="server" CssClass="form-control" MaxLength="20" />
               <asp:RequiredFieldValidator runat="server" ControlToValidate="txtnama"
                    CssClass="text-danger" ErrorMessage="The Name field is required." />
          </div>
      </div>
                                        <%-- MajorGroup --%>
                 <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="ddlmajorgroup" CssClass="col-sm-2 control-label text-xl-right" Text="Major Group"></asp:Label>
    <div class="col-md-10">
        <asp:dropdownlist ID="ddlmajorgroup" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlmajorgroup_SelectedIndexChanged" AppendDataBoundItems="false" AutoPostBack="True"
               EnableViewState="True" ViewStateMode="Enabled">
            <asp:ListItem Value="-1">None</asp:ListItem>
        </asp:dropdownlist>
        <asp:TextBox runat="server" ID="txtaddmajorgroup" CssClass="form-control" Visible="false" ReadOnly="true" />
           <asp:LinkButton ID="lnkFakemajor" runat="server" Style="display: none" />
            
    </div>
</div>
       <%-- MajorGroup --%>
                      <%-- ItemGroup --%>
                      <div class="form-group row">
         <asp:Label runat="server" AssociatedControlID="ddlitemgroup" CssClass="col-sm-2 control-label text-xl-right" Text="Item Group"></asp:Label>
         <div class="col-md-10">
             <asp:dropdownlist ID="ddlitemgroup" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlitemgroup_SelectedIndexChanged" AppendDataBoundItems="false" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled">
                 <asp:ListItem Value="-1">None</asp:ListItem>
             </asp:dropdownlist>
             <asp:TextBox runat="server" ID="txtaddgroup" CssClass="form-control" Visible="false" ReadOnly="true" />
                <asp:LinkButton ID="lnkFake" runat="server" Style="display: none" />
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlitemgroup" InitialValue="-1"
                    CssClass="text-danger" ErrorMessage="The Item Group field is required." />
         </div>
     </div>
            <%-- ItemGroup --%>           
                                        <%-- Currency --%>
                 <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="ddlCurrency" CssClass="col-sm-2 control-label text-xl-right" Text="Currency"></asp:Label>
    <div class="col-md-10">
        <asp:dropdownlist ID="ddlCurrency" runat="server" CssClass="form-control" AppendDataBoundItems="false" AutoPostBack="True"
               EnableViewState="True" ViewStateMode="Enabled">
            <asp:ListItem Value="-1">None</asp:ListItem>
        </asp:dropdownlist>
        <asp:TextBox runat="server" ID="txtaddcurrency" CssClass="form-control" Visible="false" ReadOnly="true" />
           <asp:LinkButton ID="lnkFakecurrency" runat="server" Style="display: none" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCurrency" InitialValue="-1"
               CssClass="text-danger" ErrorMessage="The Currency field is required." />
    </div>
</div>
       <%-- Currency --%>    
                                                              <%-- MenuItemizerID --%>
                 <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="ddlMenuItemizer" CssClass="col-sm-2 control-label text-xl-right" Text="MenuItemizer"></asp:Label>
    <div class="col-md-10">
        <asp:dropdownlist ID="ddlMenuItemizer" runat="server" CssClass="form-control" AppendDataBoundItems="false" AutoPostBack="True"
               EnableViewState="True" ViewStateMode="Enabled">
            <asp:ListItem Value="-1">None</asp:ListItem>
        </asp:dropdownlist>
        <asp:TextBox runat="server" ID="txtaddMenuItemizer" CssClass="form-control" Visible="false" ReadOnly="true" />
           <asp:LinkButton ID="lnkFakeMenuItemizer" runat="server" Style="display: none" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMenuItemizer" InitialValue="-1"
               CssClass="text-danger" ErrorMessage="The MenuItemizer field is required." />
    </div>
</div>
       <%-- MenuItemizerID --%>   
                                                                                      <%-- ChargeItemizer --%>
                 <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="ddlChargeItemizer" CssClass="col-sm-2 control-label text-xl-right" Text="ChargeItemizer"></asp:Label>
    <div class="col-md-10">
        <asp:dropdownlist ID="ddlChargeItemizer" runat="server" CssClass="form-control" AppendDataBoundItems="false" AutoPostBack="True"
               EnableViewState="True" ViewStateMode="Enabled">
            <asp:ListItem Value="-1">None</asp:ListItem>
        </asp:dropdownlist>
        <asp:TextBox runat="server" ID="txtaddChargeItemizer" CssClass="form-control" Visible="false" ReadOnly="true" />
           <asp:LinkButton ID="lnkFakeChargeItemizer" runat="server" Style="display: none" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlChargeItemizer" InitialValue="-1"
               CssClass="text-danger" ErrorMessage="The ddlChargeItemizer field is required." />
    </div>
</div>
       <%-- ChargeItemizer --%>   
       <%-- Unit --%>
                 <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="ddlUnit" CssClass="col-sm-2 control-label text-xl-right" Text="Unit"></asp:Label>
    <div class="col-md-10">
        <asp:dropdownlist ID="ddlUnit" runat="server" CssClass="form-control" AppendDataBoundItems="false" AutoPostBack="True"
               EnableViewState="True" ViewStateMode="Enabled">
            <asp:ListItem Value="-1">None</asp:ListItem>
        </asp:dropdownlist>
        <asp:TextBox runat="server" ID="txtaddUnit" CssClass="form-control" Visible="false" ReadOnly="true" />
           <asp:LinkButton ID="lnkFakeUnit" runat="server" Style="display: none" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlUnit" InitialValue="-1"
               CssClass="text-danger" ErrorMessage="The ddlUnit field is required." />
    </div>
</div>
       <%-- Unit --%> 

<%-- barcode --%>
       <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtbarcode" CssClass="col-sm-2 control-label text-xl-right" Text="barcode"></asp:Label>
    <div class="col-md-10">
        <asp:Textbox ID="txtbarcode" runat="server" CssClass="form-control" MaxLength="20" />
            </div>
</div>
<%-- barcode --%>
               <%-- Startdate --%>
                 <div class="form-group row">
                                  <asp:label runat="server" CssClass="col-sm-2 control-label text-xl-right text-nowrap" ID="Label1" Text="Start Date: " AssociatedControlID="txteffdate"></asp:label>
<div class="input-group col-md-10">
  
            <div class="input-group">
                
                  <div class="input-group-prepend">
              <span class="input-group-text">
                <asp:CheckBox runat="server" ID="chk_eff" AutoPostBack="true"/>
              </span>
            </div>
           <div class="input-group-prepend">
           <span class="input-group-text">
             <i class="far fa-calendar-alt"></i>
           </span>
         </div>
        
       
           <asp:TextBox style="text-align:left; width:auto;" runat="server" ID="txteffdate" Class="form-control" AutoPostBack="false" Enabled="false" />
               <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txteffdate" enabled="true"
                 Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
       </div>             
       </div>  
                </div>

       <%-- Startdate --%> 
                           <%-- Enddate --%>
                 <div class="form-group row">
                    <asp:label runat="server" CssClass="col-sm-2 control-label text-xl-right text-nowrap" ID="Label2" Text="End Date: " AssociatedControlID="txtenddate"></asp:label>
                        <div class="input-group col-md-10">
  
                                    <div class="input-group">

                                          <div class="input-group-prepend">
                                      <span class="input-group-text">
                                        <asp:CheckBox runat="server" ID="chk_end" AutoPostBack="true"/>
                                      </span>
                                    </div>
                                   <div class="input-group-prepend">
                                   <span class="input-group-text">
                                     <i class="far fa-calendar-alt"></i>
                                   </span>
                                 </div>
    
   
                                   <asp:TextBox style="text-align:left; width:auto;" runat="server" ID="txtenddate" Class="form-control" AutoPostBack="false" Enabled="false" />
                                       <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtenddate" enabled="true"
                                         Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                               </div>             
                               </div>
                </div>

       <%-- Enddate --%> 
                                   <%-- Option --%>
                 <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="lvwOption" CssClass="col-sm-2 control-label text-xl-right" Text="Option"></asp:Label>
                        <div class="col-md-10" style="width: 50%;height:100%;text-align:center; flex:inherit;">
               
                            <asp:ListView ID="lvwOption" runat="server">
                                    <LayoutTemplate>
                                        <table id="tblitem_xdetail" class="table table-bordered">
                                            <thead>
                                                <tr>
                                                <th style="text-align:center;">
                                                        <asp:CheckBox ID="cbxOption" OnCheckedChanged="cbxOption_Clicked" AutoPostBack="true" ToolTip="Select All" runat="server" />
                                                    </th>
                                                <th style="text-align:center;display:none;">
                                                <b>ID</b>
                                                </th> 
                                                    <th style="text-align:center;">
                                                <b>Item Option</b>
                                                </th> 
                               
                                                </tr>
                                            </thead>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width:5%">
                                                <asp:CheckBox ID="cbxOptionDetail" runat="server"></asp:CheckBox>        
                                            </td>
                                            <td style="display:none">
                                                <asp:Label ID="lblDetailID" runat="server" Text=' <%# Eval("id")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDetailName" runat="server" Text=' <%# Eval("name")%>'></asp:Label>
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                       
                                </asp:ListView>

                
                        </div>
</div>
       <%-- Option --%> 

       

               <%-- Photo --%>
  <%--<div class="form-group row">
         <asp:Label runat="server" AssociatedControlID="FileUpLoad1" CssClass="col-md-2 control-label">Photo</asp:Label>
            <div class="col-md-10">
                <asp:Image ID="Image1" CssClass="form-inline" runat="server" Height="80" Width="80" />
                <asp:FileUpload ID="FileUpLoad1" CssClass="form-inline" AlternateText="You cannot upload files" runat="server"
                    Style="font-weight: 700; color: #000000; background-color: #FFFFFF" />
            </div>
</div>--%>
       <%-- Photo --%> 

               <%-- SLU --%>
                 <%--<div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="ddlSLU" CssClass="col-sm-2 control-label" Text="SLU"></asp:Label>
    <div class="col-md-10">
        <asp:dropdownlist ID="ddlSLU" runat="server" CssClass="form-control" AppendDataBoundItems="false" AutoPostBack="True"
               EnableViewState="True" ViewStateMode="Enabled">
            <asp:ListItem Value="-1">None</asp:ListItem>
        </asp:dropdownlist>
        <asp:TextBox runat="server" ID="txtaddSLU" CssClass="form-control" Visible="false" ReadOnly="true" />
           <asp:LinkButton ID="lnkFakeSLU" runat="server" Style="display: none" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSLU" InitialValue="-1"
               CssClass="text-danger" ErrorMessage="The SLU field is required." />
    </div>
</div>--%>
       <%-- SLU --%> 
         
                         <%-- Lang1 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtLang1" CssClass="col-sm-2 control-label text-xl-right" Text="Lang1"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtLang1" runat="server" CssClass="form-control" MaxLength="30">
    </asp:textbox>
    
</div>
</div>
         <%-- Lang1 --%> 
                                 <%-- Lang2 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtLang2" CssClass="col-sm-2 control-label text-xl-right" Text="Lang2"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtLang2" runat="server" CssClass="form-control" MaxLength="30">
    </asp:textbox>
    
</div>
</div>
         <%-- Lang2 --%>
                                         <%-- Lang3 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtLang3" CssClass="col-sm-2 control-label text-xl-right" Text="Lang3"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtLang3" runat="server" CssClass="form-control" MaxLength="30">
    </asp:textbox>
    
</div>
</div>
         <%-- Lang3 --%>
       <%-- showdetail --%>
                 <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="chkShowDetail" CssClass="col-sm-2 control-label text-xl-right"></asp:Label>
    <div class="col-md-10">
        <asp:CheckBox ID="chkShowDetail" runat="server" Text="Show Detail" OnClick="togglePanel(this);" />
       <%--<div class="checkbox">
                     <asp:Label runat="server"><asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="Checkshowdet_Clicked" ID="chkShowDetail" />Show Detail</asp:Label>
           
                </div>--%>
    </div>
</div>
       <%-- showdetail --%> 

<div class="DetailInputs" id="DetailInputs" runat="server" style="display:none;">
             <%-- Factor1 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtfactor1" CssClass="col-sm-2 control-label text-xl-right" Text="Factor1"></asp:Label>
    <div class="col-md-10">
        <asp:textbox ID="txtfactor1" runat="server" CssClass="form-control" type="number" MaxLength="3" Text="0">
        </asp:textbox>
        
    </div>
</div>
         <%-- Factor1 --%> 
<%-- nmFactor1 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtnmfactor1" CssClass="col-sm-2 control-label text-xl-right" Text="Nama Factor1"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtnmfactor1" runat="server" CssClass="form-control" MaxLength="30">
    </asp:textbox>
    
</div>
</div>
         <%-- nmFactor1 --%>
                 <%-- Factor2 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtfactor2" CssClass="col-sm-2 control-label text-xl-right" Text="Factor2"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtfactor2" runat="server" CssClass="form-control" type="number" MaxLength="3" Text="0">
    </asp:textbox>
    
</div>
</div>
         <%-- Factor2 --%> 
<%-- nmFactor2 --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtnmfactor2" CssClass="col-sm-2 control-label text-xl-right" Text="Nama Factor2"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtnmfactor2" runat="server" CssClass="form-control" MaxLength="30">
    </asp:textbox>
    
</div>
</div>
         <%-- nmFactor2 --%>
                 <%-- ecount --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtecount" CssClass="col-sm-2 control-label text-xl-right" Text="ecount"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtecount" runat="server" CssClass="form-control" type="number" MaxLength="3" Text="0">
    </asp:textbox>
    
</div>
</div>
         <%-- ecount --%> 
                         <%-- etime --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txteTime" CssClass="col-sm-2 control-label text-xl-right" Text="eTime"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txteTime" runat="server" CssClass="form-control" type="number" MaxLength="3" Text="0">
    </asp:textbox>
    
</div>
</div>
         <%-- etime --%> 
                         <%-- stime --%> 
   <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtsTime" CssClass="col-sm-2 control-label text-xl-right" Text="sTime"></asp:Label>
<div class="col-md-10">
    <asp:textbox ID="txtsTime" runat="server" CssClass="form-control" type="number" MaxLength="3" Text="0">
    </asp:textbox>
    
</div>
</div>
         <%-- stime --%> 

                        <%-- Ingredients --%>
       <div class="form-group row">
    <asp:Label runat="server" AssociatedControlID="txtIngredients" CssClass="col-sm-2 control-label text-xl-right" Text="Ingredients"></asp:Label>
    <div class="col-md-10">
        <asp:Textbox ID="txtIngredients" runat="server" CssClass="form-control" MaxLength="30" />
         
    </div>
</div>
 <%-- Ingredients --%>
</div>

         
               <div class="form-group row">
                    <div class="col-md-offset-2 col-md-10">
                            <table>
                                <tr>
                                    <%--<td>
                                        
                                         <asp:Button runat="server" ID="btnInsert" OnClick="btnInsert_Click" Text="Add" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" />
                                    </td>--%>
                                    <td>
                                        
                                         <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" Visible="false" />
                                    </td>
                                    <td>
                                         <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-primary" Style="font-weight: 600; border-color:gray; width:108px;" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                           
                        </div>
                   </div>       
             
     

    </div>

   </div>
        </div>
     
    </div>
     
    
          <script>

              if ($(window).width() >= 960) {
                  document.getElementById("formload").style.width = "100%";
              } else {
                  document.getElementById("formload").style.width = "80%";

              }


          </script>
</asp:Content>
