using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Speech.Synthesis;
using System.Net;

namespace NFCCS
{
    public partial class Attendance : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        DDSTamaEncoder.TamaWriter nfcReader = null;
        bool RFIDConnected = false;
        NFChandler nfcandler = new NFChandler();
        string filepath = null;
        double totalamount1 = 0;
        int M_SCW = 0;
        int Second = 0;
        string B1Str = "";
        public Attendance()
        {
            InitializeComponent();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            label5.Text = new DateTime().ToString();
            Image ima = Image.FromFile(staticinfo.imgpat);
            pictureBox2.Image = ima;
            label10.Text = staticinfo.name;
            label9.Text = staticinfo.addr;
            label8.Text = staticinfo.contact;
            int i = 1;
            while (i > 16)
            {
                comboBox1.Items.Add(i);
                i = i + 1;
            }

            Connect_RFID_Device();
            timer1.Interval = 1000;
            timer1.Start();
            comboBox1.SelectedIndex = 3;
        }
        private Boolean Connect_RFID_Device()
        {
            try
            {
                nfcReader = new DDSTamaEncoder.TamaWriter();
                M_SCW = nfcReader.Find_NFC_Reader();
                if (M_SCW > 0)
                {
                    RFIDConnected = true;
                    return true;
                }
                else
                {
                    RFIDConnected = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                RFIDConnected = false;
                return false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString();

            Second = Second + 1;
            if (Second >= 2)
            {
                timer1.Stop();
                // button2_Click(sender, e);
                readData();
                //MsgBox("Timer Stopped....")
            }
        }
        private void readData()
        {
            if (RFIDConnected)
            {
                if (Check_Card_Present() == false)
                {
                    //MessageBox.Show("Card not present!");
                    // break;
                }
                else
                {
                    timer1.Enabled = false;
                    try
                    {
                        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                        synthesizer.Volume = 100;  // 0...100
                        synthesizer.Rate = -2;

                        nfcReader.BlockNo = Convert.ToSByte(comboBox1.Text);
                        B1Str = nfcReader.ReadData();
                        textBox1.Text = B1Str;

                       

                        //using (SoundPlayer player = new SoundPlayer("C://Data//bass.wav"))
                        //{
                        //    // Use PlaySync to load and then play the sound.
                        //    // ... The program will pause until the sound is complete.
                        //    player.PlaySync();
                        //}
                        bool stdexist = false;
                        string selstdinfo = "select fname,mname,lname,class,course,filepat,mobile from studenttab where idnumber='" + textBox1.Text + "' ";
                        conn = connObj.getconnection();
                        DataSet ds = connObj.selectRecords(conn, selstdinfo);
                        string mobile = "";
                        string msg = "";
                        string name = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            stdexist = true;
                            string file = ds.Tables[0].Rows[0]["filepat"].ToString();
                            mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
                            if (file != null && file != "")
                            {
                                string palettesPath = @"C:\Images\student\" + file;
                                Image ima = Image.FromFile(palettesPath);
                                pictureBox1.Image = ima;
                            }
                            
                             name = ds.Tables[0].Rows[0]["fname"].ToString() + "   " + ds.Tables[0].Rows[0]["lname"].ToString();
                            lblstdname.Text = name;
                            synthesizer.SpeakAsync(name);
                            lblclass.Text = ds.Tables[0].Rows[0]["class"].ToString();
                            lblcourse.Text = ds.Tables[0].Rows[0]["course"].ToString();
                        }
                       int cnt= nfcandler.checkForCount(textBox1.Text);
                       string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                       string date = DateTime.Now.ToString("yyyy-MM-dd");
                       if (cnt == 0 && stdexist)
                       {
                           msg = name + " IN time " + time;
                           string etattendanceIn = " INSERT INTO attendance (cardno,cnt,intime,date) VALUES ('" + textBox1.Text + "',1,'" + time + "','" + date + "')";
                           connObj.insertRecords(conn, etattendanceIn);
                           string url = "http://bhashsms.com/api/sendmsg.php?user=alkhemacademy&pass=123456&sender=ALKHEM&phone=" + mobile + "&text=" + msg + "&priority=sdnd&stype=normal";
                           try
                           {
                               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                               HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                               MessageBox.Show("SMS sent succesfully.");
                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show(ex.Message);
                           }
                       }
                       else if (cnt == 1 && stdexist)
                       {
                           msg = name + " out time " + time;
                           string etattendanceOut = " UPDATE attendance SET cnt=2,outtime='" + time + "' WHERE cardno='" + textBox1.Text + "' AND date='" + date + "' ";
                           connObj.insertRecords(conn, etattendanceOut);
                           string url = "http://bhashsms.com/api/sendmsg.php?user=alkhemacademy&pass=123456&sender=ALKHEM&phone=" + mobile + "&text=" + msg + "&priority=sdnd&stype=normal";
                           try
                           {
                               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                               HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                               MessageBox.Show("SMS sent succesfully.");
                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show(ex.Message);
                           }
                       }

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                    }
                }
            }
            timer1.Enabled = true;
            timer1.Start();
        }
        private bool Check_Card_Present()
        {
            try
            {
                if (nfcReader.IsCardPresent())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

    }
}
