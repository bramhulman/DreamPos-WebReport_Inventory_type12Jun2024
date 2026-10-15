<%@ Page Language="VB" EnableSessionState="False" %>
<script runat="server">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Response.StatusCode = If(Request.QueryString("status") = "404", 404, 500)
        Response.TrySkipIisCustomErrors = True
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()
    End Sub
</script>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="robots" content="noindex, nofollow" />
    <title>Unable to open page | Inventory Cloud</title>
    <style>
        * { box-sizing: border-box; }
        body { margin: 0; background: #f5f7fa; color: #253442; font: 16px/1.6 Arial, sans-serif; }
        header { padding: 24px; border-bottom: 1px solid #dde3e9; background: #fff; color: #176eae; font-weight: 700; }
        main { max-width: 640px; margin: 0 auto; padding: 80px 24px; }
        .marker { width: 56px; height: 56px; display: grid; place-items: center; background: #fff1d6; color: #805400; border-radius: 8px; font-size: 30px; font-weight: 700; }
        h1 { margin: 24px 0 16px; font-size: 32px; line-height: 1.25; overflow-wrap: anywhere; }
        p { margin: 0 0 16px; color: #536471; }
        .actions { margin-top: 32px; }
        a { display: inline-flex; align-items: center; justify-content: center; min-height: 48px; padding: 12px 24px; border-radius: 6px; background: #0874bb; color: white; text-decoration: none; font-weight: 700; }
        a:hover { background: #055b94; }
        a:focus-visible { outline: 3px solid #ad7200; outline-offset: 4px; }
        .note { margin-top: 32px; font-size: 14px; }
        @media (max-width: 575px) { main { padding-top: 48px; } h1 { font-size: 28px; } a { width: 100%; } }
    </style>
</head>
<body>
    <header>Inventory Cloud</header>
    <main>
        <div class="marker" aria-hidden="true">!</div>
        <h1>We couldn't open this page.</h1>
        <p>The page may be unavailable, or something went wrong while processing your request.</p>
        <p>If you were saving changes, check their status before trying again.</p>
        <div class="actions"><a href="<%= ResolveUrl("~/Inventoryhome.aspx") %>">Back to Home</a></div>
        <p class="note">If the problem continues, please contact your administrator.</p>
    </main>
</body>
</html>

