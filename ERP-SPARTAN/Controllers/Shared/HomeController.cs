using BusinesLogic.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP_SPARTAN.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _service;
        public HomeController(IUnitOfWork service) => _service = service;
        public async Task<IActionResult> Index()
        {
            var user = await _service.UserService.GetByEmailAsync(User.Identity.Name);
            ViewBag.UserFullName = user.FullName;
            return View(await _service.HomeService.Get(user.Id));
        }

    }
}
