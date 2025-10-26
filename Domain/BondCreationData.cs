namespace Domain
{
    //This class matches the structure of the CSV input for bond creation
    public record BondCreationData
    {
        // Unconventional naming to match CSV headers for BondID
        public required string BondID { get; init; }
        public required string Issuer { get; init; }
        public required string Rate { get; init; }
        public required double FaceValue { get; init; }
        public required string PaymentFrequency { get; init; }
        public required string Rating { get; init; }
        public required string Type { get; init; }
        public required double YearsToMaturity { get; init; }
        public required double DiscountFactor { get; init; }
        public string? DeskNotes { get; init; }
    }
}