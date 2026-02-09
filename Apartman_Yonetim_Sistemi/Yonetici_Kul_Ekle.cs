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
    public partial class Yonetici_Kul_Ekle : Form
    {
        public Yonetici_Kul_Ekle()
        {
            InitializeComponent();
        }

        sqlbaglantisi bag = new sqlbaglantisi();
        string secili_apartman_id = "0";

        private void Yonetici_Kul_Ekle_Load(object sender, EventArgs e)
        {
            doldurApartman();
        }

        public void doldurApartman()
        {
            try
            {
                

                using (SqlConnection baglanti = bag.baglan())
                {
                    string sorgu = "Select * from apartman_islemleri";
                    

                    SqlCommand com = new SqlCommand(sorgu, baglanti);
                    SqlDataReader oku = com.ExecuteReader();

                    combo_apartman_adi.Items.Clear();
                    while (oku.Read())
                    {
                        combo_apartman_adi.Items.Add(oku["apartman_adi"].ToString());
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Apartman Listeleme Hatası: " + hata.Message);
            }
        }

        public void doldurDaire()
        {
            try
            {
                combo_daire_no.Items.Clear();

                using (SqlConnection baglanti = bag.baglan())
                {
                    
                    SqlCommand com = new SqlCommand("Select id from apartman_islemleri where apartman_adi=@adi", baglanti);
                    com.Parameters.AddWithValue("@adi", combo_apartman_adi.Text);

                    object sonuc = com.ExecuteScalar();
                    if (sonuc != null)
                    {
                        secili_apartman_id = sonuc.ToString();

                        
                        for (int i = 1; i <= 50; i++)
                        {
                            combo_daire_no.Items.Add(i.ToString());
                        }
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Daire Listeleme Hatası: " + hata.Message);
            }
        }

        private void combo_apartman_adi_SelectedIndexChanged(object sender, EventArgs e)
        {
            doldurDaire();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox29.Text == textBox30.Text) // Şifreler uyuşuyor mu?
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
                        komut.Parameters.AddWithValue("@sifre", textBox30.Text); // Düz metin (Şifreleme yok)
                        komut.Parameters.AddWithValue("@aptId", secili_apartman_id);

                        komut.ExecuteNonQuery();

                        
                        if (comboBox7.Text == "Apartman Yöneticisi" || comboBox7.Text == "Admin")
                        {
                            
                            string yetkiSorgu = "insert into yetki(tc,gelir_isleri,gider_isleri,kasa_isleri,borc_isleri,daire_isleri,kullanici_isleri) values(@tc,'0','0','0','0','0','0')";
                            SqlCommand kmtYetki = new SqlCommand(yetkiSorgu, baglanti);
                            kmtYetki.Parameters.AddWithValue("@tc", maskedTextBox2.Text);
                            kmtYetki.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Kullanıcı Başarıyla Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    textBox25.Clear(); textBox26.Clear(); maskedTextBox2.Clear();
                    textBox27.Clear(); maskedTextBox3.Clear(); textBox29.Clear(); textBox30.Clear();
                }
                else
                {
                    MessageBox.Show("Şifreler Eşleşmiyor !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Ekleme Hatası: " + hata.Message);
            }
        }

        
        private void combo_daire_no_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}