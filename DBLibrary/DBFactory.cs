namespace student_log_api.DBLibrary
{
    public class DBFactory
    {
        private string dbName = "SqlServer";
        public IDBUtility getDBUtility()
        {
            if (dbName == "SqlServer")
            {
                return new SqlServerUtility();
            }
            else
            {
                throw new Exception("DB Configuration not avaialble");
            }
        }
    }
}