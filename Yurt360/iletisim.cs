using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Net;
using Yurt360.Yurt360;

namespace Yurt360
{
    public partial class iletisim : Form
    {Kullaniciislem kullaniciislem=new Kullaniciislem();
        private int kullaniciId;
        private string tCKimlik;
        private Kullaniciislem kullaniciIslem;
        Logger logger = new Logger();
        public iletisim(int kullaniciId, string tCKimlik)
        {
            InitializeComponent();
            this.kullaniciId = kullaniciId;
            this.tCKimlik = tCKimlik;
            kullaniciIslem = new Kullaniciislem();

        }
        private void LoadStudentInfo()
        {
            try
            {
                // Kullanıcı bilgilerini getir
                Kullanici kullanici = Kullaniciislem.KullaniciDogrulaById(kullaniciId);

                if (kullanici != null)
                {
                    logger.Log("Info", $"({kullanici.TCKimlik}): Kullanıcı bilgileri başarıyla yüklendi.");
                    // TextBox'lara doldur
                    textBox1.Text = kullanici.Adi;
                    textBox2.Text = kullanici.Soyadi;
                    textBox3.Text = kullanici.TCKimlik;
                    textBox4.Text = kullanici.Email;
                }
                else
                {
                    MessageBox.Show("Kullanıcı bilgileri yüklenemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Log("Error", $"({tCKimlik}): Kullanıcı bilgileri yüklenemedi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log("Error", $"({tCKimlik}): Kullanıcı bilgileri yüklenirken hata oluştu: {ex.Message}");
            }
        }
        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
            logger.Log("Info", $"({tCKimlik}): Uygulama kapatıldı.");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e) {
            try
            {
                // Sabit gönderen ve alıcı bilgileri
                string fromMail = "celilevisne1@gmail.com"; // Sabit gönderen e-posta
                string fromPassword = "dwal shth tzjw tszg"; // Sabit gönderen şifresi (Uygulama şifresi)
                string toMail = "222303015@ogr.uludag.edu.tr"; // Sabit alıcı e-posta

                // SMTP istemci ayarları
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromMail, fromPassword)
                };

                //mail içeriği ayarları
                string subjectText = textBox4.Text.Trim(); 
                if (string.IsNullOrEmpty(subjectText))
                {
                    subjectText = "Varsayılan Konu"; 
                }
                else
                {
                
                    subjectText = subjectText.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, "");//karakter ayarları
                }

                //mail gönderimi
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Body = richTextBox1.Text,
                    Subject = subjectText 
                };

                
                mailMessage.To.Add(toMail);

               
                smtpClient.Send(mailMessage);
                MessageBox.Show("E-posta başarıyla gönderildi. E-postanızı kontrol etmeyi unutmayınız.");
                logger.Log("Info", $"({tCKimlik}): E-posta başarıyla gönderildi.");
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show("E-posta gönderilirken SMTP hatası oluştu: " + smtpEx.Message);
                logger.Log("Error", $"({tCKimlik}): E-posta gönderimi sırasında SMTP hatası: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                logger.Log("Error", $"({tCKimlik}): E-posta gönderimi sırasında hata oluştu: {ex.Message}");
            }
        }

    
    
        private void iletisim_Load(object sender, EventArgs e)
        {
            LoadStudentInfo();
            logger.Log("Info", $"({tCKimlik}): İletişim formu yüklendi.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci anasayfasını açtı.");
            student_homepage anasayfa = new student_homepage(kullaniciId, tCKimlik);
            anasayfa.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci profil ekranını açtı.");
            ogrenciprofil ogrenciProfil = new ogrenciprofil(kullaniciId, tCKimlik);
            ogrenciProfil.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci arıza ekranını açtı.");
            ogrenci_ariza ogrenci_Ariza = new ogrenci_ariza(kullaniciId, tCKimlik);
            ogrenci_Ariza.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logger.Log("Info", $"({tCKimlik}): Öğrenci iletişim ekranını tekrar açtı.");
            iletisim Iletisim = new iletisim(kullaniciId,tCKimlik);
            Iletisim.Show();
            this.Hide();
        }
    }
}
