using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="ISCRequestService"/>
	public class SCRequestService(IRepository<SCRequest> requestRepository, IRepository<CSCurrency> currencyRepository) : ISCRequestService
	{

		private readonly IRepository<SCRequest> _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
		private readonly IRepository<CSCurrency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));

		public async Task<int> AddRequestAsync(SCRequestDto sCrequestDto)
		{
			ArgumentNullException.ThrowIfNull(sCrequestDto);

			var newRequest = new SCRequest
			{
				IncomingDate = sCrequestDto.IncomingDate
			};

			await _requestRepository.AddAsync(newRequest);
			await _requestRepository.SaveChangesAsync();

			return newRequest.Id;
		}

		public async Task<bool> DeleteRequestAsync(int requestId)
		{
			var checkCurrency = await _currencyRepository.GetEntityAsync(currency => currency.RequestId == requestId);
			if (checkCurrency is null)
			{				
					var request = await _requestRepository.GetEntityAsync(request => request.Id == requestId);
					_requestRepository.Delete(request);
					await _requestRepository.SaveChangesAsync();
					return true;				
			}
			return false;
		}

		public async Task<List<SCRequestDto>> GetAllRequestsAsync()
		{
			List<SCRequestDto> requests = [];

			var getRequests = await _requestRepository.GetAll().AsNoTracking().ToListAsync();
			foreach (var request in getRequests)
			{
				requests.Add(new SCRequestDto
				{
					Id = request.Id,
					IncomingDate = request.IncomingDate
				});
			}
			return requests;
		}

		public async Task<List<SCRequestDto>> GetRequestByDateAsync(DateTime date)
		{
			List<SCRequestDto> requests = [];

			var getrequest = await _requestRepository
				.GetAll()
				.AsNoTracking()
				.Where(request => request.IncomingDate < date)
				.ToListAsync();

			if (getrequest.Any())
			{
				foreach (var request in getrequest)
				{
					requests.Add(new SCRequestDto
					{
						Id = request.Id,
						IncomingDate = request.IncomingDate
					});
				}

				return requests;
			}
			return requests;
		}

		public async Task<List<SCRequestDto>> GetRequestByDateBetweenAsync(DateTime begin, DateTime end)
		{
			List<SCRequestDto> requests = [];

			var getrequests = await _requestRepository
				.GetAll()
				.AsNoTracking()
				.Where(request => request.IncomingDate > begin && request.IncomingDate < end)
				.OrderBy(request => request.Id)
				.ToListAsync();

			if (getrequests.Any())
			{
				foreach (var request in getrequests)
				{
					requests.Add(new SCRequestDto
					{
						Id = request.Id,
						IncomingDate = request.IncomingDate
					});
				}

				return requests;
			}
			return requests;
		}

		public async Task<SCRequestDto?> GetRequestByIdAsync(int requestId)
		{
			var getrequest = await _requestRepository.GetEntityAsync(request => request.Id == requestId);
			if (getrequest is null)
			{
				return null;
			}
			var requestDto = new SCRequestDto
			{
				Id = getrequest.Id,
				IncomingDate = getrequest.IncomingDate
			};
			return requestDto;
		}
	}
}
