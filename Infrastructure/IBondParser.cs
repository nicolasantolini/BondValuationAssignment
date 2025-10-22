using Domain;

namespace Infrastructure
{
    public interface IBondParser
    {
        List<Bond> ParseBonds(string filePath);
    }
}