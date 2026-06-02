using student_log_api.Common;
using System.ComponentModel;

namespace student_log_api.Models
{
    public class GetStudentDataModel
    {
        [DefaultValue("")]
        public string? Prefix { get; set; }
        [DefaultValue(0)]
        public int StudentID { get; set; }
        [DefaultValue(0)]
        public int ClassID { get; set; }
        [DefaultValue(0)]
        public int SchoolID { get; set; }
        [DefaultValue(2)]
        public int AccountID { get; set; }
        [DefaultValue(false)]
        public bool IsDropdown { get; set; }
        [DefaultValue(2)]
        public int LoginUserID { get; set; }
    }

    public class StudentDataModel : ServiceResponse
    {
        public List<StudentDataModelData> Result { get; set; }
    }

    public class StudentDataModelData
    {
        public int AccountID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public string AdmissionNo { get; set; }
        public string RollNo { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }

    }


}