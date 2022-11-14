namespace VacationRental.Core.Domain;

public class Booking
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public int Nights { get; set; }

    public virtual Rental Rental { get; set; }
}