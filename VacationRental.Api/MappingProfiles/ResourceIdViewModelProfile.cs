using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Core.Domain;

namespace VacationRental.Api.MappingProfiles;

public class ResourceIdViewModelProfile : Profile
{
    public ResourceIdViewModelProfile()
    {
        CreateMap<Rental, ResourceIdViewModel>();
        CreateMap<Booking, ResourceIdViewModel>();
    }
}