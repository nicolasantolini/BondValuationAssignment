//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Application;
using CsvHelper;
using CsvHelper.Configuration;
using Infrastructure;
using System.Globalization;

// Setup dependencies
var bondParser = new CsvBondParser();
var valuationService = new BondValuationService(bondParser);

// Calculate valuations
var results = valuationService.CalculateValuations("bond_positions_sample.csv");

// Output results to CSV
var outputConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    Delimiter = ";",
    HasHeaderRecord = true
};

using var writer = new StreamWriter("bond_valuations_output.csv");
using var csv = new CsvWriter(writer, outputConfig);

csv.WriteRecords(results);

Console.WriteLine($"Valuation completed! {results.Count} bonds processed.");
Console.WriteLine("Output saved to: bond_valuations_output.csv");

// Display sample results
Console.WriteLine("\nSample results:");
foreach (var result in results.Take(5))
{
    Console.WriteLine($"BondID: {result.BondID}, Type: {result.Type}, PV: {result.PresentValue:C}");
}
