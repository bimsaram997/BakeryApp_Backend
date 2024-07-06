using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Filters
{
    public class StockListAdvanceFilter
    {
        public string? SearchString { get; set; }
        public Pagination Pagination { get; set; }
        public string SortBy { get; set; }
        public bool IsAscending { get; set; }
        public int? ProductId { get; set; }
        public int? Unit { get; set;}
        public int? CostCode { get; set; }
        public int? RecipeId { get; set; }
        public int? SupplyTypeId { get; set; }
        public int? SupplierId { get; set; }
        public string? ManufacturedDate { get; set; }
        public string? ExpiredDate { get; set; }
        public int? ItemQuantity { get; set; }
        public int? ReorderLevel { get; set; }
        public string? AddedDate { get; set; }
        public double? SellingPrice { get; set; }
        public double? CostPrice { get; set; }
    }
}
