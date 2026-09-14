using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Controllers;

[ApiController]
[Route("api/attendance")]
[Authorize]
public class StudentGateController : ControllerBase
{
    private readonly IStudentGateInterface service;

    public StudentGateController(IStudentGateInterface service)
    {
        this.service = service;
    }

    [HttpPost("swipe")]
    [AllowAnonymous]
    public async Task<IActionResult> RecordSwipe([FromBody] StudentGateSwipeRequest request, [FromHeader(Name = "X-Device-Key")] string? deviceKey)
    {
        if (string.IsNullOrWhiteSpace(deviceKey))
            return Unauthorized(new { Message = "Device authentication is required.", Code = "DEVICE_AUTH_REQUIRED" });

        var response = await service.RecordSwipe(request, request?.AccountID ?? 0, deviceKey);
        if (response.HasErrors) return StatusCode(StatusCodes.Status500InternalServerError, response);
        if (response.HasWarnings) return BadRequest(response);
        return Ok(response);
    }

    [HttpPost("manual-swipe")]
    public async Task<IActionResult> RecordManualSwipe([FromBody] StudentGateManualRequest request)
    {
        if (!UserContext.ValidateUser(User, out var userID, out var accountID, out var message))
            return BadRequest(new { Message = message, StatusCode = 400 });
        if (!UserContext.HasSchoolAccess(User, request.SchoolID))
            return Forbid();

        var response = await service.RecordManualSwipe(request, accountID, userID);
        if (response.HasErrors) return StatusCode(StatusCodes.Status500InternalServerError, response);
        if (response.HasWarnings) return BadRequest(response);
        return Ok(response);
    }

    [HttpPost("dashboard")]
    public async Task<IActionResult> GetDashboard([FromBody] StudentGateDashboardRequest request)
    {
        if (!UserContext.ValidateUser(User, out var userID, out var accountID, out var message))
            return BadRequest(new { Message = message, StatusCode = 400 });
        if (!UserContext.HasSchoolAccess(User, request.SchoolID))
            return Forbid();
        var response = await service.GetDashboard(request, accountID, userID);
        return ToActionResult(response);
    }

    [HttpGet("live-events")]
    public async Task<IActionResult> GetLiveEvents([FromQuery] int schoolID, [FromQuery] DateTime? since = null, [FromQuery] int pageSize = 50)
    {
        if (!UserContext.ValidateUser(User, out var userID, out var accountID, out var message))
            return BadRequest(new { Message = message, StatusCode = 400 });
        if (!UserContext.HasSchoolAccess(User, schoolID))
            return Forbid();
        var response = await service.GetLiveEvents(accountID, schoolID, since, pageSize, userID);
        return ToActionResult(response);
    }

    [HttpGet("student/{studentID:int}/history")]
    public async Task<IActionResult> GetHistory(int studentID, [FromQuery] int schoolID, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        if (!UserContext.ValidateUser(User, out var userID, out var accountID, out var message))
            return BadRequest(new { Message = message, StatusCode = 400 });
        if (!UserContext.HasSchoolAccess(User, schoolID))
            return Forbid();
        if (fromDate == default || toDate == default || fromDate > toDate)
            return BadRequest(new { Message = "A valid date range is required.", StatusCode = 400 });
        var response = await service.GetHistory(accountID, schoolID, studentID, fromDate, toDate, userID);
        return ToActionResult(response);
    }

    private IActionResult ToActionResult(ServiceResponse response)
    {
        if (response.HasErrors) return StatusCode(StatusCodes.Status500InternalServerError, response);
        if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
        return Ok(response);
    }
}
