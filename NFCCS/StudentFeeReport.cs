using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace NFCCS
{
    public partial class StudentFeeReport : Form
    {
        DBConnection connObj = new DBConnection();
        NFChandler nfcndl = new NFChandler();
        private MySqlConnection conn = null;

        Studentfeesrpt objRpt = new Studentfeesrpt();
        string ss = "";
        public StudentFeeReport()
        {
            InitializeComponent();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            objRpt = new Studentfeesrpt();

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
                adepter.Fill(Ds, "stdfees");
                objRpt.SetDataSource(Ds);
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
            //string fromdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            //string todate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
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
                paramDiscreteValue.Value = "fname";
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
                paramDiscreteValue.Value = "class";
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
                paramDiscreteValue.Value = "batch";
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
                query = query.Insert(query.Length, "att.prevbalance as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "prevbal";
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
                query = query.Insert(query.Length, "att.paid as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "paid";
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
                query = query.Insert(query.Length, "att.totalbalance as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "totalbalance";
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
            if (ss != "" && ss != null)
            {
                fileter = " Where std.fname like '%" + ss + "%' OR std.mname like '%" + ss + "%' OR std.lname like '%" + ss + "%'";
            }
            else
            {
                fileter = " ";
            }

            query += " FROM stdaccounting att INNER JOIN studenttab std on std.idnumber=att.stdid  " + fileter;
            return query;
        }

        private void txtbyname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ss = txtbyname.Text;
                btnsearch_Click(sender, e);
                txtbyname.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot process...");
            }
        }
    }
}
