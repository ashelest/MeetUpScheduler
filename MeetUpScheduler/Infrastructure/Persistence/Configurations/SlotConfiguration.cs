using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
	private const string DateTimeColumnType = "timestamp with time zone";
	
	public void Configure(EntityTypeBuilder<Slot> builder)
	{
		builder.ToTable("slots");
		
		builder.HasKey(s => s.Id);
		builder.Property(s => s.Id)
			.HasColumnName("id");

		builder.Property(s => s.StartDate)
			.HasColumnName("start_date")
			.HasColumnType(DateTimeColumnType)
			.IsRequired();
        
		builder.Property(s => s.EndDate)
			.HasColumnName("end_date")
			.HasColumnType(DateTimeColumnType)
			.IsRequired();
        
		builder.Property(s => s.IsBooked)
			.HasColumnName("booked")
			.HasDefaultValue(false)
			.IsRequired();
		
		builder.Property(s => s.SalesManagerId)
			.HasColumnName("sales_manager_id")
			.IsRequired();
        
		builder.HasOne(s => s.SalesManager)
			.WithMany(sm => sm.Slots)
			.HasForeignKey(s => s.SalesManagerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}