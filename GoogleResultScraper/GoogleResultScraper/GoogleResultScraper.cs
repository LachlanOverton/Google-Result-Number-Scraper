using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.Diagnostics;

namespace WindowsFormsApplication1
{

    /*
     TODO: 
     Add ability to choose text files through UI and store results properly
     Add comments
         */
    public partial class zGoogleResultScraper : Form
    {
        public zGoogleResultScraper()
        {
            InitializeComponent();
            //everything();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            everything();
        }

        void everything()
        {
            //request string
            string searchterms = "https://www.google.com.au/search?q=";
            //Loads search terms into an array
            string[] lines = System.IO.File.ReadAllLines(@"SearchList.txt");
            string webreq;
            string getpage = "";
            int i = 0;
            //previous results need to be saved.
            string[] result = new string[lines.Length];
            string[] count = new string[1];
            count[0] = "0";
            //how many completed requests there were last time
            count = System.IO.File.ReadAllLines(@"count.txt");


            i = Int32.Parse(count[0].ToString());
            if (i == 0) { System.IO.File.WriteAllLines(@"write.txt", result);}
            string[] re = new string[lines.Length];
            string[] temp = System.IO.File.ReadAllLines(@"write.txt");
            //if (i != 0)
            //{
                for (int x = 0; x <= temp.Length - 1;)
                {
                    re[x] = temp[x];
                    x++;
                }
            //}
            
            
            var wb = new WebBrowser();

            using (WebClient wc = new Scraper())
            {
                for (int b; i <= lines.Length-1;)
                {
                    webreq = searchterms + "" + lines[i];
                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                    try
                    {
                        getpage = wc.DownloadString(webreq);
                        
                        document.LoadHtml(getpage);
                        re[i] = document.GetElementbyId("resultStats").InnerText;
    
                    /*
                     * Working!
                     */
                    

                    //About 9,440,000 results
                    string a = re[i].Remove(0, 6);

                    re[i] = lines[i] + " " + a;
                    //9,440,000 results
                    System.IO.File.WriteAllLines(@"write.txt", re);
                    count[0] = i.ToString();
                    System.IO.File.WriteAllLines(@"count.txt", count);
                    i++;
                    }
                    catch (System.Net.WebException)
                    {

                        //run the program again and close this one
                       Process.Start(Application.StartupPath + "\\WindowsFormsApplication1.exe");
                            //or you can use Application.ExecutablePath
                          
                            //close this one
                            Process.GetCurrentProcess().Kill();
                        
                    }
                }



            }
            count[0] = "0";
            System.IO.File.WriteAllLines(@"count.txt", count);
            System.Media.SystemSounds.Beep.Play();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.OpenFile();
        }
    }

}
