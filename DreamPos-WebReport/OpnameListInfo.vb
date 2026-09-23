Imports System
Imports System.Linq
Imports System.Web
<Serializable>
Public Class OpnameListInfo
    Public Property invdate As Date
    Public Property inv_date As String
    Public Property custid As Integer
    Public Property stkid As Integer
    Public Property stkno As String
    Public Property stkname As String
    Public Property ccid As Integer
    Public Property ccname As String
    Public Property opencost As Decimal
    'Public Property openqtyu1 As Decimal
    'Public Property openqtyu2 As Decimal
    Public Property openqty As Decimal
    Public Property cost As Decimal
    Public Property ttlqty As Decimal
    Public Property bclose As Integer
    Public Property closeqty As Decimal
    Public Property closecost As Decimal
    Public Property varianceqty As Decimal
    Public Property factor1 As Decimal
    Public Property unit1 As String
    Public Property unit2 As String
    Public Property createddate As Date
    Public Property trtype As Integer
    Public Property type As String
End Class
