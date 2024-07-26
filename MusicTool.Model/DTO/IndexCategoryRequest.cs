using X.PagedList;

namespace MusicTool.Models.DTO
{
    public class IndexCategoryRequest
    {
        public string? Key { get; set; }
        public IPagedList<Category>? List { get; set; }
        public string? ActionName { get; set; }
    }
}
