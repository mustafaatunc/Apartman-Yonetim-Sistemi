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
    public partial class Loglar : Form
    {
        public Loglar()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();

        void LoglariListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM log ORDER BY id DESC", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView7.DataSource = dt;

                    // Tablo Tasarımı
                    if (dataGridView7.Columns.Count > 5)
                    {
                        dataGridView7.Columns[0].Visible = false; // ID

                        dataGridView7.Columns[1].HeaderText = "İşlem Türü";
                        dataGridView7.Columns[2].HeaderText = "IP Adresi";
                        dataGridView7.Columns[3].HeaderText = "Kullanıcı TC";
                        dataGridView7.Columns[4].HeaderText = "Açıklama";
                        dataGridView7.Columns[5].HeaderText = "Tarih";

                        dataGridView7.Columns[1].Width = 120;
                        dataGridView7.Columns[2].Width = 110;
                        dataGridView7.Columns[3].Width = 110;
                        dataGridView7.Columns[4].Width = 200;
                        dataGridView7.Columns[5].Width = 130;
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Loglar listelenirken hata oluştu: " + hata.Message);
            }
        }

        private void Loglar_Load(object sender, EventArgs e)
        {
            LoglariListele();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "SELECT * FROM log WHERE islem LIKE @ara OR ip LIKE @ara OR tc LIKE @ara OR aciklama LIKE @ara ORDER BY id DESC";

                    SqlDataAdapter ad = new SqlDataAdapter(sorgu, baglanti);
                    ad.SelectCommand.Parameters.AddWithValue("@ara", "%" + textBox15.Text + "%");

                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView7.DataSource = dt;
                }
            }
            catch (Exception) { }
        }

        
        private void textBox15_TextChanged_1(object sender, EventArgs e) { textBox15_TextChanged(sender, e); }
        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e) { }
    }
}