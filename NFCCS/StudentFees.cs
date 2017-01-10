using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using MySql.Data.MySqlClient;

namespace NFCCS
{
    public partial class StudentFees : Form
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
        public StudentFees()
        {
            InitializeComponent();
        }

        private void StudentFees_Load(object sender, EventArgs e)
        {
            try
            {
                Image ima = Image.FromFile(staticinfo.imgpat);
                pictureBox2.Image = ima;
            }
            catch { }
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

                        synthesizer.SpeakAsync("Wellcome yogesh");
                        
                        //using (SoundPlayer player = new SoundPlayer("C://Data//bass.wav"))
                        //{
                        //    // Use PlaySync to load and then play the sound.
                        //    // ... The program will pause until the sound is complete.
                        //    player.PlaySync();
                        //}

                        string selstdinfo = "select fname,mname,lname,class,course,stdacc.prevbalance,stdacc.paid,stdacc.totalbalance,filepat from studenttab std inner join stdaccounting stdacc on stdacc.stdid=std.idnumber where stdacc.stdid='" + textBox1.Text + "' ";
                        conn=connObj.getconnection();
                        DataSet ds = connObj.selectRecords(conn, selstdinfo);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string file = ds.Tables[0].Rows[0]["filepat"].ToString();
                            try
                            {
                                string palettesPath = @"C:\Images\student\" + file;
                                Image ima = Image.FromFile(palettesPath);
                                pictureBox1.Image = ima;
                            }
                            catch { }
                                
                            string name = ds.Tables[0].Rows[0]["fname"].ToString() + "   " + ds.Tables[0].Rows[0]["lname"].ToString();
                            lblstdname.Text = name;
                            lblclass.Text=ds.Tables[0].Rows[0]["class"].ToString();
                            lblcourse.Text = ds.Tables[0].Rows[0]["course"].ToString();
                            //txtbalance.Text = ds.Tables[0].Rows[0]["totalbalance"].ToString();
                            //txtpaid.Text = ds.Tables[0].Rows[0]["paid"].ToString();
                            txtprbalance.Text = ds.Tables[0].Rows[0]["totalbalance"].ToString();
                            totalamount1 = Convert.ToDouble(ds.Tables[0].Rows[0]["totalbalance"].ToString());
                        }


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

        private void timer1_Tick_1(object sender, EventArgs e)
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

        private void txtpaid_TextChanged(object sender, EventArgs e)
        {
           
            double balance1 = 0;
            double paid1 = 0;
            string[] arr = { txtbalance.Text };
            if (nfcandler.isvalidData(arr))
            {
               // totalamount1 = Convert.ToDouble(txtbalance.Text);
                try
                {
                    paid1 = Convert.ToDouble(txtpaid.Text);
                    balance1 = totalamount1 - paid1;
                    txtbalance.Text = balance1.ToString();
                }
                catch {
                    txtpaid.Text = "";
                    MessageBox.Show("Invalid Data"); }
            }
        }

        private void btnsavefees_Click(object sender, EventArgs e)
        {
            try
            {
                conn = connObj.getconnection();
                double prevbl=Convert.ToDouble(txtprbalance.Text);
                 double paid=Convert.ToDouble(txtpaid.Text);
                 double bal=Convert.ToDouble(txtbalance.Text);
                string updateFees = "Update stdaccounting set prevbalance="+prevbl+",paid="+paid+",totalbalance="+bal+" where stdid='"+textBox1.Text+"'";
                connObj.insertRecords(conn, updateFees);
                cleardarta();
                MessageBox.Show("Data updated successfully");
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cleardarta()
        {
            txtbalance.Text = "0";
            txtpaid.Text = "0";
            txtprbalance.Text = "0";
            lblclass.Text = "";
            lblcourse.Text = "";
            lblstdname.Text = "";
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtprbalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtpaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

    }
}
