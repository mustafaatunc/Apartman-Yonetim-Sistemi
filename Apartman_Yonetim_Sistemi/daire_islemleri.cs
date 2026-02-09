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
    public partial class daire_islemleri : Form
    {
        public daire_islemleri()
        {
            InitializeComponent();
        }

        sqlbaglantisi baglan = new sqlbaglantisi();
        string secili_apartman_id = "0"; 

        // --- YARDIMCI METOTLAR ---

        void Temizle()
        {
            textBox24.Clear(); // Daire Tipi
            textBox23.Clear(); // Kira
            textBox22.Clear(); // Aidat
            textBox2.Clear();  // Daire No
            comboBox1.SelectedIndex = -1; // Apartman Seçimi
            secili_apartman_id = "0";
        }

        void DaireListele()
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM daire", baglanti);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dataGridView9.DataSource = dt;

                    // Tablo Tasarımı
                    if (dataGridView9.Columns.Count > 5)
                    {
                        dataGridView9.Columns[0].Visible = false;
                        dataGridView9.Columns[4].Visible = false; 

                        dataGridView9.Columns[1].HeaderText = "Daire Tipi";
                        dataGridView9.Columns[2].HeaderText = "Kira";
                        dataGridView9.Columns[3].HeaderText = "Aidat";
                        dataGridView9.Columns[5].HeaderText = "Daire No";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message);
            }
        }

        void ApartmanlariGetir()
        {
            try
            {
                comboBox1.Items.Clear();
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlCommand komut = new SqlCommand("Select apartman_adi from apartman_islemleri", baglanti);
                    SqlDataReader oku = komut.ExecuteReader();
                    while (oku.Read())
                    {
                        comboBox1.Items.Add(oku["apartman_adi"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Apartmanlar yüklenirken hata: " + ex.Message);
            }
        }

        private void daire_islemleri_Load(object sender, EventArgs e)
        {
            DaireListele();
            ApartmanlariGetir();
        }

        // --- İŞLEMLER ---

        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = baglan.baglan())
                {
                    SqlCommand komut = new SqlCommand("Select id from apartman_islemleri where apartman_adi=@adi", baglanti);
                    komut.Parameters.AddWithValue("@adi", comboBox1.Text);

                    object sonuc = komut.ExecuteScalar();
                    if (sonuc != null)
                    {
                        secili_apartman_id = sonuc.ToString();
                    }
                }
            }
            catch { }
        }

        // EKLEME İŞLEMİ (Button13)
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (secili_apartman_id == "0" || string.IsNullOrEmpty(comboBox1.Text))
                {
                    MessageBox.Show("Lütfen bir apartman seçiniz!");
                    return;
                }

                using (SqlConnection baglanti = baglan.baglan())
                {
                    string sorgu = "insert into daire (tipi, kira_ucreti, aidat_ucreti, apartman_id, daire_no) values (@tip, @kira, @aidat, @aptId, @no)";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    komut.Parameters.AddWithValue("@tip", textBox24.Text);
                    
                    komut.Parameters.AddWithValue("@kira", string.IsNullOrEmpty(textBox23.Text) ? 0 : decimal.Parse(textBox23.Text));
                    komut.Parameters.AddWithValue("@aidat", string.IsNullOrEmpty(textBox22.Text) ? 0 : decimal.Parse(textBox22.Text));
                    komut.Parameters.AddWithValue("@aptId", secili_apartman_id);
                    komut.Parameters.AddWithValue("@no", textBox2.Text);

                    komut.ExecuteNonQuery();
                    MessageBox.Show("Daire başarıyla eklendi.");
                }
                DaireListele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message);
            }
        }

        // GÜNCELLEME İŞLEMİ (Button12)
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView9.CurrentRow != null)
                {
                    using (SqlConnection baglanti = baglan.baglan())
                    {
                        string id = dataGridView9.CurrentRow.Cells[0].Value.ToString();

                        string sorgu = "update daire set tipi=@tip, kira_ucreti=@kira, aidat_ucreti=@aidat, daire_no=@no, apartman_id=@aptId where id=@id";
                        SqlCommand komut = new SqlCommand(sorgu, baglanti);

                        komut.Parameters.AddWithValue("@tip", textBox24.Text);
                        komut.Parameters.AddWithValue("@kira", string.IsNullOrEmpty(textBox23.Text) ? 0 : decimal.Parse(textBox23.Text));
                        komut.Parameters.AddWithValue("@aidat", string.IsNullOrEmpty(textBox22.Text) ? 0 : decimal.Parse(textBox22.Text));
                        komut.Parameters.AddWithValue("@no", textBox2.Text);
                        komut.Parameters.AddWithValue("@aptId", secili_apartman_id);
                        komut.Parameters.AddWithValue("@id", id);

                        komut.ExecuteNonQuery();
                        MessageBox.Show("Daire bilgileri güncellendi.");
                    }
                    DaireListele();
                    Temizle();
                }
                else
                {
                    MessageBox.Show("Lütfen listeden güncellenecek daireyi seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message);
            }
        }

        
        private void dataGridView9_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView9.CurrentRow != null)
                {
                    textBox24.Text = dataGridView9.CurrentRow.Cells[1].Value.ToString(); // Tipi
                    textBox23.Text = dataGridView9.CurrentRow.Cells[2].Value.ToString(); // Kira
                    textBox22.Text = dataGridView9.CurrentRow.Cells[3].Value.ToString(); // Aidat
                    textBox2.Text = dataGridView9.CurrentRow.Cells[5].Value.ToString();  // Daire No

                    
                    string aptId = dataGridView9.CurrentRow.Cells[4].Value.ToString();
                    secili_apartman_id = aptId; 

                    using (SqlConnection baglanti = baglan.baglan())
                    {
                        SqlCommand komut = new SqlCommand("Select apartman_adi from apartman_islemleri where id=@id", baglanti);
                        komut.Parameters.AddWithValue("@id", aptId);
                        object sonuc = komut.ExecuteScalar();
                        if (sonuc != null)
                        {
                            comboBox1.Text = sonuc.ToString();
                        }
                    }
                }
            }
            catch { }
        }

        
        private void button14_Click(object sender, EventArgs e)

        {
            
            dataGridView9_CellClick(sender, null);
        }
        
        private void button11_Click(object sender, EventArgs e) { }

        private void combo_apartman_adi_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            comboBox1_SelectedIndexChanged(sender, e);
        }
    }

}