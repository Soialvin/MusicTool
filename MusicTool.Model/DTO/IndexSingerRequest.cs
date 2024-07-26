using X.PagedList;

namespace MusicTool.Models.DTO
{
    public class IndexSingerRequest
    {
        public string? Key { get; set; }
        public IPagedList<Singer>? List { get; set; }
        public string? ActionName { get; set; }
    }
}
