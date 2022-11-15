using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Core.Contracts;

namespace VacationRental.Api.Controllers;

[Route("api/v1/calendar")]
[ApiController]
public class CalendarController : ControllerBase
{
    private readonly IRentalManager rentalManager;
    private readonly IMapper mapper;

    public CalendarController(IRentalManager rentalManager, IMapper mapper)
    {
        this.rentalManager = rentalManager;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
    {
        var dates = await rentalManager.GetCalendarDaysAsync(rentalId, start, nights);

        var result = new CalendarViewModel 
        {
            RentalId = rentalId,
            Dates = mapper.Map<List<CalendarDateViewModel>>(dates)
        };

        return result;
    }
}