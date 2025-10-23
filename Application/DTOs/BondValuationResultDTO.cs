namespace Application.DTOs
{
    public class BondValuationResultDTO
    {
        public List<BondValuationDTO> Results { get; set; } = new List<BondValuationDTO>();
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }
}
