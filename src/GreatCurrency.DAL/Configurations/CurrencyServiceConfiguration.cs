using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class CurrencyServiceConfiguration : IEntityTypeConfiguration<CurrencyService>
	{
		public void Configure(EntityTypeBuilder<CurrencyService> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.CurrencyServices)
				.HasKey(currencyservice => currencyservice.Id);

			builder.Property(currencyservice => currencyservice.ServiceName)
			   .IsRequired();
		}
	}
}
