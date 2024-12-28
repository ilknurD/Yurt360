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
    public partial class officer_page : Form
    {
        private string tCKimlik;
        private int memurId;
        private readonly string connectionString = "server=DESKTOP-57F0A7E\\SQLEXPRESS;Database=Yurt360;Trusted_Connection=True;";
        Logger logger = new Logger();
        Kullanici kullanici = new Kullanici();
        public officer_page(string tCKimlik, int memurId)
        {
            InitializeComponent();
            this.tCKimlik = tCKimlik;
            logger = new Logger();
            logger.Log("Info", $"({tCKimlik}) Memur anasayfa formu yükleniyor.");
            LoadArizaData(); // Sayfa yüklendiğinde verileri getirir
            SetupDomainUpDown();
            this.memurId = memurId;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}) Uygulamayı kapattı.");
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void officer_page_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            logger.Log("Info", $"({tCKimlik}) Memurun anasayfası yüklendi ve arızalar listelendi.");
        }

        private void LoadArizaData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT alani, detay, olusturulma_tarihi FROM arizalar";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataTable.Columns["alani"].ColumnName = "Kategori";
                    dataTable.Columns["detay"].ColumnName = "Açıklama";
                    dataTable.Columns["olusturulma_tarihi"].ColumnName = "Tarih";


                    dataGridView1.DataSource = dataTable;
                    logger.Log("Info", $"({tCKimlik}) Arıza verileri yüklendi.");
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}) Arıza verileri yüklenirken hata oluştu: {ex.Message}"); 
                MessageBox.Show("Veriler yüklenirken hata oluştu.");
            }

        }

        private void SetupDomainUpDown()
        {
            domainUpDown1.Items.Add("Tüm Kayıtlar");
            domainUpDown1.Items.Add("Elektrik");
            domainUpDown1.Items.Add("Tesisat");
            domainUpDown1.Items.Add("Isıtma");
            domainUpDown1.Items.Add("Mobilya");
            domainUpDown1.SelectedIndex = 0;
            logger.Log("Info", $"({tCKimlik}) Seçenekler yüklendi.");
        }

        private void FormatGridRows()
        {
        }

        private void FilterData()
        {
            try
            {
                
                string filter = "";
                if (domainUpDown1.SelectedItem.ToString() != "Tüm Kayıtlar")
                {
                    filter = $"Kategori = '{domainUpDown1.SelectedItem}'";
                }

                string dateFilter = $"Tarih >= '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}'";

                // Filtreleme işlemi
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = !string.IsNullOrEmpty(filter)
                    ? $"{filter} AND {dateFilter}"
                    : dateFilter;
                logger.Log("Info", $"({tCKimlik}) Memur seçimiyle veriler {domainUpDown1.SelectedItem} ve tarih filtreleriyle filtreledi.");
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}) Filtreleme sırasında hata oluştu: {ex.Message}");
                MessageBox.Show("Filtreleme işlemi sırasında hata oluştu.");
            }


        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            officer_page officer_Page = new officer_page(tCKimlik, memurId);
            officer_Page.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gorevliprofil gorevliProfil = new gorevliprofil(memurId, tCKimlik);
            gorevliProfil.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            memur_ariza memur_Ariza = new memur_ariza(tCKimlik, memurId);
            memur_Ariza.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //iletişim
        }
    }

}

