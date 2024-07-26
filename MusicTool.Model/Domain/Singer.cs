using System.ComponentModel.DataAnnotations.Schema;

namespace MusicTool.Models.Domain
{
    public class Singer
    {
        public int Id { get; set; }
        [Column(TypeName = "Nvarchar(50)")]
        public string? Name { get; set; }
        [Column(TypeName = "Varchar(50)")]
        public string? NameNoDiacritics { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? PhotoName { get; set; }
        public DateTime? LoadingDate { get; set; }
        public bool Status { get; set; }

        public virtual IEnumerable<Song_Singer>? Song_Singer { get; set; }
    }
}
