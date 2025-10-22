namespace Domain
{
    public abstract class Bond
    {
        public string BondID { get; set; }
        public string Issuer { get; set; }
        public string Rate { get; set; }
        public decimal FaceValue { get; set; }
        public string PaymentFrequency { get; set; }
        public string Rating { get; set; }
        public string Type { get; set; }
        public double YearsToMaturity { get; set; }
        public decimal DiscountFactor { get; set; }
        public string DeskNotes { get; set; }

        public abstract decimal CalculatePresentValue();
    }
}
