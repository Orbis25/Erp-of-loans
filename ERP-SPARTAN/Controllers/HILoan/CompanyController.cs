using BusinesLogic.UnitOfWork;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models.HiLoans;
using System.Threading.Tasks;

namespace ERP_SPARTAN.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        private readonly IUnitOfWork _services;
        public CompanyController(IUnitOfWork services) => _services = services;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!await _services.CompanyService.HaveCompany(GetUserLoggedId()))
            {
                await _services.CompanyService.Add(new Company { UserId = GetUserLoggedId() });
            }
            return View(await _services.CompanyService.GetCompanyByUserId(GetUserLoggedId()));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Company model)
        {
            var result = await _services.CompanyService.Update(model);
            if (!result) BasicNotification("Lo sentimos no se pudo actualizar, intente de nuevo mas tarde", NotificationType.error);
            BasicNotification("Actualizada", NotificationType.success);
            return View(nameof(Index));
        }
    }
}