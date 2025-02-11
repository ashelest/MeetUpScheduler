namespace WebAPI.Contracts;

public class AvailableTimeSlotsFilter
{
	public DateTimeOffset Date { get; set; }
	public IEnumerable<string> Products { get; set; } = [];
	public ManagerLanguage Language { get; set; }
	public ManagerRating Rating { get; set; }
}