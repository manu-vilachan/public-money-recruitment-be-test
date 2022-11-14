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

        for (var i = 0; i < nights; i++)
        {
            var count = 0;
            foreach (var booking in rental.Bookings)
            {
                if ((booking.StartDate <= startDate.Date && booking.StartDate.AddDays(booking.Nights) > startDate.Date)
                    || (booking.StartDate < startDate.AddDays(nights) && booking.StartDate.AddDays(booking.Nights) >= startDate.AddDays(nights))
                    || (booking.StartDate > startDate && booking.StartDate.AddDays(booking.Nights) < startDate.AddDays(nights)))
                {
                    count++;
                }
            }

            if (count >= rental.Units)
                throw new ApplicationException("Not available");
        }

        var newBooking = new Booking
        {
            Rental = rental,
            Nights = nights,
            StartDate = startDate
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