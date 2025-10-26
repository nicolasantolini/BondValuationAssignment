using Infrastructure;
using Application.DTOs;

namespace Application.Services
{
    public class BondValuationService : IBondValuationService
    {
        private readonly IBondParser _bondParser;

        public BondValuationService(IBondParser bondParser)
        {
            _bondParser = bondParser;
        }

        public BondValuationResultDTO CalculateValuations(string filePath)
        {
            BondParsingResult bondParsingResult;
            try
            {
                bondParsingResult = _bondParser.ParseBonds(filePath);
            }
            catch (Exception ex)
            {
                return new BondValuationResultDTO
                {
                    Results = new List<BondValuationDTO>(),
                    Errors = new List<string> { $"An unexpected error occurred while parsing the bond file: {ex.Message}" }
                };
            }

            var valuationResults = new List<BondValuationDTO>();
            var errors = new List<string>(bondParsingResult.Errors);

            foreach (var bond in bondParsingResult.Bonds)
            {
                try
                {
                    valuationResults.Add(new BondValuationDTO
                    {
                        BondId = bond.BondId,
                        Type = bond.Type,
                        PresentValue = Math.Round(bond.CalculatePresentValue(), 2),
                        Issuer = bond.Issuer,
                        Rating = bond.Rating,
                        YearsToMaturity = bond.YearsToMaturity,
                        DeskNotes = bond.DeskNotes ?? string.Empty
                    });
                }
                catch (Exception ex)
                {
                    errors.Add($"Error calculating present value for bond {bond.BondId}: {ex.Message}");
                }
            }

            return new BondValuationResultDTO
            {
                Results = valuationResults,
                Errors = errors
            };
        }


    }
}