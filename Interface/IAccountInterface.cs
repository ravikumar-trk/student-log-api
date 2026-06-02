using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Interface
{
    public interface IAccountInterface
    {
        Task<AccountDataModel> GetAccountList(GetAccountDataModel obj);
        Task<AccountDataModel> GetAccountDetails(int accountID);
        Task<SchoolsDataModel> GetSchoolsByAccountID(int accountID);
        Task<UsersDataModel> GetUsersByAccountID(int accountID);
        Task<ClassesDataModel> GetClassesData(int accountID, int schoolID, int loginUserID);
        Task<ServiceResponse> UpsertClasses(UpsertClassesModel obj);
    }
}
