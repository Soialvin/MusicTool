using Microsoft.AspNetCore.Mvc;
using MusicTool.DataAccess.Repositories.VipRepositories;
using MusicTool.Infrastructure;

namespace MusicTool.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        private readonly IVipRepository _vipRepository;

        public AdminHomeController(IVipRepository vipRepository)
        {
            _vipRepository = vipRepository;
        }
        public IActionResult AccountName()
        {
            var user = HttpContext.Session.GetObject<Models.DTO.LoginRequest>("User");
            return Json(user);
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Year = await _vipRepository.GetAllYearAsync();
            return View();
        }
        public async Task<IActionResult> Revenue(int? year)
        {
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var revenue = await _vipRepository.GetRevenuesAsync((int)year);
            return Json(revenue);
        }
    }
}
