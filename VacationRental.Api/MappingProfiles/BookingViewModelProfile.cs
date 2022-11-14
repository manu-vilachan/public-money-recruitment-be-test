using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Core.Domain;

namespace VacationRental.Api.MappingProfiles;

public class BookingViewModelProfile : Profile
{
    public BookingViewModelProfile()
    {
        CreateMap<Booking, BookingViewModel>()
            .ForMember(m => m.RentalId, exp => exp.MapFrom(b => b.Rental.Id))
            .ForMember(m => m.Start, exp => exp.MapFrom(b => b.StartDate));
    }
}