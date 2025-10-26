namespace Application.DTOs
{
    public class BondValuationDTO
    {
        public required string BondId { get; set; }
        public required string Type { get; set; }
        public required double PresentValue { get; set; }
        public required string Issuer { get; set; }
        public required string Rating { get; set; }
        public required double YearsToMaturity { get; set; }
        public required string DeskNotes { get; set; }
    }
}
