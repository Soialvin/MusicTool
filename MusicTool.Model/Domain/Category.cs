using System.ComponentModel.DataAnnotations.Schema;

namespace MusicTool.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        [Column(TypeName = "Nvarchar(100)")]
        public string? Name { get; set; }
        [Column(TypeName = "Varchar(100)")]
        public string? NameNoDiacritics { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? PhotoName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Status { get; set; }

        public virtual IEnumerable<Song_Category>? Song_Category { get; set; }
    }
}
