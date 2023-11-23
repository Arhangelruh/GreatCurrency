using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Currency)
                .HasKey(currency => currency.Id);

            builder.Property(currency => currency.IncomingDate)
                .HasColumnType("Timestamp");

            builder.Property(currency => currency.USDBuyRate)
                .IsRequired();

            builder.Property(currency => currency.USDSaleRate)
                .IsRequired();

            builder.Property(currency => currency.EURBuyRate)
                .IsRequired();

            builder.Property(currency => currency.EURSaleRate)
                .IsRequired();

            builder.Property(currency => currency.RUBBuyRate)
                .IsRequired();

            builder.Property(currency => currency.RUBSaleRate)
                .IsRequired();

            builder.HasOne(bankdepartment=>bankdepartment.BankDepartment)
                .WithMany(currency=>currency.Currencies)
                .HasForeignKey(bankdepartment=>bankdepartment.BankDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
