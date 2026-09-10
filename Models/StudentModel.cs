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

    public class UpserStudentModel
    {
        public int? StudentID { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int? AccountID { get; set; }
        public string? SchoolCode { get; set; }
        public string? ClassCode { get; set; }
        public int? SectionID { get; set; }
        public string? AdmissionNo { get; set; }
        public int? RollNo { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? Contact1 { get; set; }
        public string? Contact2 { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public bool? IsActive { get; set; }
        public string? School { get; set; }
        public string? Class { get; set; }
        public string? StudentName { get; set; }
    }

    public class UpsertStudentsModel
    {
        public List<UpserStudentModel>? Students { get; set; }
    }

    public class UpsertStudentResult
    {
        public int RowNo { get; set; }
        public int? StudentID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SchoolCode { get; set; }
        public int? SchoolID { get; set; }
        public string? ClassCode { get; set; }
        public int? ClassID { get; set; }
        public string? AdmissionNo { get; set; }
        public string? Status { get; set; }
        public string? Operation { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class UpsertStudentsResponse : ServiceResponse
    {
        public List<UpsertStudentResult> Result { get; set; } = new();
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