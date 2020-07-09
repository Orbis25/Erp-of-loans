using BusinesLogic.Interfaces;
using BusinesLogic.Repository.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Services
{
    public class ClientUserService : BaseRepository<ClientUser>, IClientUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public ClientUserService(ApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<ClientUser>> GetAllWithRelationships(string userId, Guid? enterpriseId)
        {
            var result = GetAll().Where(x => x.CreatedBy == userId);
            if (enterpriseId != null) result = result.Where(x => x.EnterpriseId == enterpriseId.Value);
            result = result.Include(x => x.Enterprise).Include(x => x.User);
            return await result.ToListAsync();
        }

        public async Task<ClientUser> GetByIdWithRelationships(Guid id)
            => await _dbContext.ClientUsers.Include(x => x.User)
            .Include(x => x.Enterprise)
                    .Include(x => x.Movements).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<bool> SoftRemove(Guid id)
        {
            var model = await GetById(id);
            model.State = Models.Enums.State.Removed;
            return await Update(model);
        }

        public async Task<ClientUser> GetClientByUserId(string id)
            => await _dbContext.ClientUsers.FirstOrDefaultAsync(x => x.UserId == id);

        public Task<IEnumerable<ClientUser>> GetAllWithRelationShips()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientUser>> GetAllWithRelationShips(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SelectListItem>> GetListItem(Expression<Func<ClientUser, bool>> filter = null)
        {
            var result = _dbContext.ClientUsers.Include(x => x.User).AsQueryable();
            if (filter != null) result = result.Where(filter);
            return await result.Select(x => new SelectListItem { Text = $"{x.User.FullName} {x.User.UserName}", Value = x.User.Id }).ToListAsync();
        }


        public override async Task<bool> Update(ClientUser entity)
        {
            var model = await GetById(entity.Id);
            model.IdentificationCard = entity.IdentificationCard;
            model.State = entity.State;
            model.Address = entity.Address;
            model.BankId = entity.BankId;
            _dbContext.Update(model);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
