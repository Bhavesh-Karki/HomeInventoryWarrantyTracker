using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HomeInventoryWarrantyTracker.App_Code
{
    public static class DatabaseHelper
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HomeInventoryDBConnection"].ConnectionString;
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while executing non-query.", ex);
                    }
                }
            }
        }

        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while executing scalar.", ex);
                    }
                }
            }
        }

        public static DataTable GetDataTable(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving DataTable.", ex);
                    }
                }
            }
        }

        public static DataSet GetDataSet(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            return ds;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while retrieving DataSet.", ex);
                    }
                }
            }
        }

        public static void ExecuteReader(string query, Action<SqlDataReader> processReader, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            processReader(reader);
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error occurred while executing reader.", ex);
                    }
                }
            }
        }
    }
}
