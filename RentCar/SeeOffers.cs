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
    public partial class SeeOffers : Form
    {
        String maxYear = "", minYear = "";
        List<string> carsCountries = new List<string>();
        List<string> Years1 = new List<string>();
        List<string> Years2 = new List<string>();
        List<string> MARKA = new List<string>();
        List<string> MODEL = new List<string>();
        public static string marka = "", model = "", rok = "";

        public SeeOffers()
        {
            InitializeComponent();
            Init();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int licznik = 0;
            String QUES = "SELECT DISTINCT MARKA, MODEL, ROK_PROD FROM samochody WHERE ";
            if(listBox1.Text != "")
            {
                QUES += "MARKA = '" + listBox1.Text.Trim() + "' AND ";
                licznik++;
            }

            if (listBox2.Text != "")
            {
                QUES += "MODEL = '" + listBox2.Text.Trim() + "' AND ";
                licznik++;
            }

            if (listBox3.Text != "")
            {
                QUES += "ROK_PROD >= " + listBox3.Text.Trim() + " AND ";
                licznik++;
            }

            if (listBox4.Text != "")
            {
                QUES += "ROK_PROD <= " + listBox4.Text.Trim() + " AND ";
                licznik++;
            }

            if (listBox5.Text != "")
            {
                QUES += "KRAJ_PROD = '" + listBox5.Text.Trim() + "' AND ";
                licznik++;
            }

            if (textBox1.Text != "")
            {
                QUES += "POJ_SIL < " + textBox1.Text.Trim() + " AND ";
                licznik++;
            }

            if (textBox2.Text != "")
            {
                QUES += "KOSZT_DNIA < " + textBox2.Text.Trim() + " AND ";
                licznik++;
            }
          
            string hierKomm = "", dieSonne = "", year = "";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();
            if(licznik != 0)
            {
                command.CommandText = QUES.Substring(0, QUES.Length - 5) + " AND DOSTEPNOSC = 1";
            }
            else
            {
                command.CommandText = QUES.Substring(0, QUES.Length - 6) + " WHERE DOSTEPNOSC = 1";
            }
            
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            try
            {
                foreach (DataRow row in dtReg.Rows)
                {
                    hierKomm = row["MARKA"].ToString();
                    dieSonne = row["MODEL"].ToString();
                    year = row["ROK_PROD"].ToString();

                    DirectoryInfo directory = new DirectoryInfo("C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars");
                    DirectoryInfo[] directories = directory.GetDirectories();
                    Bitmap img;


                    DirectoryInfo d = new DirectoryInfo(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + hierKomm + " " + dieSonne + " " + year);//Assuming Test is your Folder
                    FileInfo[] Files = d.GetFiles("*.bmp");
                    string str = "";

                    str = Files[0].Name;
                    img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + hierKomm + " " + dieSonne + " " + year + "/" + str);
                    var lines = File.ReadLines(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + hierKomm + " " + dieSonne + " " + year + "/auto.txt");
                    int counter = 0;
                    string marka = "", model = "";

                    foreach (var line in lines)
                    {
                        if (counter == 0)
                        {
                            marka = line.ToString().Trim();
                        }
                        if (counter == 1)
                        {
                            model = line.ToString().Trim();
                        }

                        counter++;
                    }

                    dataGridView1.Rows.Add(img, marka, model, year);
                }
            }
            catch
            {

            }

            conn.CloseConnection();
        }

        private void Init()
        {
            this.dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowTemplate.Height = 253;

            DirectoryInfo directory = new DirectoryInfo("C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars");
            DirectoryInfo[] directories = directory.GetDirectories();
            Bitmap img;

            foreach (DirectoryInfo folder in directories)
            {
                DirectoryInfo d = new DirectoryInfo(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + folder.Name);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.bmp"); 
                string str = "";
                try
                {
                    str = Files[0].Name;
                    img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + folder.Name + "/" + str);
                    var lines = File.ReadLines(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + folder.Name + "/auto.txt");
                    int counter = 0;
                    string marka = "", model = "", rok = "";

                    foreach (var line in lines)
                    {
                        if (counter == 0)
                        {
                            marka = line.ToString().Trim();
                        }
                        if (counter == 1)
                        {
                            model = line.ToString().Trim();
                        }
                        if (counter == 2)
                        {
                            rok = line.ToString().Trim();
                        }

                        counter++;
                    }

                    dataGridView1.Rows.Add(img, marka, model, rok);

                    maxAndMinYear();
                    getCarsCountries();
                    markaAuta();
                    listBox1.ClearSelected();
                    listBox3.ClearSelected();
                    listBox4.ClearSelected();
                    listBox5.ClearSelected();
                    listBox1.DataSource = MARKA;
                    listBox3.DataSource = Years1;
                    listBox4.DataSource = Years2;
                    listBox5.DataSource = carsCountries;
                }
                catch
                {

                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            cl.Show();
            this.Hide();
        }

        private void maxAndMinYear()
        {
            String ques = "SELECT MAX(ROK_PROD), MIN(ROK_PROD) FROM samochody";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                maxYear = row["MAX(ROK_PROD)"].ToString();
                minYear = row["MIN(ROK_PROD)"].ToString();
            }

            conn.CloseConnection();

            bool ifDatesEquals = false;

            while (!ifDatesEquals)
            {
                if (string.Compare(minYear, maxYear) <= 0)
                {
                    Years1.Add(minYear);
                    Years2.Add(minYear);
                    minYear = (Convert.ToInt16(minYear) + 1).ToString();
                }
                else
                {
                    ifDatesEquals = true;
                }
            }
        }

        private void getCarsCountries()
        {
            String ques = "SELECT DISTINCT KRAJ_PROD FROM samochody";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            foreach(DataRow rows in dtReg.Rows)
            {
                carsCountries.Add(rows["KRAJ_PROD"].ToString());
            }

            conn.CloseConnection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = "";
            listBox1.ClearSelected();
            listBox3.ClearSelected();
            listBox4.ClearSelected();
            listBox5.ClearSelected();
        }

        private void markaAuta()
        {
            String ques = "SELECT DISTINCT MARKA FROM samochody";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            foreach (DataRow rows in dtReg.Rows)
            {
                MARKA.Add(rows["MARKA"].ToString());
            }

            conn.CloseConnection();
        }

        private void modeleAut(string marka)
        {
            String ques = "SELECT DISTINCT MODEL FROM samochody WHERE MARKA = @MARKA";
            MODEL.Clear();
            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            command.Parameters.Add("@MARKA", MySqlDbType.VarChar).Value = marka;
            adapter.Fill(dtReg);

            foreach (DataRow rows in dtReg.Rows)
            {
                MODEL.Add(rows["MODEL"].ToString());
            }

            conn.CloseConnection();
        }

        private void markiAut()
        {           
            string marka = listBox1.GetItemText(listBox1.SelectedItem).Trim();
            modeleAut(marka);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            markiAut();
            listBox2.Items.Clear();

            foreach (string yool in MODEL)
            {
                listBox2.Items.Add(yool);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string amount = textBox1.Text;
            
            if(amount.Length > 0)
            {
                if(Convert.ToInt16(amount[0]) < 48 || Convert.ToInt16(amount[0]) > 57)
                {
                    textBox1.Text = amount.Remove(0);
                }
                if ((Convert.ToInt16(amount[amount.Length - 1]) < 48 || Convert.ToInt16(amount[amount.Length - 1]) > 57) && (Convert.ToInt16(amount[amount.Length - 1]) != 44 && Convert.ToInt16(amount[amount.Length - 1]) != 46))
                {
                    textBox1.Text = amount.Remove(amount.Length - 1);
                }

                textBox1.Text = textBox1.Text.Replace(',', '.');

                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.SelectionLength = 0;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string amount = textBox2.Text;

            if (amount.Length > 0)
            {
                if ((Convert.ToInt16(amount[amount.Length - 1]) < 48 || Convert.ToInt16(amount[amount.Length - 1]) > 57))
                {
                    textBox2.Text = amount.Remove(amount.Length - 1);
                }

                textBox2.SelectionStart = textBox2.Text.Length;
                textBox2.SelectionLength = 0;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow1 = dataGridView1.Rows[selectedrowindex];
            marka = Convert.ToString(selectedRow1.Cells[1].Value);
            model = Convert.ToString(selectedRow1.Cells[2].Value);
            rok = Convert.ToString(selectedRow1.Cells[3].Value);
            SeeSingleOffer sso = new SeeSingleOffer();
            this.Hide();
            sso.Show();
        }
    }
}
