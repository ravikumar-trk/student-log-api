using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Controllers
{
    [Route("api/student")]
    [ApiController]
    // [Authorize]
    public class StudentController : ControllerBase
    {
        IStudentInterface _studentInterface;

        public StudentController(IStudentInterface studentInterface)
        {
            _studentInterface = studentInterface;
        }

        [HttpGet("students")]
        [SwaggerOperation("Get data from query parameters and return result as array", OperationId = "GetStudentsList", Summary = "Get data from query parameters and return result as array", Description = "Get data from query parameters and return result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(StudentDataModel), description: "Get data from query parameters and return result as array")]
        public async Task<IActionResult> GetStudentsList([FromQuery] GetStudentDataModel obj)
        {
            StudentDataModel response = await _studentInterface.GetStudentsList(obj);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(response);
                }
                else if (response.HasWarnings)
                {
                    return StatusCode(StatusCodes.Status202Accepted, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}