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

        public double? SellingPrice { get; set; }
        public double? CostPrice { get; set; }
        public Pagination Pagination { get; set; }
        public string? SearchString { get; set; }  
        public string? AddedDate { get; set; }
        public int? Unit { get; set; }
        public int? CostCode { get; set; }
        public int? RecipeId { get; set; }
        public double? Weight { get; set; }
        public int? DaysToExpires { get; set; }
        public int? ReOrderLevel { get; set; }

    }

   
}
