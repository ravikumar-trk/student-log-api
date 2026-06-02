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

namespace student_log_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthInterface _authInterface;
        IJwtTokenService _jwtTokenService;

        public AuthController(IAuthInterface authInterface, IJwtTokenService jwtTokenService)
        {
            _authInterface = authInterface;
            _jwtTokenService = jwtTokenService;
        }

        [Route("login"), HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("User login with email", OperationId = "Login", Summary = "Login user with email", Description = "Authenticates user by email and returns JWT token")]
        [SwaggerResponse(statusCode: 200, type: typeof(LoginResponseModel), description: "Login successful, returns JWT token")]
        [SwaggerResponse(statusCode: 401, description: "User not found or unauthorized")]
        [SwaggerResponse(statusCode: 400, description: "Invalid request")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new LoginResponseModel
                    {
                        Message = "Email is required",
                        Token = null,
                        // User = null
                    });
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new LoginResponseModel
                    {
                        Message = "Password is required",
                        Token = null,
                        // User = null
                    });
                }

                // Check if user exists in GEN_Users table
                var user = await _authInterface.GetUserByEmail(request.Email, request.Password);

                if (user == null || user.Type == 0)
                {
                    return Unauthorized(new LoginResponseModel
                    {
                        Message = user?.Message ?? "User not found or unauthorized",
                        Token = null,
                        // User = null
                    });
                }

                // Create user token data
                var userTokenData = new UserTokenData
                {
                    UserID = user.UserID,
                    Email = user.Email,
                    UserName = user.UserName,
                    AccountID = user.AccountID,
                    AccountCode = user.AccountCode,
                    UserType = user.UserType,
                    SchoolIDs = user.SchoolIDs ?? string.Empty,
                    SchoolNames = user.SchoolNames ?? string.Empty
                };

                // Generate JWT token
                var token = _jwtTokenService.GenerateToken(userTokenData);

                return Ok(new LoginResponseModel
                {
                    Message = "Valid User, Token generated successfully",
                    Token = token,
                    // User = userTokenData
                });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new LoginResponseModel
                {
                    Message = $"Database error during login: {sqlEx.Message}",
                    Token = null,
                    // User = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new LoginResponseModel
                {
                    Message = $"An error occurred during login: {ex.Message}",
                    Token = null,
                    // User = null
                });
            }
        }
    }
}