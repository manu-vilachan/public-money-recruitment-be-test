using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VacationRental.Api.Models;
using VacationRental.Core.Contracts;

namespace VacationRental.Api.Controllers;

[Route("api/v1/rentals")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly IRentalManager rentalManager;
    private readonly IMapper mapper;
    private readonly GlobalConfiguration globalConfig;

    public RentalsController(IRentalManager rentalManager, IMapper mapper, IOptions<GlobalConfiguration> globalConfig)
    {
        this.rentalManager = rentalManager;
        this.mapper = mapper;
        this.globalConfig = globalConfig.Value;
    }

    [HttpGet]
    [Route("{rentalId:int}")]
    public async Task<RentalViewModel> Get(int rentalId)
    {
        var rental = await rentalManager.GetAsync(rentalId);
        if (rental == null)
            throw new ApplicationException("Rental not found");

        return mapper.Map<RentalViewModel>(rental);
    }

    [HttpPost]
    public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
    {
        model.PreparationTimeInDays ??= globalConfig.DefaultPreparationTime;

        var rental = await rentalManager.CreateAsync(model.Units, model.PreparationTimeInDays.Value);

        return mapper.Map<ResourceIdViewModel>(rental);
    }
}