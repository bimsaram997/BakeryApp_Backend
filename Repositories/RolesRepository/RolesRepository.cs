using Models.Data;
using Models.Data.Role;
using Models.ViewModels;
using Models.ViewModels.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RolesRepository
{
    public interface IRolesRepository
    {

        public ReturnRoles GetAll();

    }
    public class RolesRepository : IRolesRepository
    {
        private AppDbContext _context;

        public RolesRepository(AppDbContext context)
        {
            _context = context;
        }

        public ReturnRoles GetAll()
        {
            IQueryable<Roles> query = _context.Role
       .Where(fi => !fi.IsDeleted);
            var values = query.Select(fi => new RolesVM
            {
                Id = fi.Id,
                RoleName = fi.RoleName,
                AddedDate = fi.AddedDate,
                ModifiedDate = fi.ModifiedDate,
            }).ToList();

            var result = new ReturnRoles
            {
                Items = values
            };
            return result;


        }



    }
}
