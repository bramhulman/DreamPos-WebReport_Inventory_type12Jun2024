<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="SiteLogin.Master" CodeBehind="Default.aspx.vb"
  Inherits="DreamPos_WebReport._Default" %>
  <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  </asp:Content>
  <asp:Content ID="Content2" ContentPlaceHolderID="LoginBody" runat="server">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <%--<style>.input-group>.form-control{ width: 800px; }</style>--%>
      <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');

        :root {
          /* Original Blue Theme with subtle accent */
          --brand-color: #04274f;
          /* Original Deep Blue */
          --brand-light: #063d7a;
          /* Original Lighter Blue */
          --accent-yellow: #fbc02d;
          /* Retaining Golden Yellow for subtle border */
          --bg-gradient: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
        }

        body {
          font-family: 'Poppins', sans-serif !important;
          background: var(--bg-gradient);
          height: 100vh;
        }

        /* Override AdminLTE login page background */
        body.login-page {
          background: var(--bg-gradient);
          align-items: center;
          display: flex;
          justify-content: center;
        }

        .login-box {
          width: 400px;
          /* Reference AdminLTE default is 360px, slightly wider is nicer */
        }

        .card {
          border: none;
          border-radius: 12px;
          box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
          overflow: hidden;
          border-top: 5px solid var(--accent-yellow);
          /* Golden Yellow Accent */
        }

        .card-primary.card-outline {
          border-top: none;
        }

        .card-header {
          background-color: white;
          /* Changed to white to let logo shine */
          color: var(--brand-color);
          text-align: center;
          padding: 20px;
          border-bottom: 1px solid #f0f0f0;
        }

        .card-body.login-card-body {
          padding: 30px 40px;
          border-radius: 12px;
        }

        .login-logo {
          margin-bottom: 20px;
        }

        .login-logo a {
          color: var(--brand-color) !important;
          font-weight: 700;
          /* DreamPos bold */
          font-size: 2rem;
          text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
          text-decoration: none;
        }

        .login-logo span.light {
          font-weight: 300;
          /* Inventory Cloud lighter */
          color: #6c757d;
          /* Grey color for contrast */
        }

        .login-box-msg {
          color: #777;
          /* Lighter text for "Sign in to..." */
          font-weight: 400;
        }

        .form-control {
          border-radius: 8px;
          border: 1px solid #e0e0e0;
          /* Lighter border */
          padding: 20px 15px;
          height: auto;
          font-size: 14px;
          transition: all 0.3s ease;
          color: #555;
          /* Lighter text inside inputs */
        }

        .form-control:focus {
          border-color: var(--brand-color);
          box-shadow: 0 0 0 0.2rem rgba(4, 39, 79, 0.15);
          /* Blue Focus Glow */
        }

        .input-group-text {
          background-color: transparent;
          border-left: none;
          border-radius: 0 8px 8px 0;
          border-color: #e0e0e0;
          color: #999;
        }

        /* Input group tweaks to make rounded corners work with append */
        .input-group .form-control {
          border-right: none;
        }

        .input-group:focus-within .input-group-text {
          border-color: var(--brand-color);
          color: var(--brand-color);
        }

        .btn-primary {
          background-color: var(--brand-color);
          border: none;
          color: white;
          border-radius: 8px;
          padding: 10px;
          font-weight: 600;
          letter-spacing: 0.5px;
          transition: all 0.3s ease;
          box-shadow: 0 4px 6px rgba(4, 39, 79, 0.2);
        }

        .btn-primary:hover {
          background-color: var(--brand-light);
          transform: translateY(-1px);
          box-shadow: 0 6px 8px rgba(4, 39, 79, 0.3);
        }

        .icheck-primary>input:first-child:checked+input[type="hidden"]+label::before,
        .icheck-primary>input:first-child:checked+label::before {
          background-color: var(--brand-color);
          border-color: var(--brand-color);
          border-radius: 4px;
          /* Soften checkbox too */
        }

        /* Loading Overlay */
        .loading {
          font-family: 'Poppins', sans-serif;
          font-size: 14px;
          font-weight: 600;
          border: none;
          border-radius: 10px;
          width: 250px;
          height: auto;
          padding: 20px;
          display: none;
          position: fixed;
          background-color: white;
          box-shadow: 0 0 20px rgba(0, 0, 0, 0.2);
          z-index: 999;
          text-align: center;
          color: var(--brand-color);
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
      </script>
      <div class="login-box">
        <div class="login-logo">
          <a href="#"><b>DreamPos</b> <span class="light">Inventory Cloud</span></a>
        </div>
        <!-- /.login-logo -->
        <div class="card">
          <div class="card-body login-card-body">
            <p class="login-box-msg">Sign In to Start Your Session</p>

            <asp:Panel runat="server" DefaultButton="btnSubmit">
              <div class="input-group mb-3">
                <asp:textbox ID="txtUser" runat="server" class="form-control" placeholder="Username"> </asp:textbox>
                <div class="input-group-append">
                  <div class="input-group-text">
                    <span class="fas fa-envelope"></span>
                  </div>
                </div>
              </div>
              <div class="input-group mb-3">
                <asp:textbox ID="txtPasword" runat="server" class="form-control" placeholder="Password"
                  TextMode="Password"> </asp:textbox>
                <div class="input-group-append">
                  <div class="input-group-text">
                    <span class="fas fa-lock"></span>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-8">
                  <div class="icheck-primary">
                    <asp:CheckBox runat="server" ID="RememberMe" />
                    <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                  </div>
                </div>
                <!-- /.col -->
                <div class="col-4">
                  <asp:button ID="btnSubmit" UseSubmitBehavior="true" OnClick="LogIn" runat="server"
                    class="btn btn-primary btn-block" CausesValidation="false" Text="Sign In"></asp:button>
                </div>
                <!-- /.col -->
              </div>
            </asp:Panel>


            <div class="social-auth-links text-center mb-3">
              <%-- <p>- OR -</p>
                <a href="#" class="btn btn-block btn-primary">
                  <i class="fab fa-facebook mr-2"></i> Sign in using Facebook
                </a>
                <a href="#" class="btn btn-block btn-danger">
                  <i class="fab fa-google-plus mr-2"></i> Sign in using Google+
                </a>--%>
            </div>
            <!-- /.social-auth-links -->

            <%--<p class="mb-1">
              <a href="forgot-password.html">I forgot my password</a>
              </p>--%>
              <%--<p class="mb-0">
                <a href="register.html" class="text-center">Register a new membership</a>
                </p>--%>
          </div>
          <!-- /.login-card-body -->
        </div>
        <div class="loading" style="text-align:center">
          Loading... Please wait...<br />
          <br />
          <img src="/images/loader.gif" alt="" />
        </div>
      </div>
      <!-- /.login-box -->

      <!-- jQuery -->
      <script src="../../plugins/jquery/jquery.min.js"></script>
      <!-- Bootstrap 4 -->
      <script src="../../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
      <!-- AdminLTE App -->
      <script src="../../dist/js/adminlte.min.js"></script>

  </asp:Content>