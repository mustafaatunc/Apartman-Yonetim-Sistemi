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
using System.Drawing.Printing; 

namespace Apartman_Yonetim_Sistemi
{
    public partial class Kasa_Tanimlari : Form
    {
        public Kasa_Tanimlari()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        // Global Değişkenler
        decimal toplamGelir = 0;
        decimal toplamGider = 0;
        decimal kasaBakiye = 0;

        // --- YARDIMCI METOTLAR ---

        
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

        
        decimal ToplamGetir(string tabloAdi)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "SELECT SUM(tutar) FROM " + tabloAdi;
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    object sonuc = komut.ExecuteScalar();

                    if (sonuc != null && sonuc != DBNull.Value)
                    {
                        return Convert.ToDecimal(sonuc);
                    }
                }
            }
            catch { }
            return 0;
        }

        // Listeleri Doldur
        void ListeleriYukle()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    // 1. Giderler Listesi
                    SqlDataAdapter adGider = new SqlDataAdapter("SELECT * FROM giderler", baglanti);
                    DataTable dtGider = new DataTable();
                    adGider.Fill(dtGider);
                    dataGridView1.DataSource = dtGider;

                    // 2. Gelirler Listesi
                    SqlDataAdapter adGelir = new SqlDataAdapter("SELECT * FROM gelirler", baglanti);
                    DataTable dtGelir = new DataTable();
                    adGelir.Fill(dtGelir);
                    dataGridView2.DataSource = dtGelir;

                    // 3. Borçlar Listesi
                    SqlDataAdapter adBorc = new SqlDataAdapter("SELECT * FROM borclar", baglanti);
                    DataTable dtBorc = new DataTable();
                    adBorc.Fill(dtBorc);
                    dataGridView14.DataSource = dtBorc;

                    // Tablo Ayarları 
                    AyarlariYap(dataGridView1);
                    AyarlariYap(dataGridView2);
                    AyarlariYap(dataGridView14);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken hata: " + ex.Message);
            }
        }

        void AyarlariYap(DataGridView dgv)
        {
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns[0].Visible = false; 
                dgv.RowHeadersVisible = false;  
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        void HesaplamalariYap()
        {
            
            toplamGelir = ToplamGetir("gelirler");
            toplamGider = ToplamGetir("giderler");
            kasaBakiye = toplamGelir - toplamGider;

            // Ekrana Yazdır
            label89.Text = toplamGelir.ToString("N2") + " TL"; // Toplam Gelir
            label88.Text = toplamGider.ToString("N2") + " TL"; // Toplam Gider
            lbl1.Text = kasaBakiye.ToString("N2") + " TL";     // Kasa Devir

            // Renklendirme
            if (kasaBakiye < 0) lbl1.ForeColor = Color.Red;
            else lbl1.ForeColor = Color.Green;
        }

        private void Kasa_Tanimlari_Load(object sender, EventArgs e)
        {
            ListeleriYukle();
            HesaplamalariYap();
        }

        // --- HESAPLA BUTONU (Button30) ---
        private void button30_Click(object sender, EventArgs e)
        {
            HesaplamalariYap();
            MessageBox.Show("Kasa durumu güncellendi.\n\nToplam Gelir: " + toplamGelir.ToString("N2") + "\nToplam Gider: " + toplamGider.ToString("N2") + "\n\nKASA: " + kasaBakiye.ToString("N2"), "Hesap Özeti", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- YAZDIRMA İŞLEMLERİ ---

        // Gelir Listesi Yazdırma (Button31)
        private void button31_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument4;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument4.Print();
            }
        }

        private void printDocument4_PrintPage(object sender, PrintPageEventArgs e)
        {
            BaslikYazdir(e, "GELİR LİSTESİ");
            TabloYazdir(e, dataGridView2);
        }

        // Gider Listesi Yazdırma (Button33)
        private void button33_Click(object sender, EventArgs e)
        {
            
            printDialog1.Document = printDocument2;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument2.Print();
            }
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            BaslikYazdir(e, "GİDER LİSTESİ");
            TabloYazdir(e, dataGridView1);
        }

        // Borç Listesi Yazdırma (Button34)
        private void button34_Click(object sender, EventArgs e)
        {
            
            printDialog1.Document = printDocument3;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument3.Print();
            }
        }

        private void printDocument3_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            try
            {
                Font baslikFont = new Font("Calibri", 20, FontStyle.Bold);
                Font altBaslikFont = new Font("Calibri", 12, FontStyle.Bold);
                Font icerikFont = new Font("Calibri", 10);
                SolidBrush firca = new SolidBrush(Color.Black);
                Pen kalem = new Pen(Color.Black);

                
                e.Graphics.DrawString("BORÇ BİLDİRİM FİŞİ", baslikFont, firca, 250, 20);
                e.Graphics.DrawLine(kalem, 40, 60, 770, 60);

                
                if (dataGridView14.CurrentRow != null)
                {
                    string tc = dataGridView14.CurrentRow.Cells[2].Value.ToString();
                    string kategori = dataGridView14.CurrentRow.Cells[1].Value.ToString();
                    string tutar = dataGridView14.CurrentRow.Cells[3].Value.ToString();
                    string aciklama = dataGridView14.CurrentRow.Cells[4].Value.ToString();

                    e.Graphics.DrawString("Sayın Daire Sakini,", altBaslikFont, firca, 40, 80);
                    e.Graphics.DrawString("Aşağıda belirtilen borç kaydınız sistemimizde görünmektedir.", icerikFont, firca, 40, 100);

                    e.Graphics.DrawString("TC Kimlik:", altBaslikFont, firca, 40, 150);
                    e.Graphics.DrawString(tc, icerikFont, firca, 200, 150);

                    e.Graphics.DrawString("Borç Türü:", altBaslikFont, firca, 40, 180);
                    e.Graphics.DrawString(kategori, icerikFont, firca, 200, 180);

                    e.Graphics.DrawString("Tutar:", altBaslikFont, firca, 40, 210);
                    e.Graphics.DrawString(tutar + " TL", icerikFont, firca, 200, 210);

                    e.Graphics.DrawString("Açıklama:", altBaslikFont, firca, 40, 240);
                    e.Graphics.DrawString(aciklama, icerikFont, firca, 200, 240);

                    e.Graphics.DrawString("Lütfen en kısa sürede ödeme yapınız.", altBaslikFont, firca, 40, 300);
                }
                else
                {
                    e.Graphics.DrawString("Lütfen listeden bir borç kaydı seçiniz.", altBaslikFont, firca, 40, 100);
                }
            }
            catch { }
        }

        
        void BaslikYazdir(PrintPageEventArgs e, string baslik)
        {
            Font myFont = new Font("Calibri", 20, FontStyle.Bold);
            SolidBrush sbrush = new SolidBrush(Color.Black);
            Pen myPen = new Pen(Color.Black);

            e.Graphics.DrawString(baslik, myFont, sbrush, 300, 20);
            e.Graphics.DrawLine(myPen, 50, 60, 750, 60);
            e.Graphics.DrawString("Tarih: " + DateTime.Now.ToShortDateString(), new Font("Calibri", 10), sbrush, 650, 40);
        }

        void TabloYazdir(PrintPageEventArgs e, DataGridView dgv)
        {
            Font baslikFont = new Font("Calibri", 10, FontStyle.Bold);
            Font icerikFont = new Font("Calibri", 10);
            SolidBrush firca = new SolidBrush(Color.Black);

            int y = 90;
            int x = 50;
            int satirAraligi = 30;

            
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                {
                    e.Graphics.DrawString(col.HeaderText, baslikFont, firca, x, y);
                    x += 120;
                }
            }

            y += satirAraligi;
            e.Graphics.DrawLine(new Pen(Color.Black), 50, y - 10, 750, y - 10);

            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                x = 50;
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Visible)
                    {
                        string deger = cell.Value != null ? cell.Value.ToString() : "";
                        // Uzun metinleri kes
                        if (deger.Length > 15) deger = deger.Substring(0, 12) + "...";

                        e.Graphics.DrawString(deger, icerikFont, firca, x, y);
                        x += 120;
                    }
                }
                y += satirAraligi;
                if (y > 1000) break; 
            }
        }
        private void button31_Click_1(object sender, EventArgs e)
        {
            button31_Click(sender, e); 
        }
    }

}