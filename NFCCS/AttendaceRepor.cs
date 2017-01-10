using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace NFCCS
{
    public partial class AttendanceReport : Form
    {
        DBConnection connObj = new DBConnection();
        NFChandler nfcndl = new NFChandler();
        private MySqlConnection conn = null;

        attendancerpt objRpt = new attendancerpt();
        string ss = "";
        public AttendanceReport()
        {
            InitializeComponent();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            objRpt = new attendancerpt();
            ss = txtname.Text;
            //Get Select query String and add parameters to the 
            //Crystal report.
            string query = CreateSelectQueryAndParameters();
            conn = connObj.getconnection();
            //if there is no item select, then exit from the method.
            if (!query.Contains("Column"))
            {
                MessageBox.Show("No selection to display!");
                return;
            }

            try
            {
                MySqlDataAdapter adepter = new MySqlDataAdapter(query, conn);
                stdinfo Ds = new stdinfo();
                adepter.Fill(Ds, "stdattendance");
                objRpt.SetDataSource(Ds);
                int j = Ds.Tables[0].Rows.Count;
                crystalReportViewer1.ReportSource = objRpt;
            }
            catch (MySqlException oleEx)
            {
                MessageBox.Show(oleEx.Message);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

      
             private string CreateSelectQueryAndParameters()
        {
            string fromdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string todate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            ReportDocument reportDocument;
            ParameterFields paramFields;

            ParameterField paramField;
            ParameterDiscreteValue paramDiscreteValue;

            reportDocument = new ReportDocument();
            paramFields = new ParameterFields();

            string query = "SELECT ";
            int columnNo = 0;


            {
                columnNo++;
                query = query.Insert(query.Length, " concat(std.fname,'  ',std.mname,'  ',std.lname) as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "orderno";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            {
                columnNo++;
                if (query.Contains("Column"))
                {
                    query = query.Insert(query.Length, ", ");
                }
                query = query.Insert(query.Length, "std.class as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "itemcode";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            {
                columnNo++; //To determine Column number
                if (query.Contains("Column"))
                {
                    query = query.Insert(query.Length, ", ");
                }
                query = query.Insert(query.Length, "std.batch as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "description";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            {
                columnNo++;
                if (query.Contains("Column"))
                {
                    query = query.Insert(query.Length, ", ");
                }
                query = query.Insert(query.Length, "att.intime as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "uom";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            {
                columnNo++;
                if (query.Contains("Column"))
                {
                    query = query.Insert(query.Length, ", ");
                }
                query = query.Insert(query.Length, "att.outtime as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "transactiondate";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            //if there is any remaining parameter, assign empty value for that 
            //parameter.
            for (int i = columnNo; i < 5; i++)
            {
                columnNo++;
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            crystalReportViewer1.ParameterFieldInfo = paramFields;

            string fileter = "";
            if (ss != ""&&ss!=null)
            {
                fileter = " AND std.fname like '%" + ss + "%'";
            }
            else
            {
                fileter = " ";
            }

            query += " FROM attendance att INNER JOIN studenttab std on std.idnumber=att.cardno WHere DATE(att.intime)>='" + fromdate + "' AND DATE(att.intime)<='" + todate + "' "+fileter;
            return query;
        }

             private void txtname_TextChanged(object sender, EventArgs e)
             {
                 try
                 {
                     ss = txtname.Text;
                     btnsearch_Click(sender, e);
                     txtname.Focus();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show("Cannot process...");
                 }
             }

             private void label2_Click(object sender, EventArgs e)
             {

             }

             private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
             {

             }

             private void AttendanceReport_Load(object sender, EventArgs e)
             {

             }
        }
    
}
