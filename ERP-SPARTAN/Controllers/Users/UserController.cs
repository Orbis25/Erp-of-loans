using BusinesLogic.UnitOfWork;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Enums;
using Models.Models;
using Models.Settings;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP_SPARTAN.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _service;
        private readonly UserManager<User> _userManager;
        private readonly DefaultValue _settings;
        public UserController(IUnitOfWork service, UserManager<User> userManager, IOptions<DefaultValue> option)
        {
            _service = service;
            _userManager = userManager;
            _settings = option.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return View(await _service.UserService.GetUserAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Update(User model)
        {
            if (ModelState.IsValid)
            {
                if (await _service.UserService.Update(model))
                {
                    BasicNotification("Actualizado Correctamente", NotificationType.success);
                    return RedirectToAction(nameof(Profile));
                }
                BasicNotification("Ha ocurrido un error , una de las razones puede ser que exista un correo igual al ingresado intente con otro", NotificationType.error);
            }
            return View(nameof(Profile), model);
        }

        [Authorize(Roles = nameof(RolsAuthorization.Admin))]
        [HttpGet]
        public async Task<IActionResult> AddToRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return new NotFoundView();
            var result = await _service.UserService.GetUserAsync(id);
            ViewBag.UserRols = await _userManager.GetRolesAsync(result);
            ViewBag.Rols = await _service.RoleService.GetListItem();
            if (result == null) return new NotFoundView();
            return View(result);
        }

        [Authorize(Roles = nameof(RolsAuthorization.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddToRole(string id, string RoleId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(RoleId)) return new NotFoundView();
            var user = await _service.UserService.GetUserAsync(id);
            if (user == null) return new NotFoundView();
            var result = await _service.UserService.AddToRole(id, RoleId);
            if (!result) BasicNotification("Intente Nuevamente", NotificationType.error, "Error");
            BasicNotification("Cliente agregado al nuevo rol, puede continuar agregando mas roles", NotificationType.success, "Listo");
            return RedirectToAction(nameof(AddToRole), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserRole(string id, string roleName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roleName)) return BadRequest();
            return Ok(await _service.UserService.RemoveUserRoleAsync(id, roleName));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) BasicNotification("Error el usuario no existe", NotificationType.error);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, _settings.Password);
            BasicNotification("Contraseña restablecida", NotificationType.success);
            return RedirectToAction(nameof(Index), nameof(ClientUser));
        }
    }
}