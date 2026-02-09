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
using System.Net; 

namespace Apartman_Yonetim_Sistemi
{
    public partial class gelir_Tanimlari : Form
    {
        public gelir_Tanimlari()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        // --- YARDIMCI METOTLAR ---

        void Temizle()
        {
            comboBox1.SelectedIndex = -1; // Ekleme TC
            comboBox2.SelectedIndex = -1; // Güncelleme TC
            textBox2.Clear(); // Ekleme Kategori
            textBox3.Clear(); // Ekleme Tutar
            textBox4.Clear(); // Güncelleme Açıklama
            textBox5.Clear(); // Güncelleme Tutar
            
        }

        void GelirListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM gelirler", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView5.DataSource = dt;

                    // Tablo Tasarımı
                    if (dataGridView5.Columns.Count > 4)
                    {
                        dataGridView5.Columns[0].Visible = false; // ID
                        dataGridView5.Columns[1].HeaderText = "Kategori";
                        dataGridView5.Columns[2].HeaderText = "Tutar";
                        dataGridView5.Columns[3].HeaderText = "Açıklama"; // Veya Tarih (Sütun sırasına bakmak lazım)
                        dataGridView5.Columns[4].HeaderText = "Tarih";
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Listeleme hatası: " + hata.Message);
            }
        }

        void KullaniciDoldur()
        {
            try
            {
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlCommand komut = new SqlCommand("select tc_no from kullanici", baglanti);
                    SqlDataReader dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        string tc = dr["tc_no"].ToString();
                        comboBox1.Items.Add(tc);
                        comboBox2.Items.Add(tc);
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Kullanıcılar yüklenirken hata: " + hata.Message);
            }
        }

        
        string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch { return "0.0.0.0"; }
        }

        // Loglama Fonksiyonu
        void LogEkle(string islemTuru, string aciklama)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "insert into log(islem, ip, tc, aciklama, tarih) values(@islem, @ip, @tc, @aciklama, @tarih)";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@islem", islemTuru);
                    komut.Parameters.AddWithValue("@ip", GetLocalIPAddress());
                    komut.Parameters.AddWithValue("@tc", Form1.giris); // Giriş yapan yönetici
                    komut.Parameters.AddWithValue("@aciklama", aciklama);
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.ExecuteNonQuery();
                }
            }
            catch { /* Log hatası programı durdurmasın */ }
        }

        private void gelir_Tanimlari_Load(object sender, EventArgs e)
        {
            GelirListele();
            KullaniciDoldur();
        }

        // --- İŞLEMLER ---

        // GELİR EKLEME (Button4)
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "insert into gelirler(kategori, tutar, tarih, aciklama) values(@kat, @tutar, @tarih, @acik)";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@kat", textBox2.Text); // Kategori
                    komut.Parameters.AddWithValue("@tutar", decimal.Parse(textBox3.Text)); // Tutar
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.Parameters.AddWithValue("@acik", "Gelir Ekleme: " + comboBox1.Text); // Açıklama olarak TC'yi ekledim

                    komut.ExecuteNonQuery();

                    LogEkle("Gelir Ekleme", textBox3.Text + " TL tutarında gelir eklendi.");
                }
                MessageBox.Show("Gelir eklendi.");
                GelirListele();
                Temizle();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Ekleme hatası: " + hata.Message);
            }
        }

        // GÜNCELLEME 
        private void gnclle_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView5.CurrentRow != null)
                {
                    using (SqlConnection baglanti = baglan.baglan())
                    {
                        string id = dataGridView5.CurrentRow.Cells[0].Value.ToString();

                        string sorgu = "update gelirler set kategori=@kat, tutar=@tutar, aciklama=@acik, tarih=@tarih where id=@id";
                        SqlCommand komut = new SqlCommand(sorgu, baglanti);
                        komut.Parameters.AddWithValue("@kat", comboBox2.Text);
                        komut.Parameters.AddWithValue("@tutar", decimal.Parse(textBox5.Text));
                        komut.Parameters.AddWithValue("@acik", textBox4.Text);
                        komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                        komut.Parameters.AddWithValue("@id", id);

                        komut.ExecuteNonQuery();

                        LogEkle("Gelir Güncelleme", id + " nolu gelir kaydı güncellendi.");
                    }
                    MessageBox.Show("Gelir güncellendi.");
                    GelirListele();
                    Temizle();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Güncelleme hatası: " + hata.Message);
            }
        }

        // SİLME (Button5)
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView5.CurrentRow != null)
                {
                    string id = dataGridView5.CurrentRow.Cells[0].Value.ToString();
                    DialogResult cevap = MessageBox.Show("Bu gelir kaydını silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cevap == DialogResult.Yes)
                    {
                        using (SqlConnection baglanti = baglan.baglan())
                        {
                            SqlCommand komut = new SqlCommand("delete from gelirler where id=@id", baglanti);
                            komut.Parameters.AddWithValue("@id", id);
                            komut.ExecuteNonQuery();

                            LogEkle("Gelir Silme", id + " nolu gelir kaydı silindi.");
                        }
                        MessageBox.Show("Kayıt silindi.");
                        GelirListele();
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Silme hatası: " + hata.Message);
            }
        }

        // TABLODAN SEÇİNCE DOLDURMA 
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView5.CurrentRow != null)
                {
                   
                    comboBox2.Text = dataGridView5.CurrentRow.Cells[1].Value.ToString(); // Kategori
                    textBox5.Text = dataGridView5.CurrentRow.Cells[2].Value.ToString(); // Tutar
                    textBox4.Text = dataGridView5.CurrentRow.Cells[3].Value.ToString(); // Açıklama

                    
                    tabControl1.SelectedTab = tabPage3;
                }
            }
            catch { }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}