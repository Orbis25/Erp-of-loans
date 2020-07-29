using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinesLogic.Interfaces;
using BusinesLogic.UnitOfWork;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;

namespace ERP_SPARTAN.Controllers
{
    [Authorize(Roles = nameof(RolsAuthorization.Admin))]
    public class RolController : BaseController
    {
        private readonly IUnitOfWork _roleService;
        public RolController(IUnitOfWork roleService) => _roleService = roleService;
        public async Task<IActionResult> Index() => View(await _roleService.RoleService.GetAll());

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if(await _roleService.RoleService.Create(model.Name)) return RedirectToAction(nameof(Index));
                return View(model);
            }
            return View(model);
        }
    }
}