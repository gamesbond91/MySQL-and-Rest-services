using MySql.Data.MySqlClient;
using SimpleRestService.Models;
using System;
using System.Configuration;
using System.Collections;

namespace SimpleRestService
{

    public class PersonHelper
    {

        private static string connectionstring = ConfigurationManager.ConnectionStrings["localDB"].ConnectionString;
       
        public long SavePerson(Person personToSave)
        {
            MySqlConnection conn;
            conn = new MySqlConnection(connectionstring);

            MySqlCommand mySqlCommand = SqlCommandInsertPerson(personToSave, conn);

            try
            {
                conn.Open();
                mySqlCommand.ExecuteNonQuery();
                conn.Close();
                return mySqlCommand.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }     
        }

        public ArrayList getPerson(long ID)
        {
            MySqlConnection conn;
            conn = new MySqlConnection(connectionstring);

            MySqlDataReader mySqlDataReader = null;

            try
            {
                conn.Open();
                mySqlDataReader = SqlCommandSelectPersonByID(ID, conn).ExecuteReader();
                return ReadDataFromDBWhitCommand(mySqlDataReader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlDataReader.Dispose();
                CloseConnection(conn);
            }         
        }


        public ArrayList GetPersons()
        {
            MySqlConnection conn;
            conn = new MySqlConnection(connectionstring);

            MySqlDataReader mySqlDataReader = null;

            try
            {                
                conn.Open();
                mySqlDataReader = SqlCommandSelectAllPersons(conn).ExecuteReader();
                return ReadDataFromDBWhitCommand(mySqlDataReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlDataReader.Dispose();
                CloseConnection(conn);
            }
        }

        public bool DeletePerson(long ID)
        {
            MySqlConnection conn;
            conn = new MySqlConnection(connectionstring);

            MySqlDataReader mySqlDataReader = null;
            try
            {
                conn.Open();
                mySqlDataReader = SqlCommandSelectPersonByID(ID, conn).ExecuteReader();

                if (mySqlDataReader.Read())
                {
                    mySqlDataReader.Close();
                    DeletePersonByID(ID, conn).ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlDataReader.Dispose();
                CloseConnection(conn);
            }
            return false;
        }

        public bool UpdatePerson(long ID, Person person)
        {
            MySqlConnection conn;
            conn = new MySqlConnection(connectionstring);

            MySqlDataReader mySqlDataReader = null;
            try
            {
                conn.Open();
                mySqlDataReader = SqlCommandSelectPersonByID(ID, conn).ExecuteReader();

                if (mySqlDataReader.Read())
                {
                    mySqlDataReader.Close();
                    SqlCommandUpdatePerson(person, conn, ID).ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlDataReader.Dispose();
                CloseConnection(conn);
            }
            return false;
        }

        private static void CloseConnection(MySqlConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }


        #region DatabaseCommands

        private MySqlCommand DeletePersonByID(long ID, MySqlConnection conn)
        {
            string cmd = "DELETE FROM tblpersons WHERE ID = @ID";
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.Add("@ID", ID);
            return mySqlCommand;
        }

        private MySqlCommand SqlCommandSelectAllPersons(MySqlConnection conn)
        {
            string cmd = "SELECT * FROM tblpersons";
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            return mySqlCommand;
        }

        private MySqlCommand SqlCommandSelectPersonByID(long ID, MySqlConnection conn)
        {
            string cmd = "SELECT * FROM tblpersons WHERE ID = @ID";
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.Add("@ID", ID);
            return mySqlCommand;
        }

        private static MySqlCommand SqlCommandInsertPerson(Person personToSave, MySqlConnection conn)
        {
            string cmd = "INSERT INTO tblpersons (`FirstName`,`LastName`,`PayRate`,`StartDate`,`EndDate`) VALUES (@Fistname, @LastName,@PayRate,@StartDate,@EndDate)";
            return ParametrizedSqlCommand(personToSave, conn, cmd, null);
        }

        private static MySqlCommand SqlCommandUpdatePerson(Person UpdatedPerson, MySqlConnection conn, long ID)
        {
            string cmd = "UPDATE tblpersons SET `FirstName`= @Fistname, `LastName`=@LastName,`PayRate`=@PayRate,`StartDate` =@StartDate ,`EndDate =@EndDate` WHERE 'ID' = @ID";
            return ParametrizedSqlCommand(UpdatedPerson, conn, cmd, ID.ToString());
        }

        private static MySqlCommand ParametrizedSqlCommand(Person personToSave, MySqlConnection conn, string cmd, string ID = null)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(cmd, conn);
            mySqlCommand.Parameters.AddWithValue("@Fistname", personToSave.FirstName);
            mySqlCommand.Parameters.AddWithValue("@LastName", personToSave.LastName);
            mySqlCommand.Parameters.AddWithValue("@PayRate", personToSave.PayRate);

            string format = "yyyy-MM-dd HH:mm:ss";

            mySqlCommand.Parameters.AddWithValue("@StartDate", personToSave.StartDate.ToString(format));
            mySqlCommand.Parameters.AddWithValue("@EndDate", personToSave.EndDate.ToString(format));
            if (ID == null)
                mySqlCommand.Parameters.AddWithValue("@ID", ID);

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

        private static ArrayList PopulatePersonWhitValuesFromDB(MySqlDataReader mySqlDataReader)
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
    }

    #endregion
}

