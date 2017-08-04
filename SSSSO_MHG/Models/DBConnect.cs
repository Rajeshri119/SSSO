using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SSSSO_MHG.Models
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {

            //server = "219.91.228.170";
            //server = "localhost";
            //database = "sssso";
            //uid = "ravi";
            //password = "saibaba123456";
            //uid = "root";
            //password = "sairam";
            string connectionString= ConfigurationManager.ConnectionStrings["saiserver"].ToString();
            //string connectionString = ConfigurationManager.ConnectionStrings["local"].ToString();
            //connectionString = "Host=" + server + ";" + "DataBase=" +            database + ";" + "Protocol=TCP;"+ "Port=3306;"+ "User id=" + uid + ";" + "Password=" + password + ";";


            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        
        //Execute stored Proc
        public List<List<Object>> execStoredProcedure(string proc_name,List<MySqlParameter> param)
        {
            List<List<Object>> list = new List<List<Object>>();
            try
            {
                if (this.OpenConnection() == true)
                {
                    
                    MySqlCommand cmd = new MySqlCommand(proc_name, connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    foreach (MySqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();


                    int i = 0;
                    List<Object> record;
                    while (dataReader.Read())
                    {
                        record = new List<Object>();
                        //for each tuple
                        for (i = 0; i < dataReader.FieldCount; i++)
                        {
                            record.Add(dataReader.GetValue(i));
                        }
                        list.Add(record);
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    return list;

                }
                else
                {
                    return list;
                }

            }
            catch(Exception ex)
            {
                //exception handle
                this.CloseConnection();
                handleException(ex);
                return list;
            }
        }

        //Select statement
        public String executeScalarQuery(string query)
        {
            

            //Create a list to store the result
            List<List<Object>> list = new List<List<Object>>();

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list

                    int i = 0;
                    String record="";
                    while (dataReader.Read())
                    {
                        record=dataReader.GetValue(0).ToString();
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    return record;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                //log exception
                this.CloseConnection();
                handleException(ex);
                return null;
            }
            
        }

        //Select statement
        public List<List<Object>> executeQuery(string query)
        {


            //Create a list to store the result
            List<List<Object>> list = new List<List<Object>>();

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list

                    int i = 0;
                    List<Object> record;
                    while (dataReader.Read())
                    {
                        record = new List<Object>();
                        //for each tuple
                        for (i = 0; i < dataReader.FieldCount; i++)
                        {
                            record.Add(dataReader.GetValue(i));
                        }
                        list.Add(record);
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    return list;
                }
                else
                {
                    return list;
                }
            }
            catch (Exception ex)
            {
                //log exception
                this.CloseConnection();
                handleException(ex);
                return list;
            }

        }

        ////Count statement
        //public int Count()
        //{
        //}

        //log the exceptions in the exception table
        public void handleException(Exception ex)
        {
            try
            {
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand("AddExceptionLog", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@input_ExceptionName", ex.Message);
                    cmd.Parameters.AddWithValue("@input_ExceptionControllerName", "DBConnect");
                    cmd.Parameters.AddWithValue("@input_ExceptionStackTrace", ex.StackTrace);
                    cmd.Parameters.AddWithValue("@input_ExceptionModule", "Main");

                    //Create a data reader and Execute the command
                    
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();


                }
                else
                    throw new Exception("Exception information not added to table: " + ex.Message);
            }
            catch(Exception dbconn)
            {
                //handle dbconn exception
                this.CloseConnection();
            }
        }

        //Restore
        public void Restore()
        {
        }
    }
}