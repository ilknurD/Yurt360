using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yurt360.Yurt360;

namespace Yurt360
{
    public partial class admin_memur_islemleri : Form
    {
        public admin_memur_islemleri()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            admin_ogr_islemleri admin_Ogr_İslemleri = new admin_ogr_islemleri();
            admin_Ogr_İslemleri.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin_memur_islemleri admin_memur_ekle = new admin_memur_islemleri();
            admin_memur_ekle.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Loglama liste sayfası gelecek
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            admin_ogr_islemleri admin_Kullanicilar = new admin_ogr_islemleri();
            admin_Kullanicilar.Show();
            this.Hide();
        }

        private void admin_memur_ekle_Load(object sender, EventArgs e)
        {
            pnl_memur_ekleme.Visible = false;
            pnl_Memur_guncelle.Visible = false;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            pnl_memur_ekleme.Show();
            pnl_Memur_guncelle.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pnl_memur_ekleme.Visible = false;
            pnl_Memur_guncelle.Show();
        }
    }
}
