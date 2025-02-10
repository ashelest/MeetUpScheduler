using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
	public DbSet<SalesManager> SalesManagers { get; set; }
	public DbSet<Slot> Slots { get; set; }
}