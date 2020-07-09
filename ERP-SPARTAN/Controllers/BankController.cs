using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinesLogic.UnitOfWork;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models.HiLoans;

namespace ERP_SPARTAN.Controllers
{
    [Authorize(Roles = nameof(RolsAuthorization.Admin))]
    public class BankController : BaseController
    {
        private readonly IUnitOfWork _services;
        public BankController(IUnitOfWork services) => _services = services;
       
        public async Task<IActionResult> Index() => View(await _services.BankService.GetList());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Bank model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _services.BankService.Add(model);
            if (!result)
            {
                BasicNotification("No se ha podido crear, intente de nuevo", NotificationType.error);
            }
            else BasicNotification("Banco creado!", NotificationType.success);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id) => View(await _services.BankService.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Remove(Bank model)
        {
            await _services.BankService.Remove(model);
            BasicNotification("Banco Eliminado!", NotificationType.success);
            return RedirectToAction(nameof(Index));
        }
    }
}