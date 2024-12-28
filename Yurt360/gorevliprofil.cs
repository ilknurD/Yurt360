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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Yurt360
{
    public partial class gorevliprofil : Form
    {
        private int memurId;
        private string tCKimlik;
        Logger logger = new Logger();
        Baglanti baglanti = new Baglanti();
        Kullanici kullanici = new Kullanici();
        Kullaniciislem Kullaniciislem = new Kullaniciislem();
      
        public gorevliprofil(int memurId, string tCKimlik)
        {
            InitializeComponent();
            this.memurId = memurId;
            this.tCKimlik = tCKimlik;
            logger.Log("Info", $"({tCKimlik}): Memur profil sayfası yükleniyor.");
        }
        private void LoadMemurProfile()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Baglanti.baglantiDizisi))
                {
                    connection.Open();
                    string query = "SELECT * FROM memur WHERE memurid = @memurId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@memurId", memurId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox1.Text = reader["adı"].ToString();
                        textBox2.Text = reader["soyadı"].ToString();
                        textBox3.Text = reader["tckimlik"].ToString();
                        textBox4.Text = reader["telno"].ToString();
                        textBox5.Text = reader["email"].ToString();
                        textBox7.Text = reader["yurtisim"].ToString();
                        textBox6.Text = reader["sifre"].ToString();
                        int yetkiTipi = Convert.ToInt32(reader["yetkitipi"]);
                        textBox8.Text = Enum.GetName(typeof(YetkiTipi), yetkiTipi);

                        logger.Log("Info", $"({tCKimlik}): Memur bilgileri başarıyla yüklendi.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Profil yüklenirken hata: {ex.Message}");
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
       
        
        private void görevliprofil_Load(object sender, EventArgs e)
        {
            LoadMemurProfile(); 
        } 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Güncellenen bilgileri al

            try
            {
                string telNo = textBox4.Text;
                string email = textBox5.Text;
                string sifre = textBox6.Text;

                using (SqlConnection conn = new SqlConnection(Baglanti.baglantiDizisi))
                {
                    conn.Open();
                    logger.Log("Info", $"({tCKimlik}): Memur bilgileri güncelleniyor.");

                    string query = "UPDATE memur SET telno = @telNo, email = @email, sifre = @sifre " +
                                "WHERE memurid = @memurId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@telNo", telNo);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@sifre", sifre);
                    cmd.Parameters.AddWithValue("@memurId", memurId);

                    cmd.ExecuteNonQuery();
                    logger.Log("Info", $"({tCKimlik}): Memur bilgileri başarıyla güncellendi.");
                    MessageBox.Show("Bilgiler başarıyla güncellendi.");
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Bilgiler güncellenirken hata: {ex.Message}");
                MessageBox.Show("Hata: " + ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Memur anasayfasını açtı.");
            officer_page anasayfa = new officer_page(tCKimlik, memurId);
            anasayfa.ShowDialog();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Memur profil ekranı yeniden açtı.");
            gorevliprofil görevli = new gorevliprofil(memurId, tCKimlik);
            görevli.ShowDialog();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Memur arıza ekranını açtı.");
            memur_ariza memur_Ariza = new memur_ariza(tCKimlik,memurId);
            memur_Ariza.Show();
            this.Hide();
        }
    }
}
