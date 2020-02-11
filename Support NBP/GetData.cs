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
using System.IO;
using System.Net;
using System.Diagnostics;

namespace SupportNBP
{
    public class GetData
    {
        public GetData()
        {

        }


        private static string DownloadString(string address)
        {
            string text;
            using (var client = new WebClient())
            {
                text = client.DownloadString(address);
            }
            return text;
        }


        private string BuildData(int counter, int days)
        {
            string adres = @"http://www.nbp.pl/kursy/xml/";
            string find = "";


            find = "";
            find = counter.ToString();
            if (counter < 10)
            {
                find = find.Insert(0, "a" + "00");
            }

            else if (counter < 100)
            {
                find = find.Insert(0, "a" + "0");
            }
            else
            {
                find = find.Insert(0, "a");
            }

            adres += find + "z" + ChangeData(days);
            return adres;
        }


        public bool CheckConnection()
        {
            try
            {
                using (var connection = new WebClient())
                using (connection.OpenRead("http://google.com"))
                    return true;
            }
            catch
            {
                return false;
            }

        }


        public int ReturnDiffDays()
        {
            DateTime start = new DateTime(2020, 1, 1);
            double diff = (DateTime.Today - start).TotalDays;
            return (int)diff;
        }


        private string ChangeData(int i)
        {
            int diff = ReturnDiffDays() - i;
            string date = DateTime.Today.AddDays(-diff).ToString();
            date = date.Substring(2).Replace("00:00:00", "").Replace("-", "").Trim();
            return date;
        }


        public int DownloadFromInternet(int i, int count)
        {
            int counter = count;

            try
            {
                string date = @BuildData(counter, i).Trim() + ".xml";
                counter++;
                var xml = DownloadString(date);
                File.WriteAllText(@"Kursy\" + ChangeData(i).ToString().Trim() + ".xml", xml);
                return counter;
            }

            catch
            {
                counter--;
                return counter;
            }
        }
    }
}
