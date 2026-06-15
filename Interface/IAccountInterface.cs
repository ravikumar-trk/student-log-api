using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Interface
{
    public interface IAccountInterface
    {
        Task<AccountDataModel> GetAccountList(GetAccountDataModel obj);
        Task<AccountDataModel> GetAccountDetails(int accountID);
        Task<SchoolsDataModel> GetSchoolsByAccountID(int accountID, int IsActive);
        Task<UsersDataModel> GetUsersByAccountID(int accountID, int IsActive);
        Task<ClassesDataModel> GetClassesData(int accountID, int schoolID, int loginUserID);
        Task<ServiceResponse> UpsertClasses(UpsertClassesModel obj);
        Task<ServiceResponse> AddSchool(AddSchoolModelPayload school, int loginAccountID, int loginUserID);
        Task<ServiceResponse> UpdateSchool(UpdateSchoolModelPayload school, int loginAccountID, int loginUserID);
        Task<ServiceResponse> AddUser(AddUserModelPayload user, int loginAccountID, int loginUserID);
        Task<ServiceResponse> UpdateUser(UpdateUserModelPayload user, int loginAccountID, int loginUserID);
    }
}
