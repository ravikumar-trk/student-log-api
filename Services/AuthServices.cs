using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using student_log_api.Common;
using student_log_api.DBLibrary;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Services
{
    public class AuthServices : IAuthInterface
    {
        public AppSettings AppSettings { get; }

        public AuthServices(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public async Task<UserDetailsModel> GetUserByEmail(string email, string Password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(Password))
                {
                    return null;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    {"Email", email},
                    {"Password", Password}
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var Result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.CHECK_VALID_USER, sqlParams);

                if (string.IsNullOrEmpty(Result))
                {
                    return null;
                }

                List<UserDetailsModel> DeserializedResult = JsonConvert.DeserializeObject<List<UserDetailsModel>>(Result);

                if (DeserializedResult == null || DeserializedResult.Count == 0)
                {
                    return null;
                }
                if (DeserializedResult[0].Type == 0) // Assuming Type == 0 indicates invalid user
                {
                    return new UserDetailsModel
                    {
                        Type = 0,
                        Message = DeserializedResult[0].Message ?? "Invalid user credentials."
                    };
                }

                return DeserializedResult[0];
            }
            catch (SqlException ex)
            {
                return ex.Message != null ? throw new Exception(ex.Message) : throw new Exception("An error occurred while fetching user details.");
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
