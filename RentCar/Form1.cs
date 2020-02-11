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
using System.Media;

namespace RentCar
{
    public partial class Form1 : Form
    {
        public static String Id_kli = "";


        public void play()
        {
            SoundPlayer player = new SoundPlayer(@"C:\Users\g580\Desktop\BSS.wav");
            player.Play();
        }

        public Form1()
        {
            InitializeComponent();
            setToLogin();
            //play();
        }

        public static String gimme()
        {
            return Id_kli;
        }

        public static void SetID()
        {
            Id_kli = "";
        }

        private void setToLogin()
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
            textBox9.Visible = false;
            textBox10.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button4.Visible = false;
            button3.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
        }

        private void setToRegistation()
        {
            label1.Visible = label2.Visible = label3.Visible = false;
            label4.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button4.Visible = true;
            button3.Visible = true;
            textBox1.Visible = textBox2.Visible = false;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            textBox8.Visible = true;
            textBox9.Visible = true;
            textBox10.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = true;
            label13.Visible = true;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();
            String QuesLog = "SELECT * FROM `uzytkownicy` WHERE `LOGIN` = @usn AND `HASLO` = @pass";

            command.CommandText = QuesLog;
            command.Connection = conn.GetConnection();
            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = textBox2.Text;

            adapter.SelectCommand = command;
            try
            {
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Id_kli = row["ID_UZYT"].ToString();

                    this.Hide();
                    if (textBox1.Text.Equals("admin"))
                    {
                        admin ad = new admin();
                        this.Hide();
                        ad.Show();
                    }
                    else
                    {
                        Client cl = new Client();
                        this.Hide();
                        cl.Show();
                    }
                }

                else
                {
                    if (textBox1.Text.Trim().Equals("") && !textBox2.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Give your login!", "Empty login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }

                    else if (textBox2.Text.Trim().Equals("") && !textBox1.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Give your password!", "Empty password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }

                    else if (textBox1.Text.Trim().Equals("") && textBox2.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Give your password and login!", "Empty login and password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }
                }

            }
            catch
            {
                MessageBox.Show("Unable to connect with database!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setToRegistation();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setToLogin();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            DataTable dtReg2 = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();
            MySqlCommand msc2 = new MySqlCommand();

            String Question = "SELECT * FROM `uzytkownicy` WHERE `LOGIN` = @RegName";
            String MaxValue = "SELECT MAX(ID_UZYT+1) FROM uzytkownicy";
            String InsertValues = "INSERT INTO `uzytkownicy` VALUES (@ID, @InUser, @InPass)";
            String InsertClientData = "INSERT INTO `klienci` VALUES (@ID, @UserName, @UserSurname, @DrivingNo, 'B', @City, @Street)";
            String maxVal = "";

            conn.OpenConnection();

            msc.CommandText = MaxValue;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dtReg2);

            if (dtReg2.Rows.Count != 0)
            {
                DataRow row = dtReg2.Rows[0];
                maxVal = row["MAX(ID_UZYT+1)"].ToString();
            }

            msc.CommandText = Question;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@RegName", MySqlDbType.VarChar).Value = textBox8.Text;

            msda.SelectCommand = msc;
            
            if (textBox3.Text.Equals("") || textBox4.Text.Equals("") || textBox5.Text.Equals("") || textBox6.Text.Equals("") || textBox7.Text.Equals("") || textBox8.Text.Equals("") || textBox9.Text.Equals("") || textBox10.Text.Equals(""))
            {
                MessageBox.Show("Fill all gaps!");
            }
            else
            {
                
                if (!textBox9.Text.Equals(textBox10.Text))
                {
                    MessageBox.Show("Check if you write the same passwords");
                }
                else
                {
                    try
                    {
                        msda.Fill(dtReg);

                        if (dtReg.Rows.Count != 0)
                        {
                            MessageBox.Show("Given username already exists!", "REGISTER ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }       
                        else if (textBox9.Text.Trim().Equals("") || textBox9.Text.Trim().Equals(""))
                        {
                            MessageBox.Show("Fill all gaps!", "DATA ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }      
                        else
                        {
                            msc.CommandText = InsertValues;
                            msc.Connection = conn.GetConnection();
                            msc.Parameters.Add("@ID", MySqlDbType.VarChar).Value = maxVal;
                            msc.Parameters.Add("@InUser", MySqlDbType.VarChar).Value = textBox8.Text;
                            msc.Parameters.Add("@InPass", MySqlDbType.VarChar).Value = textBox9.Text;

                            msc2.CommandText = InsertClientData;
                            msc2.Connection = conn.GetConnection();
                            msc2.Parameters.Add("@ID", MySqlDbType.VarChar).Value = maxVal;
                            msc2.Parameters.Add("@UserName", MySqlDbType.VarChar).Value = textBox3.Text;
                            msc2.Parameters.Add("@UserSurname", MySqlDbType.VarChar).Value = textBox4.Text;
                            msc2.Parameters.Add("@DrivingNo", MySqlDbType.VarChar).Value = textBox5.Text;
                            msc2.Parameters.Add("@City", MySqlDbType.VarChar).Value = textBox6.Text;
                            msc2.Parameters.Add("@Street", MySqlDbType.VarChar).Value = textBox7.Text;

                            if ((msc.ExecuteNonQuery() == 1) && (msc2.ExecuteNonQuery() == 1))
                            {
                                conn.CloseConnection();
                                MessageBox.Show("Welcome!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                setToLogin();
                            }
                            else
                            {
                                MessageBox.Show("Something goes wrong! Try again!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Something goes wrong! Try again later!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            conn.CloseConnection();
        }
    }
}
