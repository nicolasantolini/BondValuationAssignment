using CsvHelper;
using CsvHelper.Configuration;
using Domain;
using System.Globalization;

namespace Infrastructure
{
    public class CsvBondParser : IBondParser
    {
        public List<Bond> ParseBonds(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<CsvBondRecord>().ToList();

            var bonds = new List<Bond>();
            foreach (var record in records)
            {
                var bond = BondFactory.CreateBond(record.Type);

                // Map properties
                bond.BondID = record.BondID;
                bond.Issuer = record.Issuer;
                bond.Rate = record.Rate;
                bond.FaceValue = record.FaceValue;
                bond.PaymentFrequency = record.PaymentFrequency;
                bond.Rating = record.Rating;
                bond.Type = record.Type;
                bond.YearsToMaturity = record.YearsToMaturity;
                bond.DiscountFactor = record.DiscountFactor;
                bond.DeskNotes = record.DeskNotes;

                bonds.Add(bond);
            }

            return bonds;
        }
    }

    // Helper class for CSV mapping
    public class CsvBondRecord
    {
        public string BondID { get; set; }
        public string Issuer { get; set; }
        public string Rate { get; set; }
        public decimal FaceValue { get; set; }
        public string PaymentFrequency { get; set; }
        public string Rating { get; set; }
        public string Type { get; set; }
        public double YearsToMaturity { get; set; }
        public decimal DiscountFactor { get; set; }
        public string DeskNotes { get; set; }
    }
}