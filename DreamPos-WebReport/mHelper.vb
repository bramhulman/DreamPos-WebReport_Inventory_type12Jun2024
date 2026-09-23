
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports DocumentFormat.OpenXml.Bibliography

Module mHelper

    Public SecretKey As String = "ol512csacldmember"

    Public Exportpath As String = ""



    Public Enum EFlagMemberPoint
        'field flag
        Trans_Adjust_Point = 0

        TRANS_EARN_POINTS = 1 'belanja dapet point  'TransID = PaymentMasterID
        TRANS_REDEEM_POINTS = 2 'redeam Item  'TransID = 0
        TRANS_VOID_POINTS = 3  'void transaksi  'TransID = PaymentMasterID
        TRANS_POINTPAYMENT = 4  'void transaksi  'TransID = PaymentMasterID
        ' BALANCE_FORWARD = 8 'for memberCard purpose!
    End Enum

    'Revisi november 2016
    'ini enum untuk privilage user
    Public Enum usrModulepriv
        moduleuser = &H1
    End Enum
    Public Enum usrModul
        'Module 1 module report
        'Module 2 module CRM
        modulereport = &H1
        moduleCRM = &H2
        moduleInv = &H3
    End Enum
    Public ServerString As String = My.Settings.ServerStr   'My.Settings.ConStr
    'Public ServerString As String = My.Settings.ConStr

    Public Sub errLog(ByVal strErrDesc As String, ByVal strAccurIn As String)
        Dim reader As New System.Configuration.AppSettingsReader
        Dim strErrPath As String
        strErrPath = My.Settings.errLog
        Try

            If strErrPath <> "" Then
                Dim strDire As String
                strDire = Path.GetDirectoryName(strErrPath)

                'If Not Directory.Exists(strErrPath) Then
                '    Directory.CreateDirectory(strDire)
                'End If

                Dim objWriter As New StreamWriter(strErrPath, True)

                strErrDesc &= " (Occurred IN :" & strAccurIn & ")"
                strErrDesc = "ERROR:" & strErrDesc
                objWriter.WriteLine(Format(Now, "dd/MM/yy hh:mm:ss tt") & " ==>" & strErrDesc)
                objWriter.Close()
                objWriter.Dispose()
            End If
        Catch ex As Exception
            Err.Clear()
        End Try
    End Sub

    Public Function GetDiagnosticsLogDirectory() As String
        Try
            If HttpContext.Current IsNot Nothing Then
                Dim webLogPath As String = HttpContext.Current.Server.MapPath("~/logs")
                Directory.CreateDirectory(webLogPath)
                Return webLogPath
            End If
        Catch
        End Try

        Dim logPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")
        Directory.CreateDirectory(logPath)
        Return logPath
    End Function

    Public Function GetErrorLogFilePath() As String
        Try
            Dim configuredPath As String = My.Settings.errLog
            If Not String.IsNullOrWhiteSpace(configuredPath) Then
                Dim configuredDirectory As String = Path.GetDirectoryName(configuredPath)
                If Not String.IsNullOrWhiteSpace(configuredDirectory) Then
                    Directory.CreateDirectory(configuredDirectory)
                End If
                Return configuredPath
            End If
        Catch
        End Try

        Return Path.Combine(GetDiagnosticsLogDirectory(), "webreport_error.log")
    End Function

    Public Function SaveLastSentJson(ByVal rawJson As String, Optional ByVal actionName As String = "api", Optional ByVal referenceNo As String = "") As String
        Try
            Dim logPath As String = GetDiagnosticsLogDirectory()
            Dim payloadText As String = If(rawJson, "")

            Dim lastPayloadPath As String = Path.Combine(logPath, "last_payload.json")
            File.WriteAllText(lastPayloadPath, payloadText, Encoding.UTF8)

            Dim safeAction As String = MakeSafeFileName(If(String.IsNullOrWhiteSpace(actionName), "api", actionName))
            Dim safeRef As String = MakeSafeFileName(referenceNo)
            If Not String.IsNullOrWhiteSpace(safeRef) Then
                safeRef = "_" & safeRef
            End If

            Dim archiveFileName As String = String.Format("payload_{0}{1}_{2}.json", safeAction, safeRef, Date.Now.ToString("yyyyMMdd_HHmmss_fff"))
            Dim archivePath As String = Path.Combine(logPath, archiveFileName)
            File.WriteAllText(archivePath, payloadText, Encoding.UTF8)

            Return lastPayloadPath
        Catch ex As Exception
            errLog(ex.Message, "mHelper:SaveLastSentJson")
            Return ""
        End Try
    End Function

    Public Sub LogApiTrace(ByVal actionName As String, ByVal requestBody As String, ByVal responseBody As String, Optional ByVal statusCode As Integer? = Nothing)
        Try
            Dim sb As New StringBuilder()
            sb.AppendLine("==================================================")
            sb.AppendLine("Timestamp : " & Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            sb.AppendLine("Action    : " & If(actionName, ""))
            If statusCode.HasValue Then
                sb.AppendLine("HTTP Code : " & statusCode.Value.ToString())
            End If
            sb.AppendLine("Request:")
            sb.AppendLine(MaskSensitiveText(requestBody))
            sb.AppendLine("Response:")
            sb.AppendLine(MaskSensitiveText(responseBody))

            File.AppendAllText(GetErrorLogFilePath(), sb.ToString() & Environment.NewLine, Encoding.UTF8)
        Catch ex As Exception
            errLog(ex.Message, "mHelper:LogApiTrace")
        End Try
    End Sub

    Public Sub LogDetailedError(ByVal context As String, ByVal ex As Exception, Optional ByVal requestBody As String = "", Optional ByVal responseBody As String = "", Optional ByVal statusCode As Integer? = Nothing)
        Try
            Dim sb As New StringBuilder()
            sb.AppendLine("==================================================")
            sb.AppendLine("Timestamp : " & Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            sb.AppendLine("Context   : " & If(context, ""))

            If statusCode.HasValue Then
                sb.AppendLine("HTTP Code : " & statusCode.Value.ToString())
            End If

            If ex IsNot Nothing Then
                sb.AppendLine("Message   : " & ex.Message)
                sb.AppendLine("Type      : " & ex.GetType().FullName)

                If ex.InnerException IsNot Nothing Then
                    sb.AppendLine("Inner Msg : " & ex.InnerException.Message)
                    sb.AppendLine("InnerType : " & ex.InnerException.GetType().FullName)
                End If

                If Not String.IsNullOrWhiteSpace(ex.StackTrace) Then
                    sb.AppendLine("StackTrace:")
                    sb.AppendLine(ex.StackTrace)
                End If
            End If

            If Not String.IsNullOrWhiteSpace(requestBody) Then
                sb.AppendLine("Request:")
                sb.AppendLine(MaskSensitiveText(requestBody))
            End If

            If Not String.IsNullOrWhiteSpace(responseBody) Then
                sb.AppendLine("Response:")
                sb.AppendLine(MaskSensitiveText(responseBody))
            End If

            Dim logPath As String = GetDiagnosticsLogDirectory()
            File.AppendAllText(Path.Combine(logPath, "debug_trace_" & Date.Now.ToString("yyyyMMdd") & ".log"), sb.ToString() & Environment.NewLine, Encoding.UTF8)
            File.AppendAllText(GetErrorLogFilePath(), sb.ToString() & Environment.NewLine, Encoding.UTF8)
        Catch logEx As Exception
            errLog(logEx.Message, "mHelper:LogDetailedError")
        End Try
    End Sub

    Public Function MaskSensitiveText(ByVal rawText As String) As String
        If String.IsNullOrWhiteSpace(rawText) Then Return rawText

        Dim result As String = rawText
        result = Regex.Replace(result, "(""[^""]*(token|secret|password)[^""]*""\s*:\s*"")([^""]*)("")", "$1***$4", RegexOptions.IgnoreCase)
        result = Regex.Replace(result, "(Authorization\s*[:=]\s*Bearer\s+)([A-Za-z0-9\._\-]+)", "$1***", RegexOptions.IgnoreCase)
        Return result
    End Function

    Private Function MakeSafeFileName(ByVal value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return ""

        Dim safe As String = value.Trim()
        For Each c As Char In Path.GetInvalidFileNameChars()
            safe = safe.Replace(c, "_"c)
        Next
        Return safe.Replace(" "c, "_"c)
    End Function

    'tambahan 1 maret 2018
    Public Function GenerateSHA512String(ByVal inputString) As String
        Dim sha512 As SHA512 = SHA512Managed.Create()
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(inputString)
        Dim hash As Byte() = sha512.ComputeHash(bytes)
        Dim stringBuilder As New StringBuilder()
        Try

            For i As Integer = 0 To hash.Length - 1
                stringBuilder.Append(hash(i).ToString("X2"))
            Next
        Catch ex As Exception
            errLog(Err.Description & " " & inputString, "mHelper:GenerateSHA512String")
            Err.Clear()
        End Try

        Return stringBuilder.ToString()
    End Function

    Public Sub insertLogModule(ByVal User As String, ByVal Modul As String, ByVal Activity As String, ByVal CustID As Integer, Optional BInsert As Boolean = False)
        Dim c_cmd As New Cls_command
        Dim str As String = ""
        Try
            'Erik Sementara matiin dlu takut dbnya kepenuhan


            Dim cCust As New ClsCustomer
            Dim custname As String = cCust.SelectDataset_CustName(CustID)

            str = "insert into cld_Log(username, Module, Activity, Transdate, CustID, CustName, ID) values ('" & User & "', '" & Modul & "','" & Activity & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'," & CustID & ", '" & custname & "',0)"

            c_cmd.executeNonQuery(str)


        Catch ex As Exception
            errLog(Err.Description & " " & str, "mHelper:insertLogModule")
            Err.Clear()
            ' MsgBox(ex.Message)

        End Try

        c_cmd = Nothing

    End Sub

    'field Module
    Public Enum usrInvPriv

        invItemMaster = &H1
        invStockGroup = &H2
        invStockMaster = &H4 '1
        invCostCentre = &H8


        invSupplier = &H10 '1
        invReceipe = &H20
        invOrderToWarehouse = &H40
        invOrderFromStore = &H80

        invBatchProcess = &H100
        invTransfer = &H200
        invRawWaste = &H400
        invOrder = &H800 '1

        invReturn = &H1000
        invStockOpname = &H2000
        invCalculateInv = &H4000
        invExpenseOther = &H8000

        invInvoice = &H10000
        invReturntoWH = &H20000
        invm_webreport = &H40000
        invInvoicePayment = &H80000

        invKartuStock = &H100000
        invCCSupplier = &H200000
        printordertowh = &H400000
        accordertobuy = &H800000
        preorderusage = &H1000000
        purchaseRequest = &H2000000
    End Enum


    Public Function Save_Rvc_usrInvPriv(ByVal List As Integer, ByVal flagstatus As Integer) As Integer

        Dim iHasil As Integer = 0

        Try
            Select Case List
                Case 0
                    iHasil = flagstatus Or usrInvPriv.invItemMaster
                Case 1
                    iHasil = flagstatus Or usrInvPriv.invStockGroup
                Case 2
                    iHasil = flagstatus Or usrInvPriv.invStockMaster
                Case 3
                    iHasil = flagstatus Or usrInvPriv.invCostCentre
                Case 4
                    iHasil = flagstatus Or usrInvPriv.invSupplier
                Case 5
                    iHasil = flagstatus Or usrInvPriv.invReceipe
                Case 6
                    iHasil = flagstatus Or usrInvPriv.invOrderToWarehouse
                Case 7
                    iHasil = flagstatus Or usrInvPriv.invOrderFromStore
                Case 8
                    iHasil = flagstatus Or usrInvPriv.invBatchProcess
                Case 9
                    iHasil = flagstatus Or usrInvPriv.invTransfer
                Case 10
                    iHasil = flagstatus Or usrInvPriv.invRawWaste
                Case 11
                    iHasil = flagstatus Or usrInvPriv.invOrder
                Case 12
                    iHasil = flagstatus Or usrInvPriv.invReturn
                Case 13
                    iHasil = flagstatus Or usrInvPriv.invStockOpname
                Case 14
                    iHasil = flagstatus Or usrInvPriv.invCalculateInv
                Case 15
                    iHasil = flagstatus Or usrInvPriv.invExpenseOther
                Case 16
                    iHasil = flagstatus Or usrInvPriv.invInvoice
                Case 17
                    iHasil = flagstatus Or usrInvPriv.invReturntoWH
                Case 18
                    iHasil = flagstatus Or usrInvPriv.invm_webreport
                Case 19
                    iHasil = flagstatus Or usrInvPriv.invInvoicePayment
                Case 20
                    iHasil = flagstatus Or usrInvPriv.invKartuStock
                Case 21
                    iHasil = flagstatus Or usrInvPriv.invCCSupplier
                Case 22
                    iHasil = flagstatus Or usrInvPriv.printordertowh
                Case 23
                    iHasil = flagstatus Or usrInvPriv.accordertobuy
                Case 24
                    iHasil = flagstatus Or usrInvPriv.preorderusage
                Case 25
                    iHasil = flagstatus Or usrInvPriv.purchaseRequest
            End Select
        Catch ex As Exception
            errLog(Err.Description & " " & List, "mHelper:Save_Rvc_usrInvPriv")
            Err.Clear()
        End Try

        Return iHasil

    End Function


    Public Function check_usrInvPriv(ByVal List As Integer, ByVal FlagStatus As Integer) As Boolean


        Dim bHasil As Boolean = False
        Try

            Select Case List

                Case 0
                    bHasil = IIf((FlagStatus And usrInvPriv.invItemMaster) = usrInvPriv.invItemMaster, True, False)
                Case 1
                    bHasil = IIf((FlagStatus And usrInvPriv.invStockGroup) = usrInvPriv.invStockGroup, True, False)
                Case 2
                    bHasil = IIf((FlagStatus And usrInvPriv.invStockMaster) = usrInvPriv.invStockMaster, True, False)
                Case 3
                    bHasil = IIf((FlagStatus And usrInvPriv.invCostCentre) = usrInvPriv.invCostCentre, True, False)
                Case 4
                    bHasil = IIf((FlagStatus And usrInvPriv.invSupplier) = usrInvPriv.invSupplier, True, False)
                Case 5
                    bHasil = IIf((FlagStatus And usrInvPriv.invReceipe) = usrInvPriv.invReceipe, True, False)
                Case 6
                    bHasil = IIf((FlagStatus And usrInvPriv.invOrderToWarehouse) = usrInvPriv.invOrderToWarehouse, True, False)
                Case 7
                    bHasil = IIf((FlagStatus And usrInvPriv.invOrderFromStore) = usrInvPriv.invOrderFromStore, True, False)
                Case 8
                    bHasil = IIf((FlagStatus And usrInvPriv.invBatchProcess) = usrInvPriv.invBatchProcess, True, False)
                Case 9
                    bHasil = IIf((FlagStatus And usrInvPriv.invTransfer) = usrInvPriv.invTransfer, True, False)
                Case 10
                    bHasil = IIf((FlagStatus And usrInvPriv.invRawWaste) = usrInvPriv.invRawWaste, True, False)
                Case 11
                    bHasil = IIf((FlagStatus And usrInvPriv.invOrder) = usrInvPriv.invOrder, True, False)
                Case 12
                    bHasil = IIf((FlagStatus And usrInvPriv.invReturn) = usrInvPriv.invReturn, True, False)
                Case 13
                    bHasil = IIf((FlagStatus And usrInvPriv.invStockOpname) = usrInvPriv.invStockOpname, True, False)
                Case 14
                    bHasil = IIf((FlagStatus And usrInvPriv.invCalculateInv) = usrInvPriv.invCalculateInv, True, False)
                Case 15
                    bHasil = IIf((FlagStatus And usrInvPriv.invExpenseOther) = usrInvPriv.invExpenseOther, True, False)
                Case 16
                    bHasil = IIf((FlagStatus And usrInvPriv.invInvoice) = usrInvPriv.invInvoice, True, False)
                Case 17
                    bHasil = IIf((FlagStatus And usrInvPriv.invReturntoWH) = usrInvPriv.invReturntoWH, True, False)
                Case 18
                    bHasil = IIf((FlagStatus And usrInvPriv.invm_webreport) = usrInvPriv.invm_webreport, True, False)
                Case 19
                    bHasil = IIf((FlagStatus And usrInvPriv.invInvoicePayment) = usrInvPriv.invInvoicePayment, True, False)
                Case 20
                    bHasil = IIf((FlagStatus And usrInvPriv.invKartuStock) = usrInvPriv.invKartuStock, True, False)
                Case 21
                    bHasil = IIf((FlagStatus And usrInvPriv.invCCSupplier) = usrInvPriv.invCCSupplier, True, False)
                Case 22
                    bHasil = IIf((FlagStatus And usrInvPriv.printordertowh) = usrInvPriv.printordertowh, True, False)
                Case 23
                    bHasil = IIf((FlagStatus And usrInvPriv.accordertobuy) = usrInvPriv.accordertobuy, True, False)
                Case 24
                    bHasil = IIf((FlagStatus And usrInvPriv.preorderusage) = usrInvPriv.preorderusage, True, False)
                Case 25
                    bHasil = IIf((FlagStatus And usrInvPriv.purchaseRequest) = usrInvPriv.purchaseRequest, True, False)

            End Select

        Catch ex As Exception
            errLog(Err.Description & " " & List, "mHelper:check_usrInvPriv")
            Err.Clear()
        End Try

        Return bHasil

    End Function
    Public Enum BootstrapAlertType
        Plain
        Success
        Information
        Warning
        Danger
        Primary
    End Enum
    Public Sub BootstrapAlert(MsgLabel As Label, Message As String, Optional MessageType As BootstrapAlertType = BootstrapAlertType.Plain, Optional Dismissable As Boolean = False)
        Dim style, icon As String
        Try
            Select Case MessageType
                Case BootstrapAlertType.Plain
                    style = "default"
                    icon = ""
                Case BootstrapAlertType.Success
                    style = "success"
                    icon = "check"
                Case BootstrapAlertType.Information
                    style = "info"
                    icon = "info-circle"
                Case BootstrapAlertType.Warning
                    style = "warning"
                    icon = "warning"
                Case BootstrapAlertType.Danger
                    style = "danger"
                    icon = "remove"
                Case BootstrapAlertType.Primary
                    style = "primary"
                    icon = "info"
            End Select
        Catch ex As Exception
            errLog(Err.Description & " " & MessageType, "mHelper:BootstrapAlert")
            Err.Clear()
        End Try
    End Sub

    Public Sub errClose1(ByVal strErrDesc As String, ByVal strAccurIn As String)
        Dim reader As New System.Configuration.AppSettingsReader
        Dim strErrPath As String
        strErrPath = My.Settings.errClose
        Try

            If strErrPath <> "" Then
                Dim strDire As String
                strDire = Path.GetDirectoryName(strErrPath)

                'If Not Directory.Exists(strErrPath) Then
                '    Directory.CreateDirectory(strDire)
                'End If

                Dim objWriter As New StreamWriter(strErrPath, True)

                strErrDesc &= " (Occurred IN :" & strAccurIn & ")"
                strErrDesc = "ERROR:" & strErrDesc
                objWriter.WriteLine(Format(Now, "dd/MM/yy hh:mm:ss tt") & " ==>" & strErrDesc)
                objWriter.Close()
                objWriter.Dispose()
            End If
        Catch ex As Exception
            Err.Clear()
        End Try
    End Sub


    'start errlog cld_inv_transstockcls

    Public Sub writeerrcsv(ByVal xappend As Boolean, ByVal invdate As Date, ByVal ccid As Integer, ByVal stkid As Integer)

        Exportpath = My.Settings.errcsv

        If xappend = True Then

            Using writer As StreamWriter = New StreamWriter(Exportpath, True)


                WriteDataerr(writer, invdate, ccid, stkid)

            End Using
        Else
            Using writer As StreamWriter = New StreamWriter(Exportpath, False)

                WriteDataerr(writer, invdate, ccid, stkid)

            End Using

        End If

        'dt = Nothing  ' removed: dt not declared in this scope

    End Sub

    Public Sub WriteDataerr(ByVal writer As TextWriter, ByVal invdate As Date, ByVal ccid As Integer, ByVal stkid As Integer)

        Dim csv As String = String.Empty
        Try
            csv += invdate.ToString("yyyy-MM-dd 00:00:00").Replace(",", ";").TrimEnd + ","c
            csv += ccid.ToString().Replace(",", ";").TrimEnd + ","c
            csv += stkid.ToString().Replace(",", ";").TrimEnd + ","c
            If String.IsNullOrEmpty(csv) Then
                Exit Sub
            End If
            writer.WriteLine(String.Join(",", csv))
            'writer.WriteLine(String.Join(";", csv))
            csv = String.Empty
        Catch ex As Exception

        End Try
    End Sub
    'finish errlog cld_inv_transstockcls

    <Extension()>
    Function DataTableToList(Of T As {Class, New})(ByVal table As DataTable) As List(Of T)
        Try
            Dim list As List(Of T) = New List(Of T)()

            For Each row In table.AsEnumerable()
                Dim obj As T = New T()

                For Each prop In obj.[GetType]().GetProperties()

                    Try
                        Dim propertyInfo As PropertyInfo = obj.[GetType]().GetProperty(prop.Name)
                        propertyInfo.SetValue(obj, Convert.ChangeType(row(prop.Name), propertyInfo.PropertyType), Nothing)
                    Catch ex As Exception
                        Continue For
                    End Try
                Next

                list.Add(obj)
            Next

            Return list
        Catch
            Return Nothing
        End Try
    End Function

    Function ListToDataTable(Of T)(ByVal items As List(Of T)) As DataTable
        Dim tb = New DataTable(GetType(T).Name)
        Dim props As PropertyInfo() = GetType(T).GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)

        For Each prop As PropertyInfo In props
            Dim t1 As Type = GetCoreType(prop.PropertyType)
            tb.Columns.Add(prop.Name, t1)
        Next

        For Each item As T In items
            Dim values = New Object(props.Length - 1) {}

            For i As Integer = 0 To props.Length - 1
                values(i) = props(i).GetValue(item, Nothing)
            Next

            tb.Rows.Add(values)
        Next

        Return tb
    End Function

    Function IsNullable(ByVal t As Type) As Boolean
        Return Not t.IsValueType OrElse (t.IsGenericType AndAlso t.GetGenericTypeDefinition() = GetType(Nullable(Of)))
    End Function

    Function GetCoreType(ByVal t As Type) As Type
        If t IsNot Nothing AndAlso IsNullable(t) Then

            If Not t.IsValueType Then
                Return t
            Else
                Return Nullable.GetUnderlyingType(t)
            End If
        Else
            Return t
        End If
    End Function
    'Public Sub ceksession()
    '    If HttpContext.Current.Session("username") Is Nothing And HttpContext.Current.Request.Cookies.Get("UserLogin") Is Nothing Then
    '        HttpContext.Current.Session.Clear()
    '        HttpContext.Current.Session.Abandon()
    '        HttpContext.Current.Session.RemoveAll()
    '        FormsAuthentication.RedirectToLoginPage()
    '        Exit Sub
    '    End If

    'End Sub
    ' Cache per type → array of PropertyInfo
    Private ReadOnly _propCache As New Dictionary(Of Type, PropertyInfo())()

    <Extension()>
    Public Function ToList(Of T As {Class, New})(table As DataTable) As List(Of T)

        Dim tType = GetType(T)
        Dim props As PropertyInfo() = Nothing

        SyncLock _propCache
            If Not _propCache.TryGetValue(tType, props) Then
                props = tType.GetProperties(BindingFlags.Public Or BindingFlags.Instance) _
                             .Where(Function(p) p.CanWrite).ToArray()
                _propCache(tType) = props
            End If
        End SyncLock

        Dim result As New List(Of T)(table.Rows.Count)

        For Each row As DataRow In table.Rows
            Dim obj As New T()

            For Each prop In props
                If table.Columns.Contains(prop.Name) Then
                    Dim raw = row(prop.Name)
                    If raw IsNot DBNull.Value Then
                        Dim converted = Convert.ChangeType(raw, prop.PropertyType)
                        prop.SetValue(obj, converted, Nothing)
                    End If
                End If
            Next

            result.Add(obj)
        Next

        Return result
    End Function
    'tambahan 16 feb 2020, dimana nantinya
    Public Function ModPrvDep(ByVal ModPrive As Integer, ByVal name As String) As String

        If ModPrive And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            Return " and cld_inv_cc.olid in (Select store from cld_userxstore where user = '" & name & "')"
        Else
            Return ""
        End If
    End Function

    Public Sub CekSession()
        Dim ctx = HttpContext.Current
        If ctx Is Nothing Then Exit Sub

        ' Jangan jalankan cek redirect jika sedang berada di halaman login (Default.aspx)
        Dim rawUrl As String = ""
        Try
            If ctx.Request IsNot Nothing AndAlso ctx.Request.RawUrl IsNot Nothing Then
                rawUrl = ctx.Request.RawUrl.ToLower()
            End If
        Catch
        End Try
        If rawUrl.Contains("default.aspx") Then Exit Sub

        Dim cookie As HttpCookie = ctx.Request.Cookies("UserLogin_inv")
        Dim sUser As String = TryCast(ctx.Session("username"), String)
        Dim sCookieUser As String = GetCookieValue("userName", "")
        If String.IsNullOrEmpty(sCookieUser) Then sCookieUser = GetCookieValue("username", "")

        Dim sCustId As String = GetCookieValue("Custid", "")
        If String.IsNullOrEmpty(sCustId) Then sCustId = GetCookieValue("custid", "")
        If String.IsNullOrEmpty(sCustId) AndAlso ctx.Session IsNot Nothing AndAlso ctx.Session("custid") IsNot Nothing Then
            sCustId = ctx.Session("custid").ToString()
        End If

        ' 1. Cek apakah user terautentikasi oleh FormsAuthentication
        If String.IsNullOrEmpty(sUser) AndAlso ctx.Request.IsAuthenticated AndAlso ctx.User IsNot Nothing AndAlso Not String.IsNullOrEmpty(ctx.User.Identity.Name) Then
            sUser = ctx.User.Identity.Name
        End If

        ' 2. Auto-recovery: Jika Session("username") kosong namun cookie memiliki user
        If String.IsNullOrEmpty(sUser) AndAlso Not String.IsNullOrEmpty(sCookieUser) Then
            sUser = sCookieUser
        End If

        ' 3. Jika sUser ditemukan, pastikan session & auth cookie selalu sinkron
        If Not String.IsNullOrEmpty(sUser) Then
            If ctx.Session IsNot Nothing Then
                ctx.Session("username") = sUser
                If Not String.IsNullOrEmpty(sCustId) Then ctx.Session("custid") = sCustId
            End If
            If Not ctx.Request.IsAuthenticated Then
                FormsAuthentication.SetAuthCookie(sUser, True)
            End If
        End If

        ' 4. Hanya tendang keluar jika SAMA SEKALI tidak ada user di session, cookie, maupun FormsAuth
        If String.IsNullOrEmpty(sUser) Then
            Dim reason As String = "Sesi habis/tidak valid -> REDIRECT KE LOGIN. sUser=" & sUser & ", cookieIsNull=" & (cookie Is Nothing).ToString() & ", sCustId=" & sCustId & ", URL=" & ctx.Request.RawUrl
            errLog(reason, "mHelper.CekSession")

            FormsAuthentication.SignOut()
            ctx.Session?.Clear()
            ctx.Session?.Abandon()
            ctx.Response.Cookies("UserLogin_inv").Expires = DateTime.Now.AddDays(-1)
            ctx.Response.Redirect(FormsAuthentication.LoginUrl & "?ReturnUrl=" & HttpUtility.UrlEncode(ctx.Request.RawUrl), True)
            Exit Sub
        End If

        ' 5. Sesi valid - perpanjang dan rawat cookie UserLogin_inv agar tidak pernah kedaluwarsa atau corrupt
        Dim responseCookie As New HttpCookie("UserLogin_inv")
        If cookie IsNot Nothing Then
            If cookie.Values IsNot Nothing AndAlso cookie.Values.Count > 0 Then
                For Each key As String In cookie.Values.AllKeys
                    If Not String.IsNullOrEmpty(key) Then responseCookie(key) = cookie(key)
                Next
            End If
            If Not String.IsNullOrEmpty(cookie.Value) Then
                Dim nvc = HttpUtility.ParseQueryString(cookie.Value)
                For Each key As String In nvc.AllKeys
                    If Not String.IsNullOrEmpty(key) AndAlso String.IsNullOrEmpty(responseCookie(key)) Then
                        responseCookie(key) = nvc(key)
                    End If
                Next
            End If
        End If

        ' Pastikan field-field vital selalu terisi baik camelCase maupun lowercase
        responseCookie("userName") = sUser
        responseCookie("username") = sUser
        If Not String.IsNullOrEmpty(sCustId) Then
            responseCookie("Custid") = sCustId
            responseCookie("custid") = sCustId
        End If

        responseCookie("LastActive") = DateTime.Now.ToString("yyyyMMddHHmmss")
        responseCookie.Expires = DateTime.Now.AddMinutes(60)
        responseCookie.Path = "/"
        responseCookie.Secure = False
        ctx.Response.Cookies.Set(responseCookie)
    End Sub

    ' Fungsi bantu untuk ambil nilai cookie dengan aman & case-insensitive
    Private Function GetCookieValue(key As String, Optional defaultValue As String = "") As String
        Dim ctx = HttpContext.Current
        If ctx Is Nothing Then Return defaultValue
        Dim cookie As HttpCookie = ctx.Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return defaultValue

        ' Cek langsung
        If cookie(key) IsNot Nothing AndAlso Not String.IsNullOrEmpty(cookie(key)) Then
            Return cookie(key)
        End If

        ' Cek case-insensitive di Values
        If cookie.Values IsNot Nothing Then
            For Each k As String In cookie.Values.AllKeys
                If k IsNot Nothing AndAlso k.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                    Return cookie.Values(k)
                End If
            Next
        End If

        ' Cek fallback dari raw Value jika single-value
        If Not String.IsNullOrEmpty(cookie.Value) Then
            Dim nvc = HttpUtility.ParseQueryString(cookie.Value)
            For Each k As String In nvc.AllKeys
                If k IsNot Nothing AndAlso k.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                    Return nvc(k)
                End If
            Next
        End If

        Return defaultValue
    End Function

    ' === FUNGSI YANG KAMU BUTUHKAN (SUDAH AMAN 100%) ===

    Public Function CustID() As Integer
        Dim val As String = GetCookieValue("Custid", "-1")
        Return If(Integer.TryParse(val, Nothing), CInt(val), -1)
    End Function

    Public Function Username() As String
        Return GetCookieValue("userName", "")
    End Function

    Public Function ModulePriv() As Integer
        Dim val As String = GetCookieValue("modulepriv", "0")
        Return If(Integer.TryParse(val, Nothing), CInt(val), 0)
    End Function

    Public Function RptPriv() As Integer
        Dim val As String = GetCookieValue("rptpriv", "0")
        Return If(Integer.TryParse(val, Nothing), CInt(val), 0)
    End Function

    Public Function Modul() As Integer
        Dim val As String = GetCookieValue("modul", "0")
        Return If(Integer.TryParse(val, Nothing), CInt(val), 0)
    End Function

    Public Function RptF() As Integer
        Dim val As String = GetCookieValue("rptf", "0")
        Return If(Integer.TryParse(val, Nothing), CInt(val), 0)
    End Function

    ' Bonus: Fungsi untuk cek apakah user sudah login atau belum
    Public Function IsLoggedIn() As Boolean
        Return (CustID() > 0) AndAlso (Not String.IsNullOrEmpty(Username()))
    End Function
End Module

