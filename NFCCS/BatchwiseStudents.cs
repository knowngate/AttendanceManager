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
    public partial class BatchwiseStudents : Form
    {
        DBConnection connObj = new DBConnection();
        NFChandler nfcndl = new NFChandler();
        private MySqlConnection conn = null;

        stdinforpt objRpt = new stdinforpt();
        string ss = "";
        public BatchwiseStudents()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void BatchwiseStudents_Load(object sender, EventArgs e)
        {

        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            objRpt = new stdinforpt();

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
                adepter.Fill(Ds, "stdinfo");
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
                query = query.Insert(query.Length, " concat(fname,'  ',mname,'  ',lname) as Column" +
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
                query = query.Insert(query.Length, "mobile as Column" +
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
                query = query.Insert(query.Length, "address as Column" +
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
                query = query.Insert(query.Length, "sex as Column" +
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
                query = query.Insert(query.Length, "class as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "transactiondate";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            //{
            //    columnNo++;
            //    if (query.Contains("Column"))
            //    {
            //        query = query.Insert(query.Length, ", ");
            //    }
            //    query = query.Insert(query.Length, "reason as Column" +
            //    columnNo.ToString());

            //    paramField = new ParameterField();
            //    paramField.Name = "col" + columnNo.ToString();
            //    paramDiscreteValue = new ParameterDiscreteValue();
            //    paramDiscreteValue.Value = "reason";
            //    paramField.CurrentValues.Add(paramDiscreteValue);
            //    //Add the paramField to paramFields
            //    paramFields.Add(paramField);
            //}

            {
                columnNo++;
                if (query.Contains("Column"))
                {
                    query = query.Insert(query.Length, ", ");
                }
                query = query.Insert(query.Length, "batch as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "quantity";
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
                query = query.Insert(query.Length, "course as Column" +
                columnNo.ToString());

                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "price";
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
                fileter = " Where fname like '%" + ss + "%' OR mname like '%" + ss + "%' OR lname like '%" + ss + "%' ";
            }
            else
            {
                fileter = " ";
            }

            query += " FROM studenttab " + fileter;
            return query;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

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
