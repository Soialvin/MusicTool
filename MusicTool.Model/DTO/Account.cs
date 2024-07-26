using MusicTool.Models.Domain;

namespace MusicTool.Models.DTO
{
    public class Account
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UserType { get; set; }
        public bool Type { get; set; }
        public string? PhotoName { get; set; }

        public virtual IEnumerable<Song>? Songs { get; set; }
        public virtual IEnumerable<VIP>? VIPs { get; set; }
    }
}
