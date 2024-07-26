using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.AccountRepositories;
using MusicTool.Infrastructure;
using MusicTool.Models.DTO;

namespace MusicTool.Controllers
{
    public class LoginSignupController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public LoginSignupController(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public IActionResult AccountName()
        {
            var user = HttpContext.Session.GetObject<Models.DTO.LoginRequest>("User");
            return Json(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> SubmitLogin(AddLoginRequest addLoginRequest)
        {
            if (ModelState.IsValid)
            {
                var accountInput = new Models.Domain.Account()
                {
                    UserName = addLoginRequest.UserName,
                    Password = addLoginRequest.Password
                };
                var accountOutput = await _accountRepository.GetByUserNameAsync(accountInput);
                if (accountOutput == null)
                {
                    ViewBag.ErrAccount = "Sai tài khoản hoặc mật khẩu";
                    return View(addLoginRequest);
                }
                if (accountOutput.Type)
                {
                    var user = new MusicTool.Models.DTO.LoginRequest()
                    {
                        Id = accountOutput.Id,
                        PhotoName = accountOutput.PhotoName,
                        Type = accountOutput.Type
                    };
                    HttpContext.Session.SetObject("User", user);
                    return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                }
                else
                {
                    var user = new MusicTool.Models.DTO.LoginRequest()
                    {
                        Id = accountOutput.Id,
                        PhotoName = accountOutput.PhotoName,
                        Type = accountOutput.Type,
                        UserType = accountOutput.UserType
                    };
                    HttpContext.Session.SetObject("User", user);
                    return RedirectToAction("Index", "MTHome");
                }
            }
            return View(addLoginRequest);
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Signup")]
        public async Task<IActionResult> SubmitSignup(AddAccountRequest addAccountRequest)
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
            if (ModelState.IsValid)
            {
                var account = new Models.Domain.Account()
                {
                    UserName = addAccountRequest.UserName,
                    Email = addAccountRequest.Email,
                    Password = addAccountRequest.Password,
                    Name = addAccountRequest.Name,
                    Type = false,
                    UserType = "normal",
                    Status = true,
                    CreatedDate = DateTime.Now
                };
                await _accountRepository.AddAsync(account);
                TempData["success"] = "Đăng thành công";
                return RedirectToAction("Login");
            }
            return View(addAccountRequest);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "MTHome");
        }
    }
}
