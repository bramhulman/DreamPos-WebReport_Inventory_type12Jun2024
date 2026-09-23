Public Class Cls_ITrans
    Private ctrf As New Cls_ITrf
    Private cRw As New Cls_Rwaste
    Private cRcv As New Cls_IRcv
    Private CRtn As New Cls_IReturn
    Private CExO As New Cls_ExOther
    Private cWare As New Cls_IWarehouse

    Public Function SelectDataset_invtrans(ByVal linkid As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * From cld_inv_trans where linkid = '" & linkid & "' and type = 1")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    'perubahan 12 oct 2017, ini untuk detail inv transfer
    Public Function SelectDataTable_invtrans_Transfer(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""

        'Perbaikan 12 oct 2017
        '- ambil stocknya dari ccFrom, awalnya dari ccTO

        'Dim a As New clsDbo("SELECT A.*, IFNULL(B.ttlQty, 0) AS Stock  FROM cld_inv_trans A LEFT JOIN cld_inv_transstock B ON B.stkId = A.Stk_Id  AND B.ccid = A.Cc  WHERE A.linkid = '" & linkid & "' and A.type = 1 and A.custid = " & custid & "")
        Dim a As New clsDbo("SELECT A.*, IFNULL(B.ttlQty, 0) AS Stock  FROM cld_inv_trans A INNER JOIN cld_invlk_trf c ON a.CustID = c.CustID AND a.Linkid = c.id " & _
                            "LEFT JOIN cld_inv_transstock B ON B.stkId = A.Stk_Id  AND B.ccid = c.ccFrom AND b.custId = c.CustID " & _
                            "WHERE A.linkid = '" & linkid & "' and A.type = 1 and A.custid = " & custid & "")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

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
    Public Function idExOther(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "1"
        Dim idtrans As String = CExO.SelectDataset_longId(custid)

        Dim ids As String = Val(Mid(idtrans, 7, 2)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If ids.Length = 1 Then
            ids = "0" & ids
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = mon & "/" & yer & "-" & awal

        Else

            str = mon & "/" & yer & "-" & ids


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

    Public Function idRcv(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cRcv.SelectDataset_longId(custid)
        Dim test As String = Val(Mid(idtrans, 9, 13))
        Dim ids As String = Val(Mid(idtrans, 9, 13)) + 1
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
    Public Function idReturn(ByVal custid As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cRtn.SelectDataset_longId(custid)
        Dim test As String = Val(Mid(idtrans, 9, 13))
        Dim ids As String = Val(Mid(idtrans, 9, 13)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "RT" & mon & "/" & yer & "-" & awal

        Else
            If ids.Length = 1 Then
                str = "RT" & mon & "/" & yer & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "RT" & mon & "/" & yer & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "RT" & mon & "/" & yer & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "RT" & mon & "/" & yer & "-0" & ids
            ElseIf ids.Length = 5 Then

                str = "RT" & mon & "/" & yer & "-" & ids

            End If
        End If

        Return str

    End Function

    Public Function idwarehouse(ByVal custid As Integer, ByVal icc As Integer) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = cWare.SelectDataset_longId(custid, icc)

        Dim ids As String = Val(Mid(idtrans, 13, 17)) + 1
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

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


End Class
