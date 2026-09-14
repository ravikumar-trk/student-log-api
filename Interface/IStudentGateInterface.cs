using student_log_api.Models;

namespace student_log_api.Interface;

public interface IStudentGateInterface
{
    Task<StudentGateSwipeResponse> RecordSwipe(StudentGateSwipeRequest request, int accountID, string deviceApiKey);
    Task<StudentGateSwipeResponse> RecordManualSwipe(StudentGateManualRequest request, int accountID, int userID);
    Task<StudentGateDashboardResponse> GetDashboard(StudentGateDashboardRequest request, int accountID, int userID);
    Task<StudentGateLiveResponse> GetLiveEvents(int accountID, int schoolID, DateTime? since, int pageSize, int userID);
    Task<StudentGateHistoryResponse> GetHistory(int accountID, int schoolID, int studentID, DateTime fromDate, DateTime toDate, int userID);
}
