using Application;
using Application.Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register your custom services for dependency injection.
builder.Services.AddScoped<IBondValuationService, BondValuationService>();
builder.Services.AddScoped<IBondParser, CsvBondParser>();

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

//using Application;
//using Application.Services;
//using CsvHelper;
//using CsvHelper.Configuration;
//using Infrastructure;
//using System.Globalization;

//// Setup dependencies
//var bondParser = new CsvBondParser();
//var valuationService = new BondValuationService(bondParser);

//// Calculate valuations
//var results = valuationService.CalculateValuations("bond_positions_sample.csv");

//// Output results to CSV
//var outputConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//{
//    Delimiter = ";",
//    HasHeaderRecord = true
//};

//using var writer = new StreamWriter("bond_valuations_output.csv");
//using var csv = new CsvWriter(writer, outputConfig);

//csv.WriteRecords(results.Results);

//Console.WriteLine($"Valuation completed! {results.Results.Count} bonds processed.");
//Console.WriteLine("Output saved to: bond_valuations_output.csv");

//if (results.HasErrors)
//{
//    foreach (var error in results.Errors)
//    {
//        Console.WriteLine($"Error: {error}");
//    }
//}

//// Display sample results
//Console.WriteLine("\nSample results:");
//foreach (var result in results.Results.Take(5))
//{
//    Console.WriteLine($"BondID: {result.BondID}, Type: {result.Type}, PV: {result.PresentValue:C}");
//}
