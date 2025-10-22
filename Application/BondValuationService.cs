using Infrastructure;

namespace Application
{
    public class BondValuationService : IBondValuationService
    {
        private readonly IBondParser _bondParser;

        public BondValuationService(IBondParser bondParser)
        {
            _bondParser = bondParser;
        }

        public List<BondValuationResult> CalculateValuations(string filePath)
        {
            var bonds = _bondParser.ParseBonds(filePath);

            var results = bonds.Select(bond => new BondValuationResult
            {
                BondID = bond.BondID,
                Type = bond.Type,
                PresentValue = bond.CalculatePresentValue(),
                Issuer = bond.Issuer,
                Rating = bond.Rating,
                YearsToMaturity = bond.YearsToMaturity,
                DeskNotes = bond.DeskNotes
            }).ToList();

            return results;
        }
    }
}