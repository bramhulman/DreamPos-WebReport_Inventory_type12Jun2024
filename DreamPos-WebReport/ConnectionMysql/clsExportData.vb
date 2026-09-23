Public Class clsExportData

    Public Function get_Exp_Datatable(ByVal CustID As Integer, ByVal sOutlet As String, ByVal exportname As String, ByVal dFrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As Integer) As DataTable
        Dim dsRslt As New DataTable

        Select Case exportname.ToUpper

            Case "DailySales".ToUpper
                dsRslt = SelectDataTable_DailySales(CustID, sOutlet, dFrom, dTo, iOwn, iBrand)
            Case "MonthlySales".ToUpper
                dsRslt = SelectDataTable_MonthlySales(CustID, sOutlet, dFrom, iOwn, iBrand)
            Case "ItemGofood".ToUpper
                dsRslt = SelectDataTable_GoFood(CustID, sOutlet, dFrom, iOwn, iBrand)
            Case "ItemGofoodDaily".ToUpper
                dsRslt = SelectDataTable_GoFood_Daily(CustID, sOutlet, dFrom, dTo, iOwn, iBrand)

        End Select

        Return dsRslt

    End Function

    Public Function get_Exp_Datatable2(ByVal CustID As Integer, ByVal sOutlet As String, ByVal exportname As String, ByVal dFrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As Integer, ByVal iCity As Integer) As DataTable
        Dim dsRslt As New DataTable

        Select Case exportname.ToUpper

            Case "Sales_Item".ToUpper
                dsRslt = SelectDataTable_SlsItem(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
            Case "Monthly_Channel".ToUpper
                dsRslt = SelectDataTable_MonChannel(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
            Case "Sales_Cover".ToUpper
                dsRslt = SelectDataTable_slsCover(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
            Case "Sales_vs_Payment".ToUpper
                dsRslt = SelectDataTable_SalesVSPayment(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)

        End Select

        Return dsRslt

    End Function

    Public Function get_Exp_Dataset(ByVal CustID As Integer, ByVal sOutlet As String, ByVal exportname As String, ByVal dFrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As Integer) As DataSet
        Dim dsRslt As New DataSet

        Select Case exportname.ToUpper

            Case "DailySales".ToUpper
                dsRslt = SelectDataset_DailySales(CustID, sOutlet, dFrom, dTo, iOwn, iBrand)
            Case "MonthlySales".ToUpper
                dsRslt = SelectDataset_MonthlySales(CustID, sOutlet, dFrom, iOwn, iBrand)
            Case "ItemGofood".ToUpper
                dsRslt = SelectDataset_Gofood(CustID, sOutlet, dFrom, iOwn, iBrand)
            Case "ItemGofoodDaily".ToUpper
                dsRslt = SelectDataset_Gofood_Daily(CustID, sOutlet, dFrom, dTo, iOwn, iBrand)

        End Select

        Return dsRslt

    End Function

    Public Function get_Exp_Dataset2(ByVal CustID As Integer, ByVal sOutlet As String, ByVal exportname As String, ByVal dFrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataSet
        Dim dsRslt As New DataSet

        Select Case exportname.ToUpper

            Case "Sales_Item".ToUpper
                dsRslt = SelectDataset_SlsItem(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
            Case "Monthly_Channel".ToUpper
                dsRslt = SelectDataset_MonChannel(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
            Case "Sales_Cover".ToUpper
                dsRslt = SelectDataset_SlsCover(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
            Case "Sales_vs_Payment".ToUpper
                dsRslt = SelectDataset_SalesVSPayment(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)

        End Select

        Return dsRslt

    End Function

    Public Function SelectDataset_Kepemilikan(ByVal CustID As Integer) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
                        "Select Id, Name from cld_own where custid = " & CustID

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Kepemilikan")
            Err.Clear()
        End Try


        Return ds


    End Function
    '26Apr2019
    Public Function SelectDataset_Brand(ByVal CustID As Integer, ByVal sText As String, Optional ByVal withall As Boolean = True) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            If withall Then
                strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
                      "Select ID, Name3 from cld_brand where custid = " & CustID &
                " and ( cld_brand.Name like  '%" & sText & "%' Or cld_brand.Name2 like '%" & sText & "%' Or cld_brand.Name3 like '%" & sText & "%' ) order by id"
            Else
                strfilter = "Select ID, Name3 as nama from cld_brand where custid = " & CustID &
                " and ( cld_brand.Name like  '%" & sText & "%' Or cld_brand.Name2 like '%" & sText & "%' Or cld_brand.Name3 like '%" & sText & "%' ) order by id"
            End If

            Dim c_Cmd As New clsDbo(ServerString)

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Brand")
            Err.Clear()
        End Try


        Return ds


    End Function

    Public Function SelectDataset_Outlet(ByVal iKem As Integer, ByVal ibrand As String, ByVal CustID As Integer, ByVal sText As String, ByVal sUser As String, ByVal ModPriv As Integer) As DataSet

        Dim ds As New DataSet

        Dim sKemp As String = ""
        Dim sBrand As String = ""
        Dim strstring As String = ""

        Try
            If iKem <> 0 Then
                sKemp = " and ownID = " & iKem
            End If

            If ibrand <> 0 Then
                sBrand = " and brandid in(" & ibrand & ")"
            End If

            If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strstring = "Select Kode, nama, Kode2 from cld_outlet where custid = " & CustID & sKemp & sBrand &
                    " and id in (Select store from cld_userxstore where user = '" & sUser & "')" &
                    " and ( cld_outlet.Kode like  '%" & sText & "%' Or cld_outlet.nama like '%" & sText & "%' Or cld_outlet.Kode2 like '%" & sText & "%' ) order by Kode limit 100"


            Else

                strstring = "Select Kode, nama, Kode2 from cld_outlet where custid = " & CustID & sKemp & sBrand &
                                  " and ( cld_outlet.Kode like  '%" & sText & "%' Or cld_outlet.nama like '%" & sText & "%' Or cld_outlet.Kode2 like '%" & sText & "%' ) order by Kode limit 100"

            End If

            Dim c_Cmd As New clsDbo(strstring)





            ds = c_Cmd.ReturnDataset(strstring)






            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Brand")
            Err.Clear()
        End Try


        Return ds


    End Function

    'Bram4Sep2018 Update Report
    Public Function SelectDataset_City(ByVal CustID As Integer, ByVal sText As String) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            strfilter = "Select ID, kota from cld_city where custid = " & CustID &
            " And (cld_city.Name Like  '%" & sText & "%' Or cld_city.kota like '%" & sText & "%') order by id"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_City")
            Err.Clear()
        End Try


        Return ds


    End Function

    'ihsan 13 Feb 2020
    Public Function SelectDataset_Store(ByVal CustID As Integer, ByVal sText As String) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            strfilter = "Select * from cld_outlet where custid = " & CustID &
            " order by id"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Store")
            Err.Clear()
        End Try


        Return ds


    End Function

    'Bram04Sep2018
    'outlet_ParamCity
    Public Function SelectDataset_Outlet2(ByVal iown As String, ByVal ibrand As String, ByVal iCity As String, ByVal CustID As Integer, ByVal sText As String, ByVal sUser As String, ByVal ModPriv As Integer) As DataSet

        Dim ds As New DataSet

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""
        Dim sout As String = ""
        Dim strstring As String = ""

        Try
            If ibrand = "0" Or ibrand = "" Then
                sBrand = ""
            Else
                sBrand = " and brandID in (" & ibrand & ")"
            End If

            If iown = "0" Or iown = "" Then
                sOwn = ""
            Else
                sOwn = " and ownID = " & iown
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cityID in (" & iCity & ")"
            End If

            If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                sout = " and Id in (Select Store From cld_userxstore where user = '" & sUser & "')"

            End If




            strstring = "SELECT '0' AS kode, 'All Store' AS Nama, 'All Store' AS Kode2 UNION ALL " &
                "Select Kode, nama, Kode2 from cld_outlet where custid = " & CustID & sOwn & sBrand & sCity & sout &
                " and ( cld_outlet.Kode like  '%" & sText & "%' Or cld_outlet.nama like '%" & sText & "%' Or cld_outlet.Kode2 like '%" & sText & "%' ) order by Kode limit 100"


            Dim c_Cmd As New clsDbo(strstring)



            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Outlet2")
            Err.Clear()
        End Try


        Return ds


    End Function

    'perubahan 20 des 2017
    'subtotal
    '26Apr2019
    Private Function Query_DailySales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and smd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        sStr = "SELECT  o.Kode AS l1, o.Nama AS l2, DATE_FORMAT(smd.BusiDate, '%c/%d/%Y') AS l3, sum(smd.Subtotal - smd.Discount) AS l4, 'actual' l5, MONTH(smd.BusiDate) l6, " &
               "CASE  MONTH(smd.BusiDate) " &
               "WHEN 1 THEN 'Q1' " &
               "WHEN 2 THEN 'Q1' " &
               "WHEN 3 THEN 'Q1' " &
               "WHEN 4 THEN 'Q2' " &
               "WHEN 5 THEN 'Q2' " &
               "WHEN 6 THEN 'Q2' " &
               "WHEN 7 THEN 'Q3' " &
               "WHEN 8 THEN 'Q3' " &
               "WHEN 9 THEN 'Q3' " &
               "ELSE 'Q4' " &
               "END AS l7 " &
               ",YEAR(smd.BusiDate) l8, brd.Name l9, '' l10, '' l11, '' l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
               "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "Where smd.custid = " & CustID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand &
               " Group by o.Kode2, smd.BusiDate, brd.Name"
        'sStr = "SELECT  o.Kode2 AS l1, DATE_FORMAT(smd.BusiDate, '%c/%d/%Y') AS l2, sum(smd.Subtotal - smd.Discount) AS l3, 'actual' l4, MONTH(smd.BusiDate) l5, " &
        '       "CASE  MONTH(smd.BusiDate) " &
        '       "WHEN 1 THEN 'Q1' " &
        '       "WHEN 2 THEN 'Q1' " &
        '       "WHEN 3 THEN 'Q1' " &
        '       "WHEN 4 THEN 'Q2' " &
        '       "WHEN 5 THEN 'Q2' " &
        '       "WHEN 6 THEN 'Q2' " &
        '       "WHEN 7 THEN 'Q3' " &
        '       "WHEN 8 THEN 'Q3' " &
        '       "WHEN 9 THEN 'Q3' " &
        '       "ELSE 'Q4' " &
        '       "END AS l6 " &
        '       ",YEAR(smd.BusiDate) l7, brd.Name l8, '' l9, '' l10, '' l11, '' l12 " &
        '       "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
        '       "ON smd.CustID = o.custID AND smd.olId = o.id " &
        '       "INNER JOIN cld_brand brd " &
        '       "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
        '       "Where smd.custid = " & CustID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand &
        '       " Group by o.Kode2, smd.BusiDate, brd.Name"

        Return sStr

    End Function

    'perubahan 20 des 2017
    'subtotal 
    '26Apr2019
    Private Function Query_monthlySales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and smd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        sStr = "SELECT sum(smd.Subtotal - smd.Discount) AS l1, o.Kode AS l2, o.Nama AS l3, DATE_FORMAT(smd.BusiDate, '%c/%Y') AS l4,  'Actual' l5, MONTH(smd.BusiDate) l6, " &
               "CASE  MONTH(smd.BusiDate) " &
               "WHEN 1 THEN 'Q1' " &
               "WHEN 2 THEN 'Q1' " &
               "WHEN 3 THEN 'Q1' " &
               "WHEN 4 THEN 'Q2' " &
               "WHEN 5 THEN 'Q2' " &
               "WHEN 6 THEN 'Q2' " &
               "WHEN 7 THEN 'Q3' " &
               "WHEN 8 THEN 'Q3' " &
               "WHEN 9 THEN 'Q3' " &
               "ELSE 'Q4' " &
               "END AS l7 " &
               ",YEAR(smd.BusiDate) l8, brd.Name l9 , ct.Name l10, own.Name l11,  ar.Name l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
               "FROM cld_sumsales smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "INNER JOIN cld_own own " &
               "ON own.CustID = o.custID AND own.ID = o.ownid " &
               "INNER JOIN cld_city ct " &
               "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
               "INNER JOIN cld_area ar " &
               "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
               "Where month(busidate) = " & dDate.Month & " and year(busidate) = " & dDate.Year & " and smd.custid = " & CustID & sOut & sOwn & sBrand &
               " Group by o.Kode2, smd.BusiDate, brd.Name"
        'sStr = "SELECT sum(smd.Subtotal - smd.Discount) AS l1, o.Kode2 AS l2, DATE_FORMAT(smd.BusiDate, '%c/%Y') AS l3,  'Actual' l4, MONTH(smd.BusiDate) l5, " &
        '       "CASE  MONTH(smd.BusiDate) " &
        '       "WHEN 1 THEN 'Q1' " &
        '       "WHEN 2 THEN 'Q1' " &
        '       "WHEN 3 THEN 'Q1' " &
        '       "WHEN 4 THEN 'Q2' " &
        '       "WHEN 5 THEN 'Q2' " &
        '       "WHEN 6 THEN 'Q2' " &
        '       "WHEN 7 THEN 'Q3' " &
        '       "WHEN 8 THEN 'Q3' " &
        '       "WHEN 9 THEN 'Q3' " &
        '       "ELSE 'Q4' " &
        '       "END AS l6 " &
        '       ",YEAR(smd.BusiDate) l7, brd.Name l8 , ct.Name l9, own.Name l10,  ar.Name l11, '' l12 " &
        '       "FROM cld_sumsales smd INNER JOIN cld_outlet o " &
        '       "ON smd.CustID = o.custID AND smd.olId = o.id " &
        '       "INNER JOIN cld_brand brd " &
        '       "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
        '       "INNER JOIN cld_own own " &
        '       "ON own.CustID = o.custID AND own.ID = o.ownid " &
        '       "INNER JOIN cld_city ct " &
        '       "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
        '       "INNER JOIN cld_area ar " &
        '       "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
        '       "Where month(busidate) = " & dDate.Month & " and year(busidate) = " & dDate.Year & " and smd.custid = " & CustID & sOut & sOwn & sBrand &
        '       " Group by o.Kode2, smd.BusiDate, brd.Name"

        Return sStr

    End Function

    'perubahan 20 des 2017
    'subtotal
    'perubahan 22 des 2017
    'datenya ga pake format
    Private Function Query_DailySalesExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and smd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If

        ' sStr = "SELECT  o.Kode2 AS Outlet, DATE_FORMAT(smd.BusiDate, '%c/%d/%Y') AS Date, (smd.Subtotal - smd.discount) AS Amount, 'actual' Type, MONTH(smd.BusiDate) wpb, " &

        sStr = "SELECT  o.Kode2 AS Outlet, smd.BusiDate AS Date, sum(smd.Subtotal - smd.Discount) AS Amount, 'actual' Type, MONTH(smd.BusiDate) wpb, " &
               "CASE  MONTH(smd.BusiDate) " &
               "WHEN 1 THEN 'Q1' " &
               "WHEN 2 THEN 'Q1' " &
               "WHEN 3 THEN 'Q1' " &
               "WHEN 4 THEN 'Q2' " &
               "WHEN 5 THEN 'Q2' " &
               "WHEN 6 THEN 'Q2' " &
               "WHEN 7 THEN 'Q3' " &
               "WHEN 8 THEN 'Q3' " &
               "WHEN 9 THEN 'Q3' " &
               "ELSE 'Q4' " &
               "END AS quarter " &
               ",YEAR(smd.BusiDate) year, brd.Name brand " &
               "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "Where smd.custid = " & CustID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand &
               " Group by o.Kode2, smd.BusiDate, brd.Name"

        Return sStr

    End Function

    'perubahan 20 des 2017
    'subtotal 
    'perubahan 22 des 2017
    'datenya ga pake format
    Private Function Query_monthlySalesExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and smd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If

        'sStr = "SELECT  (smd.Subtotal - smd.discount) AS Amount, o.Kode2 AS 'Kode outlet', DATE_FORMAT(smd.BusiDate, '%c/%Y') AS Month,  'Actual' Type, MONTH(smd.BusiDate) wpb, " &

        sStr = "SELECT Sum(smd.Subtotal - smd.Discount) AS Amount, o.Kode2 AS 'Kode outlet', smd.BusiDate AS Month,  'Actual' Type, MONTH(smd.BusiDate) wpb, " &
               "CASE  MONTH(smd.BusiDate) " &
               "WHEN 1 THEN 'Q1' " &
               "WHEN 2 THEN 'Q1' " &
               "WHEN 3 THEN 'Q1' " &
               "WHEN 4 THEN 'Q2' " &
               "WHEN 5 THEN 'Q2' " &
               "WHEN 6 THEN 'Q2' " &
               "WHEN 7 THEN 'Q3' " &
               "WHEN 8 THEN 'Q3' " &
               "WHEN 9 THEN 'Q3' " &
               "ELSE 'Q4' " &
               "END AS quarter " &
               ",YEAR(smd.BusiDate) year, brd.Name brand , ct.Name kota, own.Name owner,  ar.Name area " &
               "FROM cld_sumsales smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "INNER JOIN cld_own own " &
               "ON own.CustID = o.custID AND own.ID = o.ownid " &
               "INNER JOIN cld_city ct " &
               "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
               "INNER JOIN cld_area ar " &
               "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
               "Where month(busidate) = " & dDate.Month & " and year(busidate) = " & dDate.Year & " and smd.custid = " & CustID & sOut & sOwn & sBrand &
               " Group by o.Kode2, smd.BusiDate, brd.Name"

        Return sStr

    End Function

    Public Function SelectDataset_DailySales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_DailySales(CustID, sOutlet, dfrom, dTo, iOwn, iBrand)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_DailySales")
            Err.Clear()
        End Try


        Return ds


    End Function

    Public Function SelectDataset_MonthlySales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet

        Dim ds As New DataSet
        Dim sStr As String

        Try

            sStr = Query_monthlySales(CustID, sOutlet, dDate, iOwn, iBrand)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_MonthlySales")
            Err.Clear()
        End Try


        Return ds


    End Function






    Public Function SelectDataTable_DailySales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_DailySalesExcel(CustID, sOutlet, dfrom, dTo, iOwn, iBrand)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataTable_MonthlySales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_monthlySalesExcel(CustID, sOutlet, dDate, iOwn, iBrand)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    'Tambahan 18 jan 2018
    '26Apr2019
    Private Function Query_GoFoodSales(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""

        Dim sSlsType As String = ""

        If My.Settings.gofood = "" Then
            sSlsType = ""
        Else
            sSlsType = " and ssd.SlsTypeNm in (" & My.Settings.gofood & ")"
        End If

        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and ssd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        sStr = "SELECT ItemNo as l1, ItemName as l2, Qty as l3, total as l4, o.Kode2 as l5, DATE_FORMAT(ssd.BusiDate, '%c/%d/%Y') AS l6, '' as l7, '' as l8, '' as l9, '' as l10, '' as l11, '' l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
               "FROM cld_sumsalesdetail ssd INNER JOIN cld_outlet o " &
               "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
               "Where ssd.custid = " & CustID & " and month(ssd.BusiDate) = " & dDate.Month & " and year(ssd.BusiDate) = " & dDate.Year & sOut & sOwn & sBrand & sSlsType

        'sStr = "SELECT ItemNo as l1, ItemName as l2, Qty as l3, total as l4, o.Kode2 as l5, DATE_FORMAT(ssd.BusiDate, '%c/%d/%Y') AS l6, '' as l7, '' as l8, '' as l9, '' as l10, '' as l11, '' as l12 " &
        '       "FROM cld_sumsalesdetail ssd INNER JOIN cld_outlet o " &
        '       "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
        '       "Where ssd.custid = " & CustID & " and month(ssd.BusiDate) = " & dDate.Month & " and year(ssd.BusiDate) = " & dDate.Year & sOut & sOwn & sBrand & sSlsType

        Return sStr

    End Function


    Private Function Query_GoFoodSalesExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""

        Dim sSlsType As String = ""

        If My.Settings.gofood = "" Then
            sSlsType = ""
        Else
            sSlsType = " and ssd.SlsTypeNm in (" & My.Settings.gofood & ")"
        End If


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and ssd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If

        sStr = "SELECT ItemNo as Code, ItemName as 'Item Name', Qty as Qty, total as 'Net Sales', o.Kode2 as Outlet, DATE_FORMAT(ssd.BusiDate, '%c/%d/%Y') AS Periode " &
               "FROM cld_sumsalesdetail ssd INNER JOIN cld_outlet o " &
               "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
               "Where ssd.custid = " & CustID & " and Month(ssd.BusiDate) = " & Month(dDate) & " and year(ssd.BusiDate) = " & Year(dDate) & sOut & sOwn & sBrand & sSlsType



        Return sStr

    End Function

    Public Function SelectDataset_Gofood(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_GoFoodSales(CustID, sOutlet, dfrom, iOwn, iBrand)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Gofood")
            Err.Clear()
        End Try


        Return ds


    End Function

    Public Function SelectDataTable_GoFood(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_GoFoodSalesExcel(CustID, sOutlet, dDate, iOwn, iBrand)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function



    'Tambahan 18 jan 2018
    '26Apr2019
    Private Function Query_GoFoodSales_Daily(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dFrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""

        Dim sSlsType As String = ""

        If My.Settings.gofood = "" Then
            sSlsType = ""
        Else
            sSlsType = " and ssd.SlsTypeNm in (" & My.Settings.gofood & ")"
        End If

        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and ssd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        sStr = "SELECT ItemNo as l1, ItemName as l2, Qty as l3, total as l4, o.Kode2 as l5, ssd.BusiDate AS l6, '' as l7, '' as l8, '' as l9, '' as l10, '' as l11, '' l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
               "FROM cld_sumsalesdetaildaily ssd INNER JOIN cld_outlet o " &
               "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
               "Where ssd.custid = " & CustID & " and BusiDate >= '" & dFrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sSlsType &
               "Order by ItemNo, ItemName, o.Kode2, ssd.BusiDate "
        'sStr = "SELECT ItemNo as l1, ItemName as l2, Qty as l3, total as l4, o.Kode2 as l5, ssd.BusiDate AS l6, '' as l7, '' as l8, '' as l9, '' as l10, '' as l11, '' as l12 " &
        '       "FROM cld_sumsalesdetaildaily ssd INNER JOIN cld_outlet o " &
        '       "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
        '       "Where ssd.custid = " & CustID & " and BusiDate >= '" & dFrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sSlsType &
        '       "Order by ItemNo, ItemName, o.Kode2, ssd.BusiDate "

        Return sStr

    End Function


    Private Function Query_GoFoodSalesExcel_Daily(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dto As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""

        Dim sSlsType As String = ""

        If My.Settings.gofood = "" Then
            sSlsType = ""
        Else
            sSlsType = " and ssd.SlsTypeNm in (" & My.Settings.gofood & ")"
        End If


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and ssd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If

        sStr = "SELECT ItemNo as Code, ItemName as 'Item Name', Qty as Qty, total as 'Net Sales', o.Kode2 as Outlet, ssd.BusiDate AS Periode " &
               "FROM cld_sumsalesdetaildaily ssd INNER JOIN cld_outlet o " &
               "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
               "Where ssd.custid = " & CustID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dto.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOwn & sBrand & sSlsType &
               "Order by ItemNo, ItemName, o.Kode2, ssd.BusiDate "

        Return sStr

    End Function

    Public Function SelectDataset_Gofood_Daily(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_GoFoodSales_Daily(CustID, sOutlet, dfrom, dTO, iOwn, iBrand)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Gofood_Daily")
            Err.Clear()
        End Try


        Return ds


    End Function

    Public Function SelectDataTable_GoFood_Daily(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_GoFoodSalesExcel_Daily(CustID, sOutlet, dDate, dTO, iOwn, iBrand)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function



    Private Function Query_ReceiptDetail(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dto As Date, ByVal iOwn As Integer, ByVal iBrand As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""

        Dim sSlsType As String = ""

        If My.Settings.gofood = "" Then
            sSlsType = ""
        Else
            sSlsType = " and ssd.SlsTypeNm in (" & My.Settings.gofood & ")"
        End If


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and ssd.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If

        sStr = "SELECT ItemNo as Code, ItemName as 'Item Name', Qty as Qty, total as 'Net Sales', o.Kode2 as Outlet, ssd.BusiDate AS Periode " &
               "FROM cld_sumsalesdetaildaily ssd INNER JOIN cld_outlet o " &
               "ON ssd.olId = o.ID AND ssd.CustID = o.custID " &
               "Where ssd.custid = " & CustID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dto.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOwn & sBrand & sSlsType &
               "Order by ItemNo, ItemName, o.Kode2, ssd.BusiDate "

        Return sStr

    End Function

    Public Function SelectDataTable_ReceiptDetail(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_ReceiptDetail(CustID, sOutlet, dDate, dTO, iOwn, iBrand)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    'Public Function SelectDataset_dailyreport(ByVal CustID As Integer, ByVal busidate As Date) As DataSet
    '        Dim ds As New DataSet

    '        Try


    '        Dim a As New clsDbo("SELECT brand.`Name2` AS brand, outlet.`Nama` AS cabang, format(ssd.`Subtotal` - ssd.`Discount`,'###,###') AS netto, format(ssd.`Tax`,'###,###') as Tax, format(ssd.`Total`,'###,###') AS bruto " &
    '                            "FROM `cld_brand` brand LEFT JOIN `cld_outlet` outlet ON outlet.`brandID` = brand.`ID` AND outlet.`custID`=brand.`CustID` " &
    '                            "LEFT JOIN `cld_sumsalesdaily` ssd ON ssd.`olId` = outlet.`ID` AND ssd.`olNumber` = outlet.`Kode` AND ssd.`CustID` = outlet.`custID` " &
    '                            "WHERE ssd.`BusiDate` = '" & busidate.ToString("yyyy-MM-dd 00:00:00") & "' " &
    '                            "AND ssd.`CustID` = " & CustID & " AND outlet.`custID` = " & CustID & " AND brand.`CustID` = " & CustID & " AND outlet.`ownID` = 1 ORDER BY brand.`Name2`")

    '        ds = a.selectDataset

    '            a.dispose()
    '            a = Nothing

    '        Catch ex As Exception
    '            errLog(Err.Description, "clsemail_dailyreport:SelectDataset_dailyreport")
    '            Err.Clear()
    '        End Try

    '        Return ds

    '    End Function

    '    Public Function SelectDataset_sumdailyreport(ByVal CustID As Integer, ByVal busidate As Date) As DataSet
    '        Dim ds As New DataSet

    '        Try


    '        Dim a As New clsDbo("SELECT brand.`Name2` AS CABANG, format(SUM(ssd.`Subtotal` - ssd.`Discount`),'###,###') AS NETTO, format(SUM(ssd.`Tax`),'###,###') AS TAX, format(SUM(ssd.`Total`),'###,###') AS BRUTO " &
    '                            "FROM `cld_brand` brand LEFT JOIN `cld_outlet` outlet ON outlet.`brandID` = brand.`ID` AND outlet.`custID`=brand.`CustID` " &
    '                            "LEFT JOIN `cld_sumsalesdaily` ssd ON ssd.`olId` = outlet.`ID` AND ssd.`olNumber` = outlet.`Kode` AND ssd.`CustID` = outlet.`custID` " &
    '                            "WHERE ssd.`BusiDate`= '" & busidate.ToString("yyyy-MM-dd 00:00:00") & "' " &
    '                            "AND ssd.`CustID` = " & CustID & " AND outlet.`custID` = " & CustID & " AND brand.`CustID` = " & CustID & " AND outlet.`ownID` = 1 GROUP BY brand.`Name2`")

    '        ds = a.selectDataset

    '            a.dispose()
    '            a = Nothing

    '        Catch ex As Exception
    '            errLog(Err.Description, "clsemail_dailyreport:SelectDataset_sumdailyreport")
    '            Err.Clear()
    '        End Try

    '        Return ds

    '    End Function


    'Automatic Email report
    'perubahan 25 jan 2018
    'parameternya custid, date dan own

    'perubahan 16 maret 2018
    'melakukan pengecekan dengan log eod

    Public Function SelectDataset_dailyreport(ByVal CustID As Integer, ByVal busidate As Date, ByVal ownid As Integer) As DataSet
        Dim ds As New DataSet

        Try

            'Dim a As New clsDbo("SELECT brand.`Name2` AS brand, outlet.`Nama` AS cabang, format(sum(ssd.`Subtotal` - ssd.`Discount`),'###,###') AS netto, format(ssd.`Tax`,'###,###') as Tax, format(ssd.`Total`,'###,###') AS bruto " &
            '                    "FROM `cld_brand` brand LEFT JOIN `cld_outlet` outlet ON outlet.`brandID` = brand.`ID` AND outlet.`custID`=brand.`CustID` " &
            '                    "LEFT JOIN `cld_sumsalesdaily` ssd ON ssd.`olId` = outlet.`ID` AND ssd.`olNumber` = outlet.`Kode` AND ssd.`CustID` = outlet.`custID` " &
            '                    "WHERE ssd.`BusiDate` = '" & busidate.ToString("yyyy-MM-dd 00:00:00") & "' " &
            '                    "AND ssd.`CustID` = " & CustID & " AND outlet.`ownID` = " & ownid & " GROUP BY outlet.`Nama` ORDER BY brand.`Name2`")

            Dim a As New clsDbo("SELECT brand.`Name2` AS brand, outlet.`Nama` AS cabang, IFNULL(FORMAT(SUM(ssd.`Subtotal` - ssd.`Discount`),'###,###'),0) AS netto " &
                                ", IFNULL(FORMAT(ssd.`Tax`,'###,###'),0) AS Tax, IFNULL(FORMAT(ssd.`Total`,'###,###'),0) AS bruto, CASE WHEN eod.eoddate IS NULL THEN '' ELSE 'eod' END AS eod " &
                                "FROM `cld_outlet` outlet LEFT JOIN `cld_brand` brand ON outlet.`brandID` = brand.`ID` AND outlet.`custID`=brand.`CustID` " &
                                "LEFT JOIN (SELECT * FROM cld_sumsalesdaily WHERE BusiDate = '" & busidate.ToString("yyyy-MM-dd 00:00:00") & "') ssd ON ssd.`olId` = outlet.`ID` AND ssd.`olNumber` = outlet.`Kode` AND ssd.`CustID` = outlet.`custID` " &
                                "LEFT JOIN `cld_eod` eod ON eod.`storeId` = outlet.`ID` AND eod.storeno = outlet.`Kode` AND eod.`CustID` = outlet.`custID` AND eod.eoddate = ssd.busidate " &
                                "WHERE outlet.`CustID` = " & CustID & " AND outlet.`active` = 1 AND outlet.`ownID` = " & ownid & " " &
                                "GROUP BY outlet.id ORDER BY brand.`Name2`,outlet.`Nama`")


            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_dailyreport")
            Err.Clear()
        End Try

        Return ds

    End Function

    'perubahan 25 jan 2018
    'parameternya custid, date dan own
    'perubahan 16 maret 2018
    'melakukan pengecekan dengan log eod
    Public Function SelectDataset_sumdailyreport(ByVal CustID As Integer, ByVal busidate As Date, ByVal ownid As Integer) As DataSet
        Dim ds As New DataSet

        Try
            'Dim a As New clsDbo("SELECT brand.`Name2` AS CABANG, format((SUM(ssd.`Subtotal`) - SUM(ssd.`Discount`)),'###,###') AS NETTO, format(SUM(ssd.`Tax`),'###,###') AS TAX, format(SUM(ssd.`Total`),'###,###') AS BRUTO " &
            '                    "FROM `cld_brand` brand LEFT JOIN `cld_outlet` outlet ON outlet.`brandID` = brand.`ID` AND outlet.`custID`=brand.`CustID` " &
            '                    "LEFT JOIN `cld_sumsalesdaily` ssd ON ssd.`olId` = outlet.`ID` AND ssd.`olNumber` = outlet.`Kode` AND ssd.`CustID` = outlet.`custID` " &
            '                    "WHERE ssd.`BusiDate`= '" & busidate.ToString("yyyy-MM-dd 00:00:00") & "' " &
            '                    "AND ssd.`CustID` = " & CustID & " AND outlet.`ownID` = " & ownid & " GROUP BY brand.`Name2`")

            Dim a As New clsDbo("SELECT brand.`Name2` AS CABANG, FORMAT((SUM(ssd.`Subtotal`) - SUM(ssd.`Discount`)),'###,###') AS NETTO " &
                                ", FORMAT(SUM(ssd.`Tax`),'###,###') AS TAX, FORMAT(SUM(ssd.`Total`),'###,###') AS BRUTO, CONCAT(cast(SUM(CASE WHEN eod.eoddate IS NULL THEN 0 ELSE 1 END) as char(20)),'/',cast(SUM(CASE WHEN eod.eoddate IS NULL THEN 1 ELSE 0 END) as char(20)))AS eod " &
                                "FROM `cld_brand` brand " &
                                "LEFT JOIN `cld_outlet` outlet ON outlet.`brandID` = brand.`ID` AND outlet.`custID`=brand.`CustID` " &
                                "LEFT JOIN (SELECT * FROM cld_sumsalesdaily WHERE BusiDate = '" & busidate.ToString("yyyy-MM-dd 00:00:00") & "') ssd ON ssd.`olId` = outlet.`ID` AND ssd.`olNumber` = outlet.`Kode` AND ssd.`CustID` = outlet.`custID` " &
                                "LEFT JOIN `cld_eod` eod ON eod.`storeId` = outlet.`ID` AND eod.storeno = outlet.`Kode` AND eod.`CustID` = outlet.`custID` AND eod.eoddate = ssd.busidate " &
                                "WHERE outlet.`CustID` = " & CustID & " AND outlet.`active` = 1 AND outlet.`ownID` = " & ownid & " " &
                                "GROUP BY brand.`Name2`")



            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_sumdailyreport")
            Err.Clear()
        End Try

        Return ds

    End Function



    'Bram04Sep2018
    Public Function SelectDataset_SlsItem(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_SlsItemReport(CustID, sOutlet, dfrom, dTo, iOwn, iBrand, iCity)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_SlsItem")
            Err.Clear()
        End Try


        Return ds


    End Function

    'Bram04Sep2018
    Public Function SelectDataset_MonChannel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_MonChannelReport(CustID, sOutlet, dfrom, dTo, iOwn, iBrand, iCity)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_MonChannel")
            Err.Clear()
        End Try


        Return ds


    End Function

    'Bram04Sep2018
    Public Function SelectDataset_SlsCover(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_SlsCoverReport(CustID, sOutlet, dfrom, dTo, iOwn, iBrand, iCity)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_SlsCover")
            Err.Clear()
        End Try


        Return ds


    End Function
    'Bram04Sep2018
    Public Function SelectDataTable_SlsItem(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_SlsItemReportExcel(CustID, sOutlet, dDate, dTO, iOwn, iBrand, iCity)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    'Bram04Sep2018
    Public Function SelectDataTable_MonChannel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_MonChannelReportExcel(CustID, sOutlet, dDate, dTO, iOwn, iBrand, iCity)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    'Bram04Sep2018
    Public Function SelectDataTable_slsCover(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_SlsCoverReportExcel(CustID, sOutlet, dDate, dTO, iOwn, iBrand, iCity)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    'Bram04Sep2018
    'SalesItemReport
    Private Function Query_SlsItemReportExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and s.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        sStr = "SELECT Item,Qty, ROUND(total,0) AS NetSales,ROUND((total * 100 / t2.sum),2) AS Contribution, Date, SlsTypeNm AS Type, Month, Quarter, Year, NAME AS Brand " &
                "FROM " &
                "(SELECT s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%y') DATE,s.`SlsTypeNm`,MONTH(s.busidate) MONTH, " &
                "CONCAT('Q',QUARTER(s.`BusiDate`)) QUARTER, YEAR(s.`BusiDate`) YEAR,b.`Name` " &
                 "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
                "CROSS JOIN " &
                "(SELECT SUM(s.total) AS SUM " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                 "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2 "

        Return sStr

    End Function
    'Bram04Sep2018
    'MonthlyChannelReport
    Private Function Query_MonChannelReportExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and s.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        sStr = "SELECT s.`olName` AS Outlet,DATE_FORMAT(s.busidate,'%d-%b-%y') Date,ROUND(SUM(s.`Total`),0) AS NetSales, s.SlsTypeNm as SalesType ,DATE_FORMAT(s.busidate,'%M') AS Month, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, b.name as Brand, " &
            "ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END),0) AS GofoodSales, " &
            "ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / SUM(s.`Total`),2) AS Gofood " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`,s.`SlsTypeNm` "


        Return sStr

    End Function
    'Bram04Sep2018
    'SalesCoverReport

    Private Function Query_SlsCoverReportExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and s.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        'sStr = "SELECT s.`olName` AS Outlet,DATE_FORMAT(s.busidate,'%d-%b-%y') Date,SUM(s.`Subtotal`-s.`Discount`) AS NetSales, s.SlsTypeNm as SalesType ,DATE_FORMAT(s.busidate,'%M') AS Month, " &
        '    "CONCAT('Q',QUARTER(s.`BusiDate`)) AS Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, " &
        '    "b.`Name` AS Brand, SUM(s.cover) AS Cover, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS Average , SUM(s.QtyTrans) AS Bill " &
        '     "FROM `cld_sumsalesdaily` s " &
        '    "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
        '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
        '    "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
        '    "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` "


        sStr = "SELECT s.`olName` AS Outlet, DATE_FORMAT(s.busidate,'%d-%b-%y') Date,SUM(s.`Subtotal`-s.`Discount`) AS NetSales, s.SlsTypeNm as SalesType ,DATE_FORMAT(s.busidate,'%M') AS Month, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) AS Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, " &
            "b.`Name` AS Brand, SUM(s.cover) AS Cover, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS Average , SUM(s.QtyTrans) AS Bill, IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0) TotalItem " &
             "FROM `cld_sumsalesdaily` s " &
            "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` " &
            "Union all " &
            "SELECT 'Total' AS Outlet, '' Date, SUM(s.`Subtotal`-s.`Discount`) AS NetSales, '' AS SalesType, '' AS Month, " &
            "'' AS Quarter, '' AS Year, " &
            "'' AS Brand, SUM(s.cover) AS Cover, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS Average, SUM(s.QtyTrans) AS Bill, SUM(IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0)) TotalItem " &
            "FROM `cld_sumsalesdaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity

        Return sStr

    End Function

    'Bram04Sep2018
    'SalesItemReport
    Private Function Query_SlsItemReport(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and s.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        sStr = "SELECT item AS l1,qty AS l2, ROUND(total,0) AS l3,ROUND((total * 100 / t2.sum),2) AS l4, DATE AS l5, SlsTypeNm AS l6, MONTH AS l7, QUARTER AS l8, YEAR AS l9, NAME AS l10, '' l11, '' as l12 , '' l13, '' l14, '' l15, '' l16, '' l17 " &
                "FROM " &
                "(SELECT s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%y') DATE,s.`SlsTypeNm`,MONTH(s.busidate) MONTH, " &
                "CONCAT('Q',QUARTER(s.`BusiDate`)) QUARTER, YEAR(s.`BusiDate`) YEAR,b.`Name` " &
                 "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
                "CROSS JOIN " &
                "(SELECT SUM(s.total) AS SUM " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                 "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2 "

        Return sStr

    End Function
    'Bram04Sep2018
    'MonthlyChannelReport
    Private Function Query_MonChannelReport(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and s.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        'perubahan 15 sept 2018, tambahin SlsTypeNm

        'sStr = "SELECT s.`olName` AS l1,DATE_FORMAT(s.busidate,'%d-%b-%y') l2,ROUND(SUM(s.`Total`),0) AS l3,DATE_FORMAT(s.busidate,'%M') AS l4, " &
        '    "QUARTER(s.`BusiDate`) AS l5, DATE_FORMAT(s.busidate,'%Y') AS l6, b.name as l7, " &
        '    "ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN('GOJEK', 'GOFOOD')THEN s.total ELSE 0 END),0) AS l8, " &
        '    "ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN('GOJEK', 'GOFOOD')THEN s.total ELSE 0 END) * 100 / SUM(s.`Total`),2) AS l9, '' l10, '' l11 " &
        '     "FROM `cld_sumsalesdetaildaily` s " &
        '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
        '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
        '    "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
        '    "GROUP BY s.`olId`,s.`BusiDate` "


        sStr = "SELECT s.`olName` AS l1,DATE_FORMAT(s.busidate,'%d-%b-%y') l2,ROUND(SUM(s.`Total`),0) AS l3 , s.SlsTypeNm AS l4 ,DATE_FORMAT(s.busidate,'%M') AS l5, " &
            "QUARTER(s.`BusiDate`) AS l6, DATE_FORMAT(s.busidate,'%Y') AS l7, b.name as l8, " &
            "ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END),0) AS l9, " &
            "ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / SUM(s.`Total`),2) AS l10, '' l11, '' l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`, s.SlsTypeNm "



        Return sStr

    End Function

    Private Function Query_SlsCoverReport(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String


        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and s.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        'sStr = "SELECT s.`olName` AS l1,DATE_FORMAT(s.busidate,'%d-%b-%y') l2, SUM(s.`Subtotal`-s.`Discount`) AS l3, s.`SlsTypeNm` AS l4, DATE_FORMAT(s.busidate,'%M') AS l5, " &
        '    "CONCAT('Q',QUARTER(s.`BusiDate`)) AS l6, DATE_FORMAT(s.busidate,'%Y') AS l7, " &
        '    "b.`Name` AS l8, SUM(s.cover) AS l9, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS l10, SUM(s.QtyTrans) AS l11, '' l12 " &
        '     "FROM `cld_sumsalesdaily` s " &
        '    "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
        '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
        '    "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
        '    "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` "



        sStr = "SELECT s.`olName` AS l1,DATE_FORMAT(s.busidate,'%d-%b-%y') l2, SUM(s.`Subtotal`-s.`Discount`) AS l3, s.`SlsTypeNm` AS l4, DATE_FORMAT(s.busidate,'%M') AS l5, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) AS l6, DATE_FORMAT(s.busidate,'%Y') AS l7, " &
            "b.`Name` AS l8, SUM(s.cover) AS l9, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS l10, SUM(s.QtyTrans) AS l11, IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0) l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
             "FROM `cld_sumsalesdaily` s " &
            "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` " &
            "Union all " &
            "SELECT 'Total' AS l1, '' l2, SUM(s.`Subtotal`-s.`Discount`) AS l3, '' AS l4, '' AS l5, " &
            "'' AS l6, '' AS l7, " &
            "'' AS l8, SUM(s.cover) AS l9, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS l10, SUM(s.QtyTrans) AS l11, SUM(IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0)) l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
            "FROM `cld_sumsalesdaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity

        Return sStr

    End Function
    Private Function Query_SalesVSPayment(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String


        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and cld_sumsalesdaily.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If





        sStr = "SELECT cld_sumsalesdaily.busidate AS l1, cld_sumsalesdaily.`olnumber` AS l2,cld_sumsalesdaily.olname AS l3,SUM(cld_sumsalesdaily.`Subtotal`) AS l4,SUM(cld_sumsalesdaily.total) AS l5 " &
        ",(SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`) AS l6, '' l7, '' l8, '' l9, '' l10, '' l11, '' l12, '' l13, '' l14, '' l15, '' l16, '' l17 " &
        " FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut &
        " GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate"

        Return sStr

    End Function

    Private Function Query_SalesVSPaymentExcel(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As String

        Dim sOut As String = ""
        Dim sStr As String = ""

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""


        If sOutlet = "0" Then
            sOut = ""
        Else
            sOut = " and cld_sumsalesdaily.olnumber in (" & sOutlet & ")"
        End If

        If iBrand = "0" Then
            sBrand = ""
        Else
            sBrand = " and o.brandID in (" & iBrand & ")"
        End If

        If iOwn = "0" Then
            sOwn = ""
        Else
            sOwn = " and o.ownID in (" & iOwn & ")"
        End If
        If iCity = "0" Then
            sCity = ""
        Else
            sCity = " and o.cityID in (" & iCity & ")"
        End If

        'sStr = "SELECT s.`olName` AS Outlet,DATE_FORMAT(s.busidate,'%d-%b-%y') Date,SUM(s.`Subtotal`-s.`Discount`) AS NetSales, s.SlsTypeNm as SalesType ,DATE_FORMAT(s.busidate,'%M') AS Month, " &
        '    "CONCAT('Q',QUARTER(s.`BusiDate`)) AS Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, " &
        '    "b.`Name` AS Brand, SUM(s.cover) AS Cover, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS Average , SUM(s.QtyTrans) AS Bill " &
        '     "FROM `cld_sumsalesdaily` s " &
        '    "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
        '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
        '    "Where s.custid = " & CustID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
        '    "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` "


        sStr = "SELECT cld_sumsalesdaily.busidate, cld_sumsalesdaily.`olnumber`,cld_sumsalesdaily.olname,SUM(cld_sumsalesdaily.`Subtotal`) subtotal,SUM(cld_sumsalesdaily.total)totalsm " &
        ",(SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`) paymentdaily " &
        " FROM(cld_sumsalesdaily) WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut &
        " GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate"

        Return sStr

    End Function

    Public Function SelectDataset_SalesVSPayment(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dfrom As Date, ByVal dTo As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataSet

        Dim ds As New DataSet

        Dim sStr As String

        Try

            sStr = Query_SalesVSPayment(CustID, sOutlet, dfrom, dTo, iOwn, iBrand, iCity)

            Dim c_Cmd As New clsDbo(ServerString)
            ds = c_Cmd.ReturnDataset(sStr)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_SlsCover")
            Err.Clear()
        End Try


        Return ds


    End Function
    Public Function SelectDataTable_SalesVSPayment(ByVal CustID As Integer, ByVal sOutlet As String, ByVal dDate As Date, ByVal dTO As Date, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String) As DataTable

        Dim dt As New DataTable
        Dim strFilter As String = Query_SalesVSPaymentExcel(CustID, sOutlet, dDate, dTO, iOwn, iBrand, iCity)

        Dim a As New clsDbo(strFilter)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    Public Function SelectDataset_Supplier(ByVal CustID As Integer, ByVal sText As String) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            strfilter = "Select * from cld_inv_supplier where custid = " & CustID &
            " AND ( cld_inv_supplier.Name like  '%" & sText & "%' Or cld_inv_supplier.Email like '%" & sText & "%' Or cld_inv_supplier.Address like '%" & sText & "%' ) order by id"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Supplier")
            Err.Clear()
        End Try


        Return ds


    End Function

End Class




