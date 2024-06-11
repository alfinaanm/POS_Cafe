Imports System.Data.Odbc
Imports MySql.Data.MySqlClient

Public Class FormAdmin
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        ComboBox1.Items.Clear()
        ComboBox1.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        ComboBox1.Enabled = False

        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"
        Call Koneksi()
        Da = New MySqlDataAdapter("Select kodeadmin, namaadmin, leveladmin From tabel_admin", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tabel_admin")
        DataGridView1.DataSource = Ds.Tables("tabel_admin")
        DataGridView1.ReadOnly = True
    End Sub

    Sub Isi()
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        ComboBox1.Enabled = True
        ComboBox1.Items.Add("ADMIN")
        ComboBox1.Items.Add("USER")
    End Sub

    Private Sub FormMasterAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call Isi()
            Call NomorOtomatis()
            TextBox1.Enabled = False
            TextBox2.Focus()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim InputData As String = "insert into tabel_admin values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & ComboBox1.Text & "')"
                Cmd = New MySqlCommand(InputData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Input Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Edit" Then
            Button2.Text = "Simpan"
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call Isi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim UpdateData As String = "update tabel_admin set namaadmin = '" & TextBox2.Text & "', passwordadmin = '" & TextBox3.Text & "', leveladmin = '" & ComboBox1.Text & "' where kodeadmin = '" & TextBox1.Text & "'"
                Cmd = New MySqlCommand(UpdateData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Update Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New MySqlCommand("Select * From tabel_admin where kodeadmin = '" & TextBox1.Text & "'", Conn)
            Dr = Cmd.ExecuteReader
            Dr.Read()
            If Not Dr.HasRows Then
                MsgBox("Kode Admin Tidak Ada")
            Else
                TextBox1.Text = Dr.Item("kodeadmin")
                TextBox2.Text = Dr.Item("namaadmin")
                TextBox3.Text = Dr.Item("passwordadmin")
                ComboBox1.Text = Dr.Item("leveladmin")
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Hapus" Then
            Button3.Text = "Delete"
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Text = "Batal"
            Call Isi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim HapusData As String = "delete from tabel_admin where kodeadmin = '" & TextBox1.Text & "'"
                Cmd = New MySqlCommand(HapusData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Hapus Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "Tutup" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub

    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New MySqlCommand("Select * from tabel_admin where kodeadmin in(select max(kodeadmin) from tabel_admin)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Dr = Cmd.ExecuteReader
        Dr.Read()
        If Not Dr.HasRows Then
            UrutanKode = "ADM" + "01"
        Else
            Hitung = Microsoft.VisualBasic.Right(Dr.GetString(0), 2) + 1
            UrutanKode = "ADM" + Microsoft.VisualBasic.Right("00" & Hitung, 2)
        End If
        TextBox1.Text = UrutanKode
    End Sub
End Class