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
    public class TicketData
    {
        public int SchoolID { get; set; }
        public int TicketID { get; set; }
        public string TicketName { get; set; } = String.Empty;
        public string IssueType { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Priority { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public string CreatedBy { get; set; } = String.Empty;
        public string CreatedOn { get; set; } = String.Empty;
    }

    public class TicketDataModel : ServiceResponse
    {
        public List<TicketData> Result { get; set; }
    }


    public class TicketDetailsData
    {
        public int TicketID { get; set; }

        public string TicketName { get; set; } = string.Empty;

        public string IssueType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public int AccountID { get; set; }

        public int SchoolID { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int? AssignedTo { get; set; }

        public string AssignedToName { get; set; } = string.Empty;

        public DateTime? AssignedOn { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; } = string.Empty;
        public string CreatedByUserEmail { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public string UpdatedByName { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; }
    }

    public class TicketDetailsDataModel : ServiceResponse
    {
        public List<TicketDetailsData> Result { get; set; }
    }
}