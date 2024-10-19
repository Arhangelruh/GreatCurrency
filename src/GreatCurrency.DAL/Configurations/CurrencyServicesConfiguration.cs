using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class CurrencyServicesConfiguration : IEntityTypeConfiguration<CurrencyServices>
	{
		public void Configure(EntityTypeBuilder<CurrencyServices> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.CurrencyServices)
				.HasKey(currencyservice => currencyservice.Id);

			builder.Property(currencyservice => currencyservice.ServiceName)
			   .IsRequired();
		}
	}
}
