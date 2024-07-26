namespace MusicTool.Models.DTO
{
    public class SongAndCateroryList
    {
        public List<Song>? NewSong { get; set; }
        public List<Song>? SongList { get; set; }
        public List<Song>? ListeningRantings { get; set; }
        public List<Song>? DownloadRantings { get; set; }
        public List<Category>? CategoryList { get; set; }
    }
}
