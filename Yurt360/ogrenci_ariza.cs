using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yurt360.Yurt360;

namespace Yurt360
{
    public partial class ogrenci_ariza : Form
    {
        private Logger logger = new Logger();
        Baglanti baglanti = new Baglanti();
        public int kullaniciID;
        public int kullaniciId;
        public string tCKimlik;
        public ogrenci_ariza(int kullaniciID, string tCKimlik)
        {
            InitializeComponent();
            this.tCKimlik = tCKimlik;
            this.kullaniciID = kullaniciID;
            logger.Log("Info", $"({tCKimlik}): Öğrenci arıza formu başlatıldı.");
            ListeleArizalar();
        }


        private void ListeleArizalar()
        {
             try
             {
                logger.Log("Info", $"({tCKimlik}): Arızalar listeleniyor.");
                string query = "SELECT arizaid, alani, konu, digerkonu, detay, olusturulma_tarihi, durum FROM arizalar WHERE kullanıcıid = @kullaniciid";

                using (SqlConnection connection = new SqlConnection(Baglanti.baglantiDizisi))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@kullaniciid", kullaniciID); // Giriş yapan kullanıcının ID'si ile filtrele

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    logger.Log("Info", $"({tCKimlik}): Arızalar başarıyla listelendi.");
                }
            }
             catch (Exception ex)
             {
                logger.Log("Error", $"({tCKimlik}): Arızalar listelenirken hata oluştu: {ex.Message}");
                MessageBox.Show("Hata: " + ex.Message);
             }
            //ilknurs
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void arızalar_Load(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci arıza formu yükleniyor.");
            ListeleArizalar();

            dataGridView1.Columns["arizaid"].HeaderText = "Arıza ID";
            dataGridView1.Columns["alani"].HeaderText = "Konu";
            dataGridView1.Columns["konu"].HeaderText = "Konu";
            dataGridView1.Columns["digerkonu"].HeaderText = "Diğer Konu";
            dataGridView1.Columns["detay"].HeaderText = "Detay";
            dataGridView1.Columns["olusturulma_tarihi"].HeaderText = "Oluşturulma Tarihi ";
            dataGridView1.Columns["durum"].HeaderText = "Durum";
            logger.Log("Info", $"({tCKimlik}): Veri sütun başlıkları ayarlandı.");
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Uygulama kapattı.");
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci anasayfasına geri döndü.");
            student_homepage homepageForm = new student_homepage(kullaniciID,tCKimlik);
            homepageForm.kullaniciID = this.kullaniciID; // Kullanıcı ID'sini aktar
            homepageForm.Show();
            this.Hide();
            logger.Log("Info", $"({tCKimlik}): Anasayfaya yönlendirildi.");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): DataGridView hücresine tıklandı. Satır: {e.RowIndex}, Sütun: {e.ColumnIndex}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Arızaları yeniden listeledi.");
            ListeleArizalar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ogrenci_ariza ogrenci_Ariza = new ogrenci_ariza(kullaniciID,tCKimlik);
            ogrenci_Ariza.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ogrenciprofil ogrenciProfil = new ogrenciprofil(kullaniciID, tCKimlik);
            ogrenciProfil.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci iletişim ekranını açtı.");
            iletisim Iletisim = new iletisim(kullaniciId, tCKimlik);
            Iletisim.Show();
            this.Hide();
        }
    }
}
