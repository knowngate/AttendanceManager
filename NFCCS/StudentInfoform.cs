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
using System.IO;

namespace NFCCS
{
    public partial class StudentInfoform : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        DDSTamaEncoder.TamaWriter nfcReader = null;
        bool RFIDConnected = false;
        NFChandler nfcandler = new NFChandler();
        string filepath = null;
        int M_SCW = 0;
        int Second = 0;
        string B1Str = "";
        public StudentInfoform()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StudentInfo_Load(object sender, EventArgs e)
        {
            Image ima = Image.FromFile(staticinfo.imgpat);
            pictureBox2.Image = ima;
            label18.Text = staticinfo.name;
            label17.Text = staticinfo.addr;
            label1.Text = staticinfo.contact;
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
            //label2.Text = DateTime.Now.ToString();

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
                        conn = connObj.getconnection();
                        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                        synthesizer.Volume = 100;  // 0...100
                        synthesizer.Rate = -2;

                        nfcReader.BlockNo = Convert.ToSByte(comboBox1.Text);
                        B1Str = nfcReader.ReadData();
                        textBox1.Text = B1Str;
                        string[] arr = { textBox1.Text };
                        bool b = nfcandler.isvalidData(arr);
                        if (b)
                        {
                            synthesizer.SpeakAsync("Wellcome yogesh");
                            //Image im = Image.FromFile("C:\\Users\\Public\\Pictures\\Sample Pictures\\Chrysanthemum.jpg");
                            //pictureBox1.Image = im;
                            //using (SoundPlayer player = new SoundPlayer("C://Data//bass.wav"))
                            //{
                            //    // Use PlaySync to load and then play the sound.
                            //    // ... The program will pause until the sound is complete.
                            //    player.PlaySync();
                            //}
                            string selequery = "select idnumber,fname,mname,lname,mobile,landline,age,sex,address"
                         + ",address2,class,batch,course,type,createdon,filepat from studenttab where idnumber='" + textBox1.Text + "' ";

                            DataSet ds = connObj.selectRecords(conn, selequery);
                            for (int jo = 0; jo < ds.Tables[0].Rows.Count; jo++)
                            {
                                //string path = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("bin"));
                                
                                string file = ds.Tables[0].Rows[jo]["filepat"].ToString();
                                //path = path + "NFCIMAGES\\student\\" + file;
                                try
                                {
                                    string palettesPath = @"C:\Images\student\" + file;
                                    Image ima = Image.FromFile(palettesPath);
                                    pictureBox1.Image = ima;
                                }
                                catch { }
                                
                                textBox1.Text = ds.Tables[0].Rows[jo]["idnumber"].ToString();
                                txtname.Text = ds.Tables[0].Rows[jo]["fname"].ToString();
                                txtmname.Text = ds.Tables[0].Rows[jo]["mname"].ToString();
                                txtlname.Text = ds.Tables[0].Rows[jo]["lname"].ToString();
                                txtmobile.Text = ds.Tables[0].Rows[jo]["mobile"].ToString();
                                //txtmobile2.Text = ds.Tables[0].Rows[jo]["mobile2"].ToString();
                                txtlandline.Text = ds.Tables[0].Rows[jo]["landline"].ToString();
                                cmbage.Text = ds.Tables[0].Rows[jo]["age"].ToString();
                                cmbsex.Text = ds.Tables[0].Rows[jo]["sex"].ToString();
                                txtaddress.Text = ds.Tables[0].Rows[jo]["address"].ToString();
                                txtaddress1.Text = ds.Tables[0].Rows[jo]["address2"].ToString();
                                cmbclass.Text = ds.Tables[0].Rows[jo]["class"].ToString();
                                cmbbatch.Text = ds.Tables[0].Rows[jo]["batch"].ToString();
                                cmbcourse.Text = ds.Tables[0].Rows[jo]["course"].ToString();
                                
                            }
                        }
                        timer1.Interval = 10;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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

        private void btnsave_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
