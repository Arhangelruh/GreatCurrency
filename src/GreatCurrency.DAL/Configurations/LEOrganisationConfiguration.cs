using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	internal class LEOrganisationConfiguration : IEntityTypeConfiguration<LEOrganisation>
	{
		public void Configure(EntityTypeBuilder<LEOrganisation> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.LEOrganisations)
				.HasKey(organisation => organisation.Id);

			builder.Property(organisation => organisation.Name)
				.IsRequired();
		}
	}
}
