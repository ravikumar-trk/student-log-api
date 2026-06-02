using Microsoft.AspNetCore.Http;
using student_log_api.Common;

namespace student_log_api.Models
{
    public class CreateTicketPayload
    {
        public string IssueType { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Priority { get; set; } = String.Empty;
        public string SchoolID { get; set; }
        public IFormFile ExcelFile { get; set; }
    }
    public class CreateTicketPayloadV2
    {
        public string IssueType { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Priority { get; set; } = String.Empty;
        public int CreatedBy { get; set; } = 0;
        public int AccountID { get; set; } = 0;
        public string AccountCode { get; set; } = String.Empty;
        public int SchoolID { get; set; }
        public DateTime CreatedOn { get; set; }
        public IFormFile ExcelFile { get; set; }
    }

    public class CreateTicketResponse
    {
        public int TicketID { get; set; }
        public string TicketName { get; set; } = String.Empty;
    }

    public class UploadExcelResponse : ServiceResponse
    {
        public string FileName { get; set; } = String.Empty;
    }
}