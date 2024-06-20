using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Filters
{
    public class SupplierListAdvanceFilter
    {
        public string SortBy { get; set; }
        public bool IsAscending { get; set; }
        public Pagination Pagination { get; set; }
        public string? SearchString { get; set; }
        public string? AddedDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsRawMaterial { get; set; }
        public bool? IsProduct { get; set; }
        public List<int>? RawMaterialIds { get; set; }
        public List<int>? ProductIds { get; set; }
    }
}
