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

namespace Yurt360
{
    public partial class memur_ariza : Form
    {
        private string tCKimlik;
        private int memurId;
        private Logger logger;
        public memur_ariza(string tCKimlik, int memurId)
        {
            InitializeComponent();
            this.tCKimlik = tCKimlik;
            this.memurId = memurId;
            logger = new Logger();
   
        }
        private void panel(object sender, EventArgs e)
        {

        }

        private void panel5_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Log("Info", $"({tCKimlik}): Uygulamadan çıkış yapıldı.");
                Application.Exit();
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Çıkış sırasında hata: {ex.Message}");
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnKaydet"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int arizaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["arizaid"].Value);
                    string yeniDurum = dataGridView1.Rows[e.RowIndex].Cells["cmbDurum"].Value?.ToString();

                    if (string.IsNullOrEmpty(yeniDurum))
                    {
                        logger.Log("Warning", $"({tCKimlik}): Bir durum seçilmedi.");
                        MessageBox.Show("Lütfen bir durum seçin.");
                        return;
                    }

                    using (SqlConnection baglanti = new SqlConnection(Baglanti.baglantiDizisi))
                    {
                        baglanti.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE arizalar SET durum = @durum WHERE arizaid = @arizaid", baglanti);
                        cmd.Parameters.AddWithValue("@durum", yeniDurum);
                        cmd.Parameters.AddWithValue("@arizaid", arizaID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            logger.Log("Info", $"({tCKimlik}): Arıza ID: {arizaID} durumu başarıyla güncellendi.");
                            MessageBox.Show("Durum başarıyla güncellendi.");
                            ListeleArizalar();
                        }
                        else
                        {
                            logger.Log("Warning", $"({tCKimlik}): Arıza ID: {arizaID} için güncelleme başarısız.");
                            MessageBox.Show("Hiçbir satır güncellenmedi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Log("Error", $"({tCKimlik}): Arıza ID güncellenirken hata: {ex.Message}");
                    MessageBox.Show($"Hata: {ex.Message}\n{ex.StackTrace}");
                }
            }

        }
        private void ariza2_Load(object sender, EventArgs e)
        {

            try
            {
                logger.Log("Info", $"({tCKimlik}): Memur arıza formu başlatıldı.");
                ListeleArizalar();
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Memur arıza formu yüklenirken hata: {ex.Message}");
            }

            dataGridView1.Columns["arizaid"].HeaderText = "Arıza ID";
            dataGridView1.Columns["alani"].HeaderText = "Alan";
            dataGridView1.Columns["konu"].HeaderText = "Konu";
            dataGridView1.Columns["digerkonu"].HeaderText = "Diğer Konu";
            dataGridView1.Columns["detay"].HeaderText = "Detay";
            dataGridView1.Columns["olusturulma_tarihi"].HeaderText = "Oluşturulma Tarihi";
            dataGridView1.Columns["durum"].HeaderText = "Durum";         
        }

        private void ListeleArizalar()
        {
            try
            {
                using (SqlConnection baglanti = new SqlConnection("server=DESKTOP-57F0A7E\\SQLEXPRESS;database=Yurt360;integrated security=True"))
                {
                    string query = "SELECT arizaid, alani, konu, digerkonu, detay, olusturulma_tarihi, durum FROM arizalar";
                    SqlDataAdapter da = new SqlDataAdapter(query, baglanti);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    logger.Log("Info", $"({tCKimlik}): Arızalar listelendi.");
                }

                if (dataGridView1.Columns["cmbDurum"] == null)
                {
                    DataGridViewComboBoxColumn cmbDurum = new DataGridViewComboBoxColumn
                    {
                        HeaderText = "Durum",
                        Name = "cmbDurum",
                        DataSource = new string[] { "Bekliyor", "Tamamlandı", "İptal Edildi" }
                    };
                    dataGridView1.Columns.Add(cmbDurum);
                }

                if (dataGridView1.Columns["btnKaydet"] == null)
                {
                    DataGridViewButtonColumn btnKaydet = new DataGridViewButtonColumn
                    {
                        HeaderText = "Kaydet",
                        Name = "btnKaydet",
                        Text = "Kaydet",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridView1.Columns.Add(btnKaydet);
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Arızalar listelenirken hata: {ex.Message}");
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Log("Info", $"({tCKimlik}): Memur anasayfasını açtı.");
                officer_page officerForm = new officer_page(tCKimlik, memurId);
                officerForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Formlar arası geçişte hata: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Memur arıza ekranını açtı.");
            memur_ariza memur_Ariza = new memur_ariza(tCKimlik, memurId);
            memur_Ariza.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Memur profil sayfasını açtı.");
            gorevliprofil gorevliProfil = new gorevliprofil(memurId, tCKimlik);
            gorevliProfil.Show();
            this.Hide();
        }
    }
}
