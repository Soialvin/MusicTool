namespace MusicTool.Models.DTO
{
    public class SongAndSingerAndCategoryAdd
    {
        public AddSongRequest? addSongRequest { get; set; }
        public List<Singer>? singerList { get; set; }
        public List<Category>? categoryList { get; set; }
    }
}
