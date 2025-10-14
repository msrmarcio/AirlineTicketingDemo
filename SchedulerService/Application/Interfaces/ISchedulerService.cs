namespace SchedulerService.Application.Interfaces
{
    public interface ISchedulerService
    {
        Task SchedulePaymentTimeoutAsync(Guid reservationId, string customerName, string customerEmail);
    }

}
