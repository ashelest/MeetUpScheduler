using Application.Contracts;

namespace Application.Interfaces;

public interface IMeetUpSchedulerService
{
	Task<IEnumerable<AvailableSlotDto>> GetAvailableTimeSlotsAsync(AvailableTimeSlotsFilter filter);
}