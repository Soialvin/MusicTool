namespace MusicTool.Models.DTO
{
    public class DataDetailBySinger
    {
        public List<Song>? SongList { get; set; }
        public List<Song_Singer>? SongListBySinger { get; set; }
        public Song_Singer? Song_Singer { get; set; }
    }
}
