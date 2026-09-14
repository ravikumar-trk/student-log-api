using student_log_api.Common;

namespace student_log_api.Models
{
    public class DailyWorkRequest
    {
        public int WorkID { get; set; }
        public int SchoolID { get; set; }
        public int ClassID { get; set; }
        public int SectionID { get; set; }
        public int SubjectID { get; set; }
        public string WorkType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime WorkDate { get; set; }
    }

    public class DailyWorkItem
    {
        public int WorkID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }
        public int ClassID { get; set; }
        public int SectionID { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime WorkDate { get; set; }
        public int? Marks { get; set; }
        public string TeacherRemarks { get; set; } = string.Empty;
    }
}