using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.DAL.Configurations
{
	internal class SCRequestConfiguration : IEntityTypeConfiguration<SCRequest>
	{
		public void Configure(EntityTypeBuilder<SCRequest> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.SCRequests)
				.HasKey(request => request.Id);

			builder.Property(request => request.IncomingDate)
				.HasColumnType("Timestamp");
		}
	}
}
