using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	public class DealStockRatesConfiguration : IEntityTypeConfiguration<DealStockRates>
	{
		public void Configure(EntityTypeBuilder<DealStockRates> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.DealStockRates)
				.HasKey(rate => rate.Id);

			builder.HasOne(deal => deal.LERequest)
			.WithMany(request => request.dealStockRates)
			.HasForeignKey(deal => deal.RequestId)
			.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(deal => deal.Currency)
			.WithMany(с => с.dealStockRates)
			.HasForeignKey(deal => deal.CurrencyId)
			.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
