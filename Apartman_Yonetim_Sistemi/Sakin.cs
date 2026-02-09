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

namespace Apartman_Yonetim_Sistemi
{
    public partial class Sakin : Form
    {
        public Sakin()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        // --- VERİ ÇEKME FONKSİYONLARI ---

        void ToplamBorcGetir()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    
                    SqlCommand komut = new SqlCommand("Select SUM(tutar) From borclar where kullanici=@tc", baglanti);
                    komut.Parameters.AddWithValue("@tc", Form1.giris);

                    object sonuc = komut.ExecuteScalar();

                    if (sonuc != null && sonuc != DBNull.Value)
                    {
                        decimal tutar = Convert.ToDecimal(sonuc);
                        label1.Text = tutar.ToString("N2") + " TL"; // Para formatı (1.250,00 TL)
                    }
                    else
                    {
                        label1.Text = "0,00 TL";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Borç hesaplanırken hata: " + ex.Message);
            }
        }

        void ToplamOdenenGetir()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlCommand komut = new SqlCommand("Select SUM(miktar) From odenen where odeyen=@tc", baglanti);
                    komut.Parameters.AddWithValue("@tc", Form1.giris);

                    object sonuc = komut.ExecuteScalar();

                    if (sonuc != null && sonuc != DBNull.Value)
                    {
                        decimal miktar = Convert.ToDecimal(sonuc);
                        label2.Text = miktar.ToString("N2") + " TL";
                    }
                    else
                    {
                        label2.Text = "0,00 TL";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ödenen hesaplanırken hata: " + ex.Message);
            }
        }

        void BorclariListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    
                    SqlDataAdapter ad = new SqlDataAdapter("Select * From borclar where kullanici=@tc", baglanti);
                    ad.SelectCommand.Parameters.AddWithValue("@tc", Form1.giris);

                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView4.DataSource = dt;

                    // Tablo Tasarımı
                    if (dataGridView4.Columns.Count > 5)
                    {
                        dataGridView4.Columns[0].Visible = false; // ID
                        dataGridView4.Columns[6].Visible = false; // Kullanici TC 

                        dataGridView4.Columns[1].HeaderText = "Kategori";
                        dataGridView4.Columns[2].HeaderText = "TC Kimlik No";
                        dataGridView4.Columns[3].HeaderText = "Tutar";
                        dataGridView4.Columns[4].HeaderText = "Açıklama";
                        dataGridView4.Columns[5].HeaderText = "Tarih";
                    }
                }
            }
            catch (Exception) { }
        }

        void OdemeleriListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("Select * From odenen where odeyen=@tc", baglanti);
                    ad.SelectCommand.Parameters.AddWithValue("@tc", Form1.giris);

                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView3.DataSource = dt;

                    if (dataGridView3.Columns.Count > 4)
                    {
                        dataGridView3.Columns[0].Visible = false;
                        dataGridView3.Columns[1].HeaderText = "Ödeyen TC";
                        dataGridView3.Columns[2].HeaderText = "Miktar";
                        dataGridView3.Columns[3].HeaderText = "Tür";
                    }
                }
            }
            catch (Exception) { }
        }

        private void Sakin_Load(object sender, EventArgs e)
        {
            
            OdemeleriListele();
            ToplamOdenenGetir();
            ToplamBorcGetir();
            BorclariListele();

            
            BilgileriGetir();
        }

        void BilgileriGetir()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlCommand komut = new SqlCommand("Select * from kullanici where tc_no=@tc", baglanti);
                    komut.Parameters.AddWithValue("@tc", Form1.giris);
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        
                        maskedTextBox1.Text = dr["tc_no"].ToString();
                        textBox10.Text = dr["ad"].ToString();
                        textBox11.Text = dr["soyisim"].ToString();
                        textBox12.Text = dr["email"].ToString();
                        textBox13.Text = dr["telefon"].ToString();
                        textBox14.Text = dr["daire_no"].ToString();
                        comboBox5.Text = dr["ev_durumu"].ToString();
                    }
                }
            }
            catch { }
        }

        // --- ARAMA İŞLEMİ ---
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    
                    string sorgu = "SELECT * FROM borclar WHERE kullanici=@tc AND (kategori LIKE @ara OR aciklama LIKE @ara)";
                    SqlDataAdapter ad = new SqlDataAdapter(sorgu, baglanti);
                    ad.SelectCommand.Parameters.AddWithValue("@tc", Form1.giris);
                    ad.SelectCommand.Parameters.AddWithValue("@ara", "%" + textBox1.Text + "%");

                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch { }
        }

        // --- YAZDIRMA İŞLEMİ ---
        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                Font baslikFont = new Font("Calibri", 10, FontStyle.Bold);
                Font icerikFont = new Font("Calibri", 8);
                SolidBrush firca = new SolidBrush(Color.Black);
                Pen kalem = new Pen(Color.Black);

                // Başlık
                e.Graphics.DrawString("BORÇ LİSTESİ DÖKÜMÜ", baslikFont, firca, 300, 25);
                e.Graphics.DrawString("Tarih: " + DateTime.Now.ToString(), icerikFont, firca, 50, 50);
                e.Graphics.DrawLine(kalem, 50, 70, 750, 70);

                // Tablo Başlıkları
                int y = 90;
                e.Graphics.DrawString("Kategori", baslikFont, firca, 50, y);
                e.Graphics.DrawString("Tutar", baslikFont, firca, 200, y);
                e.Graphics.DrawString("Açıklama", baslikFont, firca, 300, y);
                e.Graphics.DrawString("Tarih", baslikFont, firca, 600, y);

                y += 30;
                e.Graphics.DrawLine(kalem, 50, y - 10, 750, y - 10);

                // Satırlar
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    
                    if (dataGridView4.Rows[i].Cells[0].Value == null) continue;

                    string kategori = dataGridView4.Rows[i].Cells[1].Value.ToString();
                    string tutar = dataGridView4.Rows[i].Cells[3].Value.ToString();
                    string aciklama = dataGridView4.Rows[i].Cells[4].Value.ToString();
                    string tarih = dataGridView4.Rows[i].Cells[5].Value.ToString();

                    e.Graphics.DrawString(kategori, icerikFont, firca, 50, y);
                    e.Graphics.DrawString(tutar + " TL", icerikFont, firca, 200, y);
                    e.Graphics.DrawString(aciklama, icerikFont, firca, 300, y);
                    e.Graphics.DrawString(tarih, icerikFont, firca, 600, y);

                    y += 25;
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        // --- PROFİL GÜNCELLEME ---
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "update kullanici set ad=@ad, soyisim=@soy, email=@mail, telefon=@tel, ev_durumu=@drm where tc_no=@tc";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    komut.Parameters.AddWithValue("@ad", textBox10.Text);
                    komut.Parameters.AddWithValue("@soy", textBox11.Text);
                    komut.Parameters.AddWithValue("@mail", textBox12.Text);
                    komut.Parameters.AddWithValue("@tel", textBox13.Text);
                    komut.Parameters.AddWithValue("@drm", comboBox5.Text);
                    komut.Parameters.AddWithValue("@tc", Form1.giris); // Giriş yapan kullanıcının TC'si değişmez

                    komut.ExecuteNonQuery();
                    MessageBox.Show("Bilgileriniz başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message);
            }
        }

        // --- ŞİFRE DEĞİŞTİRME ---
        private void button10_Click(object sender, EventArgs e)
        {
            
            if (textBox18.Text == Form1.sifre) 
            {
                if (textBox17.Text == textBox16.Text) 
                {
                    try
                    {
                        using (SqlConnection baglanti = baglan.baglan())
                        {
                            // Şifresiz (Düz metin) kayıt
                            SqlCommand komut = new SqlCommand("update kullanici set sifre=@yeniSifre where tc_no=@tc", baglanti);
                            komut.Parameters.AddWithValue("@yeniSifre", textBox17.Text);
                            komut.Parameters.AddWithValue("@tc", Form1.giris);
                            komut.ExecuteNonQuery();

                            MessageBox.Show("Şifreniz başarıyla değiştirildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            
                            Form1.sifre = textBox17.Text;

                            
                            textBox18.Clear();
                            textBox17.Clear();
                            textBox16.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Yeni şifreler birbiriyle uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Mevcut şifrenizi yanlış girdiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- PANEL GEÇİŞLERİ ---
        
        private void chromeButton7_Click(object sender, EventArgs e)
        {
            panel20.Hide(); // Profil panelini gizle
            panel8.Hide();  // Şifre panelini gizle
        }

        private void chromeButton6_Click(object sender, EventArgs e)
        {
            panel20.Hide();
            panel8.Show(); 
        }

        
        private void textBox1_TextChanged_1(object sender, EventArgs e) { textBox1_TextChanged(sender, e); }
        private void button1_Click_1(object sender, EventArgs e) { button1_Click(sender, e); }
        private void button9_Click_1(object sender, EventArgs e) { button9_Click(sender, e); }
        private void button10_Click_1(object sender, EventArgs e) { button10_Click(sender, e); }
    }
}