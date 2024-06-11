Imports MySql.Data.MySqlClient

Module Module1
    Public Conn As MySqlConnection
    Public Dr As MySqlDataReader
    Public Da As MySqlDataAdapter
    Public Cmd As MySqlCommand
    Public Ds As DataSet

    Sub Koneksi()
        Try
            Dim SqlConn As String
            SqlConn = "server = localhost; uid = root; database = db_poscafe"
            Conn = New MySqlConnection(SqlConn)
            If Conn.State = ConnectionState.Closed Then
                Conn.Open()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Module
