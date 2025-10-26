namespace Domain
{
    public class ZeroCouponBond : Bond
    {
        public override double CalculatePresentValue()
        {
            if (FaceValue == 0 || DiscountFactor == 0)
                return 0.0;

            // Zero-coupon bonds: PV = ((1 + Rate)^{Years to Maturity} × Face value) × DF
            double rate = Rate.Value;
            double futureValueFactor = Math.Pow(1 + rate, YearsToMaturity);
            double presentValue = futureValueFactor * FaceValue * DiscountFactor;

            return presentValue;
        }

    }
}