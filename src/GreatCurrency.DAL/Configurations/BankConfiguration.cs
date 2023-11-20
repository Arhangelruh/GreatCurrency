using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
    internal class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Bank)
                .HasKey(bank => bank.Id);

            builder.Property(bank => bank.BankName)
                .IsRequired();
        }
    }
}
