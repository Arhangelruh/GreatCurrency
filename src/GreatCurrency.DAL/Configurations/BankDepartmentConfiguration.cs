using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
    internal class BankDepartmentConfiguration : IEntityTypeConfiguration<BankDepartment>
    {
        public void Configure(EntityTypeBuilder<BankDepartment> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.BankDepartment)
                .HasKey(bankdepartnent => bankdepartnent.Id);

            builder.Property(bankdepartnent => bankdepartnent.DepartmentAddress)
                .IsRequired();

            builder.HasOne(bank => bank.Bank)
                .WithMany(bankdepartment => bankdepartment.BankDepartments)
                .HasForeignKey(bank => bank.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bank => bank.City)
                .WithMany(bankdepartment => bankdepartment.BankDepartments)
                .HasForeignKey(bank => bank.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
