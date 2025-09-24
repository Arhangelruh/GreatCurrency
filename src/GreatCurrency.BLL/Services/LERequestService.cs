using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	public class LERequestService : ILERequestService
	{
		private readonly IRepository<LERequest> _requestRepository;
		private readonly IRepository<LECurrency> _currencyRepository;

		public LERequestService(IRepository<LERequest> requestRepository, IRepository<LECurrency> currencyRepository)
		{
			_requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
			_currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
		}

		public async Task<int> AddRequestAsync(LERequestDto requestDto)
		{
			if (requestDto is null)
			{
				throw new ArgumentNullException(nameof(requestDto));
			}

			var newRequest = new LERequest
			{
				IncomingDate = requestDto.IncomingDate
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

		public async Task<List<LERequestDto>> GetAllRequestsAsync()
		{
			List<LERequestDto> requests = [];

			var getRequests = await _requestRepository.GetAll().AsNoTracking().ToListAsync();
			foreach (var request in getRequests)
			{
				requests.Add(new LERequestDto
				{
					Id = request.Id,
					IncomingDate = request.IncomingDate
				});
			}
			return requests;
		}

		public async Task<List<LERequestDto>> GetRequestByDateAsync(DateTime date)
		{
			List<LERequestDto> requests = [];

			var getrequest = await _requestRepository
				.GetAll()
				.AsNoTracking()
				.Where(request => request.IncomingDate < date)
				.ToListAsync();

			if (getrequest.Any())
			{
				foreach (var request in getrequest)
				{
					requests.Add(new LERequestDto
					{
						Id = request.Id,
						IncomingDate = request.IncomingDate
					});
				}

				return requests;
			}
			return requests;
		}

		public async Task<List<LERequestDto>> GetRequestByDateBetweenAsync(DateTime begin, DateTime end)
		{
			List<LERequestDto> requests = [];

			var getrequests = await _requestRepository
				.GetAll()
				.AsNoTracking()
				.Where(request => request.IncomingDate > begin && request.IncomingDate < end)
				.ToListAsync();

			if (getrequests.Any())
			{
				foreach (var request in getrequests)
				{
					requests.Add(new LERequestDto
					{
						Id = request.Id,
						IncomingDate = request.IncomingDate
					});
				}

				return requests;
			}
			return requests;
		}

		public async Task<LERequestDto> GetRequestByIdAsync(int requestId)
		{
			var getrequest = await _requestRepository.GetEntityAsync(request => request.Id == requestId);
			if (getrequest is null)
			{
				return null;
			}
			var requestDto = new LERequestDto
			{
				Id = getrequest.Id,
				IncomingDate = getrequest.IncomingDate
			};
			return requestDto;
		}
	}
}
