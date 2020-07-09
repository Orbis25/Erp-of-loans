using BusinesLogic.Interfaces.HiLoans;
using BusinesLogic.Repository.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Services.HiLoans
{
    public class BankService : BaseRepository<Bank>, IBankService
    {
        private readonly ApplicationDbContext _context;
        public BankService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Bank>> GetAllWithRelationShips()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bank>> GetAllWithRelationShips(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SelectListItem>> GetListItem(Expression<Func<Bank, bool>> filter = null)
            => await Filter(filter).Select(x => new SelectListItem {  Text = x.Name , Value = x.Id.ToString() }).ToListAsync();
    }
}
