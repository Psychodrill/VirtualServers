using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Configuration;

namespace VirtualServers.Models
{

     public class VirtualServersDbContext
    {
        public VirtualServersDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static string _connectionString;
        public static string ConnectionString => _connectionString;


        public  List<VirtualServer> GetAllServers()
        {
            string sql = @"SELECT * FROM Tests";

            List<VirtualServer> vsList = new List<VirtualServer>();
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                ds = new DataSet();
                try
                {

                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        vsList.Add(new VirtualServer
                        {
                            VirtualServerId = Convert.ToInt32(ds.Tables[0].Rows[i]["VirtualServerId"] as int?),
                            CreateDateTime = ds.Tables[0].Rows[i]["CreateDateTime"] as DateTime?,
                            RemoveDateTime = ds.Tables[0].Rows[i]["RemoveDateTime"] as DateTime?
                        });
                    }

                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return vsList;
        }

        public void MarkAsRemoved(int? id)
        {
            string sql = $@"UPDATE [RemoveDateTime] = GetDate() FROM Tests WHERE Id={id}";


            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);


                //  com.Parameters.Add(new SqlParameter("VirtualServerId", SqlDbType.Int) { Value = id});


                try
                {
                    con.Open();

                    com.ExecuteNonQuery();
                    con.Close();

                }
                catch (SqlException e)
                {
                    throw e;  
                }
                catch (Exception e)
                {
                    throw e;  
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }

        }

        public void AddServer()
        {
            string sql = @"UPDATE [dbo].[Tests] SET [VirtualServerId]=NEWID(), [CreateDateTime]=GetDate()";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);


                //  com.Parameters.Add(new SqlParameter("VirtualServerId", SqlDbType.Int) { Value = id});


                try
                {
                    con.Open();

                    com.ExecuteNonQuery();
                    con.Close();

                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
        }

    }


}
