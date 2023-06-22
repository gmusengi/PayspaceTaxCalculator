using Microsoft.EntityFrameworkCore;
using PayspaceTaxCalculator.Application.Command;
using PayspaceTaxCalculator.Domain.Interfaces;
using PayspaceTaxCalculator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(options => options.RegisterServicesFromAssemblies(typeof(CreateProgressiveTaxRateCommand).Assembly));
IConfiguration configuration = builder.Configuration;
builder.Services.AddDbContext<PayspaceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepository, Repository>();

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
