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
    public partial class student : Form
    {
        int isSetChangeable = 0;
        public static String remIndeks = "";

        public student()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            listBox1.Visible = true;
            label1.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            dataGridView1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = true;
        }

        private void checkData()
        {
            listBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            dataGridView1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = false;
            setUnchangeable();
        }

        private void setUnchangeable()
        {
            textBox1.Enabled = false; textBox1.ReadOnly = true;
            textBox2.Enabled = false; textBox2.ReadOnly = true;
            textBox3.Enabled = false; textBox3.ReadOnly = true;
            textBox4.Enabled = false; textBox4.ReadOnly = true;
            textBox5.Enabled = false; textBox5.ReadOnly = true;
            textBox6.Enabled = false; textBox6.ReadOnly = true;
            textBox7.Enabled = false; textBox7.ReadOnly = true;
            isSetChangeable = 0;
        }

        private void setChangeable()
        {
            textBox2.Enabled = true; textBox2.ReadOnly = false;
            textBox3.Enabled = true; textBox3.ReadOnly = false;
            isSetChangeable = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkData();
            getPersonalData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(isSetChangeable == 0)
            {
                setChangeable();
            }
            
            else
            {
                setUnchangeable();
                updateAfterEdit();
                getPersonalData();
            }
        }

        private void getPersonalData()
        {
            String whichCurrAmout = "SELECT * FROM student WHERE ID = @ID";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = whichCurrAmout;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@ID", MySqlDbType.Int16).Value = Form1.retId();
            msda.SelectCommand = msc;
            msda.Fill(dtReg);


            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                textBox1.Text = row["PESEL"].ToString();
                textBox2.Text = row["imie"].ToString();
                textBox3.Text = row["nazwisko"].ToString();
                textBox4.Text = row["INDEKS"].ToString();
                textBox5.Text = row["typ_uzyt"].ToString();
                textBox6.Text = row["dataStartu"].ToString().Substring(0,10).Trim();
                textBox7.Text = row["semestr"].ToString();
            }

            conn.CloseConnection();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1.setId();
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = listBox1.GetItemText(listBox1.SelectedItem);
            if(text != "")
            {
                String getResult = "SELECT p.nazwa, p.opis, o.wartosc FROM przedmiot p JOIN student_przedmiot_ocena spo ON p.id_przedmiotu = spo.id_przedmiotu JOIN ocena o ON spo.id_oceny = o.id_oceny WHERE spo.INDEKS = @INDEKS AND p.nr_sem = @Num";
                String checkSemester = "SELECT TIMESTAMPDIFF(MONTH, dataStartu, CURRENT_DATE()) AS ILE FROM `student` WHERE ID = @ID";
                String whichCurrAmout = "SELECT INDEKS FROM student WHERE ID = @ID";
                String nazwa = "", wartosc = "", indeks = "", opis = "", whichSem = "";
                Connection conn = new Connection();
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();


                command.CommandText = checkSemester;
                command.Connection = conn.GetConnection();
                command.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.retId());
                adapter.SelectCommand = command;
                adapter.Fill(dt3);

                if (dt3.Rows.Count > 0)
                {
                    DataRow row = dt3.Rows[0];
                    whichSem = row["ILE"].ToString();
                }

                command.CommandText = whichCurrAmout;
                command.Connection = conn.GetConnection();
                adapter.SelectCommand = command;
                adapter.Fill(dt2);

                if (dt2.Rows.Count > 0)
                {
                    DataRow row = dt2.Rows[0];
                    indeks = row["INDEKS"].ToString();
                }

                int chosenSem = Convert.ToInt16(text.Trim().Substring(text.Length - 1, 1));

                command.CommandText = getResult;
                command.Connection = conn.GetConnection();
                command.Parameters.Add("@INDEKS", MySqlDbType.VarChar).Value = indeks;
                command.Parameters.Add("@Num", MySqlDbType.Int16).Value = chosenSem;

                adapter.SelectCommand = command;

                int semestr = Convert.ToInt16(whichSem)/6+1;
               
                adapter.Fill(dt);

                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                if (chosenSem <= semestr)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            nazwa = row["nazwa"].ToString();
                            opis = row["opis"].ToString();
                            wartosc = row["wartosc"].ToString(); 
                            dataGridView1.Rows.Add(nazwa, opis, wartosc);
                        }


                        /*try
                        {

                            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                            {
                                if(dataGridView1.Rows[rows].Cells[2].Value.ToString().Equals("5") || dataGridView1.Rows[rows].Cells[2].Value.ToString().Equals("4,5"))
                                {
                                    MessageBox.Show(dataGridView1.Rows[rows].Cells[0].Value.ToString());
                                }
                                
                            }
                        }
                        catch
                        {
                            //MessageBox.Show("try again" + ex);
                        }*/


                    }
                }
                else
                {
                    MessageBox.Show("Semester hasn't been started yet");
                }
                    
            }      
        }

        private void updateAfterEdit()
        {
            String WymianaUpdate = "UPDATE `student` SET `imie`= @imie,`nazwisko`=@nazwisko WHERE ID = @ID";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = WymianaUpdate;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@imie", MySqlDbType.VarChar).Value = textBox2.Text;
            msc.Parameters.Add("@nazwisko", MySqlDbType.VarChar).Value = textBox3.Text;
            msc.Parameters.Add("@ID", MySqlDbType.Int16).Value = Form1.retId();

            if ((msc.ExecuteNonQuery() == 1))
            {
                MessageBox.Show("Your data has been updated");               
            }
            else
            {
                MessageBox.Show("Something went wrong. Contact personally with office");
            }

            conn.CloseConnection();

        }
    }
}
