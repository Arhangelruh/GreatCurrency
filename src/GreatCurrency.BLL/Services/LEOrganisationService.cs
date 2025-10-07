using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	public class LEOrganisationService(IRepository<LEOrganisation> repository, IRepository<LECurrency> currencyRepository) : ILEOrganisationService
	{
		private readonly IRepository<LEOrganisation> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
		private readonly IRepository<LECurrency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));

		public async Task<int> AddOrganisationAsync(LEOrganisationDto organisationDto)
		{
			ArgumentNullException.ThrowIfNull(organisationDto, nameof(organisationDto));

			var newOrganisation = new LEOrganisation
			{
				Name = organisationDto.Name
			};
			await _repository.AddAsync(newOrganisation);
			await _repository.SaveChangesAsync();

			return newOrganisation.Id;
		}

		public async Task<bool> DeleteOrganisationAsync(LEOrganisationDto organisationDto)
		{
			ArgumentNullException.ThrowIfNull(organisationDto, nameof(organisationDto));

			var currency = await _currencyRepository.GetEntityAsync(currency => currency.OrganisationId == organisationDto.Id);
			if (currency is null)
			{
				var organisation = await _repository.GetEntityAsync(organisation => organisation.Id == organisationDto.Id);
				_repository.Delete(organisation);
				await _repository.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<List<LEOrganisationDto>> GetAllOrganisationsAsync()
		{
			List<LEOrganisationDto> organisations = [];
			var getOrganisations = await _repository.GetAll().AsNoTracking().ToListAsync();
			foreach (var organisation in getOrganisations)
			{
				organisations.Add(new LEOrganisationDto
				{
					Id = organisation.Id,
					Name = organisation.Name
				});
			}
			return organisations;
		}

		public async Task<LEOrganisationDto> GetOrganisationByIdAsync(int organisationid)
		{
			var getOrganisation = await _repository.GetEntityAsync(organisation => organisation.Id == organisationid);
			if (getOrganisation is null)
			{
				return null;
			}

			var organisationDto = new LEOrganisationDto
			{
				Id = getOrganisation.Id,
				Name = getOrganisation.Name
			};
			return organisationDto;
		}

		public async Task<LEOrganisationDto> GetOrganisationByNameAsync(string organisationName)
		{
			var getOrganisation = await _repository.GetEntityAsync(organisation => organisation.Name == organisationName);

			if (getOrganisation is null)
			{
				return null;
			}
			var organisationDto = new LEOrganisationDto
			{
				Id = getOrganisation.Id,
				Name = getOrganisation.Name
			};
			return organisationDto;
		}

		public async Task UpdateOrganisationAsync(LEOrganisationDto organisationDto)
		{
			ArgumentNullException.ThrowIfNull(organisationDto, nameof(organisationDto));

			var getOrganisation = await _repository.GetEntityAsync(organisation => organisation.Id == organisationDto.Id);
			getOrganisation.Name = organisationDto.Name;

			_repository.Update(getOrganisation);
			await _repository.SaveChangesAsync();
		}

		public async Task CheckOrDeleteAsync()
		{
			var getOrganisations = await _repository.GetAll().AsNoTracking().ToListAsync();
			foreach (var organisation in getOrganisations)
			{
				var currency = await _currencyRepository.GetEntityAsync(currency => currency.OrganisationId == organisation.Id);
				if (currency is null)
				{
					_repository.Delete(organisation);
					await _repository.SaveChangesAsync();
				}
			}
		}
	}
}
