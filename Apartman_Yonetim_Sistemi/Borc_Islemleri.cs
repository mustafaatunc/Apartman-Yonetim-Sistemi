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
    public partial class Borc_Islemleri : Form
    {
        public Borc_Islemleri()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();
        string bilgisayarAdi = Dns.GetHostName();

        // --- YARDIMCI METOTLAR ---

        void Temizle()
        {
            
            comboBox10.SelectedIndex = -1; // Borç Tipi
            comboBox11.SelectedIndex = -1; // Kişi TC
            textBox39.Clear(); // Tutar
            textBox40.Clear(); // Açıklama

            
            comboBox13.SelectedIndex = -1;
            comboBox12.SelectedIndex = -1;
            textBox43.Clear();
            textBox42.Clear();
        }

        void BorclariListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM borclar", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView11.DataSource = dt;

                    // Tablo Başlıkları
                    if (dataGridView11.Columns.Count > 5)
                    {
                        dataGridView11.Columns[0].Visible = false; // ID
                        dataGridView11.Columns[1].HeaderText = "Kategori";
                        dataGridView11.Columns[2].HeaderText = "TC Kimlik No";
                        dataGridView11.Columns[3].HeaderText = "Tutar";
                        dataGridView11.Columns[4].HeaderText = "Açıklama";
                        dataGridView11.Columns[5].HeaderText = "Tarih";
                        dataGridView11.Columns[6].Visible = false; // Ekstra kolon varsa gizle
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Listeleme hatası: " + hata.Message);
            }
        }

        void KombolariDoldur()
        {
            try
            {
                comboBox10.Items.Clear(); // Borç Tipi Ekleme
                comboBox13.Items.Clear(); // Borç Tipi Güncelleme
                comboBox11.Items.Clear(); // Kişi TC Ekleme
                comboBox12.Items.Clear(); // Kişi TC Güncelleme

                using (SqlConnection baglanti = baglan.baglan())
                {
                   
                    try
                    {
                        SqlCommand komutTip = new SqlCommand("select * from borc_tipi", baglanti);
                        SqlDataReader drTip = komutTip.ExecuteReader();
                        while (drTip.Read())
                        {
                            string tip = drTip["tip"].ToString(); // Sütun adı 'tip' varsayıyorum
                            comboBox10.Items.Add(tip);
                            comboBox13.Items.Add(tip);
                        }
                        drTip.Close();
                    }
                    catch
                    {
                        
                        string[] tipler = { "Aidat", "Yakıt", "Demirbaş", "Ortak Gider", "Diğer" };
                        comboBox10.Items.AddRange(tipler);
                        comboBox13.Items.AddRange(tipler);
                    }

                    
                    SqlCommand komutKul = new SqlCommand("select tc_no from kullanici", baglanti);
                    SqlDataReader drKul = komutKul.ExecuteReader();
                    while (drKul.Read())
                    {
                        string tc = drKul["tc_no"].ToString();
                        comboBox11.Items.Add(tc);
                        comboBox12.Items.Add(tc);
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Veri doldurma hatası: " + hata.Message);
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

        private void Borc_Islemleri_Load(object sender, EventArgs e)
        {
            BorclariListele();
            KombolariDoldur();
        }

        // --- BUTON İŞLEMLERİ ---

        
        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "insert into borclar(kategori, kullanici, tutar, aciklama, tarih) values(@kat, @kul, @tutar, @acik, @tarih)";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@kat", comboBox10.Text);
                    komut.Parameters.AddWithValue("@kul", comboBox11.Text);
                    komut.Parameters.AddWithValue("@tutar", decimal.Parse(textBox39.Text)); // Decimal daha güvenli
                    komut.Parameters.AddWithValue("@acik", textBox40.Text);
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.ExecuteNonQuery();

                    LogEkle("Borç Ekleme", comboBox11.Text + " kişisine " + textBox39.Text + " TL borç eklendi.");
                }
                MessageBox.Show("Borç başarıyla eklendi.");
                BorclariListele();
                Temizle();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Ekleme hatası: " + hata.Message);
            }
        }

        
        private void dataGridView11_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView11.CurrentRow != null)
                {
                    
                    comboBox13.Text = dataGridView11.CurrentRow.Cells[1].Value.ToString(); // Kategori
                    comboBox12.Text = dataGridView11.CurrentRow.Cells[2].Value.ToString(); // TC
                    textBox43.Text = dataGridView11.CurrentRow.Cells[3].Value.ToString(); // Tutar
                    textBox42.Text = dataGridView11.CurrentRow.Cells[4].Value.ToString(); // Açıklama

                    
                    tabControl1.SelectedTab = tabPage3;
                }
            }
            catch { }
        }

        // BORÇ GÜNCELLEME (Button25)
        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    
                    string id = dataGridView11.CurrentRow.Cells[0].Value.ToString();

                    string sorgu = "update borclar set kategori=@kat, kullanici=@kul, tutar=@tutar, aciklama=@acik where id=@id";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@kat", comboBox13.Text);
                    komut.Parameters.AddWithValue("@kul", comboBox12.Text);
                    komut.Parameters.AddWithValue("@tutar", decimal.Parse(textBox43.Text));
                    komut.Parameters.AddWithValue("@acik", textBox42.Text);
                    komut.Parameters.AddWithValue("@id", id);
                    komut.ExecuteNonQuery();

                    LogEkle("Borç Güncelleme", id + " nolu borç kaydı güncellendi.");
                }
                MessageBox.Show("Güncelleme başarılı.");
                BorclariListele();
                Temizle();
                tabControl1.SelectedTab = tabPage2; 
            }
            catch (Exception hata)
            {
                MessageBox.Show("Güncelleme hatası: " + hata.Message);
            }
        }

        // BORÇ SİLME (Button24 - Tahmini)
        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView11.CurrentRow != null)
                {
                    string id = dataGridView11.CurrentRow.Cells[0].Value.ToString();
                    DialogResult cevap = MessageBox.Show("Bu borç kaydını silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cevap == DialogResult.Yes)
                    {
                        using (SqlConnection baglanti = baglan.baglan())
                        {
                            SqlCommand komut = new SqlCommand("delete from borclar where id=@id", baglanti);
                            komut.Parameters.AddWithValue("@id", id);
                            komut.ExecuteNonQuery();

                            LogEkle("Borç Silme", id + " nolu borç kaydı silindi.");
                        }
                        MessageBox.Show("Kayıt silindi.");
                        BorclariListele();
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Silme hatası: " + hata.Message);
            }
        }

        
        private void button23_Click(object sender, EventArgs e) { } 
                                                                    
        private void textBox41_TextChanged(object sender, EventArgs e) { } 
    }
}