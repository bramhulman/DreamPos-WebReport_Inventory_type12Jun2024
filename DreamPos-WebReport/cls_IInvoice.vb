
Public Class cls_IInvoice

    Public Function Selectrcv_invoice_col(ByVal CustID As Integer, ByVal SupId As Integer) As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("SELECT rcv.id, rcv.do, rcv.`DocTot`, CASE cc.olId WHEN 0 THEN cc.Name ELSE o.nama END AS outlet, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_cc` cc ON (cc.ID = rcv.cc AND cc.custid = rcv.custid) " & _
                                "LEFT JOIN `cld_outlet` o ON (cc.olid = o.ID AND o.custid = rcv.custid) " & _
                                "WHERE(rcv.invoiceID = 0 And rcv.custid = " & CustID & " And rcv.SupId = " & SupId & ") " & _
                                " LIMIT 0")

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:SelectInv_invoice")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function Selectrcv_invoice(ByVal CustID As Integer, ByVal SupId As Integer, Optional ByVal stext As String = "") As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("SELECT rcv.id, rcv.do, rcv.`DocTot`, CASE cc.olId WHEN 0 THEN cc.Name ELSE o.nama END AS outlet, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_cc` cc ON (cc.ID = rcv.cc AND cc.custid = rcv.custid) " & _
                                "LEFT JOIN `cld_outlet` o ON (cc.olid = o.ID AND o.custid = rcv.custid) " & _
                                "WHERE(rcv.invoiceID = 0 And rcv.custid = " & CustID & " And rcv.SupId = " & SupId & ") " & _
                                " AND (rcv.id LIKE '%" & stext & "%' OR rcv.do LIKE '%" & stext & "%' OR o.nama LIKE '%" & stext & "%' OR CASE cc.olId WHEN 0 THEN cc.Name ELSE o.nama END LIKE '%" & stext & "%') ORDER BY rcv.CreatedDate")

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:SelectInv_invoice")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function Selectget_invoice(ByVal CustID As Integer, ByVal SupId As Integer, Optional ByVal stext As String = "") As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("SELECT rcv.id, rcv.do, rcv.`DocTot`, CASE cc.olId WHEN 0 THEN cc.Name ELSE o.nama END AS outlet, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_cc` cc ON (cc.ID = rcv.cc AND cc.custid = rcv.custid) " & _
                                "LEFT JOIN `cld_outlet` o ON (cc.olid = o.ID AND o.custid = rcv.custid) " & _
                                "WHERE(rcv.invoiceID <> 0 And rcv.custid = " & CustID & " And rcv.SupId = " & SupId & ") " & _
                                " AND (rcv.id LIKE '%" & stext & "%' OR rcv.do LIKE '%" & stext & "%' OR o.nama LIKE '%" & stext & "%' OR CASE cc.olId WHEN 0 THEN cc.Name ELSE o.nama END LIKE '%" & stext & "%') ORDER BY rcv.CreatedDate")

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:SelectInv_invoice")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function Selectrcv_invByID(ByVal CustID As Integer, ByVal ID As String) As DataSet
        Dim ds As New DataSet

        Try


            Dim a As New clsDbo("SELECT rcv.id, rcv.do, rcv.`DocTot`, CASE cc.olId WHEN 0 THEN cc.Name ELSE o.nama END AS outlet, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_cc` cc ON (cc.ID = rcv.cc AND cc.custid = rcv.custid) " & _
                                "LEFT JOIN `cld_outlet` o ON (cc.olid = o.ID AND o.custid = rcv.custid) " & _
                                "WHERE(rcv.invoiceID = 0 And rcv.custid = " & CustID & ") " & _
                                "AND (rcv.id='" & ID & "') ")

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:Selectrcv_invByID")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function Selectrcv_transByID(ByVal CustID As Integer, ByVal ID As String) As DataSet
        Dim ds As New DataSet

        Try


            Dim a As New clsDbo("SELECT rcv.id, rcv.do, trans.Stk_Id, trans.Stk_Name, trans.U1, trans.U1_nm, trans.U2, trans.U2_nm, trans.Factor1, trans.ttlStock,trans.CostU1, trans.Cost, SUM(trans.ttlStock*trans.Cost) as ttlCost " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_trans` trans ON (trans.Linkid = rcv.ID AND trans.custid = rcv.custid) " & _
                                "WHERE(rcv.invoiceID = 0 And rcv.custid = " & CustID & ") " & _
                                "AND (rcv.id='" & ID & "') ")

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:Selectrcv_invByID")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function getsupplierinv(ByVal CustID As Integer) As DataSet
        Dim ds As New DataSet

        Try

            Dim a As New clsDbo("SELECT DISTINCT supp.id, supp.NAME " & _
                                "FROM `cld_inv_supplier` supp INNER JOIN `cld_invlk_rcv` rcv ON supp.id = rcv.`SupId` AND supp.custid = rcv.custid " & _
                                "WHERE supp.id IN (SELECT SupId FROM `cld_invlk_rcv` WHERE CustID=" & CustID & " AND invoiceID=0) AND supp.`CustID`=" & CustID)


            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:getsupplierinv")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function getmaxid_invoice(ByVal custid As Integer) As Integer

        Dim no As Integer
        Dim c_cmd As New Cls_command
        Try

            Dim str As String


            str = "SELECT max(ID) from cld_inv_invoice where CustID=" & custid

            no = c_cmd.ExecuteScalar(str)

        Catch ex As Exception

            errLog(Err.Description, "clsInventoryInvoice:getmaxid_invoice")
            Err.Clear()
        End Try
        c_cmd = Nothing
        Return no
    End Function
    Public Function Update_invrcv(ByVal IDrcv As String, ByVal CustID As Integer, ByVal idinvoice As Integer) As Boolean
        Dim c_cmd As New Cls_command
        Dim hasil As Boolean = False
        Dim str As String
        Try
            str = "Update cld_invlk_rcv set invoiceID = " & idinvoice & _
                " where ID = '" & IDrcv & "' and CustID = " & CustID & ""
            hasil = c_cmd.executeNonQueryImport(str)

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:Update_invrcv")
            Err.Clear()
        End Try
        c_cmd = Nothing
        Return hasil
    End Function
    Public Function InsertInv_invoice(ByVal CustID As Integer, ByVal number As String, ByVal invoiceDate As Date, ByVal invoiceTot As Decimal, ByVal notes As String) As Integer
        Dim c_cmd As New Cls_command

        Dim str As String
        Dim idinvoice As Integer = 0

        Try
            Dim getid As Integer = getmaxid_invoice(CustID)
            str = "insert into cld_inv_invoice(ID,CustID, number,invoiceDate,invoiceTot,notes)values (" & getid & ", " & CustID & ", '" & number & "', '" & invoiceDate.ToString("yyyy-MM-dd HH:mm:ss") & "', " & invoiceTot & ", '" & notes & "')"
            If c_cmd.executeNonQueryImport(str) Then
                idinvoice = getid
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:InsertInv_invoice")
            Err.Clear()
        End Try
        c_cmd = Nothing
        Return idinvoice
    End Function

    Public Function getDS_InvInvoice_all(ByVal Custid As Integer) As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("SELECT number, DATE_FORMAT(invoicedate,'%d %b %Y') AS invoicedate, invoicetot " & _
                                "FROM `cld_inv_invoice` " & _
                                "WHERE custid = " & Custid)



            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:getDS_InvInvoice")
            Err.Clear()
        End Try


        Return ds

    End Function
    Public Function getDS_InvInvoice(ByVal Custid As Integer, ByVal invoicedate As Date) As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("SELECT invoice.number,supp.`Name`, DATE_FORMAT(invoice.invoicedate,'%d %b %Y') AS invoicedate, invoice.invoicetot " & _
                                "FROM `cld_inv_invoice` invoice LEFT JOIN `cld_invlk_rcv` rcv ON rcv.`invoiceID`=invoice.`ID` AND rcv.`CustID`=invoice.`CustID` " & _
                                "LEFT JOIN `cld_inv_supplier` supp ON supp.id = rcv.`SupId` AND supp.`CustID` = rcv.`CustID` " & _
                                "WHERE invoice.custid = " & Custid & " AND invoicedate ='" & invoicedate.ToString("yyyy-MM-dd 00:00:00") & "' " & _
                                "GROUP BY invoice.number ")



            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryInvoice:getDS_InvInvoice")
            Err.Clear()
        End Try


        Return ds

    End Function
End Class
