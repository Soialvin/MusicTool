using MusicTool.Models.Domain;

namespace MusicTool.Models.DTO
{
    public class DataDetail
    {
        public List<Song>? SongList { get; set; }
        public List<Song_Singer>? RelatedSong { get; set; }
        public Song? Song { get; set; }
    }
}
