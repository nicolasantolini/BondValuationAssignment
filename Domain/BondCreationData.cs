namespace Domain
{
    public record BondCreationData
    {
        public required string BondId { get; init; }
        public required string Issuer { get; init; }
        public required string Rate { get; init; }
        public required decimal FaceValue { get; init; }
        public required string PaymentFrequency { get; init; }
        public required string Rating { get; init; }
        public required string Type { get; init; }
        public required decimal YearsToMaturity { get; init; }
        public required decimal DiscountFactor { get; init; }
        public string? DeskNotes { get; init; }
    }
}