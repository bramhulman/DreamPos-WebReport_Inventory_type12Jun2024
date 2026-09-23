Public Class ClsLogin
    Private mfunction As ModFunction

    Public Function getLogin(ByVal name As String, ByVal pas As String) As DataSet

        Dim ds As New DataSet
        Dim cLogin As New ClsLogin

        Try

            Dim a As New clsDbo("Select * from cld_user where usr = '" & name & "' and psw = '" & pas & "'")
            ds = a.selectDataset
            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "clsWebService:getLogin")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function getChangePassword(ByVal name As String, ByVal pas As String, ByVal id As Integer) As Boolean
        Dim bHasil As Boolean = False

        Try
            Dim a As New clsDbo("")
            a.executeNonQuery("Update cld_user set psw = '" & pas & "' where usr ='" & name & "' And custid =" & id)
            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "clslogin:getChangePassword")
            Err.Clear()
        End Try

        Return bHasil
    End Function


End Class
