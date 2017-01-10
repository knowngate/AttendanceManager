using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace NFCCS
{
    class NFChandler
    {
        Boolean empty = false;
        DBConnection connObj = new DBConnection();
        MySqlConnection conn = null;
        public Boolean isvalidData(String[] data)
        {
            try
            {
               
                for (int i = 0; i < data.Length; i++)
                {

                    if (data[i] != null && data[i] != "")
                    {
                        if (data[i].Trim().Length != 0)
                        {
                            if (data[i] != "")
                            {
                                empty = true;
                            }
                        }

                    }
                    else
                    {
                        empty = false;
                        break;
                    }
                }
                return empty;
            }
            catch
            {
                return false;
            }
        }

        public double getCourseFee(MySqlConnection conn, string course)
        {
            double fess = 0;
            try
            {
                string selcorsefee = "select fees from course where name='" + course + "'";
               conn= connObj.getconnection();
               DataSet ds=  connObj.selectRecords(conn,selcorsefee);
               for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   fess = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
               }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return fess;
        }
        public DataSet getAllBatces()
        {
            string selbatces = "select name from batch ";
            conn = connObj.getconnection();
            DataSet ds = connObj.selectRecords(conn, selbatces);
            return ds;
        }
        public DataSet getAllCourses()
        {
            string selcourse = "select name from course ";
            conn = connObj.getconnection();
            DataSet ds = connObj.selectRecords(conn, selcourse);
            return ds;
        }
        public DataSet getAllClasses()
        {
            string selcourse = "select classname from class ";
            conn = connObj.getconnection();
            DataSet ds = connObj.selectRecords(conn, selcourse);
            return ds;
        }
        public int checkForCount(string id)
        {
            string selcnt = "select cnt from attendance where cardno='" + id + "' AND date='" + new DateTime().ToString("yyyy-MM-dd") + "' ";
            conn = connObj.getconnection();
            DataSet ds = connObj.selectRecords(conn, selcnt);
            int cnt = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                 cnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            return cnt;
        }
        public int checkForExist(string id)
        {
            string selcnt = "select * from studenttab where idnumber='" + id + "'";
            conn = connObj.getconnection();
            DataSet ds = connObj.selectRecords(conn, selcnt);
            int cnt = 0;

            return ds.Tables[0].Rows.Count;
        }
    
    }
}
