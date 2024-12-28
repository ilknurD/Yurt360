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
using Yurt360;
using System.Runtime.InteropServices;

namespace Yurt360
{
    public partial class ogrenciprofil : Form
    {   Baglanti baglanti=new Baglanti();
        Kullanici kullanici=new Kullanici();
        Logger logger = new Logger();
        Kullaniciislem Kullaniciislem=new Kullaniciislem();
        private int kullaniciId;
        private string tCKimlik;
        public ogrenciprofil(int kullaniciId, string tCKimlik)
        {
            InitializeComponent();

            this.kullaniciId = kullaniciId;
            this.tCKimlik = tCKimlik;
            logger.Log("Info", $"({tCKimlik}): Öğrenci profil ekranına giriş yaptı.");
        }

        private void OgrenciBilgileriniYukle()
        {
            try
            {
                logger.Log("Info", $"({tCKimlik}): Öğrenci bilgileri yükleniyor.");

                // Kullanıcı bilgilerini veritabanından alıyoruz
                Kullanici ogrenci = Kullaniciislem.KullaniciDogrulaById(kullaniciId);

                // Veritabanından gelen bilgileri TextBox'lara yüklüyoruz
                textBox1.Text = ogrenci.Adi;
                textBox2.Text = ogrenci.Soyadi;
                textBox3.Text = ogrenci.TCKimlik;
                textBox4.Text = ogrenci.TelNo;
                textBox5.Text = ogrenci.Email;
                textBox6.Text = ogrenci.Sifre;
                textBox7.Text = ogrenci.YurtIsmi;
                textBox8.Text = ogrenci.OdaNo;
                textBox9.Text = ogrenci.YatakNo;

                logger.Log("Info", $"({tCKimlik}): Öğrenci bilgileri başarıyla yüklendi.");
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Öğrenci bilgileri yüklenirken hata oluştu: {ex.Message}");
                MessageBox.Show("Hata: " + ex.Message);
            }

        }
        private void ogrenciprofilcs_Load(object sender, EventArgs e)
        {
            OgrenciBilgileriniYukle();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Uygulamayı kapattı.");
            Application.Exit();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox'lardaki yeni değerleri alıyoruz
                string yeniTelNo = textBox4.Text;
                string yeniEmail = textBox5.Text;
                string yeniSifre = textBox6.Text;

                // Bu bilgileri veritabanına kaydediyoruz
                bool basarili = Kullaniciislem.OgrenciBilgileriniGuncelle(kullaniciId, yeniTelNo, yeniEmail, yeniSifre);

                if (basarili)
                {
                    logger.Log("Info", $"({tCKimlik}): Öğrenci bilgileri başarıyla güncellendi.");
                    MessageBox.Show("Bilgiler başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    logger.Log("Error", $"({tCKimlik}): Öğrenci bilgileri güncellenirken hata oluştu.");
                    MessageBox.Show("Bilgiler güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error", $"({tCKimlik}): Öğrenci bilgileri güncellenirken hata oluştu: {ex.Message}");
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci anasayfasını açtı.");
            student_homepage anasayfa = new student_homepage(kullaniciId,tCKimlik);
            anasayfa.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci profil ekranı yeniden açtı.");
            ogrenciprofil ogrenciProfil = new ogrenciprofil(kullaniciId,tCKimlik);
            ogrenciProfil.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci arıza ekranını açtı.");
            ogrenci_ariza ogrenci_Ariza = new ogrenci_ariza(kullaniciId,tCKimlik);
            ogrenci_Ariza.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci iletişim ekranını açtı.");
            iletisim Iletisim = new iletisim(kullaniciId,tCKimlik);
            Iletisim.Show();
            this.Hide();
        }
    }
}
