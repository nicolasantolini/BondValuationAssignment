using Application.DTOs;

namespace Application
{
    public interface IBondValuationService
    {
        BondValuationResultDTO CalculateValuations(string filePath);
    }
}