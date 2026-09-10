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

        public async Task<UpsertStudentsResponse> UpsertStudents(UpsertStudentsModel items, int loginUserID, int loginAccountID)
        {
            UpsertStudentsResponse response = new();
            try
            {
                if (items == null || items.Students == null || items.Students.Count == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }

                var studentsJson = JsonConvert.SerializeObject(items.Students.Select(student => new
                {
                    StudentID = student.StudentID == 0 ? null : student.StudentID,
                    FirstName = student.FirstName ?? student.StudentName,
                    MiddleName = student.MiddleName,
                    LastName = student.LastName,
                    AccountID = loginAccountID,
                    SchoolCode = student.SchoolCode ?? student.School,
                    ClassCode = student.ClassCode ?? student.Class,
                    SectionID = student.SectionID,
                    AdmissionNo = student.AdmissionNo,
                    RollNo = student.RollNo,
                    Gender = student.Gender,
                    DOB = student.DOB ?? DateTime.Now.ToString("dd-MM-yyyy"),
                    FatherName = student.FatherName ?? "",
                    MotherName = student.MotherName,
                    Contact1 = student.Contact1 ?? "",
                    Contact2 = student.Contact2,
                    AddressLine1 = student.AddressLine1 ?? "",
                    AddressLine2 = student.AddressLine2 ?? "",
                    City = student.City ?? "",
                    State = student.State ?? "",
                    IsActive = true,
                    UserID = loginUserID
                }));
                var sqlParams = new Dictionary<string, object>
                {
                    {"StudentsJson", studentsJson}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_STUDENTS_JSON, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<UpsertStudentResult>()
                    : JsonConvert.DeserializeObject<List<UpsertStudentResult>>(result) ?? new List<UpsertStudentResult>();
                response.Message = response.Result.Count == 0 ? "Success" : "Students processed.";
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
