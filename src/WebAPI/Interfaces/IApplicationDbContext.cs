using Microsoft.EntityFrameworkCore;
using WebAPI.Domain;

namespace WebAPI.Interfaces;

public interface IApplicationDbContext
{
	public DbSet<SalesManager> SalesManagers { get; set; }
	public DbSet<Slot> Slots { get; set; }
}