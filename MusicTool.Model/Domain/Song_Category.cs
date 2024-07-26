namespace MusicTool.Models.Domain
{
    public class Song_Category
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? SongId { get; set; }

        public virtual Song? Song { get; set; }
        public virtual Category? Category { get; set; }
    }
}
