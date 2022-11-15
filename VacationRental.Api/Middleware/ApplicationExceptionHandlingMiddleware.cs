using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using VacationRental.Api.Models;

namespace VacationRental.Api.Middleware;

public class ApplicationExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment hostEnvironment;

    public ApplicationExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment hostEnvironment)
    {
        this.next = next;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApplicationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = new ErrorViewModel
            {
                StatusCode = context.Response.StatusCode,
                ErrorType = nameof(ApplicationException),
                Message = ex.Message, 
                StackTrace = !hostEnvironment.IsDevelopment() ? "REDACTED" : ex.StackTrace
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(message));
        }
    }
}