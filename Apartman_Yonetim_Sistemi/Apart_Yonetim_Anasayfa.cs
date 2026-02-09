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
    public partial class Apart_Yonetim_Anasayfa : Form
    {
        public Apart_Yonetim_Anasayfa()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        
        private string ToplamGetir(string sorgu)
        {
            try
            {
                
                SqlCommand komut = new SqlCommand(sorgu, baglan.baglan());
                object sonuc = komut.ExecuteScalar();

                if (sonuc != null && sonuc != DBNull.Value)
                {
                    
                    double miktar = Convert.ToDouble(sonuc);
                    return miktar.ToString("N2"); 
                }
                return "0,00";
            }
            catch (Exception)
            {
                return "0,00";
            }
        }

        private void VerileriYukle()
        {
            try
            {
                // 1. TOPLAM BORÇ (SUM)
                string toplamBorc = ToplamGetir("Select SUM(tutar) From borclar");
                label1.Text = toplamBorc + " TL";

                // 2. TOPLAM ÖDENEN (SUM)
                string toplamOdenen = ToplamGetir("Select SUM(miktar) From odenen");
                label2.Text = toplamOdenen + " TL";

                // 3. KULLANICI SAYISI (COUNT)
               
                SqlCommand komutSayi = new SqlCommand("Select Count(*) From kullanici", baglan.baglan());
                object kisiSayisi = komutSayi.ExecuteScalar();

                label3.Text = kisiSayisi.ToString() + " Kullanıcı";

                
            }
            catch (Exception hata)
            {
                MessageBox.Show("Veriler yüklenirken hata oluştu: " + hata.Message);
            }
        }

        private void Apart_Yonetim_Anasayfa_Load(object sender, EventArgs e)
        {
            VerileriYukle();
        }
    }
}