Imports System.Data.Odbc
Imports MySql.Data.MySqlClient

Public Class FormLogin
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Sub Terbuka()
        FormMenuUtama.LoginToolStripMenuItem.Enabled = False
        FormMenuUtama.LogoutToolStripMenuItem.Enabled = True
        FormMenuUtama.MasterToolStripMenuItem.Enabled = True
        FormMenuUtama.TransaksiToolStripMenuItem.Enabled = True
        FormMenuUtama.RekapTransaksiToolStripMenuItem.Enabled = True
    End Sub

    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MsgBox("Kode Admin dan Password tidak boleh kosong!!!")
        Else
            Call Koneksi()
            Cmd = New MySqlCommand("select * from tabel_admin where kodeadmin = '" & TextBox1.Text & "' AND passwordadmin = '" & TextBox2.Text & "'", Conn)
            Dr = Cmd.ExecuteReader
            Dr.Read()
            If Dr.HasRows Then
                Me.Close()
                Call Terbuka()
                FormMenuUtama.STLabel2.Text = Dr!KodeAdmin
                FormMenuUtama.STLabel4.Text = Dr!NamaAdmin
                FormMenuUtama.STLabel6.Text = Dr!LevelAdmin
                If FormMenuUtama.STLabel6.Text = "USER" Then
                    FormMenuUtama.AdminToolStripMenuItem.Enabled = False
                End If
            Else
                    MsgBox("Kode Admin dan Password salah!")
            End If
        End If
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub
End Class