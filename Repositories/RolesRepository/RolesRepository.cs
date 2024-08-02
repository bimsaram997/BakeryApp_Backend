using Models.Data;
using Models.Data.Location;
using Models.Data.Role;
using Models.Filters;
using Models.Helpers;
using Models.ViewModels.Address;
using Models.ViewModels.EnumType;
using Models.ViewModels.Location;
using Models.ViewModels.Roles;
using Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RolesRepository
{
    public interface IRolesRepository
    {

        public PaginatedRoles GetAll(RoleAdvanceFilter filter);

    }
    public class RolesRepository : IRepositoryBase<RolesVM>, IRolesRepository
    {
        private AppDbContext _context;

        public RolesRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(RolesVM entity)
        {
            throw new NotImplementedException();
        }

        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public PaginatedRoles GetAll(RoleAdvanceFilter filter)
        {
            IQueryable<Roles> query = _context.Role
             .Where(fi => !fi.IsDeleted);
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(fi =>
                    fi.RoleName.Contains(filter.SearchString) 
                );
            }
            if (filter.Status.HasValue)
            {
                query = query.Where(fi => fi.Status == filter.Status);
            }

            if (filter.LocationId.HasValue)
            {
                query = query.Where(fi => fi.LocationId == filter.LocationId);
            }

            if (!string.IsNullOrEmpty(filter.AddedDate) && DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
            {
                // Adjust date filter to consider the whole day
                DateTime nextDay = filterDate.AddDays(1);
                query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < nextDay);

            }
            int totalCount = query.Count();

            // Apply sorting
            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

            // Apply pagination
            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize)
                         .Take(filter.Pagination.PageSize);

            // Project and materialize the results
            var paginatedResult = query
                .Select(fi => new AllRolesVM
                {
                    Id = fi.Id,
                    RoleName = fi.RoleName,
                    RoleDescription = fi.RoleDescription,
                    AddedDate = fi.AddedDate,
                    Status = fi.Status,
                    LocationId = fi.LocationId,
                    ModifiedDate= fi.ModifiedDate,
                    StatusName = _context.MasterData
                        .Where(masterData => masterData.Id == fi.Status)
                        .Select(masterData => masterData.MasterDataName)
                        .FirstOrDefault(),
                    LocationName = _context.Location
                        .Where(location => location.Id == fi.LocationId)
                        .Select(location => location.LocationName)
                        .FirstOrDefault(),

                })
                .ToList();
            var result = new PaginatedRoles
            {
                Items = paginatedResult,
                TotalCount = totalCount,
                PageIndex = filter.Pagination.PageIndex,
                PageSize = filter.Pagination.PageSize
            };

            return result;

        }

        public RolesVM GetById(int id)
        {
            RolesVM? role = _context.Role.Where(r => r.Id == id && !r.IsDeleted).Select(role => new RolesVM()
            {
                Id = role.Id,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                Status = role.Status,
                AddedDate = role.AddedDate,
                ModifiedDate = role.ModifiedDate,
                IsDeleted = role.IsDeleted,
                LocationId = role.LocationId,
            }).FirstOrDefault();
            return role;
        }

        public int UpdateById(int id, RolesVM entity)
        {
          Roles? previousRoles = _context.Role.FirstOrDefault(l => l.Id == id && !l.IsDeleted);
          if (previousRoles == null)
            {
                return -1;
            }
          previousRoles.RoleName = entity.RoleName;
          previousRoles.RoleDescription = entity.RoleDescription;
          previousRoles.Status = entity.Status;
          previousRoles.ModifiedDate = DateTime.Now;
          previousRoles.LocationId = entity.LocationId;
            _context.SaveChanges();
            return previousRoles.Id;
        }
    }
}
