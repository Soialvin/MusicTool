using X.PagedList;

namespace MusicTool.Models.DTO
{
    public class IndexSongRequest
    {
        public string? Key { get; set; }
        public IPagedList<Song>? List { get; set; }
        public string? ActionName { get; set; }
    }
}
