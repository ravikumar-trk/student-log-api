using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace student_log_api.DBLibrary
{
    public interface IDBUtility
    {
        Task<string> GetjsonData(string connectionString, string sp, Dictionary<string, object> sqlparems);
        Task<string> GetjsonDataFromDataset(string connectionString, string sp, Dictionary<string, object> sqlparems);
        DataTable GetDataTable(string connectionString, string sp, Dictionary<string, object> sqlparems);
        List<T> ExecuteDataReader<T>(string connectionString, string sp, Dictionary<string, object> parameters);
        Task ExecuteNonQuery(string connectionString, string procedureName, Dictionary<string, object> parameters);
    }
}
