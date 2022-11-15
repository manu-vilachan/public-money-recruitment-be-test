namespace VacationRental.Core.Domain;

public class CalendarDay
{
    public DateTime Date { get; set; }

    public List<Booking>? Bookings { get; set; }

    public List<Unit>? PreparationTimes { get; set; }
}