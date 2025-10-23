using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using Application.DTOs;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BondValuationController : ControllerBase
    {
        private readonly IBondValuationService _bondValuationService;

        public BondValuationController(IBondValuationService bondValuationService)
        {
            _bondValuationService = bondValuationService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateValuations(IFormFile file, [FromQuery] string format = "json")
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded");

            // Save the uploaded file temporarily
            var tempFilePath = Path.GetTempFileName();
            try
            {
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Process the file using the valuation service
                var results = _bondValuationService.CalculateValuations(tempFilePath);

                // Return results in the requested format
                return format.ToLower() switch
                {
                    "json" => Ok(results),
                    "csv" => File(GenerateCsv(results.Results), "text/csv", "bond_valuations_"+DateTime.Now.ToShortDateString()+".csv"),
                    _ => BadRequest($"Unsupported format: {format}")
                };
            }
            finally
            {
                // Clean up temporary file
                if (System.IO.File.Exists(tempFilePath))
                    System.IO.File.Delete(tempFilePath);
            }
        }

        private byte[] GenerateCsv(List<BondValuationDTO> results)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(results);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}