using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinesLogic.UnitOfWork;
using Commons.Others;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Models.Accounting;

namespace ERP_SPARTAN.Controllers
{
    [Authorize(Roles = ControllersRol.EnterPrise)]
    public class EnterpriseController : BaseController
    {
        private readonly IUnitOfWork _services;
        public EnterpriseController(IUnitOfWork services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return View(await _services.EnterpriseService.GetList(userId));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Enterprise model)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            model.UserId = userId;
            if (!ModelState.IsValid) return View(model);
            var result = await _services.EnterpriseService.Add(model);
            if (!result)
            {
                BasicNotification("Ocurrio un error intente de nuevo", NotificationType.error, "Lo sentimos");
                return View(model);
            }
            BasicNotification("Agregado correctamente", NotificationType.success, "Exito");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _services.EnterpriseService.GetById(id);
            if (result == null) return new NotFoundView();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Enterprise model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _services.EnterpriseService.Update(model);
            if (!result)
            {
                BasicNotification("Ocurrio un error intente de nuevo", NotificationType.error, "Lo sentimos intente de nuevo");
                return View(model);
            }
            BasicNotification("Actualizado", NotificationType.success, "Exito");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var enterprise = await _services.EnterpriseService.GetById(id);
            if (enterprise == null) return new NotFoundView();
            return View(enterprise);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var enterprise = await _services.EnterpriseService.GetById(id);
            if (enterprise == null) return new NotFoundView();
            var result = await _services.EnterpriseService.Remove(enterprise);
            if (!result)
            {
                BasicNotification("Lo sentimos, intente de nuevo", NotificationType.error, "Lo sentimos");
                return View(nameof(Remove), enterprise);
            }
            BasicNotification("Eliminado con exito", NotificationType.success, "Eliminado");
            return RedirectToAction(nameof(Index));
        }
    }
}