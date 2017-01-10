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
    public partial class Classes : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        NFChandler nfcandler = new NFChandler();
        public Classes()
        {
            InitializeComponent();
        }

        private void Classes_Load(object sender, EventArgs e)
        {
            Image ima = Image.FromFile(staticinfo.imgpat);
            pictureBox1.Image = ima;
            label8.Text = staticinfo.name;
            label7.Text = staticinfo.addr;
            label6.Text = staticinfo.contact;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsaveclass_Click(object sender, EventArgs e)
        {
            string[] arr = { txtclassname.Text,cmbteacher.Text};
           bool b= nfcandler.isvalidData(arr);
           if (b)
           {
               conn = connObj.getconnection();
               string classquery = "insert into class (classname,classteacher) Values('" + txtclassname.Text + "','" + cmbteacher.Text + "') ";
               connObj.insertRecords(conn, classquery);
               clearalldata();
               MessageBox.Show("Class added successfully..");
           }
           else
           {
               MessageBox.Show("please enter valid data");
           }
        }

        private void clearalldata()
        {
            txtclassname.Text = "";
            cmbteacher.Text = "";
        }
    }
}
