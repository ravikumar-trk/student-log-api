using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Interface
{
    public interface IAuthInterface
    {
        Task<UserDetailsModel> GetUserByEmail(string email, string Password);
    }
}
