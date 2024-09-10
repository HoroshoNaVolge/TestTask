using Microsoft.EntityFrameworkCore;
using TestTask.Application.DTOs;
using TestTask.Application.Interfaces;
using TestTask.Application.Services;
using TestTask.Domain.Entities.Other;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Infrastructure.Data;
using TestTask.Infrastructure.Repositories.Other;
using TestTask.Infrastructure.Repositories.Persons;

namespace TestTask.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddScoped<IPersonRepository<Doctor>, DoctorRepository>();
            builder.Services.AddScoped<IPersonRepository<Patient>, PatientRepository>();

            builder.Services.AddScoped<ICommonRepository<Uchastok>, UchastokRepository>();
            builder.Services.AddScoped<ICommonRepository<Specialization>, SpecializationRepository>();
            builder.Services.AddScoped<ICommonRepository<Cabinet>, CabinetRepository>();

            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
