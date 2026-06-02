using student_log_api.Common;
using student_log_api.Models;
using System.Threading.Tasks;

namespace student_log_api.Interface
{
    public interface ITicketInterface
    {
        Task<ServiceResponse> CreateTicket(CreateTicketPayloadV2 model);
    }
}