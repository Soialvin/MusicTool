namespace MusicTool.Models.DTO
{
    public class AddVIPRequest
    {
        public DateTime? VIPRegistrationDate { get; set; }
        public DateTime? VIPExpirationDate { get; set; }
        public double Price { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }
    }
}
