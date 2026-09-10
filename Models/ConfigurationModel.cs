using student_log_api.Common;

namespace student_log_api.Models
{
    public class SubjectRequest
    {
        public int SubjectID { get; set; }
        public int SchoolID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class SubjectItem
    {
        public int SubjectID { get; set; }
        public int SchoolID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public string ModifiedDate { get; set; } = string.Empty;
    }

    public class SubjectsResponse : ServiceResponse
    {
        public List<SubjectItem> Result { get; set; } = new();
    }
}