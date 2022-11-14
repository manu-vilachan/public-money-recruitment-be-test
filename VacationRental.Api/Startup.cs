using AutoMapper.EquivalencyExpression;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VacationRental.Business;
using VacationRental.Core.Contracts;
using VacationRental.Data;

namespace VacationRental.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static IConfiguration Configuration { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvcCore();

        services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

        services.AddDbContext<VacationRentalDbContext>(builder => builder
            .UseInMemoryDatabase("VacationRental")
            .UseLazyLoadingProxies()); //I use lazy loading just for convenience in this testing project. Further analysis should be done in case of a real scenario.

        services.AddTransient<IBookingManager, BookingManager>();
        services.AddTransient<IRentalManager, RentalManager>();

        services.AddAutoMapper(cfg => cfg.AddCollectionMappers(), typeof(Startup).Assembly);
        services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        // It would be very convenient to configure Authentication, and Authorization if needed.
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(builder => builder.MapControllers());
        app.UseSwagger();
        app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
    }
}