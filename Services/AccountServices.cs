using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using student_log_api.Common;
using student_log_api.DBLibrary;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Services
{
    public class AccountServices : IAccountInterface
    {
        public AppSettings AppSettings { get; }

        public AccountServices(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public async Task<AccountDataModel> GetAccountList(GetAccountDataModel items)
        {
            AccountDataModel response = new();
            try
            {
                if (items.loginUserID == 0 || items.accountID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }
                var sqlParams = new Dictionary<string, object>
                {
                    {"isDropdown",items.isDropdown},
                    {"accountID",items.accountID},
                    {"loginUserID",items.loginUserID}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_ACCOUNT_DETAILS, sqlParams);
                List<AccountDataModelData> DeserializedResult = JsonConvert.DeserializeObject<List<AccountDataModelData>>(Result);
                if (DeserializedResult == null || DeserializedResult.Count == 0)
                {
                    response.Message = "No data found.";
                }
                else
                {
                    response.Message = "Scucess";
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

        public async Task<AccountDataModel> GetAccountDetails(int accountID)
        {
            AccountDataModel response = new();
            try
            {
                if (accountID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }
                var sqlParams = new Dictionary<string, object>
                {
                    {"AccountID",accountID}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_ACCOUNT_DETAILS, sqlParams);
                List<AccountDataModelData> DeserializedResult = JsonConvert.DeserializeObject<List<AccountDataModelData>>(Result);
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

        public async Task<SchoolsDataModel> GetSchoolsByAccountID(int accountID, int IsActive)
        {
            SchoolsDataModel response = new();
            try
            {
                if (accountID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }
                var sqlParams = new Dictionary<string, object>
                {
                    {"AccountID",accountID},
                    {"IsActive",IsActive}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_SCHOOLS_BY_ACCOUNTID, sqlParams);
                List<SchoolsDataModelData> DeserializedResult = JsonConvert.DeserializeObject<List<SchoolsDataModelData>>(Result);
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

        public async Task<UsersDataModel> GetUsersByAccountID(int accountID, int IsActive)
        {
            UsersDataModel response = new();
            try
            {
                // if (accountID == 0)
                // {
                //     response.addWarning("Invalid Data");
                //     return response;
                // }
                var sqlParams = new Dictionary<string, object>
                {
                    {"AccountID",accountID},
                    {"IsActive",IsActive}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_USERS_BY_ACCOUNTID, sqlParams);
                List<UsersDataModelData> DeserializedResult = JsonConvert.DeserializeObject<List<UsersDataModelData>>(Result);
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

        public async Task<ClassesDataModel> GetClassesData(int accountID, int schoolID, int loginUserID, int IsActive)
        {
            ClassesDataModel response = new();
            try
            {
                // if (accountID == 0 || loginUserID == 0)
                // {
                //     response.addWarning("Invalid Data");
                //     return response;
                // }
                var sqlParams = new Dictionary<string, object>
                {
                    {"AccountID",accountID},
                    {"SchoolID",schoolID},
                    {"IsActive",IsActive}
                    // {"LoginUserID",loginUserID}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_CLASSES_LIST, sqlParams);
                List<ClassesDataModelData> DeserializedResult = JsonConvert.DeserializeObject<List<ClassesDataModelData>>(Result);
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

        public async Task<ServiceResponse> UpsertClasses(UpsertClassesModel items)
        {
            ServiceResponse response = new();
            try
            {
                if (items == null || items.loginUserID == 0 || items.classes == null || items.classes.Count == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }

                var jsonData = JsonConvert.SerializeObject(items);
                var sqlParams = new Dictionary<string, object>
                {
                    {"JsonData", jsonData}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_CLASSES_JSON, sqlParams);

                if (string.IsNullOrEmpty(Result))
                {
                    response.Message = "Success";
                }
                else
                {
                    response.Message = Result;
                }
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

        public async Task<ServiceResponse> AddSchool(AddSchoolModelPayload school, int loginAccountID, int loginUserID)
        {
            ServiceResponse response = new();
            try
            {
                if (school == null || string.IsNullOrEmpty(school.SchoolName) || string.IsNullOrEmpty(school.SchoolCode) || loginAccountID == 0 || loginUserID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    {"SchoolID", 0}, // Assuming SchoolID is auto-generated in the database
                    {"SchoolName", school.SchoolName},
                    {"SchoolCode", school.SchoolCode},
                    {"City", school.City},
                    {"AccountID", loginAccountID},
                    {"LoginUserID", loginUserID},
                    {"IsActive", school.Status == "Active" ? 1 :0} // Assuming new schools are active by default
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_SCHOOL, sqlParams);

                List<CommonAPIResponse> DeserializedResult = JsonConvert.DeserializeObject<List<CommonAPIResponse>>(Result);
                if (DeserializedResult == null || DeserializedResult[0].Type != 1)
                {
                    response.addWarning(DeserializedResult?.FirstOrDefault()?.Message);
                    return response;
                }
                response.Message = DeserializedResult[0].Message;
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

        public async Task<ServiceResponse> UpdateSchool(UpdateSchoolModelPayload school, int loginAccountID, int loginUserID)
        {
            ServiceResponse response = new();
            try
            {
                if (school == null || string.IsNullOrEmpty(school.SchoolName) || string.IsNullOrEmpty(school.SchoolCode) || loginAccountID == 0 || loginUserID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    {"SchoolID", school.SchoolID}, // Assuming SchoolID is auto-generated in the database
                    {"SchoolName", school.SchoolName},
                    {"SchoolCode", school.SchoolCode},
                    {"City", school.City},
                    {"AccountID", loginAccountID},
                    {"LoginUserID", loginUserID},
                    {"IsActive", school.Status == "Active" ? 1 :0}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_SCHOOL, sqlParams);

                List<CommonAPIResponse> DeserializedResult = JsonConvert.DeserializeObject<List<CommonAPIResponse>>(Result);
                if (DeserializedResult == null || DeserializedResult[0].Type != 1)
                {
                    response.addWarning(DeserializedResult?.FirstOrDefault()?.Message);
                    return response;
                }
                response.Message = DeserializedResult[0].Message;
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

        public async Task<ServiceResponse> AddUser(AddUserModelPayload user, int loginAccountID, int loginUserID)
        {
            ServiceResponse response = new();
            try
            {
                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email) || loginAccountID == 0 || loginUserID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    {"UserID", 0}, // Assuming UserID is auto-generated in the database
                    {"UserName", user.UserName},
                    {"Email", user.Email},
                    {"SchoolIDs", user.SchoolIDs},
                    {"SchoolNames", user.SchoolNames},
                    {"AccountID", loginAccountID},
                    {"LoginUserID", loginUserID},
                    {"IsActive", user.Status == "Active" ? 1 :0} // Assuming new users are active by default
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_USER, sqlParams);

                List<CommonAPIResponse> DeserializedResult = JsonConvert.DeserializeObject<List<CommonAPIResponse>>(Result);
                if (DeserializedResult == null || DeserializedResult[0].Type != 1)
                {
                    response.addWarning(DeserializedResult?.FirstOrDefault()?.Message);
                    return response;
                }
                response.Message = DeserializedResult[0].Message;
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

        public async Task<ServiceResponse> UpdateUser(UpdateUserModelPayload user, int loginAccountID, int loginUserID)
        {
            ServiceResponse response = new();
            try
            {
                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email) || loginAccountID == 0 || loginUserID == 0)
                {
                    response.addWarning("Invalid Data");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    {"UserID", user.UserID}, // Assuming UserID is auto-generated in the database
                    {"UserName", user.UserName},
                    {"Email", user.Email},
                    {"SchoolIDs", user.SchoolIDs},
                    {"SchoolNames", user.SchoolNames},
                    {"AccountID", loginAccountID},
                    {"LoginUserID", loginUserID},
                    {"IsActive", user.Status == "Active" ? 1 :0}
                };
                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_USER, sqlParams);

                List<CommonAPIResponse> DeserializedResult = JsonConvert.DeserializeObject<List<CommonAPIResponse>>(Result);
                if (DeserializedResult == null || DeserializedResult[0].Type != 1)
                {
                    response.addWarning(DeserializedResult?.FirstOrDefault()?.Message);
                    return response;
                }
                response.Message = DeserializedResult[0].Message;
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
