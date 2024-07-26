namespace MusicTool.Models.DTO
{
    public class Singer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhotoName { get; set; }
        public DateTime? LoadingDate { get; set; }
        public virtual IEnumerable<Song_Singer>? Song_Singer { get; set; }
    }
}
