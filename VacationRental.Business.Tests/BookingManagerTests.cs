using Microsoft.Extensions.DependencyInjection;
using VacationRental.Core.Domain;

namespace VacationRental.Business.Tests;

[Collection(nameof(DataBaseTestFixture))]
public class BookingManagerTests: IDisposable
{
    private readonly DataBaseTestFixture fixture;
    private readonly IServiceProvider container;

    public BookingManagerTests(DataBaseTestFixture fixture)
    {
        this.fixture = fixture;
        this.container = fixture.Container.CreateScope().ServiceProvider;
    }

    public void Dispose()
    {
        fixture.CleanupDb();
    }

    [Fact]
    public async Task CreateBooking_WithoutAvailabilityDuePreparationTime_ThrowsException()
    {
        var rental = await CreateRentalAsync(2, 1);

        var bookingManager = container.GetRequiredService<BookingManager>();

        await bookingManager.CreateAsync(rental.Id, new DateTime(2022, 1, 1), 2);
        await bookingManager.CreateAsync(rental.Id, new DateTime(2022, 1, 3), 2);

        await Assert.ThrowsAsync<ApplicationException>(async () => await bookingManager.CreateAsync(rental.Id, new DateTime(2022, 1, 3), 2));
    }

    [Fact]
    public async Task CreateBooking_WithoutAvailabilityJustAfterPreparationTime_CreateTheBooking()
    {
        var rental = await CreateRentalAsync(2, 1);

        var bookingManager = container.GetRequiredService<BookingManager>();

        await bookingManager.CreateAsync(rental.Id, new DateTime(2022, 1, 1), 2);
        await bookingManager.CreateAsync(rental.Id, new DateTime(2022, 1, 3), 2);

        var newBooking = await bookingManager.CreateAsync(rental.Id, new DateTime(2022, 1, 4), 2);
        Assert.NotNull(newBooking);
        Assert.Equal(1, newBooking.Unit.UnitNumber);
    }

    private async Task<Rental> CreateRentalAsync(int units, int preparationTime)
    {
        var rentalManager = container.GetRequiredService<RentalManager>();
        return await rentalManager.CreateAsync(units, preparationTime);
    }
}