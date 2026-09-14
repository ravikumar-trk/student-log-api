using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using student_log_api.Common;
using student_log_api.DBLibrary;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Services;

public class StudentGateServices : IStudentGateInterface
{
    private readonly AppSettings appSettings;

    public StudentGateServices(IOptions<AppSettings> appSettings)
    {
        this.appSettings = appSettings.Value;
    }

    public async Task<StudentGateSwipeResponse> RecordSwipe(StudentGateSwipeRequest request, int accountID, string deviceApiKey)
    {
        var response = new StudentGateSwipeResponse();
        if (request == null || request.SchoolID <= 0 || string.IsNullOrWhiteSpace(request.GateCode) || string.IsNullOrWhiteSpace(request.DeviceCode) ||
            (string.IsNullOrWhiteSpace(request.CardNumber) && string.IsNullOrWhiteSpace(request.RFIDUID)) || request.DeviceEventTime == default)
        {
            response.addWarning("School, gate, device, card/RFID, and device event time are required.");
            return response;
        }

        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["AccountID"] = accountID,
                ["SchoolID"] = request.SchoolID,
                ["GateCode"] = request.GateCode.Trim(),
                ["DeviceCode"] = request.DeviceCode.Trim(),
                ["CardNumber"] = (object?)request.CardNumber?.Trim() ?? DBNull.Value,
                ["RFIDUID"] = (object?)request.RFIDUID?.Trim() ?? DBNull.Value,
                ["DeviceEventID"] = (object?)request.DeviceEventID?.Trim() ?? DBNull.Value,
                ["RequestedEventType"] = (object?)request.EventType?.Trim().ToUpperInvariant() ?? DBNull.Value,
                ["DeviceEventTime"] = request.DeviceEventTime.ToUniversalTime(),
                ["ServerReceivedTime"] = DateTime.UtcNow,
                ["DeviceApiKeyHash"] = HashApiKey(deviceApiKey)
            };

            var db = new DBFactory().getDBUtility();
            var json = await db.GetjsonData(appSettings.ConnectionString, SQLConstants.RECORD_STUDENT_GATE_SWIPE, parameters);
            response.Result = DeserializeSingle<StudentGateSwipeResult>(json) ?? new StudentGateSwipeResult { Success = false, Code = "NO_RESULT", Message = "No swipe result returned." };
            response.Message = response.Result.Message;
        }
        catch (Exception ex)
        {
            response.addError(ex.Message);
        }

        return response;
    }

    public async Task<StudentGateSwipeResponse> RecordManualSwipe(StudentGateManualRequest request, int accountID, int userID)
    {
        var response = new StudentGateSwipeResponse();
        if (request == null || request.SchoolID <= 0 || request.StudentID <= 0 || string.IsNullOrWhiteSpace(request.GateCode))
        {
            response.addWarning("School, student, and gate are required.");
            return response;
        }

        try
        {
            var db = new DBFactory().getDBUtility();
            var json = await db.GetjsonData(appSettings.ConnectionString, SQLConstants.RECORD_MANUAL_STUDENT_GATE_SWIPE, new Dictionary<string, object>
            {
                ["AccountID"] = accountID,
                ["SchoolID"] = request.SchoolID,
                ["StudentID"] = request.StudentID,
                ["GateCode"] = request.GateCode.Trim(),
                ["RequestedEventType"] = (object?)request.EventType?.Trim().ToUpperInvariant() ?? DBNull.Value,
                ["EventTime"] = (request.EventTime ?? DateTime.UtcNow).ToUniversalTime(),
                ["CreatedBy"] = userID,
                ["Remarks"] = (object?)request.Remarks?.Trim() ?? DBNull.Value
            });
            response.Result = DeserializeSingle<StudentGateSwipeResult>(json);
            response.Message = response.Result.Message;
        }
        catch (Exception ex)
        {
            response.addError(ex.Message);
        }

        return response;
    }

    public async Task<StudentGateDashboardResponse> GetDashboard(StudentGateDashboardRequest request, int accountID, int userID)
    {
        var response = new StudentGateDashboardResponse
        {
            PageNumber = Math.Max(1, request.PageNumber),
            PageSize = Math.Clamp(request.PageSize, 1, 200)
        };
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["AccountID"] = accountID,
                ["SchoolID"] = request.SchoolID,
                ["AttendanceDate"] = (request.AttendanceDate ?? DateTime.UtcNow).Date,
                ["PageNumber"] = response.PageNumber,
                ["PageSize"] = response.PageSize,
                ["Search"] = (object?)request.Search?.Trim() ?? DBNull.Value,
                ["Status"] = (object?)request.Status?.Trim().ToUpperInvariant() ?? DBNull.Value
            };
            var db = new DBFactory().getDBUtility();
            var json = await db.GetjsonDataFromDataset(appSettings.ConnectionString, SQLConstants.GET_STUDENT_GATE_DASHBOARD, parameters);
            var dataset = JsonConvert.DeserializeObject<JObject>(json) ?? new JObject();
            var summaryRows = dataset["Table"]?.ToObject<List<JObject>>() ?? new();
            var dashboardRows = dataset["Table1"]?.ToObject<List<JObject>>() ?? new();
            response.Summary = summaryRows.Select(x => x.ToObject<StudentGateDashboardSummary>()).FirstOrDefault() ?? new();
            response.Result = dashboardRows.Select(x => x.ToObject<StudentGateDashboardRow>()).Where(x => x != null).Cast<StudentGateDashboardRow>().ToList();
            response.Message = "Success";
        }
        catch (Exception ex)
        {
            response.addError(ex.Message);
        }
        return response;
    }

    public async Task<StudentGateLiveResponse> GetLiveEvents(int accountID, int schoolID, DateTime? since, int pageSize, int userID)
    {
        var response = new StudentGateLiveResponse();
        try
        {
            var db = new DBFactory().getDBUtility();
            var json = await db.GetjsonData(appSettings.ConnectionString, SQLConstants.GET_STUDENT_GATE_LIVE_EVENTS, new Dictionary<string, object>
            {
                ["AccountID"] = accountID,
                ["SchoolID"] = schoolID,
                ["Since"] = (object?)since?.ToUniversalTime() ?? DBNull.Value,
                ["PageSize"] = Math.Clamp(pageSize, 1, 100)
            });
            response.Result = DeserializeList<StudentGateLiveEvent>(json);
            response.Message = "Success";
        }
        catch (Exception ex) { response.addError(ex.Message); }
        return response;
    }

    public async Task<StudentGateHistoryResponse> GetHistory(int accountID, int schoolID, int studentID, DateTime fromDate, DateTime toDate, int userID)
    {
        var response = new StudentGateHistoryResponse();
        try
        {
            var db = new DBFactory().getDBUtility();
            var json = await db.GetjsonDataFromDataset(appSettings.ConnectionString, SQLConstants.GET_STUDENT_GATE_HISTORY, new Dictionary<string, object>
            {
                ["AccountID"] = accountID,
                ["SchoolID"] = schoolID,
                ["StudentID"] = studentID,
                ["FromDate"] = fromDate.Date,
                ["ToDate"] = toDate.Date
            });
            var dataset = JsonConvert.DeserializeObject<JObject>(json) ?? new JObject();
            var eventRows = dataset["Table"]?.ToObject<List<JObject>>() ?? new();
            var summaryRows = dataset["Table1"]?.ToObject<List<JObject>>() ?? new();
            response.Events = eventRows.Select(x => x.ToObject<StudentGateHistoryEvent>()).Where(x => x != null).Cast<StudentGateHistoryEvent>().ToList();
            response.Result = summaryRows.Select(x => x.ToObject<StudentGateDailySummary>()).Where(x => x != null).Cast<StudentGateDailySummary>().ToList();
            response.Message = "Success";
        }
        catch (Exception ex) { response.addError(ex.Message); }
        return response;
    }

    private static byte[] HashApiKey(string value) => SHA256.HashData(Encoding.UTF8.GetBytes(value ?? string.Empty));

    private static T DeserializeSingle<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return default!;
        var values = JsonConvert.DeserializeObject<List<T>>(json);
        return values is { Count: > 0 } ? values[0] : default!;
    }
    private static List<T> DeserializeList<T>(string json) => string.IsNullOrWhiteSpace(json) ? new() : JsonConvert.DeserializeObject<List<T>>(json) ?? new();
}
