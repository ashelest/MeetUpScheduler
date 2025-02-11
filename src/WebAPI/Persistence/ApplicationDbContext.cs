using Microsoft.EntityFrameworkCore;
using WebAPI.Domain;
using WebAPI.Interfaces;
using WebAPI.Persistence.Configurations;

namespace WebAPI.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new SlotConfiguration());
		builder.ApplyConfiguration(new SalesManagerConfiguration());

		base.OnModelCreating(builder);
	}

	public DbSet<SalesManager> SalesManagers { get; set; }
	public DbSet<Slot> Slots { get; set; }
}