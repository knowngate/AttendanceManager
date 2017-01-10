using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MySql.Data.MySqlClient;

namespace NFCCS
{
    public partial class ManualSms : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        NFChandler nfcandler = new NFChandler();
        string filepath = null;
        public ManualSms()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mobileno = txtmobileno.Text;
            string unm = txtunm.Text;
            string pass = txtpass.Text;
            string smstype = cmbsmstype.Text;
            string smspriority = cmbsmspriority.Text;
            string senderid = cmbsenderid.Text;
            string msg = textBox1.Text;
            string url = "http://bhashsms.com/api/sendmsg.php?user=alkhemacademy&pass=123456&sender=ALKHEM&phone=" + mobileno + "&text=" + msg + "&priority=sdnd&stype=normal";
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

        private void ManualSms_Load(object sender, EventArgs e)
        {
            try
            {
                Image ima = Image.FromFile(staticinfo.imgpat);
                pictureBox1.Image = ima;
                label8.Text = staticinfo.name;
                label7.Text = staticinfo.addr;
                label6.Text = staticinfo.contact;
            }
            catch { }
            try
            {
                MySqlConnection conn = connObj.getconnection();
                string seledistAndtal = "select classname from class order by classname ";
                DataSet ds = connObj.selectRecords(conn, seledistAndtal);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBox2.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch { }
        }

        private void txtmobileno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection conn = connObj.getconnection();
                string seledistAndtal = "select fname,mname,lname,mobile from studenttab where class='" + comboBox2.Text + "' AND batch='"+comboBox1.Text+"' order by fname ";
                DataSet ds = connObj.selectRecords(conn, seledistAndtal);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = ds.Tables[0].Rows[i]["fname"].ToString() + "  " + ds.Tables[0].Rows[i]["mname"].ToString() + "  " + ds.Tables[0].Rows[i]["lname"].ToString();
                    string number = ds.Tables[0].Rows[i]["mobile"].ToString();
                    dataGridView1.Rows.Add(false,name,number);
                }
            }
            catch { }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection conn = connObj.getconnection();
                string seledistAndtal = "select name from batch where class='"+comboBox2.Text+"' order by name ";
                DataSet ds = connObj.selectRecords(conn, seledistAndtal);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch { }
        }

        private void label11_Click(object sender, EventArgs e)
        {

            txtmobileno.Text = "";
            if (dataGridView1.Rows.Count >= 1)
            {
                string orderNo = "";
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string s = Convert.ToString(dataGridView1[0, j].Value);
                    if (s == "True")
                    {
                        if (j == 0)
                        {
                             orderNo = dataGridView1[2, j].Value.ToString();
                        }
                        else
                        {
                            orderNo += "," + dataGridView1[2, j].Value.ToString();
                        }
                       
                    }
                }
                txtmobileno.Text = orderNo;
                
            }
            else
            {
                MessageBox.Show("Select record to Issue");
            }
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string mobileno = txtmobileno.Text;
            string unm = txtunm.Text;
            string pass = txtpass.Text;
            string smstype = cmbsmstype.Text;
            string smspriority = cmbsmspriority.Text;
            string senderid = cmbsenderid.Text;
            string msg = textBox1.Text;
            string url = "http://bhashsms.com/api/sendmsg.php?user=alkhemacademy&pass=123456&sender=ALKHEM&phone=" + mobileno + "&text=" + msg + "&priority=sdnd&stype=normal";
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
}
