using MusicTool.Models.Domain;

namespace MusicTool.Models.DTO
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhotoName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual IEnumerable<Song_Category>? Song_Category { get; set; }
    }
}
