using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class AddCategoryRequest
    {
        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        public string? Name { get; set; }
        public string? PhotoName { get; set; }
    }
}
