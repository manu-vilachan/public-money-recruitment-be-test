namespace VacationRental.Core.Domain;

public class Unit
{
    public int Id { get; set; }

    public int UnitNumber { get; set; }

    public virtual List<Booking> Bookings { get; set; }

    //It can hold other important information in the future. Eg: # of guests, own bathroom...
}