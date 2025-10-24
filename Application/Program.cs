using Application;
using Application.Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Custom services for dependency injection.
builder.Services.AddScoped<IBondValuationService, BondValuationService>();
builder.Services.AddScoped<IBondParser, CsvBondParser>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();