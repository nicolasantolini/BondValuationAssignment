using System;

namespace Application.Exceptions
{
    public class BondValuationException : Exception
    {
        public string BondId { get; }

        public BondValuationException(string message, string bondId = "", Exception innerException = null)
            : base(message, innerException)
        {
            BondId = bondId;
        }

        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(BondId))
                {
                    return $"Valuation error for BondID '{BondId}': {base.Message}";
                }
                return base.Message;
            }
        }
    }
}