using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yurt360.Yurt360;

namespace Yurt360
{
    public partial class Login : Form
    {
        private string tCKimlik;
        private int kullaniciID;
        private int memurId;
        Logger logger = new Logger();
        Kullanici kullanici = new Kullanici();
        public Login(int kullaniciID, string tCKlimlik)
        {
            InitializeComponent();
            this.kullaniciID = kullaniciID;
            this.tCKimlik = tCKlimlik;
            this.memurId = kullaniciID;
            logger.Log("Info", $" Login formu başlatıldı.");
        }

        public Login()
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Click(object sender, EventArgs e)
        {
            Application.Exit();
            logger.Log("Info", $"({kullanici.TCKimlik}): Uygulamayı kapattı.");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({kullanici.TCKimlik}): Üye ol formu açılıyor.");
            uyeol uyeOl = new uyeol(tCKimlik, kullaniciID);
            uyeOl.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            KullaniciGirisi();
        }
        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakam girişine izin ver (backspace hariç)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            // Eğer 11 basamağı geçtiyse, yeni girişe izin verme
            if (textBox1.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            if (!long.TryParse(textBox1.Text, out _) || textBox1.Text.Length > 11)
            {
                MessageBox.Show("Sadece 11 basamaklı bir sayı girebilirsiniz!", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Text = string.Empty; // TextBox'ı temizle
            }
        }

        public void KullaniciGirisi()
        {
            string tcKimlik = textBox1.Text;
            string sifre = textBox2.Text;
            try
            {
                Kullanici kullanici = Baglanti.KullaniciDogrula(tcKimlik, sifre);
                if (kullanici != null)
                {
                    int kullaniciID = kullanici.Id;
                    string tCKimlik = kullanici.TCKimlik; //altta okumadığı için tekrar kullanici.Adi diyerek almam gerekti.
                    int memurId = kullanici.Id;
                    MessageBox.Show($"Giriş başarılı! Hoş geldiniz, {kullanici.Adi} {kullanici.Soyadi}.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    switch (kullanici.YetkiTipi)
                    {
                        
                        case YetkiTipi.Ogrenci:
                            logger.Log("Info", $"{kullanici.Adi} {kullanici.Soyadi} ({kullanici.TCKimlik}) sisteme öğrenci olarak giriş yaptı.");
                            student_homepage student_Homepage = new student_homepage(kullaniciID, tCKimlik);
                            student_Homepage.Show();
                            this.Hide();
                            break;
                        case YetkiTipi.Admin:
                            logger.Log("Info", $"{kullanici.Adi} {kullanici.Soyadi} ({kullanici.TCKimlik}) sisteme admin olarak giriş yaptı.");
                            admin_ogr_islemleri admin_Ogr_İslemleri = new admin_ogr_islemleri();
                            admin_Ogr_İslemleri.Show();
                            this.Hide();
                            break;
                        case YetkiTipi.Memur:
                            logger.Log("Info", $"{kullanici.Adi} {kullanici.Soyadi} ({kullanici.TCKimlik}) sisteme memur olarak giriş yaptı.");
                            officer_page officer_Page = new officer_page(tCKimlik, memurId);
                            officer_Page.Show();
                            this.Hide();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("TC Kimlik Numarası veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Log("Warning", $"({tcKimlik} {sifre}): Hatalı giriş denemesi.");
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tcKimlik} {sifre}): Giriş sırasında hata oluştu: {ex.Message}");
                MessageBox.Show("Giriş sırasında bir hata oluştu: " + ex.Message);
            }

        }
    }
}
