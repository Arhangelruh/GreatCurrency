using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
    public class BestCurrencyConfiguration : IEntityTypeConfiguration<BestCurrency>
    {
        public void Configure(EntityTypeBuilder<BestCurrency> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.BestCurrency)
                .HasKey(bestcurrency => bestcurrency.Id);

            builder.Property(bestcurrency => bestcurrency.USDBuyRate)
                .IsRequired();

            builder.Property(bestcurrency => bestcurrency.USDSaleRate)
                .IsRequired();

            builder.Property(bestcurrency => bestcurrency.EURBuyRate)
                .IsRequired();

            builder.Property(bestcurrency => bestcurrency.EURSaleRate)
                .IsRequired();

            builder.Property(bestcurrency => bestcurrency.RUBBuyRate)
                .IsRequired();

            builder.Property(bestcurrency => bestcurrency.RUBSaleRate)
                .IsRequired();

            builder.HasOne(request => request.Request)
                .WithMany(bestcurrency => bestcurrency.BestCurrencies)
                .HasForeignKey(request => request.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bank => bank.Bank)
                .WithMany(bestcurrency => bestcurrency.BestCurrencies)
                .HasForeignKey(bank => bank.BankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
