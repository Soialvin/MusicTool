using X.PagedList;

namespace MusicTool.Models.DTO
{
    public class SongAndCategoryPagination
    {
        public List<Song>? NewSong { get; set; }
        public IPagedList<Song>? SongList { get; set; }
        public List<Song>? ListeningRantings { get; set; }
        public List<Song>? DownloadRantings { get; set; }
        public List<Category>? CategoryList { get; set; }
        public int? CategoryId { get; set; }
        public string? Key { get; set; }
        public string? Action { get; set; }
    }
}
