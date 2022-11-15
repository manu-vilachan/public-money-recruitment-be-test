using VacationRental.Core.Contracts;
using VacationRental.Core.Domain;
using VacationRental.Data;

namespace VacationRental.Business;

public class BookingManager : IBookingManager
{
    private readonly VacationRentalDbContext db;

    public BookingManager(VacationRentalDbContext dbContext)
    {
        db = dbContext;
    }

    public async Task<Booking> CreateAsync(int rentalId, DateTime startDate, int nights, CancellationToken cancellationToken = default)
    {
        var rental = await db.Rentals.FindAsync(rentalId);
        if (rental == null)
            throw new ApplicationException("Rental not found");

        Unit? freeUnitForBooking = null;
        foreach (var unit in rental.Units)
        {
            bool isFree = true;
            foreach (var booking in (unit.Bookings ?? new List<Booking>()))
            {
                var bookingTotalDays = booking.Nights + rental.PreparationTime;
                if ((booking.StartDate <= startDate.Date && booking.StartDate.AddDays(bookingTotalDays) > startDate.Date)
                    || (booking.StartDate < startDate.AddDays(nights) && booking.StartDate.AddDays(bookingTotalDays) >= startDate.AddDays(nights))
                    || (booking.StartDate > startDate && booking.StartDate.AddDays(bookingTotalDays) < startDate.AddDays(nights)))
                {
                    isFree = false;
                    break;
                }
            }

            if (isFree)
            {
                freeUnitForBooking = unit;
                break;
            }
        }

        if (freeUnitForBooking == null)
            throw new ApplicationException("Not available");

        var newBooking = new Booking
        {
            Rental = rental,
            Nights = nights,
            StartDate = startDate,
            Unit = freeUnitForBooking
        };

        await db.Bookings.AddAsync(newBooking, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return newBooking;
    }

    public Task<Booking?> GetAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        return db.Bookings.FindAsync(new object[] { bookingId }, cancellationToken:cancellationToken).AsTask();
    }
}