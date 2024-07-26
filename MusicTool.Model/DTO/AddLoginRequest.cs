using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class AddLoginRequest
    {
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string? Password { get; set; }
    }
}
