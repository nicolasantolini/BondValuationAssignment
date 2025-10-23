using Domain;

namespace Infrastructure
{
    public class BondParsingResult
    {
        public List<Bond> Bonds { get; } = new List<Bond>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }
}