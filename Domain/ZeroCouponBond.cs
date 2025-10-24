using System;
using System.Globalization;

namespace Domain
{
    public class ZeroCouponBond : Bond
    {
        public override decimal CalculatePresentValue()
        {
            if (FaceValue == 0 || DiscountFactor == 0)
                return 0m;

            // Zero-coupon bonds: PV = ((1 + Rate)^{Years to Maturity} × Face value) × DF
            decimal rate = Rate.Value;
            decimal futureValueFactor = (decimal)Math.Pow((double)(1 + rate), (double)YearsToMaturity);
            decimal presentValue = futureValueFactor * FaceValue * DiscountFactor;

            return Math.Round(presentValue, 2);
        }

    }
}