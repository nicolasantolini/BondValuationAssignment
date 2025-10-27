namespace Domain
{
    public class ZeroCouponBond : Bond
    {
        public override decimal CalculatePresentValue()
        {
            if (FaceValue == 0 || DiscountFactor == 0)
                return 0.0m;

            // Zero-coupon bonds: PV = ((1 + Rate)^{Years to Maturity} × Face value) × DF
            double rate = (double)Rate.Value;
            double yearsToMaturity = (double)YearsToMaturity;
            double faceValue = (double)FaceValue;
            double discountFactor = (double)DiscountFactor;

            double futureValueFactor = Math.Pow(1 + rate, yearsToMaturity);
            double presentValue = futureValueFactor * faceValue * discountFactor;

            //Preserving accuracy up to 4 decimal places
            decimal finalPresentValue = (decimal)Math.Round(presentValue * 10000) / 10000m;

            return finalPresentValue;
        }

    }
}