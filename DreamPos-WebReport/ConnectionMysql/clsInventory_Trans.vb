Imports MySql.Data.MySqlClient
Public Class clsInventory_Trans

    Private cRcv As New clsInventory_Receive
    Private cRw As New clsInventory_Rwaste
    Private cTrf As New clsInventory_Transfer
    Private cWare As New clsInventory_Warehouse

    Public Function idRcv(ByVal custid As Integer, Optional ByVal try2 As Integer = 0) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cRcv.SelectDataset_longId(custid)
        Dim test As String = Val(Mid(idtrans, 9, 13))
        Dim ids As String = Val(Mid(idtrans, 9, 13)) + 1
        If try2 > 0 Then
            ids = ids + try2
        End If
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "RC" & mon & "/" & yer & "-" & awal

        Else
            If ids.Length = 1 Then
                str = "RC" & mon & "/" & yer & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "RC" & mon & "/" & yer & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "RC" & mon & "/" & yer & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "RC" & mon & "/" & yer & "-0" & ids
            ElseIf ids.Length = 5 Then

                str = "RC" & mon & "/" & yer & "-" & ids

            End If
        End If

        Return str

    End Function


    Public Function idRWaste(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cRw.SelectDataset_longId(custid)
        Dim test As String = Val(Mid(idtrans, 9, 13))
        Dim ids As String = Val(Mid(idtrans, 9, 13)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "RW" & mon & "/" & yer & "-" & awal

        Else
            If ids.Length = 1 Then
                str = "RW" & mon & "/" & yer & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "RW" & mon & "/" & yer & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "RW" & mon & "/" & yer & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "RW" & mon & "/" & yer & "-0" & ids
            ElseIf ids.Length = 5 Then

                str = "RW" & mon & "/" & yer & "-" & ids

            End If
        End If

        Return str

    End Function

    Public Function idCWaste(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cRw.SelectDataset_longId2(custid)
        Dim test As String = Val(Mid(idtrans, 9, 13))
        Dim ids As String = Val(Mid(idtrans, 9, 13)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "CW" & mon & "/" & yer & "-" & awal

        Else
            If ids.Length = 1 Then
                str = "CW" & mon & "/" & yer & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "CW" & mon & "/" & yer & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "CW" & mon & "/" & yer & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "CW" & mon & "/" & yer & "-0" & ids
            ElseIf ids.Length = 5 Then

                str = "CW" & mon & "/" & yer & "-" & ids

            End If
        End If

        Return str

    End Function


    Public Function idtransfer(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = ctrf.SelectDataset_longId(custid)
        Dim test As String = Val(Mid(idtrans, 8, 12))
        Dim ids As String = Val(Mid(idtrans, 8, 12)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "T" & mon & "/" & yer & "-" & awal

        Else
            If ids.Length = 1 Then
                str = "T" & mon & "/" & yer & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "T" & mon & "/" & yer & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "T" & mon & "/" & yer & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "T" & mon & "/" & yer & "-0" & ids
            ElseIf ids.Length = 5 Then

                str = "T" & mon & "/" & yer & "-" & ids

            End If
        End If

        Return str

    End Function

    Public Function idrtntoWH(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cTrf.SelectDataset_longIdTW(custid)
        Dim test As String = Val(Mid(idtrans, 9, 13))
        Dim ids As String = Val(Mid(idtrans, 9, 13)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "TW" & mon & "/" & yer & "-" & awal

        Else
            If ids.Length = 1 Then
                str = "TW" & mon & "/" & yer & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "TW" & mon & "/" & yer & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "TW" & mon & "/" & yer & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "TW" & mon & "/" & yer & "-0" & ids
            ElseIf ids.Length = 5 Then

                str = "TW" & mon & "/" & yer & "-" & ids

            End If
        End If

        Return str

    End Function

    Public Function idwarehouse(ByVal custid As Integer, ByVal icc As Integer, Optional ByVal try2 As Integer = 0) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cWare.SelectDataset_longId(custid, icc)


        Dim ids As String = Val(Mid(idtrans, 13, 17)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If try2 > 0 Then
            ids = ids + try2
        End If

        Dim cc As String = icc

        If mon.Length = 1 Then
            mon = "0" & mon
        End If

        If cc.Length = 1 Then
            cc = "00" & cc
        ElseIf cc.Length = 2 Then
            cc = "0" & cc
        End If

        If Date.Now.Day = 1 And idtrans = "" Then
            str = "OW" & mon & "/" & yer & "-" & cc & "-" & awal
        Else
            If ids.Length = 1 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-0" & ids
            ElseIf ids.Length = 5 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-" & ids
            End If
        End If

        Return str

    End Function

    Public Function idwarehouse(ByVal custid As Integer, ByVal icc As Integer, ByRef trans As MySqlTransaction, Optional ByVal try2 As Integer = 0) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cWare.SelectDataset_longId(custid, icc, trans)


        Dim ids As String = Val(Mid(idtrans, 13, 17)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If try2 > 0 Then
            ids = ids + try2
        End If

        Dim cc As String = icc

        If mon.Length = 1 Then
            mon = "0" & mon
        End If

        If cc.Length = 1 Then
            cc = "00" & cc
        ElseIf cc.Length = 2 Then
            cc = "0" & cc
        End If

        If Date.Now.Day = 1 And idtrans = "" Then
            str = "OW" & mon & "/" & yer & "-" & cc & "-" & awal
        Else
            If ids.Length = 1 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-0" & ids
            ElseIf ids.Length = 5 Then
                str = "OW" & mon & "/" & yer & "-" & cc & "-" & ids
            End If
        End If

        Return str

    End Function
    Public Function idwarehouseDel(ByVal custid As Integer, ByVal iWare As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cWare.SelectDataset_longDelId(custid, iWare)

        Dim ids As String = Val(Mid(idtrans, 13, 17)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        Dim cc As String = iWare

        If mon.Length = 1 Then
            mon = "0" & mon
        End If

        If cc.Length = 1 Then
            cc = "00" & cc
        ElseIf cc.Length = 2 Then
            cc = "0" & cc
        End If

        If Date.Now.Day = 1 And idtrans = "" Then
            str = "DL" & mon & "/" & yer & "-" & cc & "-" & awal
        Else
            If ids.Length = 1 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-0" & ids
            ElseIf ids.Length = 5 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-" & ids
            End If
        End If

        Return str

    End Function

    Public Function idwarehouseDel(ByVal custid As Integer, ByVal iWare As Integer, ByRef trans As MySqlTransaction) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cWare.SelectDataset_longDelId(custid, iWare, trans)

        Dim ids As String = Val(Mid(idtrans, 13, 17)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        Dim cc As String = iWare

        If mon.Length = 1 Then
            mon = "0" & mon
        End If

        If cc.Length = 1 Then
            cc = "00" & cc
        ElseIf cc.Length = 2 Then
            cc = "0" & cc
        End If

        If Date.Now.Day = 1 And idtrans = "" Then
            str = "DL" & mon & "/" & yer & "-" & cc & "-" & awal
        Else
            If ids.Length = 1 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-0" & ids
            ElseIf ids.Length = 5 Then
                str = "DL" & mon & "/" & yer & "-" & cc & "-" & ids
            End If
        End If

        Return str

    End Function
End Class
