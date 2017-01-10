using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace NFCCS
{
   public class DBConnection 
    {

    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;
    MySqlConnection conn = null;
    public MySqlConnection getconnection()
    {
        server = "localhost";
        database = "nfcsc";
        uid = "root";
        password = "root";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
	    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        conn = new MySqlConnection(connectionString);
        
        return conn;
       }
    public DataSet selectRecords(MySqlConnection conn, String query)
    {
        DataSet ds = new DataSet();
        try
        {
            conn.Open();
            MySqlDataAdapter adp = new MySqlDataAdapter(query, conn);
             ds = new DataSet();
            adp.Fill(ds);
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            conn.Close();

        } return ds;
        
    }
    public int deleteRecords(MySqlConnection conn, String query)
    {
         int rws=0;
        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rws= cmd.ExecuteNonQuery();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            conn.Close();
        }
        return rws;
    }
    public int insertRecords(MySqlConnection conn, String query)
    {
         int rws=0;
        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rws= cmd.ExecuteNonQuery();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return 0;
        }
        finally
        {
            conn.Close();
        }
        
        return rws;
    }
    }
    
}
