using System;

namespace Domain.Exceptions
{
    public class InvalidBondDataException : Exception
    {
        public string BondID { get; }
        public string FieldName { get; }
        public string InvalidValue { get; }

        public InvalidBondDataException(string message, string bondId = null, string fieldName = null, string invalidValue = null) : base(message)
        {
            BondID = bondId;
            FieldName = fieldName;
            InvalidValue = invalidValue;
        }

        public override string Message
        {
            get
            {
                if (BondID != null && FieldName != null)
                {
                    return $"Error processing BondID '{BondID}': Invalid value '{InvalidValue}' for field '{FieldName}'. {base.Message}";
                }
                return base.Message;
            }
        }
    }
}