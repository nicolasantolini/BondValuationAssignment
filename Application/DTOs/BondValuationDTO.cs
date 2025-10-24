namespace Application.DTOs
{
    public class BondValuationDTO
    {
        public string BondId { get; set; }
        public string Type { get; set; }
        public decimal PresentValue { get; set; }
        public string Issuer { get; set; }
        public string Rating { get; set; }
        public decimal YearsToMaturity { get; set; }
        public string DeskNotes { get; set; }
    }
}
