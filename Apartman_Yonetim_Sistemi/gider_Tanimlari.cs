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
using System.Net; // IP işlemleri için

namespace Apartman_Yonetim_Sistemi
{
    public partial class gider_Tanimlari : Form
    {
        public gider_Tanimlari()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        // --- YARDIMCI METOTLAR ---

        void Temizle()
        {
            // Ekleme Bölümü
            textBox6.Clear(); // Açıklama
            textBox7.Clear(); // Tutar
            comboBox3.SelectedIndex = -1; // Kategori

            // Güncelleme Bölümü
            textBox8.Clear(); // Açıklama
            textBox9.Clear(); // Tutar
            comboBox4.SelectedIndex = -1; // Kategori
        }

        void GiderListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM giderler", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView6.DataSource = dt;

                    // Tablo Tasarımı
                    if (dataGridView6.Columns.Count > 4)
                    {
                        dataGridView6.Columns[0].Visible = false; // ID
                        dataGridView6.Columns[1].HeaderText = "Gider Kategorisi";
                        dataGridView6.Columns[2].HeaderText = "Gider Tutarı";
                        dataGridView6.Columns[3].HeaderText = "Açıklama";
                        dataGridView6.Columns[4].HeaderText = "Tarih";
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Listeleme hatası: " + hata.Message);
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

        private void gider_Tanimlari_Load(object sender, EventArgs e)
        {
            GiderListele();
           
        }

        // --- İŞLEMLER ---

        // GİDER EKLEME (Button6)
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "insert into giderler(kategori, tutar, aciklama, tarih) values(@kat, @tutar, @acik, @tarih)";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    komut.Parameters.AddWithValue("@kat", comboBox3.Text);
                    komut.Parameters.AddWithValue("@tutar", decimal.Parse(textBox7.Text)); // Decimal kullanımı daha güvenli
                    komut.Parameters.AddWithValue("@acik", textBox6.Text);
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);

                    komut.ExecuteNonQuery();

                    LogEkle("Gider Ekleme", textBox7.Text + " TL tutarında gider eklendi (" + comboBox3.Text + ")");
                }
                MessageBox.Show("Gider eklendi.");
                GiderListele();
                Temizle();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Ekleme hatası: " + hata.Message);
            }
        }

        // GİDER SİLME (Button7)
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView6.CurrentRow != null)
                {
                    string id = dataGridView6.CurrentRow.Cells[0].Value.ToString();
                    DialogResult cevap = MessageBox.Show("Bu gider kaydını silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cevap == DialogResult.Yes)
                    {
                        using (SqlConnection baglanti = baglan.baglan())
                        {
                            SqlCommand komut = new SqlCommand("delete from giderler where id=@id", baglanti);
                            komut.Parameters.AddWithValue("@id", id);
                            komut.ExecuteNonQuery();

                            LogEkle("Gider Silme", id + " nolu gider kaydı silindi.");
                        }
                        MessageBox.Show("Kayıt silindi.");
                        GiderListele();
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Silme hatası: " + hata.Message);
            }
        }

        // GİDER GÜNCELLEME (Button8)
        private void button8_Click_1(object sender, EventArgs e) // Designer'da button8_Click_1 olarak kayıtlı olabilir
        {
            try
            {
                if (dataGridView6.CurrentRow != null)
                {
                    using (SqlConnection baglanti = baglan.baglan())
                    {
                        string id = dataGridView6.CurrentRow.Cells[0].Value.ToString();

                        string sorgu = "update giderler set kategori=@kat, tutar=@tutar, aciklama=@acik, tarih=@tarih where id=@id";
                        SqlCommand komut = new SqlCommand(sorgu, baglanti);

                        komut.Parameters.AddWithValue("@kat", comboBox4.Text);
                        komut.Parameters.AddWithValue("@tutar", decimal.Parse(textBox9.Text));
                        komut.Parameters.AddWithValue("@acik", textBox8.Text);
                        komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                        komut.Parameters.AddWithValue("@id", id);

                        komut.ExecuteNonQuery();

                        LogEkle("Gider Güncelleme", id + " nolu gider kaydı güncellendi.");
                    }
                    MessageBox.Show("Gider güncellendi.");
                    GiderListele();
                    Temizle();
                    
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Güncelleme hatası: " + hata.Message);
            }
        }

        // TABLODAN SEÇİNCE DOLDURMA 
        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView6.CurrentRow != null)
                {
                    
                    comboBox4.Text = dataGridView6.CurrentRow.Cells[1].Value.ToString(); // Kategori
                    textBox9.Text = dataGridView6.CurrentRow.Cells[2].Value.ToString();  // Tutar
                    textBox8.Text = dataGridView6.CurrentRow.Cells[3].Value.ToString();  // Açıklama

                    
                    tabControl1.SelectedTab = tabPage3;
                }
            }
            catch { }
        }

        // TEMİZLEME BUTONU (Button3)
        private void button3_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        
        private void button8_Click(object sender, EventArgs e) { button8_Click_1(sender, e); }
        
        private void button7_Click_1(object sender, EventArgs e)
        {
            button7_Click(sender, e); 
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            button3_Click(sender, e); 
        }
    }

}