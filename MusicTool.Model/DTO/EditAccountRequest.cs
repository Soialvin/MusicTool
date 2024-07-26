using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class EditAccountRequest
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email sai định dạng")]
        public string? Email { get; set; }
        public bool Type { get; set; }
        public string? PhotoName { get; set; }
        public DateTime? Date { get; set; }
    }
}
