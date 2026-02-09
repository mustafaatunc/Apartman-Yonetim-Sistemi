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
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        // Yetki Değişkenleri 
        string yetki_kullanici = "0";
        string yetki_gider = "0";
        string yetki_gelir = "0";
        string yetki_kasa = "0";
        string yetki_borc = "0";
        string yetki_daire = "0";

        private void menu_Load(object sender, EventArgs e)
        {
            YetkileriGetir();
        }

        void YetkileriGetir()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    
                    SqlCommand komut = new SqlCommand("Select * from yetki where tc=@tc", baglanti);
                    komut.Parameters.AddWithValue("@tc", Form1.giris);

                    SqlDataReader oku = komut.ExecuteReader();
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
            }
            catch (Exception hata)
            {
                MessageBox.Show("Yetkiler alınırken hata oluştu: " + hata.Message);
            }
        }

        // --- MENÜ BUTONLARI ---

        // 1. YÖNETİCİ İŞLEMLERİ (Kullanıcı Ekle/Sil)
        private void yöneticiİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_kullanici == "1")
            {
                Apartman_Yonetici_Islemleri frm = new Apartman_Yonetici_Islemleri();
                frm.MdiParent = this; // Bu formun içinde açıl
                frm.Show();
            }
            else
            {
                MessageBox.Show("Bu alana giriş yetkiniz yok! (Kullanıcı İşleri Yetkisi Gerekli)", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 2. APARTMAN İŞLEMLERİ (Daire Ekle/Sil)
        private void apartmanİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (yetki_daire == "1")
            {
                Apartman_Islemleri frm = new Apartman_Islemleri();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Bu alana giriş yetkiniz yok! (Daire İşleri Yetkisi Gerekli)", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 3. AYARLAR (Kategori/Borç Tipi Ekleme)
        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (yetki_borc == "1")
            {
                kategori_islemleri frm = new kategori_islemleri();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Bu alana giriş yetkiniz yok! (Borç İşleri Yetkisi Gerekli)", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 4. İSTATİSTİKLER (Ana Sayfa Özeti)
        private void istatistiklerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Apart_Yonetim_Anasayfa frm = new Apart_Yonetim_Anasayfa();
            frm.MdiParent = this;
            frm.Show();
        }

        // 5. LOGLAR (Güvenlik Kayıtları)
        private void loglarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (yetki_kullanici == "1")
            {
                Loglar frm = new Loglar();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Log kayıtlarını görüntüleme yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}