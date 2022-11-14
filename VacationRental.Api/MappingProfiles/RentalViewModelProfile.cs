using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Core.Domain;

namespace VacationRental.Api.MappingProfiles;

public class RentalViewModelProfile : Profile
{
    public RentalViewModelProfile()
    {
        CreateMap<Rental, RentalViewModel>();
    }
}