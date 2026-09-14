using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/daily-work")]
    public class DailyWorkController : ControllerBase
    {
        private readonly IDailyWorkInterface service;
        public DailyWorkController(IDailyWorkInterface service) => this.service = service;

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] DailyWorkRequest request)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            ServiceResponse response = await service.AssignWork(request, loginAccountID, loginUserID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpGet("teacher")]
        public async Task<IActionResult> TeacherWork([FromQuery] DateTime? date = null)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            DailyWorkResponse response = await service.GetTeacherWork(loginAccountID, loginUserID, date);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> StudentWork(
            [FromQuery] int schoolID,
            [FromQuery] int classID,
            [FromQuery] DateTime date)
        {
            if (!UserContext.ValidateUser(User, out _, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            if (schoolID == 0 || classID == 0)
            {
                return BadRequest(new { Message = "School and class are required.", StatusCode = 400 });
            }

            DailyWorkResponse response = await service.GetStudentWork(
                loginAccountID,
                schoolID,
                classID,
                date == default ? DateTime.Today : date);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpGet("{workID:int}")]
        public async Task<IActionResult> Details(int workID)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            DailyWorkResponse response = await service.GetWorkDetails(loginAccountID, workID, loginUserID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

    }
}