using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Net;
using System.Threading;

namespace WalutyOnline
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            button1.Visible = true;
            progressBar1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            label2.Visible = true;
            progressBar1.Visible = true;

            GetData GT = new GetData();
            int counter = 1;

            if (GT.CheckConnection())
            {
                int DaysDiff = GT.ReturnDiffDays();
                int div = DaysDiff / 20;

                for (int i = 0; i <= DaysDiff; i++)
                {
                    counter = GT.DownloadFromInternet(i, counter);
                    if(i%div == 0)
                    {
                        progressBar1.Increment(5);
                    }
                }

                progressBar1.Value = 100;
                this.Hide();
                Courses cs = new Courses();
                cs.Show();
            }
            else
            {
                MessageBox.Show("There's no internet connection. Data will be downloaded from files saved on your computer");
            }
        }
    }
}
