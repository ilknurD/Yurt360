using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yurt360.Yurt360;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Yurt360
{
    public partial class uyeol : Form
    {
        public string tCKimlik;
        public int kullaniciID;
        public uyeol(string tCKimlik, int kullaniciID)
        {
            InitializeComponent();
            this.tCKimlik = tCKimlik;
            this.kullaniciID = kullaniciID;
        }
        private void uyeol_Load(object sender, EventArgs e)
        {

        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Formdan alınan kullanıcı bilgileri
            Kullanici kullanici = new Kullanici
            {
                Adi = textBox1.Text,
                Soyadi = textBox2.Text,
                TCKimlik = textBox3.Text,
                TelNo = textBox4.Text,
                Email = textBox5.Text,
                YurtIsmi = textBox6.Text,
                OdaNo = textBox7.Text,
                YatakNo = textBox8.Text,
                Sifre = textBox9.Text,
                sifretekrar = textBox10.Text,
                YetkiTipi = YetkiTipi.Ogrenci // Yetki Tipi enum'undan bir değer
            };

            bool kayitBasarili = Kullaniciislem.KayitYap(kullanici);
            if (kayitBasarili)
            {
                Login login = new Login(kullaniciID, tCKimlik);
                login.Show();
                this.Hide();
            }
        }
    }
}

     





     
    

              

               


               





