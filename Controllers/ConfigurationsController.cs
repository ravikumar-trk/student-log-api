using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/configurations")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfigurationsInterface service;
        public ConfigurationsController(IConfigurationsInterface service) => this.service = service;

        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects([FromQuery] int schoolID, [FromQuery] bool includeInactive = false)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            SubjectsResponse response = await service.GetSubjects(loginAccountID, schoolID, includeInactive);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpPost("subjects")]
        public async Task<IActionResult> UpsertSubject([FromBody] SubjectRequest request)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            ServiceResponse response = await service.UpsertSubject(request, loginAccountID, loginUserID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpGet("subjects/{subjectID:int}")]
        public async Task<IActionResult> GetSubject(int subjectID)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            SubjectsResponse response = await service.GetSubject(loginAccountID, subjectID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors) return Ok(response);
                else if (response.HasWarnings) return StatusCode(StatusCodes.Status202Accepted, response);
                else return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return BadRequest(response);
        }

        [HttpDelete("subjects/{subjectID:int}")]
        public async Task<IActionResult> DeactivateSubject(int subjectID)
        {
            if (!UserContext.ValidateUser(User, out int loginUserID, out int loginAccountID, out string message))
            {
                return BadRequest(new { Message = message, StatusCode = 400 });
            }

            ServiceResponse response = await service.DeactivateSubject(subjectID, loginAccountID, loginUserID);
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