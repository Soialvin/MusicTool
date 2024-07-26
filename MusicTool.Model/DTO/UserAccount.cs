namespace MusicTool.Models.DTO
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; }
        public string? PhotoName { get; set; }
    }
}
