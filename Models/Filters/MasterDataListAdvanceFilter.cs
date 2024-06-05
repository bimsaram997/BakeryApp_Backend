using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Filters
{
    public class MasterDataListAdvanceFilter
    {
        public string SortBy { get; set; }
        public bool IsAscending { get; set; }
        public Pagination Pagination { get; set; }
        public string? SearchString { get; set; }
        public string? AddedDate { get; set; }
        public int? EnumTypeId { get; set; }
      
    }
}
