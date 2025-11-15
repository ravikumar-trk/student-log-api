using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace student_log_api.DBLibrary
{
    public class SqlServerUtility : IDBUtility
    {
        public async Task<string> GetjsonData(string connectionString, string sp, Dictionary<string, object> sqlparems)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(sp, sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = sqlConnection.ConnectionTimeout
                    };
                    foreach (KeyValuePair<string, object> parametar in sqlparems)
                        sqlCommand.Parameters.AddWithValue(parametar.Key, parametar.Value);

                    DataTable dt = new DataTable();
                    (new SqlDataAdapter(sqlCommand)).Fill(dt);
                    sqlCommand.Parameters.Clear();

                    return JsonConvert.SerializeObject(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public async Task<string> GetjsonDataFromDataset(string connectionString, string sp, Dictionary<string, object> sqlparems)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(sp, sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = sqlConnection.ConnectionTimeout
                    };
                    foreach (KeyValuePair<string, object> parametar in sqlparems)
                        sqlCommand.Parameters.AddWithValue(parametar.Key, parametar.Value);

                    DataSet ds = new DataSet();
                    (new SqlDataAdapter(sqlCommand)).Fill(ds);
                    sqlCommand.Parameters.Clear();
                    string str = JsonConvert.SerializeObject(ds, Formatting.Indented);
                    return str;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public DataTable GetDataTable(string connectionString, string sp, Dictionary<string, object> sqlparems)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(sp, sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = sqlConnection.ConnectionTimeout
                    };
                    foreach (KeyValuePair<string, object> parametar in sqlparems)
                        sqlCommand.Parameters.AddWithValue(parametar.Key, parametar.Value);

                    DataTable dt = new DataTable();
                    (new SqlDataAdapter(sqlCommand)).Fill(dt);
                    sqlCommand.Parameters.Clear();

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        private static List<SqlParameter> GetSqlParameters(Dictionary<string, object> sqlParameter)
        {
            return sqlParameter.Select(p => new SqlParameter(p.Key, p.Value)).ToList();
        }
        private static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        public List<T> ExecuteDataReader<T>(string connectionString, string sp, Dictionary<string, object> parameters)
        {
            List<T> result = new List<T>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlParameter = GetSqlParameters(parameters);
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        if (sqlParameter.Any())
                            sqlCommand.Parameters.AddRange(sqlParameter.ToArray());
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = sp;
                        var dataReader = sqlCommand.ExecuteReader();
                        result = DataReaderMapToList<T>(dataReader);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            return result;
        }
        public async Task ExecuteNonQuery(string connectionString, string procedureName, Dictionary<string, object> parameters)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                try
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.CommandText = procedureName;

                        if (parameters.Count > 0)
                        {
                            sqlCommand.Parameters.AddRange(GetSqlParameters(parameters).ToArray());
                        }
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public static String GetSqlS(String Sql, string connectionstring)
        {
            String S = String.Empty;
            IDataReader rs = SqlServerUtility.GetRS(Sql, connectionstring);
            if (rs.Read())
            {
                S = RSFieldByLocale(rs, "S", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
                if (S.Equals(DBNull.Value))
                {
                    S = String.Empty;
                }
            }
            rs.Close();
            return S;
        }
        public static IDataReader GetRS(String Sql, string conn)
        {

            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = conn
            };
            dbconn.Open();
            SqlCommand cmd = new SqlCommand(Sql, dbconn);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static String RSFieldByLocale(IDataReader rs, String fieldname, String LocaleSetting)
        {
            String tmpS = String.Empty;


            int idx = rs.GetOrdinal(fieldname);
            if (rs.IsDBNull(idx))
            {
                tmpS = String.Empty;
            }
            else
            {
                tmpS = rs.GetString(idx);
            }


            return tmpS;

        }

        public static DataSet GetDS(String Sql, string connectionString)
        {

            DataSet ds = new DataSet();
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = connectionString
            };


            dbconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(Sql, dbconn);
            da.SelectCommand.CommandTimeout = 90; // Will allow the Data set to be filled in 90 Secs
            da.Fill(ds, "Table");
            dbconn.Close();

            return ds;
        }

        static public int GetSqlN(String Sql, string connectionString)
        {
            int N = 0;
            IDataReader rs;
            rs = SqlServerUtility.GetRS(Sql, connectionString);
            if (rs.Read())
            {
                N = SqlServerUtility.RSFieldInt(rs, "N");
            }
            rs.Close();
            return N;
        }
        public static int RSFieldInt(IDataReader rs, String fieldname)
        {

            int idx = rs.GetOrdinal(fieldname);
            if (rs.IsDBNull(idx))
            {
                return 0;
            }
            return rs.GetInt32(idx);

        }
    }
}
