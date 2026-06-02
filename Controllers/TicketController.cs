using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace student_log_api.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    // [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketInterface _ticketInterface;

        public TicketController(ITicketInterface ticketInterface)
        {
            _ticketInterface = ticketInterface;
        }

        [HttpPost("create-ticket")]
        [SwaggerOperation("Create ticket", OperationId = "CreateTicket", Summary = "Create a new ticket", Description = "Creates a new ticket with the provided details")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceResponse), description: "Ticket created successfully")]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketPayload model)
        {
            // CreatedBy 
            // CreatedOn
            // AccountID
            CreateTicketPayloadV2 payloadV2 = new()
            {
                IssueType = model.IssueType,
                Description = model.Description,
                Priority = model.Priority,
                SchoolID = Convert.ToInt32(model.SchoolID),
                ExcelFile = model.ExcelFile,
                CreatedBy = 1, // This should ideally come from the authenticated user context
                CreatedOn = DateTime.UtcNow,
                AccountID = 1, // This should ideally be set based on the authenticated user's account
                AccountCode = "TRK" // This should ideally be set based on the authenticated user's account
            };

            ServiceResponse response = await _ticketInterface.CreateTicket(payloadV2);
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