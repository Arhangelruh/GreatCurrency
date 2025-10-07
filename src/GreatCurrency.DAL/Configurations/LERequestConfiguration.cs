using GreatCurrency.DAL.Constants;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreatCurrency.DAL.Configurations
{
	internal class LERequestConfiguration : IEntityTypeConfiguration<LERequest>
	{
		public void Configure(EntityTypeBuilder<LERequest> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.LERequests)
				.HasKey(request => request.Id);

			builder.Property(request => request.IncomingDate)
				.HasColumnType("Timestamp");
		}
	}
}
