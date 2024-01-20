using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Pagination;

namespace Models.Filters
{
    public class ProductListAdvanceFilter
    {
        public string SortBy { get; set; }
        public bool IsAscending { get; set; }
        public int? FoodTypeId { get; set; }
        public double? FoodPrice { get; set; }
        public Pagination Pagination { get; set; }

        public string? SearchString { get; set; }  
        public string? AddedDate { get; set; }
        public long? BatchId { get; set; }
        public bool? Available { get; set; }

    }

   
}
