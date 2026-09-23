Public Class clsInventory_Invoice


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

            Dim a As New clsDbo("SELECT invoice.number,supp.`Name`, DATE_FORMAT(invoice.invoicedate,'%d %b %Y') AS invoicedate, invoice.invoicetot " &
                                "FROM `cld_inv_invoice` invoice LEFT JOIN `cld_invlk_rcv` rcv ON rcv.`invoiceID`=invoice.`ID` AND rcv.`CustID`=invoice.`CustID` " &
                                "LEFT JOIN `cld_inv_supplier` supp ON supp.id = rcv.`SupId` AND supp.`CustID` = rcv.`CustID` " &
                                "WHERE invoice.custid = " & Custid & " AND invoicedate ='" & invoicedate.ToString("yyyy-MM-dd 00:00:00") & "' " &
                                "GROUP BY invoice.number, invoice.invoicetot,supp.`Name`")


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



