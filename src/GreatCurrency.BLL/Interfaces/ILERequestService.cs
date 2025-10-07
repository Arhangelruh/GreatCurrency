using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ILERequestService
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
		Task<List<LERequestDto>> GetAllRequestsAsync();

		/// <summary>
		/// Add request.
		/// </summary>
		/// <param name="requestDto">Dto model</param>
		Task<int> AddRequestAsync(LERequestDto requestDto);

		/// <summary>
		/// Get request by id.
		/// </summary>
		/// <param name="requestId">Request id</param>
		/// <returns>Request dto model</returns>
		Task<LERequestDto> GetRequestByIdAsync(int requestId);

		/// <summary>
		/// Get request by date.
		/// </summary>
		/// <param name="date">Date</param>
		/// <returns>List requestdto</returns>
		Task<List<LERequestDto>> GetRequestByDateAsync(DateTime date);

		/// <summary>
		/// Get requests betwen two dates.
		/// </summary>
		/// <param name="begin">Start date.</param>
		/// <param name="end">Finish date.</param>
		/// <returns>List requestdto</returns>
		Task<List<LERequestDto>> GetRequestByDateBetweenAsync(DateTime begin, DateTime end);
	}
}
