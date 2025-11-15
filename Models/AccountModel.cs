using student_log_api.Common;
using System.ComponentModel;

namespace student_log_api.Models
{
    public class GetAccountDataModel
    {
        [DefaultValue(false)]
        public bool isDropdown { get; set; }
        public int accountID { get; set; }
        public int loginUserID { get; set; }
    }

    public class AccountDataModel : ServiceResponse
    {
        public List<AccountDataModelData> Result { get; set; }
    }

    public class AccountDataModelData
    {
        public int ID { get; set; }
        public string AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PlanCode { get; set; }
        public string Email { get; set; }
        public int Schools { get; set; }
        public int Users { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }

    public class SchoolsDataModel : ServiceResponse
    {
        public List<SchoolsDataModelData> Result { get; set; }
    }

    public class SchoolsDataModelData
    {
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string Status { get; set; }
    }

    public class UsersDataModel : ServiceResponse
    {
        public List<UsersDataModelData> Result { get; set; }
    }

    public class UsersDataModelData
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int UserTypeID { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string SchoolIDs { get; set; }
        public string SchoolNames { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string Status { get; set; }
    }
}
