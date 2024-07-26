using System.ComponentModel.DataAnnotations.Schema;

namespace MusicTool.Models.Domain
{
    public class Song
    {
        public int Id { get; set; }
        [Column(TypeName = "Nvarchar(max)")]
        public string? Name { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? NameNoDiacritics { get; set; }
        public int? AccountId { get; set; }
        [Column(TypeName = "Nvarchar(max)")]
        public string? Lyrics { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? PhotoName { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? SongFile { get; set; }
        public int? NumberOfListens { get; set; }
        public int? NumberOfDownloads { get; set; }
        public DateTime? LoadingDate { get; set; }
        public bool Status { get; set; }

        public virtual Account? Account { get; set; }
        public virtual IEnumerable<Song_Category>? Song_Category { get; set; }
        public virtual IEnumerable<Song_Singer>? Song_Singer { get; set; }
    }
}
