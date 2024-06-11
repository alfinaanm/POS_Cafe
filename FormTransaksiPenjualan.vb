Imports MySql.Data.MySqlClient

Public Class FormTransaksiPenjualan
    Dim TglMySql As String

    Sub KondisiAwal()
        lblStruk.Text = ""
        lblMenu.Text = ""
        lblHarga.Text = ""
        txtJumlah.Text = ""
        lblJenis.Text = ""
        txtTunai.Text = ""
        LblKembalian.Text = ""
        lblTanggal.Text = Today
        Call MunculKode()
        Call NomorOtomatis()
        Call BuatKolom()
        lblTotal.Text = "0"
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblJam.Text = TimeOfDay
    End Sub

    Private Sub FormTransaksiPenjualan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
        Call MunculKode()
        Call NomorOtomatis()
        Call BuatKolom()
        lblTotal.Text = "0"
    End Sub

    Sub MunculKode()
        Call Koneksi()
        Cmd = New MySqlCommand("Select * from tabel_barang", Conn)
        Dr = Cmd.ExecuteReader
        Do While Dr.Read
            cbKode.Items.Add(Dr.Item(0))
        Loop
    End Sub

    Private Sub cbKode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbKode.SelectedIndexChanged
        Call Koneksi()
        Cmd = New MySqlCommand("Select * from tabel_barang where kode = '" & cbKode.Text & "'", Conn)
        Dr = Cmd.ExecuteReader
        Dr.Read()
        If Dr.HasRows Then
            lblMenu.Text = Dr!Menu
            lblHarga.Text = Dr!Harga
            lblJenis.Text = Dr!Jenis
        End If
    End Sub

    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New MySqlCommand("Select * from tabel_jual where no in(select max(no) from tabel_jual)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Dr = Cmd.ExecuteReader
        Dr.Read()
        If Not Dr.HasRows Then
            UrutanKode = "J" + Format(Now, "ddMMyy") + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Dr.GetString(0), 9) + 1
            UrutanKode = "J" + Format(Now, "ddMMyy") + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        lblStruk.Text = UrutanKode
    End Sub

    Sub BuatKolom()
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Nomor Struk", "Nomor Struk")
        DataGridView1.Columns.Add("Kode Menu", "Kode Menu")
        DataGridView1.Columns.Add("Menu", "Menu")
        DataGridView1.Columns.Add("Harga", "Harga")
        DataGridView1.Columns.Add("Jumlah", "Jumlah")
        DataGridView1.Columns.Add("Jenis", "Jenis")
        DataGridView1.Columns.Add("Subtotal", "Subtotal")
    End Sub

    Private Sub btnInput_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        If lblMenu.Text = "" Or lblHarga.Text = "" Then
            MsgBox("Silahkan Masukkan Kode!")
        End If
        DataGridView1.Rows.Add(New String() {lblStruk.Text, cbKode.Text, lblMenu.Text, lblHarga.Text, txtJumlah.Text, lblJenis.Text, Val(lblHarga.Text) * Val(txtJumlah.Text)})
        Call CariSubtotal()
        Call RumusCariItem()
    End Sub

    Sub CariSubtotal()
        Dim Hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            Hitung = Hitung + DataGridView1.Rows(i).Cells(4).Value
            lblTotal.Text = Hitung
        Next
    End Sub

    Private Sub txtTunai_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTunai.KeyPress
        If e.KeyChar = Chr(13) Then
            If Val(txtTunai.Text) < Val(lblTotal.Text) Then
                MsgBox("Pembayaran Kurang!")
            ElseIf Val(txtTunai.Text) = Val(lblTotal.Text) Then
                LblKembalian.Text = "0"
            ElseIf Val(txtTunai.Text) > Val(lblTotal.Text) Then
                LblKembalian.Text = Val(txtTunai.Text) - Val(lblTotal.Text)
            End If
        End If
    End Sub

    Sub RumusCariItem()
        Dim HitungItem As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            HitungItem = HitungItem + DataGridView1.Rows(i).Cells(4).Value
            lblItem.Text = HitungItem
        Next
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If LblKembalian.Text = "" Or lblTotal.Text = "" Or txtTunai.Text = "" Then
            MsgBox("Transaksi tidak ada, silahkan lakukan transaksi terlebih dahulu")
        Else
            TglMySql = Format(lblTanggal, "ddMMyy")
            Dim SimpanJual As String = "Insert into tabel_jual values('" & lblStruk.Text & "', '" & TglMySql & "', '" & lblJam.Text & "', '" & lblMenu.Text & "', '" & lblTotal.Text & "', '" & txtTunai.Text & "', '" & LblKembalian.Text & "')"
            Cmd = New MySqlCommand(SimpanJual, Conn)
            Cmd.ExecuteNonQuery()

            For Baris As Integer = 0 To DataGridView1.Rows.Count - 2
                Dim SimpanDetail As String = "insert into tabel_detail_jual values('" & lblStruk.Text & "','" & DataGridView1.Rows(Baris).Cells(0).Value & "','" & DataGridView1.Rows(Baris).Cells(4).Value & "')"
                Cmd = New MySqlCommand(SimpanDetail, Conn)
                Cmd.ExecuteNonQuery()

                Cmd = New MySqlCommand("select * from tabel_barang where kode = '" & DataGridView1.Rows(Baris).Cells(0).Value & "'", Conn)
                Dr = Cmd.ExecuteReader
                Dr.Read()
                Dim KurangiStok As String = "update tabel_barang set jumlah = '" & Dr.Item("Jumlah") - DataGridView1.Rows(Baris).Cells(3).Value & "' where kode = '" & DataGridView1.Rows(Baris).Cells(0).Value & "'"
                Cmd = New MySqlCommand(KurangiStok, Conn)
                Cmd.ExecuteNonQuery()
            Next
            Call KondisiAwal()
            MsgBox("Transaksi Telah Berhasil Disimpan")
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class