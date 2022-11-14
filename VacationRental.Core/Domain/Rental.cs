namespace VacationRental.Core.Domain;

public class Rental
{
    public int Id { get; set; }

    public int PreparationTime { get; set; }

    public int Units { get; set; }

    public virtual List<Booking> Bookings { get; set; }
}