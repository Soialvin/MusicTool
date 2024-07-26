using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.CategoryRepositories;
using MusicTool.DataAccess.Repositories.SingerRepositories;
using MusicTool.DataAccess.Repositories.Song_CategoryRepositories;
using MusicTool.DataAccess.Repositories.Song_SingerRepositories;
using MusicTool.DataAccess.Repositories.SongRepositories;
using X.PagedList;

namespace MusicTool.Controllers
{
    public class MTHomeController : Controller
    {
        private readonly ISong_CategoryRepository _song_CategoryRepository;
        private readonly ISong_SingerRepository _song_SingerRepository;
        private readonly ISingerRepository _singerRepository;
        private readonly ISongRepository _songRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public int size = 10;
        public int sizeSinger = 12;

        public MTHomeController(ISong_CategoryRepository song_CategoryRepository, ISong_SingerRepository song_SingerRepository, ISingerRepository singerRepository, ISongRepository songRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _song_CategoryRepository = song_CategoryRepository;
            _song_SingerRepository = song_SingerRepository;
            _singerRepository = singerRepository;
            _songRepository = songRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var songs = await _songRepository.GetAllAsync();
            if (songs.Count() < 10)
            {
                return NotFound();
            }
            var newSong = await _songRepository.GetTOPAsync("NewSong", 10);
            var songList = await _songRepository.GetTOPAsync("SongList", 5);
            var listeningRantings = await _songRepository.GetTOPAsync("ListeningRantings", 10);
            var downloadRantings = await _songRepository.GetTOPAsync("DownloadRantings", 10);
            if (newSong == null || songList == null || listeningRantings == null || downloadRantings == null)
            {
                return NotFound();
            }
            var categorys = await _categoryRepository.GetAllAsync();
            var songAndCategoryList = new Models.DTO.SongAndCateroryList()
            {
                NewSong = _mapper.Map<List<Models.DTO.Song>>(newSong),
                SongList = _mapper.Map<List<Models.DTO.Song>>(songList),
                ListeningRantings = _mapper.Map<List<Models.DTO.Song>>(listeningRantings),
                DownloadRantings = _mapper.Map<List<Models.DTO.Song>>(downloadRantings),
                CategoryList = _mapper.Map<List<Models.DTO.Category>>(categorys)
            };
            return View(songAndCategoryList);
        }
        public async Task<IActionResult> SongDetail(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            var songList = await _songRepository.GetTOPAsync("SongList", 5);
            if (songList == null)
            {
                return NotFound();
            }
            var relatedSong = new List<Models.Domain.Song_Singer>();
            var singerList = song.Song_Singer.Select(x => x.Singer).ToList();
            foreach (var item in singerList)
            {
                var song_Singer = await _song_SingerRepository.GetRelatedSongAsync(item.Id);
                foreach (var item1 in song_Singer)
                {
                    if (!relatedSong.Any(x => x.SongId == item1.SongId))
                    {
                        relatedSong.Add(item1);
                    }
                }
            }
            if (relatedSong == null)
            {
                return NotFound();
            }
            var data = new Models.DTO.DataDetail()
            {
                Song = _mapper.Map<Models.DTO.Song>(song),
                SongList = _mapper.Map<List<Models.DTO.Song>>(songList),
                RelatedSong = _mapper.Map<List<Models.DTO.Song_Singer>>(relatedSong)
            };
            return View(data);
        }
        public async Task<IActionResult> Ranking()
        {
            var songs = await _songRepository.GetAllAsync();
            if (songs.Count() < 10)
            {
                return NotFound();
            }
            var listeningRantings = await _songRepository.GetTOPAsync("ListeningRantings", 10);
            if (listeningRantings == null)
            {
                return NotFound();
            }
            var categorys = await _categoryRepository.GetAllAsync();
            var songAndCategory = new Models.DTO.SongAndCateroryList()
            {
                ListeningRantings = _mapper.Map<List<Models.DTO.Song>>(listeningRantings),
                CategoryList = _mapper.Map<List<Models.DTO.Category>>(categorys)
            };
            return View(songAndCategory);
        }
        public async Task<IActionResult> SongList(int? id, int page = 1)
        {
            var songs = await _songRepository.GetAllAsync();
            if (songs.Count() < 10)
            {
                return NotFound();
            }
            if (id == null)
            {
                page = page < 1 ? 1 : page;
                var songList = await _songRepository.GetPagingAsync(page, size);
                if (songList == null)
                {
                    return NotFound();
                }
                var newSong = await _songRepository.GetTOPAsync("NewSong", 10);
                var listeningRantings = await _songRepository.GetTOPAsync("ListeningRantings", 10);
                var downloadRantings = await _songRepository.GetTOPAsync("DownloadRantings", 10);
                if (newSong == null || listeningRantings == null || downloadRantings == null)
                {
                    return NotFound();
                }
                var categorys = await _categoryRepository.GetAllAsync();
                var songAndCategoryList = new Models.DTO.SongAndCategoryPagination()
                {
                    NewSong = _mapper.Map<List<Models.DTO.Song>>(newSong),
                    SongList = new StaticPagedList<Models.DTO.Song>(
                        songList.Select(song => _mapper.Map<Models.DTO.Song>(song)),
                        songList.GetMetaData()
                    ),
                    ListeningRantings = _mapper.Map<List<Models.DTO.Song>>(listeningRantings),
                    DownloadRantings = _mapper.Map<List<Models.DTO.Song>>(downloadRantings),
                    CategoryList = _mapper.Map<List<Models.DTO.Category>>(categorys),
                    CategoryId = id,
                    Action = "SongList"
                };
                return View(songAndCategoryList);
            }
            else
            {
                page = page < 1 ? 1 : page;
                var songList = await _song_CategoryRepository.GetSongByCategoryIdAsync((int)id, page, size);
                if (songList == null)
                {
                    return NotFound();
                }
                var newSong = await _songRepository.GetTOPAsync("NewSong", 10);
                var listeningRantings = await _songRepository.GetTOPAsync("ListeningRantings", 10);
                var downloadRantings = await _songRepository.GetTOPAsync("DownloadRantings", 10);
                if (newSong == null || listeningRantings == null || downloadRantings == null)
                {
                    return NotFound();
                }
                var categorys = await _categoryRepository.GetAllAsync();
                var songAndCategoryList = new Models.DTO.SongAndCategoryPagination()
                {
                    NewSong = _mapper.Map<List<Models.DTO.Song>>(newSong),
                    SongList = new StaticPagedList<Models.DTO.Song>(
                        songList.Select(song => _mapper.Map<Models.DTO.Song>(song)),
                        songList.GetMetaData()
                    ),
                    ListeningRantings = _mapper.Map<List<Models.DTO.Song>>(listeningRantings),
                    DownloadRantings = _mapper.Map<List<Models.DTO.Song>>(downloadRantings),
                    CategoryList = _mapper.Map<List<Models.DTO.Category>>(categorys),
                    CategoryId = id,
                    Action = "SongList"
                };
                return View(songAndCategoryList);
            }
        }
        public async Task<IActionResult> SingerList(char? key, int page = 1)
        {
            if (key == null)
            {
                page = page < 1 ? 1 : page;
                var singerList = await _singerRepository.GetPagingAsync(page, sizeSinger);
                if (singerList == null)
                {
                    return View(null);
                }
                return View(singerList);
            }
            else
            {
                ViewBag.key = key;
                page = page < 1 ? 1 : page;
                var singerList = await _singerRepository.GetSingerByKeyPagingAsync((char)key, page, sizeSinger);
                if (singerList == null)
                {
                    return View(null);
                }
                return View(singerList);
            }
        }
        public async Task<IActionResult> SongDetailBySinger(int singerId)
        {
            var songBySinger = await _song_SingerRepository.GetBySingerIdAsync(singerId);
            if (songBySinger == null)
            {
                return NotFound();
            }
            var song = songBySinger.FirstOrDefault();
            var songList = await _songRepository.GetTOPAsync("SongList", 5);
            if (songList == null)
            {
                return NotFound();
            }
            var data = new Models.DTO.DataDetailBySinger()
            {
                Song_Singer = _mapper.Map<Models.DTO.Song_Singer>(song),
                SongList = _mapper.Map<List<Models.DTO.Song>>(songList),
                SongListBySinger = _mapper.Map<List<Models.DTO.Song_Singer>>(songBySinger)
            };
            return View(data);
        }
        public async Task<IActionResult> Search(string key, int page = 1)
        {
            page = page < 1 ? 1 : page;
            var songs = await _songRepository.SearchAsync(key, page, size);
            var categorys = await _categoryRepository.GetAllAsync();
            var viewModel = new Models.DTO.SongAndCategoryPagination();
            if (songs != null)
            {
                viewModel.SongList = new StaticPagedList<Models.DTO.Song>(
                        songs.Select(song => _mapper.Map<Models.DTO.Song>(song)),
                        songs.GetMetaData()
                );
            }
            viewModel.CategoryList = _mapper.Map<List<Models.DTO.Category>>(categorys);
            viewModel.Key = key;
            viewModel.Action = "Search";
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ItemCategory()
        {
            var categorys = await _categoryRepository.GetAllAsync();
            var categorysDTO = _mapper.Map<List<Models.DTO.NavHomeCategory>>(categorys);
            return Json(categorysDTO);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateListenCount(int songId)
        {
            var song = await _songRepository.GetByIdAsync(songId);
            if (song == null)
            {
                return NotFound();
            }
            await _songRepository.UpdateNumberOfListensAsync(song);
            if (song == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDownloadCount(int songId)
        {
            var song = await _songRepository.GetByIdAsync(songId);
            if (song == null)
            {
                return NotFound();
            }
            await _songRepository.UpdateNumberOfDownloadsAsync(song);
            if (song == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok();
        }
        public async Task<IActionResult> AutoSearch(string key)
        {
            var songs = await _songRepository.AutoSearchAsync(key);
            var data = _mapper.Map<List<Models.DTO.AutoSearch>>(songs);
            return Json(data);
        }
    }
}
