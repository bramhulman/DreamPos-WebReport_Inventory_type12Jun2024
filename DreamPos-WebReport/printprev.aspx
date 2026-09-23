<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="printprev.aspx.vb" Inherits="DreamPos_WebReport.printprev" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
<link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
    <script type="text/javascript">
    function printDiv() {
        var printContents = document.getElementById("grid").innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }
</script>
</head>
<body>
    
    <form id="form1" runat="server">
       
     <div align="center" style="background: #b6b7bc;">
         <div align="center" style="height: 100%; width:950px; overflow: hidden;border-style: ridge;background-color: #fff; padding-top: 50px;padding-left: 65px; padding-right: 65px;padding-bottom: 40px;" runat="server" id="grid">
                    
                </div>
                <asp:Button Height="30px" ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" Font-Bold="True" /> 
         
         <%--<asp:Button Height="30px" ID="btnpdf" runat="server" Text="Export PDF" OnClientClick="printDiv()" Font-Bold="True"/>--%>  
          <asp:Button Height="30px" ID="btnpdf" runat="server" Text="Export PDF" OnClick="btnpdf_Click" Font-Bold="True"/> 
    </div>
    </form>
</body>
</html>
