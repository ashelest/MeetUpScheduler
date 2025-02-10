namespace Domain;

public class SalesManager
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;
	public List<string> Languages { get; set; } = new();
	public List<string> Products { get; set; } = new();
	public List<string> CustomerRatings { get; set; } = new();

	public List<Slot> Slots { get; set; } = new();
}