using Domain.Exceptions;

namespace Domain
{
    public class CouponBond : Bond
    {
        public override double CalculatePresentValue()
        {
            if (FaceValue == 0 || DiscountFactor == 0)
                return 0.0;

            return CalculateCouponBondPresentValue();
        }

        private double CalculateCouponBondPresentValue()
        {
            double rate = Rate.Value;
            int paymentsPerYear = GetPaymentsPerYear();

            if(paymentsPerYear <= 0)
                throw new InvalidBondDataException("Invalid payment frequency for coupon bond.", BondId, nameof(PaymentFrequency), PaymentFrequency);

            // Coupon bonds: PV = ((1 + (Rate / Payments per Year))^{Years to Maturity × Payments per Year} × Face value) × DF
            double couponPerPeriod = rate / paymentsPerYear;
            double numberOfPeriods = YearsToMaturity * paymentsPerYear;

            double baseValue = 1.0 + couponPerPeriod;
            double exponent = numberOfPeriods;
            double futureValueFactor = Math.Pow(baseValue, exponent);

            double presentValue = futureValueFactor * FaceValue * DiscountFactor;

            return presentValue;
        }

        
    }
}