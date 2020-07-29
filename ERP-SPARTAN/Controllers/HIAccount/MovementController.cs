using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinesLogic.UnitOfWork;
using Commons.Others;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;
using Models.ViewModels.ClientUsers;

namespace ERP_SPARTAN.Controllers
{
    [Authorize]
    public class MovementController : BaseController
    {
        private readonly IUnitOfWork _service;
        public MovementController(IUnitOfWork service) => _service = service;

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles =  ControllersRol.ClientUser +","+ nameof(RolsAuthorization.Client))]
        [HttpGet]
        public async Task<IActionResult> GetByClientUser(Guid id)
        {
            var model = await _service.ClientUserService.GetByIdWithRelationships(id);
            if (model != null)
            {
                return View(new ClientUserWithMovementVM
                {
                    Client = model,
                    Debs = _service.MovementsService.CountOfDebs(id),
                    Payments = _service.MovementsService.CountOfPayment(id),
                    Total = _service.MovementsService.TotalDebs(id)
                });
            }
            return new NotFoundView();
        }
        [Authorize(Roles = nameof(RolsAuthorization.ClientsUser))]

        [HttpPost]
        public async Task<IActionResult> Create(Movement moviment)
        {
            if (ModelState.IsValid)
            {
                if (moviment.Type == TypeOfMovement.Payment) moviment.Amount = -moviment.Amount;
                if (await _service.MovementsService.Add(moviment))
                {
                    BasicNotification("Agregado Correctamente", NotificationType.success);
                }
                else
                {
                    BasicNotification("Ocurrió un error, intente de nuevo mas tarde", NotificationType.error);
                }
            }
            else
            {
                BasicNotification("Algunos datos son requeridos, por favor completarlos correctamente", NotificationType.error);
            }
            return RedirectToAction(nameof(GetByClientUser), new { id = moviment.ClientUserId });
        }
        [Authorize(Roles = nameof(RolsAuthorization.ClientsUser))]

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (await _service.MovementsService.SoftDelete(id)) return Ok(true);
            return BadRequest();
        }
        [Authorize(Roles = nameof(RolsAuthorization.ClientsUser))]

        [HttpPost]
        public async Task<IActionResult> PayAll(Guid id)
        {
            if (await _service.MovementsService.PayAll(id)) return Ok(true);
            return BadRequest();
        }

  

    }
}