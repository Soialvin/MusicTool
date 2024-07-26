using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class EditSingerRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        public string? PhotoName { get; set; }
    }
}
