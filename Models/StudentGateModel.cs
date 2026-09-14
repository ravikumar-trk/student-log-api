using student_log_api.Common;

namespace student_log_api.Models;

public class StudentGateSwipeRequest
{
    public int AccountID { get; set; }
    public int SchoolID { get; set; }
    public string GateCode { get; set; } = string.Empty;
    public string DeviceCode { get; set; } = string.Empty;
    public string? CardNumber { get; set; }
    public string? RFIDUID { get; set; }
    public string? DeviceEventID { get; set; }
    public string? EventType { get; set; }
    public DateTime DeviceEventTime { get; set; }
}

public class StudentGateSwipeResult
{
    public bool Success { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public long? GateEventID { get; set; }
    public int? StudentID { get; set; }
    public string? StudentName { get; set; }
    public string? EventType { get; set; }
    public string? CurrentStatus { get; set; }
}

public class StudentGateSwipeResponse : ServiceResponse
{
    public StudentGateSwipeResult Result { get; set; } = new();
}

public class StudentGateManualRequest
{
    public int SchoolID { get; set; }
    public int StudentID { get; set; }
    public string GateCode { get; set; } = string.Empty;
    public string? EventType { get; set; }
    public DateTime? EventTime { get; set; }
    public string? Remarks { get; set; }
}

public class StudentGateDashboardRequest
{
    public int SchoolID { get; set; }
    public DateTime? AttendanceDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? Search { get; set; }
    public string? Status { get; set; }
}

public class StudentGateDashboardSummary
{
    public long TotalRows { get; set; }
    public int CurrentlyInside { get; set; }
    public int CurrentlyOutside { get; set; }
    public int NotYetEntered { get; set; }
    public int LateArrivals { get; set; }
    public int EarlyExits { get; set; }
}

public class StudentGateDashboardRow
{
    public int StudentID { get; set; }
    public string AdmissionNo { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public int ClassID { get; set; }
    public int? SectionID { get; set; }
    public DateTime? FirstInTime { get; set; }
    public DateTime? LastOutTime { get; set; }
    public int TotalPresenceMinutes { get; set; }
    public int NumberOfEntries { get; set; }
    public int NumberOfExits { get; set; }
    public string CurrentStatus { get; set; } = string.Empty;
    public string? LastEventType { get; set; }
    public DateTime? LastSwipeTime { get; set; }
    public string? LastGate { get; set; }
}

public class StudentGateDashboardResponse : ServiceResponse
{
    public StudentGateDashboardSummary Summary { get; set; } = new();
    public List<StudentGateDashboardRow> Result { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class StudentGateLiveEvent
{
    public long GateEventID { get; set; }
    public int StudentID { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string AdmissionNo { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime DeviceEventTime { get; set; }
    public DateTime ServerReceivedTime { get; set; }
    public bool IsValid { get; set; }
    public string? ValidationCode { get; set; }
    public string? GateName { get; set; }
}

public class StudentGateLiveResponse : ServiceResponse
{
    public List<StudentGateLiveEvent> Result { get; set; } = new();
}

public class StudentGateHistoryEvent
{
    public long GateEventID { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public DateTime DeviceEventTime { get; set; }
    public DateTime ServerReceivedTime { get; set; }
    public bool IsValid { get; set; }
    public string? ValidationCode { get; set; }
    public long? GateID { get; set; }
    public string? GateName { get; set; }
}

public class StudentGateDailySummary
{
    public DateTime AttendanceDate { get; set; }
    public DateTime? FirstInTime { get; set; }
    public DateTime? LastOutTime { get; set; }
    public int TotalPresenceMinutes { get; set; }
    public int NumberOfEntries { get; set; }
    public int NumberOfExits { get; set; }
    public string CurrentStatus { get; set; } = string.Empty;
    public bool IsPresent { get; set; }
}

public class StudentGateHistoryResponse : ServiceResponse
{
    public List<StudentGateHistoryEvent> Events { get; set; } = new();
    public List<StudentGateDailySummary> Result { get; set; } = new();
}
