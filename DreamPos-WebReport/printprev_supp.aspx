<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="printprev_supp.aspx.vb" Inherits="DreamPos_WebReport.printprev_supp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pdf Preview</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background:#f5f5f5; padding:20px; }
        .preview-container {
            background:white;
            padding:30px;
            border-radius:8px;
            box-shadow:0 4px 12px rgba(0,0,0,0.1);
            margin-bottom:20px;
        }
        .btn-bottom {
            position:fixed;
            bottom:0;
            left:0;
            right:0;
            padding:15px;
            background:#343a40;
            text-align:center;
            z-index:1000;
        }
    </style>
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
     <%--<div align="center" style="background: #b6b7bc;">
         <div align="center" style="height: 100%; width:950px; overflow: hidden;border-style: ridge;background-color: #fff; padding-top: 50px;
            padding-left: 65px; padding-right: 65px;padding-bottom: 40px;" runat="server" id = "grid">
                    
                </div>
                <asp:Button Height="30px" ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" Font-Bold="True" /> 
         <asp:Button Height="30px" ID="btnpdf" runat="server" Text="Export PDF" OnClick="btnpdf_Click" Font-Bold="True"/>
       <asp:Button ID="btnemail" runat="server" Text="Attach to Email" 
    CssClass="btn btn-info" OnClick="btnemail_Click" UseSubmitBehavior="false" />
        
    </div>--%>
        <div class="container preview-container" align="center" style="background: #b6b7bc;">
          <%--  <asp:Literal ID="litPreview" runat="server"></asp:Literal>--%>
         
             <div align="center" style="height: 100%; width:950px; overflow: hidden;border-style: ridge;background-color: #fff; padding-top: 50px;
    padding-left: 65px; padding-right: 65px;padding-bottom: 40px;" id="grid" runat="server" class="preview-panel">
        </div>
            </div>
        <!-- Tombol tetap di bawah (fixed) -->
        <div class="btn-bottom">
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-secondary" OnClick="btnclose_Click" />
            <asp:Button ID="btnpdf" runat="server" Text="Export PDF" CssClass="btn btn-primary mx-2" OnClick="btnpdf_Click" />
            <asp:Button ID="btnemail" runat="server" Text="Attach to Email" CssClass="btn btn-success" OnClick="btnemail_Click" />
        </div>
       <!-- Modal Kirim Email -->
<div class="modal fade" id="emailModal" tabindex="-1">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title">Kirim Purchase Order via Email</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
            </div>

            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>To (bisa lebih dari satu, pisahkan dengan koma atau titik koma)</label>
                            <asp:TextBox ID="txtEmailTo" runat="server" CssClass="form-control" placeholder="email1@supplier.com, email2@supplier.com"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Subject</label>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" 
                                Text="Purchase Order #RC11/25-00585"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Message (Silakan diedit)</label>
                            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"
                                Text="Yth. Bapak/Ibu Supplier,%0A%0ABerikut kami lampirkan Purchase Order resmi dari Mahawirya Makmur Sentosa.%0A%0AMohon segera diproses dan dikonfirmasi penerimaannya.%0A%0ATerima kasih atas kerja samanya.%0A%0AHormat kami,%0A%0A[ Nama Pengirim ]%0AMahawirya Makmur Sentosa"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <asp:Button ID="btnSendEmail" runat="server" Text="Kirim Email" 
                    CssClass="btn btn-success btn-lg px-4" OnClick="btnSendEmail_Click" />
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Batal</button>
            </div>
        </div>
    </div>
</div>
    </form>
</body>
</html>
