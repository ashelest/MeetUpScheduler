using Application.Contracts;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MeetUpSchedulerService(IApplicationDbContext dbContext, ICachingService cachingService) : IMeetUpSchedulerService
{
	public async Task<IEnumerable<AvailableSlotDto>> GetAvailableTimeSlotsAsync(AvailableTimeSlotsFilter filter)
	{
		var cachedResults = await cachingService.GetAsync<AvailableTimeSlotsFilter, IList<AvailableSlotDto>>(filter);

		if (cachedResults is not null && cachedResults.Count > 0)
		{
			return cachedResults;
		}
		
		var managers = await GetManagerSlotsAsync(filter);

		var filteredResult = GetFilteredTimeSlots(managers);

		if (filteredResult.Count > 0)
		{
			await cachingService.SetAsync(filter, filteredResult);
		}

		return filteredResult;
	}

	private static IList<AvailableSlotDto> GetFilteredTimeSlots(List<ManagerSlotsDto> managers)
	{
		var resultsDictionary = new Dictionary<DateTimeOffset, int>();

		foreach (var manager in managers)
		{
			DateTimeOffset? lastBookedEndTime = null;

			var managerSlots = manager.Slots.ToList();

			for (int slotIndex = 0; slotIndex < managerSlots.Count; slotIndex++)
			{
				var slot = managerSlots[slotIndex];

				if (slot.IsBooked)
				{
					lastBookedEndTime = slot.EndDate;
					continue;
				}

				// Check, that we don't have overlapping with a previously booked slot
				if (lastBookedEndTime is not null && slot.StartDate < lastBookedEndTime)
				{
					continue;
				}

				// Check, that we don't have overlapping on the next booked slot
				bool overlapsWithNextBooked = IsOverlappingWithNextBooked(slotIndex, managerSlots, slot);

				if (!overlapsWithNextBooked)
				{
					if (!resultsDictionary.TryAdd(slot.StartDate, 1))
					{
						resultsDictionary[slot.StartDate]++;
					}
				}
			}
		}

		return resultsDictionary.Select(t => new AvailableSlotDto
		{
			StartDate = t.Key,
			AvailableCount = t.Value
		}).ToList();
	}

	private async Task<List<ManagerSlotsDto>> GetManagerSlotsAsync(AvailableTimeSlotsFilter filter)
	{
		var managersQuery = dbContext.SalesManagers
			.Where(manager => manager.CustomerRatings.Contains(filter.Rating.ToString()))
			.Where(manager => manager.Languages.Contains(filter.Language.ToString()));

		if (filter.Products.Any())
		{
			managersQuery =
				managersQuery.Where(manager => filter.Products.All(requiredProduct => manager.Products.Contains(requiredProduct)));
		}

		var managers = await managersQuery
			.Select(manager =>
				new ManagerSlotsDto
				{
					Slots = manager.Slots
						.Where(s => s.StartDate.Date == filter.Date.Date &&
						            s.EndDate.Date == filter.Date.Date)
						.OrderBy(s => s.StartDate)
						.Select(slot => new SlotDto { StartDate = slot.StartDate, EndDate = slot.EndDate, IsBooked = slot.IsBooked })
				})
			.ToListAsync();
		return managers;
	}

	private static bool IsOverlappingWithNextBooked(int currentSlotIndex, List<SlotDto> managerSlots, SlotDto slot)
	{
		bool overlapsWithNextBooked = false;

		for (int j = currentSlotIndex + 1; j < managerSlots.Count; j++)
		{
			if (managerSlots[j].IsBooked && managerSlots[j].StartDate < slot.EndDate)
			{
				overlapsWithNextBooked = true;

				break;
			}
		}

		return overlapsWithNextBooked;
	}
}