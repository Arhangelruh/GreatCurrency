using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class CSCurrenciesConfiguration : IEntityTypeConfiguration<CSCurrencies>
	{
		public void Configure(EntityTypeBuilder<CSCurrencies> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.CSCurrencies)
				.HasKey(currency => currency.Id);

			builder.HasOne(request => request.Request)
			   .WithMany(currency => currency.CSCurrencies)
			   .HasForeignKey(request => request.RequestId)
			   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(currencyservice => currencyservice.CurrencyServices)
				.WithMany(currency => currency.CSCurrencies)
				.HasForeignKey(currencyservice => currencyservice.CurrencyServicesId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
