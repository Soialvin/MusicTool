using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.AccountRepositories;
using MusicTool.DataAccess.Repositories.FileUploadRepositories;
using MusicTool.Models.DTO;
using X.PagedList;

namespace MusicTool.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public int size = 10;

        private readonly HashSet<string> _imgMimeTypes = new HashSet<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };

        public AccountController(IFileUploadRepository fileUploadRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            _fileUploadRepository = fileUploadRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            var accounts = await _accountRepository.GetPagingAsync(page, size);
            var data = new Models.DTO.IndexAccountRequest()
            {
                ActionName = "Index"
            };
            if (accounts != null)
            {
                data.List = new StaticPagedList<Models.DTO.Account>(
                        accounts.Select(account => _mapper.Map<Models.DTO.Account>(account)),
                        accounts.GetMetaData()
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
        public async Task<IActionResult> AddSubmitAccount(AddAccountRequest addAccountRequest, IFormFile? PhotoName)
        {
            if (addAccountRequest.UserName != null)
            {
                var checkUserName = await _accountRepository.ExistingAsync(addAccountRequest.UserName, "UserName");
                if ((bool)checkUserName)
                {
                    ModelState.AddModelError("UserName", "Tên tài khoản đã tồn tại");
                }
            }
            if (addAccountRequest.Email != null)
            {
                var checkEmail = await _accountRepository.ExistingAsync(addAccountRequest.Email, "Email");
                if ((bool)checkEmail)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng");
                }
            }
            if (PhotoName == null)
            {
                ModelState.AddModelError("PhotoName", "Hình ảnh không được để trống");
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
                var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Accounts");
                addAccountRequest.PhotoName = filePathPT.FileName;
                var account = new Models.Domain.Account()
                {
                    UserName = addAccountRequest.UserName,
                    Email = addAccountRequest.Email,
                    Password = addAccountRequest.Password,
                    Name = addAccountRequest.Name,
                    Type = addAccountRequest.Type,
                    Status = true,
                    PhotoName = addAccountRequest.PhotoName,
                    CreatedDate = DateTime.Now
                };
                await _accountRepository.AddAsync(account);
                TempData["success"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            return View(addAccountRequest);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("EmailCurrent", account.Email);
            var viewModel = new EditAccountRequest()
            {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email,
                Name = account.Name,
                Type = account.Type,
                PhotoName = account.PhotoName
            };
            return View(viewModel);
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditSubmitAccount(EditAccountRequest editAccountRequest, IFormFile? PhotoName)
        {
            if (editAccountRequest.Email != null)
            {
                var emailCurrent = HttpContext.Session.GetString("EmailCurrent");
                if (emailCurrent != editAccountRequest.Email)
                {
                    var checkEmail = await _accountRepository.ExistingAsync(editAccountRequest.Email, "Email");
                    if ((bool)checkEmail)
                    {
                        ModelState.AddModelError("Email", "Email đã được sử dụng");
                    }
                }
            }
            if (PhotoName != null)
            {
                if (!_imgMimeTypes.Contains(PhotoName.ContentType))
                {
                    ModelState.AddModelError("PhotoName", "Định dạng ảnh không hợp lệ");
                }
            }
            if (ModelState.IsValid)
            {
                HttpContext.Session.Remove("EmailCurrent");
                if (PhotoName != null)
                {
                    var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Accounts");
                    editAccountRequest.PhotoName = filePathPT.FileName;
                }
                var account = new Models.Domain.Account()
                {
                    Id = editAccountRequest.Id,
                    Name = editAccountRequest.Name,
                    UserName = editAccountRequest.UserName,
                    Email = editAccountRequest.Email,
                    Type = editAccountRequest.Type,
                    PhotoName = editAccountRequest.PhotoName
                };
                await _accountRepository.UpdateAsync(account);
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(editAccountRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int txt_id)
        {
            var account = await _accountRepository.DeleteAsync(txt_id);
            if (account == null)
            {
                return NotFound();
            }
            TempData["success"] = "Đã xóa thành công";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Search(string key, int page = 1)
        {
            ViewBag.Key = key;
            page = page < 1 ? 1 : page;
            var accounts = await _accountRepository.SearchAsync(key, page, size);
            var data = new Models.DTO.IndexAccountRequest()
            {
                Key = key,
                ActionName = "Search"
            };
            if (accounts != null)
            {
                data.List = new StaticPagedList<Models.DTO.Account>(
                        accounts.Select(account => _mapper.Map<Models.DTO.Account>(account)),
                        accounts.GetMetaData()
                    );
            }
            return View("Index", data);
        }
    }
}
