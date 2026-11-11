Imports System.IO
Imports System.Web
Imports System.Xml
Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json.Linq

Public Class clsSchemaMigrator
    Public Class MigrationDiff
        Public Property MissingTables As New List(Of String)
        Public Property MissingColumns As New List(Of MissingColumnInfo)

        Public ReadOnly Property HasChanges As Boolean
            Get
                Return MissingTables.Count > 0 OrElse MissingColumns.Count > 0
            End Get
        End Property

        Public ReadOnly Property TotalChanges As Integer
            Get
                Return MissingTables.Count + MissingColumns.Count
            End Get
        End Property
    End Class

    Public Class MissingColumnInfo
        Public Property TableName As String
        Public Property ColumnName As String
        Public Property Definition As String
    End Class

    Public Class MigrationResult
        Public Property Success As Boolean
        Public Property ExecutedCount As Integer
        Public Property Messages As New List(Of String)
        Public Property ErrorMessage As String
    End Class

    Private Shared ReadOnly MaintenanceFlagFile As String = "~/Pathdata/maintenance.flag"
    Private Shared ReadOnly LogFilePath As String = "~/ErrLog.txt"

    ''' <summary>
    ''' Membersihkan / merotasi file log jika ukurannya melebihi 5 MB atau arsip lebih dari 14 hari
    ''' </summary>
    Private Shared Sub CheckAndRotateLogFile(ByVal logPath As String)
        Try
            If File.Exists(logPath) Then
                Dim fi As New FileInfo(logPath)
                Dim maxBytes As Long = 5 * 1024 * 1024 ' 5 MB
                If fi.Length > maxBytes Then
                    Dim archivePath As String = System.IO.Path.Combine(fi.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(fi.Name) & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".bak")
                    File.Copy(logPath, archivePath, True)
                    File.WriteAllText(logPath, DateTime.Now.ToString("dd/MM/yy HH:mm:ss") & " ==> [INFO] Log dirotasi otomatis karena melebihi batas 5MB." & Environment.NewLine)
                End If

                If fi.Directory.Exists Then
                    For Each oldFile In fi.Directory.GetFiles(System.IO.Path.GetFileNameWithoutExtension(fi.Name) & "_*.bak")
                        If (DateTime.Now - oldFile.LastWriteTime).TotalDays > 14 Then
                            oldFile.Delete()
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Menyelesaikan path file baik dalam konteks HttpContext, HttpRuntime, maupun non-HTTP / thread eksternal
    ''' </summary>
    Public Shared Function ResolvePath(ByVal virtualOrRelativePath As String) As String
        Dim cleanPath As String = virtualOrRelativePath.Replace("~/", "").Replace("/", "\")
        If cleanPath.StartsWith("\") Then cleanPath = cleanPath.Substring(1)

        Try
            If HttpContext.Current IsNot Nothing Then
                Return HttpContext.Current.Server.MapPath(virtualOrRelativePath)
            End If
        Catch
        End Try

        Try
            If Not String.IsNullOrEmpty(HttpRuntime.AppDomainAppPath) Then
                Return System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, cleanPath)
            End If
        Catch
        End Try

        Try
            Dim asmLoc As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            If Not String.IsNullOrEmpty(asmLoc) Then
                Dim binDir As String = System.IO.Path.GetDirectoryName(asmLoc)
                If binDir.EndsWith("\bin", StringComparison.OrdinalIgnoreCase) Then
                    Return System.IO.Path.Combine(System.IO.Directory.GetParent(binDir).FullName, cleanPath)
                Else
                    Return System.IO.Path.Combine(binDir, cleanPath)
                End If
            End If
        Catch
        End Try

        Return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cleanPath)
    End Function

    ''' <summary>
    ''' Mencatat log detail ke ErrLog.txt dengan pembatasan ukuran otomatis
    ''' </summary>
    Public Shared Sub WriteLog(ByVal message As String, Optional ByVal isError As Boolean = False)
        Try
            Dim path As String = ResolvePath(LogFilePath)
            ' Pastikan tidak di folder bin
            Dim parts = path.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar)
            If parts.Any(Function(p) String.Equals(p, "bin", StringComparison.OrdinalIgnoreCase)) Then
                path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrLog.txt")
            End If
            CheckAndRotateLogFile(path)
            Dim prefix As String = If(isError, "[ERROR]", "[INFO]")
            Dim logLine As String = String.Format("{0:dd/MM/yy HH:mm:ss} ==> {1} [clsSchemaMigrator] {2}{3}", DateTime.Now, prefix, message, Environment.NewLine)
            File.AppendAllText(path, logLine)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Mendapatkan connection string dari Web.config (ServerStr)
    ''' </summary>
    Public Shared Function GetConnectionString() As String
        Try
            Return My.Settings.ServerStr
        Catch ex As Exception
            WriteLog("Gagal membaca ServerStr dari Web.config: " & ex.Message, isError:=True)
            Return String.Empty
        End Try
    End Function

    Private Shared ReadOnly WebConfigSyncLock As New Object()

    ''' <summary>
    ''' Mengecek apakah mode maintenance sedang aktif (dengan batas waktu 5 menit)
    ''' </summary>
    Public Shared Function IsMaintenanceActive() As Boolean
        Try
            Dim path As String = ResolvePath(MaintenanceFlagFile)
            If Not File.Exists(path) Then Return False
            Dim fi As New FileInfo(path)
            If (DateTime.Now - fi.LastWriteTime).TotalMinutes > 5 Then
                File.Delete(path)
                WriteLog("Maintenance flag otomatis dihapus karena melebihi batas waktu 5 menit.")
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Mengaktifkan atau menonaktifkan mode maintenance
    ''' </summary>
    Public Shared Sub SetMaintenanceMode(ByVal active As Boolean, Optional ByVal reason As String = "")
        Try
            Dim path As String = ResolvePath(MaintenanceFlagFile)
            If active Then
                Dim dir As String = System.IO.Path.GetDirectoryName(path)
                If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)
                File.WriteAllText(path, "MAINTENANCE_ACTIVE|" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "|" & reason)
                WriteLog("Maintenance mode DIAKTIFKAN: " & reason)
            Else
                If File.Exists(path) Then
                    File.Delete(path)
                End If
                WriteLog("Maintenance mode DINONAKTIFKAN.")
            End If
        Catch ex As Exception
            WriteLog("Error SetMaintenanceMode: " & ex.Message, isError:=True)
        End Try
    End Sub

    ''' <summary>
    ''' Mengecek saat startup / login apakah Web.config server memerlukan pembaruan konfigurasi
    ''' (termasuk sinkronisasi setting dari Web.config.template tanpa menimpa database connection).
    ''' </summary>
    Public Shared Function EnsureWebConfigUpdated() As Boolean
        Try
            Dim configPath As String = ResolvePath("~/Web.config")
            If Not File.Exists(configPath) Then Return False

            ' Pastikan file legacy bin\ErrLog.txt dihapus
            Dim legacyBinLog As String = ResolvePath("~/bin/ErrLog.txt")
            If File.Exists(legacyBinLog) Then
                Try
                    File.Delete(legacyBinLog)
                    WriteLog("EnsureWebConfigUpdated: Menghapus file legacy bin\ErrLog.txt.")
                Catch
                End Try
            End If

            Dim templatePath As String = ResolvePath("~/Web.config.template")
            Dim cfgTicks As Long = File.GetLastWriteTimeUtc(configPath).Ticks
            Dim tplTicks As Long = If(File.Exists(templatePath), File.GetLastWriteTimeUtc(templatePath).Ticks, 0L)
            Dim asmVer As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
            Dim cacheKey As String = String.Format("WEBCONFIG_VERIFIED_{0}_{1}_{2}", asmVer, cfgTicks, tplTicks)

            If HttpRuntime.Cache(cacheKey) IsNot Nothing Then
                Return False
            End If

            SyncLock WebConfigSyncLock
                If HttpRuntime.Cache(cacheKey) IsNot Nothing Then
                    Return False
                End If

                Dim pendingChanges As List(Of String) = InspectAndPatchWebConfig(configPath, templatePath, applyChanges:=False)
                If pendingChanges.Count = 0 Then
                    HttpRuntime.Cache.Insert(cacheKey, True, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(12))
                    Return False
                End If

                WriteLog(String.Format("EnsureWebConfigUpdated: Terdeteksi {0} item Web.config yang perlu diperbarui: {1}", pendingChanges.Count, String.Join("; ", pendingChanges.ToArray())))
                SetMaintenanceMode(True, "Sedang memperbarui konfigurasi Web.config secara otomatis...")

                Try
                    Dim backupPath As String = configPath & ".bak_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
                    File.Copy(configPath, backupPath, True)
                    WriteLog("EnsureWebConfigUpdated: Backup Web.config dibuat di " & backupPath)

                    Dim appliedChanges As List(Of String) = InspectAndPatchWebConfig(configPath, templatePath, applyChanges:=True)
                    For Each changeMsg In appliedChanges
                        WriteLog("EnsureWebConfigUpdated [APPLIED]: " & changeMsg)
                    Next

                    Dim newCfgTicks As Long = File.GetLastWriteTimeUtc(configPath).Ticks
                    Dim newCacheKey As String = String.Format("WEBCONFIG_VERIFIED_{0}_{1}_{2}", asmVer, newCfgTicks, tplTicks)
                    HttpRuntime.Cache.Insert(newCacheKey, True, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(12))
                    WriteLog("EnsureWebConfigUpdated: Pembaruan otomatis Web.config selesai.")
                    Return True
                Finally
                    SetMaintenanceMode(False)
                End Try
            End SyncLock
        Catch ex As Exception
            WriteLog("Error EnsureWebConfigUpdated: " & ex.ToString(), isError:=True)
            Return False
        End Try
    End Function

    Private Shared Function FindOrCreateXmlElement(ByVal doc As XmlDocument, ByVal parent As XmlNode, ByVal tagName As String, ByRef created As Boolean) As XmlElement
        For Each child As XmlNode In parent.ChildNodes
            If child.NodeType = XmlNodeType.Element AndAlso String.Equals(child.LocalName, tagName, StringComparison.OrdinalIgnoreCase) Then
                Return DirectCast(child, XmlElement)
            End If
        Next
        created = True
        Dim elem As XmlElement = doc.CreateElement(tagName)
        parent.AppendChild(elem)
        Return elem
    End Function

    Private Shared Function InspectAndPatchWebConfig(ByVal configPath As String, ByVal templatePath As String, ByVal applyChanges As Boolean) As List(Of String)
        Dim changes As New List(Of String)()
        Dim doc As New XmlDocument()
        doc.PreserveWhitespace = True
        doc.Load(configPath)

        Dim cfg As XmlElement = TryCast(doc.SelectSingleNode("/configuration"), XmlElement)
        If cfg Is Nothing Then Return changes

        Dim dummyCreated As Boolean = False
        Dim requiredMaxBytes As Long = 104857600L ' 100 MB
        Dim requiredMaxKb As Integer = 102400     ' 100 MB

        ' 1. Cek <system.webServer><security><requestFiltering><requestLimits maxAllowedContentLength="104857600" />
        Dim sysWebServer As XmlElement = FindOrCreateXmlElement(doc, cfg, "system.webServer", dummyCreated)
        Dim security As XmlElement = FindOrCreateXmlElement(doc, sysWebServer, "security", dummyCreated)
        Dim reqFiltering As XmlElement = FindOrCreateXmlElement(doc, security, "requestFiltering", dummyCreated)
        Dim reqLimits As XmlElement = FindOrCreateXmlElement(doc, reqFiltering, "requestLimits", dummyCreated)
        Dim curBytes As Long = 0
        If Not Long.TryParse(reqLimits.GetAttribute("maxAllowedContentLength"), curBytes) OrElse curBytes < requiredMaxBytes Then
            reqLimits.SetAttribute("maxAllowedContentLength", requiredMaxBytes.ToString())
            changes.Add("requestLimits.maxAllowedContentLength=" & requiredMaxBytes.ToString())
        End If

        ' 2. Cek <system.web><httpRuntime maxRequestLength="102400" />
        Dim sysWeb As XmlElement = FindOrCreateXmlElement(doc, cfg, "system.web", dummyCreated)
        Dim httpRuntime As XmlElement = FindOrCreateXmlElement(doc, sysWeb, "httpRuntime", dummyCreated)
        Dim curKb As Integer = 0
        If Not Integer.TryParse(httpRuntime.GetAttribute("maxRequestLength"), curKb) OrElse curKb < requiredMaxKb Then
            httpRuntime.SetAttribute("maxRequestLength", requiredMaxKb.ToString())
            changes.Add("system.web/httpRuntime.maxRequestLength=" & requiredMaxKb.ToString())
        End If

        ' 3. Cek setting errLog agar tidak mengarah ke dalam folder bin\ atau path dev lokal yang salah
        Dim errLogValNode As XmlNode = doc.SelectSingleNode("/configuration/applicationSettings/DreamPos_WebReport.My.MySettings/setting[@name='errLog']/value")
        If errLogValNode IsNot Nothing Then
            Dim curErrLog As String = If(errLogValNode.InnerText, "").Trim()
            If curErrLog.IndexOf("\bin\", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               curErrLog.IndexOf("/bin/", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               (curErrLog.IndexOf(":\", StringComparison.OrdinalIgnoreCase) >= 0 AndAlso Not File.Exists(curErrLog) AndAlso Not Directory.Exists(System.IO.Path.GetDirectoryName(curErrLog))) Then
                Dim safeLogPath As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(configPath), "ErrLog.txt")
                errLogValNode.InnerText = safeLogPath
                changes.Add("Relokasi setting errLog ke " & safeLogPath)
            End If
        End If

        ' 4. Cek MIME type .woff dan .woff2 pada <system.webServer><staticContent>
        Dim staticContent As XmlElement = FindOrCreateXmlElement(doc, sysWebServer, "staticContent", dummyCreated)
        Dim fontExts As String()() = {
            New String() {".woff", "application/font-woff"},
            New String() {".woff2", "application/font-woff2"}
        }
        For Each f In fontExts
            Dim ext As String = f(0)
            Dim mime As String = f(1)
            If staticContent.SelectSingleNode("remove[@fileExtension='" & ext & "']") Is Nothing Then
                Dim remNode As XmlElement = doc.CreateElement("remove")
                remNode.SetAttribute("fileExtension", ext)
                staticContent.AppendChild(remNode)
                changes.Add("staticContent/remove[" & ext & "]")
            End If
            If staticContent.SelectSingleNode("mimeMap[@fileExtension='" & ext & "']") Is Nothing Then
                Dim mimeNode As XmlElement = doc.CreateElement("mimeMap")
                mimeNode.SetAttribute("fileExtension", ext)
                mimeNode.SetAttribute("mimeType", mime)
                staticContent.AppendChild(mimeNode)
                changes.Add("staticContent/mimeMap[" & ext & "]")
            End If
        Next

        ' 5. Sinkronisasi setting / key baru dari Web.config.template tanpa menimpa nilai server eksisting
        If Not String.IsNullOrEmpty(templatePath) AndAlso File.Exists(templatePath) Then
            Try
                Dim tplDoc As New XmlDocument()
                tplDoc.Load(templatePath)

                ' 5a. Merge appSettings
                Dim tplAppSettings As XmlNodeList = tplDoc.SelectNodes("/configuration/appSettings/add[@key]")
                If tplAppSettings IsNot Nothing AndAlso tplAppSettings.Count > 0 Then
                    Dim targetAppSettings As XmlElement = FindOrCreateXmlElement(doc, cfg, "appSettings", dummyCreated)
                    For Each tplAdd As XmlElement In tplAppSettings
                        Dim k As String = tplAdd.GetAttribute("key")
                        If Not String.IsNullOrEmpty(k) AndAlso targetAppSettings.SelectSingleNode("add[@key='" & k & "']") Is Nothing Then
                            Dim imported As XmlNode = doc.ImportNode(tplAdd, True)
                            targetAppSettings.AppendChild(imported)
                            changes.Add("Tambah appSettings[" & k & "] dari Web.config.template")
                        End If
                    Next
                End If

                ' 5b. Merge applicationSettings (MySettings)
                Dim tplMySettings As XmlNodeList = tplDoc.SelectNodes("/configuration/applicationSettings/DreamPos_WebReport.My.MySettings/setting[@name]")
                Dim targetMySettings As XmlNode = doc.SelectSingleNode("/configuration/applicationSettings/DreamPos_WebReport.My.MySettings")
                If tplMySettings IsNot Nothing AndAlso targetMySettings IsNot Nothing Then
                    For Each tplSet As XmlElement In tplMySettings
                        Dim sName As String = tplSet.GetAttribute("name")
                        ' Jangan menimpa ServerStr atau slssvr milik server
                        If Not String.IsNullOrEmpty(sName) AndAlso targetMySettings.SelectSingleNode("setting[@name='" & sName & "']") Is Nothing Then
                            Dim imported As XmlNode = doc.ImportNode(tplSet, True)
                            targetMySettings.AppendChild(imported)
                            changes.Add("Tambah applicationSettings[" & sName & "] dari Web.config.template")
                        End If
                    Next
                End If

                ' 5c. Merge location blocks
                Dim tplLocations As XmlNodeList = tplDoc.SelectNodes("/configuration/location[@path]")
                If tplLocations IsNot Nothing Then
                    Dim runtimeNode As XmlNode = doc.SelectSingleNode("/configuration/runtime")
                    For Each tplLoc As XmlElement In tplLocations
                        Dim lPath As String = tplLoc.GetAttribute("path")
                        If Not String.IsNullOrEmpty(lPath) AndAlso doc.SelectSingleNode("/configuration/location[@path='" & lPath & "']") Is Nothing Then
                            Dim imported As XmlNode = doc.ImportNode(tplLoc, True)
                            If runtimeNode IsNot Nothing Then
                                cfg.InsertBefore(imported, runtimeNode)
                            Else
                                cfg.AppendChild(imported)
                            End If
                            changes.Add("Tambah location[" & lPath & "] dari Web.config.template")
                        End If
                    Next
                End If
            Catch exTpl As Exception
                WriteLog("Peringatan saat membaca Web.config.template: " & exTpl.Message, isError:=True)
            End Try
        End If

        If applyChanges AndAlso changes.Count > 0 Then
            Dim settings As New XmlWriterSettings()
            settings.Indent = True
            settings.IndentChars = "  "
            settings.Encoding = New System.Text.UTF8Encoding(False)
            Using writer As XmlWriter = XmlWriter.Create(configPath, settings)
                doc.Save(writer)
            End Using
        End If

        Return changes
    End Function

    ''' <summary>
    ''' Membandingkan schema_master.json dengan struktur database klien secara in-memory cached
    ''' </summary>
    Public Shared Function CheckSchemaDiff(Optional ByVal forceCheck As Boolean = False) As MigrationDiff
        Dim diff As New MigrationDiff()
        Dim context As HttpContext = HttpContext.Current

        Dim jsonPath As String = ResolvePath("~/Pathdata/schema_master.json")
        If Not File.Exists(jsonPath) Then
            Return diff
        End If

        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim currentVer As String = asm.GetName().Version.ToString()
        Dim cacheKey As String = "DB_SCHEMA_SYNCED_VER_" & currentVer

        If Not forceCheck AndAlso context IsNot Nothing AndAlso context.Application(cacheKey) IsNot Nothing Then
            Return diff
        End If

        Try
            Dim jsonContent As String = File.ReadAllText(jsonPath)
            Dim rootObj As JObject = JObject.Parse(jsonContent)
            Dim tablesObj As JObject = rootObj("tables")
            If tablesObj Is Nothing Then Return diff

            Dim connStr As String = GetConnectionString()
            If String.IsNullOrEmpty(connStr) Then Return diff

            Using conn As New MySqlConnection(connStr)
                conn.Open()

                Dim currentTables As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
                Using cmd As New MySqlCommand("SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_TYPE = 'BASE TABLE'", conn)
                    Using rdr As MySqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            currentTables.Add(rdr.GetString(0))
                        End While
                    End Using
                End Using

                Dim currentColumns As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
                Using cmd As New MySqlCommand("SELECT TABLE_NAME, COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE()", conn)
                    Using rdr As MySqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            currentColumns.Add(rdr.GetString(0) & "." & rdr.GetString(1))
                        End While
                    End Using
                End Using

                For Each tblProp In tablesObj.Properties()
                    Dim tblName As String = tblProp.Name
                    If Not currentTables.Contains(tblName) Then
                        diff.MissingTables.Add(tblName)
                    Else
                        Dim tblData As JObject = CType(tblProp.Value, JObject)
                        Dim colsObj As JObject = tblData("columns")
                        If colsObj IsNot Nothing Then
                            For Each colProp In colsObj.Properties()
                                Dim colName As String = colProp.Name
                                Dim key As String = tblName & "." & colName
                                If Not currentColumns.Contains(key) Then
                                    Dim colDef As String = colProp.Value("definition").ToString()
                                    diff.MissingColumns.Add(New MissingColumnInfo With {
                                        .TableName = tblName,
                                        .ColumnName = colName,
                                        .Definition = colDef
                                    })
                                End If
                            Next
                        End If
                    End If
                Next
            End Using

            If Not diff.HasChanges AndAlso context IsNot Nothing Then
                context.Application(cacheKey) = True
            End If
        Catch ex As Exception
            WriteLog("Error CheckSchemaDiff: " & ex.ToString(), isError:=True)
        End Try

        Return diff
    End Function

    ''' <summary>
    ''' Mengeksekusi penambahan tabel dan kolom yang kurang secara otomatis dan aman
    ''' </summary>
    Public Shared Function ExecuteMigration() As MigrationResult
        Dim result As New MigrationResult()
        Dim jsonPath As String = ResolvePath("~/Pathdata/schema_master.json")

        If Not File.Exists(jsonPath) Then
            result.ErrorMessage = "File schema_master.json tidak ditemukan."
            Return result
        End If

        WriteLog("--- MEMULAI PROSES MIGRASI DATABASE OTOMATIS ---")
        Dim diff As MigrationDiff = CheckSchemaDiff(forceCheck:=True)
        If Not diff.HasChanges Then
            result.Success = True
            result.Messages.Add("Database sudah sesuai skema master.")
            Return result
        End If

        Dim connStr As String = GetConnectionString()
        Dim jsonContent As String = File.ReadAllText(jsonPath)
        Dim rootObj As JObject = JObject.Parse(jsonContent)
        Dim tablesObj As JObject = rootObj("tables")

        SetMaintenanceMode(True, "Sedang melakukan pembaruan struktur database otomatis.")

        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                ' A. Tambah Tabel Baru
                For Each tblName In diff.MissingTables
                    Dim tblData As JObject = tablesObj(tblName)
                    If tblData IsNot Nothing Then
                        Dim colsObj As JObject = tblData("columns")
                        Dim pkArray As JArray = tblData("primary_key")

                        Dim colDefs As New List(Of String)
                        For Each colProp In colsObj.Properties()
                            colDefs.Add("`" & colProp.Name & "` " & colProp.Value("definition").ToString())
                        Next

                        If pkArray IsNot Nothing AndAlso pkArray.Count > 0 Then
                            Dim pks As New List(Of String)
                            For Each pk In pkArray
                                pks.Add("`" & pk.ToString() & "`")
                            Next
                            colDefs.Add("PRIMARY KEY (" & String.Join(", ", pks.ToArray()) & ")")
                        End If

                        Dim createSql As String = "CREATE TABLE IF NOT EXISTS `" & tblName & "` (" & String.Join(", ", colDefs.ToArray()) & ") ENGINE=InnoDB DEFAULT CHARSET=utf8;"
                        WriteLog("Menjalankan DDL: " & createSql)

                        Using cmd As New MySqlCommand(createSql, conn)
                            cmd.ExecuteNonQuery()
                        End Using
                        result.ExecutedCount += 1
                        result.Messages.Add("Tabel `" & tblName & "` berhasil dibuat.")
                    End If
                Next

                ' B. Tambah Kolom Baru
                For Each col In diff.MissingColumns
                    Dim alterSql As String = "ALTER TABLE `" & col.TableName & "` ADD COLUMN `" & col.ColumnName & "` " & col.Definition
                    WriteLog("Menjalankan DDL: " & alterSql)

                    Try
                        Using cmd As New MySqlCommand(alterSql, conn)
                            cmd.ExecuteNonQuery()
                        End Using
                        result.ExecutedCount += 1
                        result.Messages.Add("Kolom `" & col.ColumnName & "` berhasil ditambahkan pada tabel `" & col.TableName & "`.")
                    Catch exCol As MySqlException When exCol.Number = 1060 OrElse exCol.Message.ToLower().Contains("duplicate column")
                        result.Messages.Add("Kolom `" & col.ColumnName & "` sudah ada pada tabel `" & col.TableName & "`.")
                    End Try
                Next

                result.Success = True
                WriteLog(String.Format("--- MIGRASI SELESAI: {0} DDL dieksekusi ---", result.ExecutedCount))

                Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
                Dim currentVer As String = asm.GetName().Version.ToString()
                If HttpContext.Current IsNot Nothing Then
                    HttpContext.Current.Application("DB_SCHEMA_SYNCED_VER_" & currentVer) = True
                End If
            End Using
        Catch ex As Exception
            result.Success = False
            result.ErrorMessage = ex.Message
            WriteLog("GAGAL EKSEKUSI MIGRASI: " & ex.ToString(), isError:=True)
        Finally
            SetMaintenanceMode(False)
        End Try

        Return result
    End Function
End Class
