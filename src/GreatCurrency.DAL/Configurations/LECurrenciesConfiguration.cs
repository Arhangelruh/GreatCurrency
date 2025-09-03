using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class LECurrenciesConfiguration : IEntityTypeConfiguration<LECurrency>
	{
		public void Configure(EntityTypeBuilder<LECurrency> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.LECurrencies)
				.HasKey(currency => currency.Id);

			builder.HasOne(request => request.LERequest)
			   .WithMany(currency => currency.LECurrencies)
			   .HasForeignKey(request => request.RequestId)
			   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(organisation => organisation.Organisation)
				.WithMany(currency => currency.LECurrencies)
				.HasForeignKey(organisation => organisation.Id)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
