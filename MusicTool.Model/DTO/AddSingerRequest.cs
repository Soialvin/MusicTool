using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class AddSingerRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        public string? PhotoName { get; set; }
    }
}
