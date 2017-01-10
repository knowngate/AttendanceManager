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
    public partial class MainWindow : Form
    {
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void oterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void wellcome_Load(object sender, EventArgs e)
        {
            try
            {

                string s = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string instinfo = "SELECT name,address,contact,logo from instituteinfo";
                conn = connObj.getconnection();
                DataSet ds = connObj.selectRecords(conn, instinfo);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string impt = ds.Tables[0].Rows[0][3].ToString();
                    string address = ds.Tables[0].Rows[0][1].ToString();
                    string contact = ds.Tables[0].Rows[0][2].ToString();
                    string name = ds.Tables[0].Rows[0][0].ToString();

                    try
                    {
                        string palettesPath = @"C:\Images\logo\" + impt;
                        Image ima = Image.FromFile(palettesPath);
                        pictureBox1.Image = ima;
                        staticinfo stinf = new staticinfo(palettesPath, name, address, contact);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }

                    
                    ////MessageBox.Show(staticinfo.addr + staticinfo.contact);
                }
            }
            catch
            {
            }

        }

        private void studentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
        }

        private void instituteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Institute inst = new Institute();
            inst.Show();
        }

        private void classesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes cls = new Classes();
            cls.Show();
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Courses crs = new Courses();
            crs.Show();
        }

        private void batcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Batches btc = new Batches();
            btc.Show();
        }

        private void studentFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentFees stdfee = new StudentFees();
            stdfee.Show();
        }

        private void studentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchwiseStudents sdtinbtc = new BatchwiseStudents();
            sdtinbtc.Show();
        }

        private void attendanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AttendanceReport attdn = new AttendanceReport();
            attdn.Show();
        }

        private void attendaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Attendance attndfrm = new Attendance();
            attndfrm.Show();
        }

        private void studentFeesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StudentInfoform stdinfm = new StudentInfoform();
            stdinfm.Show();
        }

        private void studentFeesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentFeeReport stdfeerpt = new StudentFeeReport();
            stdfeerpt.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualSms sms = new ManualSms();
            sms.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void label58_Click(object sender, EventArgs e)
        {

        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void label82_Click(object sender, EventArgs e)
        {

        }

        private void label83_Click(object sender, EventArgs e)
        {

        }

        private void label84_Click(object sender, EventArgs e)
        {

        }

        private void label85_Click(object sender, EventArgs e)
        {

        }

        private void label86_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label94_Click(object sender, EventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }
    }
}
