using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Yurt360
{

    public partial class admin_ogr_islemleri : Form
    {
        
        public admin_ogr_islemleri()
        {
            InitializeComponent();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
          
        }

        private void admin_kullanicilar_Load(object sender, EventArgs e) //Öğrenci işlemleri 
        {
            pnl_ogr_ekle.Visible = false;
            pnl_ogr_guncelle.Visible = false;
 
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {}

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void button4_Click(object sender, EventArgs e)
        {
            pnl_ogr_ekle.Show();
            pnl_ogr_guncelle.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pnl_ogr_ekle.Visible = false;
            pnl_ogr_guncelle.Show();
        }
    }
    }
