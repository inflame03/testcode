using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using BaBu.Models;

namespace BaBu.DataAccess
{
    

    public class BaBuDataAccess
    {
        public BaBuDataAccess() {  }

        public List<FoodComponent> GetAllItems(ItemType type)
        {
            List<FoodComponent> result = new List<FoodComponent>();

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"SELECT * FROM " + this.GetTableName(type) + " (NOLOCK)";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                conn.Open();

                DataSet ds = new DataSet();

                sda.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(new FoodComponent { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type });
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

        public FoodComponent GetItem(int id, ItemType type)
        {
            FoodComponent result = null;

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"SELECT * FROM " + this.GetTableName(type) + " (NOLOCK) WHERE ID = @userID";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(new SqlParameter("@id", id));

            SqlDataReader sdr;

            try
            {
                conn.Open();

                sdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (sdr.Read())
                {
                    result = new FoodComponent { ID = Int32.Parse(sdr["id"].ToString()), Name = sdr["name"].ToString() };
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

        public FoodComponent SaveItem(FoodComponent item, ItemType type)
        {
            FoodComponent result = new FoodComponent();
            result.ID = 0;

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString;

            if (item.ID == 0)
                queryString = @"INSERT INTO " + this.GetTableName(type) + " VALUES ( @name ); SELECT SCOPE_IDENTITY() AS 'newID' ";
            else
                queryString = @"UPDATE " + this.GetTableName(type) + " SET name = @name WHERE id = @id ";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(new SqlParameter("@name", item.Name));


            if (item.ID > 0)
                cmd.Parameters.Add(new SqlParameter("@id", item.ID));

            SqlDataReader sdr;

            try
            {
                conn.Open();

                sdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (sdr.Read())
                {
                    result = item;
                    result.ID = Int32.Parse(sdr["newID"].ToString());
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

        public bool DeleteItem(int id, ItemType type)
        {
            bool result = false;

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"DELETE FROM " + this.GetTableName(type) + " WHERE ID = @id";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(new SqlParameter("@id", id));

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

        private string GetTableName( ItemType item )
        {
            switch(item)
            {
                case ItemType.Condiment: return "CONDIMENTS";
                case ItemType.Ingredients: return "INGREDIENTS";
                case ItemType.Spices: return "SPICES";
                case ItemType.Garnish: return "GARNISH";
                case ItemType.CookingMethod: return "COOKINGMETHOD";
                default: return "INGREDIENTS";
            }
        }

        public IList GetAllItemsByType(ItemType type)
        {
            IList result = null;

            switch (type)
            {
                case ItemType.Condiment: result = new List<Condiments>(); break;
                case ItemType.Spices: result = new List<Spices>(); break;
                case ItemType.Garnish: result = new List<Garnish>(); break;
                case ItemType.Ingredients: result = new List<Ingredients>(); break;
                case ItemType.CookingMethod: result = new List<CookingMethod>(); break;

                default: result = new List<FoodComponent>(); break;
            }

            string connString = ConfigurationManager.ConnectionStrings["dbConn"].ToString().Trim();
            string queryString = @"SELECT * FROM " + this.GetTableName(type) + " (NOLOCK) ORDER BY NEWID()";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(queryString, conn);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                conn.Open();

                DataSet ds = new DataSet();

                sda.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case ItemType.Condiment: result.Add(new Condiments { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type }); break;
                        case ItemType.Spices: result.Add(new Spices { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type }); break;
                        case ItemType.Garnish: result.Add(new Garnish { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type }); break;
                        case ItemType.Ingredients: result.Add(new Ingredients { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type }); break;
                        case ItemType.CookingMethod: result.Add(new CookingMethod { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type }); break;

                        default: result.Add(new FoodComponent { ID = Int32.Parse(row["id"].ToString()), Name = row["name"].ToString(), ItemType = type }); break;
                    }
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
