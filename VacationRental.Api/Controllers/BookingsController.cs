using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Core.Contracts;

namespace VacationRental.Api.Controllers;

[Route("api/v1/bookings")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingManager bookingManager;
    private readonly IValidator<BookingBindingModel> bookingBindingValidator;
    private readonly IMapper mapper;

    public BookingsController(IBookingManager bookingManager, IValidator<BookingBindingModel> bookingBindingValidator, IMapper mapper)
    {
        this.bookingManager = bookingManager;
        this.bookingBindingValidator = bookingBindingValidator;
        this.mapper = mapper;
    }

    [HttpGet]
    [Route("{bookingId:int}")]
    public async Task<BookingViewModel> Get(int bookingId)
    {
        var booking = await bookingManager.GetAsync(bookingId);

        if (booking == null)
            throw new ApplicationException("Booking not found");

        return mapper.Map<BookingViewModel>(booking);
    }

    [HttpPost]
    public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
    {
        var validationResult = bookingBindingValidator.Validate(model);
        if (!validationResult.IsValid)
            throw new ApplicationException(validationResult.ToString());

        var booking = await bookingManager.CreateAsync(model.RentalId, model.Start, model.Nights);

        return mapper.Map<ResourceIdViewModel>(booking);
    }
}