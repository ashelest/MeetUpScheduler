namespace WebAPI.Domain;

public class Slot
{
	public int Id { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
	public bool IsBooked { get; set; }
	public int SalesManagerId { get; set; }
	public SalesManager SalesManager { get; set; } = null!;
}