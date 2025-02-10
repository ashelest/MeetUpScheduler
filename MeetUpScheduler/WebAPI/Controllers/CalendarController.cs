using Application.Contracts;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController(IMeetUpSchedulerService schedulerService) : ControllerBase
{
	[HttpPost("query")]
	public async Task<IEnumerable<AvailableSlotDto>> GetAvailableTimeSlots(AvailableTimeSlotsFilter filter)
	{
		var availableSlots = await schedulerService.GetAvailableTimeSlotsAsync(filter);

		return availableSlots;
	}
}