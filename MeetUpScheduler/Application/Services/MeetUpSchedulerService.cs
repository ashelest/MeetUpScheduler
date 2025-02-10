using Application.Contracts;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MeetUpSchedulerService(IApplicationDbContext dbContext) : IMeetUpSchedulerService
{
	public async Task<IEnumerable<AvailableSlotDto>> GetAvailableTimeSlotsAsync(AvailableTimeSlotsFilter filter)
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
			.Select(manager => new
			{
				Manager = manager,

				Slots = manager.Slots
					.Where(s => s.StartDate.Date == filter.Date.Date &&
					            s.EndDate.Date == filter.Date.Date)
					.OrderBy(s => s.StartDate)
					.ToList()
			})
			.ToListAsync();

		var result = new Dictionary<DateTimeOffset, int>();

		foreach (var manager in managers)
		{
			DateTimeOffset? lastBookedEndTime = null;

			var managerSlots = manager.Slots;

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
					if (!result.TryAdd(slot.StartDate, 1))
					{
						result[slot.StartDate]++;
					}
				}
			}
		}

		return result.Select(t => new AvailableSlotDto
		{
			StartDate = t.Key,
			AvailableCount = t.Value
		});
	}

	private static bool IsOverlappingWithNextBooked(int currentSlotIndex, List<Slot> managerSlots, Slot slot)
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