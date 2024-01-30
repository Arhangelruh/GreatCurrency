using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
    /// <inheritdoc cref="IRequestService"/>
    public class RequestService : IRequestService
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<BestCurrency> _bestCurrencyRepository;

        public RequestService(IRepository<Request> requestRepository, IRepository<Currency> currencyRepository, IRepository<BestCurrency> bestCurrencyRepository)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _bestCurrencyRepository = bestCurrencyRepository ?? throw new ArgumentNullException(nameof(bestCurrencyRepository));
        }

        public async Task<int> AddRequestAsync(RequestDto requestDto)
        {
            if (requestDto is null)
            {
                throw new ArgumentNullException(nameof(requestDto));
            }

            var newRequest = new Request
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
                var checkBestCurrency = await _bestCurrencyRepository.GetEntityAsync(bestcurrency => bestcurrency.RequestId == requestId);
                if (checkBestCurrency is null) {
                    var request = await _requestRepository.GetEntityAsync(request => request.Id == requestId);
                    _requestRepository.Delete(request);
                    await _requestRepository.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<RequestDto>> GetAllRequestsAsync()
        {
            List<RequestDto> requests = [];

            var getRequests = await _requestRepository.GetAll().AsNoTracking().ToListAsync();
            foreach (var request in getRequests)
            {
                requests.Add(new RequestDto
                {
                    Id = request.Id,
                    IncomingDate = request.IncomingDate
                });
            }
            return requests;
        }

        public async Task<RequestDto> GetRequestByIdAsync(int requestId)
        {
            var getrequest = await _requestRepository.GetEntityAsync(request => request.Id == requestId);
            if (getrequest is null)
            {
                return null;
            }
            var requestDto = new RequestDto
            {
                Id = getrequest.Id,
                IncomingDate = getrequest.IncomingDate
            };
            return requestDto;
        }

        public async Task<List<RequestDto>> GetRequestByDateAsync(DateTime date)
        {
            List<RequestDto> requests = [];

            var getrequest = await _requestRepository
                .GetAll()
                .AsNoTracking()
                .Where(request => request.IncomingDate < date)
                .ToListAsync();

            if (getrequest.Any())
            {
                foreach (var request in getrequest)
                {
                    requests.Add(new RequestDto {
                        Id = request.Id,
                        IncomingDate = request.IncomingDate
                    });
                }
                
                return requests;
            }
            return requests;
        }

        public async Task<List<RequestDto>> GetRequestByDateBetweenAsync(DateTime begin, DateTime end)
        {
            List<RequestDto> requests = [];

            var getrequests = await _requestRepository
                .GetAll()
                .AsNoTracking()
                .Where(request => request.IncomingDate > begin && request.IncomingDate < end)
                .ToListAsync();

            if (getrequests.Any())
            {
                foreach (var request in getrequests)
                {
                    requests.Add(new RequestDto
                    {
                        Id = request.Id,
                        IncomingDate = request.IncomingDate
                    });
                }

                return requests;
            }
            return requests;
        }
    }
}
