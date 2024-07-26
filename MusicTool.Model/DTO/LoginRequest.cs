namespace MusicTool.Models.DTO
{
    public class LoginRequest
    {
        public int Id { get; set; }
        public string? PhotoName { get; set; }
        public bool Type { get; set; }
        public string? UserType { get; set; }
    }
}
