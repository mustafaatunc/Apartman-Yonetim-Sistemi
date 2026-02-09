using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace Apartman_Yonetim_Sistemi
{
    public partial class Apartman_Yonetici_Islemleri : Form
    {
        public Apartman_Yonetici_Islemleri()
        {
            InitializeComponent();
        }

        
        
        string secili_apartman_id = "0";
        string secili_tc = "";

        sqlbaglantisi bag = new sqlbaglantisi();

        

        void Temizle()
        {
            textBox25.Clear(); // Ad
            textBox26.Clear(); // Soyad
            maskedTextBox2.Clear(); // TC
            maskedTextBox3.Clear(); // Telefon
            textBox27.Clear(); // Email
            textBox29.Clear(); // Şifre
            textBox30.Clear(); // Şifre Tekrar
            comboBox6.SelectedIndex = -1; // Durum
            comboBox7.SelectedIndex = -1; // Rol
            combo_apartman_adi.SelectedIndex = -1;
            combo_daire_no.SelectedIndex = -1;
        }

        void KullaniciListele()
        {
            try
            {
                using (SqlConnection baglanti = bag.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM kullanici", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView10.DataSource = dt;
                }

                // Tablo Ayarları
                dataGridView10.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView10.RowHeadersVisible = false;

                
                if (dataGridView10.Columns.Count > 10)
                {
                    dataGridView10.Columns[0].Visible = false; // ID
                    dataGridView10.Columns[8].Visible = false; // Şifre
                    dataGridView10.Columns[9].Visible = false; // Apartman ID

                    dataGridView10.Columns[1].HeaderText = "TC Kimlik No";
                    dataGridView10.Columns[2].HeaderText = "Ad";
                    dataGridView10.Columns[3].HeaderText = "Soyad";
                    dataGridView10.Columns[4].HeaderText = "Email";
                    dataGridView10.Columns[5].HeaderText = "Telefon";
                    dataGridView10.Columns[6].HeaderText = "Daire No";
                    dataGridView10.Columns[7].HeaderText = "Durum";
                    dataGridView10.Columns[10].HeaderText = "Rol";
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Kullanıcılar listelenirken hata: " + hata.Message);
            }
        }

        void ApartmanDoldur()
        {
            try
            {
                combo_apartman_adi.Items.Clear();
                comboBox1.Items.Clear();

                using (SqlConnection baglanti = bag.baglan())
                {
                    SqlCommand com = new SqlCommand("Select apartman_adi from apartman_islemleri", baglanti);
                    SqlDataReader oku = com.ExecuteReader();
                    while (oku.Read())
                    {
                        string aptAdi = oku["apartman_adi"].ToString();
                        combo_apartman_adi.Items.Add(aptAdi);
                        comboBox1.Items.Add(aptAdi);
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Apartmanlar listelenemedi: " + hata.Message);
            }
        }

        void DaireDoldur(string aptAdi, ComboBox hedefCombo)
        {
            try
            {
                hedefCombo.Items.Clear();
                string aptId = "0";

                using (SqlConnection baglanti = bag.baglan())
                {
                    
                    SqlCommand com = new SqlCommand("Select id from apartman_islemleri where apartman_adi=@p1", baglanti);
                    com.Parameters.AddWithValue("@p1", aptAdi);
                    SqlDataReader oku = com.ExecuteReader();
                    if (oku.Read())
                    {
                        aptId = oku["id"].ToString();
                    }
                    oku.Close(); 

                    
                    if (hedefCombo == combo_daire_no) secili_apartman_id = aptId;

                   
                    for (int i = 1; i <= 20; i++)
                    {
                        hedefCombo.Items.Add(i.ToString());
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Daireler listelenirken hata: " + hata.Message);
            }
        }

        private void Apartman_Yonetici_Islemleri_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            groupBox1.Visible = false;
            ApartmanDoldur();
            KullaniciListele();
        }

        // --- BUTON İŞLEMLERİ ---

        // KULLANICI EKLEME (Button15)
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox29.Text == textBox30.Text)
                {
                    using (SqlConnection baglanti = bag.baglan())
                    {
                        
                        string sorgu = "insert into kullanici(tc_no,ad,soyisim,email,telefon,daire_no,ev_durumu,rol,sifre,apartman_id) values(@tc,@ad,@soy,@mail,@tel,@daire,@drm,@rol,@sifre,@aptId)";

                        SqlCommand komut = new SqlCommand(sorgu, baglanti);
                        komut.Parameters.AddWithValue("@tc", maskedTextBox2.Text);
                        komut.Parameters.AddWithValue("@ad", textBox25.Text);
                        komut.Parameters.AddWithValue("@soy", textBox26.Text);
                        komut.Parameters.AddWithValue("@mail", textBox27.Text);
                        komut.Parameters.AddWithValue("@tel", maskedTextBox3.Text);
                        komut.Parameters.AddWithValue("@daire", combo_daire_no.Text);
                        komut.Parameters.AddWithValue("@drm", comboBox6.Text);
                        komut.Parameters.AddWithValue("@rol", comboBox7.Text);
                        komut.Parameters.AddWithValue("@sifre", textBox30.Text); // Şifresiz
                        komut.Parameters.AddWithValue("@aptId", secili_apartman_id);

                        komut.ExecuteNonQuery();

                        
                        if (comboBox7.Text == "Admin" || comboBox7.Text == "Apartman Yöneticisi")
                        {
                            string yetkiSorgu = "insert into yetki(tc,gelir_isleri,gider_isleri,kasa_isleri,borc_isleri,daire_isleri,kullanici_isleri) values(@tc,@y1,@y2,@y3,@y4,@y5,@y6)";
                            SqlCommand kmtYetki = new SqlCommand(yetkiSorgu, baglanti);
                            kmtYetki.Parameters.AddWithValue("@tc", maskedTextBox2.Text);
                            
                            kmtYetki.Parameters.AddWithValue("@y1", checkedListBox1.GetItemChecked(0) ? "1" : "0");
                            kmtYetki.Parameters.AddWithValue("@y2", checkedListBox1.GetItemChecked(1) ? "1" : "0");
                            kmtYetki.Parameters.AddWithValue("@y3", checkedListBox1.GetItemChecked(2) ? "1" : "0");
                            kmtYetki.Parameters.AddWithValue("@y4", checkedListBox1.GetItemChecked(3) ? "1" : "0");
                            kmtYetki.Parameters.AddWithValue("@y5", checkedListBox1.GetItemChecked(4) ? "1" : "0");
                            kmtYetki.Parameters.AddWithValue("@y6", checkedListBox1.GetItemChecked(5) ? "1" : "0");
                            kmtYetki.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Kullanıcı Başarıyla Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Temizle();
                    KullaniciListele();
                }
                else
                {
                    MessageBox.Show("Şifreler uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Ekleme hatası: " + hata.Message);
            }
        }

        // KULLANICI DÜZENLEME EKRANINA GEÇİŞ (Button16)
        private void button16_Click(object sender, EventArgs e)
        {
            if (dataGridView10.CurrentRow != null)
            {
                
                maskedTextBox5.Text = dataGridView10.CurrentRow.Cells[1].Value.ToString(); // TC
                textBox37.Text = dataGridView10.CurrentRow.Cells[2].Value.ToString(); // Ad
                textBox36.Text = dataGridView10.CurrentRow.Cells[3].Value.ToString(); // Soyad
                textBox35.Text = dataGridView10.CurrentRow.Cells[4].Value.ToString(); // Email
                maskedTextBox4.Text = dataGridView10.CurrentRow.Cells[5].Value.ToString(); // Tel
                comboBox2.Text = dataGridView10.CurrentRow.Cells[6].Value.ToString(); // Daire
                comboBox9.Text = dataGridView10.CurrentRow.Cells[7].Value.ToString(); // Durum
                comboBox8.Text = dataGridView10.CurrentRow.Cells[10].Value.ToString(); // Rol 

                
                textBox33.Text = dataGridView10.CurrentRow.Cells[8].Value.ToString();
                textBox32.Text = textBox33.Text;

                
                tabControl1.SelectedTab = tabPage4; 
            }
        }

        // KULLANICI GÜNCELLEME (Button20)
        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox33.Text == textBox32.Text)
                {
                    using (SqlConnection baglanti = bag.baglan())
                    {
                        string sorgu = "update kullanici set tc_no=@tc, ad=@ad, soyisim=@soy, email=@mail, telefon=@tel, daire_no=@daire, ev_durumu=@drm, rol=@rol, sifre=@sifre where tc_no=@eskiTc";
                        SqlCommand komut = new SqlCommand(sorgu, baglanti);
                        komut.Parameters.AddWithValue("@tc", maskedTextBox5.Text);
                        komut.Parameters.AddWithValue("@ad", textBox37.Text);
                        komut.Parameters.AddWithValue("@soy", textBox36.Text);
                        komut.Parameters.AddWithValue("@mail", textBox35.Text);
                        komut.Parameters.AddWithValue("@tel", maskedTextBox4.Text);
                        komut.Parameters.AddWithValue("@daire", comboBox2.Text);
                        komut.Parameters.AddWithValue("@drm", comboBox9.Text);
                        komut.Parameters.AddWithValue("@rol", comboBox8.Text);
                        komut.Parameters.AddWithValue("@sifre", textBox33.Text); // Şifresiz
                        komut.Parameters.AddWithValue("@eskiTc", dataGridView10.CurrentRow.Cells[1].Value.ToString()); // Güncellenen kişinin eski TC'si

                        komut.ExecuteNonQuery();
                    }
                    MessageBox.Show("Kullanıcı Güncellendi");
                    KullaniciListele();
                    tabControl1.SelectedTab = tabPage3; // Listeye geri dön
                }
                else
                {
                    MessageBox.Show("Şifreler uyuşmuyor!");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Güncelleme hatası: " + hata.Message);
            }
        }

        // KULLANICI SİLME (Button17)
        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView10.CurrentRow != null)
                {
                    string silinecekTc = dataGridView10.CurrentRow.Cells[1].Value.ToString();
                    DialogResult cevap = MessageBox.Show(silinecekTc + " TC numaralı kullanıcıyı silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cevap == DialogResult.Yes)
                    {
                        using (SqlConnection baglanti = bag.baglan())
                        {
                            SqlCommand komut = new SqlCommand("delete from kullanici where tc_no=@tc", baglanti);
                            komut.Parameters.AddWithValue("@tc", silinecekTc);
                            komut.ExecuteNonQuery();

                            
                            SqlCommand komut2 = new SqlCommand("delete from yetki where tc=@tc", baglanti);
                            komut2.Parameters.AddWithValue("@tc", silinecekTc);
                            komut2.ExecuteNonQuery();
                        }
                        MessageBox.Show("Kullanıcı Silindi");
                        KullaniciListele();
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Silme hatası: " + hata.Message);
            }
        }

        // ROL DÜZENLEME (Button19)
        private void button19_Click(object sender, EventArgs e)
        {
            if (dataGridView10.CurrentRow != null)
            {
                secili_tc = dataGridView10.CurrentRow.Cells[1].Value.ToString();
                textBox38.Text = secili_tc;
                tabControl1.SelectedTab = tabPage2; 
            }
        }

        // YETKİ GÜNCELLEME (Button21)
        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = bag.baglan())
                {
                    string sorgu = "update yetki set gelir_isleri=@y1, gider_isleri=@y2, kasa_isleri=@y3, borc_isleri=@y4, daire_isleri=@y5, kullanici_isleri=@y6 where tc=@tc";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@y1", checkedListBox2.GetItemChecked(0) ? "1" : "0");
                    komut.Parameters.AddWithValue("@y2", checkedListBox2.GetItemChecked(1) ? "1" : "0");
                    komut.Parameters.AddWithValue("@y3", checkedListBox2.GetItemChecked(2) ? "1" : "0");
                    komut.Parameters.AddWithValue("@y4", checkedListBox2.GetItemChecked(3) ? "1" : "0");
                    komut.Parameters.AddWithValue("@y5", checkedListBox2.GetItemChecked(4) ? "1" : "0");
                    komut.Parameters.AddWithValue("@y6", checkedListBox2.GetItemChecked(5) ? "1" : "0");
                    komut.Parameters.AddWithValue("@tc", textBox38.Text);

                    int etki = komut.ExecuteNonQuery();
                    if (etki == 0)
                    {
                        
                        MessageBox.Show("Bu kullanıcının yetki kaydı bulunamadı. Lütfen önce kullanıcıyı silip 'Admin' rolüyle tekrar ekleyin.");
                    }
                    else
                    {
                        MessageBox.Show("Yetkiler Güncellendi");
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Yetki hatası: " + hata.Message);
            }
        }

        // COMBOBOX SEÇİMLERİ
        private void combo_apartman_adi_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaireDoldur(combo_apartman_adi.Text, combo_daire_no);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DaireDoldur(comboBox1.Text, comboBox2);
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            panel_evde_yasayanlar.Controls.Clear();
            tabPage5.Controls.Clear(); // Önce temizle
            tabPage5.Controls.Add(panel_evde_yasayanlar); // Paneli geri ekle
            tabPage5.Controls.Add(panel1); // Üst paneli geri ekle

            try
            {
                using (SqlConnection baglanti = bag.baglan())
                {
                    SqlCommand cmd = new SqlCommand("select * from apartman_islemleri", baglanti);
                    SqlDataReader oku = cmd.ExecuteReader();

                    int i = 0;
                    int sol = 10;
                    int ust = 60;

                    while (oku.Read())
                    {
                        Button btn = new Button();
                        btn.Text = oku["apartman_adi"].ToString();
                        btn.Name = "btnApart_" + oku["id"].ToString();
                        btn.Tag = oku["id"].ToString();
                        btn.Size = new Size(150, 80);
                        btn.Location = new Point(sol, ust);
                        btn.Click += BtnApartman_Click; 

                        tabPage5.Controls.Add(btn);

                        sol += 160;
                        if (sol > 600) 
                        {
                            sol = 10;
                            ust += 90;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void BtnApartman_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string aptId = btn.Tag.ToString();
            
            MessageBox.Show(btn.Text + " Seçildi (ID: " + aptId + ")");
        }

        
        private void button18_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void combo_daire_no_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox31_TextChanged(object sender, EventArgs e) { }
    }
}