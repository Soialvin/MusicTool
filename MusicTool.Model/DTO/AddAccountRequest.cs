using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class AddAccountRequest
    {
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên tài khoản không được chứa khoảng trắng hoặc ký tự tiếng Việt")]
        public string? UserName { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email sai định dạng")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9\W]+$", ErrorMessage = "Mật khẩu không được chứa ký tự tiếng Việt")]
        public string? Password { get; set; }
        public bool Type { get; set; }
        public string? PhotoName { get; set; }
    }
}
