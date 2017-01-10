using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace NFCCS
{
    public partial class Batches : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        NFChandler nfcandler = new NFChandler();
        public Batches()
        {
            InitializeComponent();
        }

        private void txtfees_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnsavebatch_Click(object sender, EventArgs e)
        {
            try
            {
                string[] instr = { txtbatcname.Text, cmbduration.Text, cmbClass.Text, dtenddate.Value.ToString(), dtstartdate.Value.ToString()};
                bool isvld = nfcandler.isvalidData(instr);
                if (isvld)
                {
                    conn = connObj.getconnection();
                    string query = "insert into batch (name,duration,startdate,enddate,class) values('" + txtbatcname.Text + "','" + cmbduration.Text + "','" + dtstartdate.Value.ToString("yyyy-MM-dd") + "','" + dtenddate.Value.ToString("yyyy-MM-dd") + "','" + cmbClass.Text + "')";
                    connObj.insertRecords(conn,query);
                    clearalldata();
                    MessageBox.Show("Data inserted successfully..");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearalldata()
        {
            cmbduration.Text="";
            txtbatcname.Text="";
            cmbClass.Text="";
        }

        private void Batches_Load(object sender, EventArgs e)
        {
            conn = connObj.getconnection();
            string selclasses = "select classname from class";
            DataSet st = connObj.selectRecords(conn,selclasses);
            for (int i = 0; i < st.Tables[0].Rows.Count; i++)
            {
                cmbClass.Items.Add(st.Tables[0].Rows[i]["classname"].ToString());
            }

            Image ima = Image.FromFile(staticinfo.imgpat);
            pictureBox1.Image = ima;
            label8.Text = staticinfo.name;
            label7.Text = staticinfo.addr;
            label6.Text = staticinfo.contact;
            

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
