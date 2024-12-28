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
using Yurt360.Yurt360;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Yurt360
{
    public partial class student_homepage : Form
    {
        public string tCKimlik;
        public int kullaniciID;
        private int kullaniciId;
        private Dictionary<string, List<string>> AlanVeKonu = new Dictionary<string, List<string>>();
        SqlConnection baglanti = new SqlConnection("server=DESKTOP-57F0A7E\\SQLEXPRESS;database=Yurt360;integrated security=True");
        Logger logger = new Logger();
        Kullanici kullanici = new Kullanici();
        public student_homepage(int kullaniciID ,string tCKimlik)
        {
            InitializeComponent();
            this.tCKimlik = tCKimlik;
            this.kullaniciID = kullaniciID;
        }

        public student_homepage()
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void customTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void student_homepage_Load(object sender, EventArgs e)
        {
           
            try
            {
                AlanVeKonu.Add("Elektrik", new List<string> { "Priz", "Lamba" });
                AlanVeKonu.Add("Isıtma", new List<string> { "Kalorifer" });
                AlanVeKonu.Add("Mobilya", new List<string> { "Kapı", "Dolap", "Baza", "Masa", "Sandalye" });
                AlanVeKonu.Add("Tesisat", new List<string> { "Lavabo", "Banyo", "Klozet" });

                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(AlanVeKonu.Keys.ToArray());

                logger.Log("Info",$"({tCKimlik}) öğrenci ana sayfasına giriş yaptı.");
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}) student_homepage yüklenirken hata oluştu: {ex.Message}");
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem == null)
                {
                    comboBox2.Items.Clear(); 
                    comboBox2.SelectedIndex = -1;
                    return;
                }
                string selectedAlan = comboBox1.SelectedItem.ToString();
                comboBox2.Items.Clear();
                comboBox2.SelectedIndex = -1;

                if (AlanVeKonu.ContainsKey(selectedAlan))
                {
                    comboBox2.Items.AddRange(AlanVeKonu[selectedAlan].ToArray());
                    comboBox2.SelectedIndex = 0; 

                    logger.Log("Info", $"({tCKimlik}) {selectedAlan} alanı seçti ve ilgili konular yüklendi.");
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}) Alan seçimi sırasında hata oluştu: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int kytsay;
            string alani = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "";
            string konu = comboBox2.SelectedItem != null ? comboBox2.SelectedItem.ToString() : "";
            string digerkonu = textBox1.Text;
            string detay = richTextBox1.Text;


            try
            {
                if (string.IsNullOrEmpty(alani) || string.IsNullOrEmpty(konu))
                {
                    MessageBox.Show("Lütfen geçerli bir alan seçiniz.");
                    return;
                }
                if (string.IsNullOrEmpty(konu) && string.IsNullOrEmpty(digerkonu))
                {
                    MessageBox.Show("Lütfen geçerli bir konu seçiniz veya diğer alanı doldurunuz.");
                    return;
                }
                if (string.IsNullOrEmpty(detay))
                {
                    MessageBox.Show("Lütfen detay kısmını doldurunuz.");
                    return ;
                }

                baglanti.Open();
                SqlCommand cmd = new SqlCommand("Insert into arizalar (alani, konu, digerkonu, detay,olusturulma_tarihi, kullanıcıid) values (@alan,@konu,@diger,@detay,@tarih,@kullanıcıid)", baglanti);

                cmd.Parameters.AddWithValue("@alan", alani);
                cmd.Parameters.AddWithValue("@konu", konu);
                cmd.Parameters.AddWithValue("@diger", digerkonu);
                cmd.Parameters.AddWithValue("@detay", detay);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                cmd.Parameters.AddWithValue("@kullanıcıid", kullaniciID);

                kytsay = cmd.ExecuteNonQuery();

                if (kytsay > 0)
                {
                    MessageBox.Show("Kayıt Eklendi.");
                    logger.Log("Info", $"({tCKimlik}) Kullanıcı kayıt ekledi: Alan={alani}, Konu={konu}, Diğer={digerkonu}, Detay={detay}");
                   
                }
                else
                {
                    MessageBox.Show("Kayıt Ekleme Hatası!");
                    logger.Log("Error", "Kayıt eklenemedi.");
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"Kayıt ekleme sırasında hata oluştu: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            student_homepage student_Homepage = new student_homepage(kullaniciID, tCKimlik);
            student_Homepage.Show();
            this.Hide();
            logger.Log("Info", $"({tCKimlik}) öğrenci anasayfasını tekrar açtı.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ogrenciprofil ogrenciProfil = new ogrenciprofil(kullaniciID, tCKimlik);
            ogrenciProfil.Show();
            this.Hide();
            logger.Log("Info", $"({tCKimlik}) öğrenci profili açtı.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ogrenci_ariza ogrenci_Ariza = new ogrenci_ariza(kullaniciID, tCKimlik);
            ogrenci_Ariza.Show();
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
