namespace Application
{
    public interface IBondValuationService
    {
        List<BondValuationResult> CalculateValuations(string filePath);
    }

    public class BondValuationResult
    {
        public string BondID { get; set; }
        public string Type { get; set; }
        public decimal PresentValue { get; set; }
        public string Issuer { get; set; }
        public string Rating { get; set; }
        public double YearsToMaturity { get; set; }
        public string DeskNotes { get; set; }
    }
}