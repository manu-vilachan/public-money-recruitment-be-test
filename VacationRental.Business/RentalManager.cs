﻿using VacationRental.Core.Contracts;
using VacationRental.Core.Domain;
using VacationRental.Data;

namespace VacationRental.Business;

public class RentalManager : IRentalManager
{
    private readonly VacationRentalDbContext db;

    public RentalManager(VacationRentalDbContext dbContext)
    {
        db = dbContext;
    }

    public async Task<Rental> CreateAsync(int units, int preparationTime, CancellationToken cancellationToken = default)
    {
        if (units <= 0)
            throw new ApplicationException("Units should be greater than 0");
        
        if (preparationTime < 0)
            throw new ApplicationException("Preparation time cannot be a negative number");

        var postEntity = new Rental { Units = new List<Unit>(), PreparationTime = preparationTime };
        for (int i = 1; i <= units; i++)
        {
            postEntity.Units.Add(new Unit { UnitNumber = i });
        }

        var entry = await db.Rentals.AddAsync(postEntity, cancellationToken).AsTask();
        await db.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public Task<Rental?> GetAsync(int rentalId, CancellationToken cancellationToken = default)
    {
        return db.Rentals.FindAsync(new object?[] { rentalId }, cancellationToken).AsTask();
    }

    public async Task<IList<CalendarDay>> GetCalendarDaysAsync(int rentalId, DateTime startDate, int nights, CancellationToken cancellationToken = default)
    {
        if (nights < 0)
            throw new ApplicationException("Nights must be positive");

        var rental = await GetAsync(rentalId, cancellationToken);

        if (rental == null)
            throw new ApplicationException("Rental not found");

        var dates = new List<CalendarDay>();
        for (var i = 0; i < nights; i++)
        {
            var date = new CalendarDay()
            {
                Date = startDate.Date.AddDays(i),
                Bookings = new List<Booking>(),
                PreparationTimes = new List<Unit>()
            };

            foreach (var booking in rental.Bookings)
            {
                if (booking.StartDate <= date.Date && date.Date < booking.StartDate.AddDays(booking.Nights))
                {
                    date.Bookings.Add(booking);
                }

                if (booking.StartDate.AddDays(booking.Nights) <= date.Date
                    && date.Date < booking.StartDate.AddDays(booking.Nights + rental.PreparationTime))
                {
                    date.PreparationTimes.Add(booking.Unit);
                }
            }

            dates.Add(date);
        }

        return dates;
    }
}