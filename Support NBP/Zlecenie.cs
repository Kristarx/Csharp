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
using MySql.Data.MySqlClient;

namespace SupportNBP
{
    public partial class Zlecenie : Form
    {
        public Zlecenie()
        {
            InitializeComponent();
            Init();
        }

        private String getAccNum()
        {
            String AccNum = "";
            String AccNumReq = "SELECT NrKonta FROM klient WHERE id_kli = @ID";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            DataTable dtReg2 = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = AccNumReq;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@ID", MySqlDbType.VarChar).Value = Form1.gimme();
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                AccNum = row["NrKonta"].ToString();
            }

            conn.CloseConnection();

            return AccNum;
        }

        private void Init()
        {
            String AccNum = "";
            String getCur = "SELECT * FROM konto WHERE NrKonta = @AccNum";
            String PLN = "", EUR = "", USD = "", GBP = "";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            AccNum = getAccNum();

            msc.CommandText = getCur;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@AccNum", MySqlDbType.VarChar).Value = AccNum;
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                PLN = row["SaldoPLN"].ToString();
                EUR = row["SaldoEUR"].ToString();
                USD = row["SaldoUSD"].ToString();
                GBP = row["SaldoGBP"].ToString();
            }

            textBox1.Text = textBox2.Text = textBox3.Text = "";
            checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = checkBox4.Checked = false;
            checkBox5.Checked = checkBox6.Checked = checkBox7.Checked = checkBox8.Checked = false;
            label7.Text = "PLN: " + PLN;
            label8.Text = "EUR: " + EUR;
            label9.Text = "USD: " + USD;
            label10.Text = "GBP: " + GBP;

            conn.CloseConnection();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            cl.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ifNum = 0;
            String amount = textBox3.Text;

            if (amount.Equals(""))
            {
                MessageBox.Show("Write the amount!");
                ifNum++;
                textBox3.Text = "";
            }
            else
            {
                for(int i = 0; i < amount.Length; i++)
                {
                    if(Convert.ToInt16(amount[i]) < 48 || Convert.ToInt16(amount[i]) > 57)
                    {
                        ifNum++;
                        break;
                    }
                }

                if (ifNum == 0)
                {
                    String insPLN = "UPDATE `konto` SET `SaldoPLN`= @Amount WHERE NrKonta = @AccNum";
                    String getAmount = "SELECT * FROM konto WHERE NrKonta = @AccNum";
                    int prevCash = 0;

                    Connection conn = new Connection();
                    DataTable dtReg = new DataTable();
                    DataTable dtReg2 = new DataTable();
                    MySqlDataAdapter msda = new MySqlDataAdapter();
                    MySqlCommand msc = new MySqlCommand();

                    conn.OpenConnection();

                    msc.CommandText = getAmount;
                    msc.Connection = conn.GetConnection();
                    msc.Parameters.Add("@AccNum", MySqlDbType.VarChar).Value = getAccNum();
                    msda.SelectCommand = msc;
                    msda.Fill(dtReg2);

                    if (dtReg2.Rows.Count != 0)
                    {
                        DataRow row = dtReg2.Rows[0];
                        prevCash = Convert.ToInt16(row["SaldoPLN"]);
                    }

                    msc.CommandText = insPLN;
                    msc.Connection = conn.GetConnection();
                    msc.Parameters.Add("@Amount", MySqlDbType.Int16).Value = prevCash + Convert.ToInt16(amount);
                    

                    if ((msc.ExecuteNonQuery() == 1))
                    {
                        conn.CloseConnection();
                        label7.Text = "PLN: " + (prevCash + Convert.ToInt16(amount)).ToString();
                        MessageBox.Show("Successful!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    textBox3.Text = "";
                    conn.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Check the value!");
                    textBox3.Text = "";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true || checkBox3.Checked == true || checkBox4.Checked == true || checkBox8.Checked == true)
            {
                checkBox1.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true || checkBox3.Checked == true || checkBox4.Checked == true || checkBox7.Checked == true)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true || checkBox2.Checked == true || checkBox4.Checked == true || checkBox6.Checked == true)
            {
                checkBox3.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true || checkBox2.Checked == true || checkBox3.Checked == true || checkBox5.Checked == true)
            {
                checkBox4.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true || checkBox6.Checked == true || checkBox5.Checked == true || checkBox1.Checked == true)
            {
                checkBox8.Checked = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true || checkBox6.Checked == true || checkBox5.Checked == true || checkBox2.Checked == true)
            {
                checkBox7.Checked = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true || checkBox7.Checked == true || checkBox5.Checked == true || checkBox3.Checked == true)
            {
                checkBox6.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true || checkBox7.Checked == true || checkBox6.Checked == true || checkBox4.Checked == true)
            {
                checkBox5.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ifNum = 0;
            String amount = textBox1.Text;

            if (amount.Equals(""))
            {
                MessageBox.Show("Write the amount!");
                ifNum++;
                textBox1.Text = "";
            }
            else
            {
                for (int i = 0; i < amount.Length; i++)
                {
                    if (Convert.ToInt16(amount[i]) < 48 || Convert.ToInt16(amount[i]) > 57)
                    {
                        ifNum++;
                        break;
                    }
                }

                if (ifNum == 0)
                {
                    String CurFrom = "", CurTo = "";

                    if ((checkBox1.Checked == true || checkBox2.Checked == true || checkBox3.Checked == true || checkBox4.Checked == true) &&
                       (checkBox5.Checked == true || checkBox6.Checked == true || checkBox7.Checked == true || checkBox8.Checked == true))
                    {
                        CurFrom = CurrFrom();
                        CurTo = CurrTo();


                        String whichCurrAmout = "SELECT * FROM konto WHERE NrKonta = @AccNum";
                        String WymianaUpdate = "UPDATE `konto` SET Saldo" + CurFrom.Trim() + " = @FirstCurr, Saldo" + CurTo.Trim() + " = @SecondCurr  WHERE NrKonta = @AccNum";

                        int prevFirstCash = 0, prevSecondCash = 0;

                        Connection conn = new Connection();
                        DataTable dtReg = new DataTable();
                        MySqlDataAdapter msda = new MySqlDataAdapter();
                        MySqlCommand msc = new MySqlCommand();

                        conn.OpenConnection();

                        msc.CommandText = whichCurrAmout;
                        msc.Connection = conn.GetConnection();
                        msc.Parameters.Add("@AccNum", MySqlDbType.VarChar).Value = getAccNum();
                        msda.SelectCommand = msc;
                        msda.Fill(dtReg);


                        if (dtReg.Rows.Count != 0)
                        {
                            DataRow row = dtReg.Rows[0];
                            prevFirstCash = Convert.ToInt16(row["Saldo"+ CurFrom.Trim()]);
                            prevSecondCash = Convert.ToInt16(row["Saldo" + CurTo.Trim()]);
                        }

                        if(prevFirstCash < Convert.ToInt16(textBox1.Text))
                        {
                            MessageBox.Show("Not enough amout!");
                        }
                        else
                        {
                            double realVal = Convert.ToInt16(textBox1.Text);
                            realVal *= 0.95;
                            msc.CommandText = WymianaUpdate;
                            msc.Connection = conn.GetConnection();
                            msc.Parameters.Add("@FirstCurr", MySqlDbType.Int16).Value = prevFirstCash - Convert.ToInt16(textBox1.Text);
                            msc.Parameters.Add("@SecondCurr", MySqlDbType.Int16).Value = prevSecondCash + realVal * retCourse(CurFrom, CurTo);
                            writeTransaction();

                            if ((msc.ExecuteNonQuery() == 1))
                            {
                                conn.CloseConnection();
                                MessageBox.Show("Successful!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Init();
                            }
                        }
                        textBox3.Text = "";
                        conn.CloseConnection();

                        textBox1.Text = "";
                        checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = checkBox4.Checked = false;
                        checkBox5.Checked = checkBox6.Checked = checkBox7.Checked = checkBox8.Checked = false;
                    }
                    else
                    {
                        MessageBox.Show("Not all boxes checked!");
                    }
                }
                else
                {
                    MessageBox.Show("Check the value!");
                    textBox1.Text = "";
                }
            }
        }



        private String CurrFrom()
        {
            String CurFrom = "";

            if (checkBox1.Checked == true)
            {
                CurFrom = checkBox1.Text;
            }
            else if (checkBox2.Checked == true)
            {
                CurFrom = checkBox2.Text;
            }
            else if (checkBox3.Checked == true)
            {
                CurFrom = checkBox3.Text;
            }
            else if (checkBox4.Checked == true)
            {
                CurFrom = checkBox4.Text;
            }

            return CurFrom;
        }



        private String CurrTo()
        {
            String CurTo = "";

            if (checkBox5.Checked == true)
            {
                CurTo = checkBox5.Text;
            }
            else if (checkBox6.Checked == true)
            {
                CurTo = checkBox6.Text;
            }
            else if (checkBox7.Checked == true)
            {
                CurTo = checkBox7.Text;
            }
            else if (checkBox8.Checked == true)
            {
                CurTo = checkBox8.Text;
            }

            return CurTo;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = (Convert.ToInt16(Convert.ToDouble(textBox1.Text) * 0.95 * retCourse(CurrFrom(), CurrTo()))).ToString();
            }
            catch
            {
                textBox2.Text = "";
            }
            
        }

        private double retCourse(String firstCur, String secondCur)
        {
            XmlDocument waluty = new XmlDocument();
            double first = 0, second = 0;

            if(firstCur.Equals("PLN"))
            {
                first = 1;
            }
            if(secondCur.Equals("PLN"))
            {
                second = 1;
            }

            bool ifGoodDate = false;
            int sub = -1;

            while(!ifGoodDate)
            {
                try
                {
                    sub++;
                    waluty.Load(@"Kursy\"+ ChangeData(sub) + ".xml");  
                    XmlNodeList spis_walut = waluty.GetElementsByTagName("pozycja");

                    foreach (XmlNode data in spis_walut)
                    {
                        if (data.LastChild.PreviousSibling.InnerText.Equals(firstCur))
                        {
                            first = Convert.ToDouble(data.LastChild.InnerText);
                        }

                        if (data.LastChild.PreviousSibling.InnerText.Equals(secondCur))
                        {
                            second = Convert.ToDouble(data.LastChild.InnerText);
                        }
                    }

                    ifGoodDate = true;
                    return first / second;
                }
                catch
                {
                    
                }
            }

            return 0;
        }

        private string ChangeData(int i)
        {
            string date = DateTime.Today.AddDays(-i).ToString();
            date = date.Substring(2).Replace("00:00:00", "").Replace("-", "").Trim();
            return date;
        }

        private void writeTransaction()
        {
            String MaxValue = "SELECT MAX(NrTransakcji+1) FROM transakcja";
            String AccNum = "SELECT NrKonta FROM klient WHERE id_kli = @ID";
            String InsertValues = "INSERT INTO `transakcja` VALUES (@NrTransakcji, @NrKonta, @IdKlienta, @firstCur, @howManyFirst, @secCur, @howManySec, @Date)";
            int maxVal = 0;

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            DataTable dtReg2 = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = MaxValue;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                try
                {
                    maxVal = Convert.ToInt16(row["MAX(NrTransakcji+1)"]);
                }
                catch
                {
                    maxVal = 1;
                }
            }

            msc.CommandText = AccNum;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@ID", MySqlDbType.Int16).Value = Form1.gimme();

            msda.SelectCommand = msc;

            msda.Fill(dtReg2);

            if (dtReg2.Rows.Count != 0)
            {
                DataRow row = dtReg2.Rows[0];
                AccNum = row["NrKonta"].ToString();
            }

            msc.CommandText = InsertValues;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@NrTransakcji", MySqlDbType.Int16).Value = maxVal;
            msc.Parameters.Add("@NrKonta", MySqlDbType.VarChar).Value = AccNum;
            msc.Parameters.Add("@IdKlienta", MySqlDbType.Int16).Value = Form1.gimme();
            msc.Parameters.Add("@firstCur", MySqlDbType.VarChar).Value = CurrFrom();
            msc.Parameters.Add("@howManyFirst", MySqlDbType.Int16).Value = Convert.ToInt16(textBox1.Text);
            msc.Parameters.Add("@secCur", MySqlDbType.VarChar).Value = CurrTo();
            msc.Parameters.Add("@howManySec", MySqlDbType.Int16).Value = textBox2.Text;
            msc.Parameters.Add("@Date", MySqlDbType.Date).Value = DateTime.Today;

            if (msc.ExecuteNonQuery() == 1)
            {
                conn.CloseConnection();
            }
        }
    }
}
