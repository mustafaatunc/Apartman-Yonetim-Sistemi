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
    public partial class kategori_islemleri : Form
    {
        public kategori_islemleri()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        // --- YARDIMCI METOTLAR ---

        void Temizle()
        {
            textBox44.Clear(); // Kategori Adı
            textBox45.Clear(); // Açıklama
            textBox46.Clear(); // Arama Kutusu
        }

        void KategoriListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM borc_tipi", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView12.DataSource = dt;

                    // Tablo Tasarımı
                    if (dataGridView12.Columns.Count > 2)
                    {
                        dataGridView12.Columns[0].Visible = false; // ID
                        dataGridView12.Columns[1].HeaderText = "Borç Tipi";
                        dataGridView12.Columns[2].HeaderText = "Borç Açıklaması";

                        dataGridView12.Columns[1].Width = 150;
                        dataGridView12.Columns[2].Width = 200;
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
                    komut.Parameters.AddWithValue("@tc", Form1.giris);
                    komut.Parameters.AddWithValue("@aciklama", aciklama);
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void kategori_islemleri_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        // --- İŞLEMLER ---

        // KATEGORİ EKLEME (Button26)
        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "insert into borc_tipi(tip, aciklama) values(@tip, @acik)";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    komut.Parameters.AddWithValue("@tip", textBox44.Text);
                    komut.Parameters.AddWithValue("@acik", textBox45.Text);

                    komut.ExecuteNonQuery();

                    LogEkle("Kategori Ekleme", textBox44.Text + " kategorisi eklendi.");
                }
                MessageBox.Show("Borç tipi başarıyla eklendi.");
                KategoriListele();
                Temizle();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Ekleme hatası: " + hata.Message);
            }
        }

        // KATEGORİ SİLME (Button28)
        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView12.CurrentRow != null)
                {
                    string id = dataGridView12.CurrentRow.Cells[0].Value.ToString();
                    string tipAdi = dataGridView12.CurrentRow.Cells[1].Value.ToString();

                    DialogResult cevap = MessageBox.Show(tipAdi + " kategorisini silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cevap == DialogResult.Yes)
                    {
                        using (SqlConnection baglanti = baglan.baglan())
                        {
                            SqlCommand komut = new SqlCommand("delete from borc_tipi where id=@id", baglanti);
                            komut.Parameters.AddWithValue("@id", id);
                            komut.ExecuteNonQuery();

                            LogEkle("Kategori Silme", tipAdi + " kategorisi silindi.");
                        }
                        MessageBox.Show("Silme işlemi başarılı.");
                        KategoriListele();
                        Temizle();
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Silme hatası: " + hata.Message);
            }
        }

        // ARAMA İŞLEMİ
        private void textBox46_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "SELECT * FROM borc_tipi WHERE tip LIKE @ara OR aciklama LIKE @ara";
                    SqlDataAdapter ad = new SqlDataAdapter(sorgu, baglanti);
                    ad.SelectCommand.Parameters.AddWithValue("@ara", "%" + textBox46.Text + "%");

                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView12.DataSource = dt;
                }
            }
            catch { }
        }

        

        private void button26_Click_1(object sender, EventArgs e) { button26_Click(sender, e); }
        private void button28_Click_1(object sender, EventArgs e) { button28_Click(sender, e); }
        private void dataGridView12_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView12_CellClick(object sender, DataGridViewCellEventArgs e) { }
    }
}