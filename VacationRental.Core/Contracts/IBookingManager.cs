using VacationRental.Core.Domain;

namespace VacationRental.Core.Contracts;

public interface IBookingManager
{
    Task<Booking> CreateAsync(int rentalId, DateTime startDate, int nights, CancellationToken cancellationToken = default);

    Task<Booking?> GetAsync(int bookingId, CancellationToken cancellationToken = default);
}