using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface IRequestService
    {
        /// <summary>
        /// Delete request.
        /// </summary>
        /// <param name="requestDto">Bank dto model</param>
        Task<bool> DeleteRequestAsync(int requestId);

        /// <summary>
        /// Get all request.
        /// </summary>
        /// <returns>list requests</returns>
        Task<List<RequestDto>> GetAllRequestsAsync();

        /// <summary>
        /// Add request.
        /// </summary>
        /// <param name="requestDto">Dto model</param>
        Task<int> AddRequestAsync(RequestDto requestDto);

        /// <summary>
        /// Get request by id.
        /// </summary>
        /// <param name="requestId">Request id</param>
        /// <returns>Request dto model</returns>
        Task<RequestDto> GetRequestByIdAsync(int requestId);

        /// <summary>
        /// Get request by date.
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>List requestdto</returns>
        Task<List<RequestDto>> GetRequestByDateAsync(DateTime date);

        /// <summary>
        /// Get requests betwen two dates.
        /// </summary>
        /// <param name="begin">Start date.</param>
        /// <param name="end">Finish date.</param>
        /// <returns>List requestdto</returns>
        Task<List<RequestDto>> GetRequestByDateBetweenAsync(DateTime begin, DateTime end);
    }
}
