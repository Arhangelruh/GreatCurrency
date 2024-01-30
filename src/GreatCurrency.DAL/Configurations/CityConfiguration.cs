using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
    internal class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.City)
                .HasKey(city => city.Id);

            builder.Property(city => city.CityName)
                .IsRequired();

            builder.Property(city => city.CityURL);                
        }
    }
}
