using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Filters
{
    public class UserAdvanceListFilter
    {
        public string SortBy { get; set; }
        public bool IsAscending { get; set; }
        public Pagination Pagination { get; set; }
        public string? SearchString { get; set; }
        public string? AddedDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Gender { get; set; }
        public int? Role { get; set; }

    }
}
