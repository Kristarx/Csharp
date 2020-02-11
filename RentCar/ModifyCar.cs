using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace RentCar
{
    public partial class ModifyCar : Form
    {
        List<string> data = new List<string>();
        String marka = "", model = "", rok = "";

        public ModifyCar()
        {
            InitializeComponent();
            loadToDataGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin ad = new admin();
            ad.Show();
            this.Hide();
        }

        private void readDataFromFile(String marka, String model, String rok)
        {
            string autoData = @"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + marka + " " + model + " " + rok + "/opis.txt";
            var lines = File.ReadLines(autoData);
            foreach (var line in lines)
            {
                data.Add(line); 
            }

        }

        private String searchSpecData(String specData)
        {
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i].Contains(specData))
                {
                    String retData = data[i].Replace(specData + ":", "").Trim(); ;
                    return retData;
                }
            }

            return "Nope";
        }

        private void loadToDataGrid()
        {
            String ques = "SELECT DISTINCT MARKA, MODEL, ROK_PROD FROM samochody";
            

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (DataRow rows in dtReg.Rows)
            {
                marka = rows["MARKA"].ToString();
                model = rows["MODEL"].ToString();
                rok = rows["ROK_PROD"].ToString();
                dataGridView1.Rows.Add(marka, model, rok);
            }

            conn.CloseConnection();
        }


        private void setDataInTextboxes()
        {
            String ifPresentInFile = searchSpecData(label4.Text.Trim());
            if(!ifPresentInFile.Equals("Nope"))
            {
                textBox1.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label2.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox2.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label3.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox3.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label8.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox7.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label13.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox12.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label14.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox13.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label7.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox6.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label6.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox5.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label12.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox11.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label11.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox10.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label10.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox9.Text = ifPresentInFile;
            }

            ifPresentInFile = searchSpecData(label9.Text.Trim());

            if (!ifPresentInFile.Equals("Nope"))
            {
                textBox8.Text = ifPresentInFile;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String marka = "", model = "", rok = "";
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow1 = dataGridView1.Rows[selectedrowindex];
            marka = Convert.ToString(selectedRow1.Cells[0].Value);
            model = Convert.ToString(selectedRow1.Cells[1].Value);
            rok = Convert.ToString(selectedRow1.Cells[2].Value);
            readDataFromFile(marka.Trim(), model.Trim(), rok.Trim());
            setDataInTextboxes();
            dataGridView1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            data.Clear();
            string autoData = @"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + textBox1.Text.Trim() + " " + textBox2.Text.Trim() + " " + textBox3.Text.Trim() + "/opis.txt";
            File.WriteAllText(autoData, string.Empty);
            insertNewData();
            
            try
            {
                using (var writer = new StreamWriter(autoData))
                {
                    for (var i = 0; i < data.Count; i++)
                    {
                        writer.WriteLine(data[i]);
                    } 
                }

                MessageBox.Show("Data updated!");
                admin ad = new admin();
                ad.Show();
                this.Hide();

            }
            catch (Exception Ex)
            {

            }
        }

        private void insertNewData()
        {
            String ifNotEmpty = searchSpecData(label4.Text.Trim());
            if (textBox1.Text != "")
            {
                data.Add(label4.Text + ": " + textBox1.Text);
            }

            if (textBox2.Text != "")
            {
                data.Add(label2.Text + ": " + textBox2.Text);
            }

            if (textBox3.Text != "")
            {
                data.Add(label3.Text + ": " + textBox3.Text);
            }

            if (textBox7.Text != "")
            {
                data.Add(label8.Text + ": " + textBox7.Text);
            }

            if (textBox12.Text != "")
            {
                data.Add(label13.Text + ": " + textBox12.Text);
            }

            if (textBox13.Text != "")
            {
                data.Add(label14.Text + ": " + textBox13.Text);
            }

            if (textBox6.Text != "")
            {
                data.Add(label7.Text + ": " + textBox6.Text);
            }

            if (textBox5.Text != "")
            {
                data.Add(label6.Text + ": " + textBox5.Text);
            }

            if (textBox11.Text != "")
            {
                data.Add(label12.Text + ": " + textBox11.Text);
            }

            if (textBox10.Text != "")
            {
                data.Add(label11.Text + ": " + textBox10.Text);
            }

            if (textBox9.Text != "")
            {
                data.Add(label10.Text + ": " + textBox9.Text);
            }

            if (textBox8.Text != "")
            {
                data.Add(label9.Text + ": " + textBox8.Text);
            }
        }
    }
}
