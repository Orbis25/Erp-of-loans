using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinesLogic.UnitOfWork;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;

namespace ERP_SPARTAN.Controllers
{
    [Authorize(Roles = nameof(RolsAuthorization.HiInventory))]
    public class InventaryController : BaseController
    {
        private readonly IUnitOfWork _service;
        public InventaryController(IUnitOfWork service) => _service = service;
        public async Task<IActionResult> Index()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return View(await _service.InventaryService.GetByIdAll(user));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Article model)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            model.UserId = userId;
            if (ModelState.IsValid)
            {
                if (await _service.InventaryService.Add(model))
                {
                    BasicNotification("Articulo agregado", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                BasicNotification("Ocurrio un error intente de nuevo", NotificationType.error);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var model = await _service.InventaryService.GetById(id);
            if (model != null) return View(model);
            return new NotFoundView();
        }

        [HttpPost]
        public async Task<IActionResult> Update(Article model)
        {
            model.UpdateAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if(await _service.InventaryService.Update(model))
                {
                    BasicNotification("Articulo Actualizado", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                BasicNotification("Ocurrio un error intente de nuevo", NotificationType.error);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var article = await _service.InventaryService.GetById(id);
            if (article == null) return NotFound();
            if (await _service.InventaryService.Remove(article)) return Ok(true);
            return BadRequest();
        }
    }
}