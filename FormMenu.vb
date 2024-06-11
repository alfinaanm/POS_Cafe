Imports System.Data.Odbc
Imports MySql.Data.MySqlClient

Public Class FormMenu
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        btnInput.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnInput.Text = "Input"
        btnEdit.Text = "Edit"
        btnDelete.Text = "Delete"
        btnClose.Text = "Close"
        Call Koneksi()
        Da = New MySqlDataAdapter("Select * From tabel_barang", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tabel_barang")
        DataGridView1.DataSource = Ds.Tables("tabel_barang")
        DataGridView1.ReadOnly = True
    End Sub

    Sub Isi()
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True
        Call MunculMenu()
        Call MunculJenis()
    End Sub

    Sub MunculMenu()
        Call Koneksi()
        Cmd = New MySqlCommand("select distinct menu from tabel_barang", Conn)
        Dr = Cmd.ExecuteReader
        ComboBox1.Items.Clear()
        Do While Dr.Read
            ComboBox1.Items.Add(Dr.Item("menu"))
        Loop
    End Sub

    Sub MunculJenis()
        Call Koneksi()
        Cmd = New MySqlCommand("select distinct jenis from tabel_barang", Conn)
        Dr = Cmd.ExecuteReader
        ComboBox2.Items.Clear()
        Do While Dr.Read
            ComboBox2.Items.Add(Dr.Item("jenis"))
        Loop
    End Sub

    Private Sub FormMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnInput.Click
        If btnInput.Text = "Input" Then
            btnInput.Text = "Save"
            btnEdit.Enabled = False
            btnDelete.Enabled = False
            btnClose.Text = "Cancel"
            Call Isi()
            Call NomorOtomatis()
            TextBox1.Enabled = False
            TextBox2.Focus()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim InputData As String = "insert into tabel_barang values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & ComboBox1.Text & "','" & ComboBox2.Text & "')"
                Cmd = New MySqlCommand(InputData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Input Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If btnEdit.Text = "Edit" Then
            btnEdit.Text = "Save"
            btnInput.Enabled = False
            btnDelete.Enabled = False
            btnClose.Text = "Cancel"
            Call Isi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim UpdateData As String = "update tabel_barang set menu = '" & ComboBox1.Text & "', harga = '" & TextBox2.Text & "', jumlah = '" & TextBox3.Text & "', jenis = '" & ComboBox2.Text & "' where kode = '" & TextBox1.Text & "'"
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
            Cmd = New MySqlCommand("Select * From tabel_barang where kode = '" & TextBox1.Text & "'", Conn)
            Dr = Cmd.ExecuteReader
            Dr.Read()
            If Not Dr.HasRows Then
                MsgBox("Kode Tidak Ada")
            Else
                TextBox1.Text = Dr.Item("kode")
                TextBox2.Text = Dr.Item("harga")
                TextBox3.Text = Dr.Item("jumlah")
                ComboBox1.Text = Dr.Item("menu")
                ComboBox2.Text = Dr.Item("jenis")
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If btnDelete.Text = "Hapus" Then
            btnDelete.Text = "Delete"
            btnInput.Enabled = False
            btnEdit.Enabled = False
            btnClose.Text = "Cancel"
            Call Isi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim HapusData As String = "delete from tabel_barang where kode = '" & TextBox1.Text & "'"
                Cmd = New MySqlCommand(HapusData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Hapus Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If btnClose.Text = "Close" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "50000") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "100") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub

    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New MySqlCommand("Select * from tabel_barang where kode in(select max(kode) from tabel_barang)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Dr = Cmd.ExecuteReader
        Dr.Read()
        If Not Dr.HasRows Then
            UrutanKode = "MN" + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Dr.GetString(0), 3) + 1
            UrutanKode = "MN" + Microsoft.VisualBasic.Right("00" & Hitung, 3)
        End If
        TextBox1.Text = UrutanKode
    End Sub
End Class