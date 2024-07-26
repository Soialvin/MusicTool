using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.CategoryRepositories;
using MusicTool.DataAccess.Repositories.FileUploadRepositories;
using MusicTool.Models.DTO;
using X.PagedList;

namespace MusicTool.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileUploadRepository _fileUploadRepository;

        private readonly HashSet<string> _imgMimeTypes = new HashSet<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };

        public int size = 10;

        public CategoryController(IMapper mapper, ICategoryRepository categoryRepository, IFileUploadRepository fileUploadRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _fileUploadRepository = fileUploadRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            page = page < 1 ? 1 : page;
            var categorys = await _categoryRepository.GetPagingAsync(page, size);
            var data = new Models.DTO.IndexCategoryRequest()
            {
                ActionName = "Index"
            };
            if (categorys != null)
            {
                data.List = new StaticPagedList<Models.DTO.Category>(
                        categorys.Select(category => _mapper.Map<Models.DTO.Category>(category)),
                        categorys.GetMetaData()
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
        public async Task<IActionResult> AddSubmitCategory(AddCategoryRequest addCategoryRequest, IFormFile? PhotoName)
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
                var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/TypesImg");
                addCategoryRequest.PhotoName = filePathPT.FileName;
                var category = new Models.Domain.Category()
                {
                    Name = addCategoryRequest.Name,
                    PhotoName = addCategoryRequest.PhotoName,
                    CreatedDate = DateTime.Now,
                    Status = true
                };
                await _categoryRepository.AddAsync(category);
                TempData["success"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            return View(addCategoryRequest);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                var viewModel = new EditCategoryRequest()
                {
                    Id = category.Id,
                    Name = category.Name,
                    PhotoName = category.PhotoName
                };
                return View(viewModel);
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditSubmitCategory(EditCategoryRequest editCategoryRequest, IFormFile? PhotoName)
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
                    var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/TypesImg");
                    editCategoryRequest.PhotoName = filePathPT.FileName;
                }
                var category = new Models.Domain.Category()
                {
                    Id = editCategoryRequest.Id,
                    Name = editCategoryRequest.Name,
                    PhotoName = editCategoryRequest.PhotoName
                };
                await _categoryRepository.UpdateAsync(category);
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(editCategoryRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int txt_id)
        {
            var category = await _categoryRepository.DeleteAsync(txt_id);
            if (category != null)
            {
                return RedirectToAction("Index");
            }
            TempData["success"] = "Đã xóa thành công";
            return NotFound();
        }
        public async Task<IActionResult> Search(string key, int page = 1)
        {
            page = page < 1 ? 1 : page;
            var categorys = await _categoryRepository.SearchAsync(key, page, size);
            var data = new Models.DTO.IndexCategoryRequest()
            {
                Key = key,
                ActionName = "Search"
            };
            if (categorys != null)
            {
                data.List = new StaticPagedList<Models.DTO.Category>(
                        categorys.Select(category => _mapper.Map<Models.DTO.Category>(category)),
                        categorys.GetMetaData()
                    );
            }
            return View("Index", data);
        }
    }
}
