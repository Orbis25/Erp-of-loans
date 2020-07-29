using BusinesLogic.UnitOfWork;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Models.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_SPARTAN.Controllers
{
    [Authorize]
    public class AlertController : BaseController
    {
        private readonly IUnitOfWork _services;
        public AlertController(IUnitOfWork service) => _services = service;

        [Authorize(Roles = nameof(RolsAuthorization.Admin))]
        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await _services.AlertService.GetAllWithRelationShips());

        [HttpGet]
        public async Task<IActionResult> MyAlerts()
        {
            var result = _services.AlertService.Filter(x => x.UserId == GetUserLoggedId());
            return View(await result.OrderByDescending(x => x.CreateAt).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Clients = await _services.ClientUserService.GetListItem(x => x.CreatedBy == GetUserLoggedId());
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Quantity() => Ok(await _services.AlertService.Quantity(GetUserLoggedId()));

        [HttpPost]
        public async Task<IActionResult> Create(Alert model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = false;
            if (model.IsMasive) result = await _services.AlertService.SendMasive(model, GetUserLoggedId());
            else result = await _services.AlertService.Add(model);
            if (!result)
            {
                BasicNotification("Intente de nuevo", NotificationType.error, "Error");
                return View(model);
            }
            BasicNotification("Alerta Enviada", NotificationType.success, "Exito");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MaskAsRead()
        {
            var result = await _services.AlertService.MaskAsRead(GetUserLoggedId());
            if (!result)
            {
                BasicNotification("Error intente de nuevo", NotificationType.error);
            }
            return RedirectToAction(nameof(MyAlerts));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var alert = await _services.AlertService.GetById(id);
            if (alert == null) return NotFound();
            if (await _services.AlertService.Remove(alert)) return Ok(true);
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            var result = await _services.AlertService.RemoveAll();
            if (!result) BasicNotification("Error intente de nuevo", NotificationType.error);
            BasicNotification("Acción realizada correctamente", NotificationType.success);
            return RedirectToAction(nameof(Index));
        }
    }
}