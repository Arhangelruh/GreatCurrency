using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ISCRequestService
	{
		/// <summary>
		/// Delete SCrequest.
		/// </summary>
		/// <param name="requestId">request id</param>
		Task<bool> DeleteRequestAsync(int requestId);

		/// <summary>
		/// Get all requests.
		/// </summary>
		/// <returns>list requests</returns>
		Task<List<SCRequestDto>> GetAllRequestsAsync();

		/// <summary>
		/// Add request.
		/// </summary>
		/// <param name="sCrequestDto">Dto model</param>
		Task<int> AddRequestAsync(SCRequestDto sCrequestDto);

		/// <summary>
		/// Get request by id.
		/// </summary>
		/// <param name="requestId">Request id</param>
		/// <returns>SCRequestDto dto model</returns>
		Task<SCRequestDto?> GetRequestByIdAsync(int requestId);

		/// <summary>
		/// Get request by date.
		/// </summary>
		/// <param name="date">Date</param>
		/// <returns>List SCrequestdto</returns>
		Task<List<SCRequestDto>> GetRequestByDateAsync(DateTime date);

		/// <summary>
		/// Get requests betwen two dates.
		/// </summary>
		/// <param name="begin">Start date.</param>
		/// <param name="end">Finish date.</param>
		/// <returns>List requestdto</returns>
		Task<List<SCRequestDto>> GetRequestByDateBetweenAsync(DateTime begin, DateTime end);
	}
}
