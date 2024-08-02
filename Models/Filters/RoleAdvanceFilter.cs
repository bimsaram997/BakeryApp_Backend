using Models.ViewModels.Location;
using Models.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Filters
{
    public class RoleAdvanceFilter
    {
        public int? Status { get; set; }
        public string? RoleDescription { get; set; }
        public int? LocationId { get; set; }
        public string SortBy { get; set; }
        public bool IsAscending { get; set; }
        public Pagination Pagination { get; set; }
        public string? SearchString { get; set; }
        public string? AddedDate { get; set; }
    }

    public class PaginatedRoles
    {
        public List<AllRolesVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
