using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ILEOrganisationService
	{
		/// <summary>
		/// Delete organisation.
		/// </summary>
		/// <param name="organisationDto">Organisation dto model</param>
		Task<bool> DeleteOrganisationAsync(LEOrganisationDto organisationDto);

		/// <summary>
		/// Get all organisations.
		/// </summary>
		/// <returns>list organisations</returns>
		Task<List<LEOrganisationDto>> GetAllOrganisationsAsync();

		/// <summary>
		/// Add organisation.
		/// </summary>
		/// <param name="organisationDto">Dto model</param>
		Task<int> AddOrganisationAsync(LEOrganisationDto organisationDto);

		/// <summary>
		/// Update organisation.
		/// </summary>
		/// <param name="organisationDto">Organisation dto model</param>
		/// <returns>organisation dto</returns>
		Task UpdateOrganisationAsync(LEOrganisationDto organisationDto);

		/// <summary>
		/// Get organisation by id.
		/// </summary>
		/// <param name="organisationid">Organisation id</param>
		/// <returns>organisation dto</returns>
		Task<LEOrganisationDto> GetOrganisationByIdAsync(int organisationid);

		/// <summary>
		/// Get organisation by Name.
		/// </summary>
		/// <param name="organisationName">Organisation name</param>
		/// <returns>organisation dto</returns>
		Task<LEOrganisationDto> GetOrganisationByNameAsync(string organisationName);
	}
}
