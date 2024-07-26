namespace MusicTool.Models.Domain
{
    public class VIP
    {
        public int Id { get; set; }
        public DateTime? VIPRegistrationDate { get; set; }
        public DateTime? VIPExpirationDate { get; set; }
        public double Price { get; set; }
        public int? AccountId { get; set; }
        public bool Status { get; set; }

        public virtual Account? Account { get; set; }
    }
}
