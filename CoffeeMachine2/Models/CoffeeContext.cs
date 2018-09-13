using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeMachineWebAPI.Models
{
    public class CoffeeContext : DbContext
    {
        private const string TABLE_NAME = "[dbo].[ORDER]";

        public CoffeeContext()
            : base("name=CoffeeDbConnection")
        {
            
        }

        public DbSet<Order> Orders { get; set; }

        public List<Order> GetNotProcessed()
        {
            string sql = "SELECT * FROM " + TABLE_NAME + " WHERE DAT_PROCESSING IS NULL";

            return ReturnData(sql);
        }

        public List<Order> GetAll()
        {
            string sql = "SELECT * FROM " + TABLE_NAME;

            return ReturnData(sql);
        }

        private List<Order> ReturnData(string sql)
        {
            List<Order> ret = new List<Order>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CoffeeDbConnection"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        var dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            ret.Add(new Order
                            {
                                Id = dataReader["ID_ORDER"] as int?,
                                OrderType = dataReader["COD_ORDER_TYPE"] as short?,
                                CreationDate = dataReader["DAT_CREATION"] as DateTime?,
                                ProcessingDate = dataReader["DAT_PROCESSING"] as DateTime?
                            });
                        }

                        dataReader.Close();
                    }

                    conn.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        public void UpdateOrder(Order ret)
        {
            string queryString = "UPDATE [dbo].[ORDER] SET [DAT_PROCESSING] = '" + ret.ProcessingDate + "' WHERE [ID_ORDER] = " + ret.Id;
            ExecuteNonQuery(queryString);
        }

        public void InsertOrder(Order order)
        {
            string queryString = "INSERT [dbo].[ORDER] (COD_ORDER_TYPE) VALUES (" + order.OrderType + ")";
            ExecuteNonQuery(queryString);
        }

        private void ExecuteNonQuery(string sql)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CoffeeDbConnection"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CoffeeContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
