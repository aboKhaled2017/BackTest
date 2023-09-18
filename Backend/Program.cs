using Backend;
using Backend.Configurations;
using Backend.DataModels;
using Backend.Repos;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var appAssembly = Assembly.GetExecutingAssembly();

//register the required service to DI
builder.ConfigureLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseSqlite(builder.Configuration.GetConnectionString("db"));
});

var deiscriptors = builder.Services.Where(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>)).ToList();

builder.Services.AddScoped<IDriverRepo, DriverRepo>();
builder.Services.AddScoped<INamesProcessingRepo, NamesProcessingRepo>();

builder.Services.AddControllers()
    .AddFluentValidation(opt =>
    {
        opt.AutomaticValidationEnabled = true;
        opt.RegisterValidatorsFromAssembly(appAssembly);
    });

builder.Services.ConfigureOptions<SeederConfigurations>();

builder.Services.AddSingleton<DataSeederManager>();

var app = builder.Build();

app.UseMigrationsMiddleware();

await app.RunAppSeederAsync(app.Services);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCustomeExceptionMiddleware();

app.MapControllers();

app.Run();

public partial class Program { }