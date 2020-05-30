using BusinesLogic.Repository.Interfaces;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Interfaces.Shared
{
    public interface ICompanyService : IBaseRepository<Company>
    {
        Task<bool> HaveCompany(string userId);
        Task<Company> GetCompanyByUserId(string userId);

    }
}
