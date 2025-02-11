using WebAPI.Contracts;

namespace WebAPI.Interfaces;

public interface IMeetUpSchedulerService
{
	Task<IEnumerable<AvailableSlotDto>> GetAvailableTimeSlotsAsync(AvailableTimeSlotsFilter filter);
}