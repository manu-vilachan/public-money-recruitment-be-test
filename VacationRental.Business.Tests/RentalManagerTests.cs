using Microsoft.Extensions.DependencyInjection;
using VacationRental.Core.Domain;

namespace VacationRental.Business.Tests;

[Collection(nameof(DataBaseTestFixture))]
public class RentalManagerTests : IDisposable
{
    private readonly DataBaseTestFixture fixture;
    private readonly IServiceProvider container;

    public RentalManagerTests(DataBaseTestFixture fixture)
    {
        this.fixture = fixture;
        this.container = fixture.Container.CreateScope().ServiceProvider;
    }

    public void Dispose()
    {
        fixture.CleanupDb();
    }

    [Fact]
    public async Task GetCalendarDays_WithTwoBookings_DaysReflectBookedDaysAndPreparationTimes()
    {
        var rentalManager = container.GetRequiredService<RentalManager>();

        var rental = await rentalManager.CreateAsync(2, 2);
        var booking1 = await CreateBookingAsync(rental.Id, new DateTime(2000, 01, 02), 2);
        var booking2 = await CreateBookingAsync(rental.Id, new DateTime(2000, 01, 03), 2);

        var calendarDays = await rentalManager.GetCalendarDaysAsync(rental.Id, new DateTime(2000, 1, 1), 7);
        Assert.Equal(7, calendarDays.Count);

        var day1 = calendarDays[0];
        Assert.Equal(new DateTime(2000, 01, 01), day1.Date);
        Assert.Empty(day1.Bookings!);
        Assert.Empty(day1.PreparationTimes!);
                
        var day2 = calendarDays[1];
        Assert.Equal(new DateTime(2000, 01, 02), day2.Date);
        Assert.Single(day2.Bookings);
        Assert.Contains(day2.Bookings, x => x.Id == booking1.Id);
        Assert.Empty(day2.PreparationTimes);

        var day3 = calendarDays[2];
        Assert.Equal(new DateTime(2000, 01, 03), day3.Date);
        Assert.Equal(2, day3.Bookings.Count);
        Assert.Contains(day3.Bookings, x => x.Id == booking1.Id);
        Assert.Contains(day3.Bookings, x => x.Id == booking2.Id);
        Assert.Empty(day3.PreparationTimes);

        var day4 = calendarDays[3];
        Assert.Equal(new DateTime(2000, 01, 04), day4.Date);
        Assert.Single(day4.Bookings);
        Assert.Contains(day4.Bookings, x => x.Id == booking2.Id);
        Assert.Single(day4.PreparationTimes);
        Assert.Contains(day4.PreparationTimes, x => x.UnitNumber == booking1.Unit.UnitNumber);

        var day5 = calendarDays[4];
        Assert.Equal(new DateTime(2000, 01, 05), day5.Date);
        Assert.Empty(day5.Bookings);
        Assert.Equal(2, day5.PreparationTimes.Count);
        Assert.Contains(day5.PreparationTimes, x => x.UnitNumber == booking1.Unit.UnitNumber);
        Assert.Contains(day5.PreparationTimes, x => x.UnitNumber == booking2.Unit.UnitNumber);

        var day6 = calendarDays[5];
        Assert.Equal(new DateTime(2000, 01, 06), day6.Date);
        Assert.Empty(day6.Bookings);
        Assert.Single(day6.PreparationTimes);
        Assert.Contains(day6.PreparationTimes, x => x.UnitNumber == booking2.Unit.UnitNumber);

        var day7 = calendarDays[6];
        Assert.Equal(new DateTime(2000, 01, 07), day7.Date);
        Assert.Empty(day7.Bookings);
        Assert.Empty(day7.PreparationTimes);
    }

    private async Task<Booking> CreateBookingAsync(int rentalId, DateTime startDate, int nights)
    {
        var bookingManager = container.GetRequiredService<BookingManager>();
        return await bookingManager.CreateAsync(rentalId, startDate, nights);
    }
}