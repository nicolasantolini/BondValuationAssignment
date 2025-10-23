using Domain;

namespace Infrastructure
{
    public interface IBondParser
    {
        BondParsingResult ParseBonds(string filePath);
    }
}