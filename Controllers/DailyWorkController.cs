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

        [HttpGet("student/{studentID:int}")]
        public async Task<IActionResult> StudentWork(int studentID, [FromQuery] DateTime date)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }
            //if (!int.TryParse(User.FindFirst("StudentID")?.Value, out var authenticatedStudentID) || authenticatedStudentID != studentID) return Forbid();

            DailyWorkResponse response = await service.GetStudentWork(loginAccountID, studentID, date == default ? DateTime.Today : date);
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

        [HttpGet("{workID:int}/students")]
        public async Task<IActionResult> AssignedStudents(int workID)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            DailyWorkResponse response = await service.GetAssignedStudents(loginAccountID, workID, loginUserID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpPost("submissions")]
        public async Task<IActionResult> Submit([FromBody] StudentWorkSubmissionRequest request)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }
            if (!int.TryParse(User.FindFirst("StudentID")?.Value, out var studentID)) return Forbid();

            ServiceResponse response = await service.SubmitWork(request, loginAccountID, studentID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpGet("{workID:int}/submissions")]
        public async Task<IActionResult> Submissions(int workID)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            DailyWorkResponse response = await service.GetSubmissions(loginAccountID, workID, loginUserID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpPatch("submissions/{submissionID:int}/review")]
        public async Task<IActionResult> Review(int submissionID, [FromBody] ReviewSubmissionRequest request)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }
            if (request == null) return BadRequest(new { Message = "Submission is required.", StatusCode = 400 });
            request.SubmissionID = submissionID;

            ServiceResponse response = await service.ReviewSubmission(request, loginAccountID, loginUserID, false);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpPatch("submissions/{submissionID:int}/return")]
        public async Task<IActionResult> Return(int submissionID, [FromBody] ReviewSubmissionRequest request)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }
            if (request == null) return BadRequest(new { Message = "Submission is required.", StatusCode = 400 });
            request.SubmissionID = submissionID;

            ServiceResponse response = await service.ReviewSubmission(request, loginAccountID, loginUserID, true);
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