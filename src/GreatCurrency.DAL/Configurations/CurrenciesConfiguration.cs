using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class CurrenciesConfiguration : IEntityTypeConfiguration<Currencies>
	{
		public void Configure(EntityTypeBuilder<Currencies> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.Currencies)
				.HasKey(currency => currency.Id);
		}
	}
}
