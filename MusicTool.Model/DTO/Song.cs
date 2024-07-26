using MusicTool.Models.Domain;

namespace MusicTool.Models.DTO
{
    public class Song
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameNoDiacritics { get; set; }
        public string? Lyrics { get; set; }
        public string? PhotoName { get; set; }
        public string? SongFile { get; set; }
        public int? NumberOfListens { get; set; }
        public int? NumberOfDownloads { get; set; }
        public DateTime? LoadingDate { get; set; }
        public virtual Account? Account { get; set; }
        public virtual IEnumerable<Song_Category>? Song_Category { get; set; }
        public virtual IEnumerable<Song_Singer>? Song_Singer { get; set; }
    }
}
