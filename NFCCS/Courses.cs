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
    public partial class Courses : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        NFChandler nfcandler = new NFChandler();
        public Courses()
        {
            InitializeComponent();
        }

        private void btnsavecourse_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strarr = {txtcoursename.Text,cmbfees.Text };
                bool isvalid=nfcandler.isvalidData(strarr);
                if (isvalid)
                {
                    conn = connObj.getconnection();
                    string queryc = "insert into course(name,fees) VALUES ('" + txtcoursename.Text + "','" + cmbfees.Text + "')";
                    connObj.insertRecords(conn,queryc);
                    clearAll();
                    MessageBox.Show("Course added successfully..");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearAll()
        {
            cmbfees.Text = "";
            txtcoursename.Text = "";
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Courses_Load(object sender, EventArgs e)
        {
            Image ima = Image.FromFile(staticinfo.imgpat);
            pictureBox1.Image = ima;
            label8.Text = staticinfo.name;
            label7.Text = staticinfo.addr;
            label6.Text = staticinfo.contact;
        }

        private void cmbfees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
