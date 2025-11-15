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

        public async Task<SchoolsDataModel> GetSchoolsByAccountID(int accountID)
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
                    {"AccountID",accountID}
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

        public async Task<UsersDataModel> GetUsersByAccountID(int accountID)
        {
            UsersDataModel response = new();
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
    }
}
