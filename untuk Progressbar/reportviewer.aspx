<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteReport.Master" CodeBehind="ReportViewer.aspx.vb" Inherits="DreamPos_WebReport.ReportViewer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="LeftContentBar" ContentPlaceHolderID="LeftContentBar" runat="server">
    <asp:ScriptManager runat="server">         
    </asp:ScriptManager>

                  <%-- Modal Pop Up 1 Untuk Store --%>   
    <%-- <ajaxtoolkit:modalpopupextender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnOutlet"
        CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </ajaxtoolkit:modalpopupextender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="left" Style="display: none; overflow:-moz-hidden-unscrollable;" >
       
        <asp:UpdatePanel ID="UpdatePanel1" CssClass="modalPopup" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="style16">
                            <b style="color: Black">Search</b>
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button Text="Search" ID="btnSearch" runat="server" OnClick="btnSearch_Click1"
                                Style="font-weight: 700; color: black;" />
                            
                        </td>
                    </tr>
                </table>                
                    <br />
                <div style="height: 380px; width: 375px; overflow: scroll" class="menutab">
                    <table id="TblStk" style="width:370px; border-style: solid;" class="menutab">
                        <tr class="altRow">
                            <td class="style1">
                                <b style="color: #0a0a0a">Check</b>
                            </td>
                            <td class="style1">
                                <b style="color: #0a0a0a">Kode store</b>
                            </td>
                            <td class="style1">
                                <b style="color: #0a0a0a">Nama Store</b>
                            </td>
                             <%--<td class="style1">
                                <b style="color: #0a0a0a">Kode 2</b>
                            </td>--%>

                         <%--</tr>
                        <asp:ListView ID="LsvStk" runat="server">
                            <ItemTemplate>
                                <td style="width: 30px">
                                    <asp:CheckBox ID="CheckBox1" runat="server"></asp:CheckBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblID" runat="server" forecolor="black" Text=' <%# Eval("kode")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblName" ForeColor="black" runat="server" Text=' <%# Eval("nama")%>'></asp:Label>
                                </td>--%>
                                 <%-- <td>
                                    <asp:Label ID="lblBatch" ForeColor="black" runat="server" Text=' <%# Eval("kode2")%>'></asp:Label>
                                </td>--%>
                                <%--</tr>
                            </ItemTemplate>
                        </asp:ListView>

                       
                    </table>
                </div>
                 
            </ContentTemplate>
        </asp:UpdatePanel>
        
         <asp:Button ID="btnclose" runat="server" Text="Close" Style="color: black;" /> 
         <asp:Button ID="btnSubmit" runat="server" Text="Select" Style="color: black;" onclick="btnSubmit_Click"/> 
          <asp:Button ID="btnall" runat="server" Text="All Store" Style="color: black;" OnClick="btnall_Click" /> 
    </asp:Panel>--%>

    <%-- Modal Pop Up 2 Untuk Brand --%>   
    <%-- <ajaxtoolkit:modalpopupextender ID="Mp2" runat="server" PopupControlID="Panel2" TargetControlID="btnBrand"
        CancelControlID="btnClose2" BackgroundCssClass="modalBackground">
    </ajaxtoolkit:modalpopupextender>
   <div class="col-md-16" style="overflow:auto;">
       <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="left" Style="display: none" >
       
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="style16">
                            <b style="color: Black">Search</b>
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="txtSearch2" runat="server" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button Text="Search" ID="btnSearch2" runat="server" OnClick="btnSearch2_Click1"
                                 Style="color: black;" />
                            
                        </td>
                    </tr>

                </table>
                
                    <br />
                <div style="height: 380px; width: 375px; overflow: scroll" class="menutab">
                        <table id="Table1" class="menutab" style="border-style: solid; width:370px;">
                            <tr class="altRow">
                                <td class="style1">
                                    <b style="color: #0a0a0a">Check</b>
                                </td>
                                <td style="display: none">
                                    <b style="color: #0a0a0a">Id</b>
                                </td>
                                <td class="style1">
                                    <b style="color: #0a0a0a">Brand</b>
                                </td>
                            </tr>
                            <asp:ListView ID="ListViewBrand" runat="server">
                                <ItemTemplate>
                                    <td style="width: 30px">
                                        <asp:CheckBox ID="CheckBox2" ForeColor="Black" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="display: none">
                                        <asp:Label ID="LblIdBrand" ForeColor="Black" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblBrand2" ForeColor="Black" runat="server" Text=' <%# Eval("nama")%>'></asp:Label>
                                    </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>

                       
                        </table>
                </div>
                 
            </ContentTemplate>
        </asp:UpdatePanel>
        
         <asp:Button ID="btnClose2" runat="server" Text="Close" Style="color: black;" /> 
         <asp:Button ID="btnSubmit2" runat="server" Text="Select" Style="color: black;" OnClick="btnSubmit2_Click1"/> 
          <asp:Button ID="btnAll2" runat="server" Text="All Brand" Style="color: black;" OnClick="btnAll2_Click1"/> 
    </asp:Panel>
       </div>--%>
   


    <%-- Modal Pop Up 3 Untuk City --%>  
   <%-- <ajaxtoolkit:modalpopupextender ID="Mp3" runat="server" PopupControlID="Panel3" TargetControlID="btnCity"
        CancelControlID="btnClose3" BackgroundCssClass="modalBackground">
    </ajaxtoolkit:modalpopupextender>   
   <div class="col-md-16" style="overflow:auto;">
       <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" align="left" Style="display: none" >
       
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="style16">
                            <b style="color: Black">Search</b>
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="txtSearch3" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button Text="Search" ID="btnSearch3" runat="server" OnClick="btnSearch3_Click1"
                                Style="color: black;" />
                            
                        </td>
                    </tr>

                </table>
                
                    <br />
                <div style="height: 380px; width: 375px; overflow: scroll" class="menutab">
                    <table id="tblCity"  class="menutab" style="border-style: solid;width:370px;">
                        <tr class="altRow">
                            <td class="style1">
                                <b style="color: #0a0a0a">Check</b>
                            </td>
                            <td class="style1">
                                <b style="color: #0a0a0a">Id</b>
                            </td>
                            <td class="style1">
                                <b style="color: #0a0a0a">City</b>
                            </td>
                        </tr>
                        <asp:ListView ID="ListViewCity" runat="server">
                            <ItemTemplate>
                                <td style="width: 30px">
                                    <asp:CheckBox ID="CheckBox3" ForeColor="Black" runat="server"></asp:CheckBox>
                                </td>
                                <td >
                                    <asp:Label ID="LblIdCity" ForeColor="Black" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lblCity2" ForeColor="Black" runat="server" Text=' <%# Eval("nama")%>'></asp:Label>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>

                       
                    </table>
                </div>
                 
            </ContentTemplate>
        </asp:UpdatePanel>
        
         <asp:Button ID="btnClose3" runat="server" Text="Close"  Style="color: black;"/> 
         <asp:Button ID="btnSubmit3" runat="server" Text="Select" Style="color: black;" OnClick="btnSubmit3_Click" /> 
          <asp:Button ID="btnAll3" runat="server" Text="All Brand" Style="color: black;" onclick="btnAll3_Click"/> 
    </asp:Panel>
         </div>--%>

   

   <%--        <style type="text/css">
         .style1
        {
            width: 72px;
            background-color:#DFDFFF;
            color: #000000;
        }
        
       #leftContent 
        {
            background-color:#DFDFFF;
            width: 244px;    
            padding: 10px;
            font-size: 90%;
            text-align: left;
            position: absolute;
            top: 0px;
            margin-top: 0px;
            left: -1px;
            margin-left:0px;
	        height: 1080px;
	        color:Black;
            }
            
        #leftContent h3 {
            text-transform: uppercase;
            letter-spacing: 2px;
            text-align: center;
            border-top: dotted 1px #660;
            border-bottom: dotted 1px #660;
            color:Black;
            }

        #leftContent h4 
        {
            text-transform: uppercase;
            letter-spacing: 2px;
            text-align: center;
            border-top: dotted 1px #660;
            border-bottom: dotted 1px #660;
            color:Black;
            }
            
        #mainContent {
            text-align: left;
            margin-left: 265px;
            padding: 10px;
	        height: 1080px;
	        background-color:#DFDFFF;
	        width: 820px; 
	        margin-right: 0px;
	        color:Black;
            }

        #content{
	            background:#FFF; /*369*/
	            height:880px;
	            width:950px;
	            position:relative;
	            top:1200px;
	            z-index:0;
                }
        .modal{
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        
         .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
         .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 375px;
            height: 400px;
            overflow-x:auto;
        }
        
        .center
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center img
        {
            height: 128px;
            width: 128px;
        }
          .btnstyle
        {
         text-align: center;
        }
        .menu
    #tblEmpDetails
    {
        margin-right: 0px;
    }
    </style>--%>    


    <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
          <!-- Add icons to the links using the .nav-icon class
               with font-awesome or any other icon font library -->

        <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
          
              <h4 style="text-align: center;color:white;">
                Select Report
                 <asp:DropDownList ID="ddlRpt" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled">
                </asp:DropDownList>
              </h4>
          </li>
       
         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <h3 style="text-align:center;color:white;"> 
                  Parameter 
              </h3>
          </li>
       
        <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                  Own dan Franchise 
                  <asp:DropDownList ID="ddlOwn" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled"></asp:DropDownList>
              </p>
          </li>

         <li class="nav-item">
              

                               <table style="width:100%">
                                  
                                    <tr>
                                    <td rowspan="2" style="width:40%; text-align:center"><asp:Label runat="server" AssociatedControlID="btnshowmdlbrand" Text="Brand: " ForeColor="White"></asp:Label></td>
                                    <td style="width:60%; margin-left:10px; text-align:center""><button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshowmdlbrand" data-target="#ModalBrand" style="padding:1px 4px;">
                                         Get Brand
                                         </button>
                                        </td>
                                  </tr>
                                  <tr>
                                    <td style="text-align:center;"><asp:Label ID="lblNmBrand" runat="server" Text="All Brand" Textalign="center" ForeColor="White"></asp:Label> </td>
                                  </tr>

                                   <tr>
                                    <td rowspan="2" style="width:40%; text-align:center"><asp:Label runat="server" AssociatedControlID="btnshowmdlcity" Text="City: " ForeColor="White"></asp:Label></td>
                                    <td style="width:60%; margin-left:10px; text-align:center;"><button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshowmdlcity" data-target="#ModalCity" style="padding:1px 4px;">
                                         Get City
                                         </button>
                                        </td>
                                  </tr>
                                  <tr>
                                    <td style="text-align:center;"><asp:Label ID="LblNmCity" runat="server" Text="All City" Textalign="center" ForeColor="White"></asp:Label> </td>
                                    
                                  </tr>

                                    <tr>
                                    <td rowspan="2" style="width:40%; text-align:center"><asp:Label runat="server" AssociatedControlID="btnshowmdlstore" Text="Store: " ForeColor="White"></asp:Label></td>
                                    <td style="width:60%; margin-left:10px; text-align:center;"><button type="button" runat="server" class="btn btn-primary" data-toggle="modal" id="btnshowmdlstore" data-target="#ModalStore" style="padding:1px 4px;">

                                         Get Store
                                         </button>
                                        </td>
                                  </tr>
                                  <tr>
                                    <td style="text-align:center;"><asp:Label ID="lblOutletNameData" runat="server" Text="All Store" Textalign="center" ForeColor="White"></asp:Label> </td>
                                    
                                  </tr>

                                </table>
                               
                               
            </li>
          
         <li class="nav-item" id="paramitem" style="display:none" runat="server">
             <div class="row">
                    <div class="form-horizontal">
                           <div class="card-body">

                               <table style="width:100%">
                                  
                                    <tr>
                                    <td rowspan="2" style="width:40%; text-align:center"><asp:Label runat="server" AssociatedControlID="btnItem" Text="Item: " ForeColor="White"></asp:Label></td>
                                    <td style="width:60%; margin-left:10px; text-align:center;"><asp:button runat="server" class="btn btn-primary" id="btnItem" style="padding:1px 4px;" Text="Get Store">
                                        </asp:button>
                                        </td>
                                  </tr>
                                  <tr>
                                    <td style="text-align:center;"><asp:Label ID="LblNmItem" runat="server" Text="All Item" Textalign="center" ForeColor="White"></asp:Label> </td>
                                    
                                  </tr>
                                   </table>
                               </div>
                        </div>
                 </div>
             
          </li>
         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <div class="form-group" style="text-align:center;color:white;" > 
                  <asp:label runat="server" ID="lblparamfrom" Text="Date From"></asp:label> <br />
                  <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID ="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>--%>
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtFrom" Class="form-control" AutoPostBack="False" />
                          <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFrom" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>                                  
              </div>
          </li>

         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <div style="text-align:center;color:white;" id="paramto" runat="server" > 
                  <asp:label runat="server" ID="lblparamto" Text="Date To"></asp:label> <br />
                   <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt" runat="server" id="iconparamto"></i>
                      </span>
                    </div>
                     <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID ="ToolkitScriptManager2"></ajaxToolkit:ToolkitScriptManager>--%>
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtTo" Class="form-control" AutoPostBack="False" />
                          <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>                               
              </div>
          </li>

         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
             <div style="text-align:center;color:white;">
              <%--<p style="text-align:center;color:white;" >--%> 
                 <br />
                 <%-- Update 21 Februari 2020 --%>
                  <asp:Button ID="btnShow" runat="server" Style="font-weight: 600; border-color:gray;" OnClick="btnShow_Click"
                    Text="OK" CssClass="btn btn-primary" OnClientClick="showPopup()"/>
                  <br />
                  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                      <ContentTemplate></ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="btnShow"  EventName="Click"/>
                      </Triggers>
                  </asp:UpdatePanel>
                  <br />

                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" style="display:none"/>
                    <ajaxToolkit:ModalPopupExtender runat="server" ID="programmaticModalPopup"
                        BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup"
                        PopupControlID="programmaticPopup"
                        BackgroundCssClass="modalBackground"
                        DropShadow="True"
                        RepositionMode="RepositionOnWindowScroll" >
                    </ajaxToolkit:ModalPopupExtender>

                    <asp:Panel runat="server" CssClass="modalPopupProgress" ID="programmaticPopup" style="background-color:lightgray;display:none;height:auto;width:auto;padding:10px">
                    <div id='messagediv' style="text-align:center">
                        <img alt="" src="/images/loader.gif" />
                    </div>

                    </asp:Panel>
                <%--</p>--%>
                 </div>
          </li>
         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                 <asp:Button Text="Export CSV" ID="btnExport" runat="server" OnClick="btnExport_Click"
                    Style="font-weight: 600; border-color:gray;" CssClass="btn btn-primary" /> 
                  </p>
          </li>
         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                 <asp:Button Text="Export Excel" ID="btnExport2" runat="server" OnClick="btnExport2_Click"
                    Style="font-weight: 600; border-color:gray;" CssClass="btn btn-primary" /> 
                  <br />
                  <br />
              </p>
          </li>
        <li class="nav-item">
            <asp:Label ID="lblnobrand" runat="server" Text="0" Visible="false"></asp:Label>
            <asp:Label ID="lblnoCity" runat="server" Text="0" Visible="false"></asp:Label>
            <asp:Label ID="lblNoOutlet" runat="server" Text="0"  Visible = "false"></asp:Label>
             <asp:Label ID="lblnoitem" runat="server" Text="0"  Visible = "false"></asp:Label>
        </li>
</ul>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    
    <section class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1>Report Menu</h1>
          </div>
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="/Home.aspx">Home</a></li>
              <li class="breadcrumb-item active">Report Menu</li>
            </ol>
          </div>
        </div>
      </div><!-- /.container-fluid -->
    </section>
    <div class="card">
        <div class="card-header">
          <h3 class="card-title">Report Menu</h3>

          <div class="card-tools">
            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
              <i class="fas fa-times"></i></button>
          </div>
        </div>
        
          <asp:DataGrid runat="server" ID="dg1" HeaderStyle-backcolor="#4b6c9e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" 
              HeaderStyle-BorderColor="White" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" AlternatingItemStyle-BackColor="#ccffff" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke" Width="100%" CssClass="jsgrid" AllowPaging="true" PageSize="15" OnPageIndexChanged="dg1_Paging"
              PagerStyle-Mode="NextPrev"
  PagerStyle-Position="Bottom"
  PagerStyle-HorizontalAlign="Center"
  PagerStyle-NextPageText="Next"
  PagerStyle-PrevPageText="Prev">
    </asp:DataGrid>
        </div>
        <!-- /.card-body -->
        <div class="card-footer">
          <%--Report Menu--%>
        </div>
        <!-- /.card-footer-->
      </div>

      <!-- Modal Brand -->
     <asp:UpdatePanel ID="UpdatePanel2" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalBrand" tabindex="-1" role="dialog" aria-labelledby="ModalBrand" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalBrandLabel">Select Brand</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>

      </div>

        <div class="row">
        <div class="form-horizontal" id="formload" runat="server" style="width:100%;">
                   <div class="card-body">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="txtSearch2" CssClass="col-sm-2 col-form-label text-nowrap" Text="Brand: "></asp:Label>
                 
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
                    <asp:TextBox id="txtSearch2" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                

                    <button runat="server" id="btnSearch2" onserverclick="btnSearch2_Click" class="btn btn-mini" title="Search">
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
  <table class="table">
   <thead>
    <tr>
      <th scope="col"> <asp:CheckBox ID="Checkbox_allbrand" runat="server" ToolTip="SelectAll" onclick="javascript:fnSelectAll();" OnCheckedChanged="Checkbox_allbrand_CheckedChange"></asp:CheckBox></th>
      <th scope="col" hidden="hidden">ID</th>
      <th scope="col">Brand</th>
    </tr>
  </thead>
     <tbody>
         
      <asp:ListView ID="ListViewBrand" runat="server">
            <ItemTemplate>
                <tr>
                <td>
                                    <asp:CheckBox ID="CheckBox2" runat="server"></asp:CheckBox>
                                </td>
                                <td hidden="hidden">
                                    <asp:Label ID="LblIdBrand" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lblBrand2" runat="server" Text=' <%# Eval("nama")%>'></asp:Label>
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
       <%-- <button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit2_Click">Select</button>
          <button type="button" id="btnAll2" runat="server" class="btn btn-primary" onserverclick="btnAll2_Click">SelectAll</button>--%>
          <button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit2_Click" id="RadButton3">Select</button>
          <button type="button" class="btn btn-primary" runat="server" onserverclick="btnAll3_ServerClick">SelectAll</button>
      </div>
    </div>
  </div>
</div>

                </ContentTemplate>
         </asp:UpdatePanel>
    <!-- Modal Brand -->
    
     <!-- Modal City -->
     <asp:UpdatePanel ID="UpdatePanel4" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalCity" tabindex="-1" role="dialog" aria-labelledby="ModalCity" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalCityLabel">Select City</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>

        <div class="row">
        <div class="form-horizontal" id="Div1" runat="server" style="width:100%;">
                   <div class="card-body">
                  
                      


               <div class="form-group row">
                    <asp:Label runat="server" AssociatedControlID="txtSearch3" CssClass="col-sm-2 col-form-label text-nowrap" Text="City: "></asp:Label>
                 
                        <div class="input-group-append">
                        <div class="input-group input-group-sm">
                    <asp:TextBox id="txtSearch3" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>
                

                    <button runat="server" id="btnSearch3" onserverclick="btnSearch3_Click" class="btn btn-mini" title="Search">
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
  <table class="table">
   <thead>
    <tr>
      <th scope="col"> <asp:CheckBox ID="Checkbox_allcity" runat="server" ToolTip="SelectAll" onclick="javascript:fnSelectAllCity();" OnCheckedChanged="Checkbox_allcity_CheckedChange"></asp:CheckBox></th>
      <th scope="col" hidden="hidden">ID</th>
      <th scope="col">City</th>
    </tr>
  </thead>
     <tbody>
         
      <asp:ListView ID="ListViewCity" runat="server">
            <ItemTemplate>
                <tr>
                <td>
                                    <asp:CheckBox ID="CheckBox3" runat="server"></asp:CheckBox>
                                </td>
                                <td hidden="hidden">
                                    <asp:Label ID="LblIdCity" runat="server" Text=' <%# Eval("Id")%>'></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="lblCity2" runat="server" Text=' <%# Eval("kota")%>'></asp:Label>
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
        <%--<button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit3_Click">Select</button>
          <button type="button" id="btnAll3" runat="server" class="btn btn-primary" onserverclick="btnAll3_Click">SelectAll</button>--%>
          <button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit3_Click" id="RadButton2">Select</button>
          <button type="button" class="btn btn-primary" runat="server" onserverclick="btnAll2_ServerClick">SelectAll</button>
      </div>
    </div>
  </div>
</div>

                </ContentTemplate>
         </asp:UpdatePanel>
    <!-- Modal City -->

     <!-- Modal Store -->
     <asp:UpdatePanel ID="UpdatePanel3" CssClass="modalPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

               
<div class="modal fade" id="ModalStore" tabindex="-1" role="dialog" aria-labelledby="ModalStore" aria-hidden="true" data-backdrop="false" style="opacity:1;z-index:1050;">
  <div class="modal-dialog modal-dialog-scrollable" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="ModalStoreLabel">Select Store</h5>
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
                    <asp:TextBox id="txtSearch" runat="server" CssClass="form-control" placeholder="Search" type="search" aria-label="Search"></asp:TextBox>

                    <button runat="server" id="btnSearch" onserverclick="btnSearch_Click1" class="btn btn-mini" title="Search">
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
  <table class="table">
   <thead>
    <tr>
      <th scope="col"> <asp:CheckBox ID="Checkbox_allstore" runat="server" ToolTip="SelectAll" onclick="javascript:fnSelectAllStore();" OnCheckedChanged="Checkbox_allstore_CheckedChanged"></asp:CheckBox></th>
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
                                    <asp:Label ID="lblkode" runat="server" Text=' <%# Eval("kode")%>'></asp:Label>
                </td>
                                <td >
                                    <asp:Label ID="lblName" runat="server" Text=' <%# Eval("nama")%>'></asp:Label>
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
          <button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit_Click" id="RadButton1">Select</button>
          <button id="btnAll1" type="button" runat="server" class="btn btn-primary" onserverclick="btnAll1_ServerClick">SelectAll</button>
        <%--<button type="button" class="btn btn-primary" runat="server" onserverclick="btnSubmit_Click">Select</button>
          <button type="button" id="btnAll" runat="server" class="btn btn-primary" onserverclick="btnAll_Click">SelectAll</button>--%>
      </div>
    </div>
  </div>
</div>

                </ContentTemplate>
         </asp:UpdatePanel>
    <!-- Modal Store -->

    <script>
        function OnKeyPress(e) {
            if (e.keyCode == "27") {
                    $find("ModalBrand").hide();
                    $find("ModalCity").hide();
                    $find("ModalStore").hide();
                    
            }
        }

       /*Update 21 Februari 2020*/
        function showPopup() {
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');       
            modalPopupBehavior.show();
        }
        function hidepopup()
        {
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function openModalBrand() {
            $('#ModalBrand').modal({ show: true });
        }
        function openModalCity() {
            $('#ModalCity').modal({ show: true });
        }
        function openModalStore() {
            $('#ModalStore').modal({ show: true });
        }

    </script>
    
</asp:Content>

