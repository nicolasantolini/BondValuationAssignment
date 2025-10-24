using System;

namespace Application.Exceptions
{
    public class BondValuationException : Exception
    {
        public string BondID { get; }

        public BondValuationException(string message, string bondId = null, Exception innerException = null)
            : base(message, innerException)
        {
            BondID = bondId;
        }

        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(BondID))
                {
                    return $"Valuation error for BondId '{BondID}': {base.Message}";
                }
                return base.Message;
            }
        }
    }
}