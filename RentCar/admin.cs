using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentCar
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide(); 
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddCarsAdmin aca = new AddCarsAdmin();
            aca.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DeleteCar dc = new DeleteCar();
            dc.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ModifyCar mc = new ModifyCar();
            this.Hide();
            mc.Show();
        } 
    }
}
