using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using student_log_api.Common;
using student_log_api.DBLibrary;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Services
{
    public class ConfigurationsServices : IConfigurationsInterface
    {
        public AppSettings AppSettings { get; }

        public ConfigurationsServices(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public async Task<SubjectsResponse> GetSubjects(
            int accountID,
            int schoolID,
            bool includeInactive)
        {
            var response = new SubjectsResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "SchoolID", schoolID },
                    { "IncludeInactive", includeInactive }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_SUBJECTS, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<SubjectItem>()
                    : JsonConvert.DeserializeObject<List<SubjectItem>>(result)
                        ?? new List<SubjectItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<SubjectsResponse> GetSubject(
            int accountID,
            int subjectID)
        {
            var response = new SubjectsResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "SubjectID", subjectID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_SUBJECT_BY_ID, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<SubjectItem>()
                    : JsonConvert.DeserializeObject<List<SubjectItem>>(result)
                        ?? new List<SubjectItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> UpsertSubject(
            SubjectRequest request,
            int accountID,
            int userID)
        {
            var response = new ServiceResponse();

            try
            {
                if (request == null)
                {
                    response.addWarning("Subject request is required.");
                    return response;
                }

                // if (request.SchoolID == 0)
                // {
                //     response.addWarning("School is required.");
                //     return response;
                // }

                if (string.IsNullOrWhiteSpace(request.SubjectName))
                {
                    response.addWarning("Subject name is required.");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    { "SubjectID", request.SubjectID },
                    { "AccountID", accountID },
                    { "SchoolID", request.SchoolID },
                    { "SubjectName", request.SubjectName.Trim() },
                    { "SubjectCode", request.SubjectCode?.Trim() ?? string.Empty },
                    { "IsActive", request.IsActive ? 1: 0 },
                    { "LoginUserID", userID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.UPSERT_SUBJECT, sqlParams);

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> DeactivateSubject(
            int subjectID,
            int accountID,
            int userID)
        {
            var response = new ServiceResponse();

            try
            {
                if (subjectID == 0)
                {
                    response.addWarning("Subject is required.");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    { "SubjectID", subjectID },
                    { "AccountID", accountID },
                    { "LoginUserID", userID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.DEACTIVATE_SUBJECT, sqlParams);

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

    }
}