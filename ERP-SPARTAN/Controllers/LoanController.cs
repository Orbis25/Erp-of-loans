using BusinesLogic.UnitOfWork;
using Commons.Extensions;
using ERP_SPARTAN.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Enums.HiAccounting;
using Models.Models.HiAccounting;
using Models.ViewModels.HiLoans.Loans;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP_SPARTAN.Controllers
{
    [Authorize(Roles = nameof(RolsAuthorization.HILoans))]
    public class LoanController : BaseController
    {
        private readonly IUnitOfWork _service;
        public LoanController(IUnitOfWork unitOfWork) => _service = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> Index(Guid? idEnterprise = null)
        {
            var userId = GetUserLoggedId();
            ViewBag.Enterprises = await _service.EnterpriseService.GetListItem(x => x.UserId == userId);
            return View(await _service.LoanService.GetAllWithRelationShip(userId, idEnterprise));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _service.ClientUserService.GetAllWithRelationships(GetUserLoggedId(), null);
            ViewBag.Clients = result.Select(x => new SelectListItem
            {
                Text = x.User.FullName,
                Value = x.Id.ToString(),
                Group = new SelectListGroup { Name = x.Enterprise.Name }
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Loan model, string contractDate = null)
        {
            //for change the contract day
            if (!string.IsNullOrEmpty(contractDate)) model.CreateAt = contractDate.ToDateTime();

            model.UserId = GetUserLoggedId();
            var clients = await _service.ClientUserService.GetAllWithRelationships(GetUserLoggedId(), null);
            ViewBag.Clients = clients.Select(x => new SelectListItem
            {
                Text = x.User.FullName,
                Value = x.Id.ToString(),
                Group = new SelectListGroup { Name = x.Enterprise.Name }
            });



            if (!ModelState.IsValid) return View(model);

            if (model.AmortitationType == AmortitationType.Open_o_Personalfee)
            {
                model.Shares = 1;
            }
            if (model.Shares <= 0 || model.AmountDeb <= 0 || model.InitialCapital <= 0)
            {
                BasicNotification("Lo sentimos, hay campos en el formulario que deben ser mayor a cero", NotificationType.error);
                return View(model);
            }
            if (model.AmortitationType == AmortitationType.Open_o_Personalfee)
            {
                model.Shares = _service.LoanService.getShares(model);
            }

            var result = await _service.LoanService.Add(model);
            if (!result)
            {
                BasicNotification("Lo sentimos, Intente de nuevo", NotificationType.error);
                return View(model);
            }
            BasicNotification("Agregado correctamente", NotificationType.success);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id, State stateDeb = State.All)
        {
            ViewBag.Selected = stateDeb;
            ViewBag.Action = nameof(GetById);
            ViewBag.AccessUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{nameof(Loan)}/{nameof(GetMyLoan)}";
            var result = await _service.LoanService.GetByIdWithRelationships(id, stateDeb);
            if (result == null) new NotFoundView();
            return View(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMyLoan(Guid id, State stateDeb = State.All, Guid? reclosingId = null)
        {
            ViewBag.Selected = stateDeb;
            ViewBag.Action = nameof(GetMyLoan);
            ViewBag.ReclosingId = reclosingId;
            var result = await _service.LoanService.GetByIdWithRelationships(id, stateDeb);
            if (result == null) new NotFoundView();
            return View(nameof(GetById), result);
        }


        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await _service.LoanService.SoftRemove(id);
            if (!result) return BadRequest();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentDeb(PaymentLoanVM model)
        {
            var total = Math.Abs(model.AmortizationTotal - model.Amortization);
            if (model.ExtraMount > total)
            {
                BasicNotification("El monto a abonar es mayor que el capital actual, intente abonar un monto menor", NotificationType.warning, "Error");
                return RedirectToAction(nameof(GetById), new { id = model.IdLoan });
            }
            var result = await _service.LoanService.PaymentDeb(model.IdDeb, model.IdLoan, model.ExtraMount, model.InterestOnly);
            if (!result) BasicNotification("Error intente de nuevo", NotificationType.error);
            BasicNotification("Acción Realizada", NotificationType.success);
            return RedirectToAction(nameof(GetById), new { id = model.IdLoan });
        }

        [HttpGet]
        public IActionResult GetAmortization(Loan model, string contractDate = null)
        {
            if (!string.IsNullOrEmpty(contractDate)) model.CreateAt = contractDate.ToDateTime();

            return PartialView("_GetAmortizationPartial", _service.LoanService.GetAmortization(model));
        }


        [HttpGet]
        public async Task<IActionResult> Reclosing(Guid id, State stateDeb = State.All)
        {

            ViewBag.Selected = stateDeb;
            ViewBag.Action = nameof(GetById);
            var result = await _service.LoanService.GetByIdWithRelationships(id, stateDeb);
            if (result == null) new NotFoundView();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Reclosing(Loan model)
        {
            model.Id = Guid.Empty;
            model.UserId = GetUserLoggedId();
            model.InitialCapital = model.ActualCapital + model.ReclosingAmount;
            var clients = await _service.ClientUserService.GetAllWithRelationships(GetUserLoggedId(), null);
            ViewBag.Clients = clients.Select(x => new SelectListItem
            {
                Text = x.User.FullName,
                Value = x.Id.ToString(),
                Group = new SelectListGroup { Name = x.Enterprise.Name }
            });
            if (!ModelState.IsValid) return View(model);

            var result = await _service.LoanService.Add(model);
            if (!result)
            {

                BasicNotification("Lo sentimos, Intente de nuevo", NotificationType.error);
                return View(model);

            }

            result = await _service.LoanService.AddReclosing(model);
            if (!result)
            {
                BasicNotification("Lo sentimos, Intente de nuevo", NotificationType.error);
                return View(model);
            }
            BasicNotification("Reenganchado correctamente", NotificationType.success);

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetReclosingHistory(Guid id)
        {
            ViewBag.reclosingId = id;
            return PartialView("_GetReclosingHistoryPartial", await _service.LoanService.GetReclosing(id));
        }



        [HttpGet]
        public IActionResult GetAmortizationReclosing(Loan model)
        {
            model.ActualCapital += model.ReclosingAmount;
            return PartialView("_GetAmortizationPartial", _service.LoanService.GetAmortization(model));
        }

        [HttpGet]
        public async Task<IActionResult> OverdueInstallments() => PartialView("_OverdueInstallmentsPartial", await _service.LoanService.GetPaymentPendingClients(GetUserLoggedId()));

        [HttpGet]
        public async Task<IActionResult> GetLoanByMonth() => Ok(await _service.LoanService.GetLoanByMonth(GetUserLoggedId()));

        [HttpGet]
        public async Task<IActionResult> GetBadAndGoodPayments()
            => Ok(await _service.LoanService.GetBadAndGoodClientPayments(GetUserLoggedId()));
    }
}