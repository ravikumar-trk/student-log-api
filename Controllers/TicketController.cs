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

        // get ticket list
        [HttpGet("tickets")]
        [SwaggerOperation("Get list of tickets", OperationId = "GetTickets", Summary = "Get a list of all tickets", Description = "Retrieves a list of all tickets")]
        [SwaggerResponse(statusCode: 200, type: typeof(TicketDataModel), description: "List of tickets retrieved successfully")]
        public async Task<IActionResult> GetTickets([FromQuery][Required] int accountID, [FromQuery][Required] string schoolIDs)
        {
            TicketDataModel response = await _ticketInterface.GetTicketList(accountID, schoolIDs);

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

        // Get Ticket details by ticket id - can be used in future for ticket details page
        [HttpGet("{ticketID}/ticket-details")]
        [SwaggerOperation("Get ticket details", OperationId = "GetTicketDetails", Summary = "Get details of a specific ticket", Description = "Retrieves details of a specific ticket based on the provided ticket ID")]
        [SwaggerResponse(statusCode: 200, type: typeof(TicketDetailsDataModel), description: "Ticket details retrieved successfully")]
        public async Task<IActionResult> GetTicketDetails([FromRoute][Required] int ticketID)
        {
            // This method can be implemented in future to get ticket details by ticket id
            TicketDetailsDataModel response = await _ticketInterface.GetTicketDetails(ticketID);
            return Ok(response);
        }

    }
}