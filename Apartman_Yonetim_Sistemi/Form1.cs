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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        public static string giris = "";
        public static string sifre = "";
        public static string yetki = "";
        public static string apartman_id = "0";

        // Yetki Kontrol Değişkenleri
        public static string yetki_kullanici = "0";
        public static string yetki_gider = "0";
        public static string yetki_gelir = "0";
        public static string yetki_kasa = "0";
        public static string yetki_borc = "0";
        public static string yetki_daire = "0";

        sqlbaglantisi baglan = new sqlbaglantisi();

        private void Form1_Load(object sender, EventArgs e)
        {
            
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        
        private void KullaniciGirisi(string tc, string girilenSifre, string beklenenYetki)
        {
            try
            {
                // 1. ADIM: Güvenli SQL Sorgusu 
                SqlCommand com = new SqlCommand("Select * from kullanici where tc_no=@p1 and sifre=@p2", baglan.baglan());
                com.Parameters.AddWithValue("@p1", tc);
                com.Parameters.AddWithValue("@p2", girilenSifre);

                SqlDataReader oku = com.ExecuteReader();

                if (oku.Read())
                {
                    
                    giris = tc;
                    sifre = girilenSifre;
                    yetki = oku["rol"].ToString();
                    apartman_id = oku["apartman_id"].ToString();

                    // 2. ADIM: Yetki Kontrolü 
                    if (yetki == beklenenYetki)
                    {
                        // 3. ADIM: Detaylı Yetkileri Çek 
                        YetkileriGetir(tc);

                        // 4. ADIM: İlgili Menüyü Aç
                        if (yetki == "Admin")
                        {
                            menu menuForm = new menu();
                            menuForm.Show();
                        }
                        else if (yetki == "Apartman Yöneticisi")
                        {
                            Yonetici yoneticiForm = new Yonetici();
                            yoneticiForm.Show();
                        }
                        else if (yetki == "Sakin")
                        {
                            Sakin sakinForm = new Sakin();
                            sakinForm.Show();
                        }

                        this.Hide(); 
                    }
                    else
                    {
                        MessageBox.Show("Hatalı Giriş! Bu alandan sadece " + beklenenYetki + " girişi yapılabilir.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir hata oluştu: " + hata.Message);
            }
        }

        
        private void YetkileriGetir(string tc)
        {
            try
            {
                SqlCommand comt = new SqlCommand("Select * from yetki where tc=@p1", baglan.baglan());
                comt.Parameters.AddWithValue("@p1", tc);
                SqlDataReader oku = comt.ExecuteReader();
                if (oku.Read())
                {
                    yetki_kullanici = oku["kullanici_isleri"].ToString();
                    yetki_gider = oku["gider_isleri"].ToString();
                    yetki_gelir = oku["gelir_isleri"].ToString();
                    yetki_kasa = oku["kasa_isleri"].ToString();
                    yetki_borc = oku["borc_isleri"].ToString();
                    yetki_daire = oku["daire_isleri"].ToString();
                }
            }
            catch
            {
                
            }
        }

        // --- BUTON TIKLAMALARI 

        // Admin Giriş Butonu
        private void button4_Click(object sender, EventArgs e)
        {
            KullaniciGirisi(textBox1.Text, textBox2.Text, "Admin");
        }

        // Apartman Yöneticisi Giriş Butonu
        private void button5_Click(object sender, EventArgs e)
        {
            KullaniciGirisi(textBox4.Text, textBox3.Text, "Apartman Yöneticisi");
        }

        // Sakin Giriş Butonu
        private void button6_Click(object sender, EventArgs e)
        {
            KullaniciGirisi(textBox6.Text, textBox5.Text, "Sakin");
        }

        // --- PANEL GEÇİŞLERİ ---
        private void button1_Click(object sender, EventArgs e) // Admin Paneli Aç
        {
            PanelleriAyarla(true, false, false);
        }

        private void button3_Click(object sender, EventArgs e) // Yönetici Paneli Aç
        {
            PanelleriAyarla(false, true, false);
        }

        private void button2_Click(object sender, EventArgs e) // Sakin Paneli Aç
        {
            PanelleriAyarla(false, false, true);
        }

        
        private void PanelleriAyarla(bool p1, bool p2, bool p3)
        {
            panel1.Visible = p1;
            panel2.Visible = p2;
            panel3.Visible = p3;
        }
    }
}