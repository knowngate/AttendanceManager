using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Speech.Synthesis;
using MySql.Data.MySqlClient;
using System.IO;


namespace NFCCS
{
    public partial class Form1 : Form
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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (Connect_RFID_Device() == true)
                {
                    label1.Text = "RFID Device Found on COM Port : " + M_SCW;
                }
                else
                {
                    label1.Text = "RDIF Device not found!";
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
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


            DataSet ds = nfcandler.getAllBatces();
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                cmbbatch.Items.Add(ds.Tables[0].Rows[j][0].ToString());
            }
            DataSet ds1 = nfcandler.getAllClasses();
            for (int j1 = 0; j1 < ds1.Tables[0].Rows.Count; j1++)
            {
                cmbclass.Items.Add(ds1.Tables[0].Rows[j1][0].ToString());
            }
            DataSet ds2 = nfcandler.getAllCourses();
            for (int j2 = 0; j2 < ds2.Tables[0].Rows.Count; j2++)
            {
                cmbcourse.Items.Add(ds2.Tables[0].Rows[j2][0].ToString());
            }

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

        private void button2_Click(object sender, EventArgs e)
        {

            if (RFIDConnected)
            {
                if (Check_Card_Present() == false)
                {
                    //MessageBox.Show("Card not present!");
                    // break;
                }
                try
                {
                    nfcReader.BlockNo = Convert.ToSByte(comboBox1.Text);
                    B1Str = nfcReader.ReadData();
                    textBox1.Text = B1Str;
                }
                catch (Exception ex)
                {

                }
            }

            timer1.Start();
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

                        synthesizer.SpeakAsync("Wellcome yogesh");
                        Image im = Image.FromFile(filepath);
                        pictureBox1.Image = im;
                        //using (SoundPlayer player = new SoundPlayer("C://Data//bass.wav"))
                        //{
                        //    // Use PlaySync to load and then play the sound.
                        //    // ... The program will pause until the sound is complete.
                        //    player.PlaySync();
                        //}
                    }
                    catch (Exception ex)
                    {

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

        private void btnbrows_Click(object sender, EventArgs e)
        {
            try
            {
                 openFileDialog1.ShowDialog();
                 filepath = openFileDialog1.FileName;
                 Image im = Image.FromFile(filepath);
                 pictureBox1.Image = im;
                 //char[] flname = filepath.Split(".");
                 //if (flname.Length > 1)
                 //{
                 //    filepath = flname[1].ToString();
                 //}
                 //string appPath = Application.StartupPath;
                 //string dbPath = @"\NFCIMAGES";

                 //string fullpath = appPath + @"\NFCIMAGES\student\" + filepath;
                 //MessageBox.Show(fullpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                 conn = connObj.getconnection();
                 string[] arr = {textBox1.Text,txtaddress.Text,txtlname.Text,txtmname.Text,txtmobile.Text,txtname.Text,cmbclass.Text,cmbcourse.Text };
                 bool vl=nfcandler.isvalidData(arr);
                 if (vl)
                 {
                     string imagepath = filepath;
                     string picname = imagepath.Substring(imagepath.LastIndexOf('\\'));
                     //string path = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("bin"));
                     //Bitmap imgImage = new Bitmap(pictureBox1.Image);    
                     //imgImage.Save(path + "\\NFCIMAGES\\student\\" + picname + ".jpg");

                     try
                     {
                         string extension = Path.GetExtension(imagepath);
                         string palettesPath = "C:\\Images\\student\\";
                         // If the directory doesn't exist, create it.
                         if (!Directory.Exists(palettesPath))
                         {
                             Directory.CreateDirectory(palettesPath);
                         }
                         string subPath = palettesPath + picname ;
                         Bitmap imgImage = new Bitmap(pictureBox1.Image);    //Create an object of Bitmap class/
                         imgImage.Save(subPath + ".jpg");
                     }
                     catch (Exception)
                     {
                         // Fail silently
                     }
                 

                     string dDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                     int age = Convert.ToInt32(cmbage.Text);
                     if (nfcandler.checkForExist(textBox1.Text) == 0)
                     {
                         string insert = "insert into studenttab (idnumber,fname,mname,lname,mobile,landline,age,sex,address"
                         + ",address2,class,batch,course,type,createdon,filepat) values ('" + textBox1.Text + "','" + txtname.Text + "','" + txtmname.Text + "','" + txtlname.Text + "',"
                         + "'" + txtmobile.Text + "','" + txtlandline.Text + "'," + age + ",'" + cmbsex.Text + "','" + txtaddress.Text + "','" + txtaddress1.Text + "','" + cmbclass.Text + "'"
                         + ",'" + cmbbatch.Text + "','" + cmbcourse.Text + "','','" + dDate + "','" + picname + "')";
                         connObj.insertRecords(conn, insert);

                         double fees = nfcandler.getCourseFee(conn, cmbcourse.Text);

                         string insrtcoursefee = "insert into stdaccounting (totalbalance,stdid) values(" + fees + ",'" + textBox1.Text + "')";
                         connObj.insertRecords(conn, insrtcoursefee);

                         clearaldata();
                         MessageBox.Show("Student added successfully..");
                     }
                     else
                     {
                         MessageBox.Show("Student already exist for this card..");
                     }
                 }
                 else
                 {
                     MessageBox.Show("please enter valid data");
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearaldata()
        {
            txtaddress.Text = "";
            txtaddress1.Text = "";
            txtlandline.Text = "";
            txtlname.Text = "";
            txtmname.Text = "";
            txtmobile.Text = "";
            txtmobile2.Text = "";
            txtname.Text = "";
            cmbage.Text = "0";
            cmbbatch.Text = "";
            cmbclass.Text = "";
            cmbcourse.Text = "";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtmobile2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtlandline_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
