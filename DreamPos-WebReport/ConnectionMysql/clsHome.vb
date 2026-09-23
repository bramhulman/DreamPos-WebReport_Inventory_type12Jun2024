Public Class clsHome

    'Dim strgofood As String = "'GOJEK', 'GOFOOD'"
    Private mfunction As ModFunction
    Public Function getds_homebrand(ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim cHome As New clsHome

        Try

            Dim a As New clsDbo("SELECT 0 as ID, 'ALL' AS name UNION ALL " &
                                     "SELECT ID , name3 FROM cld_brand " &
                                     "Where custid = " & custid & "" & ModPrvBrnd(Modpriv, user))
            ds = a.selectDataset
            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "clsWebService:getds_homebrand")
            Err.Clear()

        End Try

        Return ds
    End Function



    Public Function get_Outletcount(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xolcount As Integer = 0

        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT COUNT(DISTINCT o.`ID`) olcount FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " AND ssd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("olcount")) = False Then
                xolcount = ds.Tables(0).Rows(0)("olcount")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.ol_count")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xolcount

    End Function

    Public Function get_TotalSales(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(ssd.`Subtotal`-ssd.`Discount`) tsales FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " AND ssd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlSls")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function

    Public Function get_TotalSalesmonthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(s.`Subtotal`-s.`Discount`) tsales FROM cld_sumsalesdaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
                                "WHERE s.`CustID` = " & custid & " " & strbrand & " AND s.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And s.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlSls_monthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSlsPAT(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(ssd.`Subtotal`-ssd.`Discount`) tsales FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " and SlsTypeNm in (" & My.Settings.patSales & ") AND ssd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSlsPAT")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function

    Public Function get_TSlsPATmonthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(s.`Subtotal`-s.`Discount`) tsales FROM cld_sumsalesdaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
                                "WHERE s.`CustID` = " & custid & " " & strbrand & " and SlsTypeNm in (" & My.Settings.patSales & ") AND s.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And s.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSlsPATmonthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSlsdirdeliver(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(ssd.`Subtotal`-ssd.`Discount`) tsales FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " and SlsTypeNm in (" & My.Settings.dirdeliver & ") AND ssd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSlsPAT")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function

    Public Function get_TSlsdirdelivermonthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(s.`Subtotal`-s.`Discount`) tsales FROM cld_sumsalesdaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
                                "WHERE s.`CustID` = " & custid & " " & strbrand & " and SlsTypeNm in (" & My.Settings.dirdeliver & ") AND s.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And s.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSlsPATmonthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function

    Public Function get_TotalCover(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xtcover As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If
            Dim strFilter As String = ""

          
            Dim a As New clsDbo("SELECT sum(ssd.`Cover`) tcover FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " AND ssd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset
            If IsDBNull(ds.Tables(0).Rows(0)("tcover")) = False Then
                xtcover = ds.Tables(0).Rows(0)("tcover")
            End If


            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlCover")
            Err.Clear()

        End Try

        Return xtcover

    End Function

    Public Function get_TotalCoverMonthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xtcover As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If
            Dim strFilter As String = ""


            Dim a As New clsDbo("SELECT sum(cover) tcover FROM cld_sumsalesdaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
                                "WHERE s.`CustID` = " & custid & " " & strbrand & " AND s.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And s.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tcover")) = False Then
                xtcover = ds.Tables(0).Rows(0)("tcover")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlCover_monthly")
            Err.Clear()

        End Try

        Return xtcover

    End Function

    Public Function get_AvgCover(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xavgpcover As Decimal = 0

        Try

            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(ssd.`Subtotal`-ssd.`Discount`)/SUM(ssd.cover) avgpcover FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " AND ssd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset
            If IsDBNull(ds.Tables(0).Rows(0)("avgpcover")) = False Then
                xavgpcover = ds.Tables(0).Rows(0)("avgpcover")
            End If


            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.AvgpCover")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xavgpcover

    End Function

    Public Function get_AvgCover_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet
        Dim xavgpcover As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If

            Dim a As New clsDbo("SELECT SUM(s.`Subtotal`-s.`Discount`)/SUM(s.cover) avgpcover FROM cld_sumsalesdaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
                                "WHERE s.`CustID` = " & custid & " " & strbrand & " AND s.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And s.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("avgpcover")) = False Then
                xavgpcover = ds.Tables(0).Rows(0)("avgpcover")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.AvgpCover_monthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xavgpcover

    End Function

    Public Function get_TSls_Gofood(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.gofood & ") " & strbrand & " AND sdd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlSls_gofood")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSls_Gofood_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.gofood & ") " & strbrand & " AND sdd.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And sdd.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSls_Gofood_monthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSls_Grab(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.grab & ") " & strbrand & " AND sdd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlSls_gofood")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSls_Grab_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.grab & ") " & strbrand & " AND sdd.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And sdd.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSls_Grab_monthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSls_waorder(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.waorder & ") " & strbrand & " AND sdd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.TtlSls_waorder")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function get_TSls_waorder_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String) As String

        Dim ds As New DataSet

        Dim xtsales As Decimal
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If


            Dim a As New clsDbo("SELECT SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.waorder & ") " & strbrand & " AND sdd.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And sdd.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & "")

            ds = a.selectDataset

            If IsDBNull(ds.Tables(0).Rows(0)("tsales")) = False Then
                xtsales = ds.Tables(0).Rows(0)("tsales")
            End If

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.get_TSls_waorder_monthly")
            Err.Clear()

        End Try

        ds.Dispose()

        Return xtsales

    End Function
    Public Function getds_TopSales(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal top As Integer, ByVal Modpriv As Integer, ByVal user As String) As DataSet

        Dim ds As New DataSet
        Dim topstr As String = "desc"
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If

            If top = 2 Then
                topstr = "ASC"
            End If

            Dim a As New clsDbo("SELECT olname, SUM(ssd.`Subtotal`-ssd.`Discount`) tsales FROM cld_sumsalesdaily ssd LEFT JOIN " &
                                "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
                                "WHERE ssd.`CustID` = " & custid & " " & strbrand & " 
                                AND ssd.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And ssd.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & " " &
                                "group by o.brandid ,olid order by SUM(ssd.`Subtotal`-ssd.`Discount`) " & topstr & " limit 5 ")

            'Dim a As New clsDbo("SELECT olname, SUM(ssd.`Subtotal`-ssd.`Discount`) tsales FROM cld_sumsalesdaily ssd LEFT JOIN " &
            '                    "cld_outlet o ON ssd.`CustID` = o.`custID` And ssd.`olId` = o.`ID` " &
            '                    "WHERE ssd.`CustID` = " & custid & " " & strbrand & " AND YEAR(ssd.busidate) = YEAR(NOW()) AND MONTH(ssd.busidate)=MONTH(NOW()) " & ModPrv(Modpriv, user) & " " &
            '                    "group by o.brandid ,olid order by SUM(ssd.`Subtotal`-ssd.`Discount`) " & topstr & " limit 5 ")

            ds = a.selectDataset


            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.ds_TopSls")
            Err.Clear()

        End Try

        Return ds

    End Function


    'tambahan 4 sept 2018
    Public Function getds_TopSales_kosong() As DataSet

        Dim ds As New DataSet

        Try

            Dim a As New clsDbo(" SELECT'-' AS olname,0 AS tsales ")
            ds = a.selectDataset

            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.ds_TopSls_kosong")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function getds_TopItem_fnb_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal top As Integer, ByVal fnb As Integer, ByVal Modpriv As Integer, ByVal user As String) As DataSet

        Dim ds As New DataSet
        Dim topstr As String = "desc"
        Dim fnbstr As String = ""

        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If

            If top = 2 Then
                topstr = "ASC"
            End If
            If fnb = 1 Then
                fnbstr = "FOOD"
            ElseIf fnb = 2 Then
                fnbstr = "BEVERAGES"
            End If
            Dim a As New clsDbo("SELECT s.ItemName item,SUM(s.qty) qty,SUM(s.total) netamount FROM cld_sumsalesdetaildaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                                "WHERE s.`CustID` = " & custid & " " & strbrand & " AND s.MajorGroup = '" & fnbstr & "' AND YEAR(s.busidate) = YEAR(NOW()) AND MONTH(s.busidate)=MONTH(NOW()) " & ModPrv(Modpriv, user) & " " &
                                "group by ItemNo order by SUM(s.Qty) " & topstr & " limit 5 ")
            ds = a.selectDataset


            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.getds_TopItem_fnb_monthly")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function getds_TopItem_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal fnb As Integer, ByVal Modpriv As Integer, ByVal user As String, Optional ByVal slstype As Integer = 0) As DataSet

        Dim ds As New DataSet
        Dim fnbstr As String = ""
        Dim slstypestr As String = ""

        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If

            If fnb = 1 Then
                fnbstr = "FOOD"
            ElseIf fnb = 2 Then
                fnbstr = "BEVERAGES"
            End If
            If slstype = 1 Then
                slstypestr = "and SlsTypeNm in (" & My.Settings.patSales & "," & My.Settings.dirdeliver & ")"
            ElseIf slstype = 2 Then
                slstypestr = "and SlsTypeNm in (" & My.Settings.gofood & "," & My.Settings.grab & ")"
            End If
            Dim a As New clsDbo("SELECT s.ItemName item,SUM(s.qty) qty,SUM(s.total) netamount FROM cld_sumsalesdetaildaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                                "WHERE s.total>0 and s.`CustID` = " & custid & " " & strbrand & " " & slstypestr & " AND s.MajorGroup = '" & fnbstr & "' AND s.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And s.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " & ModPrv(Modpriv, user) & " " &
                                "group by ItemNo order by SUM(s.Qty) desc limit 5 ")
            ds = a.selectDataset


            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.getds_TopItem_monthly")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function getds_TopItem_kosong() As DataSet

        Dim ds As New DataSet

        Try

            Dim a As New clsDbo(" SELECT'-' AS item,0 AS qty,0 AS netamount  ")
            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.ds_TopItem_kosong")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Function getds_TopSales_gofood(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal top As Integer, ByVal Modpriv As Integer, ByVal user As String) As DataSet

        Dim ds As New DataSet
        Dim topstr As String = "desc"
        Try
            Dim strbrand As String = ""

            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If

            If top = 2 Then
                topstr = "ASC"
            End If

            Dim a As New clsDbo("SELECT olname,SUM(sdd.`Subtotal`-sdd.`Discount`) tsales FROM cld_sumsalesdaily sdd LEFT JOIN " &
                                "cld_outlet o ON sdd.`CustID` = o.`custID` And sdd.`olId` = o.`ID` " &
                                "WHERE sdd.`CustID` = " & custid & " AND sdd.SlsTypeNm IN(" & My.Settings.gofood & ") " & strbrand & " AND sdd.`BusiDate` = '" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "'" & ModPrv(Modpriv, user) & " " &
                                "group by o.brandid ,olid order by SUM(sdd.total) " & topstr & " limit 5 ")

            ds = a.selectDataset


            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.ds_TopSls_gofood")
            Err.Clear()

        End Try

        Return ds

    End Function
    Public Function getds_TopItem_fnb_gofood_monthly(ByVal brandid As Integer, ByVal Tgl1 As Date, ByVal custid As Integer, ByVal top As Integer, ByVal fnb As Integer, ByVal Modpriv As Integer, ByVal user As String) As DataSet

        Dim ds As New DataSet
        Dim topstr As String = "desc"
        Dim fnbstr As String = ""
        Try
            Dim strbrand As String = ""
            If brandid > 0 Then
                strbrand = "AND o.`brandID` = " & brandid & ""
            End If

           
            If top = 2 Then
                topstr = "ASC"
            End If
            If fnb = 1 Then
                fnbstr = "FOOD"
            ElseIf fnb = 2 Then
                fnbstr = "BEVERAGES"
            End If
            Dim a As New clsDbo("SELECT s.ItemName item,SUM(s.qty) qty,SUM(s.total) netamount FROM cld_sumsalesdetaildaily s LEFT JOIN " &
                                "cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                                "WHERE s.total>0 and s.`CustID` = " & custid & " AND s.SlsTypeNm IN(" & My.Settings.gofood & ") " & strbrand & " AND s.MajorGroup = '" & fnbstr & "' AND YEAR(s.busidate) = YEAR(NOW()) AND MONTH(s.busidate)=MONTH(NOW()) " & ModPrv(Modpriv, user) & " " &
                                "group by ItemNo order by SUM(s.Qty) " & topstr & " limit 5 ")
            ds = a.selectDataset


            a.dispose()
            a = Nothing


        Catch ex As Exception
            mfunction.errLog(Err.Description, "ClsHome.ds_TopSls_fnb_monthly")
            Err.Clear()

        End Try

        Return ds

    End Function


    Public Function ModPrv(ByVal ModPrive As Integer, ByVal name As String) As String
        'tambahan untuk select
        If ModPrive And ModFunction.usrModulepriv.moduleuser = ModFunction.usrModulepriv.moduleuser Then
            Return "and olId in (Select store from cld_userxstore where user = '" & name & "')"
        Else
            Return ""
        End If

    End Function

    Public Function ModPrvBrnd(ByVal ModPrive As Integer, ByVal sUSerName As String) As String
        'tambahan untuk select
        If ModPrive And ModFunction.usrModulepriv.moduleuser = ModFunction.usrModulepriv.moduleuser Then

            Return " and ID in(SELECT o.brandid FROM cld_userxstore us " &
                    "LEFT JOIN cld_outlet o On us.store = o.id " &
                    "WHERE USER = '" & sUSerName & "')"
        Else
            Return ""
        End If

    End Function

End Class
