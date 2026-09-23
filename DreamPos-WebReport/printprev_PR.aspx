<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="printprev_PR.aspx.vb" Inherits="DreamPos_WebReport.printprev_PR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Request Preview</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f5f5f5; padding: 20px; padding-bottom: 80px; }
        .preview-container {
            background: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            margin-bottom: 20px;
        }
        .btn-bottom {
            position: fixed;
            bottom: 0;
            left: 0;
            right: 0;
            padding: 15px;
            background: #343a40;
            text-align: center;
            z-index: 1000;
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
        <div class="container preview-container" align="center" style="background: #b6b7bc;">
            <div align="center" style="height: 100%; width:950px; overflow: hidden; border-style: ridge; background-color: #fff;
                 padding-top: 50px; padding-left: 65px; padding-right: 65px; padding-bottom: 40px;" 
                 id="grid" runat="server" class="preview-panel">
            </div>
        </div>

        <!-- Tombol tetap di bawah (fixed) -->
        <div class="btn-bottom">
            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-secondary" OnClick="btnclose_Click" />
            <asp:Button ID="btnpdf" runat="server" Text="Export PDF" CssClass="btn btn-primary mx-2" OnClick="btnpdf_Click" />
            <asp:Button ID="btnemail" runat="server" Text="Attach to Email" CssClass="btn btn-success" OnClick="btnemail_Click" />
        </div>

        <!-- Modal Kirim Email -->
        <div class="modal fade" id="emailModal" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title">Kirim Purchase Request via Email</h5>
                        <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label><strong>To</strong> (bisa lebih dari satu, pisahkan dengan koma atau titik koma)</label>
                                    <asp:TextBox ID="txtEmailTo" runat="server" CssClass="form-control" placeholder="email1@ho.com, email2@ho.com"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label><strong>Subject</strong></label>
                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label><strong>Message</strong> (Silakan diedit)</label>
                                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
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
