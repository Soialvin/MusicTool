using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.CategoryRepositories;
using MusicTool.DataAccess.Repositories.FileUploadRepositories;
using MusicTool.DataAccess.Repositories.SingerRepositories;
using MusicTool.DataAccess.Repositories.Song_CategoryRepositories;
using MusicTool.DataAccess.Repositories.Song_SingerRepositories;
using MusicTool.DataAccess.Repositories.SongRepositories;
using MusicTool.Infrastructure;
using MusicTool.Models.DTO;
using X.PagedList;

namespace MusicTool.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SongController : Controller
    {
        private readonly ISongRepository _songRepository;
        private readonly ISingerRepository _singerRepository;
        private readonly ISong_CategoryRepository _song_CategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly ISong_SingerRepository _song_SingerRepository;

        private readonly HashSet<string> _imgMimeTypes = new HashSet<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };
        private readonly HashSet<string> _AudioMimeTypes = new HashSet<string>
        {
            "audio/mpeg",
            "audio/wav",
            "audio/ogg",
            "audio/flac",
            "audio/aac",
            "audio/webm"
        };

        public int size = 10;

        public SongController(ISong_CategoryRepository song_CategoryRepository, ICategoryRepository categoryRepository, ISongRepository songRepository, ISingerRepository singerRepository, IMapper mapper, IFileUploadRepository fileUploadRepository, ISong_SingerRepository song_SingerRepository)
        {
            _song_CategoryRepository = song_CategoryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _fileUploadRepository = fileUploadRepository;
            _songRepository = songRepository;
            _singerRepository = singerRepository;
            _song_SingerRepository = song_SingerRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            var songs = await _songRepository.GetPagingAsync(page, size);
            var data = new Models.DTO.IndexSongRequest()
            {
                ActionName = "Index"
            };
            if (songs != null)
            {
                data.List = new StaticPagedList<Models.DTO.Song>(
                            songs.Select(song => _mapper.Map<Models.DTO.Song>(song)),
                            songs.GetMetaData()
                        );
            }
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var singers = await _singerRepository.GetAllAsync();
            var singerDTO = _mapper.Map<List<Models.DTO.Singer>>(singers);
            var categorys = await _categoryRepository.GetAllAsync();
            var categoryDTO = _mapper.Map<List<Models.DTO.Category>>(categorys);
            var viewModel = new SongAndSingerAndCategoryAdd()
            {
                categoryList = categoryDTO,
                singerList = singerDTO
            };
            return View(viewModel);
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> AddSubmitSong(SongAndSingerAndCategoryAdd songAndSingerAndCategoryAdd, IFormFile? PhotoName, IFormFile? SongFile)
        {
            var singer = await _singerRepository.GetAllAsync();
            var singerDTO = _mapper.Map<List<Models.DTO.Singer>>(singer);
            var categorys = await _categoryRepository.GetAllAsync();
            var categoryDTO = _mapper.Map<List<Models.DTO.Category>>(categorys);
            songAndSingerAndCategoryAdd.singerList = singerDTO;
            songAndSingerAndCategoryAdd.categoryList = categoryDTO;
            if (PhotoName != null)
            {
                if (!_imgMimeTypes.Contains(PhotoName.ContentType))
                {
                    ModelState.AddModelError("addSongRequest.PhotoName", "Định dạng ảnh không hợp lệ");
                }
            }
            else
            {
                ModelState.AddModelError("addSongRequest.PhotoName", "Hình ảnh không được để trống");
            }
            if (SongFile != null)
            {
                if (!_AudioMimeTypes.Contains(SongFile.ContentType))
                {
                    ModelState.AddModelError("addSongRequest.SongFile", "Định dạng nhạc không hợp lệ");
                }
            }
            else
            {
                ModelState.AddModelError("addSongRequest.SongFile", "Nhạc không được bỏ trống");
            }
            if (ModelState.IsValid)
            {
                var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Songs");
                var filePathS = await _fileUploadRepository.UploadFileAsync(SongFile, "wwwroot/Songs");
                songAndSingerAndCategoryAdd.addSongRequest.PhotoName = filePathPT.FileName;
                songAndSingerAndCategoryAdd.addSongRequest.SongFile = filePathS.FileName;
                var user = HttpContext.Session.GetObject<Models.DTO.LoginRequest>("User");
                var song = new Models.Domain.Song()
                {
                    Name = songAndSingerAndCategoryAdd.addSongRequest.Name,
                    AccountId = user.Id,
                    Lyrics = songAndSingerAndCategoryAdd.addSongRequest.Lyrics,
                    PhotoName = songAndSingerAndCategoryAdd.addSongRequest.PhotoName,
                    SongFile = songAndSingerAndCategoryAdd.addSongRequest.SongFile,
                    NumberOfDownloads = 0,
                    NumberOfListens = 0,
                    LoadingDate = DateTime.Now,
                    Status = true
                };
                await _songRepository.AddAsync(song);
                if (songAndSingerAndCategoryAdd.addSongRequest.SingerId != null && songAndSingerAndCategoryAdd.addSongRequest.SingerId.Count() != 0)
                {
                    foreach (var item in songAndSingerAndCategoryAdd.addSongRequest.SingerId)
                    {
                        var song_Singer = new Models.Domain.Song_Singer()
                        {
                            SongId = song.Id,
                            SingerId = item
                        };
                        await _song_SingerRepository.AddAsync(song_Singer);
                    }
                }
                if (songAndSingerAndCategoryAdd.addSongRequest.CategoryId != null && songAndSingerAndCategoryAdd.addSongRequest.CategoryId.Count() != 0)
                {
                    foreach (var item in songAndSingerAndCategoryAdd.addSongRequest.CategoryId)
                    {
                        var song_Category = new Models.Domain.Song_Category()
                        {
                            SongId = song.Id,
                            CategoryId = item
                        };
                        await _song_CategoryRepository.AddAsync(song_Category);
                    }
                }
                TempData["success"] = "Thêm mới thành công";
                return RedirectToAction("Index");

            }
            return View(songAndSingerAndCategoryAdd);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var singer = await _singerRepository.GetAllAsync();
            var singerDTO = _mapper.Map<List<Models.DTO.Singer>>(singer);
            var categorys = await _categoryRepository.GetAllAsync();
            var categoryDTO = _mapper.Map<List<Models.DTO.Category>>(categorys);
            var viewModel = new SongAndSingerAndCategoryEdit()
            {
                categoryList = categoryDTO,
                singerList = singerDTO
            };
            var song = await _songRepository.GetByIdAsync(id);
            if (song != null)
            {
                viewModel.editSongRequest = new EditSongRequest()
                {
                    Id = song.Id,
                    Name = song.Name,
                    Lyrics = song.Lyrics,
                    PhotoName = song.PhotoName,
                    SongFile = song.SongFile,
                    SingerId = song.Song_Singer.Select(x => x.SingerId).Where(id => id.HasValue).Select(id => id.Value).ToList(),
                    CategoryId = song.Song_Category.Select(x => x.CategoryId).Where(id => id.HasValue).Select(id => id.Value).ToList(),
                };
                return View(viewModel);
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditSubmitSong(SongAndSingerAndCategoryEdit songAndSingerAndCategoryEdit, IFormFile? PhotoName, IFormFile? SongFile)
        {
            var singer = await _singerRepository.GetAllAsync();
            var singerDTO = _mapper.Map<List<Models.DTO.Singer>>(singer);
            var categorys = await _categoryRepository.GetAllAsync();
            var categoryDTO = _mapper.Map<List<Models.DTO.Category>>(categorys);
            songAndSingerAndCategoryEdit.singerList = singerDTO;
            songAndSingerAndCategoryEdit.categoryList = categoryDTO;
            if (PhotoName != null)
            {
                if (!_imgMimeTypes.Contains(PhotoName.ContentType))
                {
                    ModelState.AddModelError("editSongRequest.PhotoName", "Định dạng ảnh không hợp lệ");
                }
            }
            if (SongFile != null)
            {
                if (!_AudioMimeTypes.Contains(SongFile.ContentType))
                {
                    ModelState.AddModelError("editSongRequest.SongFile", "Định dạng nhạc không hợp lệ");
                }
            }
            if (ModelState.IsValid)
            {
                if (PhotoName != null)
                {
                    var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Songs");
                    songAndSingerAndCategoryEdit.editSongRequest.PhotoName = filePathPT.FileName;
                }
                if (SongFile != null)
                {
                    var filePathS = await _fileUploadRepository.UploadFileAsync(SongFile, "wwwroot/Songs");
                    songAndSingerAndCategoryEdit.editSongRequest.SongFile = filePathS.FileName;
                }
                var song = new Models.Domain.Song()
                {
                    Id = songAndSingerAndCategoryEdit.editSongRequest.Id,
                    Name = songAndSingerAndCategoryEdit.editSongRequest.Name,
                    Lyrics = songAndSingerAndCategoryEdit.editSongRequest.Lyrics,
                    PhotoName = songAndSingerAndCategoryEdit.editSongRequest.PhotoName,
                    SongFile = songAndSingerAndCategoryEdit.editSongRequest.SongFile,
                };
                await _songRepository.UpdateAsync(song);
                var song_SingerId = await _song_SingerRepository.GetByIdAsync(songAndSingerAndCategoryEdit.editSongRequest.Id);
                songAndSingerAndCategoryEdit.editSongRequest.Song_SingerId = song_SingerId;
                if (songAndSingerAndCategoryEdit.editSongRequest.Song_SingerId != null && songAndSingerAndCategoryEdit.editSongRequest.Song_SingerId.Count() > 0)
                {
                    foreach (var item in songAndSingerAndCategoryEdit.editSongRequest.Song_SingerId)
                    {
                        await _song_SingerRepository.DeleteAsync(item);
                    }
                    if (songAndSingerAndCategoryEdit.editSongRequest.SingerId != null && songAndSingerAndCategoryEdit.editSongRequest.SingerId.Count() != 0)
                    {
                        foreach (var item in songAndSingerAndCategoryEdit.editSongRequest.SingerId)
                        {
                            var song_Singer = new Models.Domain.Song_Singer()
                            {
                                SongId = song.Id,
                                SingerId = item
                            };
                            await _song_SingerRepository.AddAsync(song_Singer);
                        }
                    }
                }
                var song_CategoryId = await _song_CategoryRepository.GetByIdAsync(songAndSingerAndCategoryEdit.editSongRequest.Id);
                songAndSingerAndCategoryEdit.editSongRequest.Song_CategoryId = song_CategoryId;
                if (songAndSingerAndCategoryEdit.editSongRequest.Song_CategoryId != null && songAndSingerAndCategoryEdit.editSongRequest.Song_CategoryId.Count() > 0)
                {
                    foreach (var item in songAndSingerAndCategoryEdit.editSongRequest.Song_CategoryId)
                    {
                        await _song_CategoryRepository.DeleteAsync(item);
                    }
                    if (songAndSingerAndCategoryEdit.editSongRequest.CategoryId != null && songAndSingerAndCategoryEdit.editSongRequest.CategoryId.Count() != 0)
                    {
                        foreach (var item in songAndSingerAndCategoryEdit.editSongRequest.CategoryId)
                        {
                            var song_Category = new Models.Domain.Song_Category()
                            {
                                SongId = song.Id,
                                CategoryId = item
                            };
                            await _song_CategoryRepository.AddAsync(song_Category);
                        }
                    }
                }
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(songAndSingerAndCategoryEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int txt_id)
        {
            var song = await _songRepository.DeleteAsync(txt_id);
            if (song == null)
            {
                return NotFound();
            }
            TempData["success"] = "Đã xóa thành công";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Search(string key, int page = 1)
        {
            page = page < 1 ? 1 : page;
            var songs = await _songRepository.SearchAsync(key, page, size);
            var data = new Models.DTO.IndexSongRequest()
            {
                Key = key,
                ActionName = "Search"
            };
            if (songs != null)
            {
                data.List = new StaticPagedList<Models.DTO.Song>(
                            songs.Select(song => _mapper.Map<Models.DTO.Song>(song)),
                            songs.GetMetaData()
                        );
            }
            return View("Index", data);
        }
    }
}
