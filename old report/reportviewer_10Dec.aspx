<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteReport.Master" CodeBehind="reportviewer.aspx.vb" Inherits="DreamPos_WebReport.reportviewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            border-collapse: collapse;
            width: 100%;
            height: 42px;
            margin-bottom: 20px;
        }
    </style>
     <script type="text/javascript">
         $(document).ready(function () {
             $('.btnstyle').click(function () {
                 $('#spinner').show();
             });
         });
         function checkTextAreaMaxLength(textBox, e, length) {

             var mLen = textBox["MaxLength"];
             if (null == mLen)
                 mLen = length;

             var maxLength = parseInt(mLen);
             if (!checkSpecialKeys(e)) {
                 if (textBox.value.length > maxLength - 1) {
                     if (window.event)//IE
                         e.returnValue = false;
                     else//Firefox
                         e.preventDefault();
                 }
             }
         }
         function checkSpecialKeys(e) {
             if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                 return false;
             else
                 return true;
         }
         function checkDate(sender, args) {
             if (sender._selectedDate > new Date()) {
                 alert("You cannot select a day greater than today!");
                 sender._selectedDate = new Date();
                 // set the date back to the current date
                 sender._textbox.set_Value(sender._selectedDate.format(sender._format))
             }
         }
         function pageLoad() {

             $addHandler(document,

'keydown', onKeypress);

         }

         function onKeypress(args) {

             if (args.keyCode == Sys.UI.Key.esc) {

                 var mdl = [$find('BehaviorID'), $find('BehaviorID2'), $find('BehaviorID3'), $find('BehaviorID4')];
                 for (var i = 0; i < mdl.length; i++) {
                     mdl[i].hide();
                 }
                 

             }

         }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftbarContent" runat="server">

      <%-- Modal Pop Up 1 Untuk Store --%>   
     <ajaxtoolkit:modalpopupextender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnMember"
        CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </ajaxtoolkit:modalpopupextender>
   <div class="col-md-16" style="text-align:center" >
       <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
       <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="left" Style="display: none;" >
       
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                            <asp:Button Text="Search" ID="btnSearch" runat="server"
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
                                <b style="color: #0a0a0a">nama Store</b>
                            </td>
                             <td class="style1">
                                <b style="color: #0a0a0a">Kode 2</b>
                            </td>

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
                                  <td>
                                    <asp:Label ID="lblBatch" ForeColor="black" runat="server" Text=' <%# Eval("kode2")%>'></asp:Label>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>

                       
                    </table>
                </div>
                 
            </ContentTemplate>
        </asp:UpdatePanel>
        
         <asp:Button ID="btnclose" runat="server" Text="Close" Style="color: black;" /> 
         <asp:Button ID="btnSubmit" runat="server" Text="Select" OnClick="btnSubmit_Click" Style="color: black;" /> 
          <asp:Button ID="btnall" runat="server" Text="All Store" OnClick="btnall_Click"  Style="color: black;" /> 
    </asp:Panel>
       </div>

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
                            <asp:Button Text="Search" ID="btnSearch2" runat="server"
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
                            <td class="style1">
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
                                <td >
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
         <asp:Button ID="btnSubmit2" runat="server" Text="Select" OnClick="btnSubmit2_Click"  Style="color: black;"/> 
          <asp:Button ID="btnAll2" runat="server" Text="All Brand" OnClick="btnall2_Click" Style="color: black;" /> 
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
                            <asp:Button Text="Search" ID="Button1" runat="server"
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
         <asp:Button ID="btnSubmit3" runat="server" Text="Select" OnClick="btnSubmit3_Click" Style="color: black;" /> 
          <asp:Button ID="btnAll3" runat="server" Text="All Brand" OnClick="btnall3_Click" Style="color: black;" /> 
    </asp:Panel>
         </div>


    <div style="color:black; margin:0 auto;">
        
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        
        <div class="form-group" style="margin-top:10px;">
            <asp:Label runat="server" AssociatedControlID="ddlRpt" CssClass="control-label">Export</asp:Label>
            <div class="col-md-16" style="text-align:center;">
                <asp:DropDownList ID="ddlRpt" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlRpt_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled">
                </asp:DropDownList>
               
                               
            </div>
        </div>

       <%-- <div class="form-group">
                
                <asp:Label runat="server" CssClass="control-label">Parameter</asp:Label>
               
             </div>--%>
       <div class="form-group">
            <asp:Label ID="lblOwn" Width="20%" runat="server" AssociatedControlID="ddlOwn" CssClass="control-label">Own</asp:Label>
         
               <asp:DropDownList ID="ddlOwn" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlRpt_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="True"
                    EnableViewState="True" ViewStateMode="Enabled"></asp:DropDownList>
                 
             
        </div>

       <%--  
        <div class="form-group">      
            <asp:Label ID="Label12" runat="server" Text="Brand" Textalign ="Centre"></asp:Label>
            
            <div class="col-md-16" style="text-align:center;">
            <asp:Label ID="Label13" runat="server" Text="All Brand" Textalign = "Centre"></asp:Label>                          
            </div> 

            <div class="col-md-16" style="text-align:center;">
               <asp:Button ID="Button1" runat="server" Text="GetBrand" 
                             Width="120px" Height="20px" />                         
            </div>
        </div> --%>

         <%-- Untuk PopUp Brand --%>
        <div class="form-group">  
            <table style=" text-align:left; width:100%;padding:inherit;">
                <tr>
                    <td>
                        <asp:Label ID="lblBrand" runat="server" Text="Brand" Textalign ="Centre"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNmBrand" runat="server" Text="All Brand" Textalign = "Centre"></asp:Label>                          
                    </td>
                </tr>
            </table>    
            
            
          <%--  <div class="col-md-16" style="text-align:center;">
            
            </div> --%>

            <div class="col-md-16" style="text-align:center;">
               <asp:Button ID="BtnBrand" runat="server" Text="GetBrand" 
                             Style="font-weight: 600; border-color:gray;" CssClass="btn btn-default" />                         
            </div>
        </div>

         <%-- Untuk PopUp City --%>
        <div class="form-group">  
            <table style=" text-align:left; width:100%;padding:inherit;">
                <tr>
                    <td>
                        <asp:Label ID="lblCity" runat="server" Text="City" Textalign ="Centre"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNmCity" runat="server" Text="All City" Textalign = "Centre"></asp:Label>   
                    </td>
                    </tr>
                </table>    
            

            <div class="col-md-16" style="text-align:center;">
                <asp:Button ID="btnCity" runat="server" Text="Get City" 
                            Style="font-weight: 600; border-color:gray;" CssClass="btn btn-default"/>                            
            </div>
        </div>


        <%-- Untuk PopUp Store --%>
        <div class="form-group">
             <table style=" text-align:left; width:100%;padding:inherit;">
                <tr>
                    <td>
                         <asp:Label ID="lblOutletName" runat="server" Text="Store" Textalign ="Centre"></asp:Label> 
                    </td>
                    <td>
                        <asp:Label ID="lblOutletNameData" runat="server" Text="All Store" Textalign = "Centre"></asp:Label>    
                    </td>
                    </tr>
                 </table>

           
           
            <div class="col-md-16" style="text-align:center;">
                <asp:Button ID="btnMember" runat="server" Text="Get Store" 
                             Style="font-weight: 600; border-color:gray;" CssClass="btn btn-default" />                              
            </div>
        </div>

         <div class="form-group">
            <asp:Label ID="lblFrom" Width="20%" runat="server" AssociatedControlID="txtFrom" CssClass="control-label">From</asp:Label>
         
                <asp:TextBox Width="75%" style="text-align:center;" runat="server" ID="txtFrom" CssClass="form-inline" />
                 <ajaxtoolkit:CalendarExtender ID="cleFrom" runat="server" TargetControlID="txtFrom" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                 
             
        </div>

        

         <div class="form-group">
            <asp:Label ID="lblTo" Width="20%" runat="server" AssociatedControlID="txtTo" CssClass="control-label">To</asp:Label>
         
                <asp:TextBox Width="75%" style="text-align:center;" runat="server" ID="txtTo" CssClass="form-inline" />
                <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTo" 
                            Format="dd MMM yyyy"></ajaxtoolkit:CalendarExtender>
                 
                
        </div>
          
        <div class="form-group">
            <div class="col-md-16" style="text-align:center;">
             <asp:Button ID="btnShow" OnClick="btnShow_Click" runat="server" Style="font-weight: 600; border-color:gray;"
                    Text=".SHOW EXPORT DATA." CssClass="btn btn-default" />
                </div>
        </div>
        <div class="form-group">
            <div class="col-md-16" style="text-align:center;">
               
                <asp:Button Text="Export Excel" ID="btnExport" runat="server" OnClick="btnExport_Click"
                    Style="font-weight: 600; border-color:gray;" CssClass="btn btn-default" />
            </div>
        </div>
            
    </div>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <div id="spinner" class="modal" style="display: none">
        <div class="center">
            <img alt="" src="/images/loader.gif" />
        </div>
    </div>
    <div class="col-md-14">
               <div style="width: 100%;height:auto;text-align:center; overflow-y:auto">
     <div class="table-responsive">
            <table id="tblEmpDetails" class="table table-bordered" style="background-color:aliceblue">
               <thead style="background-color:#4b6c9e; color:#fff; text-align:center">
                   <tr>
                       <th>
                    <b><asp:Label ID="lbl1" runat="server" Text=''></asp:Label></b>
                   </th>   
                     <th>
                    <b><asp:Label ID="lbl2" runat="server" Text=''></asp:Label></b>
                   </th>
                    <th>
                    <b><asp:Label ID="lbl3" runat="server" Text=''></asp:Label></b>
                   </th>
                   <th>
                    <b><asp:Label ID="lbl4" runat="server" Text=''></asp:Label></b>
                   </th>
                        <th>
                    <b><asp:Label ID="lbl5" runat="server" Text=''></asp:Label></b>
                   </th>
                        <th>
                    <b><asp:Label ID="lbl6" runat="server" Text=''></asp:Label></b>
                   </th>      
                       <th>
                    <b><asp:Label ID="lbl7" runat="server" Text=''></asp:Label></b>
                   </th>    
                       <th>
                    <b><asp:Label ID="lbl8" runat="server" Text=''></asp:Label></b>
                   </th>    
                       <th>
                    <b><asp:Label ID="lbl9" runat="server" Text=''></asp:Label></b>
                   </th>    
                       <th>
                    <b><asp:Label ID="lbl10" runat="server" Text=''></asp:Label></b>
                   </th>    
                       <th>
                    <b><asp:Label ID="lbl11" runat="server" Text=''></asp:Label></b>
                   </th>        
                   </tr>
               </thead>
            
            <asp:ListView ID="lstViewReport" runat="server"
                OnPagePropertiesChanging="OnPagePropertiesChanging">
                <ItemTemplate>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=' <%# Eval("l1", "{0:dd MM yyyy}")%>'></asp:Label>
                       </td>
                     <td>
                        <asp:Label ID="Label2" runat="server" Text=' <%# Eval("l2")%>'></asp:Label>
                       </td>
                     <td>
                        <asp:Label ID="Label3" runat="server" Text=' <%# Eval("l3")%>'></asp:Label>
                       </td>
                     <td>
                        <asp:Label ID="Label4" runat="server" Text=' <%# Eval("l4")%>'></asp:Label>
                       </td>
                     <td>
                        <asp:Label ID="Label5" runat="server" Text=' <%# Eval("l5")%>'></asp:Label>
                       </td>
                     <td>
                        <asp:Label ID="Label6" runat="server" Text=' <%# Eval("l6")%>'></asp:Label>
                          </td>
                          <td>
                        <asp:Label ID="Label7" runat="server" Text=' <%# Eval("l7")%>'></asp:Label>
                       </td>
                          <td>
                        <asp:Label ID="Label8" runat="server" Text=' <%# Eval("l8")%>'></asp:Label>
                       </td>
                          <td>
                        <asp:Label ID="Label9" runat="server" Text=' <%# Eval("l9")%>'></asp:Label>
                       </td>
                          <td>
                        <asp:Label ID="Label10" runat="server" Text=' <%# Eval("l10")%>'></asp:Label>
                       </td>
                          <td>
                        <asp:Label ID="Label11" runat="server" Text=' <%# Eval("l11")%>'></asp:Label>
                       </td>
                      
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </table>
   
    <br />
    </div>
                 <%--  <div class="table-responsive">
        <table class="table">
            <tr>
                <td>
                    <asp:Label runat="server" Font-Bold="True">Page: </asp:Label>
                    <asp:DataPager ID="lvDataPager1" runat="server" PagedControlID="lstViewReport" PageSize="15">
                        <Fields>
                            <asp:NumericPagerField ButtonType="Link" />
                        </Fields>
                    </asp:DataPager>

                     
                </td>
            </tr>
        </table>
    </div>--%>
                    <asp:Label ID="lblNoBrand" runat="server" Text="0" Textalign = "Centre" Visible = false></asp:Label>
                   <asp:Label ID="lblNoCity" runat="server" Text="0" Textalign = "Centre" Visible = false></asp:Label>
   <asp:Label ID="lblNoOutlet" runat="server" Text="0" Textalign = "Centre" Visible = false></asp:Label>
                                       </div>
        </div>
    </asp:Content>