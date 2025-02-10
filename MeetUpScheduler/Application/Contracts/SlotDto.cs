namespace Application.Contracts;

public class SlotDto
{
	public bool IsBooked { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
}