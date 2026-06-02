using student_log_api.Common;

namespace student_log_api.Models
{
    // Login request model
    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Login response model
    public class LoginResponseModel : ServiceResponse
    {
        public string Token { get; set; }
        // public UserTokenData User { get; set; }
    }

    // User data to be stored in token
    public class UserTokenData
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AccountID { get; set; }
        public string AccountCode { get; set; }
        public string UserType { get; set; }
        public string SchoolIDs { get; set; }
        public string SchoolNames { get; set; }
    }

    // User details from GEN_Users table
    public class UserDetailsModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string AccountID { get; set; }
        public string AccountCode { get; set; }
        public string SchoolIDs { get; set; }
        public string SchoolNames { get; set; }
        public string Status { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
    }
}
