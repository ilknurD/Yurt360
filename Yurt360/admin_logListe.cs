using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yurt360
{
    public partial class admin_logListe : Form
    {
        public admin_logListe()
        {
            InitializeComponent();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            admin_ogr_islemleri admin_Kullanicilar = new admin_ogr_islemleri();
            admin_Kullanicilar.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            admin_ogr_islemleri admin_Kullanicilar = new admin_ogr_islemleri();
            admin_Kullanicilar.Show();
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
            admin_logListe admin_Log = new admin_logListe();
            admin_Log.Show();
            this.Hide();
        }
    }
}
