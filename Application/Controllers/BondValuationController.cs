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

        [HttpPost()]
        public async Task<IActionResult> CalculateValuations(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded");

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only CSV files are supported");

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

                // Content negotiation based on Accept header
                var acceptHeader = Request.Headers["Accept"].ToString();

                if (acceptHeader.Contains("text/csv"))
                {
                    return File(GenerateCsv(results.Results), "text/csv", "bond_valuations_" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
                }
                
                // Default to JSON
                return Ok(results);
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