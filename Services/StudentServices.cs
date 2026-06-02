using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using student_log_api.Common;
using student_log_api.DBLibrary;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Services
{
    public class StudentServices : IStudentInterface
    {
        public AppSettings AppSettings { get; }

        public StudentServices(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public async Task<StudentDataModel> GetStudentsList(GetStudentDataModel items)
        {
            StudentDataModel response = new();
            try
            {
                if (items.LoginUserID == 0 || items.AccountID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }
                var sqlParams = new Dictionary<string, object>
                {
                    {"Prefix",items.Prefix},
                    {"StudentID",items.StudentID},
                    {"ClassID",items.ClassID},
                    {"SchoolID",items.SchoolID},
                    {"AccountID",items.AccountID},
                    {"IsDropdown",items.IsDropdown},
                    {"LoginUserID",items.LoginUserID}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_STUDENTS_LIST, sqlParams);
                List<StudentDataModelData> DeserializedResult = JsonConvert.DeserializeObject<List<StudentDataModelData>>(Result);
                if (DeserializedResult == null || DeserializedResult.Count == 0)
                {
                    response.Message = "No data found.";
                }
                else
                {
                    response.Message = "Success";
                }
                response.Result = DeserializedResult;
            }
            catch (SqlException e)
            {
                response.addError(e.Message);
            }
            catch (ArgumentNullException e)
            {
                response.addError(e.Message);
            }
            catch (Exception e)
            {
                response.addError(e.Message);
            }
            return response;
        }
    }
}
