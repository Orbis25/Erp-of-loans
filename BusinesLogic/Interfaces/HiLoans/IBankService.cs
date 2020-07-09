using BusinesLogic.Repository.Interfaces;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinesLogic.Interfaces.HiLoans
{
    public interface IBankService : IBaseRepository<Bank> , IHelperRepository<Bank>
    {
    }
}
