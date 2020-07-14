using BusinesLogic.Interfaces.Shared;
using BusinesLogic.Repository.Services;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Services.Shared
{
    public class CompanyService : BaseRepository<Company>, ICompanyService
    {
        private readonly ApplicationDbContext _dbContext;
        public CompanyService(ApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

        public async Task<Company> GetCompanyByUserId(string userId)
            => await GetAll().Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<bool> HaveCompany(string userId)
            => await _dbContext.Companies.AnyAsync(x => x.UserId == userId);

        public override async Task<bool> Update(Company entity)
        {
            var result = await GetById(entity.Id);
            result.Name = entity.Name;
            result.PhoneNumber = entity.PhoneNumber;
            result.Rnc = entity.Rnc;
            result.Address = entity.Address;
            return await base.Update(result);
        }
    }
}
