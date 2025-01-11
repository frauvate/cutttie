using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class AnaMenu : Form
    {
        public AnaMenu()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new object[] { "3x3", "5x5", "7x7" });
            comboBox2.Items.AddRange(new object[] { "Bilgisayara Karşı", "İki Kişilik" });
        }

        private void btnOyunuBaslat_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm seçenekleri seçiniz.");
                return;
            }

            int gridSize = comboBox1.SelectedItem.ToString() == "3x3" ? 3 :
                           comboBox1.SelectedItem.ToString() == "5x5" ? 5 : 7;
            bool isTwoPlayer = comboBox2.SelectedItem.ToString() == "İki Kişilik";

            Form1 oyunFormu = new Form1(gridSize, isTwoPlayer);
            oyunFormu.Show();
            this.Hide();
        }

    }
}
