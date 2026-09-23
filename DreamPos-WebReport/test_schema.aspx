<%@ Page Language="VB" AutoEventWireup="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="MySql.Data.MySqlClient" %>
<script runat="server">
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MyConnectionString").ConnectionString
        If connStr = "" Then connStr = "server=localhost;user id=root;password=;database=pos_testing"
        Dim result As String = ""
        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()
                Using cmd As New MySqlCommand("DESCRIBE cld_inv_prd", conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            result &= reader("Field").ToString() & "|" & reader("Type").ToString() & "<br/>"
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            result = ex.Message
        End Try
        System.IO.File.WriteAllText(Server.MapPath("~/schema_out.txt"), result.Replace("<br/>", vbCrLf))
        Response.Write("Done")
    End Sub
</script>
