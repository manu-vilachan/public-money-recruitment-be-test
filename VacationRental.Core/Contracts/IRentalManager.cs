using VacationRental.Core.Domain;

namespace VacationRental.Core.Contracts;

public interface IRentalManager
{
    Task<Rental> CreateAsync(int units, int preparationTime, CancellationToken cancellationToken = default);

    Task<Rental?> GetAsync(int rentalId, CancellationToken cancellationToken = default);

    Task<IList<CalendarDay>> GetCalendarDaysAsync(int rentalId, DateTime startDate, int nights, CancellationToken cancellationToken = default);
}