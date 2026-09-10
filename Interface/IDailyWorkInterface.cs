using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Interface
{
    public interface IDailyWorkInterface
    {
        Task<ServiceResponse> AssignWork(DailyWorkRequest request, int accountID, int userID);
        Task<DailyWorkResponse> GetTeacherWork(int accountID, int userID, DateTime? date);
        Task<DailyWorkResponse> GetStudentWork(int accountID, int studentID, DateTime date);
        Task<DailyWorkResponse> GetWorkDetails(int accountID, int workID, int userID);
        Task<DailyWorkResponse> GetAssignedStudents(int accountID, int workID, int userID);
        Task<ServiceResponse> SubmitWork(StudentWorkSubmissionRequest request, int accountID, int studentID);
        Task<DailyWorkResponse> GetSubmissions(int accountID, int workID, int userID);
        Task<ServiceResponse> ReviewSubmission(ReviewSubmissionRequest request, int accountID, int userID, bool returnForCorrection);
    }

    public class DailyWorkResponse : ServiceResponse
    {
        public List<DailyWorkItem> Result { get; set; } = new();
    }
}