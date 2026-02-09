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
    public partial class Apartman_Islemleri : Form
    {
        public Apartman_Islemleri()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        

        void VerileriListele()
        {
            try
            {
                
                DataTable dt = new DataTable();

                
                using (SqlConnection baglanti = bgl.baglanti())
                {
                    SqlDataAdapter da = new SqlDataAdapter("Select * from apartman_islemleri", baglanti);
                    da.Fill(dt);
                }

                dataGridView1.DataSource = dt;

                
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "Apartman Adı";
                    dataGridView1.Columns[2].HeaderText = "Blok";
                    dataGridView1.Columns[3].HeaderText = "Adres";
                    dataGridView1.Columns[4].HeaderText = "Daire Sayısı";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler listelenirken hata oluştu: " + ex.Message);
            }
        }

        void Temizle()
        {
            textBox1.Clear(); // Apartman Adı
            textBox2.Clear(); // Blok
            textBox3.Clear(); // Adres
            textBox4.Clear(); // Daire Sayısı
            textBox1.Focus(); // İmleci ilk kutuya getir
        }

        private void Apartman_Islemleri_Load(object sender, EventArgs e)
        {
            VerileriListele();
        }

        // --- İŞLEM BUTONLARI ---

        // EKLE BUTONU (Button19)
        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = bgl.baglanti())
                {
                    // Parametreli ve Güvenli Kayıt
                    SqlCommand komut = new SqlCommand("insert into apartman_islemleri (apartman_adi, blok, adres, daire_sayisi) values (@p1, @p2, @p3, @p4)", baglanti);
                    komut.Parameters.AddWithValue("@p1", textBox1.Text);
                    komut.Parameters.AddWithValue("@p2", textBox2.Text);
                    komut.Parameters.AddWithValue("@p3", textBox3.Text);
                    komut.Parameters.AddWithValue("@p4", textBox4.Text); // Sayı girmesi lazım

                    komut.ExecuteNonQuery();
                }

                MessageBox.Show("Apartman Bilgisi Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VerileriListele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // SİL BUTONU (Button17)
        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                
                DialogResult secim = MessageBox.Show("Bu apartman kaydını silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (secim == DialogResult.Yes)
                {
                    using (SqlConnection baglanti = bgl.baglanti())
                    {
                        
                        SqlCommand komut = new SqlCommand("delete from apartman_islemleri where apartman_adi=@p1", baglanti);
                        komut.Parameters.AddWithValue("@p1", textBox1.Text);
                        komut.ExecuteNonQuery();
                    }
                    MessageBox.Show("Kayıt Silindi");
                    VerileriListele();
                    Temizle();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // GÜNCELLE BUTONU (Button16)
        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = bgl.baglanti())
                {
                    SqlCommand komut = new SqlCommand("update apartman_islemleri set blok=@p1, adres=@p2, daire_sayisi=@p3 where apartman_adi=@p4", baglanti);
                    komut.Parameters.AddWithValue("@p1", textBox2.Text);
                    komut.Parameters.AddWithValue("@p2", textBox3.Text);
                    komut.Parameters.AddWithValue("@p3", textBox4.Text);
                    komut.Parameters.AddWithValue("@p4", textBox1.Text); // Şartımız apartman adı
                    komut.ExecuteNonQuery();
                }
                MessageBox.Show("Kayıt Güncellendi");
                VerileriListele();
                Temizle();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // TEMİZLE BUTONU (Button1)
        private void button1_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // İsim değişikliği yaptım, Designer'dan tekrar tıklaman gerekebilir.
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;

               
                textBox1.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString(); // Adı
                textBox2.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString(); // Blok
                textBox3.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString(); // Adres
                textBox4.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString(); // Daire Sayısı
            }
            catch { }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        
        private void button2_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
    }
}