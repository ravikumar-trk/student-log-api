using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;

namespace student_log_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountInterface _accountInterface;

        public AccountController(IAccountInterface accountInterface)
        {
            _accountInterface = accountInterface;
        }

        [Route("GetAccountList"), HttpPost]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetAccountDetails", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(AccountDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetAccountList([FromBody] GetAccountDataModel obj)
        {
            AccountDataModel response = await _accountInterface.GetAccountList(obj);
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

        [Route("GetAccountDetails/{accountID}"), HttpGet]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetAccountDetails", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(AccountDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetAccountDetails([FromRoute][Required] int accountID)
        {
            AccountDataModel response = await _accountInterface.GetAccountDetails(accountID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 200
                    });
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 500
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    StatusCode = 400
                });
            }
        }

        // get schools by account id
        [Route("GetSchoolsByAccountID/{accountID}"), HttpGet]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetSchoolsByAccountID", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(SchoolsDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetSchoolsByAccountID([FromRoute][Required] int accountID)
        {
            SchoolsDataModel response = await _accountInterface.GetSchoolsByAccountID(accountID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 200
                    });
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 500
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    StatusCode = 400
                });
            }
        }

        [Route("GetUsersByAccountID/{accountID}"), HttpGet]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetUsersByAccountID", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(UsersDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetUsersByAccountID([FromRoute][Required] int accountID)
        {
            UsersDataModel response = await _accountInterface.GetUsersByAccountID(accountID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 200
                    });
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        StatusCode = 500
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    StatusCode = 400
                });
            }
        }
    }
}