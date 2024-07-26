using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.AccountRepositories;
using MusicTool.DataAccess.Repositories.FileUploadRepositories;
using MusicTool.DataAccess.Repositories.VipRepositories;
using MusicTool.DataAccess.Services;
using MusicTool.Infrastructure;
using MusicTool.Models.DTO;

namespace MusicTool.Controllers
{
    public class UserController : Controller
    {
        private readonly IVipRepository _vipRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly HashSet<string> _imgMimeTypes = new HashSet<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };
        public UserController(IVipRepository vipRepository, IVnPayService vnPayService, IFileUploadRepository fileUploadRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            _vipRepository = vipRepository;
            _vnPayService = vnPayService;
            _fileUploadRepository = fileUploadRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var user = HttpContext.Session.GetObject<Models.DTO.LoginRequest>("User");
            if (id == null)
            {
                id = user.Id;
            }
            var account = await _accountRepository.GetByIdAsync((int)id);
            if (account == null)
            {
                return NotFound();
            }
            var accountCurrent = new UserAccount()
            {
                Id = account.Id,
                Email = account.Email,
                UserType = account.UserType,
                Password = account.Password,
                PhotoName = account.PhotoName
            };
            HttpContext.Session.SetObject("accountCurrent", accountCurrent);
            var viewModel = new EditAccountRequest()
            {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email,
                Name = account.Name,
                PhotoName = account.PhotoName,
                Date = account.VIPs.Max(x => x.VIPExpirationDate)
            };
            return View(viewModel);
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> UserSubmit(EditAccountRequest editAccountRequest, IFormFile? PhotoName)
        {
            var accountCurent = HttpContext.Session.GetObject<UserAccount>("accountCurrent");
            editAccountRequest.PhotoName = accountCurent.PhotoName;
            if (editAccountRequest.Email != null)
            {
                if (accountCurent.Email != editAccountRequest.Email)
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
                if (PhotoName != null)
                {
                    var filePathPT = await _fileUploadRepository.UploadFileAsync(PhotoName, "wwwroot/images/Accounts");
                    editAccountRequest.PhotoName = filePathPT.FileName;
                }
                else
                {
                    editAccountRequest.PhotoName = null;
                }
                var account = new Models.Domain.Account()
                {
                    Id = editAccountRequest.Id,
                    Name = editAccountRequest.Name,
                    UserName = editAccountRequest.UserName,
                    Password = editAccountRequest.Password,
                    Email = editAccountRequest.Email,
                    Type = false,
                    UserType = accountCurent.UserType,
                    PhotoName = editAccountRequest.PhotoName
                };
                await _accountRepository.UpdateUserAsync(account);
                TempData["success"] = "Cập nhật thành công";
                HttpContext.Session.Remove("accountCurrent");
                return RedirectToAction("Edit", new { id = account.Id });
            }
            return View(editAccountRequest);
        }
        public async Task<IActionResult> VIP(int price, int accountId, string type)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null)
                {
                    return NotFound();
                }
                var vip = new Models.DTO.AddVIPRequest()
                {
                    Type = type,
                    AccountId = account.Id,
                    Price = price
                };
                HttpContext.Session.SetObject("VIP", vip);
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = vip.Price,
                    CreatedDate = DateTime.Now,
                    OrderId = new Random().Next(1000, 10000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            TempData["error"] = "Thanh toán lỗi ";
            return RedirectToAction("Edit", new { id = accountId });
        }
        public async Task<IActionResult> PaymentCallBack()
        {
            var accountVIP = HttpContext.Session.GetObject<Models.DTO.AddVIPRequest>("VIP");
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["error"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("Edit", new { id = accountVIP.AccountId });
            }
            var vip = new Models.Domain.VIP()
            {
                VIPRegistrationDate = response.PayDate,
                Price = (response.Amount) / 100,
                AccountId = accountVIP.AccountId,
                Status = true
            };
            var account = await _accountRepository.GetByIdAsync(accountVIP.AccountId);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                await _accountRepository.UpdateUserTypeAsync(account.Id);
                if (account.VIPs.Count() > 0)
                {
                    if (accountVIP.Type == "year")
                    {
                        vip.VIPExpirationDate = account.VIPs.Max(x => x.VIPExpirationDate).Value.AddYears(1);
                    }
                    if (accountVIP.Type == "month")
                    {
                        vip.VIPExpirationDate = account.VIPs.Max(x => x.VIPExpirationDate).Value.AddMonths(1);
                    }
                }
                else
                {
                    if (accountVIP.Type == "year")
                    {
                        vip.VIPExpirationDate = response.PayDate.AddYears(1);
                    }
                    if (accountVIP.Type == "month")
                    {
                        vip.VIPExpirationDate = response.PayDate.AddMonths(1);
                    }
                }
            }
            await _vipRepository.AddAsync(vip);
            TempData["success"] = $"Thanh toán thành công";
            return RedirectToAction("Edit", new { id = accountVIP.AccountId });
        }
    }
}
