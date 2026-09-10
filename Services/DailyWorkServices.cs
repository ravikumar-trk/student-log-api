using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using student_log_api.Common;
using student_log_api.DBLibrary;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Services
{
    public class DailyWorkServices : IDailyWorkInterface
    {
        public AppSettings AppSettings { get; }

        public DailyWorkServices(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
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

        public async Task<ServiceResponse> AssignWork(
            DailyWorkRequest request,
            int accountID,
            int userID)
        {
            var response = new ServiceResponse();

            try
            {
                if (request == null)
                {
                    response.addWarning("Daily work request is required.");
                    return response;
                }

                if (request.SchoolID == 0)
                {
                    response.addWarning("School is required.");
                    return response;
                }

                if (request.ClassID == 0)
                {
                    response.addWarning("Class is required.");
                    return response;
                }

                if (request.SubjectID == 0)
                {
                    response.addWarning("Subject is required.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    response.addWarning("Work title is required.");
                    return response;
                }

                if (request.WorkDate == default)
                {
                    response.addWarning("Work date is required.");
                    return response;
                }

                if (!request.EntireClass &&
                    (request.StudentIDs == null || request.StudentIDs.Count == 0))
                {
                    response.addWarning("At least one student must be selected.");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    { "WorkID", request.WorkID },
                    { "AccountID", accountID },
                    { "TeacherID", userID },
                    { "SchoolID", request.SchoolID },
                    { "ClassID", request.ClassID },
                    { "SectionID", request.SectionID },
                    { "SubjectID", request.SubjectID },
                    { "WorkType", request.WorkType },
                    { "Title", request.Title.Trim() },
                    { "Description", request.Description?.Trim() ?? string.Empty },
                    { "WorkDate", request.WorkDate.Date },
                    { "DueDate", (object?)request.DueDate?.Date ?? DBNull.Value },
                    { "DueTime", (object?)request.DueTime ?? DBNull.Value },
                    { "EntireClass", request.EntireClass },
                    {
                        "StudentIDsJson",
                        JsonConvert.SerializeObject(
                            request.StudentIDs ?? new List<int>())
                    },
                    { "LoginUserID", userID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.ASSIGN_DAILY_WORK, sqlParams);

                response.Message = "Work assigned successfully.";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
                response.addError(ex.ToString());
                response.Message = "Daily work assignment failed.";
            }

            return response;
        }

        public async Task<DailyWorkResponse> GetTeacherWork(
            int accountID,
            int userID,
            DateTime? date)
        {
            var response = new DailyWorkResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "TeacherID", userID },
                    { "WorkDate", (object?)date?.Date ?? DBNull.Value }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_TEACHER_DAILY_WORK, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<DailyWorkItem>()
                    : JsonConvert.DeserializeObject<List<DailyWorkItem>>(result)
                        ?? new List<DailyWorkItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<DailyWorkResponse> GetStudentWork(
            int accountID,
            int studentID,
            DateTime date)
        {
            var response = new DailyWorkResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "StudentID", studentID },
                    { "WorkDate", date.Date }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_STUDENT_DAILY_WORK, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<DailyWorkItem>()
                    : JsonConvert.DeserializeObject<List<DailyWorkItem>>(result)
                        ?? new List<DailyWorkItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<DailyWorkResponse> GetWorkDetails(
            int accountID,
            int workID,
            int userID)
        {
            var response = new DailyWorkResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "WorkID", workID },
                    { "LoginUserID", userID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_DAILY_WORK_DETAILS, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<DailyWorkItem>()
                    : JsonConvert.DeserializeObject<List<DailyWorkItem>>(result)
                        ?? new List<DailyWorkItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<DailyWorkResponse> GetAssignedStudents(
            int accountID,
            int workID,
            int userID)
        {
            var response = new DailyWorkResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "WorkID", workID },
                    { "LoginUserID", userID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_DAILY_WORK_STUDENTS, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<DailyWorkItem>()
                    : JsonConvert.DeserializeObject<List<DailyWorkItem>>(result)
                        ?? new List<DailyWorkItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> SubmitWork(
            StudentWorkSubmissionRequest request,
            int accountID,
            int studentID)
        {
            var response = new ServiceResponse();

            try
            {
                if (request == null)
                {
                    response.addWarning("Submission request is required.");
                    return response;
                }

                if (request.AssignmentID == 0)
                {
                    response.addWarning("Assignment is required.");
                    return response;
                }

                var sqlParams = new Dictionary<string, object>
                {
                    { "AssignmentID", request.AssignmentID },
                    { "AccountID", accountID },
                    { "StudentID", studentID },
                    { "AnswerText", request.AnswerText ?? string.Empty },
                    { "AttachmentName", request.AttachmentName ?? string.Empty },
                    { "AttachmentUrl", request.AttachmentUrl ?? string.Empty }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.SUBMIT_STUDENT_WORK, sqlParams);

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<DailyWorkResponse> GetSubmissions(
            int accountID,
            int workID,
            int userID)
        {
            var response = new DailyWorkResponse();

            try
            {
                var sqlParams = new Dictionary<string, object>
                {
                    { "AccountID", accountID },
                    { "WorkID", workID },
                    { "TeacherID", userID }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, SQLConstants.GET_DAILY_WORK_SUBMISSIONS, sqlParams);

                response.Result = string.IsNullOrWhiteSpace(result)
                    ? new List<DailyWorkItem>()
                    : JsonConvert.DeserializeObject<List<DailyWorkItem>>(result)
                        ?? new List<DailyWorkItem>();

                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.addError(ex.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> ReviewSubmission(
            ReviewSubmissionRequest request,
            int accountID,
            int userID,
            bool returnForCorrection)
        {
            var response = new ServiceResponse();

            try
            {
                if (request == null)
                {
                    response.addWarning("Review request is required.");
                    return response;
                }

                if (request.SubmissionID == 0)
                {
                    response.addWarning("Submission is required.");
                    return response;
                }

                var procedure = returnForCorrection
                    ? SQLConstants.RETURN_STUDENT_WORK
                    : SQLConstants.REVIEW_STUDENT_WORK;

                var sqlParams = new Dictionary<string, object>
                {
                    { "SubmissionID", request.SubmissionID },
                    { "AccountID", accountID },
                    { "LoginUserID", userID },
                    { "Marks", (object?)request.Marks ?? DBNull.Value },
                    { "TeacherRemarks", request.TeacherRemarks ?? string.Empty }
                };

                DBFactory factory = new DBFactory();
                IDBUtility DbUtility = factory.getDBUtility();
                var result = await DbUtility.GetjsonData(AppSettings.ConnectionString, procedure, sqlParams);

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