using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Domain;

namespace WebAPI.Persistence.Configurations;

public class SalesManagerConfiguration : IEntityTypeConfiguration<SalesManager>
{
	private const string ArrayColumnType = "varchar(100)[]";

	public void Configure(EntityTypeBuilder<SalesManager> builder)
	{
		builder.ToTable("sales_managers");

		builder.Property(s => s.Id)
			.HasColumnName("id");

		builder.Property(sm => sm.Name)
			.HasColumnName("name")
			.IsRequired();

		builder.Property(s => s.Languages)
			.HasColumnName("languages")
			.HasColumnType(ArrayColumnType);

		builder.Property(s => s.Products)
			.HasColumnName("products")
			.HasColumnType(ArrayColumnType);

		builder.Property(s => s.CustomerRatings)
			.HasColumnName("customer_ratings")
			.HasColumnType(ArrayColumnType);
	}
}