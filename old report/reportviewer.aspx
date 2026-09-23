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
     <ajaxtoolkit:modalpopupextender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnOutlet"
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

                        </tr>
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
                                </td>
                                 <%-- <td>
                                    <asp:Label ID="lblBatch" ForeColor="black" runat="server" Text=' <%# Eval("kode2")%>'></asp:Label>
                                </td>--%>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>

                       
                    </table>
                </div>
                 
            </ContentTemplate>
        </asp:UpdatePanel>
        
         <asp:Button ID="btnclose" runat="server" Text="Close" Style="color: black;" /> 
         <asp:Button ID="btnSubmit" runat="server" Text="Select" Style="color: black;" onclick="btnSubmit_Click1"/> 
          <asp:Button ID="btnall" runat="server" Text="All Store" Style="color: black;" OnClick="btnall_Click1" /> 
    </asp:Panel>

    <%-- Modal Pop Up 2 Untuk Brand --%>   
     <ajaxtoolkit:modalpopupextender ID="Mp2" runat="server" PopupControlID="Panel2" TargetControlID="btnBrand"
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
       </div>
   


    <%-- Modal Pop Up 3 Untuk City --%>  
    <ajaxtoolkit:modalpopupextender ID="Mp3" runat="server" PopupControlID="Panel3" TargetControlID="btnCity"
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
         <asp:Button ID="btnSubmit3" runat="server" Text="Select" Style="color: black;" OnClick="btnSubmit3_Click1" /> 
          <asp:Button ID="btnAll3" runat="server" Text="All Brand" Style="color: black;" onclick="btnAll3_Click1"/> 
    </asp:Panel>
         </div>
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
                  Own 
                  <asp:DropDownList ID="ddlOwn" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled"></asp:DropDownList>
              </p>
          </li>

         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                  Brand <br />

                  <asp:Button ID="BtnBrand" runat="server" Text="Get Brand"
                   Class="btn btn-primary" />

                  <br />
                  <asp:Label ID="lblNmBrand" runat="server" Text="All Brand" Textalign = "Centre"></asp:Label> 
              </p>
          </li>

         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                  City <br />
                  <asp:Button ID="btnCity" runat="server" Text="Get City" 
                   Class="btn btn-primary" />
                  <br />
                  <asp:Label ID="LblNmCity" runat="server" Text="All City" Textalign = "Centre"></asp:Label> 
              </p>
          </li>

        <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                  Store <br />
                  <asp:Button ID="btnOutlet" runat="server" Text="Get Store" 
                   Class="btn btn-primary" />
                  <br />
                  <asp:Label ID="lblOutletNameData" runat="server" Text="All Store" Textalign = "Centre"></asp:Label> 
              </p>
          </li>
         <li class="nav-item" id="paramitem" style="display:none">
            
              <p style="text-align:center;color:white;" > 
                  Item <br />
                  <asp:Button ID="btnItem" runat="server" Text="Get Item" 
                   Class="btn btn-primary" />
                  <br />
                  <asp:Label ID="LblNmItem" runat="server" Text="All Item" Textalign = "Centre"></asp:Label> 
              </p>
          </li>
         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <div class="form-group" style="text-align:center;color:white;" > 
                  Date From <br />
                  <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtFrom" Class="form-control" />
                          <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFrom" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>                                  
              </div>
          </li>

         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <div style="text-align:center;color:white;" > 
                  Date To <br />
                   <div class="input-group">
                      <div class="input-group-prepend">
                      <span class="input-group-text">
                        <i class="far fa-calendar-alt"></i>
                      </span>
                    </div>
                     
                      <asp:TextBox style="text-align:center;" runat="server" ID="txtTo" Class="form-control"/>
                          <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>                               
                  </div>                               
              </div>
          </li>

         <li class="nav-item">
            <%--<a runat="server" href="~/Home.aspx" class="nav-link">--%>
              <%--<i class="nav-icon fas fa-tachometer-alt"></i>--%>
              <p style="text-align:center;color:white;" > 
                 <br />
                  <asp:Button ID="btnShow" runat="server" Style="font-weight: 600; border-color:gray;" OnClick="btnShow_Click"
                    Text=".SHOW EXPORT DATA." CssClass="btn btn-primary" />  
                  <br />
                  <br />
                     <asp:Button Text="Export Excel" ID="btnExport" runat="server" OnClick="btnExport_Click"
                    Style="font-weight: 600; border-color:gray;" CssClass="btn btn-primary" />          
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
        <div class="card-body">
          <asp:DataGrid runat="server" ID="dg1" HeaderStyle-backcolor="#4b6c9e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-BorderColor="White" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" AlternatingItemStyle-BackColor="#ccffff" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke" Width="100%" CssClass="jsgrid">
    </asp:DataGrid>
        </div>
        <!-- /.card-body -->
        <div class="card-footer">
          <%--Report Menu--%>
        </div>
        <!-- /.card-footer-->
      </div>
    
</asp:Content>

