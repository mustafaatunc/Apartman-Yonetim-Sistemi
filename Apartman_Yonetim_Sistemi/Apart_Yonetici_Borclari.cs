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
    public partial class Apart_Yonetici_Borclari : Form
    {
        public Apart_Yonetici_Borclari()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        
        private void VerileriGetir(string sorgu, DataGridView tablo, string[] basliklar, int[] genislikler)
        {
            try
            {
                
                SqlCommand komut = new SqlCommand(sorgu, baglan.baglan());
                komut.Parameters.AddWithValue("@tc", Form1.giris); // SQL Injection engellendi

                SqlDataAdapter ad = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                tablo.DataSource = dt;

                
                if (tablo.Columns.Count > 0)
                {
                    tablo.Columns[0].Visible = false; 

                    for (int i = 0; i < basliklar.Length; i++)
                    {
                        
                        if (i + 1 < tablo.Columns.Count)
                        {
                            tablo.Columns[i + 1].HeaderText = basliklar[i];
                            tablo.Columns[i + 1].Width = genislikler[i];
                        }
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Veriler çekilirken hata oluştu: " + hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Apart_Yonetici_Borclari_Load(object sender, EventArgs e)
        {
            // 1. ÖDENENLERİ GETİR
            string sorguOdenen = "Select * From odenen where odeyen=@tc";
            string[] basliklarOdenen = { "Ödeyenin TC NO", "Ödenen Miktar", "Ödeme Türü", "Ödeme Alan TC" };
            int[] genisliklerOdenen = { 120, 120, 110, 130 };

            VerileriGetir(sorguOdenen, dataGridView3, basliklarOdenen, genisliklerOdenen);

            // 2. BORÇLARI GETİR
            string sorguBorc = "Select * From borclar where kullanici=@tc";
            string[] basliklarBorc = { "Borç Kategorisi", "TC Kimlik No", "Borç Tutarı", "Borç Açıklaması", "Borç Tarihi" };
            int[] genisliklerBorc = { 110, 110, 110, 110, 110 };

            VerileriGetir(sorguBorc, dataGridView4, basliklarBorc, genisliklerBorc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               
                printDocument1.DefaultPageSettings.PaperSize = printDocument1.PrinterSettings.PaperSizes[5];
                printDocument1.Print();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Yazdırma hatası: " + hata.Message);
            }
        }
    }
}