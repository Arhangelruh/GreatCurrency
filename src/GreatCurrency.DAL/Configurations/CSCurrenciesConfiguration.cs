using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class CSCurrenciesConfiguration : IEntityTypeConfiguration<CSCurrency>
	{
		public void Configure(EntityTypeBuilder<CSCurrency> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.CSCurrencies)
				.HasKey(currency => currency.Id);

			builder.HasOne(request => request.SCRequest)
			   .WithMany(currency => currency.CSCurrencies)
			   .HasForeignKey(request => request.RequestId)
			   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(currencyservice => currencyservice.CurrencyService)
				.WithMany(currency => currency.CSCurrencies)
				.HasForeignKey(currencyservice => currencyservice.CurrencyServicesId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
