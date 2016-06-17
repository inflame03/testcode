using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Configuration;

using Common.Models;

namespace Common.DataAccess
{
    public class UserDataAccess
    {
        public UserDataAccess()
        {

        }

        public List<User> GetAllUsers()
        {
            List<User> result = new List<User>();

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"SELECT * FROM Users (NOLOCK)";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                conn.Open();

                DataSet ds = new DataSet();

                sda.Fill(ds);

                foreach( DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(new User { id = Int32.Parse(row["id"].ToString()), name = row["name"].ToString(), userName = row["username"].ToString() });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public User GetUser(int userID)
        {
            User result = null;

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"SELECT * FROM Users (NOLOCK) WHERE ID = @userID";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(new SqlParameter("@userID", userID));

            SqlDataReader sdr;

            try
            {
                conn.Open();

                sdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if(sdr.Read())
                {
                    result = new User { id = Int32.Parse(sdr["id"].ToString()), name = sdr["name"].ToString(), userName = sdr["username"].ToString() };
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public User SaveUser(User userInfo)
        {
            User result = new User();
            result.id = 0;

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString;
            
            if( userInfo.id == 0 )
                queryString = @"INSERT INTO Users VALUES ( @username, @name); SELECT SCOPE_IDENTITY() AS 'newID' ";
            else
                queryString = @"UPDATE Users SET name = @name, username = @username WHERE id = @id ";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(new SqlParameter("@username", userInfo.userName));
            cmd.Parameters.Add(new SqlParameter("@name", userInfo.name));


            if (userInfo.id > 0)
                cmd.Parameters.Add(new SqlParameter("@id", userInfo.id));

            SqlDataReader sdr;

            try
            {
                conn.Open();

                sdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (sdr.Read())
                {
                    result = userInfo;
                    result.id = Int32.Parse(sdr["newID"].ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeleteUser(int userID)
        {
            bool result = false;

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"DELETE FROM Users WHERE ID = @userID";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(new SqlParameter("@userID", userID));

            int rows = 0;

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

    }
}
