using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Data;

namespace VacationRental.Business.Tests;

[CollectionDefinition(nameof(DataBaseTestFixture))]
public class DataBaseTestFixture: IDisposable, ICollectionFixture<DataBaseTestFixture>
{
    public ServiceProvider Container { get; set; }

    public DataBaseTestFixture()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<VacationRentalDbContext>(options => options.UseInMemoryDatabase("BusinessTests"));
        serviceCollection.AddTransient<RentalManager>();
        serviceCollection.AddTransient<BookingManager>();

        Container = serviceCollection.BuildServiceProvider();
    }

    public void CleanupDb()
    {
        var db = Container.GetRequiredService<VacationRentalDbContext>();

        db.Bookings.ToList().ForEach(b => db.Remove(b));
        db.Rentals.ToList().ForEach(r => db.Remove(r));
        db.SaveChanges();
    }

    public void Dispose()
    {
        Container.Dispose();
    }
}