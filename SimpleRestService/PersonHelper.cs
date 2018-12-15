using MySql.Data.MySqlClient;
using SimpleRestService.Models;
using System;
using System.Diagnostics;

namespace SimpleRestService
{

    public class PersonHelper
    {
        //TODO: Move the private parameters in config file
        private static string myServerAddress = "127.0.0.1";
        private static string dbPort = "3306";
        private static string database = "persons";
        private static string username = "root";
        private static string password = "Pass1234";
        static MySql.Data.MySqlClient.MySqlConnection conn;

        public PersonHelper()
        {
            //string connectionstring = $"Server=tcp:kirmajerdatabase1.database.windows.net,1433;Initial Catalog=Database1;Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string connectionstring = $"server={myServerAddress};Port={dbPort};uid={username};pwd={password};database={database}";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connectionstring;
                conn.Open();
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {
                CloseConnection();
            }
        }

        public long SavePerson(Person personToSave)
        {
            MySqlCommand mySqlCommand = InsertPersonCommand(personToSave);

            try
            {
                mySqlCommand.ExecuteNonQuery();
                CloseConnection();
                return mySqlCommand.LastInsertedId;
            }
            catch (Exception ex)
            {
                //TODO: handle ex
            }
            finally
            {
                CloseConnection();
            }
            return 0;
        }

        public Person getPerson(long ID)
        {
            Person person = new Person();
            MySqlDataReader mySqlDataReader = null;

            try
            {
                mySqlDataReader = SelectPersonByID(ID).ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    PopulatePersonWhitValuesFromDB(person, mySqlDataReader);
                }
                CloseConnection();
                return person;

            }
            catch (Exception ex)
            {
                //TODO: handle ex
            }
            finally
            {
                CloseConnection();                
            }
            return null;
        }
        

        private MySqlCommand SelectPersonByID(long ID)
        {
            string cmd = "SELECT * FROM tblpersons WHERE ID = @ID";
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.Add("@ID", ID);

            return mySqlCommand; 
        }
  
        private static MySql.Data.MySqlClient.MySqlCommand InsertPersonCommand(Person personToSave)
        {
            string cmd = "INSERT INTO tblpersons (`FirstName`,`LastName`,`PayRate`,`StartDate`,`EndDate`) VALUES (@Fistname, @LastName,@PayRate,@StartDate,@EndDate)";
            MySql.Data.MySqlClient.MySqlCommand mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.AddWithValue("@Fistname", personToSave.FirstName);
            mySqlCommand.Parameters.AddWithValue("@LastName", personToSave.LastName);
            mySqlCommand.Parameters.AddWithValue("@PayRate", personToSave.PayRate);

            string format = "yyyy-MM-dd HH:mm:ss";

            mySqlCommand.Parameters.AddWithValue("@StartDate", personToSave.StartDate.ToString(format));
            mySqlCommand.Parameters.AddWithValue("@EndDate", personToSave.EndDate.ToString(format));
       
            return mySqlCommand;
        }

        private static void PopulatePersonWhitValuesFromDB(Person person, MySqlDataReader mySqlDataReader)
        {
            person.ID = mySqlDataReader.GetInt32(0);
            person.FirstName = mySqlDataReader.GetString(1);
            person.LastName = mySqlDataReader.GetString(2);
            person.PayRate = mySqlDataReader.GetDouble(3);
            person.StartDate = mySqlDataReader.GetDateTime(4);
            person.EndDate = mySqlDataReader.GetDateTime(5);
        }

        private static void CloseConnection()
        {
            conn.Close();
            conn.Dispose();
        }

        public Process[] GetProcesses()
        {
            var allProcesses = Process.GetProcesses();
            return allProcesses;
        }
    }
    
}