using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;
using student_log_api.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace student_log_api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        IAccountInterface _accountInterface;

        public AccountController(IAccountInterface accountInterface)
        {
            _accountInterface = accountInterface;
        }

        [Route("accounts"), HttpPost]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetAccountDetails", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(AccountDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetAccountList([FromBody] GetAccountDataModel obj)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
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

        [Route("{accountID}/account-details"), HttpGet]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetAccountDetails", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(AccountDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetAccountDetails([FromRoute][Required] int accountID)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            AccountDataModel response = await _accountInterface.GetAccountDetails(accountID);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(response);
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
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
        [Route("{accountID}/schools"), HttpGet]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetSchoolsByAccountID", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(SchoolsDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetSchoolsByAccountID([FromRoute][Required] int accountID, [FromQuery] int IsActive = 1)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            SchoolsDataModel response = await _accountInterface.GetSchoolsByAccountID(accountID, IsActive);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(response);
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
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

        [Route("{accountID}/users"), HttpGet]
        [SwaggerOperation("Get data from boady and send result as array", OperationId = "GetUsersByAccountID", Summary = "Get data from boady and send result as array", Description = "Get data from boady and send result as array")]
        [SwaggerResponse(statusCode: 200, type: typeof(UsersDataModel), description: "Get data from boady and send result as array")]
        public async Task<IActionResult> GetUsersByAccountID([FromRoute][Required] int accountID, [FromQuery] int IsActive = 1)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }

            UsersDataModel response = await _accountInterface.GetUsersByAccountID(accountID, IsActive);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(response);
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
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

        [HttpGet]
        [Route("{accountID}/school/{schoolID}/classes"), HttpGet]
        [SwaggerOperation("Get classes data by account and login user", OperationId = "GetClassesData", Summary = "Get classes data", Description = "Fetch classes data using accountID and loginUserID as query parameters")]
        [SwaggerResponse(statusCode: 200, type: typeof(ClassesDataModel), description: "Classes data fetched successfully")]
        public async Task<IActionResult> GetClassesData([FromRoute] int accountID, [FromRoute] int schoolID, [FromQuery] int loginUserID)
        {
            // if (!UserContext.ValidateUser(
            // User,
            // out int loginUserID,
            // out int loginAccountID,
            // out string message))
            // {
            //     return BadRequest(new
            //     {
            //         Message = message,
            //         StatusCode = 400
            //     });
            // }
            ClassesDataModel response = await _accountInterface.GetClassesData(accountID, schoolID, loginUserID);

            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(response);
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Data = response.Result,
                        Message = response.Message,
                        Warnings = response.Warnings,
                        Errors = response.Errors,
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

        // Upsert classes via JSON payload
        [HttpPost]
        [Route("UpsertClasses")]
        [SwaggerOperation("Upsert classes via JSON payload", OperationId = "UpsertClasses", Summary = "Upsert classes using a JSON payload", Description = "Upsert classes using a JSON payload that includes loginUserID and a classes array")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceResponse), description: "Upsert result")]
        public async Task<IActionResult> UpsertClasses([FromBody] UpsertClassesModel obj)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            ServiceResponse response = await _accountInterface.UpsertClasses(obj);
            if (response != null)
            {
                if (!response.HasWarnings && !response.HasErrors)
                {
                    return Ok(response);
                }
                else if (response.HasWarnings)
                {
                    return Accepted(new
                    {
                        Message = response.Message,
                        Warnings = response.Warnings,
                        StatusCode = 202
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Message = response.Message,
                        Errors = response.Errors,
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

        [HttpPost]
        [Route("school")]
        [SwaggerOperation("Add School", OperationId = "AddSchool", Summary = "Add a new school", Description = "Add a new school with the provided details in the request body")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceResponse), description: "School added successfully")]
        public async Task<IActionResult> AddSchool([FromBody] AddSchoolModelPayload school)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            ServiceResponse response = await _accountInterface.AddSchool(school, loginAccountID, loginUserID);
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

        [HttpPatch]
        [Route("school")]
        [SwaggerOperation("Update School", OperationId = "UpdateSchool", Summary = "Update school", Description = "Update school with the provided details in the request body")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceResponse), description: "School added successfully")]
        public async Task<IActionResult> UpdateSchool([FromBody] UpdateSchoolModelPayload school)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            ServiceResponse response = await _accountInterface.UpdateSchool(school, loginAccountID, loginUserID);
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

        [HttpPost]
        [Route("user")]
        [SwaggerOperation("Add User", OperationId = "AddUser", Summary = "Add a new user", Description = "Add a new user with the provided details in the request body")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceResponse), description: "User added successfully")]
        public async Task<IActionResult> AddUser([FromBody] AddUserModelPayload user)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            ServiceResponse response = await _accountInterface.AddUser(user, loginAccountID, loginUserID);
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

        [HttpPatch]
        [Route("user")]
        [SwaggerOperation("Update User", OperationId = "UpdateUser", Summary = "Update user", Description = "Update user with the provided details in the request body")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceResponse), description: "User added successfully")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModelPayload user)
        {
            if (!UserContext.ValidateUser(
            User,
            out int loginUserID,
            out int loginAccountID,
            out string message))
            {
                return BadRequest(new
                {
                    Message = message,
                    StatusCode = 400
                });
            }
            ServiceResponse response = await _accountInterface.UpdateUser(user, loginAccountID, loginUserID);
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