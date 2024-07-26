using System.ComponentModel.DataAnnotations;

namespace MusicTool.Models.DTO
{
    public class AddSongRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Hãy chọn ca sĩ")]
        public List<int>? SingerId { get; set; }
        [Required(ErrorMessage = "Hãy chọn thể loại")]
        public List<int>? CategoryId { get; set; }
        public string? Lyrics { get; set; }
        public string? PhotoName { get; set; }
        public string? SongFile { get; set; }
    }
}
