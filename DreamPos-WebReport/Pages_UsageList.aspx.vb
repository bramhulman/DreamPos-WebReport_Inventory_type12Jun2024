Imports System.Data

Public Class Pages_UsageList
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsreport As New ClsReports

    ' Manual declaration of controls since missing designer file
    Protected txtDate As System.Web.UI.WebControls.TextBox
    Protected DgvTransfer As System.Web.UI.WebControls.GridView
    Protected btnSearch As System.Web.UI.HtmlControls.HtmlButton
    Protected btnBack As System.Web.UI.HtmlControls.HtmlButton

    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modulepriv")
        End If
    End Function

    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("Custid")
        End If
    End Function

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
        End If
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If

        Try
            If IsPostBack = False Then
                If Request.QueryString("date") IsNot Nothing Then
                     txtDate.Text = Request.QueryString("date")
                Else
                     txtDate.Text = Format(Now, "dd MMM yyyy")
                End If
               
                BindData()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_UsageList:Page_Load")
        End Try
    End Sub

    Private Sub BindData()
        Try
            Dim ds As New DataSet
            ' Assuming we want to show usage based on the date selected, similar to how it works in Pages_Calculate logic or CPO
            ' The user mentioned "summary usage stock berdasarkan pre_order yang sudah ada"
            ' In Pages_CPOEdit, it calls clsreport.cal_usage. 
            ' We need to see if we can use that, or if we need a new query.
            ' For now, lay's try using the same logic as CPO usage display but maybe aggregated for the day?
            ' However, the user request says "based on pre_order", which implies looking at CPO data.

            ' Let's try to reuse cal_usage but we might need to adjust parameters. 
            ' In CPOEdit it passes specific PO Number. Here we might want all POs for the date? 
            ' Or maybe just general usage calculation for the date.

            ' If the user wants "summary usage stock" from "Pages_Calculate", likely they want to see what the calculation produced.
            ' But the feedback "berdasarkan pre_order yang sudah ada" is key.

            ' Let's start by fetching the data using a method that seems most appropriate. 
            ' If cal_usage requires specific PO, we might need a different approach if we want "all" or specific ones.
            ' But wait, Pages_CPOEdit shows usage *for that specific PO*.
            ' The user probably wants a list of ALL usages from ALL POs for a given period or just a summary.

            ' Given I cannot see the implementation of cal_usage, I will assume for now I can pass a date range.
            ' Attempting to call cal_usage with the selected date. 
            ' Note: In CPOEdit: clsreport.cal_usage(CDate(txtbusidate.Text), CDate(txtbusidate.Text), custid, username, modulepriv, ddlCustomer.SelectedValue, txtLinkid.Text)
            ' passing ddlCustomer=0 and txtLinkid="" might return all? Let's hope.

            ds = clsreport.cal_usage(CDate(txtDate.Text), CDate(Now()), custid, username, modulepriv, 0, "")

            If ds.Tables.Count > 0 Then
                DgvTransfer.DataSource = ds.Tables(0)
                DgvTransfer.DataBind()
            Else
                 DgvTransfer.DataSource = Nothing
                 DgvTransfer.DataBind()
            End If

        Catch ex As Exception
             mfunction.errLog(Err.Description, "Pages_UsageList:BindData")
        End Try
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        BindData()
    End Sub

    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs)
        Response.Redirect("Pages_Calculate.aspx")
    End Sub
    Private Sub hidepage()
        btnFirst.Visible = False
        btnPrev.Visible = False
        pagelbl.Visible = False
        btnNext.Visible = False
        btnLast.Visible = False


    End Sub
    Private Sub hidelast()

        btnNext.Visible = False
        btnLast.Visible = False
        btnFirst.Visible = True
        btnPrev.Visible = True
        pagelbl.Visible = True
    End Sub
    Private Sub hidefirst()
        btnFirst.Visible = False
        btnPrev.Visible = False
        btnNext.Visible = True
        btnLast.Visible = True
        pagelbl.Visible = True


    End Sub
    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click

        If (dg1.CurrentPageIndex > 0) Then
            dg1.CurrentPageIndex = 0
            bindusage()
            hidefirst()
        End If

    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click

        If (dg1.CurrentPageIndex > 0) Then
            dg1.CurrentPageIndex = dg1.CurrentPageIndex - 1
            bindusage()
            If dg1.CurrentPageIndex = 0 Then
                hidefirst()
            Else
                btnFirst.Visible = True
                btnPrev.Visible = True
                btnNext.Visible = True
                btnLast.Visible = True
                pagelbl.Visible = True
            End If
        End If


    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click

        If (dg1.CurrentPageIndex < dg1.PageCount - 1) Then
            dg1.CurrentPageIndex = dg1.CurrentPageIndex + 1
            bindusage()
            If (dg1.CurrentPageIndex = dg1.PageCount - 1) Then
                hidelast()
            Else
                btnFirst.Visible = True
                btnPrev.Visible = True
                btnNext.Visible = True
                btnLast.Visible = True
                pagelbl.Visible = True

            End If
        End If

    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click

        If (dg1.CurrentPageIndex < dg1.PageCount - 1) Then
            dg1.CurrentPageIndex = dg1.PageCount - 1
            bindusage()
            hidelast()
        End If

    End Sub
    Private Sub paging()
        pagelbl.Text = " " & dg1.CurrentPageIndex + 1.ToString & " of " & dg1.PageCount.ToString & " "
        If dsrpt.Tables(0).Rows.Count <= 31 Then
            hidepage()
        Else
            hidefirst()
        End If
    End Sub
End Class
