using WebAPI.Contracts;
using WebAPI.Interfaces;

namespace WebAPI.Services;

public class CachedMeetUpSchedulerService(
	IMeetUpSchedulerService schedulerService,
	ICachingService cachingService) : IMeetUpSchedulerService
{
	public async Task<IEnumerable<AvailableSlotDto>> GetAvailableTimeSlotsAsync(AvailableTimeSlotsFilter filter)
	{
		var cachedData = await cachingService.GetAsync<AvailableTimeSlotsFilter, IList<AvailableSlotDto>>(filter);

		if (cachedData is not null && cachedData.Count > 0)
		{
			return cachedData;
		}

		var availableTimeSlots = await schedulerService.GetAvailableTimeSlotsAsync(filter);

		var availableSlotDtos = availableTimeSlots.ToList();

		if (availableSlotDtos.Count > 0)
		{
			await cachingService.SetAsync(filter, availableSlotDtos);
		}

		return availableTimeSlots;
	}
}