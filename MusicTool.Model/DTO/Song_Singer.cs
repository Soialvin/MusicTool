namespace MusicTool.Models.DTO
{
    public class Song_Singer
    {
        public int Id { get; set; }
        public int? SongId { get; set; }
        public int? SingerId { get; set; }
        public virtual Song? Song { get; set; }
        public virtual Singer? Singer { get; set; }
    }
}
