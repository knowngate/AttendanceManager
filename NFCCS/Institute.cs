using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace NFCCS
{
    public partial class Institute : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        NFChandler nfcandler = new NFChandler();
        string filepath = null;

        public Institute()
        {
            InitializeComponent();
        }

        private void Institute_Load(object sender, EventArgs e)
        {
            lblselinst.Visible = false;
            cmbinst.Visible = false;
            try
            {
                Image ima = Image.FromFile(staticinfo.imgpat);
                pictureBox2.Image = ima;
                label18.Text = staticinfo.name;
                label17.Text = staticinfo.addr;
                label12.Text = staticinfo.contact;
            }
            catch { }

            try
            {
                cmbdistrict.Items.Clear();
                cmbdistrict.Text = "";
                //if (cmbstate.Text == "Maharastra")
                {
                    MySqlConnection conn = connObj.getconnection();
                    string seledistAndtal = "select name from district order by name ";
                    DataSet ds = connObj.selectRecords(conn, seledistAndtal);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cmbdistrict.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                    }
                }
            }
            catch { }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnsaveinst_Click(object sender, EventArgs e)
        {    
            conn = connObj.getconnection();
            string[] sarr={txtaddress.Text,txtcontact.Text,txtfax.Text,txtinstname.Text,txtlandline.Text,txtmobile.Text,txtpin.Text,cmbdistrict.Text,cmbtaluka.Text};
            bool isvalid=nfcandler.isvalidData(sarr);
            if (isvalid)
            {
                string picname = "";
                try
                {
                    picname = filepath.Substring(filepath.LastIndexOf('\\'));
                    string extension = Path.GetExtension(filepath);
                    string palettesPath = "C:\\Images\\logo\\";
                    // If the directory doesn't exist, create it.
                    if (!Directory.Exists(palettesPath))
                    {
                        Directory.CreateDirectory(palettesPath);
                    }
                    string subPath = palettesPath + picname ;
                    Bitmap imgImage = new Bitmap(pictureBox1.Image);    //Create an object of Bitmap class/
                    imgImage.Save(subPath);
                }
                catch (Exception)
                {
                    // Fail silently
                }
                if (chkedit.Checked == true)
                {
                    string delInst = "delete from instituteinfo where name='" + cmbinst.Text + "'";
                    connObj.deleteRecords(conn, delInst);
                }
                string query = "insert into instituteinfo (name,address,pin,contact,mobile,landline,fax,logo,princilple"
                + ",district,taluka) VALUES ('" + txtinstname.Text + "','" + txtaddress.Text + "','" + txtpin.Text + "','" + txtcontact.Text + "'"
                + ",'" + txtmobile.Text + "','" + txtlandline.Text + "','" + txtfax.Text + "','" + picname + "','" + txtpriciple.Text + "','" + cmbdistrict.Text + "','" + cmbtaluka.Text + "')";
                connObj.insertRecords(conn, query);
                clearAllDAta();
                MessageBox.Show("Institute Added successfully..");
            }
            else
            {
                MessageBox.Show("Please enter valid data");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void clearAllDAta()
        {
            txtpriciple.Text = "";
            txtaddress.Text = "";
            txtcontact.Text = "";
            txtfax.Text = "";
            txtinstname.Text = "";
            txtlandline.Text = "";
            txtmobile.Text = "";
            txtpin.Text = "";
            cmbdistrict.Text = "";
            cmbtaluka.Text = "";
            lblselinst.Visible = false;

            cmbinst.Visible = false;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbdistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbtaluka.Items.Clear();
                cmbtaluka.Text = "";
                MySqlConnection conn = connObj.getconnection();
                DataSet ds1 = connObj.selectRecords(conn, "select code from district where name='" + cmbdistrict.Text + "'");
                string distcode = ds1.Tables[0].Rows[0][0].ToString();
                string seledistAndtal = "select name from taluka where districtcode=" + distcode + " order by name ";
                DataSet ds = connObj.selectRecords(conn, seledistAndtal);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbtaluka.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                }

            }
            catch { }
        }

        private void txtcontact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtpin_KeyPress(object sender, KeyPressEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = connObj.getconnection();
            string seledistAndtal = "select * from instituteinfo where name='"+cmbinst.Text+"' ";
            DataSet ds = connObj.selectRecords(conn, seledistAndtal);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string name = ds.Tables[0].Rows[i]["name"].ToString();
                string address = ds.Tables[0].Rows[i]["address"].ToString();
                string pin = ds.Tables[0].Rows[i]["pin"].ToString();
                string contact = ds.Tables[0].Rows[i]["contact"].ToString();
                string mobile = ds.Tables[0].Rows[i]["mobile"].ToString();
                string landline = ds.Tables[0].Rows[i]["landline"].ToString();
                string fax = ds.Tables[0].Rows[i]["fax"].ToString();
                string logo = ds.Tables[0].Rows[i]["logo"].ToString();
                string princilple = ds.Tables[0].Rows[i]["princilple"].ToString();
                string district = ds.Tables[0].Rows[i]["district"].ToString();
                string taluka = ds.Tables[0].Rows[i]["taluka"].ToString();

                txtaddress.Text = address;
                txtcontact.Text = contact;
                txtfax.Text = fax;
                txtinstname.Text = name;
                txtlandline.Text = landline;
                txtmobile.Text = mobile;
                txtpin.Text = pin;
                txtpriciple.Text = princilple;
                cmbdistrict.Text = district;
                cmbtaluka.Text = taluka;
                try
                {
                    string palettesPath = @"C:\Images\logo\" + logo;
                    Image ima = Image.FromFile(palettesPath);
                    pictureBox1.Image = ima;
                    staticinfo stinf = new staticinfo(palettesPath, name, address, contact);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void chkedit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkedit.Checked == true)
            {
                lblselinst.Visible = true;
                cmbinst.Visible = true;
                MySqlConnection conn = connObj.getconnection();
                string seledistAndtal = "select name from instituteinfo ";
                DataSet ds = connObj.selectRecords(conn, seledistAndtal);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbinst.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                }
                chkadd.Checked = false;
            }
            else
            {
                lblselinst.Visible = false;
                
                cmbinst.Visible = false;
            }
        }

        private void chkadd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkadd.Checked == true)
            {
                chkedit.Checked = false;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }
    }
}
