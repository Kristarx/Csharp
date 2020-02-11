using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Dziekanat
{
    public partial class admin : Form
    {
        public static String maxId = "";

        public static void setMaxId(String ID)
        {
            maxId = ID;
        }

        public static String retMaxId()
        {
            return maxId;
        }

        public admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!textBox1.Text.Equals("") && !textBox2.Text.Equals("") && !textBox3.Text.Equals("") && listBox1.Text != "")
            {
                String nowyProw = "INSERT INTO `prowadzacy`(`id`, `imie`, `nazwisko`, `PESEL`, `ty_uzyt`) VALUES (@id, @imie, @nazwisko, @PESEL, 'prowadzacy')";

                Connection conn = new Connection();
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                MySqlDataAdapter msda = new MySqlDataAdapter();
                MySqlCommand msc = new MySqlCommand();

                conn.OpenConnection();

                string newUser = listBox1.GetItemText(listBox1.SelectedItem);

                if(newUser[0] == 's')
                {
                    String MaxSubVal = "SELECT MAX(id_przedmiotu) FROM przedmiot";
                    String nowyStud = "INSERT INTO `student`(`ID`, `PESEL`, `imie`, `nazwisko`, `INDEKS`, `typ_uzyt`, `dataStartu`, `semestr`) VALUES (@id, @PESEL, @imie, @nazwisko, @INDEKS, 'student', @data, @semestr)";
                    String index = createIndex();
                    String maxValue = maxVal();
                    String maxValSub = "";

                    admProwOrazStu();

                    msc.CommandText = MaxSubVal;
                    msc.Connection = conn.GetConnection();
                    msda.SelectCommand = msc;
                    msda.Fill(dt);

                    if (dt.Rows.Count != 0)
                    {
                        DataRow row = dt.Rows[0];
                        maxValSub = row["MAX(id_przedmiotu)"].ToString();
                    }

                    for (int i = 1; i <= Convert.ToInt16(maxValSub); i++)
                    {
                        string Query2 = "INSERT INTO `student_przedmiot_ocena`(`INDEKS`, `id_przedmiotu`, `id_oceny`) VALUES (" + index + "," + i + "," + "7)";
                        MySqlCommand cmdDataBase2 = new MySqlCommand(Query2, conn.GetConnection());
                        cmdDataBase2.ExecuteNonQuery();
                    }

                    msc.CommandText = nowyStud;
                    msc.Connection = conn.GetConnection();
                    msc.Parameters.Add("@id", MySqlDbType.Int16).Value = Convert.ToInt16(maxValue);
                    msc.Parameters.Add("@INDEKS", MySqlDbType.VarChar).Value = index;
                    msc.Parameters.Add("@PESEL", MySqlDbType.VarChar).Value = textBox3.Text;
                    msc.Parameters.Add("@imie", MySqlDbType.VarChar).Value = textBox2.Text;
                    msc.Parameters.Add("@nazwisko", MySqlDbType.VarChar).Value = textBox1.Text;
                    msc.Parameters.Add("@data", MySqlDbType.Date).Value = dateTimePicker1.Value;
                    msc.Parameters.Add("@semestr", MySqlDbType.VarChar).Value = Convert.ToInt16(listBox2.GetItemText(listBox2.SelectedItem));

                    msc.ExecuteNonQuery();
                }
                else if(newUser[0] == 'p')
                {
                    admProwOrazStu();
                    msc.CommandText = nowyProw;
                    msc.Connection = conn.GetConnection();
                    msc.Parameters.Add("@id", MySqlDbType.Int16).Value = Convert.ToInt16(retMaxId());
                    msc.Parameters.Add("@imie", MySqlDbType.VarChar).Value = textBox1.Text;
                    msc.Parameters.Add("@nazwisko", MySqlDbType.VarChar).Value = textBox2.Text;
                    msc.Parameters.Add("@PESEL", MySqlDbType.VarChar).Value = textBox3.Text;
                    msc.ExecuteNonQuery();
                    textBox1.Text = textBox2.Text = textBox3.Text = "";
                }
                else if(newUser[0] == 'a')
                {
                    admProwOrazStu();
                    textBox1.Text = textBox2.Text = textBox3.Text = "";
                }

                conn.CloseConnection();
            }
            else
            {
                MessageBox.Show("Fill all gaps!");
                textBox1.Text = textBox2.Text = textBox3.Text = "";
            }
        }

        private String maxVal()
        {
            String MaxValue = "SELECT MAX(id_klienta+1) FROM uzytkownicy";
            String maxVal = "";

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = MaxValue;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                DataRow row = dt.Rows[0];
                maxVal = row["MAX(id_klienta+1)"].ToString();
            }

            conn.CloseConnection();
            return maxVal;
        }

        private String createPass()
        {
            Random letterSize = new Random();
            Random letter = new Random();
            String password = "";

            for(int i = 0; i < 12; i++)
            {
                int size = letterSize.Next(0, 2);

                if (size == 0)
                {
                    password += (char)(letter.Next(65, 90));
                }
                else if(size == 1)
                {
                    password += (char)(letter.Next(97, 122));
                }
                
            }
            return password;
        }

        private String createIndex()
        {
            Random rand = new Random();
            String index = "";
            for(int i = 0; i < 6; i++)
            {
                int cipher = rand.Next(49, 58);
                index += (char)cipher;
            }

            return index;
        }

        private void admProwOrazStu()
        {
            String createUser = "INSERT INTO `uzytkownicy`(`id_klienta`, `login`, `haslo`, `typ_uzyt`) VALUES (@id_kli, @login, @haslo, @tag)";
            string newUser = listBox1.GetItemText(listBox1.SelectedItem);

            Connection conn = new Connection();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();
            setMaxId(maxVal());
            String maxID = retMaxId();

            msc.CommandText = createUser;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@id_kli", MySqlDbType.Int16).Value = Convert.ToInt16(maxID);
            msc.Parameters.Add("@haslo", MySqlDbType.VarChar).Value = createPass();
            msc.Parameters.Add("@login", MySqlDbType.VarChar).Value = textBox1.Text[0] + textBox2.Text + textBox3.Text[0] + textBox3.Text[1];
            msc.Parameters.Add("@tag", MySqlDbType.VarChar).Value = newUser[0];
            

            if (msc.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Account created!");
            }
            conn.CloseConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Modyfikacja mod = new Modyfikacja();
            mod.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StudentObs SO = new StudentObs();
            this.Hide();
            SO.Show();
        }
    }
}
