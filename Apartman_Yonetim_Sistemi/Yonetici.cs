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
    public partial class Yonetici : Form
    {
        public Yonetici()
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

        private void Yonetici_Load(object sender, EventArgs e)
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
                MessageBox.Show("Yetkiler alınırken hata: " + hata.Message);
            }
        }

        // --- MENÜ İŞLEMLERİ ---

        // GELİR TANIMLARI
        private void gelirTanımlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_gelir == "1")
            {
                gelir_Tanimlari frm = new gelir_Tanimlari();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Gelir işlemleri için yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // GİDER TANIMLARI
        private void giderTanımlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_gider == "1")
            {
                gider_Tanimlari frm = new gider_Tanimlari();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Gider işlemleri için yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // KASA TANIMLARI
        private void kasaTanımlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_kasa == "1")
            {
                Kasa_Tanimlari frm = new Kasa_Tanimlari();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Kasa işlemleri için yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // DAİRE İŞLEMLERİ
        private void daireİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_daire == "1")
            {
                daire_islemleri frm = new daire_islemleri();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Daire işlemleri için yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // BORÇ İŞLEMLERİ (Yönetim)
        private void borçİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_borc == "1")
            {
                Borc_Islemleri frm = new Borc_Islemleri();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Borç tanımlama işlemleri için yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // BORÇLARIM (Kişisel)
        private void borçlarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Apart_Yonetici_Borclari frm = new Apart_Yonetici_Borclari();
            frm.MdiParent = this;
            frm.Show();
        }

        // APARTMAN SAKİNİ EKLE
        private void apartmanSakiniEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yetki_kullanici == "1")
            {
                Apartman_Yonetici_Islemleri frm = new Apartman_Yonetici_Islemleri();
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı işlemleri için yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}