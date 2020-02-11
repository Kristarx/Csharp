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
    public partial class SeeSingleOffer : Form
    {
        public static int left = 0, middle = 0, right = 0, max = 0, koszt = 0;

        public SeeSingleOffer()
        {
            InitializeComponent();
            Init();
            getCarID();
            setPhotos();
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker2.MinDate = DateTime.Today;
            dateTimePicker1.MaxDate = new DateTime(DateTime.Today.Year+2, DateTime.Today.Month, DateTime.Today.Day);
            dateTimePicker2.MaxDate = new DateTime(DateTime.Today.Year+2, DateTime.Today.Month, DateTime.Today.Day);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SeeOffers so = new SeeOffers();
            this.Hide();
            so.Show();
        }

        private void Init()
        {
            List<String> Description = new List<string>();
            var lines = File.ReadLines(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " "+ SeeOffers.model + " " + SeeOffers.rok + "/opis.txt");

            foreach (var line in lines)
            {
                Description.Add(line);
            }
            listBox1.DataSource = Description;
        }

        

        private void setPhotos()
        {
            Bitmap img;
            DirectoryInfo d = new DirectoryInfo(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.bmp");
            string str = "";

            str = Files[0].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox1.Image = img;
            str = Files[1].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox2.Image = img;
            str = Files[2].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox3.Image = img;

            left = 0;
            middle = 1;
            right = 2;
            max = Files.Length;
        }

        private String maxIDVal()
        {
            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            String MaxValue = "SELECT MAX(ID_WYP+1) FROM wypozyczenia";
            conn.OpenConnection();

            msc.CommandText = MaxValue;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                MaxValue = row["MAX(ID_WYP+1)"].ToString();
            }

            conn.CloseConnection();
            return MaxValue;
        }

        private int checkIfCarAvailable()
        {
            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            String checkCarID = "SELECT s.ID_SAM from samochody s WHERE s.ID_SAM not in (SELECT ID_SAM FROM wypozyczenia w WHERE ((w.DATA_WYP<='" + dateTimePicker1.Value.ToString().Substring(0, 10) + "' AND w.DATA_ZWR>='" + dateTimePicker2.Value.ToString().Substring(0, 10) + "')) OR (w.DATA_WYP>= '" + dateTimePicker1.Value.ToString().Substring(0, 10) + "' AND w.DATA_WYP<= '" + dateTimePicker2.Value.ToString().Substring(0, 10) + "') OR (w.DATA_ZWR>= '" + dateTimePicker1.Value.ToString().Substring(0, 10) + "' AND w.DATA_ZWR<= '" + dateTimePicker2.Value.ToString().Substring(0, 10) + "') ) AND S.MARKA = '" + SeeOffers.marka + "' AND s.MODEL = '" + SeeOffers.model + "' AND S.ROK_PROD = " +SeeOffers.rok;
            conn.OpenConnection();

            msc.CommandText = checkCarID;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                int carID = Convert.ToInt16(row["ID_SAM"].ToString());
                conn.CloseConnection();
                return carID;
            }

            conn.CloseConnection();
            return 0;
        }
        
        private String getCarID()
        {
            String getID = @"SELECT MIN(ID_SAM), KOSZT_DNIA FROM samochody WHERE MARKA = '" + SeeOffers.marka + "' AND MODEL = '" + SeeOffers.model + "' AND ROK_PROD = " + SeeOffers.rok;// + " AND DOSTEPNOSC = 1";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = getID;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                getID = row["MIN(ID_SAM)"].ToString();
                koszt = Convert.ToInt16(row["KOSZT_DNIA"]);
            }

            conn.CloseConnection();
            return getID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int carID = checkIfCarAvailable();

            if (DateTime.Compare(dateTimePicker1.Value, dateTimePicker2.Value) >= 0)
            {
                MessageBox.Show("Choose correct date!");
            }
            else if (carID == 0)
            {
                MessageBox.Show("Car is not available at the moment");
            }
            else
            {
                String insertRes = @"INSERT INTO `wypozyczenia`(`ID_WYP`, `ID_SAM`, `ID_KLI`, `DATA_WYP`, `DATA_ZWR`, `KOSZT`) VALUES (" + maxIDVal() + ", " + carID + ", " + Form1.gimme() + ", '" + dateTimePicker1.Value.ToString().Substring(0, 10) + "', '" + dateTimePicker2.Value.ToString().Substring(0, 10) + "', " + totalCost().ToString() + ")";

                Connection conn = new Connection();
                MySqlDataAdapter msda = new MySqlDataAdapter();
                MySqlCommand msc = new MySqlCommand();
                conn.OpenConnection();
                msc.CommandText = insertRes;
                msc.Connection = conn.GetConnection();


                if (msc.ExecuteNonQuery() == 1)
                {
                    conn.CloseConnection();
                    MessageBox.Show("Rezerwacja zostala zalozona!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SeeOffers so = new SeeOffers();
                    so.Show();
                    this.Hide();
                }
            }
        }

        private int totalCost()
        {
            int cena = Convert.ToInt16((dateTimePicker2.Value - dateTimePicker1.Value).TotalDays) * koszt;
            return cena;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int cena = Convert.ToInt16((dateTimePicker2.Value - dateTimePicker1.Value).TotalDays) * koszt;
            if (cena >= 0)
            {
                textBox2.Text = cena.ToString();
            }
            else
            {
                textBox2.Text = "0";
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            int cena = Convert.ToInt16((dateTimePicker2.Value - dateTimePicker1.Value).TotalDays) * koszt;
            if(cena >= 0)
            {
                textBox2.Text = cena.ToString();
            }
            else
            {
                textBox2.Text = "0";
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            right++;
            if(right == max)
            {
                right = 0;
            }

            middle++;
            if(middle == max)
            {
                middle = 0;
            }

            left++;
            if(left == max)
            {
                left = 0;
            }

            Bitmap img;
            DirectoryInfo d = new DirectoryInfo(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.bmp");
            string str = "";

            str = Files[left].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox1.Image = img;
            str = Files[middle].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox2.Image = img;
            str = Files[right].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox3.Image = img;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            right--;
            if (right <= 0)
            {
                right = max-1;
            }

            middle--;
            if (middle <= 0)
            {
                middle = max-1;
            }

            left--;
            if (left <= 0)
            {
                left = max-1;
            }

            Bitmap img;
            DirectoryInfo d = new DirectoryInfo(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.bmp");
            string str = "";

            str = Files[left].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox1.Image = img;
            str = Files[middle].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox2.Image = img;
            str = Files[right].Name;
            img = new Bitmap(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + SeeOffers.marka + " " + SeeOffers.model + " " + SeeOffers.rok + "/" + str);
            pictureBox3.Image = img;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            /*SELECT s.ID_SAM from samochody s 
            WHERE s.ID_SAM not in (SELECT ID_SAM FROM wypozyczenia w
                       WHERE(w.DATA_WYP >= '2020-01-13' AND w.DATA_WYP <= '2020-01-16')
                       OR(w.DATA_ZWR >= '2020-01-13' AND w.DATA_ZWR <= '2020-01-16')
                       OR(w.DATA_WYP <= '2020-01-13' AND w.DATA_ZWR >= '2020-01-16') )  AND S.MARKA = 'Dodge' AND s.MODEL = 'Challenger' AND S.ROK_PROD = 2016 */
        }
    }
}
