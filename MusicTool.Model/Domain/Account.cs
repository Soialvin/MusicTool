using System.ComponentModel.DataAnnotations.Schema;

namespace MusicTool.Models.Domain
{
    public class Account
    {
        public int Id { get; set; }
        [Column(TypeName = "Varchar(50)")]
        public string? UserName { get; set; }
        [Column(TypeName = "Nvarchar(100)")]
        public string? Name { get; set; }
        [Column(TypeName = "Varchar(100)")]
        public string? Email { get; set; }
        [Column(TypeName = "Varchar(60)")]
        public string? Password { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? PhotoName { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "Varchar(6)")]
        public string? UserType { get; set; }
        public bool Type { get; set; }
        public bool Status { get; set; }

        public virtual IEnumerable<Song>? Songs { get; set; }
        public virtual IEnumerable<VIP>? VIPs { get; set; }
    }
}
