using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Interface
{
    public interface IConfigurationsInterface
    {
        Task<SubjectsResponse> GetSubjects(int accountID, int schoolID, bool includeInactive);
        Task<SubjectsResponse> GetSubject(int accountID, int subjectID);
        Task<ServiceResponse> UpsertSubject(SubjectRequest request, int accountID, int userID);
        Task<ServiceResponse> DeactivateSubject(int subjectID, int accountID, int userID);

    }

    public class ConfigurationsResponse : ServiceResponse
    {
        public List<DailyWorkItem> Result { get; set; } = new();
    }
}