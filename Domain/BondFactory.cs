namespace Domain
{
    public static class BondFactory
    {
        public static Bond CreateBond(string type)
        {
            return type?.ToLower() switch
            {
                "bond" => new CouponBond(),
                "zero-coupon" => new ZeroCouponBond(),
                "inflation-linked" => new CouponBond(), // Inflation-linked use coupon bond calculation
                _ => new CouponBond() // Default to coupon bond
            };
        }
    }
}