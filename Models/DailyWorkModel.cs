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
        public DateTime? DueDate { get; set; }
        public TimeSpan? DueTime { get; set; }
        public bool EntireClass { get; set; }
        public List<int>? StudentIDs { get; set; } = new();
    }

    public class DailyWorkItem
    {
        public int WorkID { get; set; }
        public int AssignmentID { get; set; }
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
        public DateTime? DueDate { get; set; }
        public TimeSpan? DueTime { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? Marks { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public string TeacherRemarks { get; set; } = string.Empty;
        public DateTime? SubmittedDate { get; set; }
    }

    public class StudentWorkSubmissionRequest
    {
        public int AssignmentID { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public string AttachmentName { get; set; } = string.Empty;
        public string AttachmentUrl { get; set; } = string.Empty;
    }

    public class ReviewSubmissionRequest
    {
        public int SubmissionID { get; set; }
        public int? Marks { get; set; }
        public string TeacherRemarks { get; set; } = string.Empty;
    }
}