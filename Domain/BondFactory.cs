namespace Domain
{
    public static class BondFactory
    {
        public static Bond CreateBond(BondCreationData data)
        {
            var t = data.Type.Trim();

            Bond bond = t switch
            {
                var s when s.Equals("bond", StringComparison.OrdinalIgnoreCase)
                        || s.Equals("inflation-linked", StringComparison.OrdinalIgnoreCase) // uses coupon logic
                    => new CouponBond
                    {
                        BondId = data.BondId,
                        Issuer = data.Issuer,
                        Rate = Rate.Parse(data.Rate, data.BondId),
                        FaceValue = data.FaceValue,
                        PaymentFrequency = data.PaymentFrequency,
                        Rating = data.Rating,
                        Type = data.Type,
                        YearsToMaturity = data.YearsToMaturity,
                        DiscountFactor = data.DiscountFactor,
                        DeskNotes = data.DeskNotes
                    },

                var s when s.Equals("zero-coupon", StringComparison.OrdinalIgnoreCase)
                    => new ZeroCouponBond
                    {
                        BondId = data.BondId,
                        Issuer = data.Issuer,
                        Rate = Rate.Parse(data.Rate, data.BondId),
                        FaceValue = data.FaceValue,
                        PaymentFrequency = data.PaymentFrequency,
                        Rating = data.Rating,
                        Type = data.Type,
                        YearsToMaturity = data.YearsToMaturity,
                        DiscountFactor = data.DiscountFactor,
                        DeskNotes = data.DeskNotes
                    },

                _ => throw new ArgumentException($"Unknown bond type '{data.Type}'.", nameof(data.Type))
            };

            return bond;
        }
    }
}