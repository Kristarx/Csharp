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
using System.IO;

namespace RentCar
{
    public partial class AddCarsAdmin : Form
    {
        public AddCarsAdmin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin ad = new admin();
            this.Hide();
            ad.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                String ques = "Select * FROM samochody s WHERE s.marka = @marka AND s.model = @model AND s.rok_prod = @rokProd";
                String marka = "", model = "", nrRej = "", rokProd = "", krajProd = "", pojSil = "", kosztDnia = "", typPrawa = "";
                Connection conn = new Connection();
                DataTable dtReg = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();

                conn.OpenConnection();

                command.CommandText = ques;
                command.Connection = conn.GetConnection();
                adapter.SelectCommand = command;
                command.Parameters.Add("@marka", MySqlDbType.VarChar).Value = textBox1.Text.ToString().Trim();
                command.Parameters.Add("@model", MySqlDbType.VarChar).Value = textBox2.Text.ToString().Trim();
                command.Parameters.Add("@rokProd", MySqlDbType.Int16).Value = Convert.ToInt16(textBox3.Text.Trim());

                adapter.Fill(dtReg);

                if(dtReg.Rows.Count != 0)
                {
                    DataRow carInfo = dtReg.Rows[0];
                    marka = carInfo["MARKA"].ToString();
                    model = carInfo["MODEL"].ToString();
                    nrRej = textBox4.Text.ToString().Trim();
                    rokProd = carInfo["ROK_PROD"].ToString();
                    krajProd = carInfo["KRAJ_PROD"].ToString();
                    pojSil = carInfo["POJ_SIL"].ToString();
                    kosztDnia = carInfo["KOSZT_DNIA"].ToString();
                    typPrawa = carInfo["TYP_PRAWA_JAZDY"].ToString();

                    String maxValOfSpecCar = "SELECT MAX(s.ID_SAM + 1) FROM samochody s";
                    conn = new Connection();
                    dtReg = new DataTable();
                    adapter = new MySqlDataAdapter();
                    command = new MySqlCommand();

                    conn.OpenConnection();

                    command.CommandText = maxValOfSpecCar;
                    command.Connection = conn.GetConnection();
                    adapter.SelectCommand = command;
                    adapter.Fill(dtReg);

                    if(dtReg.Rows.Count != 0)
                    {
                        DataRow row = dtReg.Rows[0];
                        int maxID = Convert.ToInt16(row["MAX(s.ID_SAM + 1)"].ToString());
                        String insertCarToDataBase = "INSERT INTO `samochody`(`ID_SAM`, `NR_REJ`, `MARKA`, `MODEL`, `ROK_PROD`, `KRAJ_PROD`, `POJ_SIL`, `KOSZT_DNIA`, `TYP_PRAWA_JAZDY`, `INFO_USZKODZENIA`, `DOSTEPNOSC`, `ZDJECIE`) VALUES (@maxID, @nrRej, @marka, @model, @rokProd, @krajProd, @pojSil, @kosztDnia, 'B', NULL, 1, NULL)";
                        conn = new Connection();
                        dtReg = new DataTable();
                        adapter = new MySqlDataAdapter();
                        command = new MySqlCommand();

                        conn.OpenConnection();
                        command.CommandText = insertCarToDataBase;
                        command.Connection = conn.GetConnection();
                        command.Parameters.Add("@marka", MySqlDbType.VarChar).Value = marka;
                        command.Parameters.Add("@model", MySqlDbType.VarChar).Value = model;
                        command.Parameters.Add("@maxID", MySqlDbType.Int16).Value = maxID;
                        command.Parameters.Add("@nrRej", MySqlDbType.VarChar).Value = nrRej;
                        command.Parameters.Add("@krajProd", MySqlDbType.VarChar).Value = krajProd;
                        command.Parameters.Add("@rokProd", MySqlDbType.VarChar).Value = rokProd;
                        command.Parameters.Add("@pojSil", MySqlDbType.Double).Value = Convert.ToDouble(pojSil);
                        command.Parameters.Add("@kosztDnia", MySqlDbType.Int16).Value = Convert.ToInt16(kosztDnia);


                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Car added");
                            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
                        }
                    }
                    
                }
                else
                {
                    marka = textBox1.Text.Trim();
                    model = textBox2.Text.Trim();
                    rokProd = textBox3.Text.Trim();
                    Directory.CreateDirectory(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + marka + " " + model + " " + rokProd);
                    Directory.CreateDirectory(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + marka + " " + model + " " + rokProd);

                    string fileName = @"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + marka + " " + model + " " + rokProd+"/auto.txt";
                    string autoData = @"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + marka + " " + model + " " + rokProd + "/opis.txt";
                    FileStream fs = File.Create(autoData);

                    try
                    {
                        using (var sw = new StreamWriter(fileName))
                        {
                            for (var i = 0; i <= 2; i++)
                            {
                                if(i == 0)
                                {
                                    sw.WriteLine(marka);
                                }
                                if(i == 1)
                                {
                                    sw.WriteLine(model);
                                }
                                if(i == 2)
                                {
                                    sw.Write(rokProd);
                                }
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        
                    }

                    label6.Visible = label7.Visible = label8.Visible = label9.Visible = label10.Visible = label11.Visible = label12.Visible = label13.Visible = label14.Visible = true;
                    textBox5.Visible = textBox6.Visible = textBox7.Visible = textBox8.Visible = textBox9.Visible = textBox10.Visible = textBox11.Visible = textBox12.Visible = textBox13.Visible = true;
                    button3.Visible = true;
                    button1.Visible = false;

                }


                conn.CloseConnection();
            }
            else
            {
                MessageBox.Show("Fill all gaps!");
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "")
            {
                String maxValOfSpecCar = "SELECT MAX(s.ID_SAM + 1) FROM samochody s";

                Connection conn = new Connection();
                DataTable dtReg = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();

                conn = new Connection();
                dtReg = new DataTable();
                adapter = new MySqlDataAdapter();
                command = new MySqlCommand();

                conn.OpenConnection();

                command.CommandText = maxValOfSpecCar;
                command.Connection = conn.GetConnection();
                adapter.SelectCommand = command;
                adapter.Fill(dtReg);

                if (dtReg.Rows.Count != 0)
                {
                    List<String> carData = new List<String>();
                    DataRow row = dtReg.Rows[0];
                    int maxID = Convert.ToInt16(row["MAX(s.ID_SAM + 1)"].ToString());
                    String insertCarToDataBase = "INSERT INTO `samochody`(`ID_SAM`, `NR_REJ`, `MARKA`, `MODEL`, `ROK_PROD`, `KRAJ_PROD`, `POJ_SIL`, `KOSZT_DNIA`, `TYP_PRAWA_JAZDY`, `INFO_USZKODZENIA`, `DOSTEPNOSC`, `ZDJECIE`) VALUES (@maxID, @nrRej, @marka, @model, @rokProd, @krajProd, @pojSil, @kosztDnia, 'B', NULL, 1, NULL)";
                    conn = new Connection();
                    dtReg = new DataTable();
                    adapter = new MySqlDataAdapter();
                    command = new MySqlCommand();

                    conn.OpenConnection();
                    command.CommandText = insertCarToDataBase;
                    command.Connection = conn.GetConnection();
                    command.Parameters.Add("@marka", MySqlDbType.VarChar).Value = textBox1.Text.ToString(); carData.Add("Marka: " + textBox1.Text.ToString());
                    command.Parameters.Add("@model", MySqlDbType.VarChar).Value = textBox2.Text.ToString(); carData.Add("Model: " + textBox2.Text.ToString());
                    command.Parameters.Add("@maxID", MySqlDbType.Int16).Value = maxID;
                    command.Parameters.Add("@nrRej", MySqlDbType.VarChar).Value = textBox4.Text.ToString();
                    command.Parameters.Add("@krajProd", MySqlDbType.VarChar).Value = textBox7.Text.ToString();  carData.Add("Kraj produkcji: " + textBox7.Text.ToString());
                    command.Parameters.Add("@rokProd", MySqlDbType.VarChar).Value = textBox3.Text.ToString();   carData.Add("Rok produkcji: " + textBox3.Text.ToString());
                    command.Parameters.Add("@pojSil", MySqlDbType.Double).Value = Convert.ToDouble(textBox6.Text);  carData.Add("Pojemnosc skokowa: " + textBox6.Text.ToString());
                    command.Parameters.Add("@kosztDnia", MySqlDbType.Int16).Value = Convert.ToInt16(textBox5.Text); carData.Add("Dzienny koszt: " + textBox5.Text.ToString());


                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Car added");
                        string autoData = @"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + textBox1.Text.ToString().Trim() + " " + textBox2.Text.ToString().Trim() + " " + textBox3.Text.ToString().Trim() + "/opis.txt";
                        
                        try
                        {
                            using (var writer = new StreamWriter(autoData))
                            {
                                for (var i = 0; i <= carData.Count; i++)
                                {
                                    writer.WriteLine(carData[i]);
                                }
                            }

                        }
                        catch (Exception Ex)
                        {

                        }

                    }

                    textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
                    conn.CloseConnection();
                    admin ad = new admin();
                    ad.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Check if you filled all necessary gaps:\n- Marka auta\n- Model auta\n- Rok produkcji\n- Kraj produkcji\n- Numer rejestracyjny\n- Pojemność silnika\n- Dzienny koszt");
            }
        }
    }
}
