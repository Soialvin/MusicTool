using X.PagedList;

namespace MusicTool.Models.DTO
{
    public class IndexAccountRequest
    {
        public string? Key { get; set; }
        public IPagedList<Account>? List { get; set; }
        public string? ActionName { get; set; }
    }
}
