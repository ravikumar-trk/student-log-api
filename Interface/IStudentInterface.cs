using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Interface
{
    public interface IStudentInterface
    {
        Task<StudentDataModel> GetStudentsList(GetStudentDataModel obj);
    }
}
