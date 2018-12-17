using MySql.Data.MySqlClient;
using SimpleRestService.Models;
using System;
using System.Diagnostics;
using System.Collections;

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
        static MySqlConnection conn;

        public PersonHelper()
        {
            //string connectionstring = $"Server=tcp:kirmajerdatabase1.database.windows.net,1433;Initial Catalog=Database1;Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string connectionstring = $"server={myServerAddress};Port={dbPort};uid={username};pwd={password};database={database}";

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connectionstring;
                conn.Open();
            }

            catch (MySqlException)
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

        public ArrayList getPerson(long ID)
        {
            
            MySqlDataReader mySqlDataReader = null;

            try
            {
                mySqlDataReader = SelectPersonByID(ID).ExecuteReader();
                return ReadDataFromDBWhitCommand(mySqlDataReader);

            }
            catch (Exception ex)
            {
                //TODO: handle ex
            }
            finally
            {
                mySqlDataReader.Dispose();
                CloseConnection();                
            }
            return null;
        }


        public ArrayList GetPersons()
        {    
            MySqlDataReader mySqlDataReader = null;

            try
            {
                mySqlDataReader = SelectAllPersons().ExecuteReader();
                return ReadDataFromDBWhitCommand(mySqlDataReader);
            }
            catch (Exception ex)
            {
                //TODO: handle ex
            }
            finally
            {             
                mySqlDataReader.Dispose();
                CloseConnection();
            }
            return null;
        }
        private MySqlCommand SelectAllPersons()
        {
            string cmd = "SELECT * FROM tblpersons";
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            return mySqlCommand;
        }

        private MySqlCommand SelectPersonByID(long ID)
        {
            string cmd = "SELECT * FROM tblpersons WHERE ID = @ID";
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.Add("@ID", ID);

            return mySqlCommand; 
        }
  
        private static MySqlCommand InsertPersonCommand(Person personToSave)
        {
            string cmd = "INSERT INTO tblpersons (`FirstName`,`LastName`,`PayRate`,`StartDate`,`EndDate`) VALUES (@Fistname, @LastName,@PayRate,@StartDate,@EndDate)";
            MySqlCommand mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.AddWithValue("@Fistname", personToSave.FirstName);
            mySqlCommand.Parameters.AddWithValue("@LastName", personToSave.LastName);
            mySqlCommand.Parameters.AddWithValue("@PayRate", personToSave.PayRate);

            string format = "yyyy-MM-dd HH:mm:ss";

            mySqlCommand.Parameters.AddWithValue("@StartDate", personToSave.StartDate.ToString(format));
            mySqlCommand.Parameters.AddWithValue("@EndDate", personToSave.EndDate.ToString(format));
       
            return mySqlCommand;
        }


        private static ArrayList ReadDataFromDBWhitCommand(MySqlDataReader mySqlDataReader)
        {
            ArrayList Person = new ArrayList();
            while (mySqlDataReader.Read())
            {
                Person.Add(PopulatePersonWhitValuesFromDB(mySqlDataReader));
            }
            mySqlDataReader.Close();
            return Person;
        }

        private static ArrayList PopulatePersonWhitValuesFromDB( MySqlDataReader mySqlDataReader)
        {
            ArrayList persons = new ArrayList();

            Person person = new Person();
            person.ID = mySqlDataReader.GetInt32(0);
            person.FirstName = mySqlDataReader.GetString(1);
            person.LastName = mySqlDataReader.GetString(2);
            person.PayRate = mySqlDataReader.GetDouble(3);
            person.StartDate = mySqlDataReader.GetDateTime(4);
            person.EndDate = mySqlDataReader.GetDateTime(5);
            persons.Add(person);

            return persons;
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