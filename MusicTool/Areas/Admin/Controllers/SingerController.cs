using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.FileUploadRepositories;
using MusicTool.DataAccess.Repositories.SingerRepositories;
using MusicTool.Models.DTO;
using X.PagedList;

namespace MusicTool.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SingerController : Controller
    {
        private readonly ISingerRepository _singerRepository;
        private readonly IMapper _mapper;
        private readonly IFileUploadRepository _fileUploadRepository;

        private readonly HashSet<string> _imgMimeTypes = new HashSet<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };

        public int size = 5;
        public SingerController(IMapper mapper, IFileUploadRepository fileUploadRepository, ISingerRepository singerRepository)
        {
            _mapper = mapper;
            _fileUploadRepository = fileUploadRepository;
            _singerRepository = singerRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            var singers = await _singerRepository.GetPagingAsync(page, size);
            var data = new Models.DTO.IndexSingerRequest()
            {
                ActionName = "Index"
            };
            if (singers != null)
            {
                data.List = new StaticPagedList<Models.DTO.Singer>(
                        singers.Select(singer => _mapper.Map<Models.DTO.Singer>(singer)),
                        singers.GetMetaData()
                    );
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> AddSubmitSinger(AddSingerRequest addSingerRequest, IFormFile? PhotoName)
        {
            if (PhotoName == null)
            {
                ModelState.AddModelError("PhotoName", "hình ảnh không được để trống");
            }
            else
            {
                if (!_imgMimeTypes.Contains(PhotoName.ContentType))
                {
                    ModelState.AddModelError("PhotoName", "Định dạng ảnh không hợp lệ");
                }
            }
            if (ModelState.IsValid)
            {
                var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Singers");
                addSingerRequest.PhotoName = filePathPT.FileName;
                var singer = new Models.Domain.Singer()
                {
                    Name = addSingerRequest.Name,
                    PhotoName = addSingerRequest.PhotoName,
                    LoadingDate = DateTime.Now,
                    Status = true
                };
                await _singerRepository.AddAsync(singer);
                TempData["success"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            return View(addSingerRequest);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var singer = await _singerRepository.GetByIdAsync(id);
            if (singer != null)
            {
                var editSingerRequest = new EditSingerRequest()
                {
                    Id = singer.Id,
                    Name = singer.Name,
                    PhotoName = singer.PhotoName
                };
                return View(editSingerRequest);
            }

            return NotFound();
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditSubmitSinger(EditSingerRequest editSingerRequest, IFormFile? PhotoName)
        {
            if (PhotoName != null)
            {
                if (!_imgMimeTypes.Contains(PhotoName.ContentType))
                {
                    ModelState.AddModelError("PhotoName", "Định dạng ảnh không hợp lệ");
                }
            }
            if (ModelState.IsValid)
            {
                if (PhotoName != null)
                {
                    var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Singers");
                    editSingerRequest.PhotoName = filePathPT.FileName;
                }
                var singer = new Models.Domain.Singer()
                {
                    Id = editSingerRequest.Id,
                    Name = editSingerRequest.Name,
                    PhotoName = editSingerRequest.PhotoName
                };
                await _singerRepository.UpdateAsync(singer);
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(editSingerRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int txt_id)
        {
            var singer = await _singerRepository.DeleteAsync(txt_id);
            if (singer == null)
            {
                return NotFound();
            }
            TempData["success"] = "Đã xóa thành công";
            return RedirectToAction("Index", "Singer");
        }
        public async Task<IActionResult> Search(string key, int page = 1)
        {
            ViewBag.Key = key;
            page = page < 1 ? 1 : page;
            var singers = await _singerRepository.SearchAsync(key, page, size);
            var data = new Models.DTO.IndexSingerRequest()
            {
                Key = key,
                ActionName = "Search"
            };
            if (singers != null)
            {
                data.List = new StaticPagedList<Models.DTO.Singer>(
                        singers.Select(singer => _mapper.Map<Models.DTO.Singer>(singer)),
                        singers.GetMetaData()
                    );
            }
            return View("Index", data);
        }
    }
}
