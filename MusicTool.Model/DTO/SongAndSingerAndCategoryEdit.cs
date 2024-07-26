namespace MusicTool.Models.DTO
{
    public class SongAndSingerAndCategoryEdit
    {
        public EditSongRequest? editSongRequest { get; set; }
        public List<Singer>? singerList { get; set; }
        public List<Category>? categoryList { get; set; }
    }
}
