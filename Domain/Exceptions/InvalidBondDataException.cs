using System;

namespace Domain.Exceptions
{
    public class InvalidBondDataException : Exception
    {
        public string? BondId { get; }
        public string? FieldName { get; }
        public string? InvalidValue { get; }

        public InvalidBondDataException(string message, string? bondId = null, string? fieldName = null, string? invalidValue = null)
            : base(message)
        {
            BondId = bondId;
            FieldName = fieldName;
            InvalidValue = invalidValue;
        }

        public override string Message
        {
            get
            {
                if (BondId != null && FieldName != null)
                {
                    return $"Error processing BondID '{BondId}': Invalid value '{InvalidValue}' for field '{FieldName}'. {base.Message}";
                }
                return base.Message;
            }
        }
    }
}