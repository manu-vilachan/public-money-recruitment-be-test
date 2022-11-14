using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Core.Domain;

namespace VacationRental.Api.MappingProfiles;

public class CalendarDateViewModelProfile : Profile
{
    public CalendarDateViewModelProfile()
    {
        CreateMap<CalendarDay, CalendarDateViewModel>();
        CreateMap<Booking, CalendarBookingViewModel>();
    }
}