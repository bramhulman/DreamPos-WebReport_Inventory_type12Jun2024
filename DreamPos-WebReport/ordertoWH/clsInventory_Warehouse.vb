Imports System
Imports DocumentFormat.OpenXml.Office2010.Excel
Imports MySql.Data.MySqlClient

Public Class clsInventory_Warehouse
    Private mfunction As New ModFunction
    Public Function SelectDataset_longId(ByVal custid As Integer, ByVal cc As Integer) As String

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("SELECT ID from cld_inv_w where MONTH(CreatedDate) = " & Date.Now.Month & " and custid = " & custid & " and ccTo = " & cc & " order by ID desc")

            ds = a.selectDataset

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("ID")
            End If
            'tambahan jika detailnya sudah ada

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_longId")
            Err.Clear()
        End Try

        ds.Dispose()

        Return strFilter

    End Function
    Public Function Selectds_PRId(ByVal cc As Integer, ByRef trans As MySqlTransaction) As String

        Dim strFilter As String = ""

        Try
            Dim cmd As New MySqlCommand("SELECT pr_no from cld_inv_pr where MONTH(pr_date) = " & Date.Now.Month & " AND YEAR(pr_date) = " & Date.Now.Year & " and cc = " & cc & " order by pr_id desc limit 1", trans.Connection, trans)
            Dim da As New MySqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("pr_no")
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:Selectds_PRId")
            Err.Clear()
        End Try

        Return strFilter

    End Function
    Public Function SelectDataset_longId(ByVal custid As Integer, ByVal cc As Integer, ByRef trans As MySqlTransaction) As String

        Dim strFilter As String = ""

        Try
            Dim cmd As New MySqlCommand("SELECT ID from cld_inv_w where MONTH(CreatedDate) = " & Date.Now.Month & " and custid = " & custid & " and ccTo = " & cc & " order by ID desc", trans.Connection, trans)
            Dim da As New MySqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("ID")
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_longId")
            Err.Clear()
        End Try

        Return strFilter

    End Function

    Public Function SelectDataset_longDelId(ByVal custid As Integer, ByVal cc As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("SELECT DelID from cld_inv_w where DelID IS NOT null and MONTH(DelDate) = " & Date.Now.Month & " and custid = " & custid & " and ccFrom = " & cc & " order by DelID desc")

            ds = a.selectDataset

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("DelID")
            End If

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_longDelId")
            Err.Clear()
        End Try

        ds.Dispose()

        Return strFilter

    End Function

    Public Function SelectDataset_longDelId(ByVal custid As Integer, ByVal cc As Integer, ByRef trans As MySqlTransaction) As String
        Dim strFilter As String = ""

        Try
            Dim cmd As New MySqlCommand("SELECT DelID from cld_inv_w where DelID IS NOT null and MONTH(DelDate) = " & Date.Now.Month & " and custid = " & custid & " and ccFrom = " & cc & " order by DelID desc", trans.Connection, trans)
            Dim da As New MySqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("DelID")
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_longDelId")
            Err.Clear()
        End Try

        Return strFilter

    End Function


    Public Function SelectDataset_invWareByID(ByVal wID As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            strFilter = "SELECT * from cld_inv_w where ID = '" & wID & "'"

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_invWareByID")
            Err.Clear()
        End Try

        Return ds

    End Function



    Public Function SelectDataset_invWare(ByVal itipe As Integer, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT((case cld_inv_w.flag when 1 then createddate WHEN 2 THEN deldate WHEN 3 THEN rcvdate END),'%d-%b-%Y') AS statdate, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.custid = " & custid & " and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.createddate desc,cld_inv_w.id desc LIMIT 300"
            'DATE_FORMAT(cld_inv_w.exDelDate,'%d-%b-%Y') AS CreateD
            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_invWare")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function SelectDs_purchaserequest(ByVal xstat As String, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        xstat = xstat.Replace("--", "")
        Try
            Dim sSearch As String = ""
            If name <> "" Then
                sSearch = " AND (pr_no LIKE '%" & name & "%' OR request_by LIKE '%" & name & "%' OR cld_inv_cc.Name LIKE '%" & name & "%')"
            End If

            Dim statCondition As String
            If xstat.ToUpper() = "ALL" Then
                statCondition = " 1=1 "
            Else
                statCondition = " cld_inv_pr.stat = '" & xstat & "' "
            End If

            strFilter = "SELECT pr_id,pr_no, DATE_FORMAT(pr_date, '%d-%b-%Y') AS prdate, " &
                        "DATE_FORMAT(required_date, '%d-%b-%Y') AS requireddate, " &
                        "cc,cld_inv_cc.Name AS ccname,request_by,stat,appBy, DATE_FORMAT(appDate, '%d-%b-%Y') AS aprDate, " &
                        "cld_inv_pr.remarks FROM cld_inv_pr LEFT JOIN cld_inv_cc ON cld_inv_pr.cc = cld_inv_cc.ID " &
                        "where " & statCondition & mHelper.ModPrvDep(Modpriv, user) & sSearch & " " &
                        "Order by cld_inv_pr.pr_date desc, cld_inv_pr.pr_id desc LIMIT 300"

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDs_purchaserequest")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function SelectDataset_invWareSearch(ByVal itipe As Integer, ByVal name As String, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            If icc = 0 Then
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT((case cld_inv_w.flag when 1 then createddate WHEN 2 THEN deldate WHEN 3 THEN rcvdate END),'%d-%b-%Y') AS statdate,createddate, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag in (1,2) " & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.createddate DESC,cld_inv_w.id desc"
                Else
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT((case cld_inv_w.flag when 1 then createddate WHEN 2 THEN deldate WHEN 3 THEN rcvdate END),'%d-%b-%Y') AS statdate,createddate, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.createddate DESC,cld_inv_w.id desc"
                End If
            Else
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT((case cld_inv_w.flag when 1 then createddate WHEN 2 THEN deldate WHEN 3 THEN rcvdate END),'%d-%b-%Y') AS statdate,createddate, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag in (1,2) " & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.createddate DESC,cld_inv_w.id desc"
                Else
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT((case cld_inv_w.flag when 1 then createddate WHEN 2 THEN deldate WHEN 3 THEN rcvdate END),'%d-%b-%Y') AS statdate,createddate, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.createddate DESC,cld_inv_w.id desc"
                End If
            End If

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataset_invWareSearch")
            Err.Clear()
        End Try

        Return ds

    End Function

    'Bram 11July 2022
    Public Function SelectDS_invoiceStrSrch(ByVal bydate As Date, ByVal bydateto As Date, ByVal itipe As Integer, ByVal name As String, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            If icc = 0 Then
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%' or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                Order by cld_inv_w.rcvdate ,cld_inv_w.ccto"
                Else
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                  ,  case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                Order by cld_inv_w.rcvdate ,cld_inv_w.ccto"
                End If
            Else
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & "
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                Order by cld_inv_w.rcvdate,cld_inv_w.ccto"
                Else
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                Order by cld_inv_w.rcvdate,cld_inv_w.ccto"
                End If
            End If

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDS_invoiceStrSrch")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function SelectDS_invoiceStrSrch1(ByVal itipe As Integer, ByVal name As String, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            If icc = 0 Then
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%' or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & " 
                                Order by cld_inv_w.rcvdate ,cld_inv_w.ccto"
                Else
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                  ,  case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                Order by cld_inv_w.rcvdate ,cld_inv_w.ccto"
                End If
            Else
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & "
                                Order by cld_inv_w.rcvdate,cld_inv_w.ccto"
                Else
                    strFilter = "SELECT cld_inv_w.id,cld_inv_w.ccfrom,cld_inv_w.ccto,cld_inv_w.cctonm,cld_inv_w.payno,cld_inv_w.rcvdate,cld_inv_w.doctot
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                Order by cld_inv_w.rcvdate,cld_inv_w.ccto"
                End If
            End If

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDS_invoiceStrSrch")
            Err.Clear()
        End Try

        Return ds

    End Function
    'Bram 11July 2022
    Public Function SelectDS_invoicepayment(ByVal bydate As Date, ByVal bydateto As Date, ByVal itipe As Integer, ByVal name As String, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            If icc = 0 Then
                If itipe = 0 Then
                    strFilter = "(SELECT cld_inv_w.id,cld_inv_w.cctonm 'Cost Centre',cld_inv_w.payno,cld_inv_w.rcvdate 'Receive Order',cld_inv_w.paydate 'Pay date',cld_inv_w.doctot 'Total'
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  
                                where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%' or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "')
                                Union ALL
                                SELECT 'Total','','','','',sum(cld_inv_w.doctot) 'Total'
                                , '' as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  
                                where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%' or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                ORDER BY 'Receive Order' ,'Cost Centre'"
                Else
                    strFilter = "(SELECT cld_inv_w.id,cld_inv_w.cctonm 'Cost Centre',cld_inv_w.payno,cld_inv_w.rcvdate 'Receive Order',cld_inv_w.paydate 'Pay date',cld_inv_w.doctot 'Total'
                                  ,  case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid
                                where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "')
                                Union ALL
                                SELECT 'Total','','','','',sum(cld_inv_w.doctot) 'Total'
                                  , '' as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid
                                where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                ORDER BY 'Receive Order' ,'Cost Centre'"
                End If
            Else
                If itipe = 0 Then
                    strFilter = "(SELECT cld_inv_w.id,cld_inv_w.cctonm 'Cost Centre',cld_inv_w.payno,cld_inv_w.rcvdate 'Receive Order',cld_inv_w.paydate 'Pay date',cld_inv_w.doctot 'Total'
                                , case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid
                                where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "')
                                Union ALL
                                 SELECT 'Total','','','','',sum(cld_inv_w.doctot) 'Total'
                                , '' as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid
                                where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 " & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                ORDER BY 'Receive Order' ,'Cost Centre'"
                Else
                    strFilter = "(SELECT cld_inv_w.id,cld_inv_w.cctonm 'Cost Centre',cld_inv_w.payno,cld_inv_w.rcvdate 'Receive Order',cld_inv_w.paydate 'Pay date',cld_inv_w.doctot 'Total'
                                , '' as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid
                                where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & "
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "')
                                Union ALL
                                SELECT 'Total','','','','',sum(cld_inv_w.doctot) 'Total'
                                 , '' as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid
                                where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " 
                                and  cld_inv_w.rcvdate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_w.rcvdate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                                ORDER BY 'Receive Order' ,'Cost Centre'"
                End If
            End If

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDS_invoiceStrSrch")
            Err.Clear()
        End Try

        Return ds

    End Function
    'Bram 10Jul22
    Public Function SelectDS_invoiceStrByid(ByVal itipe As Integer, ByVal id As String, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            If icc = 0 Then
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%' or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 and cld_inv_w.id ='" & id & "' " & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.rcvdate ,cld_inv_w.ccto"
                Else
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.id ='" & id & "' and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.rcvdate ,cld_inv_w.ccto"
                End If
            Else
                If itipe = 0 Then
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.flag >2 and cld_inv_w.id ='" & id & "' " & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.rcvdate,cld_inv_w.ccto"
                Else
                    strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag WHEN 3 THEN 'Receive' WHEN 4 THEN 'Paid' END as Status FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%'or cld_inv_w.cctonm Like '%" & name & "%') and cld_inv_w.id ='" & id & "' and cld_inv_w.flag = " & itipe & mHelper.ModPrvDep(Modpriv, user) & " Order by cld_inv_w.rcvdate,cld_inv_w.ccto"
                End If
            End If

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDS_invoiceStrSrch")
            Err.Clear()
        End Try

        Return ds

    End Function


    'detail transaksi
    Public Function SelectDataTable_invWare(ByVal id As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""
        Dim c_cmd1 As New clsDbo("")
        Dim xmarkup As Decimal = 0
        Try
            'perubahan 14 sept 2017
            'jadi klo ada warehousenya
            If ccware <> 0 Then
                'strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & ")/w_dtl.factor1, 0) AS stock " & _
                '                "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " & _
                '                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
                'strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stock " &
                '                "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                '                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo

                'strFilter = "SELECT w_dtl.wid,w_dtl.custid,w_dtl.ccto,w_dtl.stk_id,w_dtl.stk_name,w_dtl.totalu2,w_dtl.U2_nm,w_dtl.u1Del,w_dtl.u1Rcv,cld_inv_stk.Factor1, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id 
                '                AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock,
                '                (case when w_dtl.cost=0 and w_dtl.u1del=0 then (CASE
                '                WHEN o.ownid =1 THEN price1
                '                Else price2 END) else w_dtl.cost end) cost
                '                FROM cld_inv_w_dtl AS w_dtl 
                '                LEFT JOIN cld_inv_cc cc ON cc.id = w_dtl.ccto AND cc.custid = w_dtl.custid 
                '                LEFT JOIN cld_outlet o ON o.id = cc.olid AND o.custid = w_dtl.custid   
                '                INNER JOIN cld_inv_stk ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID
                '                 WHERE w_dtl.wID = '" & id & "' AND w_dtl.custid = " & custid & " AND w_dtl.ccTo = " & ccTo

                xmarkup = c_cmd1.ExecuteScalar("SELECT markup FROM `cld_own` 
                                                INNER JOIN `cld_outlet` o ON o.ownid = cld_own.id
                                                INNER JOIN `cld_inv_cc` cc ON cc.olid=o.id
                                                WHERE cc.id =" & ccTo & "", 0)

                'INNER JOIN (SELECT stkid,(cost+cost* " & xmarkup & "/100) p1 FROM cld_inv_transstockcls WHERE ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "') stk
                'langsung ke table cld_inv_stk_cc
                'If My.Settings.stock_cost = 1 Then
                '    strFilter = "SELECT w_dtl.wid,w_dtl.custid,w_dtl.ccto,w_dtl.stk_id,w_dtl.stk_name,w_dtl.totalu2,w_dtl.U2_nm,w_dtl.u1Del,w_dtl.u1Rcv,cld_inv_stk.Factor1, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id 
                '                AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock,
                '                (case when w_dtl.cost=0 and w_dtl.u1del=0 then p1 else w_dtl.cost end) cost, w_dtl.u1Del*(CASE WHEN w_dtl.cost=0 AND w_dtl.u1del=0 THEN p1 ELSE w_dtl.cost END) totalcost
                '                FROM cld_inv_w_dtl AS w_dtl 
                '                LEFT JOIN cld_inv_cc cc ON cc.id = w_dtl.ccto AND cc.custid = w_dtl.custid 
                '                LEFT JOIN cld_outlet o ON o.id = cc.olid AND o.custid = w_dtl.custid   
                '                INNER JOIN cld_inv_stk ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID
                '                INNER JOIN (SELECT stkid,(cost+cost* " & xmarkup & "/100) p1 FROM cld_inv_stk_cc WHERE ccid = " & ccware & " AND custid = " & custid & ") stk
                '                  ON stk.stkid = w_dtl.stk_id 
                '                 WHERE w_dtl.wID = '" & id & "' AND w_dtl.custid = " & custid & " AND w_dtl.ccTo = " & ccTo
                'Else
                strFilter = "SELECT w_dtl.wid,w_dtl.custid,w_dtl.ccto,w_dtl.stk_id,w_dtl.stk_name,w_dtl.totalu2,w_dtl.U2_nm,w_dtl.u1Del,w_dtl.u1Rcv,cld_inv_stk.Factor1, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id 
                                AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock,
                                (case when w_dtl.cost=0 and w_dtl.u1del=0 then p1 else w_dtl.cost end) cost, w_dtl.u1Del*(CASE WHEN w_dtl.cost=0 AND w_dtl.u1del=0 THEN p1 ELSE w_dtl.cost END) totalcost, w_dtl.U1, w_dtl.U1_nm
                                FROM cld_inv_w_dtl AS w_dtl 
                                LEFT JOIN cld_inv_cc cc ON cc.id = w_dtl.ccto AND cc.custid = w_dtl.custid 
                                LEFT JOIN cld_outlet o ON o.id = cc.olid AND o.custid = w_dtl.custid   
                                INNER JOIN cld_inv_stk ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID
                                INNER JOIN (SELECT stkid,(cost+cost* " & xmarkup & "/100) p1 FROM cld_inv_transstockcls WHERE ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "') stk
                                  ON stk.stkid = w_dtl.stk_id 
                                 WHERE w_dtl.wID = '" & id & "' AND w_dtl.custid = " & custid & " AND w_dtl.ccTo = " & ccTo
                ' End If



                '(IFNULL((SELECT cost FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id 
                '                And ccid = " & ccware & " And custid = " & custid & " And invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0))


            Else
                strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2  FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
            End If



            Dim a As New clsDbo(strFilter)

            ds = a.selectDatatable

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataTable_invWare")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Function SelectDataTable_invWare3(ByVal id As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""
        Dim c_cmd1 As New clsDbo("")
        Dim xmarkup As Decimal = 0
        Try

            If ccware <> 0 Then
                xmarkup = c_cmd1.ExecuteScalar("SELECT markup FROM `cld_own` 
                                                INNER JOIN `cld_outlet` o ON o.ownid = cld_own.id
                                                INNER JOIN `cld_inv_cc` cc ON cc.olid=o.id
                                                WHERE cc.id =" & ccTo & "", 0)

                c_cmd1.resetCmd("")



                '               strFilter = "SELECT wid,custid,ccto,stk_id,stk_name,totalu2,u2_nm,u1del,u1rcv,factor1,number,unit2,stock,CASE WHEN batch>cost THEN batch ELSE cost END AS cost,totalcost FROM
                '(SELECT w_dtl.wid,w_dtl.custid,w_dtl.ccto,w_dtl.stk_id,w_dtl.stk_name,
                'CASE batch WHEN 1 THEN (SELECT (cost+cost* " & xmarkup & "/100)/cld_inv_stk.factor1 FROM `cld_inv_rcp_cc` WHERE ccid =" & ccware & " AND rcpid = cld_inv_stk.blink) ELSE 0 END AS batch,
                'w_dtl.totalu2,w_dtl.U2_nm,w_dtl.u1Del,w_dtl.u1Rcv,cld_inv_stk.Factor1, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id 
                '                               AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock,
                '                               (CASE WHEN w_dtl.cost=0 AND w_dtl.u1del=0 THEN p1 ELSE w_dtl.cost END) cost, w_dtl.u1Del*(CASE WHEN w_dtl.cost=0 AND w_dtl.u1del=0 THEN p1 ELSE w_dtl.cost END) totalcost
                '                               FROM cld_inv_w_dtl AS w_dtl 
                '                               LEFT JOIN cld_inv_cc cc ON cc.id = w_dtl.ccto AND cc.custid = w_dtl.custid 
                '                               LEFT JOIN cld_outlet o ON o.id = cc.olid AND o.custid = w_dtl.custid   
                '                               INNER JOIN cld_inv_stk ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID
                '                               INNER JOIN (SELECT stkid,(cost+cost* " & xmarkup & "/100) p1 FROM cld_inv_transstockcls WHERE ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "') stk
                '                                 ON stk.stkid = w_dtl.stk_id 
                '                                WHERE w_dtl.wID = '" & id & "' AND w_dtl.custid = " & custid & " AND w_dtl.ccTo = " & ccTo & ") AS tbl1"


                strFilter = "SELECT wid,custid,ccto,stk_id,stk_name,totalu2,u2_nm,u1del,u1rcv,factor1,number,unit2,stock,CASE WHEN batch>cost THEN batch ELSE cost END AS cost,totalcost FROM
 (SELECT w_dtl.wid,w_dtl.custid,w_dtl.ccto,w_dtl.stk_id,w_dtl.stk_name,
 CASE batch WHEN 1 THEN (SELECT (ROUND(SUM(((cld_inv_rcpdtl.qty +(( cld_inv_rcpdtl.qty * cld_inv_rcpdtl.lossf)/100))/ S2.Factor2) * (SELECT Cost FROM `cld_inv_transstockcls` 
 WHERE stkid=cld_inv_rcpdtl.`kodeId` AND cld_inv_transstockcls.`ccid` = " & ccware & " AND InvDate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')), 2)*(1+ " & xmarkup & "/100)/cld_inv_stk.Factor1) AS batch
                                                                 FROM cld_inv_rcpdtl INNER JOIN cld_inv_stk S2
                                                                 ON cld_inv_rcpdtl.`kodeId` = s2.id
                                                                 WHERE rcpid = cld_inv_stk.blink AND tipe = 1) ELSE 0 END AS batch,
 w_dtl.totalu2,w_dtl.U2_nm,w_dtl.u1Del,w_dtl.u1Rcv,cld_inv_stk.Factor1, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id 
                                AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock,
                                (CASE WHEN w_dtl.cost=0 AND w_dtl.u1del=0 THEN p1 ELSE w_dtl.cost END) cost, w_dtl.u1Del*(CASE WHEN w_dtl.cost=0 AND w_dtl.u1del=0 THEN p1 ELSE w_dtl.cost END) totalcost
                                FROM cld_inv_w_dtl AS w_dtl 
                                LEFT JOIN cld_inv_cc cc ON cc.id = w_dtl.ccto AND cc.custid = w_dtl.custid 
                                LEFT JOIN cld_outlet o ON o.id = cc.olid AND o.custid = w_dtl.custid   
                                INNER JOIN cld_inv_stk ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID
                                INNER JOIN (SELECT stkid,(cost+cost* " & xmarkup & "/100) p1 FROM cld_inv_transstockcls WHERE ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "') stk
                                  ON stk.stkid = w_dtl.stk_id 
                                 WHERE w_dtl.wID = '" & id & "' AND w_dtl.custid = " & custid & " AND w_dtl.ccTo = " & ccTo & ") AS tbl1"

            Else
                strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2  FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
            End If



            Dim a As New clsDbo(strFilter)

            ds = a.selectDatatable

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataTable_invWare")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Function SelectDataTable_invWare2(ByVal id As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""

        Try
            'perubahan 14 sept 2017
            'jadi klo ada warehousenya
            'If ccware <> 0 Then
            '    'strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & ")/w_dtl.factor1, 0) AS stock " & _
            '    '                "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " & _
            '    '                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
            '    strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stock
            '                , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccTo & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stockcc
            '                , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stockwh
            '                , IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccTo & " and custid = " & custid & "), 0) AS stklimit " &
            '                    "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
            '                    "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo

            'Else
            '    strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2 , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stock 
            '                , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccTo & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stockcc
            '                , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "')/w_dtl.factor1, 0) AS stockwh
            '                , IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccTo & " and custid = " & custid & "), 0) AS stklimit " &
            '                "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
            '                    "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
            'End If
            If ccware <> 0 Then

                strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock
                            , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccTo & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stockcc
                            , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stockwh
                            , IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccTo & " and custid = " & custid & "), 0) AS stklimit " &
                                "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo

            Else
                strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2 , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock 
                            , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccTo & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stockcc
                            , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & " AND invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stockwh
                            , IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccTo & " and custid = " & custid & "), 0) AS stklimit " &
                            "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
            End If
            Dim a As New clsDbo(strFilter)

            ds = a.selectDatatable

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataTable_invWare")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Sub InsertInvDetail_invWare(ByVal custid As Integer, ByVal iType As Integer, ByVal iCc As Integer, ByVal iStk As Integer, ByVal sStk As String, ByVal dU1 As Decimal, ByVal dU2 As Decimal, ByVal dFactor1 As Decimal, ByVal dCost As Decimal, ByVal slinkId As String, ByVal iFtrans As Integer, ByVal sU1nm As String, ByVal sCreatedBy As String, ByVal sStkNo As String, ByVal sCcNm As String, ByVal sU2nm As String, ByVal xdate As Date)
        Dim str As String = ""
        Dim c_cmd As New clsDbo("")
        Try
            If cekInvDetail_invWare(custid, iCc, iStk, slinkId) > 0 Then
                str = "update cld_Inv_Trans set  U1 = " & dU1 & ", U2 = " & dU2 & ", factor1 = " & dFactor1 & ", ttlstock = " & ((dU1 * dFactor1) + dU2) & ", Cost = " & dCost & ", " &
                  " ModifBy = '" & sCreatedBy & "', flagtrans = " & iFtrans & ", Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'  where linkid = '" & slinkId & "' and custid = " & custid & " and cc = " & iCc & " and  stk_id = " & iStk
                c_cmd.executeNonQuery(str)
            Else
                str = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "'," & iType & ", " & iCc & ", " &
                    " " & iStk & ", '" & sStk & "', " & dU1 & ", " & dU2 & ", " & dFactor1 & ", " & CDec((dU1 * dFactor1) + dU2) & ", 0, " & dCost & ", " &
                    " '" & slinkId & "', " & iFtrans & ", '" & sU1nm & "', '" & sU2nm & "', '" & sCreatedBy & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & sStkNo & "', '" & sCcNm & "')"
                c_cmd.executeNonQuery(str)

                ', " & CDec(dU1 * dFactor1) & "
            End If
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:InsertInvDetail_invWare")
            Err.Clear()
        End Try



    End Sub

    Public Sub InsertInvDetail_invWare(ByVal custid As Integer, ByVal iType As Integer, ByVal iCc As Integer, ByVal iStk As Integer, ByVal sStk As String, ByVal dU1 As Decimal, ByVal dU2 As Decimal, ByVal dFactor1 As Decimal, ByVal dCost As Decimal, ByVal slinkId As String, ByVal iFtrans As Integer, ByVal sU1nm As String, ByVal sCreatedBy As String, ByVal sStkNo As String, ByVal sCcNm As String, ByVal sU2nm As String, ByVal xdate As Date, ByRef trans As MySqlTransaction)
        Dim str As String = ""
        Try
            Dim iID As Integer = 0
            Using cmd As New MySqlCommand("select Max(ID) from cld_inv_Trans", trans.Connection, trans)
                If IsDBNull(cmd.ExecuteScalar) Then
                    iID = 1
                Else
                    iID = cmd.ExecuteScalar + 1
                End If
            End Using

            If cekInvDetail_invWare(custid, iCc, iStk, slinkId, trans) > 0 Then
                str = "update cld_Inv_Trans set  U1 = " & dU1 & ", U2 = " & dU2 & ", factor1 = " & dFactor1 & ", ttlstock = " & ((dU1 * dFactor1) + dU2) & ", Cost = " & dCost & ", " &
                  " ModifBy = '" & sCreatedBy & "', flagtrans = " & iFtrans & ", Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'  where linkid = '" & slinkId & "' and custid = " & custid & " and cc = " & iCc & " and  stk_id = " & iStk
                Using cmd As New MySqlCommand(str, trans.Connection, trans)
                    cmd.ExecuteNonQuery()
                End Using
            Else
                str = "insert into cld_inv_Trans values (" & iID & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "'," & iType & ", " & iCc & ", " &
                    " " & iStk & ", '" & sStk & "', " & dU1 & ", " & dU2 & ", " & dFactor1 & ", " & CDec((dU1 * dFactor1) + dU2) & ", 0, " & dCost & ", " &
                    " '" & slinkId & "', " & iFtrans & ", '" & sU1nm & "', '" & sU2nm & "', '" & sCreatedBy & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & sStkNo & "', '" & sCcNm & "')"
                Using cmd As New MySqlCommand(str, trans.Connection, trans)
                    cmd.ExecuteNonQuery()
                End Using

                ', " & CDec(dU1 * dFactor1) & "
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function SelectDataTable_invWare_rcv_grid(ByVal id As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""

        Try
            'perubahan 14 sept 2017
            'jadi klo ada warehousenya
            If ccware <> 0 Then
                strFilter = "SELECT number, w_dtl.stk_name 'Name',FORMAT(w_dtl.totalu2, 2) 'Order' ,FORMAT(w_dtl.U1Del, 2) Delivery,FORMAT(w_dtl.u1Rcv, 2) Receive,w_dtl.U2_nm Unit,FORMAT(w_dtl.cost, 2) Price, FORMAT((w_dtl.u1Rcv * w_dtl.cost), 2) Total, '' as note, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & ")/w_dtl.factor1, 0) AS stock " &
                                "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo

            Else
                strFilter = "SELECT number, w_dtl.stk_name 'Name',FORMAT(w_dtl.totalu2, 2) 'Order' ,FORMAT(w_dtl.U1Del, 2) Delivery,FORMAT(w_dtl.u1Rcv, 2) Receive,w_dtl.U2_nm Unit,FORMAT(w_dtl.cost, 2) Price, FORMAT((w_dtl.u1Rcv * w_dtl.cost), 2) Total, '' as note FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " &
                                "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
            End If

            Dim a As New clsDbo(strFilter)

            ds = a.selectDatatable

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:SelectDataTable_invWaregrid")
            Err.Clear()
        End Try

        Return ds

    End Function


    Private Function cekInvDetail_invWare(ByVal custid As Integer, ByVal iCc As Integer, ByVal iStk As Integer, ByVal slinkId As String) As Integer

        Dim c_cmd As New clsDbo("")

        Dim sID As Integer
        Try
            sID = c_cmd.ExecuteScalar("select id from cld_inv_trans where CustID = " & custid & " and Cc = " & iCc & " and Stk_Id = " & iStk & " and Linkid = '" & slinkId & "'", 2)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:cekInvDetail_invWare")
            Err.Clear()
        End Try

        Return sID

    End Function

    Private Function cekInvDetail_invWare(ByVal custid As Integer, ByVal iCc As Integer, ByVal iStk As Integer, ByVal slinkId As String, ByRef trans As MySqlTransaction) As Integer
        Dim sID As Integer
        Try
            Using cmd As New MySqlCommand("select id from cld_inv_trans where CustID = " & custid & " and Cc = " & iCc & " and Stk_Id = " & iStk & " and Linkid = '" & slinkId & "'", trans.Connection, trans)
                Dim obj = cmd.ExecuteScalar()
                If IsDBNull(obj) OrElse obj Is Nothing Then
                    sID = 0
                Else
                    sID = Convert.ToInt32(obj)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try

        Return sID

    End Function
    Public Function Update_invrcv(ByVal sid As String, ByVal ccfrom As Integer, ByVal ccto As Integer, ByVal CustID As Integer, ByVal Paymentid As String, ByVal paydate As Date, ByVal paynotes As String) As Boolean
        Dim c_cmd As New Cls_command
        Dim hasil As Boolean = False
        Dim str As String
        Try
            str = "Update cld_inv_w set paydate = '" & paydate.ToString("yyyy-MM-dd HH:mm:ss") & "' , payno ='" & Paymentid & "', paynotes ='" & paynotes & "', flag = 4 " &
                " where ID = '" & sid & "' and ccfrom = " & ccfrom & " and ccto = " & ccto & " and CustID = " & CustID & ""
            hasil = c_cmd.executeNonQueryImport(str)

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:Update_invrcv")
            Err.Clear()
        End Try
        c_cmd = Nothing
        Return hasil
    End Function
    Public Function ware_date(ByVal ID As String) As Integer
        'Dim ds As New DataSet

        Dim edtable As Integer = 0

        Try
            Dim a As New clsDbo("")
            edtable = a.ExecuteScalar("SELECT count(id) from cld_inv_w where id = '" & ID & "' and cld_inv_w.CreatedDate >= '" & Now.AddDays(-30).ToString("yyyy-MM-dd") & "'", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:u1_invorderrcv")
            Err.Clear()
        End Try

        Return edtable

    End Function
    Public Function get_invchangecostSrch(ByVal bydate As Date, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim c_cmd1 As New clsDbo("")
        Dim xmarkup As String = ""
        Dim ccware As Integer = 0
        xmarkup = "+cost *((SELECT markup FROM `cld_own` 
                                                INNER JOIN `cld_outlet` o ON o.ownid = cld_own.id
                                                INNER JOIN `cld_inv_cc` cc ON cc.olid=o.id
                                                WHERE cc.id = cld_inv_w_dtl.ccto LIMIT 1)/100)"

        c_cmd1.resetCmd("")
        ccware = c_cmd1.ExecuteScalar("select id from `cld_inv_cc` where olid = 0 ", 0)
        'strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
        '                "where cld_inv_Trans.type =  9 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_Trans.stk_name Like '%" & name & "%')"



        strFilter = "SELECT '" & bydate.ToString("dd-MMM-yyyy") & "' as date,(select name from `cld_inv_cc` where id = cld_inv_w_dtl.ccto limit 1) CC, cld_inv_w_dtl.*,(SELECT cost " & xmarkup & " FROM cld_inv_transstockcls WHERE ccid=" & ccware & " AND stkid=cld_inv_w_dtl.`stk_id` AND invdate='" & bydate.ToString("yyyy-MM-dd") & "') costcls FROM `cld_inv_w_dtl` WHERE wid IN
(SELECT id FROM `cld_inv_w` WHERE deldate='" & bydate.ToString("yyyy-MM-dd") & "')
AND ROUND((SELECT cost " & xmarkup & " FROM cld_inv_transstockcls WHERE ccid=" & ccware & " AND stkid=cld_inv_w_dtl.`stk_id` AND invdate='" & bydate.ToString("yyyy-MM-dd") & "'),1)<>ROUND(cost,1) and (cld_inv_w_dtl.wID Like '%" & name & "%' or cld_inv_w_dtl.stk_name Like '%" & name & "%')"





        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function Update_changedcost(ByVal wid As String, ByVal stkid As Integer, ByVal ccware As Integer, ByVal invdate As Date) As Boolean
        Dim c_cmd As New Cls_command
        Dim hasil As Boolean = False
        Dim str As String
        Try
            If wid = "" And stkid = 0 Then
                str = "UPDATE `cld_inv_w_dtl` SET cost= (SELECT cost *1.12 FROM cld_inv_transstockcls WHERE ccid=" & ccware & " AND stkid=cld_inv_w_dtl.`stk_id` AND invdate='" & invdate.ToString("yyyy-MM-dd") & "') WHERE wid IN
                    (SELECT id FROM `cld_inv_w` WHERE deldate='" & invdate.ToString("yyyy-MM-dd") & "')
                    AND ROUND((SELECT cost *1.12 FROM cld_inv_transstockcls WHERE ccid=" & ccware & " AND stkid=cld_inv_w_dtl.`stk_id` AND invdate='" & invdate.ToString("yyyy-MM-dd") & "'),1)<>ROUND(cost,1);
                    UPDATE `cld_inv_trans` tr 
                    JOIN cld_inv_w_dtl dtl ON dtl.wid = tr.linkid AND tr.stk_id=dtl.stk_id 
                    SET tr.cost=dtl.cost
                    WHERE TYPE IN (7,8) AND invdate>='" & invdate.ToString("yyyy-MM-dd") & "' AND tr.cost<>dtl.cost"
            Else
                str = "UPDATE `cld_inv_w_dtl` SET cost= (SELECT cost *1.12 FROM cld_inv_transstockcls WHERE ccid=" & ccware & " AND stkid=cld_inv_w_dtl.`stk_id` AND invdate='" & invdate.ToString("yyyy-MM-dd") & "') WHERE wid IN
                    (SELECT id FROM `cld_inv_w` WHERE deldate='" & invdate.ToString("yyyy-MM-dd") & "')
                    AND ROUND((SELECT cost *1.12 FROM cld_inv_transstockcls WHERE ccid=" & ccware & " AND stkid=cld_inv_w_dtl.`stk_id` AND invdate='" & invdate.ToString("yyyy-MM-dd") & "'),1)<>ROUND(cost,1)
                    AND wid='" & wid & "' AND stk_id = " & stkid & ";
                    UPDATE `cld_inv_trans` tr 
                    JOIN cld_inv_w_dtl dtl ON dtl.wid = tr.linkid AND tr.stk_id=dtl.stk_id 
                    SET tr.cost=dtl.cost
                    WHERE TYPE IN (7,8) AND invdate>='" & invdate.ToString("yyyy-MM-dd") & "' AND tr.cost<>dtl.cost
                    AND dtl.wid='" & wid & "' AND tr.stk_id = " & stkid & ""
            End If

            hasil = c_cmd.executeNonQuery(str)
            updatedoctot_byreceive()
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:Update_changedcost")
            Err.Clear()
        End Try
        c_cmd = Nothing
        Return hasil
    End Function
    Private Sub updatedoctot_byreceive()
        Dim dtorder As New DataTable
        Dim ClsInventory As New clsInventory
        Try
            dtorder = load_difdoctot(Now.Date.AddDays(-14), Now.Date)
            If dtorder.Rows.Count > 0 Then
                For Each drow As DataRow In dtorder.Rows
                    update_ccinvoice(drow.Item("createddate"), drow.Item("ccto"), drow.Item("id"), drow.Item("ttldtl"))
                Next


            End If
        Catch ex As Exception
            errLog(Err.Description, "frmsales_summary:updatedoctot_byreceive")
            Err.Clear()
        End Try
    End Sub
    Public Function load_difdoctot(ByVal dtfrom As Date, ByVal dtto As Date) As DataTable
        Dim dt As New DataTable

        Try

            Dim a As New clsDbo("SELECT createddate,id,ccto,doctot, (SELECT SUM(u1Rcv*cost) FROM cld_inv_w_dtl WHERE wid = w.id AND ccto = w.ccto ) ttldtl FROM cld_inv_w w 
                        WHERE createddate >='" & dtfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND createddate <='" & dtto.ToString("yyyy-MM-dd HH:mm:ss") & "' AND flag>2
                        HAVING (ttldtl-doctot) >1 OR (ttldtl-doctot) <-0.5
                        ORDER BY ccto,createddate")
            dt = a.selectDatatable

            a.dispose()
            a = Nothing


        Catch ex As Exception
            errLog(Err.Description, "load_difdoctot")
            Err.Clear()
        End Try
        Return dt

    End Function
    Public Sub update_ccinvoice(ByVal createddate As Date, ByVal ccto As Integer, ByVal wid As String, ByVal newdoctot As Double)

        Dim c_cmd As New clsDbo("")


        Try

            c_cmd.ReNew()
            c_cmd.executeNonQuery("update cld_inv_w set doctot = " & newdoctot & " WHERE ccto = " & ccto & " and id = '" & wid & "' AND createddate = '" & createddate.ToString("yyyy-MM-dd 00:00:00") & "' ")

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:update_fixederr")
            Err.Clear()
        End Try


    End Sub

#Region "Purchase Request - cld_inv_pr / cld_inv_prd"

    ''' <summary>
    ''' Generate PR number format: PR-YYMM-CC-NNNN
    ''' </summary>
    Public Function idpurchaserequest(ByVal icc As Integer, ByRef trans As MySqlTransaction, Optional ByVal try2 As Integer = 0) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = Selectds_PRId(icc, trans)


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
            str = "PR" & mon & "/" & yer & "-" & cc & "-" & awal
        Else
            If ids.Length = 1 Then
                str = "PR" & mon & "/" & yer & "-" & cc & "-0000" & ids
            ElseIf ids.Length = 2 Then
                str = "PR" & mon & "/" & yer & "-" & cc & "-000" & ids
            ElseIf ids.Length = 3 Then
                str = "PR" & mon & "/" & yer & "-" & cc & "-00" & ids
            ElseIf ids.Length = 4 Then
                str = "PR" & mon & "/" & yer & "-" & cc & "-0" & ids
            ElseIf ids.Length = 5 Then
                str = "PR" & mon & "/" & yer & "-" & cc & "-" & ids
            End If
        End If

        Return str

    End Function

    ''' <summary>
    ''' Load detail items dari cld_inv_prd untuk edit mode.
    ''' Kolom alias: pr_id, stk_id, stk_name, qty_req, u2_nm, number, stockcc, stockwh, stklimit
    ''' </summary>
    Public Function getDTInvPR(ByVal prNo As String, ByVal cc As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Try
            If prNo = "" Then
                ' New document — return empty DataTable dengan schema yang sama
                strFilter = "SELECT '' AS pr_id, '' AS pr_no, " & cc & " AS cc, " &
                            "0 AS stk_id, '' AS stk_name, 0 AS u1, '' AS u1_nm,0 as factor1, 0 AS u2, '' AS u2_nm,0 as Total, " &
                            "0 AS app_qty,'REQUEST' AS stat,'' AS remarks, 0 AS stockcc, 0 AS stklimit LIMIT 0"
            Else
                strFilter = "SELECT d.pr_id AS pr_id, pr.pr_no, pr.cc, d.stk_id, " &
                            "stk.name stk_name, d.u1, stk.unit1 u1_nm,stk.factor1, d.u2, stk.unit2 u2_nm,d.Total, " &
                            "d.app_qty,d.stat, d.supplier_id, d.remarks, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid=d.stk_id AND ccid=" & cc & " AND invdate='" & Date.Now.Date.ToString("yyyy-MM-dd") & "'),0) AS stockcc, " &
                            "IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid=stk.id AND ccid=" & cc & "),0) AS stklimit " &
                            "FROM cld_inv_prd d INNER JOIN cld_inv_stk stk ON stk.id=d.stk_id " &
                            "INNER JOIN cld_inv_pr pr ON pr.pr_id=d.pr_id " &
                            "WHERE pr.pr_no='" & prNo & "' AND pr.cc=" & cc
            End If

            Dim a As New clsDbo(strFilter)
            dt = a.selectDatatable
            a.dispose()

            ' Tambahkan kolom total jika belum ada schema (untuk new document)
            If dt.Columns.Count = 0 OrElse Not dt.Columns.Contains("Total") Then
                ' Bila query return empty schema, buat manual
                If Not dt.Columns.Contains("pr_id") Then dt.Columns.Add("pr_id", GetType(String))
                If Not dt.Columns.Contains("pr_no") Then dt.Columns.Add("pr_no", GetType(String))
                If Not dt.Columns.Contains("custid") Then dt.Columns.Add("custid", GetType(Integer))
                If Not dt.Columns.Contains("cc") Then dt.Columns.Add("cc", GetType(Integer))
                If Not dt.Columns.Contains("stk_id") Then dt.Columns.Add("stk_id", GetType(Integer))
                If Not dt.Columns.Contains("stk_name") Then dt.Columns.Add("stk_name", GetType(String))
                If Not dt.Columns.Contains("qty_req") Then dt.Columns.Add("qty_req", GetType(Decimal))
                If Not dt.Columns.Contains("u2_nm") Then dt.Columns.Add("u2_nm", GetType(String))
                If Not dt.Columns.Contains("number") Then dt.Columns.Add("number", GetType(String))
                If Not dt.Columns.Contains("unit1") Then dt.Columns.Add("unit1", GetType(String))
                If Not dt.Columns.Contains("factor1") Then dt.Columns.Add("factor1", GetType(Decimal))
                If Not dt.Columns.Contains("stockcc") Then dt.Columns.Add("stockcc", GetType(Decimal))
                If Not dt.Columns.Contains("stockwh") Then dt.Columns.Add("stockwh", GetType(Decimal))
                If Not dt.Columns.Contains("stklimit") Then dt.Columns.Add("stklimit", GetType(Decimal))
            End If
            ' alias Total = qty_req untuk kompatibilitas dengan GridView
            If Not dt.Columns.Contains("Total") Then
                Dim col As New DataColumn("Total", GetType(Decimal))
                col.DefaultValue = 0
                dt.Columns.Add(col)
                For Each row As DataRow In dt.Rows
                    row("Total") = row("qty_req")
                Next
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:getDTInvPR")
            Err.Clear()
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' Insert atau Update cld_inv_pr (header) dan cld_inv_prd (detail).
    ''' prNo = "" berarti insert baru.
    ''' Mengembalikan pr_no yang dihasilkan atau di-update.
    ''' </summary>
    Public Function insertInvPR(ByVal prid As Integer, ByVal prNo As String, ByVal prDate As Date, ByVal required_date As Date, ByVal cc As Integer,
                                ByVal request_by As String, ByVal stat As String, ByVal appBy As String,
                                 ByVal appDate As Date,
                                ByVal dt As DataTable, ByVal sRemarks As String) As String
        Dim sHasil As String = ""
        Try
            Using conn As New MySql.Data.MySqlClient.MySqlConnection(My.Settings.ServerStr)
                conn.Open()
                Dim trans As MySql.Data.MySqlClient.MySqlTransaction = conn.BeginTransaction()
                Try
                    Dim c_cmd As New MySql.Data.MySqlClient.MySqlCommand()
                    c_cmd.Connection = conn
                    c_cmd.Transaction = trans
                    Dim xprid As Integer = 0

                    If prNo = "" Then
                        ' === INSERT NEW ===
                        prNo = idpurchaserequest(cc, trans)
                        sHasil = prNo

                        ' Insert header cld_inv_pr
                        c_cmd.CommandText = "INSERT INTO cld_inv_pr (pr_no, pr_date, required_date, cc,request_by, stat,appBy, appDate, remarks) " &
                                            "VALUES (@pr_no, @pr_date, @required_date, @cc, @request_by, @stat,@appBy, @appDate, @remarks)"
                        c_cmd.Parameters.Clear()
                        c_cmd.Parameters.AddWithValue("@pr_no", prNo)
                        c_cmd.Parameters.AddWithValue("@pr_date", prDate.ToString("yyyy-MM-dd"))
                        c_cmd.Parameters.AddWithValue("@required_date", required_date.ToString("yyyy-MM-dd"))
                        c_cmd.Parameters.AddWithValue("@cc", cc)
                        c_cmd.Parameters.AddWithValue("@request_by", request_by)
                        c_cmd.Parameters.AddWithValue("@stat", stat)
                        c_cmd.Parameters.AddWithValue("@appBy", appBy)
                        If stat = "REQUEST" Then
                            c_cmd.Parameters.AddWithValue("@appDate", DBNull.Value)
                        Else
                            c_cmd.Parameters.AddWithValue("@appDate", appDate.ToString("yyyy-MM-dd"))
                        End If
                        c_cmd.Parameters.AddWithValue("@remarks", sRemarks)
                        c_cmd.ExecuteNonQuery()
                    Else
                        ' === UPDATE EXISTING ===
                        sHasil = prNo

                        ' Retrieve xprid before updating and deleting
                        c_cmd.Parameters.Clear()
                        c_cmd.CommandText = "SELECT pr_id FROM cld_inv_pr WHERE pr_no=@pr_no AND cc=@cc"
                        c_cmd.Parameters.AddWithValue("@pr_no", prNo)
                        c_cmd.Parameters.AddWithValue("@cc", cc)
                        Dim objId As Object = c_cmd.ExecuteScalar()
                        If objId IsNot Nothing AndAlso Not IsDBNull(objId) Then
                            xprid = Convert.ToInt32(objId)
                        End If

                        c_cmd.Parameters.Clear()
                        c_cmd.CommandText = "UPDATE cld_inv_pr SET remarks=@remarks, request_by=@request_by, stat=@stat, appBy=@appBy, appDate=@appDate WHERE pr_no=@pr_no AND cc=@cc"
                        c_cmd.Parameters.AddWithValue("@remarks", sRemarks)
                        c_cmd.Parameters.AddWithValue("@cc", cc)
                        c_cmd.Parameters.AddWithValue("@request_by", request_by)
                        c_cmd.Parameters.AddWithValue("@stat", stat)
                        c_cmd.Parameters.AddWithValue("@appBy", appBy)
                        If stat = "REQUEST" Then
                            c_cmd.Parameters.AddWithValue("@appDate", DBNull.Value)
                        Else
                            c_cmd.Parameters.AddWithValue("@appDate", appDate.ToString("yyyy-MM-dd"))
                        End If
                        c_cmd.Parameters.AddWithValue("@pr_no", prNo)
                        c_cmd.ExecuteNonQuery()

                        ' Hapus detail lama sebelum insert ulang
                        c_cmd.Parameters.Clear()
                        c_cmd.CommandText = "DELETE FROM cld_inv_prd WHERE pr_id=@pr_id"
                        c_cmd.Parameters.AddWithValue("@pr_id", xprid)

                        c_cmd.ExecuteNonQuery()
                    End If

                    ' Insert detail ke cld_inv_prd
                    If xprid = 0 AndAlso prNo <> "" Then
                        c_cmd.Parameters.Clear()
                        c_cmd.CommandText = "SELECT pr_id FROM cld_inv_pr WHERE pr_no=@pr_no AND cc=@cc ORDER BY pr_id DESC LIMIT 1"
                        c_cmd.Parameters.AddWithValue("@pr_no", prNo)
                        c_cmd.Parameters.AddWithValue("@cc", cc)
                        Dim objId As Object = c_cmd.ExecuteScalar()
                        If objId IsNot Nothing AndAlso Not IsDBNull(objId) Then
                            xprid = Convert.ToInt32(objId)
                        End If
                    End If
                    For Each row As DataRow In dt.Rows
                        Dim qty As Decimal = 0
                        If dt.Columns.Contains("Total") Then
                            Decimal.TryParse(row("Total").ToString, qty)
                        End If

                        c_cmd.Parameters.Clear()
                        c_cmd.CommandText = "INSERT INTO cld_inv_prd (pr_id, stk_id,U1,U2, Total, app_qty, stat, remarks, supplier_id) " &
                                            "VALUES (@pr_id, @stk_id, @u1, @u2, @total, @app_qty, @stat, @remarks, @supplier_id)"
                        c_cmd.Parameters.AddWithValue("@pr_id", xprid)
                        c_cmd.Parameters.AddWithValue("@stk_id", row("stk_id"))
                        c_cmd.Parameters.AddWithValue("@u1", row("U1"))
                        c_cmd.Parameters.AddWithValue("@u2", row("U2"))
                        c_cmd.Parameters.AddWithValue("@total", row("Total"))
                        c_cmd.Parameters.AddWithValue("@app_qty", row("app_qty"))
                        c_cmd.Parameters.AddWithValue("@stat", row("stat"))
                        c_cmd.Parameters.AddWithValue("@remarks", row("remarks"))
                        c_cmd.Parameters.AddWithValue("@supplier_id", If(dt.Columns.Contains("supplier_id") AndAlso Not IsDBNull(row("supplier_id")), row("supplier_id"), 0))
                        c_cmd.ExecuteNonQuery()
                    Next

                    trans.Commit()
                    insertLogModule(appBy, "cld_inv_pr", If(prNo = sHasil, "Insert", "Update"), custid)

                    ' Auto Generate Order to Supplier jika di-approve oleh HO
                    If stat = "APPROVED" AndAlso Not String.IsNullOrEmpty(appBy) Then
                        Try
                            Dim genMsg As String = GenerateRcvFromPR(xprid, custid, My.Settings.ServerStr, appBy, dt, prDate, appDate)
                            If Not String.IsNullOrEmpty(genMsg) Then
                                sHasil &= "|" & genMsg
                            End If
                        Catch exGen As Exception
                            errLog("Error generating RCV from PR: " & exGen.Message, "clsInventory_Warehouse:insertInvPR_Gen")
                        End Try
                    End If

                Catch ex As Exception
                    trans.Rollback()
                    errLog(ex.Message & " | " & ex.StackTrace, "clsInventory_Warehouse:insertInvPR")
                    sHasil = "Error"
                End Try
            End Using
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:insertInvPR")
            Err.Clear()
        End Try
        Return sHasil
    End Function

    ''' <summary>
    ''' Hapus PR header + semua detail berdasarkan pr_no
    ''' </summary>
    Public Sub DeleteInvPR(ByVal prNo As String, ByVal cc As Integer, ByVal username As String)
        Try
            Dim c_cmd As New clsDbo("")
            c_cmd.executeNonQuery("DELETE FROM cld_inv_prd WHERE pr_no='" & prNo & "' AND cc=" & cc)
            c_cmd.ReNew()
            c_cmd.executeNonQuery("DELETE FROM cld_inv_pr WHERE pr_no='" & prNo & "' AND cc=" & cc)
            c_cmd.dispose()
            insertLogModule(username, "cld_inv_pr", "Delete " & prNo, 0)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:DeleteInvPR")
            Err.Clear()
        End Try
    End Sub

    ''' <summary>
    ''' Hapus satu item detail dari cld_inv_prd (berdasarkan id)
    ''' </summary>
    Public Sub DeleteInvPRd(ByVal id As Integer, ByVal username As String)
        Try
            Dim c_cmd As New clsDbo("")
            c_cmd.executeNonQuery("DELETE FROM cld_inv_prd WHERE id=" & id)
            c_cmd.dispose()
            insertLogModule(username, "cld_inv_pr_detail", "Delete detail id " & id, 0)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:DeleteInvPRd")
            Err.Clear()
        End Try
    End Sub

    Public Sub RejectInvPR(ByVal prId As Integer, ByVal username As String)
        Try
            Dim c_cmd As New clsDbo("")
            ' Update Master Table
            c_cmd.executeNonQuery("UPDATE cld_inv_pr SET stat = 'REJECTED' WHERE pr_id = " & prId)

            ' Update Detail Table
            ' c_cmd.ReNew()
            c_cmd.executeNonQuery("UPDATE cld_inv_prd SET stat = 'REJECTED' WHERE pr_id = " & prId)

            c_cmd.dispose()
            insertLogModule(username, "cld_inv_pr", "Reject pr_id " & prId, 0)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:RejectInvPR")
            Err.Clear()
        End Try
    End Sub

    Public Sub RejectInvPRd(ByVal id As Integer, ByVal username As String)
        Try
            Dim c_cmd As New clsDbo("")
            ' Update Detail Table
            c_cmd.executeNonQuery("UPDATE cld_inv_prd SET stat = 'REJECTED' WHERE id = " & id)

            c_cmd.dispose()
            insertLogModule(username, "cld_inv_prd", "Reject detail id " & id, 0)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:RejectInvPRd")
            Err.Clear()
        End Try
    End Sub

    ''' <summary>
    ''' Cek apakah PR masih bisa diedit (stat=1 berarti Draft)
    ''' </summary>
    Public Function isPREditable(ByVal prNo As String) As Boolean
        Dim result As Boolean = False
        Try
            Dim a As New clsDbo("SELECT stat FROM cld_inv_pr WHERE pr_no='" & prNo & "'")
            Dim ds As DataSet = a.selectDataset
            a.dispose()
            If ds.Tables(0).Rows.Count > 0 Then
                result = (ds.Tables(0).Rows(0).Item("stat") = "REQUEST")
            End If
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:isPREditable")
            Err.Clear()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Generate Order to Supplier (Inventory Receipt) based on approved PR items.
    ''' Grouped by Supplier.
    ''' </summary>
    Private Function GenerateRcvFromPR(ByVal prId As Integer, ByVal custId As Integer, ByVal connStr As String, ByVal username As String, ByVal dt As DataTable, ByVal prDate As Date, ByVal appDate As Date) As String
        Dim ccId As Integer = 0
        Dim notes As String = ""
        Dim requiredDate As Date = prDate

        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                ' Fetch header info from cld_inv_pr
                Using cmdHeader As New MySqlCommand("SELECT cc, remarks, required_date FROM cld_inv_pr WHERE pr_id = @pr_id", conn)
                    cmdHeader.Parameters.AddWithValue("@pr_id", prId)
                    Using reader = cmdHeader.ExecuteReader()
                        If reader.Read() Then
                            ccId = CInt(reader("cc"))
                            notes = reader("remarks").ToString()
                            requiredDate = If(IsDBNull(reader("required_date")), prDate, CDate(reader("required_date")))
                        End If
                    End Using
                End Using

                ' Filter approved items with valid suppliers
                Dim approvedRows = dt.AsEnumerable().Where(Function(r) dt.Columns.Contains("stat") AndAlso r("stat").ToString().ToUpper() = "APPROVED" AndAlso
                                                             Not IsDBNull(r("supplier_id")) AndAlso CInt(r("supplier_id")) > 0 AndAlso
                                                             Not IsDBNull(r("app_qty")) AndAlso CDec(r("app_qty")) > 0)

                If Not approvedRows.Any() Then
                    errLog("GenerateRcvFromPR: No approved rows with valid suppliers found for PR #" & prId, "Info")
                    Return ""
                End If

                Dim groups = approvedRows.GroupBy(Function(r) CInt(r("supplier_id")))
                Dim CItrans As New clsInventory_Trans()
                Dim generatedIds As New StringBuilder()

                For Each group In groups
                    Dim supId = group.Key
                    Dim idtrans As String = ""
                    Dim isUpdate As Boolean = False
                    
                    ' Check for existing Order for this PR and Supplier
                    If My.Settings.supp_tax = "1" Then
                        Using cmdExists As New MySqlCommand("SELECT id, flag FROM cld_invlk_rcv WHERE pr_id = @prId AND supid = @supId AND custid = @custId LIMIT 1", conn)
                            cmdExists.Parameters.AddWithValue("@prId", prId)
                            cmdExists.Parameters.AddWithValue("@supId", supId)
                            cmdExists.Parameters.AddWithValue("@custId", custId)
                            
                            Using rdrExists As MySqlDataReader = cmdExists.ExecuteReader()
                                If rdrExists.Read() Then
                                    Dim existingFlag As Integer = rdrExists.GetInt32("flag")
                                    If existingFlag = 2 Then
                                        errLog("GenerateRcvFromPR (Sup:" & supId & "): Order already exists and is FINISHED. Skipping update from PR.", "Info")
                                        Continue For
                                    Else
                                        idtrans = rdrExists.GetString("id")
                                        isUpdate = True
                                    End If
                                End If
                            End Using
                        End Using
                    End If

                    If Not isUpdate Then
                        idtrans = CItrans.idRcv(custId)
                        Dim try2 As Integer = 0
                        ' ID Collision check
                        While True
                            Using cmdCheck As New MySqlCommand("SELECT COUNT(id) FROM cld_invlk_rcv WHERE id=@id AND custid=@custid", conn)
                                cmdCheck.Parameters.AddWithValue("@id", idtrans)
                                cmdCheck.Parameters.AddWithValue("@custid", custId)
                                If CInt(cmdCheck.ExecuteScalar()) = 0 Then Exit While
                                try2 += 1
                                idtrans = CItrans.idRcv(custId, try2)
                            End Using
                        End While
                    End If

                    Dim rcvTrans As MySqlTransaction = conn.BeginTransaction()
                    Dim sqlHeader As String = ""
                    If isUpdate Then
                        sqlHeader = "UPDATE cld_invlk_rcv SET doctot=@doctot, Notes=@Notes, modifby=@createdby, modifdate=NOW() WHERE id=@id AND custid=@custid"
                    Else
                        If My.Settings.supp_tax = "1" Then
                            sqlHeader = "INSERT INTO cld_invlk_rcv (id, custid, `Do`, supid, flag, doctot, Tipe, cc, createdby, createddate, InvDate, acc, exDelDate, Notes, p_tax, n_tax, pr_id) " &
                                        "VALUES (@id, @custid, '', @supid, 1, @doctot, 1, @cc, @createdby, NOW(), @invdate, 1, @exdeldate, @Notes, 0, 0, @pr_id)"
                        Else
                            sqlHeader = "INSERT INTO cld_invlk_rcv (id, custid, `Do`, supid, flag, doctot, Tipe, cc, createdby, createddate, InvDate, acc, exDelDate, Notes) " &
                                        "VALUES (@id, @custid, '', @supid, 1, @doctot, 1, @cc, @createdby, NOW(), @invdate, 1, @exdeldate, @Notes)"
                        End If
                    End If
                    Dim sqlDetail As String = "INSERT INTO cld_inv_trans (id, custid, InvDate, `type`, cc, stk_id, stk_name, u1, u2, factor1, ttlstock, costu1, cost, linkid, flagtrans, u1_nm, u2_nm, createdby, createddate, stk_no, Cc_name) " &
                                              "VALUES (@id, @custid, @invdate, 3, @cc, @stk_id, @stk_name, @u1, @u2, @factor1, @ttlstk, @costu1, @cost, @linkid, 0, @u1_nm, @u2_nm, @createdby, NOW(), @stk_no, @ccname)"

                    Try
                        ' If Update, delete existing items first to sync with current PR
                        If isUpdate Then
                            Using cmdDelDetail As New MySqlCommand("DELETE FROM cld_inv_trans WHERE linkid = @id AND custid = @custid", conn, rcvTrans)
                                cmdDelDetail.Parameters.AddWithValue("@id", idtrans)
                                cmdDelDetail.Parameters.AddWithValue("@custid", custId)
                                cmdDelDetail.ExecuteNonQuery()
                                errLog("GenerateRcvFromPR (Sup:" & supId & "): Existing items deleted for update of Order ID [" & idtrans & "]", "Info")
                            End Using
                        End If

                        Dim doctot As Decimal = 0

                        ' Insert/Update Header into cld_invlk_rcv
                        Using cmdHeader As New MySqlCommand(sqlHeader, conn, rcvTrans)
                            cmdHeader.Parameters.AddWithValue("@id", idtrans)
                            cmdHeader.Parameters.AddWithValue("@custid", custId)
                            cmdHeader.Parameters.AddWithValue("@supid", supId)
                            cmdHeader.Parameters.AddWithValue("@cc", ccId)
                            cmdHeader.Parameters.AddWithValue("@createdby", username)
                            cmdHeader.Parameters.AddWithValue("@doctot", 0) ' Will be updated after details
                            cmdHeader.Parameters.AddWithValue("@invdate", appDate.ToString("yyyy-MM-dd"))
                            cmdHeader.Parameters.AddWithValue("@exdeldate", requiredDate.ToString("yyyy-MM-dd"))
                            cmdHeader.Parameters.AddWithValue("@Notes", "Generated from PR #" & prId & ". " & notes)
                            If My.Settings.supp_tax = "1" Then
                                cmdHeader.Parameters.AddWithValue("@pr_id", prId)
                            End If
                            cmdHeader.ExecuteNonQuery()
                            errLog("GenerateRcvFromPR (Sup:" & supId & "): Header " & If(isUpdate, "updated", "inserted") & " successfully with ID [" & idtrans & "]", "Info")
                        End Using

                        ' Insert Details into cld_inv_trans
                        ' PREPARE Insert Command with VALUES
                        Using cmdInsDetail As New MySqlCommand(sqlDetail, conn, rcvTrans)
                            cmdInsDetail.Parameters.Add("@id", MySqlDbType.Int32)
                            cmdInsDetail.Parameters.AddWithValue("@custid", custId)
                            cmdInsDetail.Parameters.AddWithValue("@invdate", appDate.ToString("yyyy-MM-dd"))
                            cmdInsDetail.Parameters.AddWithValue("@cc", ccId)
                            cmdInsDetail.Parameters.Add("@stk_id", MySqlDbType.Int32)
                            cmdInsDetail.Parameters.Add("@stk_name", MySqlDbType.VarChar)
                            cmdInsDetail.Parameters.Add("@u1", MySqlDbType.Decimal)
                            cmdInsDetail.Parameters.Add("@u2", MySqlDbType.Decimal)
                            cmdInsDetail.Parameters.Add("@factor1", MySqlDbType.Decimal)
                            cmdInsDetail.Parameters.Add("@ttlstk", MySqlDbType.Decimal)
                            cmdInsDetail.Parameters.Add("@costu1", MySqlDbType.Decimal)
                            cmdInsDetail.Parameters.Add("@cost", MySqlDbType.Decimal)
                            cmdInsDetail.Parameters.AddWithValue("@linkid", idtrans)
                            cmdInsDetail.Parameters.Add("@u1_nm", MySqlDbType.VarChar)
                            cmdInsDetail.Parameters.Add("@u2_nm", MySqlDbType.VarChar)
                            cmdInsDetail.Parameters.AddWithValue("@createdby", username)
                            cmdInsDetail.Parameters.Add("@stk_no", MySqlDbType.VarChar)
                            cmdInsDetail.Parameters.AddWithValue("@ccname", "")

                            For Each row In group
                                Dim stkId = CInt(row("stk_id"))
                                Dim appQty = CDec(row("app_qty"))
                                Dim itemCost As Decimal = GetLatestCost(stkId, ccId, conn, rcvTrans)
                                If itemCost <= 0 AndAlso dt.Columns.Contains("cost") AndAlso Not IsDBNull(row("cost")) Then
                                    itemCost = CDec(row("cost"))
                                End If

                                Dim ttlCost = appQty * itemCost
                                doctot += ttlCost

                                ' Calculate NEW MAX ID manually to avoid empty table issues
                                Dim newId As Integer
                                Using cmdMax As New MySqlCommand("SELECT MAX(id) FROM cld_inv_trans", conn, rcvTrans)
                                    Dim maxIdObj = cmdMax.ExecuteScalar()
                                    newId = If(IsDBNull(maxIdObj), 1, Convert.ToInt32(maxIdObj) + 1)
                                End Using

                                cmdInsDetail.Parameters("@id").Value = newId
                                cmdInsDetail.Parameters("@stk_id").Value = stkId
                                cmdInsDetail.Parameters("@stk_name").Value = row("stk_name")
                                cmdInsDetail.Parameters("@u1").Value = 0
                                cmdInsDetail.Parameters("@u2").Value = appQty
                                cmdInsDetail.Parameters("@factor1").Value = row("factor1")
                                cmdInsDetail.Parameters("@ttlstk").Value = appQty
                                cmdInsDetail.Parameters("@costu1").Value = itemCost * CDec(row("factor1"))
                                cmdInsDetail.Parameters("@cost").Value = itemCost
                                cmdInsDetail.Parameters("@u1_nm").Value = row("u1_nm")
                                cmdInsDetail.Parameters("@u2_nm").Value = row("u2_nm")
                                cmdInsDetail.Parameters("@stk_no").Value = If(dt.Columns.Contains("stk_no"), row("stk_no"), "")
                                
                                cmdInsDetail.ExecuteNonQuery()
                                errLog("GenerateRcvFromPR (Sup:" & supId & "): Item [" & row("stk_name").ToString() & "] inserted successfully (Qty:" & appQty & ")", "Info")
                            Next
                        End Using

                        ' Update Header with FINAL doctot
                        Using cmdUpdHeaderFin As New MySqlCommand("UPDATE cld_invlk_rcv SET doctot = @doctot WHERE id = @id AND custid = @custid", conn, rcvTrans)
                            cmdUpdHeaderFin.Parameters.AddWithValue("@doctot", doctot)
                            cmdUpdHeaderFin.Parameters.AddWithValue("@id", idtrans)
                            cmdUpdHeaderFin.Parameters.AddWithValue("@custid", custId)
                            cmdUpdHeaderFin.ExecuteNonQuery()
                        End Using

                        rcvTrans.Commit()
                        errLog("GenerateRcvFromPR (Sup:" & supId & "): Commit successful for Order ID [" & idtrans & "]", "Info")
                        If generatedIds.Length > 0 Then generatedIds.Append(", ")
                        generatedIds.Append(idtrans)

                    Catch exMySql As MySqlException
                        rcvTrans.Rollback()
                        errLog("GenerateRcvFromPR MySqlError (Sup:" & supId & "): [" & exMySql.Number & "] " & exMySql.Message, "clsInventory_Warehouse:GenerateRcvFromPR")
                    Catch ex As Exception
                        rcvTrans.Rollback()
                        errLog("GenerateRcvFromPR group error (Sup:" & supId & "): " & ex.Message, "clsInventory_Warehouse:GenerateRcvFromPR")
                    End Try
                Next

                Return If(generatedIds.Length > 0, "Generated Orders: " & generatedIds.ToString(), "")
            End Using
        Catch ex As Exception
            errLog("Fatal error in GenerateRcvFromPR: " & ex.Message, "GenerateRcvFromPR")
        End Try
        Return ""
    End Function

    Private Function GetLatestCost(ByVal stkId As Integer, ByVal ccId As Integer, ByVal conn As MySqlConnection, ByVal trans As MySqlTransaction) As Decimal
        Try
            ' 1. Try today's or latest cost from transstockcls
            Using cmdCost As New MySqlCommand("SELECT Cost FROM cld_inv_transstockcls WHERE Stkid = @stkid AND Ccid = @ccid ORDER BY invdate DESC LIMIT 1", conn, trans)
                cmdCost.Parameters.AddWithValue("@stkid", stkId)
                cmdCost.Parameters.AddWithValue("@ccid", ccId)
                Dim cost = cmdCost.ExecuteScalar()
                If cost IsNot Nothing AndAlso Not IsDBNull(cost) AndAlso CDec(cost) > 0 Then
                    Return CDec(cost)
                End If
            End Using

            ' 2. Fallback: Try average cost from historical transactions (reasonable-cost)
            Using cmdAvg As New MySqlCommand("SELECT IFNULL(AVG(Cost), 0) FROM cld_inv_trans WHERE stk_id = @stkid AND cc = @ccid AND cost > 0", conn, trans)
                cmdAvg.Parameters.AddWithValue("@stkid", stkId)
                cmdAvg.Parameters.AddWithValue("@ccid", ccId)
                Dim avgCost = cmdAvg.ExecuteScalar()
                If avgCost IsNot Nothing AndAlso Not IsDBNull(avgCost) AndAlso CDec(avgCost) > 0 Then
                    Return CDec(avgCost)
                End If
            End Using

            ' 3. Fallback: Global average across all CCs for this item
            Using cmdGlobalAvg As New MySqlCommand("SELECT IFNULL(AVG(Cost), 0) FROM cld_inv_trans WHERE stk_id = @stkid AND cost > 0", conn, trans)
                cmdGlobalAvg.Parameters.AddWithValue("@stkid", stkId)
                Dim gAvgCost = cmdGlobalAvg.ExecuteScalar()
                If gAvgCost IsNot Nothing AndAlso Not IsDBNull(gAvgCost) AndAlso CDec(gAvgCost) > 0 Then
                    Return CDec(gAvgCost)
                End If
            End Using

        Catch ex As Exception
            ' Log or ignore
        End Try
        Return 0
    End Function

#End Region

End Class
